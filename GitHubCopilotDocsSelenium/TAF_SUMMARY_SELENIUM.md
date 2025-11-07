# Test Automation Framework (TAF) - Selenium Implementation

> **Status:** ✅ **PRODUCTION READY** | **Version:** 1.0.0 | **Last Updated:** November 5, 2025

> **Note:** This is the **Selenium WebDriver implementation** of the dual-framework solution. A parallel **Playwright implementation** is available in `../GitHubCopilotDocsPlaywright/`. See the [main README](../README.md) for solution-level documentation.

## 📋 Table of Contents
1. [Overview](#overview)
2. [Framework Architecture](#framework-architecture)
3. [Technology Stack](#technology-stack)
4. [Quick Start](#quick-start)
5. [Project Structure](#project-structure)
6. [Configuration](#configuration)
7. [Test Examples](#test-examples)
8. [Test Coverage](#test-coverage)
9. [Key Features](#key-features)
10. [Best Practices](#best-practices)
11. [Comparison with Playwright](#comparison-with-playwright)
12. [Troubleshooting](#troubleshooting)
13. [Contributing](#contributing)
14. [Related Documentation](#related-documentation)

---

## Overview

This is a **production-ready .NET 9 Selenium WebDriver NUnit test automation framework** for testing the GitHub Copilot documentation website. The framework follows industry best practices, SOLID principles, and modern C# patterns.

### Part of Dual-Framework Solution

This Selenium implementation is one of **two complete frameworks** in the TestAutomationAIGenExample solution:
- **GitHubCopilotDocsSelenium** (this project) - Selenium WebDriver-based framework
- **GitHubCopilotDocsPlaywright** - Playwright-based framework

Both frameworks:
- Share identical test coverage (16+ tests)
- Follow the same architectural patterns
- Use the same configuration approach
- Enable direct framework comparison

### Key Features
- ✅ **Layered Architecture**: Tests → Pages (POM) → Core (Infrastructure) → Driver (Selenium) → Browser
- ✅ **Auto-Diagnostics**: Automatic screenshot and HTML capture on test failures
- ✅ **Multi-Browser Support**: Chrome, Firefox, Edge, Safari
- ✅ **WebDriverManager**: Automatic browser driver management
- ✅ **Comprehensive Logging**: Structured, thread-safe logging with multiple levels
- ✅ **Data-Driven Testing**: Built-in support for parameterized tests
- ✅ **Modern .NET 9**: Nullable types, required properties, async/await patterns

## Technology Stack

| Technology | Version | Purpose |
|-----------|---------|---------|
| **.NET** | 9.0 | Target framework |
| **C#** | 13.0 | Programming language |
| **Selenium WebDriver** | 4.27.0 | Browser automation |
| **NUnit** | 4.2.2 | Test framework |
| **FluentAssertions** | 8.8.0 | Readable assertions |
| **WebDriverManager** | 2.17.4 | Automatic driver management |
| **Microsoft.Extensions.Configuration** | 9.0.10 | Configuration management |

## Quick Start

### Prerequisites
- .NET 9 SDK installed
- Chrome, Firefox, or Edge browser installed

### Setup & Run Tests

```powershell
# 1. Navigate to the Selenium project folder
cd GitHubCopilotDocsSelenium

# 2. Restore dependencies
dotnet restore

# 3. Build the project
dotnet build

# 4. Run all tests (WebDriverManager will auto-download drivers)
dotnet test

# 5. Run specific test category
dotnet test --filter Category=Smoke

# 6. Run with verbose output
dotnet test --logger "console;verbosity=detailed"
```

## Project Structure

```
GitHubCopilotDocsSelenium/
├── Core/
│   ├── Configuration/
│   │   ├── TestSettings.cs        # Strongly-typed configuration classes
│   │   └── ConfigLoader.cs        # Singleton configuration loader
│   ├── Driver/
│   │   └── SeleniumDriverManager.cs  # WebDriver lifecycle manager
│   ├── Logging/
│   │   └── TestLogger.cs          # Thread-safe structured logger
│   └── BaseTest.cs                # Base class for all tests
├── Pages/
│   ├── BasePage.cs                # Common page utilities (20+ methods)
│   └── GitHub/
│       └── CopilotDocsHomePage.cs # GitHub Copilot Docs page object
├── E2E/
│   ├── CopilotDocsHomePageTests.cs  # 9 E2E smoke tests
│   └── DataDriven/
│       └── CopilotDocsDataDrivenTests.cs  # 7 data-driven tests
├── Data/
│   └── TestDataProvider.cs       # Centralized test data
├── appsettings.json              # Default configuration
├── appsettings.Development.json  # Development overrides
└── GitHubCopilotDocs.Selenium.Tests.csproj
```

## Framework Architecture

### Layered Design
```
┌─────────────────────────────────────────┐
│   Tests (AAA Pattern)                   │
│   - E2E Tests                           │
│   - Data-Driven Tests                   │
└──────────────┬──────────────────────────┘
               │
┌──────────────▼──────────────────────────┐
│   Pages (Page Object Model)             │
│   - BasePage (20+ utilities)            │
│   - CopilotDocsHomePage                 │
└──────────────┬──────────────────────────┘
               │
┌──────────────▼──────────────────────────┐
│   Core (Infrastructure)                 │
│   - Configuration (Settings, Loader)    │
│   - Driver (SeleniumDriverManager)      │
│   - Logging (TestLogger)                │
│   - BaseTest (Setup/Teardown)           │
└──────────────┬──────────────────────────┘
               │
┌──────────────▼──────────────────────────┐
│   Driver (Selenium WebDriver)           │
│   - Browser Automation                  │
│   - Element Interactions                │
└──────────────┬──────────────────────────┘
               │
┌──────────────▼──────────────────────────┐
│   Browser (Chrome/Firefox/Edge/Safari)  │
└─────────────────────────────────────────┘
```

## Configuration

### Configuration Hierarchy (Priority: High → Low)
1. **Environment Variables** (prefix: `TEST_`)
2. **appsettings.{ENV}.json** (e.g., `appsettings.Development.json`)
3. **appsettings.json**

### Default Configuration (appsettings.json)
```json
{
  "BaseUrl": "https://docs.github.com",
  "Browser": {
    "Type": "chrome",
    "Headless": true,
    "ViewportWidth": 1920,
    "ViewportHeight": 1080,
    "ScreenshotOnFailure": true,
    "Maximize": false,
    "AcceptInsecureCerts": false
  },
  "Execution": {
    "DefaultTimeout": 30000,
    "PageLoadTimeout": 60000,
    "MaxRetries": 3,
    "RetryDelay": 1000,
    "Parallel": true,
    "Workers": 0
  },
  "Logging": {
    "MinimumLevel": "Information",
    "OutputPath": "TestResults/Logs",
    "Console": true,
    "File": true
  }
}
```

### Configuration Examples

**Switch Browser:**
```powershell
$env:TEST_Browser__Type="firefox"; dotnet test
$env:TEST_Browser__Type="edge"; dotnet test
```

**Run in Headed Mode:**
```powershell
$env:TEST_Browser__Headless="false"; dotnet test
```

**Use Development Environment:**
```powershell
$env:TEST_ENV="Development"; dotnet test
```

## Common Commands

```powershell
# Build solution
dotnet build

# Run all tests
dotnet test

# Run specific test category
dotnet test --filter Category=Smoke
dotnet test --filter Category=E2E
dotnet test --filter Category=DataDriven

# Run tests in different browsers
$env:TEST_Browser__Type="firefox"; dotnet test
$env:TEST_Browser__Type="edge"; dotnet test

# Run tests in headed mode (see browser)
$env:TEST_Browser__Headless="false"; dotnet test

# Run with custom timeout
$env:TEST_Execution__DefaultTimeout="60000"; dotnet test

# Clean and rebuild
dotnet clean; dotnet build

# Add new NuGet package
dotnet add package <PackageName>
```

## Test Examples

### E2E Test (AAA Pattern)
```csharp
[Test]
[Category("Smoke")]
[Category("E2E")]
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
    heading.Should().NotBeNullOrWhiteSpace();
}
```

### Data-Driven Test
```csharp
[Test]
[TestCase("/en/copilot", "Copilot")]
[TestCase("/en/copilot/quickstart", "Quick")]
[TestCase("/en/copilot/overview", "Overview")]
public void PageTitle_NavigateToDifferentPages_TitleContainsExpectedKeyword(
    string path, string expectedKeyword)
{
    // Arrange
    LogStep($"Navigate to: {path}");

    // Act
    NavigateTo(path);
    _homePage!.WaitForPageLoad();
    var title = _homePage.GetTitle();

    // Assert
    title.Should().Contain(expectedKeyword);
}
```

## Test Coverage

| Category | Count | Description |
|----------|-------|-------------|
| **E2E Smoke Tests** | 9 | Basic page functionality tests |
| **Data-Driven Tests** | 7 | Parameterized test scenarios |
| **Total Test Cases** | 16+ | Including parameterized variations |

### Test List
1. PageLoad_NavigateToCopilotDocs_PageLoadsWithMainHeading
2. Navigation_CheckNavigationBar_NavigationIsVisible
3. Search_CheckSearchInput_SearchIsEnabled
4. ArticleLinks_CountArticleLinks_ShouldHaveMultipleLinks
5. Breadcrumb_CheckBreadcrumb_BreadcrumbIsVisible
6. Sidebar_CheckSidebar_SidebarIsVisible
7. Footer_CheckFooter_FooterIsVisible
8. PageTitle_GetPageTitle_ShouldContainCopilot
9. URL_NavigateToCopilotDocs_URLIsCorrect
10. Navigation_NavigateToPath_PageLoadsSuccessfully (Data-Driven)
11. PageTitle_NavigateToDifferentPages_TitleContainsExpectedKeyword (Data-Driven)
12. Navigation_VisitMultiplePages_AllPagesLoadSuccessfully (Data-Driven)
13. Search_SearchForDifferentTerms_SearchIsPerformed (Data-Driven)
14. Responsive_TestDifferentViewports_PageLoadsCorrectly (Data-Driven)

## Key Features

### Auto-Diagnostics on Failure
When a test fails, the framework automatically captures:
- **Screenshot** → `TestResults/Screenshots/{TestName}_{Timestamp}.png`
- **Page HTML** → `TestResults/Screenshots/{TestName}_{Timestamp}.html`
- All artifacts attached to NUnit test results

### WebDriverManager Integration
- Automatically downloads and manages browser drivers
- No manual driver installation required
- Supports Chrome, Firefox, Edge

### Page Object Model
**BasePage** provides 20+ utility methods:

**Element Interaction:**
- `Click(selector)` - Click element
- `Type(selector, text)` - Fill input field
- `PressKey(selector, key)` - Press keyboard key
- `Hover(selector)` - Hover over element
- `ScrollIntoView(selector)` - Scroll element into view

**Element State:**
- `IsVisible(selector)` - Check visibility
- `IsEnabled(selector)` - Check if enabled
- `GetText(selector)` - Get text content
- `GetAttribute(selector, name)` - Get attribute value
- `GetCount(selector)` - Count matching elements

**Waiting:**
- `WaitForVisible(selector, timeout?)` - Wait for element
- `WaitForClickable(selector, timeout?)` - Wait for clickable
- `WaitForHidden(selector, timeout?)` - Wait for hidden

**Form Controls:**
- `SelectOption(selector, value)` - Select dropdown option
- `Check(selector)` - Check checkbox/radio
- `Uncheck(selector)` - Uncheck checkbox

**Page Information:**
- `GetCurrentUrl()` - Get current URL
- `GetTitle()` - Get page title
- `TakeScreenshot()` - Take screenshot

## Best Practices

### Test Naming Convention
```
MethodName_StateUnderTest_ExpectedBehavior
```

### Test Structure (AAA Pattern)
```csharp
[Test]
public void Example()
{
    // Arrange
    LogStep("Setup description");
    
    // Act
    await _page.DoSomethingAsync();
    
    // Assert
    result.Should().Be(expected);
}
```

### Locator Strategy (Priority Order)
1. **data-test-id attributes** (most stable)
2. **Semantic HTML** with text
3. **ARIA labels**
4. **Stable CSS selectors**
5. ❌ Avoid: Index-based XPath, brittle CSS

## Comparison: Selenium vs Playwright

### Selenium Advantages (This Framework)

**Maturity & Ecosystem:**
- ✅ **Industry Standard**: Proven in enterprise environments for 15+ years
- ✅ **Wide Browser Support**: Chrome, Firefox, Edge, Safari (actual browser, not engine)
- ✅ **Extensive Community**: Large community, abundant resources, and third-party tools
- ✅ **Tool Integration**: Seamless integration with existing Selenium infrastructure

**Team & Skills:**
- ✅ **Known Technology**: Familiar to most QA engineers
- ✅ **Transfer Skills**: Easy onboarding for teams with Selenium background
- ✅ **Legacy Support**: Works with older browser versions

**Control & Flexibility:**
- ✅ **Explicit Waits**: Fine-grained control over wait strategies
- ✅ **WebDriverManager**: Automatic driver management (no manual downloads)
- ✅ **Familiar Patterns**: Standard Page Object Model implementations

### When to Use Playwright Instead

- ✅ Need modern auto-waiting capabilities
- ✅ Want advanced debugging with trace viewer
- ✅ Require network interception/mocking
- ✅ Testing on WebKit (Safari engine) is acceptable
- ✅ Starting a greenfield project
- ✅ Need better performance and speed

### Side-by-Side Code Comparison

**Element Interaction:**
```csharp
// Selenium (Explicit Wait + Action)
var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
var element = wait.Until(ExpectedConditions.ElementToBeClickable(
    By.CssSelector("[data-test-id='submit']")));
element.Click();

// Playwright (Auto-waiting)
await page.ClickAsync("[data-test-id='submit']");
```

**Page Navigation:**
```csharp
// Selenium
driver.Navigate().GoToUrl("https://example.com");
var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
wait.Until(driver => ((IJavaScriptExecutor)driver)
    .ExecuteScript("return document.readyState").Equals("complete"));

// Playwright
await page.GotoAsync("https://example.com");
await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
```

**Taking Screenshots:**
```csharp
// Selenium
var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
screenshot.SaveAsFile("screenshot.png");

// Playwright
await page.ScreenshotAsync(new() { Path = "screenshot.png", FullPage = true });
```

**Test Structure:**
```csharp
// Selenium (Synchronous)
[Test]
public void MyTest()
{
    NavigateTo("/page");
    _page.WaitForPageLoad();
    var result = _page.GetResult();
    result.Should().Be("expected");
}

// Playwright (Asynchronous)
[Test]
public async Task MyTest()
{
    await NavigateToAsync("/page");
    await _page.WaitForPageLoadAsync();
    var result = await _page.GetResultAsync();
    result.Should().Be("expected");
}
```

### Migration Path

The identical architecture makes it easy to migrate between frameworks:
1. Both use same Page Object Model structure
2. Same configuration approach (appsettings.json + environment variables)
3. Same test organization (AAA pattern, categories, naming conventions)
4. Main difference: `async/await` vs synchronous methods

For detailed comparison, see `../docs/ADR-001-Playwright-vs-Selenium.md`

---

## Troubleshooting

### Common Issues

**Issue: WebDriver executable not found**
- **Solution**: WebDriverManager should auto-download. If it fails, check internet connection.

**Issue: Tests timeout**
- **Solution**: Increase timeout via `$env:TEST_Execution__DefaultTimeout="60000"`

**Issue: Configuration not loading**
- **Solution**: Ensure `appsettings.json` has `CopyToOutputDirectory=PreserveNewest` in `.csproj`

**Issue: Browser doesn't start**
- **Solution**: Ensure browser is installed. Try headed mode: `$env:TEST_Browser__Headless="false"`

## Contributing

### Adding New Tests
1. Inherit from `BaseTest`
2. Follow AAA pattern
3. Use `LogStep()` for readability
4. Add appropriate `[Category]` attributes
5. Run `dotnet build` before committing

### Adding New Page Objects
1. Inherit from `BasePage`
2. Define locators as constants
3. Create public methods for page interactions
4. Use descriptive method names
5. Add XML documentation comments

## License

This project is part of the TestAutomationAIGenExample repository.

## Related Documentation

- **Main README**: `../README.md`
- **Playwright Project**: `../GitHubCopilotDocsPlaywright/`
- **Troubleshooting Guide**: `../docs/TROUBLESHOOTING.md`
- **ADR - Playwright vs Selenium**: `../docs/ADR-001-Playwright-vs-Selenium.md`
- **ADR - NUnit Framework**: `../docs/ADR-002-NUnit-Test-Framework.md`

---

**Framework Version:** 1.0.0  
**Created:** November 5, 2025  
**.NET Version:** 9.0  
**Selenium WebDriver Version:** 4.27.0  
**NUnit Version:** 4.2.2  

**Status:** ✅ Production Ready
