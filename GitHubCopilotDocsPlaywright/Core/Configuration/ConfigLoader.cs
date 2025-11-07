using Microsoft.Extensions.Configuration;

namespace GitHubCopilotDocs.Tests.Core.Configuration;

/// <summary>
/// Singleton configuration loader that builds hierarchical configuration from multiple sources.
/// Priority (highest to lowest): Environment Variables → appsettings.{ENV}.json → appsettings.json
/// </summary>
public sealed class ConfigLoader
{
    private static readonly Lazy<ConfigLoader> _instance = new(() => new ConfigLoader());
    private readonly IConfiguration _configuration;
    private readonly TestSettings _settings;

    /// <summary>
    /// Singleton instance of ConfigLoader
    /// </summary>
    public static ConfigLoader Instance => _instance.Value;

    /// <summary>
    /// Loaded test settings
    /// </summary>
    public TestSettings Settings => _settings;

    private ConfigLoader()
    {
        var environment = Environment.GetEnvironmentVariable("TEST_ENV") ?? "Development";
        var basePath = AppContext.BaseDirectory;

        _configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables(prefix: "TEST_")
            .Build();

        _settings = _configuration.Get<TestSettings>()
            ?? throw new InvalidOperationException(
                $"Failed to load TestSettings from configuration. Ensure appsettings.json exists in '{basePath}' and is valid.");

        ValidateSettings();
    }

    /// <summary>
    /// Validates that required settings have valid values
    /// </summary>
    private void ValidateSettings()
    {
        if (string.IsNullOrWhiteSpace(_settings.BaseUrl))
        {
            throw new InvalidOperationException("BaseUrl cannot be empty");
        }

        if (!Uri.TryCreate(_settings.BaseUrl, UriKind.Absolute, out _))
        {
            throw new InvalidOperationException($"BaseUrl '{_settings.BaseUrl}' is not a valid absolute URL");
        }

        if (_settings.Execution.DefaultTimeout <= 0)
        {
            throw new InvalidOperationException("Execution.DefaultTimeout must be greater than 0");
        }

        if (_settings.Execution.NavigationTimeout <= 0)
        {
            throw new InvalidOperationException("Execution.NavigationTimeout must be greater than 0");
        }

        if (_settings.Execution.MaxRetries < 0)
        {
            throw new InvalidOperationException("Execution.MaxRetries cannot be negative");
        }

        if (_settings.Execution.RetryDelay < 0)
        {
            throw new InvalidOperationException("Execution.RetryDelay cannot be negative");
        }

        if (string.IsNullOrWhiteSpace(_settings.Logging.MinimumLevel))
        {
            throw new InvalidOperationException("Logging.MinimumLevel cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(_settings.Logging.OutputPath))
        {
            throw new InvalidOperationException("Logging.OutputPath cannot be empty");
        }
    }

    /// <summary>
    /// Gets a configuration value by key
    /// </summary>
    /// <param name="key">Configuration key to retrieve</param>
    /// <returns>Configuration value or null if not found</returns>
    public string? GetValue(string key)
    {
        ArgumentNullException.ThrowIfNull(key);
        return _configuration[key];
    }

    /// <summary>
    /// Gets a strongly-typed configuration section
    /// </summary>
    /// <typeparam name="T">Type to bind the configuration section to</typeparam>
    /// <param name="sectionName">Name of the configuration section</param>
    /// <returns>Typed configuration section or null if not found</returns>
    public T? GetSection<T>(string sectionName) where T : class
    {
        ArgumentNullException.ThrowIfNull(sectionName);
        return _configuration.GetSection(sectionName).Get<T>();
    }
}
