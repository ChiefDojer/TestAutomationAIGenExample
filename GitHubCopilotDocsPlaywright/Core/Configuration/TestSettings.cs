namespace GitHubCopilotDocs.Tests.Core.Configuration;

/// <summary>
/// Strongly-typed configuration settings for the test framework.
/// Maps to appsettings.json structure.
/// </summary>
public class TestSettings
{
    /// <summary>
    /// Base URL of the application under test
    /// </summary>
    public required string BaseUrl { get; init; }

    /// <summary>
    /// Browser configuration settings
    /// </summary>
    public required BrowserSettings Browser { get; init; }

    /// <summary>
    /// Test execution configuration
    /// </summary>
    public required ExecutionSettings Execution { get; init; }

    /// <summary>
    /// Logging configuration
    /// </summary>
    public required LoggingSettings Logging { get; init; }
}

public class BrowserSettings
{
    /// <summary>
    /// Browser type: chromium, firefox, webkit
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// Whether to run browser in headless mode
    /// </summary>
    public bool Headless { get; init; } = true;

    /// <summary>
    /// Browser viewport width
    /// </summary>
    public int ViewportWidth { get; init; } = 1920;

    /// <summary>
    /// Browser viewport height
    /// </summary>
    public int ViewportHeight { get; init; } = 1080;

    /// <summary>
    /// Whether to record video of test execution
    /// </summary>
    public bool RecordVideo { get; init; } = false;

    /// <summary>
    /// Whether to capture screenshots on failure
    /// </summary>
    public bool ScreenshotOnFailure { get; init; } = true;

    /// <summary>
    /// Whether to capture traces for debugging
    /// </summary>
    public bool CaptureTrace { get; init; } = true;
}

public class ExecutionSettings
{
    /// <summary>
    /// Default timeout for actions (in milliseconds)
    /// </summary>
    public int DefaultTimeout { get; init; } = 30000;

    /// <summary>
    /// Default timeout for navigation (in milliseconds)
    /// </summary>
    public int NavigationTimeout { get; init; } = 60000;

    /// <summary>
    /// Maximum number of retry attempts for flaky operations
    /// </summary>
    public int MaxRetries { get; init; } = 3;

    /// <summary>
    /// Delay between retry attempts (in milliseconds)
    /// </summary>
    public int RetryDelay { get; init; } = 1000;

    /// <summary>
    /// Whether to run tests in parallel
    /// </summary>
    public bool Parallel { get; init; } = true;

    /// <summary>
    /// Number of parallel workers (0 = auto)
    /// </summary>
    public int Workers { get; init; } = 0;
}

public class LoggingSettings
{
    /// <summary>
    /// Minimum log level: Trace, Debug, Information, Warning, Error, Critical
    /// </summary>
    public required string MinimumLevel { get; init; }

    /// <summary>
    /// Path to log output directory
    /// </summary>
    public required string OutputPath { get; init; }

    /// <summary>
    /// Whether to log to console
    /// </summary>
    public bool Console { get; init; } = true;

    /// <summary>
    /// Whether to log to file
    /// </summary>
    public bool File { get; init; } = true;
}
