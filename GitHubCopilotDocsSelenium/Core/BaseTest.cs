using GitHubCopilotDocs.Selenium.Tests.Core.Configuration;
using GitHubCopilotDocs.Selenium.Tests.Core.Driver;
using GitHubCopilotDocs.Selenium.Tests.Core.Logging;
using OpenQA.Selenium;

namespace GitHubCopilotDocs.Selenium.Tests.Core;

/// <summary>
/// Base class for all test fixtures providing common setup, teardown, and utilities.
/// </summary>
[TestFixture]
public abstract class BaseTest
{
    private SeleniumDriverManager? _driverManager;
    private TestLogger? _logger;

    /// <summary>
    /// Gets the WebDriver instance.
    /// </summary>
    protected IWebDriver Driver => _driverManager?.Driver ?? throw new InvalidOperationException("Driver not initialized");

    /// <summary>
    /// Gets the logger instance.
    /// </summary>
    protected TestLogger Logger => _logger ?? throw new InvalidOperationException("Logger not initialized");

    /// <summary>
    /// Gets the test configuration settings.
    /// </summary>
    protected TestSettings Settings => ConfigLoader.Instance.Settings;

    /// <summary>
    /// Test setup executed before each test.
    /// </summary>
    [SetUp]
    public virtual void SetUp()
    {
        var testName = TestContext.CurrentContext.Test.Name;
        var minimumLevel = Enum.Parse<TestLogger.LogLevel>(Settings.Logging.MinimumLevel);

        _logger = new TestLogger(
            testName,
            minimumLevel,
            Settings.Logging.OutputPath,
            Settings.Logging.Console,
            Settings.Logging.File
        );

        _logger.Information($"Starting test: {testName}");

        _driverManager = new SeleniumDriverManager(_logger);
        _driverManager.Initialize(Settings.Browser, Settings.Execution);

        _logger.Information("Test setup completed successfully");
    }

    /// <summary>
    /// Test teardown executed after each test.
    /// Captures diagnostics on failure and disposes resources.
    /// </summary>
    [TearDown]
    public virtual void TearDown()
    {
        var testName = TestContext.CurrentContext.Test.Name;
        var testResult = TestContext.CurrentContext.Result.Outcome.Status;

        _logger?.Information($"Test {testName} completed with status: {testResult}");

        if (testResult == NUnit.Framework.Interfaces.TestStatus.Failed && Settings.Browser.ScreenshotOnFailure)
        {
            CaptureFailureDiagnostics();
        }

        try
        {
            _driverManager?.Dispose();
            _logger?.Information("Test teardown completed successfully");
        }
        catch (Exception ex)
        {
            _logger?.Error("Error during test teardown", ex);
        }
    }

    /// <summary>
    /// Navigates to a relative path from the base URL.
    /// </summary>
    protected void NavigateTo(string relativePath)
    {
        ArgumentNullException.ThrowIfNull(relativePath);

        var url = $"{Settings.BaseUrl.TrimEnd('/')}/{relativePath.TrimStart('/')}";
        _logger?.Debug($"Navigating to: {url}");
        Driver.Navigate().GoToUrl(url);
    }

    /// <summary>
    /// Logs a test step for better readability.
    /// </summary>
    protected void LogStep(string step)
    {
        ArgumentNullException.ThrowIfNull(step);

        _logger?.Information($"STEP: {step}");
        TestContext.WriteLine($"⚡ {step}");
    }

    /// <summary>
    /// Retries an operation with exponential backoff.
    /// </summary>
    protected T Retry<T>(Func<T> operation, int? maxRetries = null, int? retryDelay = null)
    {
        ArgumentNullException.ThrowIfNull(operation);

        var retries = maxRetries ?? Settings.Execution.MaxRetries;
        var delay = retryDelay ?? Settings.Execution.RetryDelay;

        for (int i = 0; i < retries; i++)
        {
            try
            {
                return operation();
            }
            catch (Exception ex) when (i < retries - 1)
            {
                _logger?.Warning($"Operation failed (attempt {i + 1}/{retries}): {ex.Message}");
                Thread.Sleep(delay * (i + 1)); // Exponential backoff
            }
        }

        // Final attempt without catching exception
        return operation();
    }

    private void CaptureFailureDiagnostics()
    {
        if (_driverManager == null || _logger == null)
            return;

        try
        {
            var testName = TestContext.CurrentContext.Test.Name;
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var screenshotDir = Path.Combine("TestResults", "Screenshots");
            Directory.CreateDirectory(screenshotDir);

            // Capture screenshot
            var screenshotPath = Path.Combine(screenshotDir, $"{testName}_{timestamp}.png");
            var screenshotBytes = _driverManager.CaptureScreenshot();
            File.WriteAllBytes(screenshotPath, screenshotBytes);
            _logger.Information($"Screenshot saved: {screenshotPath}");
            TestContext.AddTestAttachment(screenshotPath, "Failure Screenshot");

            // Capture HTML
            var htmlPath = Path.Combine(screenshotDir, $"{testName}_{timestamp}.html");
            var pageSource = _driverManager.GetPageSource();
            File.WriteAllText(htmlPath, pageSource);
            _logger.Information($"Page HTML saved: {htmlPath}");
            TestContext.AddTestAttachment(htmlPath, "Page HTML");
        }
        catch (Exception ex)
        {
            _logger.Error("Failed to capture diagnostics", ex);
        }
    }
}
