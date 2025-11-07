namespace GitHubCopilotDocs.Tests.Data;

/// <summary>
/// Provides test data for data-driven tests
/// </summary>
public static class TestDataProvider
{
    /// <summary>
    /// Test data for search functionality
    /// </summary>
    public static IEnumerable<SearchTestData> SearchTestData()
    {
        yield return new SearchTestData("authentication", "authentication");
        yield return new SearchTestData("quickstart", "quickstart");
        yield return new SearchTestData("getting started", "getting started");
        yield return new SearchTestData("API", "API");
        yield return new SearchTestData("troubleshooting", "troubleshooting");
    }

    /// <summary>
    /// Test data for navigation testing
    /// </summary>
    public static IEnumerable<NavigationTestData> NavigationTestData()
    {
        yield return new NavigationTestData("/en/copilot", "GitHub Copilot");
        yield return new NavigationTestData("/en/copilot/quickstart", "quickstart");
    }
}

/// <summary>
/// Search test data record
/// </summary>
public record SearchTestData(string Query, string ExpectedKeyword)
{
    public override string ToString() => $"Query: {Query}";
}

/// <summary>
/// Navigation test data record
/// </summary>
public record NavigationTestData(string Path, string ExpectedTitleKeyword)
{
    public override string ToString() => $"Path: {Path}";
}
