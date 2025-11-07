using FluentAssertions;
using GitHubCopilotDocs.Tests.Core;
using GitHubCopilotDocs.Tests.Data;
using GitHubCopilotDocs.Tests.Pages.GitHub;
using NUnit.Framework;

namespace GitHubCopilotDocs.Tests.E2E.DataDriven;

/// <summary>
/// Data-driven tests for GitHub Copilot Documentation using parameterized test cases
/// </summary>
[TestFixture]
[Category("E2E")]
[Category("DataDriven")]
public class CopilotDocsDataDrivenTests : BaseTest
{
    private CopilotDocsHomePage? _homePage;

    [SetUp]
    public override async Task SetUp()
    {
        await base.SetUp();
        _homePage = new CopilotDocsHomePage(Page, Logger!);
    }

    [Test]
    [TestCaseSource(typeof(TestDataProvider), nameof(TestDataProvider.NavigationTestData))]
    public async Task Navigation_NavigateToPath_PageLoadsSuccessfully(NavigationTestData testData)
    {
        // Arrange
        LogStep($"Navigate to: {testData.Path}");

        // Act
        await NavigateToAsync(testData.Path);
        var title = await _homePage!.GetTitleAsync();

        // Assert
        LogStep($"Verify page title contains: {testData.ExpectedTitleKeyword}");
        title.Should().Contain(testData.ExpectedTitleKeyword,
            $"because navigating to {testData.Path} should load a page with '{testData.ExpectedTitleKeyword}' in the title");
    }

    [Test]
    [TestCase("/en/copilot", "Copilot")]
    [TestCase("/en/copilot/quickstart", "quickstart")]
    [TestCase("/en/copilot/using-github-copilot", "Using")]
    public async Task PageTitle_NavigateToDifferentPages_TitleContainsExpectedKeyword(string path, string expectedKeyword)
    {
        // Arrange
        LogStep($"Navigate to: {path}");

        // Act
        await NavigateToAsync(path);
        var title = await _homePage!.GetTitleAsync();

        // Assert
        LogStep($"Verify title contains: {expectedKeyword}");
        title.Should().Contain(expectedKeyword,
            $"because {path} should have '{expectedKeyword}' in the title");
    }

    [Test]
    [TestCase("/en/copilot")]
    [TestCase("/en/copilot/quickstart")]
    [TestCase("/en/copilot/using-github-copilot")]
    public async Task Navigation_VisitMultiplePages_AllPagesLoadSuccessfully(string path)
    {
        // Arrange
        LogStep($"Navigate to: {path}");

        // Act
        await NavigateToAsync(path);
        await _homePage!.WaitForPageLoadAsync();
        var heading = await _homePage.GetMainHeadingAsync();

        // Assert
        LogStep("Verify page loaded with main heading");
        heading.Should().NotBeNullOrWhiteSpace($"because {path} should load with a main heading");
    }

    [Test]
    [TestCase("authentication")]
    [TestCase("quickstart")]
    [TestCase("getting started")]
    public async Task Search_SearchForDifferentTerms_SearchIsPerformed(string searchQuery)
    {
        // Arrange
        LogStep("Navigate to Copilot docs");
        await NavigateToAsync("/en/copilot");

        // Act
        LogStep($"Perform search for: {searchQuery}");
        await _homePage!.SearchAsync(searchQuery);
        await Task.Delay(2000); // Wait for search to process

        var currentUrl = _homePage.GetCurrentUrl();

        // Assert
        LogStep("Verify search was performed (URL changed or contains query)");
        currentUrl.Should().NotBe(Settings.BaseUrl + "/en/copilot",
            $"because searching for '{searchQuery}' should navigate to search results or modify the URL");
    }

    [Test]
    [TestCase(1920, 1080)]
    [TestCase(1366, 768)]
    [TestCase(768, 1024)]
    public async Task Responsive_TestDifferentViewports_PageLoadsCorrectly(int width, int height)
    {
        // Arrange
        LogStep($"Set viewport to: {width}x{height}");
        await Page.SetViewportSizeAsync(width, height);

        // Act
        await NavigateToAsync("/en/copilot");
        await _homePage!.WaitForPageLoadAsync();
        var heading = await _homePage.GetMainHeadingAsync();

        // Assert
        LogStep($"Verify page loads correctly at {width}x{height}");
        heading.Should().NotBeNullOrWhiteSpace(
            $"because page should load correctly at {width}x{height} resolution");
    }
}
