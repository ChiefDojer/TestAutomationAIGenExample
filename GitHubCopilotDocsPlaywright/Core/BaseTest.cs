using GitHubCopilotDocs.Tests.Core.Configuration;
using GitHubCopilotDocs.Tests.Core.Driver;
using GitHubCopilotDocs.Tests.Core.Logging;
using Microsoft.Playwright;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace GitHubCopilotDocs.Tests.Core;

/// <summary>
/// Base class for all test fixtures. Provides:
/// - Playwright driver setup/teardown
/// - Configuration management
/// - Logging infrastructure
/// - Automatic failure diagnostics (screenshots, traces, HTML)
/// - Retry mechanism for flaky operations
/// </summary>
[TestFixture]
public abstract class BaseTest
{
    protected PlaywrightDriverManager? DriverManager { get; private set; }
    protected IPage Page => DriverManager!.Page;
    protected TestLogger? Logger { get; private set; }
    protected TestSettings Settings => ConfigLoader.Instance.Settings;

    [SetUp]
    public virtual async Task SetUp()
    {
        var testName = TestContext.CurrentContext.Test.MethodName ?? "UnknownTest";
        
        // Initialize logger
        var logLevel = Settings.Logging.MinimumLevel.ToLowerInvariant() switch
        {
            "trace" => LogLevel.Trace,
            "debug" => LogLevel.Debug,
            "information" => LogLevel.Information,
            "warning" => LogLevel.Warning,
            "error" => LogLevel.Error,
            "critical" => LogLevel.Critical,
            _ => LogLevel.Information
        };

        Logger = new TestLogger(
            testName,
            logLevel,
            Settings.Logging.OutputPath,
            Settings.Logging.Console,
            Settings.Logging.File);

        Logger.Information($"Starting test: {testName}");

        // Initialize Playwright driver
        DriverManager = new PlaywrightDriverManager();
        await DriverManager.InitializeAsync(
            Settings.Browser.Type,
            Settings.Browser.Headless,
            Settings.Browser.ViewportWidth,
            Settings.Browser.ViewportHeight,
            Settings.Browser.RecordVideo,
            Settings.Browser.CaptureTrace);

        // Configure default timeouts
        Page.SetDefaultTimeout(Settings.Execution.DefaultTimeout);
        Page.SetDefaultNavigationTimeout(Settings.Execution.NavigationTimeout);

        Logger.Information("Test setup completed successfully");
    }

    [TearDown]
    public virtual async Task TearDown()
    {
        var testName = TestContext.CurrentContext.Test.MethodName ?? "UnknownTest";
        var testStatus = TestContext.CurrentContext.Result.Outcome.Status;

        Logger?.Information($"Test {testName} completed with status: {testStatus}");

        // Capture diagnostics on failure
        if (testStatus == TestStatus.Failed && DriverManager != null)
        {
            await CaptureDiagnosticsAsync(testName);
        }

        // Cleanup
        if (DriverManager != null)
        {
            await DriverManager.DisposeAsync();
        }

        Logger?.Information("Test teardown completed");
    }

    /// <summary>
    /// Navigates to a relative path from the base URL
    /// </summary>
    protected async Task NavigateToAsync(string relativePath)
    {
        var url = new Uri(new Uri(Settings.BaseUrl), relativePath).ToString();
        Logger?.Debug($"Navigating to: {url}");
        await Page.GotoAsync(url);
    }

    /// <summary>
    /// Logs a test step for better readability in test output
    /// </summary>
    protected void LogStep(string step)
    {
        Logger?.Information($"STEP: {step}");
        TestContext.WriteLine($"⚡ {step}");
    }

    /// <summary>
    /// Retries an async operation with exponential backoff
    /// </summary>
    protected async Task<T> RetryAsync<T>(
        Func<Task<T>> operation,
        int? maxRetries = null,
        int? retryDelayMs = null)
    {
        var retries = maxRetries ?? Settings.Execution.MaxRetries;
        var delay = retryDelayMs ?? Settings.Execution.RetryDelay;

        for (int attempt = 1; attempt <= retries; attempt++)
        {
            try
            {
                return await operation();
            }
            catch (Exception ex) when (attempt < retries)
            {
                Logger?.Warning($"Attempt {attempt} failed: {ex.Message}. Retrying in {delay}ms...");
                await Task.Delay(delay);
                delay *= 2; // Exponential backoff
            }
        }

        // Final attempt without catching
        return await operation();
    }

    /// <summary>
    /// Captures failure diagnostics: screenshot, trace, and HTML
    /// </summary>
    private async Task CaptureDiagnosticsAsync(string testName)
    {
        try
        {
            Logger?.Warning("Test failed. Capturing diagnostics...");

            var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            var screenshotDir = "TestResults/Screenshots";
            var traceDir = "TestResults/Traces";

            Directory.CreateDirectory(screenshotDir);
            Directory.CreateDirectory(traceDir);

            // Capture screenshot
            if (Settings.Browser.ScreenshotOnFailure)
            {
                var screenshotPath = Path.Combine(screenshotDir, $"{testName}_{timestamp}.png");
                await DriverManager!.CaptureScreenshotAsync(screenshotPath);
                Logger?.Information($"Screenshot saved: {screenshotPath}");
                TestContext.AddTestAttachment(screenshotPath, "Failure Screenshot");
            }

            // Capture trace
            if (Settings.Browser.CaptureTrace)
            {
                var tracePath = Path.Combine(traceDir, $"{testName}_{timestamp}.zip");
                await DriverManager!.CaptureTraceAsync(tracePath);
                Logger?.Information($"Trace saved: {tracePath}");
                TestContext.AddTestAttachment(tracePath, "Playwright Trace");
            }

            // Capture HTML
            var htmlPath = Path.Combine(screenshotDir, $"{testName}_{timestamp}.html");
            var htmlContent = await DriverManager!.GetPageContentAsync();
            await File.WriteAllTextAsync(htmlPath, htmlContent);
            Logger?.Information($"HTML saved: {htmlPath}");
            TestContext.AddTestAttachment(htmlPath, "Page HTML");
        }
        catch (Exception ex)
        {
            Logger?.Error("Failed to capture diagnostics", ex);
        }
    }
}
