using Microsoft.Playwright;

namespace GitHubCopilotDocs.Tests.Core.Driver;

/// <summary>
/// Manages Playwright browser driver lifecycle with proper async disposal.
/// Handles browser, context, and page creation with configured settings.
/// </summary>
public sealed class PlaywrightDriverManager : IAsyncDisposable
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IBrowserContext? _context;
    private IPage? _page;
    private bool _disposed;

    /// <summary>
    /// Gets the current page instance
    /// </summary>
    public IPage Page => _page ?? throw new InvalidOperationException("Page not initialized. Call InitializeAsync first.");

    /// <summary>
    /// Initializes the Playwright driver with the specified browser settings
    /// </summary>
    public async Task InitializeAsync(
        string browserType,
        bool headless = true,
        int viewportWidth = 1920,
        int viewportHeight = 1080,
        bool recordVideo = false,
        bool captureTrace = false)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(PlaywrightDriverManager));

        _playwright = await Playwright.CreateAsync();

        _browser = browserType.ToLowerInvariant() switch
        {
            "chromium" => await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = headless }),
            "firefox" => await _playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = headless }),
            "webkit" => await _playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions { Headless = headless }),
            _ => throw new ArgumentException($"Unsupported browser type: {browserType}", nameof(browserType))
        };

        var contextOptions = new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = viewportWidth, Height = viewportHeight },
            RecordVideoDir = recordVideo ? "TestResults/Videos" : null
        };

        _context = await _browser.NewContextAsync(contextOptions);

        if (captureTrace)
        {
            await _context.Tracing.StartAsync(new TracingStartOptions
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
        }

        _page = await _context.NewPageAsync();
    }

    /// <summary>
    /// Captures a screenshot of the current page
    /// </summary>
    public async Task<byte[]> CaptureScreenshotAsync(string? path = null)
    {
        ArgumentNullException.ThrowIfNull(_page);

        var options = new PageScreenshotOptions { FullPage = true };
        if (!string.IsNullOrWhiteSpace(path))
            options.Path = path;

        return await _page.ScreenshotAsync(options);
    }

    /// <summary>
    /// Captures a Playwright trace for debugging
    /// </summary>
    public async Task CaptureTraceAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(_context);
        await _context.Tracing.StopAsync(new TracingStopOptions { Path = path });
    }

    /// <summary>
    /// Gets the current page HTML content
    /// </summary>
    public async Task<string> GetPageContentAsync()
    {
        ArgumentNullException.ThrowIfNull(_page);
        return await _page.ContentAsync();
    }

    /// <summary>
    /// Disposes all Playwright resources asynchronously
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        if (_page != null)
            await _page.CloseAsync();

        if (_context != null)
            await _context.CloseAsync();

        if (_browser != null)
            await _browser.CloseAsync();

        _playwright?.Dispose();

        _disposed = true;
        GC.SuppressFinalize(this);
    }
}
