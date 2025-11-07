namespace GitHubCopilotDocs.Selenium.Tests.Core.Configuration;

/// <summary>
/// Main configuration settings for test execution.
/// </summary>
public class TestSettings
{
    /// <summary>
    /// Gets the base URL for the application under test.
    /// </summary>
    public required string BaseUrl { get; init; }

    /// <summary>
    /// Gets the browser-specific settings.
    /// </summary>
    public required BrowserSettings Browser { get; init; }

    /// <summary>
    /// Gets the test execution settings.
    /// </summary>
    public required ExecutionSettings Execution { get; init; }

    /// <summary>
    /// Gets the logging configuration.
    /// </summary>
    public required LoggingSettings Logging { get; init; }
}

/// <summary>
/// Browser-specific configuration settings.
/// </summary>
public class BrowserSettings
{
    /// <summary>
    /// Gets the browser type to use for testing.
    /// Supported values: chrome, firefox, edge, safari.
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// Gets a value indicating whether to run browser in headless mode.
    /// </summary>
    public required bool Headless { get; init; }

    /// <summary>
    /// Gets the viewport width in pixels.
    /// </summary>
    public required int ViewportWidth { get; init; }

    /// <summary>
    /// Gets the viewport height in pixels.
    /// </summary>
    public required int ViewportHeight { get; init; }

    /// <summary>
    /// Gets a value indicating whether to take screenshots on test failure.
    /// </summary>
    public required bool ScreenshotOnFailure { get; init; }

    /// <summary>
    /// Gets a value indicating whether to maximize the browser window.
    /// </summary>
    public required bool Maximize { get; init; }

    /// <summary>
    /// Gets a value indicating whether to accept insecure certificates.
    /// </summary>
    public required bool AcceptInsecureCerts { get; init; }
}

/// <summary>
/// Test execution configuration settings.
/// </summary>
public class ExecutionSettings
{
    /// <summary>
    /// Gets the default timeout in milliseconds for element operations.
    /// </summary>
    public required int DefaultTimeout { get; init; }

    /// <summary>
    /// Gets the page load timeout in milliseconds.
    /// </summary>
    public required int PageLoadTimeout { get; init; }

    /// <summary>
    /// Gets the maximum number of retry attempts for flaky operations.
    /// </summary>
    public required int MaxRetries { get; init; }

    /// <summary>
    /// Gets the delay in milliseconds between retry attempts.
    /// </summary>
    public required int RetryDelay { get; init; }

    /// <summary>
    /// Gets a value indicating whether to run tests in parallel.
    /// </summary>
    public required bool Parallel { get; init; }

    /// <summary>
    /// Gets the number of parallel workers (0 = auto).
    /// </summary>
    public required int Workers { get; init; }
}

/// <summary>
/// Logging configuration settings.
/// </summary>
public class LoggingSettings
{
    /// <summary>
    /// Gets the minimum log level.
    /// Supported values: Trace, Debug, Information, Warning, Error, Critical.
    /// </summary>
    public required string MinimumLevel { get; init; }

    /// <summary>
    /// Gets the output path for log files.
    /// </summary>
    public required string OutputPath { get; init; }

    /// <summary>
    /// Gets a value indicating whether to log to console.
    /// </summary>
    public required bool Console { get; init; }

    /// <summary>
    /// Gets a value indicating whether to log to file.
    /// </summary>
    public required bool File { get; init; }
}
