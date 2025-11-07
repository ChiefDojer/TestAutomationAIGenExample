using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.UI;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using GitHubCopilotDocs.Selenium.Tests.Core.Configuration;
using GitHubCopilotDocs.Selenium.Tests.Core.Logging;

namespace GitHubCopilotDocs.Selenium.Tests.Core.Driver;

/// <summary>
/// Manages Selenium WebDriver lifecycle and configuration.
/// </summary>
public sealed class SeleniumDriverManager : IDisposable
{
    private IWebDriver? _driver;
    private WebDriverWait? _wait;
    private readonly TestLogger _logger;
    private bool _disposed;

    /// <summary>
    /// Gets the WebDriver instance.
    /// </summary>
    public IWebDriver Driver => _driver ?? throw new InvalidOperationException("Driver not initialized. Call InitializeAsync first.");

    /// <summary>
    /// Gets the WebDriverWait instance for explicit waits.
    /// </summary>
    public WebDriverWait Wait => _wait ?? throw new InvalidOperationException("Wait not initialized. Call InitializeAsync first.");

    /// <summary>
    /// Initializes a new instance of the <see cref="SeleniumDriverManager"/> class.
    /// </summary>
    public SeleniumDriverManager(TestLogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    /// <summary>
    /// Initializes the WebDriver with specified configuration.
    /// </summary>
    public void Initialize(BrowserSettings browserSettings, ExecutionSettings executionSettings)
    {
        ArgumentNullException.ThrowIfNull(browserSettings);
        ArgumentNullException.ThrowIfNull(executionSettings);

        _logger.Information($"Initializing {browserSettings.Type} browser (Headless: {browserSettings.Headless})");

        _driver = CreateDriver(browserSettings);
        
        // Configure timeouts
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(executionSettings.DefaultTimeout);
        _driver.Manage().Timeouts().PageLoad = TimeSpan.FromMilliseconds(executionSettings.PageLoadTimeout);

        // Configure window
        if (browserSettings.Maximize)
        {
            _driver.Manage().Window.Maximize();
        }
        else
        {
            _driver.Manage().Window.Size = new System.Drawing.Size(
                browserSettings.ViewportWidth,
                browserSettings.ViewportHeight
            );
        }

        // Initialize WebDriverWait
        _wait = new WebDriverWait(_driver, TimeSpan.FromMilliseconds(executionSettings.DefaultTimeout));

        _logger.Information("WebDriver initialized successfully");
    }

    private IWebDriver CreateDriver(BrowserSettings settings)
    {
        return settings.Type.ToLowerInvariant() switch
        {
            "chrome" or "chromium" => CreateChromeDriver(settings),
            "firefox" => CreateFirefoxDriver(settings),
            "edge" => CreateEdgeDriver(settings),
            "safari" => CreateSafariDriver(settings),
            _ => throw new ArgumentException($"Unsupported browser type: {settings.Type}")
        };
    }

    private IWebDriver CreateChromeDriver(BrowserSettings settings)
    {
        new DriverManager().SetUpDriver(new ChromeConfig());
        var options = new ChromeOptions();
        
        if (settings.Headless)
        {
            options.AddArgument("--headless=new");
        }
        
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument($"--window-size={settings.ViewportWidth},{settings.ViewportHeight}");
        
        if (settings.AcceptInsecureCerts)
        {
            options.AcceptInsecureCertificates = true;
        }

        return new ChromeDriver(options);
    }

    private IWebDriver CreateFirefoxDriver(BrowserSettings settings)
    {
        new DriverManager().SetUpDriver(new FirefoxConfig());
        var options = new FirefoxOptions();
        
        if (settings.Headless)
        {
            options.AddArgument("--headless");
        }
        
        options.SetPreference("width", settings.ViewportWidth);
        options.SetPreference("height", settings.ViewportHeight);
        
        if (settings.AcceptInsecureCerts)
        {
            options.AcceptInsecureCertificates = true;
        }

        return new FirefoxDriver(options);
    }

    private IWebDriver CreateEdgeDriver(BrowserSettings settings)
    {
        new DriverManager().SetUpDriver(new EdgeConfig());
        var options = new EdgeOptions();
        
        if (settings.Headless)
        {
            options.AddArgument("--headless=new");
        }
        
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument($"--window-size={settings.ViewportWidth},{settings.ViewportHeight}");
        
        if (settings.AcceptInsecureCerts)
        {
            options.AcceptInsecureCertificates = true;
        }

        return new EdgeDriver(options);
    }

    private IWebDriver CreateSafariDriver(BrowserSettings settings)
    {
        var options = new SafariOptions();
        
        if (settings.AcceptInsecureCerts)
        {
            options.AcceptInsecureCertificates = true;
        }

        // Note: Safari doesn't support headless mode natively
        if (settings.Headless)
        {
            _logger.Warning("Safari does not support headless mode. Running in headed mode.");
        }

        return new SafariDriver(options);
    }

    /// <summary>
    /// Captures a screenshot of the current page.
    /// </summary>
    public byte[] CaptureScreenshot()
    {
        if (_driver == null)
        {
            throw new InvalidOperationException("Driver not initialized");
        }

        var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
        return screenshot.AsByteArray;
    }

    /// <summary>
    /// Gets the page HTML content.
    /// </summary>
    public string GetPageSource()
    {
        if (_driver == null)
        {
            throw new InvalidOperationException("Driver not initialized");
        }

        return _driver.PageSource;
    }

    /// <summary>
    /// Disposes of the WebDriver instance.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        try
        {
            _driver?.Quit();
            _driver?.Dispose();
            _logger.Information("WebDriver disposed successfully");
        }
        catch (Exception ex)
        {
            _logger.Error("Error disposing WebDriver", ex);
        }
        finally
        {
            _disposed = true;
        }
    }
}
