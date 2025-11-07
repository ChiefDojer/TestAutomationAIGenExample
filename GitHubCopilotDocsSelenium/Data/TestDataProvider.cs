namespace GitHubCopilotDocs.Selenium.Tests.Data;

/// <summary>
/// Test data for search functionality.
/// </summary>
public record SearchTestData(string Query, string ExpectedKeyword);

/// <summary>
/// Test data for navigation scenarios.
/// </summary>
public record NavigationTestData(string Path, string ExpectedTitleKeyword);

/// <summary>
/// Test data for viewport/responsive testing.
/// </summary>
public record ViewportTestData(int Width, int Height, string Description);

/// <summary>
/// Provides centralized test data for data-driven tests.
/// </summary>
public static class TestDataProvider
{
    /// <summary>
    /// Gets search test data.
    /// </summary>
    public static IEnumerable<SearchTestData> SearchTestData()
    {
        yield return new SearchTestData("GitHub Copilot", "Copilot");
        yield return new SearchTestData("code completion", "completion");
        yield return new SearchTestData("AI pair programming", "AI");
    }

    /// <summary>
    /// Gets navigation test data.
    /// </summary>
    public static IEnumerable<NavigationTestData> NavigationTestData()
    {
        yield return new NavigationTestData("/en/copilot", "Copilot");
        yield return new NavigationTestData("/en/copilot/quickstart", "Quick");
        yield return new NavigationTestData("/en/copilot/overview", "Overview");
    }

    /// <summary>
    /// Gets viewport test data for responsive testing.
    /// </summary>
    public static IEnumerable<ViewportTestData> ViewportTestData()
    {
        yield return new ViewportTestData(1920, 1080, "Desktop HD");
        yield return new ViewportTestData(1366, 768, "Laptop");
        yield return new ViewportTestData(768, 1024, "Tablet Portrait");
    }
}
