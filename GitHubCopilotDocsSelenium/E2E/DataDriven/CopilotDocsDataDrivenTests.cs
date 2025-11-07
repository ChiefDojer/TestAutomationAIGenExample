using GitHubCopilotDocs.Selenium.Tests.Core;
using GitHubCopilotDocs.Selenium.Tests.Data;
using GitHubCopilotDocs.Selenium.Tests.Pages.GitHub;
using FluentAssertions;
using OpenQA.Selenium.Support.UI;

namespace GitHubCopilotDocs.Selenium.Tests.E2E.DataDriven;

/// <summary>
/// Data-driven tests for GitHub Copilot documentation.
/// </summary>
[TestFixture]
[Category("DataDriven")]
public class CopilotDocsDataDrivenTests : BaseTest
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
    [TestCaseSource(typeof(TestDataProvider), nameof(TestDataProvider.NavigationTestData))]
    public void Navigation_NavigateToPath_PageLoadsSuccessfully(NavigationTestData testData)
    {
        // Arrange
        LogStep($"Navigate to: {testData.Path}");

        // Act
        NavigateTo(testData.Path);
        _homePage!.WaitForPageLoad();
        var title = _homePage.GetTitle();

        // Assert
        LogStep($"Verify page title contains: {testData.ExpectedTitleKeyword}");
        title.Should().Contain(testData.ExpectedTitleKeyword, 
            $"because navigating to {testData.Path} should show a page with '{testData.ExpectedTitleKeyword}' in the title");
        Logger.Information($"Page title: {title}");
    }

    [Test]
    [TestCase("/en/copilot", "Copilot")]
    [TestCase("/en/copilot/quickstart", "Quick")]
    [TestCase("/en/copilot/overview", "Overview")]
    public void PageTitle_NavigateToDifferentPages_TitleContainsExpectedKeyword(string path, string expectedKeyword)
    {
        // Arrange
        LogStep($"Navigate to: {path}");

        // Act
        NavigateTo(path);
        _homePage!.WaitForPageLoad();
        var title = _homePage.GetTitle();

        // Assert
        LogStep($"Verify title contains: {expectedKeyword}");
        title.Should().Contain(expectedKeyword, 
            $"because the page at {path} should have '{expectedKeyword}' in the title");
        Logger.Information($"Page title: {title}");
    }

    [Test]
    [TestCase("/en/copilot")]
    [TestCase("/en/copilot/quickstart")]
    [TestCase("/en/copilot/overview")]
    public void Navigation_VisitMultiplePages_AllPagesLoadSuccessfully(string path)
    {
        // Arrange
        LogStep($"Navigate to: {path}");

        // Act
        NavigateTo(path);
        _homePage!.WaitForPageLoad();
        var heading = _homePage.GetMainHeading();

        // Assert
        LogStep("Verify page has loaded with heading");
        heading.Should().NotBeNullOrWhiteSpace("because every page should have a main heading");
        Logger.Information($"Main heading: {heading}");
    }

    [Test]
    [TestCase("GitHub Copilot", "Copilot")]
    [TestCase("code completion", "code")]
    [TestCase("AI programming", "AI")]
    public void Search_SearchForDifferentTerms_SearchIsPerformed(string searchQuery, string expectedKeyword)
    {
        // Arrange
        LogStep("Navigate to GitHub Copilot documentation");
        NavigateTo("/en/copilot");
        _homePage!.WaitForPageLoad();

        // Act
        LogStep($"Search for: {searchQuery}");
        _homePage.Search(searchQuery);
        Thread.Sleep(2000); // Wait for search to process

        // Assert
        LogStep("Verify search was performed");
        var currentUrl = _homePage.GetCurrentUrl();
        currentUrl.Should().Contain("search", "because performing a search should navigate to search results");
        Logger.Information($"Search performed, URL: {currentUrl}");
    }

    [Test]
    [TestCaseSource(typeof(TestDataProvider), nameof(TestDataProvider.ViewportTestData))]
    public void Responsive_TestDifferentViewports_PageLoadsCorrectly(ViewportTestData testData)
    {
        // Arrange
        LogStep($"Set viewport to: {testData.Description} ({testData.Width}x{testData.Height})");
        Driver.Manage().Window.Size = new System.Drawing.Size(testData.Width, testData.Height);

        // Act
        LogStep("Navigate to GitHub Copilot documentation");
        NavigateTo("/en/copilot");
        _homePage!.WaitForPageLoad();
        var heading = _homePage.GetMainHeading();

        // Assert
        LogStep($"Verify page loads correctly at {testData.Description} viewport");
        heading.Should().NotBeNullOrWhiteSpace("because the page should load correctly at any viewport size");
        Logger.Information($"Page loaded successfully at {testData.Width}x{testData.Height}");
    }
}
