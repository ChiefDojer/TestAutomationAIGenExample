using FluentAssertions;
using GitHubCopilotDocs.Tests.Core;
using GitHubCopilotDocs.Tests.Pages.GitHub;
using NUnit.Framework;

namespace GitHubCopilotDocs.Tests.E2E;

/// <summary>
/// End-to-end tests for GitHub Copilot Documentation Home Page
/// </summary>
[TestFixture]
[Category("E2E")]
[Category("Smoke")]
public class CopilotDocsHomePageTests : BaseTest
{
    private CopilotDocsHomePage? _homePage;

    [SetUp]
    public override async Task SetUp()
    {
        await base.SetUp();
        _homePage = new CopilotDocsHomePage(Page, Logger!);
    }

    [Test]
    public async Task PageLoad_NavigateToCopilotDocs_PageLoadsWithMainHeading()
    {
        // Arrange
        LogStep("Navigate to GitHub Copilot documentation");

        // Act
        await NavigateToAsync("/en/copilot");
        await _homePage!.WaitForPageLoadAsync();
        var heading = await _homePage.GetMainHeadingAsync();

        // Assert
        LogStep("Verify main heading is present");
        heading.Should().NotBeNullOrWhiteSpace("because the main heading should be visible on the page");
        heading.Should().Contain("Copilot", "because this is the Copilot documentation page");
    }

    [Test]
    public async Task Navigation_CheckNavigationBar_NavigationIsVisible()
    {
        // Arrange
        LogStep("Navigate to Copilot docs and check navigation");
        await NavigateToAsync("/en/copilot");

        // Act
        var isNavigationVisible = await _homePage!.IsNavigationVisibleAsync();

        // Assert
        LogStep("Verify navigation bar is visible");
        isNavigationVisible.Should().BeTrue("because navigation should always be visible");
    }

    [Test]
    public async Task Search_CheckSearchInput_SearchIsEnabled()
    {
        // Arrange
        LogStep("Navigate to Copilot docs");
        await NavigateToAsync("/en/copilot");

        // Act
        var isSearchEnabled = await _homePage!.IsSearchEnabledAsync();

        // Assert
        LogStep("Verify search input is enabled");
        isSearchEnabled.Should().BeTrue("because search functionality should be available");
    }

    [Test]
    public async Task ArticleLinks_CountArticleLinks_ShouldHaveMultipleLinks()
    {
        // Arrange
        LogStep("Navigate to Copilot docs");
        await NavigateToAsync("/en/copilot");

        // Act
        var linkCount = await _homePage!.GetArticleLinksCountAsync();

        // Assert
        LogStep($"Verify article links count (found: {linkCount})");
        linkCount.Should().BeGreaterThan(0, "because documentation should have article links");
    }

    [Test]
    public async Task Breadcrumb_CheckBreadcrumb_BreadcrumbIsVisible()
    {
        // Arrange
        LogStep("Navigate to Copilot docs");
        await NavigateToAsync("/en/copilot");

        // Act
        var isBreadcrumbVisible = await _homePage!.IsBreadcrumbVisibleAsync();

        // Assert
        LogStep("Verify breadcrumb is visible");
        isBreadcrumbVisible.Should().BeTrue("because breadcrumb navigation should be present");
    }

    [Test]
    public async Task Sidebar_CheckSidebar_SidebarIsVisible()
    {
        // Arrange
        LogStep("Navigate to Copilot docs");
        await NavigateToAsync("/en/copilot");

        // Act
        var isSidebarVisible = await _homePage!.IsSidebarVisibleAsync();

        // Assert
        LogStep("Verify sidebar is visible");
        isSidebarVisible.Should().BeTrue("because sidebar should provide navigation options");
    }

    [Test]
    public async Task Footer_CheckFooter_FooterIsVisible()
    {
        // Arrange
        LogStep("Navigate to Copilot docs");
        await NavigateToAsync("/en/copilot");

        // Act
        var isFooterVisible = await _homePage!.IsFooterVisibleAsync();

        // Assert
        LogStep("Verify footer is visible");
        isFooterVisible.Should().BeTrue("because footer should be present on all pages");
    }

    [Test]
    public async Task PageTitle_GetPageTitle_ShouldContainCopilot()
    {
        // Arrange
        LogStep("Navigate to Copilot docs");
        await NavigateToAsync("/en/copilot");

        // Act
        var title = await _homePage!.GetTitleAsync();

        // Assert
        LogStep($"Verify page title contains 'Copilot' (actual: {title})");
        title.Should().Contain("Copilot", "because page title should reflect the content");
    }

    [Test]
    public async Task URL_NavigateToCopilotDocs_URLIsCorrect()
    {
        // Arrange
        LogStep("Navigate to Copilot docs");
        await NavigateToAsync("/en/copilot");

        // Act
        var currentUrl = _homePage!.GetCurrentUrl();

        // Assert
        LogStep($"Verify URL is correct (actual: {currentUrl})");
        currentUrl.Should().Contain("/en/copilot", "because we navigated to the Copilot documentation");
    }
}
