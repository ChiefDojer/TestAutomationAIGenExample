# Test Automation Solution - AI Agent Instructions

## Project State & Architecture
This is a **production-ready .NET 9 dual-framework test automation solution** with **both Playwright and Selenium** implementations for testing the GitHub Copilot documentation website.

**Critical**: This solution contains **TWO complete frameworks** with identical test coverage:
- `GitHubCopilotDocsPlaywright/` - Playwright-based (async/await)
- `GitHubCopilotDocsSelenium/` - Selenium WebDriver-based (synchronous)

**Technology Stack**:
- .NET 9 with modern C# features (nullable types, required properties, collection expressions)
- Test Framework: NUnit 4.2.2 with FluentAssertions
- Automation: Playwright 1.55.0 **AND** Selenium WebDriver 4.27.0
- Target: https://docs.github.com/en/copilot

**Shared Architecture** (Both Frameworks):
```
Tests → Pages (POM) → Core (Infrastructure) → Driver (Playwright/Selenium) → Browser
```

## Quick Start

### Build Commands (PowerShell)
```powershell
# Build entire solution (both frameworks)
dotnet build

# Run ALL tests (both Playwright and Selenium)
dotnet test

# Run specific framework
dotnet test GitHubCopilotDocsPlaywright/GitHubCopilotDocs.Tests.csproj
dotnet test GitHubCopilotDocsSelenium/GitHubCopilotDocs.Selenium.Tests.csproj

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run specific category (works across both frameworks)
dotnet test --filter Category=Smoke
dotnet test --filter Category=E2E
dotnet test --filter Category=DataDriven

# Run in different browser
# Playwright: chromium/firefox/webkit
$env:TEST_Browser__Type="firefox"; dotnet test GitHubCopilotDocsPlaywright/

# Selenium: chrome/firefox/edge/safari
$env:TEST_Browser__Type="firefox"; dotnet test GitHubCopilotDocsSelenium/
```

**Shell Syntax**: Use `;` for command chaining in PowerShell, NOT `&&`

### First-Time Setup
```powershell
# Clone and build
dotnet restore
dotnet build

# Playwright: Install browsers (one-time)
cd GitHubCopilotDocsPlaywright
pwsh bin/Debug/net9.0/playwright.ps1 install chromium
cd ..

# Selenium: WebDriverManager auto-downloads drivers (no setup needed)
```

## Code Style & Standards

### Test Naming Convention
`MethodName_StateUnderTest_ExpectedBehavior`

Example:
```csharp
PageLoad_NavigateToCopilotDocs_PageLoadsWithMainHeading()
```

### Test Structure (AAA Pattern)
```csharp
// Playwright (async)
[Test]
public async Task Example()
{
    // Arrange
    LogStep("Setup description");
    
    // Act
    await _homePage.DoSomethingAsync();
    
    // Assert
    result.Should().Be(expected);
}

// Selenium (sync)
[Test]
public void Example()
{
    // Arrange
    LogStep("Setup description");
    
    // Act
    _homePage.DoSomething();
    
    // Assert
    result.Should().Be(expected);
}
```

### Framework Patterns
- **All test classes** must inherit from `BaseTest`
- **Always call** `await base.SetUp()` / `await base.TearDown()` (Playwright) or `base.SetUp()` / `base.TearDown()` (Selenium)
- **Use LogStep()** for test readability
- **FluentAssertions** for all assertions
- **Playwright**: async/await required; Selenium: synchronous methods

### .NET Standards
- Nullable reference types enabled
- Use `ArgumentNullException.ThrowIfNull()` for parameter validation
- Async methods accept `CancellationToken` for long operations
- Modern C# features: required properties, collection expressions

## Project Structure
```
TestAutomationAIGenExample.sln
├── GitHubCopilotDocsPlaywright/    # Playwright Framework (async)
│   ├── Core/
│   │   ├── Configuration/          # ConfigLoader, TestSettings
│   │   ├── Driver/                 # PlaywrightDriverManager
│   │   ├── Logging/                # TestLogger
│   │   └── BaseTest.cs             # Base class (async methods)
│   ├── Pages/
│   │   ├── BasePage.cs             # Common page utilities (async)
│   │   └── GitHub/                 # Page Objects
│   ├── E2E/                        # End-to-end tests
│   │   └── DataDriven/             # Parameterized tests
│   ├── Data/                       # TestDataProvider
│   └── appsettings.json            # Configuration
│
├── GitHubCopilotDocsSelenium/      # Selenium Framework (sync)
│   ├── Core/
│   │   ├── Configuration/          # ConfigLoader, TestSettings
│   │   ├── Driver/                 # SeleniumDriverManager
│   │   ├── Logging/                # TestLogger
│   │   └── BaseTest.cs             # Base class (sync methods)
│   ├── Pages/
│   │   ├── BasePage.cs             # Common page utilities (sync)
│   │   └── GitHub/                 # Page Objects
│   ├── E2E/                        # End-to-end tests
│   │   └── DataDriven/             # Parameterized tests
│   ├── Data/                       # TestDataProvider
│   └── appsettings.json            # Configuration
│
├── docs/
│   ├── ADR-001-Playwright-vs-Selenium.md
│   ├── ADR-002-NUnit-Test-Framework.md
│   └── TROUBLESHOOTING.md
│
└── .github/
    ├── workflows/test-automation-ci.yml
    ├── copilot-instructions.md         # GitHub Copilot reference
    └── instructions/
        ├── ai-prompt-engineering.instructions.md
        └── dotnet-framework.instructions.md
```

## Configuration

### Hierarchy (Highest to Lowest Priority)
1. Environment variables (prefix: `TEST_`)
2. `appsettings.{TEST_ENV}.json`
3. `appsettings.json`

Example:
```powershell
$env:TEST_Browser__Type="firefox"
$env:TEST_ENV="Development"
dotnet test
```

## Locator Strategy (Priority Order)
1. `data-test-id` attributes (most stable)
2. Semantic HTML with text: `button:has-text('Submit')` / `By.XPath("//button[contains(text(),'Submit')]")`
3. ARIA labels: `[aria-label='Search']`
4. Stable CSS: `[role='navigation']`
5. **Avoid**: Index-based XPath, brittle CSS

## Adding New Tests

### New Page Object
```csharp
// Playwright (async)
public class MyPage : BasePage
{
    public MyPage(IPage page, TestLogger logger) : base(page, logger) { }
    
    public async Task ClickButtonAsync() => await ClickAsync("[data-test-id='btn']");
}

// Selenium (sync)
public class MyPage : BasePage
{
    public MyPage(IWebDriver driver, WebDriverWait wait, TestLogger logger) 
        : base(driver, wait, logger) { }
    
    public void ClickButton() => Click(By.CssSelector("[data-test-id='btn']"));
}
```

### New Test Class
```csharp
// Playwright (async)
[TestFixture]
[Category("E2E")]
public class MyTests : BaseTest
{
    [SetUp]
    public override async Task SetUp()
    {
        await base.SetUp();  // MUST call base
        _myPage = new MyPage(Page, Logger!);
    }
    
    [Test]
    public async Task MyTest_Condition_ExpectedResult() { /*...*/ }
}

// Selenium (sync)
[TestFixture]
[Category("E2E")]
public class MyTests : BaseTest
{
    [SetUp]
    public override void SetUp()
    {
        base.SetUp();  // MUST call base
        var wait = new WebDriverWait(Driver, TimeSpan.FromMilliseconds(Settings.Execution.DefaultTimeout));
        _myPage = new MyPage(Driver, wait, Logger!);
    }
    
    [Test]
    public void MyTest_Condition_ExpectedResult() { /*...*/ }
}
```

## Common Issues

| Issue | Solution |
|-------|----------|
| **Playwright**: Browser missing | `pwsh bin/Debug/net9.0/playwright.ps1 install chromium` |
| **Selenium**: ChromeDriver version mismatch | WebDriverManager auto-downloads; check Chrome version |
| Config not loading | Verify `appsettings.json` has `CopyToOutputDirectory=PreserveNewest` |
| Tests timeout | Increase `TEST_Execution__DefaultTimeout` env var |
| Browser not closing | Ensure `base.TearDown()` is called |

## Additional Resources
- Full framework comparison: [`docs/ADR-001-Playwright-vs-Selenium.md`](docs/ADR-001-Playwright-vs-Selenium.md)
- Test framework decision: [`docs/ADR-002-NUnit-Test-Framework.md`](docs/ADR-002-NUnit-Test-Framework.md)
- Detailed troubleshooting: [`docs/TROUBLESHOOTING.md`](docs/TROUBLESHOOTING.md)
- .NET coding standards: [`.github/instructions/dotnet-framework.instructions.md`](.github/instructions/dotnet-framework.instructions.md)
- AI safety guidelines: [`.github/instructions/ai-prompt-engineering.instructions.md`](.github/instructions/ai-prompt-engineering.instructions.md)

---

**Note for AI Agents**: This file follows the https://agents.md/ standard for universal AI agent instructions. GitHub Copilot also reads from `.github/copilot-instructions.md` which references this file.
