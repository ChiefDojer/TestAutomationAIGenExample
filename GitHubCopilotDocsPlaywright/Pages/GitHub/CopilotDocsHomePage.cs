using GitHubCopilotDocs.Tests.Core.Logging;
using Microsoft.Playwright;

namespace GitHubCopilotDocs.Tests.Pages.GitHub;

/// <summary>
/// Page Object Model for GitHub Copilot Documentation Home Page
/// URL: https://docs.github.com/en/copilot
/// </summary>
public class CopilotDocsHomePage : BasePage
{
    // Locators using stable selectors
    private const string MainHeadingSelector = "h1.d-flex";
    private const string SearchInputSelector = "[data-testid='site-search-input']";
    private const string NavigationSelector = "[role='navigation']";
    private const string ArticleLinksSelector = "article a";
    private const string BreadcrumbSelector = "[data-testid='breadcrumbs']";
    private const string SidebarSelector = "[data-testid='sidebar']";
    private const string FooterSelector = "footer";
    private const string LanguageSelectorSelector = "[data-testid='language-picker']";
    private const string ThemeSelectorSelector = "[data-testid='theme-picker']";
    private const string VersionSelectorSelector = "[data-testid='version-picker']";

    public CopilotDocsHomePage(IPage page, TestLogger logger) : base(page, logger)
    {
    }

    /// <summary>
    /// Gets the main heading text
    /// </summary>
    public async Task<string> GetMainHeadingAsync()
    {
        Logger.Information("Getting main heading text");
        return await GetTextAsync(MainHeadingSelector);
    }

    /// <summary>
    /// Searches for documentation using the search box
    /// </summary>
    public async Task SearchAsync(string query)
    {
        Logger.Information($"Searching for: {query}");
        await ClickAsync(SearchInputSelector);
        await TypeAsync(SearchInputSelector, query);
        await PressKeyAsync("Enter");
    }

    /// <summary>
    /// Checks if navigation is visible
    /// </summary>
    public async Task<bool> IsNavigationVisibleAsync()
    {
        return await IsVisibleAsync(NavigationSelector);
    }

    /// <summary>
    /// Gets the count of article links on the page
    /// </summary>
    public async Task<int> GetArticleLinksCountAsync()
    {
        Logger.Information("Counting article links");
        return await GetCountAsync(ArticleLinksSelector);
    }

    /// <summary>
    /// Checks if breadcrumb is visible
    /// </summary>
    public async Task<bool> IsBreadcrumbVisibleAsync()
    {
        return await IsVisibleAsync(BreadcrumbSelector);
    }

    /// <summary>
    /// Checks if sidebar is visible
    /// </summary>
    public async Task<bool> IsSidebarVisibleAsync()
    {
        return await IsVisibleAsync(SidebarSelector);
    }

    /// <summary>
    /// Checks if footer is visible
    /// </summary>
    public async Task<bool> IsFooterVisibleAsync()
    {
        return await IsVisibleAsync(FooterSelector);
    }

    /// <summary>
    /// Checks if search input is enabled
    /// </summary>
    public async Task<bool> IsSearchEnabledAsync()
    {
        return await IsEnabledAsync(SearchInputSelector);
    }

    /// <summary>
    /// Waits for page to fully load by checking for main heading
    /// </summary>
    public async Task WaitForPageLoadAsync(int? timeout = null)
    {
        Logger.Information("Waiting for page to load");
        await WaitForVisibleAsync(MainHeadingSelector, timeout);
    }

    /// <summary>
    /// Clicks on the first article link
    /// </summary>
    public async Task ClickFirstArticleLinkAsync()
    {
        Logger.Information("Clicking first article link");
        var firstLink = Page.Locator(ArticleLinksSelector).First;
        await firstLink.ClickAsync();
    }

    /// <summary>
    /// Gets the text of all article links
    /// </summary>
    public async Task<List<string>> GetAllArticleLinksTextAsync()
    {
        Logger.Information("Getting all article link texts");
        var links = await Page.Locator(ArticleLinksSelector).AllAsync();
        var texts = new List<string>();

        foreach (var link in links)
        {
            var text = await link.TextContentAsync();
            if (!string.IsNullOrWhiteSpace(text))
                texts.Add(text.Trim());
        }

        return texts;
    }

    /// <summary>
    /// Gets the current page URL
    /// </summary>
    public new string GetCurrentUrl()
    {
        return base.GetCurrentUrl();
    }

    /// <summary>
    /// Gets the page title
    /// </summary>
    public new async Task<string> GetTitleAsync()
    {
        return await base.GetTitleAsync();
    }
}
