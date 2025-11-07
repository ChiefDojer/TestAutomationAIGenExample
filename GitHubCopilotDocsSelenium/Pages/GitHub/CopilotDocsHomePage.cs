using GitHubCopilotDocs.Selenium.Tests.Core.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace GitHubCopilotDocs.Selenium.Tests.Pages.GitHub;

/// <summary>
/// Page object for the GitHub Copilot documentation homepage.
/// </summary>
public class CopilotDocsHomePage : BasePage
{
    // Locators
    private static readonly By MainHeadingLocator = By.CssSelector("h1.d-flex");
    private static readonly By NavigationLocator = By.CssSelector("nav[aria-label='Main']");
    private static readonly By SearchInputLocator = By.CssSelector("input[type='search']");
    private static readonly By ArticleLinksLocator = By.CssSelector("article a");
    private static readonly By BreadcrumbLocator = By.CssSelector("nav[aria-label='Breadcrumb']");
    private static readonly By SidebarLocator = By.CssSelector("aside[class*='sidebar']");
    private static readonly By FooterLocator = By.TagName("footer");

    /// <summary>
    /// Initializes a new instance of the <see cref="CopilotDocsHomePage"/> class.
    /// </summary>
    public CopilotDocsHomePage(IWebDriver driver, WebDriverWait wait, TestLogger logger)
        : base(driver, wait, logger)
    {
    }

    /// <summary>
    /// Gets the main heading text.
    /// </summary>
    public string GetMainHeading()
    {
        Logger.Information("Getting main heading text");
        return GetText(MainHeadingLocator);
    }

    /// <summary>
    /// Performs a search with the given query.
    /// </summary>
    public void Search(string query)
    {
        ArgumentNullException.ThrowIfNull(query);
        Logger.Information($"Searching for: {query}");
        Type(SearchInputLocator, query);
        PressKey(SearchInputLocator, Keys.Enter);
    }

    /// <summary>
    /// Checks if the navigation bar is visible.
    /// </summary>
    public bool IsNavigationVisible()
    {
        Logger.Information("Checking if navigation is visible");
        return IsVisible(NavigationLocator);
    }

    /// <summary>
    /// Gets the count of article links on the page.
    /// </summary>
    public int GetArticleLinksCount()
    {
        Logger.Information("Counting article links");
        return GetCount(ArticleLinksLocator);
    }

    /// <summary>
    /// Checks if the breadcrumb is visible.
    /// </summary>
    public bool IsBreadcrumbVisible()
    {
        Logger.Information("Checking if breadcrumb is visible");
        return IsVisible(BreadcrumbLocator);
    }

    /// <summary>
    /// Checks if the sidebar is visible.
    /// </summary>
    public bool IsSidebarVisible()
    {
        Logger.Information("Checking if sidebar is visible");
        return IsVisible(SidebarLocator);
    }

    /// <summary>
    /// Checks if the footer is visible.
    /// </summary>
    public bool IsFooterVisible()
    {
        Logger.Information("Checking if footer is visible");
        return IsVisible(FooterLocator);
    }

    /// <summary>
    /// Checks if the search input is enabled.
    /// </summary>
    public bool IsSearchEnabled()
    {
        Logger.Information("Checking if search is enabled");
        return IsEnabled(SearchInputLocator);
    }

    /// <summary>
    /// Waits for the page to fully load.
    /// </summary>
    public void WaitForPageLoad(int? timeout = null)
    {
        Logger.Information("Waiting for page to load");
        WaitForVisible(MainHeadingLocator, timeout);
    }

    /// <summary>
    /// Clicks the first article link.
    /// </summary>
    public void ClickFirstArticleLink()
    {
        Logger.Information("Clicking first article link");
        Click(ArticleLinksLocator);
    }

    /// <summary>
    /// Gets all article link texts.
    /// </summary>
    public List<string> GetAllArticleLinksText()
    {
        Logger.Information("Getting all article link texts");
        var elements = Driver.FindElements(ArticleLinksLocator);
        return elements.Select(e => e.Text).Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
    }
}
