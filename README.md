# Test Automation Framework - GitHub Copilot Documentation

A production-grade .NET test automation solution featuring **dual framework implementations** (Playwright and Selenium WebDriver) with NUnit for testing the [GitHub Copilot Documentation](https://docs.github.com/en/copilot) website.

## 🎭 Dual Framework Architecture

This solution provides **two complete test automation frameworks** for comprehensive comparison and flexibility:

| Framework | Project | Version | Status |
|-----------|---------|---------|--------|
| **Playwright** | GitHubCopilotDocsPlaywright | 1.55.0 | ✅ Production Ready |
| **Selenium** | GitHubCopilotDocsSelenium | 4.27.0 | ✅ Production Ready |

Both frameworks share identical test coverage, architecture patterns, and follow the same coding standards, enabling direct comparison and team flexibility.

## 🎯 Framework Features

### Common Features (Both Projects)
✅ **NUnit 4.2.2** - Industry-standard test framework with FluentAssertions  
✅ **Page Object Model** - Maintainable, scalable test architecture  
✅ **Layered Architecture** - Clear separation of concerns (Tests → Pages → Core → Driver)  
✅ **Configuration Management** - Hierarchical config (appsettings → environment variables → CLI)  
✅ **Structured Logging** - Console + file logging with contextual information  
✅ **Automatic Diagnostics** - Screenshots and HTML on test failure  
✅ **Retry Mechanism** - Built-in retry logic with exponential backoff  
✅ **Data-Driven Testing** - Support for parameterized tests with external data  
✅ **Modern .NET 9** - Nullable reference types, required properties, collection expressions

### Playwright-Specific Features
✅ **Auto-Waiting** - Built-in smart waiting for elements  
✅ **Trace Viewer** - Advanced debugging with timeline and network inspection  
✅ **Multi-Browser** - Chromium, Firefox, WebKit  
✅ **Network Interception** - Mock and modify network requests  
✅ **CI/CD Ready** - GitHub Actions workflow with multi-browser matrix  

### Selenium-Specific Features
✅ **WebDriverManager** - Automatic browser driver management  
✅ **Wide Browser Support** - Chrome, Firefox, Edge, Safari  
✅ **Mature Ecosystem** - Extensive community and third-party tools  
✅ **Explicit Waits** - Fine-grained control with WebDriverWait  
✅ **Industry Standard** - Proven in enterprise environments

## 📁 Solution Structure

```
TestAutomationAIGenExample/
├── GitHubCopilotDocsPlaywright/          # Playwright Framework
│   ├── Core/
│   │   ├── Configuration/
│   │   │   ├── ConfigLoader.cs           # Loads hierarchical configuration
│   │   │   └── TestSettings.cs           # Strongly-typed settings
│   │   ├── Driver/
│   │   │   └── PlaywrightDriverManager.cs  # Browser lifecycle management
│   │   ├── Logging/
│   │   │   └── TestLogger.cs             # Structured logger
│   │   └── BaseTest.cs                   # Base class for all tests
│   ├── Pages/
│   │   ├── BasePage.cs                   # Base page with common utilities
│   │   └── GitHub/
│   │       └── CopilotDocsHomePage.cs    # Page Object for Copilot docs
│   ├── E2E/
│   │   ├── CopilotDocsHomePageTests.cs   # 9 E2E smoke tests
│   │   └── DataDriven/
│   │       └── CopilotDocsDataDrivenTests.cs  # Parameterized tests
│   ├── Data/
│   │   └── TestDataProvider.cs           # Test data sources
│   ├── appsettings.json                  # Default configuration
│   ├── appsettings.Development.json      # Development overrides
│   ├── README.md                         # Playwright project documentation
│   ├── TAF_SUMMARY_PLAYWRIGHT.md         # Complete framework summary
│   └── GitHubCopilotDocs.Tests.csproj    # Project file
│
├── GitHubCopilotDocsSelenium/            # Selenium Framework
│   ├── Core/
│   │   ├── Configuration/
│   │   │   ├── ConfigLoader.cs           # Loads hierarchical configuration
│   │   │   └── TestSettings.cs           # Strongly-typed settings
│   │   ├── Driver/
│   │   │   └── SeleniumDriverManager.cs  # WebDriver lifecycle management
│   │   ├── Logging/
│   │   │   └── TestLogger.cs             # Structured logger
│   │   └── BaseTest.cs                   # Base class for all tests
│   ├── Pages/
│   │   ├── BasePage.cs                   # Base page with common utilities
│   │   └── GitHub/
│   │       └── CopilotDocsHomePage.cs    # Page Object for Copilot docs
│   ├── E2E/
│   │   ├── CopilotDocsHomePageTests.cs   # 9 E2E smoke tests
│   │   └── DataDriven/
│   │       └── CopilotDocsDataDrivenTests.cs  # Parameterized tests
│   ├── Data/
│   │   └── TestDataProvider.cs           # Test data sources
│   ├── appsettings.json                  # Default configuration
│   ├── appsettings.Development.json      # Development overrides
│   ├── README.md                         # Selenium project documentation
│   ├── TAF_SUMMARY_SELENIUM.md           # Complete framework summary
│   └── GitHubCopilotDocs.Selenium.Tests.csproj  # Project file
│
├── docs/
│   ├── ADR-001-Playwright-vs-Selenium.md  # Architecture decision record
│   ├── ADR-002-NUnit-Test-Framework.md    # Test framework selection
│   └── TROUBLESHOOTING.md                 # Comprehensive troubleshooting guide
│
├── .github/
│   ├── workflows/
│   │   └── test-automation-ci.yml        # CI/CD pipeline (Playwright)
│   ├── copilot-instructions.md           # AI agent guidance
│   └── instructions/                     # Additional AI instructions
│
├── TestAutomationAIGenExample.sln        # Visual Studio solution file
├── README.md                             # This file - Solution overview
├── AGENTS.md                             # AI agent capabilities
└── .gitignore                            # Git ignore patterns
```

## 🚀 Quick Start

### Prerequisites

- **.NET 9 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/9.0)
- **PowerShell** (for Windows) or **Bash** (for Linux/Mac)
- **Git**
- **Browser** - Chrome, Firefox, or Edge

### Installation

```powershell
# Clone repository
git clone https://github.com/ChiefDojer/TestAutomationAIGenExample.git
cd TestAutomationAIGenExample

# Restore dependencies for entire solution
dotnet restore

# Build entire solution
dotnet build
```

### Framework-Specific Setup

#### Playwright Framework
```powershell
# Install Playwright browsers (one-time setup)
powershell -File GitHubCopilotDocsPlaywright\bin\Debug\net9.0\playwright.ps1 install chromium

# Run Playwright tests
dotnet test GitHubCopilotDocsPlaywright/GitHubCopilotDocs.Tests.csproj
```

#### Selenium Framework
```powershell
# No browser installation needed - WebDriverManager handles it automatically

# Run Selenium tests
dotnet test GitHubCopilotDocsSelenium/GitHubCopilotDocs.Selenium.Tests.csproj
```

### Running Tests

```powershell
# Run ALL tests (both frameworks)
dotnet test

# Run specific framework
dotnet test GitHubCopilotDocsPlaywright/GitHubCopilotDocs.Tests.csproj
dotnet test GitHubCopilotDocsSelenium/GitHubCopilotDocs.Selenium.Tests.csproj

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run specific category
dotnet test --filter Category=Smoke
dotnet test --filter Category=E2E
dotnet test --filter Category=DataDriven

# Run in headed mode (see browser)
$env:TEST_Browser__Headless="false"; dotnet test

# Run with different browser (Playwright)
$env:TEST_Browser__Type="firefox"; dotnet test GitHubCopilotDocsPlaywright/
$env:TEST_Browser__Type="webkit"; dotnet test GitHubCopilotDocsPlaywright/

# Run with different browser (Selenium)
$env:TEST_Browser__Type="firefox"; dotnet test GitHubCopilotDocsSelenium/
$env:TEST_Browser__Type="edge"; dotnet test GitHubCopilotDocsSelenium/
```

### Configuration

Both frameworks use identical configuration structure (precedence: CLI > Env Vars > appsettings.{ENV}.json > appsettings.json):

**appsettings.json** (Common Structure):
```json
{
  "BaseUrl": "https://docs.github.com",
  "Browser": {
    "Type": "chromium",  // Playwright: chromium/firefox/webkit | Selenium: chrome/firefox/edge/safari
    "Headless": true,
    "ViewportWidth": 1920,
    "ViewportHeight": 1080,
    "ScreenshotOnFailure": true
  },
  "Execution": {
    "DefaultTimeout": 30000,
    "MaxRetries": 3,
    "Parallel": true
  },
  "Logging": {
    "MinimumLevel": "Information",
    "Console": true,
    "File": true
  }
}
```

**Environment Variables** (prefix with `TEST_`):
```powershell
# Common settings
$env:TEST_Browser__Headless="false"
$env:TEST_Execution__MaxRetries="5"
$env:TEST_Logging__MinimumLevel="Debug"

# Framework-specific browser types
$env:TEST_Browser__Type="firefox"  # Playwright
$env:TEST_Browser__Type="edge"     # Selenium
```

## 📝 Writing Tests

Tests follow identical patterns in both frameworks, only differing in async/await usage:

### Playwright Test Example (Async)

```csharp
[Test]
public async Task PageLoad_NavigateToCopilotDocs_PageLoadsWithMainHeading()
{
    // Arrange
    LogStep("Navigate to GitHub Copilot documentation");

    // Act
    await NavigateToAsync("/en/copilot");
    await _homePage!.WaitForPageLoadAsync();
    var headingText = await _homePage.GetMainHeadingAsync();

    // Assert
    LogStep("Verify page loaded with correct heading");
    headingText.Should().NotBeNullOrWhiteSpace();
}
```

### Selenium Test Example (Sync)

```csharp
[Test]
public void PageLoad_NavigateToCopilotDocs_PageLoadsWithMainHeading()
{
    // Arrange
    LogStep("Navigate to GitHub Copilot documentation");

    // Act
    NavigateTo("/en/copilot");
    _homePage!.WaitForPageLoad();
    var headingText = _homePage.GetMainHeading();

    // Assert
    LogStep("Verify page loaded with correct heading");
    headingText.Should().NotBeNullOrWhiteSpace();
}
```

### Data-Driven Test Example

```csharp
[Test]
[TestCase("/en/copilot", "Copilot")]
[TestCase("/en/copilot/quickstart", "Quick")]
[TestCase("/en/copilot/overview", "Overview")]
public async Task PageTitle_NavigateToDifferentPages_TitleContainsExpectedKeyword(
    string path, string expectedKeyword)
{
    // Arrange & Act
    await NavigateToAsync(path);  // Playwright: async | Selenium: sync
    await _homePage!.WaitForPageLoadAsync();
    var title = await _homePage.GetTitleAsync();

    // Assert
    title.Should().Contain(expectedKeyword);
}
```

### Creating New Page Objects

Both frameworks use identical patterns:

```csharp
// Playwright
public class MyPage : BasePage
{
    private const string ButtonSelector = "[data-testid='submit-button']";

    public MyPage(IPage page, TestLogger logger) : base(page, logger) { }

    public async Task ClickSubmitAsync()
    {
        Logger.Debug("Clicking submit button");
        await ClickAsync(ButtonSelector);
    }
}

// Selenium
public class MyPage : BasePage
{
    private static readonly By ButtonSelector = By.CssSelector("[data-testid='submit-button']");

    public MyPage(IWebDriver driver, WebDriverWait wait, TestLogger logger) 
        : base(driver, wait, logger) { }

    public void ClickSubmit()
    {
        Logger.Debug("Clicking submit button");
        Click(ButtonSelector);
    }
}
```

## 🧪 Test Organization

### Categories
- **E2E** - End-to-end scenarios
- **Smoke** - Critical path tests
- **DataDriven** - Parameterized tests
- **Regression** - Full regression suite

### Naming Convention
```
MethodName_StateUnderTest_ExpectedBehavior
```

Examples:
- `PageLoad_NavigateToCopilotDocs_PageLoadsWithMainHeading`
- `Navigation_VerifyUrl_UrlMatchesExpectedPath`
- `ContentVerification_CheckArticleLinks_LinksArePresent`

## 📊 Test Results

Both frameworks generate similar diagnostic outputs:

### Playwright Results
- **Logs**: `GitHubCopilotDocsPlaywright/TestResults/Logs/`
- **Screenshots**: `GitHubCopilotDocsPlaywright/TestResults/Screenshots/` (on failure)
- **Traces**: `GitHubCopilotDocsPlaywright/TestResults/Traces/` (Playwright trace viewer)
- **Videos**: `GitHubCopilotDocsPlaywright/TestResults/Videos/` (if enabled)

### Selenium Results
- **Logs**: `GitHubCopilotDocsSelenium/TestResults/Logs/`
- **Screenshots**: `GitHubCopilotDocsSelenium/TestResults/Screenshots/` (on failure)
- **HTML Snapshots**: `GitHubCopilotDocsSelenium/TestResults/Screenshots/` (page source)

### Viewing Playwright Traces

```powershell
playwright show-trace TestResults/Traces/TestName_2024-11-05_10-30-00.zip
```

## 🔧 Troubleshooting

### Common Issues

**Playwright - Browser not found**:
```powershell
& GitHubCopilotDocsPlaywright\bin\Debug\net9.0\playwright.ps1 install
```

**Selenium - WebDriver not found**:
- WebDriverManager should auto-download drivers
- Check internet connection if download fails
- Verify browser is installed

**Configuration not loading**:
- Verify `appsettings.json` is in project directory
- Check file properties: "Copy to Output Directory" = "PreserveNewest"
- Validate JSON syntax

**Tests fail with timeout**:
- Increase timeout in `appsettings.json`: `"DefaultTimeout": 60000`
- Or via environment: `$env:TEST_Execution__DefaultTimeout="60000"`

**Headless mode issues**:
- Run with headed browser for debugging: `$env:TEST_Browser__Headless="false"`

**Build errors**:
```powershell
# Clean and rebuild
dotnet clean
dotnet build
```

For comprehensive troubleshooting, see `docs/TROUBLESHOOTING.md`

## 🏗️ Architecture Principles

### Common Architecture (Both Frameworks)

Both frameworks follow identical architectural principles:

### SOLID Compliance
- **Single Responsibility**: Each class has one reason to change
- **Open/Closed**: Extensible via inheritance, closed for modification
- **Liskov Substitution**: Page Objects are interchangeable with BasePage
- **Interface Segregation**: Minimal, focused interfaces
- **Dependency Inversion**: Depend on abstractions (IPage/IWebDriver, TestLogger)

### Design Patterns
- **Page Object Model**: UI abstraction layer
- **Factory Pattern**: Driver creation and management
- **Singleton**: Configuration loader
- **Template Method**: BaseTest lifecycle hooks
- **Strategy Pattern**: Retry policies, browser selection
- **IAsyncDisposable/IDisposable**: Proper resource cleanup

### Layered Architecture
```
Tests (NUnit + FluentAssertions)
    ↓
Pages (Page Object Model)
    ↓
Core (Infrastructure: Config, Logging, BaseTest)
    ↓
Driver (Playwright IPage / Selenium IWebDriver)
    ↓
Browser (Chromium/Firefox/WebKit/Chrome/Edge/Safari)
```

## 🔐 Security & Compliance

- ✅ No hardcoded credentials
- ✅ Secrets via environment variables or User Secrets
- ✅ PII masking in logs
- ✅ Input sanitization for test data
- ✅ HTTPS-only connections

## 📈 CI/CD Integration

### GitHub Actions

The solution includes CI/CD pipelines for both frameworks:

#### Playwright Pipeline (`.github/workflows/test-automation-ci.yml`)
- ✅ Multi-browser testing (Chromium, Firefox, WebKit)
- ✅ Parallel execution
- ✅ Automatic artifact upload (results, screenshots, traces)
- ✅ Test result reporting
- ✅ 30-day retention

#### Selenium Pipeline (To be configured)
- Use similar structure for Selenium tests
- WebDriverManager handles browser driver setup
- Multi-browser matrix support

Trigger manually:
```
GitHub Actions → test-automation-ci → Run workflow
```

## 🔀 Choosing Between Frameworks

### Use Playwright When:
✅ You need modern, fast browser automation  
✅ Advanced debugging with trace viewer is important  
✅ Network interception/mocking is required  
✅ Auto-waiting and smart element detection are priorities  
✅ Testing on WebKit (Safari engine) is needed  
✅ Starting a new project with no legacy constraints  

### Use Selenium When:
✅ Team has existing Selenium expertise  
✅ Integration with legacy test infrastructure  
✅ Testing on actual Safari browser is required  
✅ Wide third-party tool ecosystem is needed  
✅ Industry standard proven in enterprise environments  
✅ Fine-grained control over explicit waits is important  

### Why Both?
- **Direct Comparison**: Evaluate frameworks side-by-side with identical tests
- **Team Flexibility**: Support different team preferences and skills
- **Migration Path**: Gradual transition between frameworks
- **Best Tool for Job**: Use optimal framework for specific scenarios
- **Learning Resource**: Understand differences through practical examples

For detailed comparison, see `docs/ADR-001-Playwright-vs-Selenium.md`

## 🤝 Contributing

### General Guidelines
1. Follow the AAA pattern for all tests
2. Use FluentAssertions for readable assertions
3. Add XML documentation to public methods
4. Keep test methods under 30 lines
5. Use `LogStep()` for test readability
6. Ensure tests are independent and idempotent
7. Run `dotnet build` before committing

### Framework-Specific Guidelines

**Playwright:**
- Use `async`/`await` for all page interactions
- Leverage auto-waiting capabilities
- Avoid explicit `Thread.Sleep()` or manual waits

**Selenium:**
- Use explicit waits (`WebDriverWait`) over implicit waits
- Prefer `By` locators over string selectors
- Use WebDriverManager for driver management

## 📚 Resources

### Solution Documentation
- **[README.md](README.md)** - This file - Solution overview
- **[AGENTS.md](AGENTS.md)** - AI agent capabilities and usage

### Playwright Framework Documentation
- **[GitHubCopilotDocsPlaywright/README.md](GitHubCopilotDocsPlaywright/README.md)** - Playwright project guide
- **[GitHubCopilotDocsPlaywright/TAF_SUMMARY_PLAYWRIGHT.md](GitHubCopilotDocsPlaywright/TAF_SUMMARY_PLAYWRIGHT.md)** - Complete Playwright framework summary

### Selenium Framework Documentation
- **[GitHubCopilotDocsSelenium/README.md](GitHubCopilotDocsSelenium/README.md)** - Selenium project guide
- **[GitHubCopilotDocsSelenium/TAF_SUMMARY_SELENIUM.md](GitHubCopilotDocsSelenium/TAF_SUMMARY_SELENIUM.md)** - Complete Selenium framework summary

### Shared Documentation
- **[docs/ADR-001-Playwright-vs-Selenium.md](docs/ADR-001-Playwright-vs-Selenium.md)** - Framework comparison and selection rationale
- **[docs/ADR-002-NUnit-Test-Framework.md](docs/ADR-002-NUnit-Test-Framework.md)** - Why NUnit was chosen
- **[docs/TROUBLESHOOTING.md](docs/TROUBLESHOOTING.md)** - Comprehensive troubleshooting guide
- **[.github/copilot-instructions.md](.github/copilot-instructions.md)** - AI agent guidance

### External Resources
- [Playwright .NET Docs](https://playwright.dev/dotnet/)
- [Selenium WebDriver Docs](https://www.selenium.dev/documentation/)
- [NUnit Documentation](https://docs.nunit.org/)
- [FluentAssertions](https://fluentassertions.com/)
- [GitHub Copilot Docs](https://docs.github.com/en/copilot)

## 📄 License

This project is provided as a reference implementation for educational purposes.

## 📞 Support

For issues or questions:
1. Check framework-specific README files
2. Review `docs/TROUBLESHOOTING.md`
3. Review existing GitHub Issues
4. Create a new issue with detailed reproduction steps

---

## 📊 Solution Statistics

| Metric | Playwright | Selenium | Total |
|--------|------------|----------|-------|
| **Lines of Code** | ~2,500 | ~1,300 | ~3,800 |
| **Test Files** | 2 | 2 | 4 |
| **E2E Tests** | 9 | 9 | 18 |
| **Data-Driven Tests** | 7 | 7 | 14 |
| **Page Objects** | 2 | 2 | 4 |
| **Core Classes** | 6 | 5 | 11 |

**Built with ❤️ using .NET 9, Playwright, Selenium, and modern test automation practices**

---

**Solution Version:** 1.0.0  
**Last Updated:** November 5, 2025  
**.NET Version:** 9.0  
**Playwright Version:** 1.55.0  
**Selenium WebDriver Version:** 4.27.0  
**NUnit Version:** 4.2.2  

**Status:** ✅ Both Frameworks Production Ready
