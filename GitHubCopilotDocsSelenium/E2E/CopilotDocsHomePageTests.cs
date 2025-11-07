using GitHubCopilotDocs.Selenium.Tests.Core;
using GitHubCopilotDocs.Selenium.Tests.Pages.GitHub;
using FluentAssertions;
using OpenQA.Selenium.Support.UI;

namespace GitHubCopilotDocs.Selenium.Tests.E2E;

/// <summary>
/// End-to-end tests for GitHub Copilot documentation homepage.
/// </summary>
[TestFixture]
[Category("Smoke")]
[Category("E2E")]
public class CopilotDocsHomePageTests : BaseTest
{
    private CopilotDocsHomePage? _homePage;

    [SetUp]
    public override void SetUp()
    {
        base.SetUp();
        var wait = new WebDriverWait(Driver, TimeSpan.FromMilliseconds(Settings.Execution.DefaultTimeout));
        _homePage = new CopilotDocsHomePage(Driver, wait, Logger);
    }

    [Test]
    public void PageLoad_NavigateToCopilotDocs_PageLoadsWithMainHeading()
    {
        // Arrange
        LogStep("Navigate to GitHub Copilot documentation");

        // Act
        NavigateTo("/en/copilot");
        _homePage!.WaitForPageLoad();
        var heading = _homePage.GetMainHeading();

        // Assert
        LogStep("Verify main heading is present");
        heading.Should().NotBeNullOrWhiteSpace("because the page should have a main heading");
        Logger.Information($"Main heading: {heading}");
    }

    [Test]
    public void Navigation_CheckNavigationBar_NavigationIsVisible()
    {
        // Arrange
        LogStep("Navigate to GitHub Copilot documentation");
        NavigateTo("/en/copilot");
        _homePage!.WaitForPageLoad();

        // Act
        LogStep("Check navigation bar visibility");
        var isVisible = _homePage.IsNavigationVisible();

        // Assert
        isVisible.Should().BeTrue("because the navigation bar should be visible on the page");
    }

    [Test]
    public void Search_CheckSearchInput_SearchIsEnabled()
    {
        // Arrange
        LogStep("Navigate to GitHub Copilot documentation");
        NavigateTo("/en/copilot");
        _homePage!.WaitForPageLoad();

        // Act
        LogStep("Check search input availability");
        var isEnabled = _homePage.IsSearchEnabled();

        // Assert
        isEnabled.Should().BeTrue("because the search input should be enabled");
    }

    [Test]
    public void ArticleLinks_CountArticleLinks_ShouldHaveMultipleLinks()
    {
        // Arrange
        LogStep("Navigate to GitHub Copilot documentation");
        NavigateTo("/en/copilot");
        _homePage!.WaitForPageLoad();

        // Act
        LogStep("Count article links");
        var count = _homePage.GetArticleLinksCount();

        // Assert
        count.Should().BeGreaterThan(0, "because the page should have article links");
        Logger.Information($"Found {count} article links");
    }

    [Test]
    public void Breadcrumb_CheckBreadcrumb_BreadcrumbIsVisible()
    {
        // Arrange
        LogStep("Navigate to GitHub Copilot documentation");
        NavigateTo("/en/copilot");
        _homePage!.WaitForPageLoad();

        // Act
        LogStep("Check breadcrumb visibility");
        var isVisible = _homePage.IsBreadcrumbVisible();

        // Assert
        isVisible.Should().BeTrue("because the breadcrumb should be visible on the page");
    }

    [Test]
    public void Sidebar_CheckSidebar_SidebarIsVisible()
    {
        // Arrange
        LogStep("Navigate to GitHub Copilot documentation");
        NavigateTo("/en/copilot");
        _homePage!.WaitForPageLoad();

        // Act
        LogStep("Check sidebar visibility");
        var isVisible = _homePage.IsSidebarVisible();

        // Assert
        isVisible.Should().BeTrue("because the sidebar should be visible on the page");
    }

    [Test]
    public void Footer_CheckFooter_FooterIsVisible()
    {
        // Arrange
        LogStep("Navigate to GitHub Copilot documentation");
        NavigateTo("/en/copilot");
        _homePage!.WaitForPageLoad();

        // Act
        LogStep("Check footer visibility");
        var isVisible = _homePage.IsFooterVisible();

        // Assert
        isVisible.Should().BeTrue("because the footer should be visible on the page");
    }

    [Test]
    public void PageTitle_GetPageTitle_ShouldContainCopilot()
    {
        // Arrange
        LogStep("Navigate to GitHub Copilot documentation");
        NavigateTo("/en/copilot");
        _homePage!.WaitForPageLoad();

        // Act
        LogStep("Get page title");
        var title = _homePage.GetTitle();

        // Assert
        title.Should().Contain("Copilot", "because the page title should reference Copilot");
        Logger.Information($"Page title: {title}");
    }

    [Test]
    public void URL_NavigateToCopilotDocs_URLIsCorrect()
    {
        // Arrange
        LogStep("Navigate to GitHub Copilot documentation");

        // Act
        NavigateTo("/en/copilot");
        _homePage!.WaitForPageLoad();
        var currentUrl = _homePage.GetCurrentUrl();

        // Assert
        LogStep("Verify URL contains expected path");
        currentUrl.Should().Contain("/en/copilot", "because the URL should contain the Copilot documentation path");
        Logger.Information($"Current URL: {currentUrl}");
    }
}
