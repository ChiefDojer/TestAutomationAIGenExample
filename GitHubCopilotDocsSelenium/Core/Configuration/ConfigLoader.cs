using Microsoft.Extensions.Configuration;

namespace GitHubCopilotDocs.Selenium.Tests.Core.Configuration;

/// <summary>
/// Singleton configuration loader that manages application settings.
/// Supports hierarchical configuration: Environment Variables > appsettings.{ENV}.json > appsettings.json
/// </summary>
public sealed class ConfigLoader
{
    private static readonly Lazy<ConfigLoader> _instance = new(() => new ConfigLoader());
    private readonly TestSettings _settings;

    /// <summary>
    /// Gets the singleton instance of ConfigLoader.
    /// </summary>
    public static ConfigLoader Instance => _instance.Value;

    /// <summary>
    /// Gets the loaded test settings.
    /// </summary>
    public TestSettings Settings => _settings;

    private ConfigLoader()
    {
        var environment = Environment.GetEnvironmentVariable("TEST_ENV") ?? "Development";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables(prefix: "TEST_")
            .Build();

        _settings = new TestSettings
        {
            BaseUrl = configuration["BaseUrl"] ?? throw new InvalidOperationException("BaseUrl is required"),
            Browser = configuration.GetSection("Browser").Get<BrowserSettings>() ?? throw new InvalidOperationException("Browser settings are required"),
            Execution = configuration.GetSection("Execution").Get<ExecutionSettings>() ?? throw new InvalidOperationException("Execution settings are required"),
            Logging = configuration.GetSection("Logging").Get<LoggingSettings>() ?? throw new InvalidOperationException("Logging settings are required")
        };
    }
}
