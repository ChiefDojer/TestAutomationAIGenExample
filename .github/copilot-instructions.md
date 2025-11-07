# GitHub Copilot Instructions# Test Automation Solution - AI Agent Instructions



> **⚠️ Primary Instructions**: See [`AGENTS.md`](../AGENTS.md) in repository root## Project State & Architecture

> This is a **production-ready .NET 9 dual-framework test automation solution** with **both Playwright and Selenium** implementations for testing the GitHub Copilot documentation website.

> This file provides GitHub Copilot-specific context and references. For full project

> instructions, coding standards, and architecture details, **see [`AGENTS.md`](../AGENTS.md)**.**Critical**: This solution contains **TWO complete frameworks** with identical test coverage:

> - `GitHubCopilotDocsPlaywright/` - Playwright-based (async/await)

> **Sync Status**: This file is a reference wrapper. Content is maintained in `AGENTS.md`.- `GitHubCopilotDocsSelenium/` - Selenium WebDriver-based (synchronous)



## Quick Reference**Technology Stack**:

- .NET 9 with modern C# features (nullable types, required properties, collection expressions)

**Project**: .NET 9 dual-framework test automation (Playwright + Selenium)  - Test Framework: NUnit 4.2.2 with FluentAssertions

**Target**: https://docs.github.com/en/copilot  - Automation: Playwright 1.55.0 **AND** Selenium WebDriver 4.27.0

**Build**: `dotnet build`  - Target: https://docs.github.com/en/copilot

**Test**: `dotnet test`  

**Documentation**: [`AGENTS.md`](../AGENTS.md) (primary source of truth)**Shared Architecture** (Both Frameworks):

```

## Important Files (Read These First!)Tests → Pages (POM) → Core (Infrastructure) → Driver (Playwright/Selenium) → Browser

```

### Universal AI Instructions

- **[`AGENTS.md`](../AGENTS.md)** - ⭐ **PRIMARY** - Complete AI agent instructions## Critical Developer Workflows

  - Project architecture and structure

  - Build and test commands### Build & Test Commands (PowerShell)

  - Code standards and patterns```powershell

  - Framework-specific guidance (Playwright vs Selenium)# Build entire solution (both frameworks)

  - Troubleshooting and common issuesdotnet build



### Specialized Instructions# Run ALL tests (both Playwright and Selenium)

- [`.github/instructions/dotnet-framework.instructions.md`](instructions/dotnet-framework.instructions.md) - .NET 9 coding standardsdotnet test

  - Modern C# features and patterns

  - Dependency injection guidelines# Run specific framework

  - Async/await best practicesdotnet test GitHubCopilotDocsPlaywright/GitHubCopilotDocs.Tests.csproj

  - Security and performance standardsdotnet test GitHubCopilotDocsSelenium/GitHubCopilotDocs.Selenium.Tests.csproj

  

- [`.github/instructions/ai-prompt-engineering.instructions.md`](instructions/ai-prompt-engineering.instructions.md) - AI safety guidelines# Run with detailed output

  - Prompt engineering best practicesdotnet test --logger "console;verbosity=detailed"

  - Safety and bias mitigation

  - Responsible AI usage# Run specific category (works across both frameworks)

  - Security considerationsdotnet test --filter Category=Smoke

dotnet test --filter Category=E2E

### Architecture Documentationdotnet test --filter Category=DataDriven

- [`docs/ADR-001-Playwright-vs-Selenium.md`](../docs/ADR-001-Playwright-vs-Selenium.md) - Framework comparison

- [`docs/ADR-002-NUnit-Test-Framework.md`](../docs/ADR-002-NUnit-Test-Framework.md) - Test framework decision# Run in different browser (Playwright: chromium/firefox/webkit)

- [`docs/TROUBLESHOOTING.md`](../docs/TROUBLESHOOTING.md) - Detailed troubleshooting guide$env:TEST_Browser__Type="firefox"; dotnet test GitHubCopilotDocsPlaywright/



## Project Structure Overview# Run in different browser (Selenium: chrome/firefox/edge/safari)

$env:TEST_Browser__Type="firefox"; dotnet test GitHubCopilotDocsSelenium/

```

TestAutomationAIGenExample/# Add packages (specify correct project)

├── AGENTS.md                           ⭐ Primary AI instructionsdotnet add GitHubCopilotDocsPlaywright/GitHubCopilotDocs.Tests.csproj package <PackageName>

├── GitHubCopilotDocsPlaywright/        Async Playwright testsdotnet add GitHubCopilotDocsSelenium/GitHubCopilotDocs.Selenium.Tests.csproj package <PackageName>

├── GitHubCopilotDocsSelenium/          Sync Selenium tests```

├── docs/                               Architecture decisions

└── .github/**Shell Syntax**: Use `;` for command chaining in PowerShell, NOT `&&`

    ├── copilot-instructions.md         This file (reference only)

    └── instructions/                   Specialized topics### First-Time Setup

``````powershell

# Clone and build

Both `GitHubCopilotDocsPlaywright/` and `GitHubCopilotDocsSelenium/` share identical test coverage and architecture patterns, but use different automation frameworks.dotnet restore

dotnet build

## Critical Coding Standards (Quick Reminder)

# Playwright: Install browsers (one-time)

### Test Namingcd GitHubCopilotDocsPlaywright

`MethodName_StateUnderTest_ExpectedBehavior`pwsh bin/Debug/net9.0/playwright.ps1 install chromium

cd ..

### Framework Differences

| Aspect | Playwright | Selenium |# Selenium: WebDriverManager auto-downloads drivers (no setup needed)

|--------|-----------|----------|```

| **Methods** | `async Task` with `await` | Synchronous (`void`) |

| **Base class** | `await base.SetUp()` | `base.SetUp()` |### Project Structure Convention

| **Page Objects** | Accept `IPage`, use `await` | Accept `IWebDriver`, synchronous |```

| **Locators** | CSS strings: `"button:has-text('Click')"` | `By` objects: `By.CssSelector("button")` |TestAutomationAIGenExample.sln

├── GitHubCopilotDocsPlaywright/    # Playwright Framework (async)

### All Tests Must:│   ├── Core/

1. Inherit from `BaseTest`│   │   ├── Configuration/          # ConfigLoader, TestSettings

2. Call `base.SetUp()` / `base.TearDown()` (with `await` for Playwright)│   │   ├── Driver/                 # PlaywrightDriverManager

3. Use `LogStep()` for readability│   │   ├── Logging/                # TestLogger

4. Use FluentAssertions for assertions│   │   └── BaseTest.cs             # Base class (async methods)

5. Follow AAA pattern with explicit comments│   ├── Pages/

│   │   ├── BasePage.cs             # Common page utilities (async)

### Example (Playwright)│   │   └── GitHub/                 # Page Objects

```csharp│   ├── E2E/                        # End-to-end tests

[TestFixture]│   │   └── DataDriven/             # Parameterized tests

[Category("Smoke")]│   ├── Data/                       # TestDataProvider

public class MyTests : BaseTest│   ├── appsettings.json            # Default config

{│   └── appsettings.Development.json

    private MyPage? _myPage;│

    ├── GitHubCopilotDocsSelenium/      # Selenium Framework (sync)

    [SetUp]│   ├── Core/

    public override async Task SetUp()│   │   ├── Configuration/          # ConfigLoader, TestSettings (identical)

    {│   │   ├── Driver/                 # SeleniumDriverManager

        await base.SetUp();  // REQUIRED│   │   ├── Logging/                # TestLogger (identical)

        _myPage = new MyPage(Page, Logger!);│   │   └── BaseTest.cs             # Base class (sync methods)

    }│   ├── Pages/

    │   │   ├── BasePage.cs             # Common page utilities (sync)

    [Test]│   │   └── GitHub/                 # Page Objects

    public async Task PageLoad_NavigateToHome_DisplaysCorrectTitle()│   ├── E2E/                        # End-to-end tests

    {│   │   └── DataDriven/             # Parameterized tests

        // Arrange│   ├── Data/                       # TestDataProvider (identical)

        LogStep("Navigate to home page");│   ├── appsettings.json            # Default config

        │   └── appsettings.Development.json

        // Act│

        await NavigateToAsync("/");├── docs/

        var title = await _myPage!.GetTitleAsync();│   ├── ADR-001-Playwright-vs-Selenium.md  # Framework comparison

        │   ├── ADR-002-NUnit-Test-Framework.md

        // Assert│   └── TROUBLESHOOTING.md

        title.Should().Contain("Expected Text");│

    }└── .github/

}    ├── workflows/test-automation-ci.yml   # CI/CD pipeline

```    ├── copilot-instructions.md            # This file

    └── instructions/                      # AI instruction files

### Example (Selenium)        ├── ai-prompt-engineering.instructions.md

```csharp        └── dotnet-framework.instructions.md

[TestFixture]```

[Category("Smoke")]

public class MyTests : BaseTest## Code Standards (Enforced via `.github/instructions/dotnet-framework.instructions.md`)

{

    private MyPage? _myPage;### Test-Specific Patterns (MANDATORY)

    - **Naming**: `MethodName_StateUnderTest_ExpectedBehavior`

    [SetUp]  ```csharp

    public override void SetUp()  PageLoad_NavigateToCopilotDocs_PageLoadsWithMainHeading()

    {  ```

        base.SetUp();  // REQUIRED- **Structure**: AAA pattern with explicit comments

        var wait = new WebDriverWait(Driver, TimeSpan.FromMilliseconds(Settings.Execution.DefaultTimeout));  ```csharp

        _myPage = new MyPage(Driver, wait, Logger!);  // Playwright (async)

    }  [Test]

      public async Task Example()

    [Test]  {

    public void PageLoad_NavigateToHome_DisplaysCorrectTitle()      // Arrange

    {      LogStep("Setup description");

        // Arrange      

        LogStep("Navigate to home page");      // Act

              await _homePage.DoSomethingAsync();

        // Act      

        NavigateTo("/");      // Assert

        var title = _myPage!.GetTitle();      result.Should().Be(expected);

          }

        // Assert  

        title.Should().Contain("Expected Text");  // Selenium (sync)

    }  [Test]

}  public void Example()

```  {

      // Arrange

## Running Tests      LogStep("Setup description");

      

```powershell      // Act

# All tests (both frameworks)      _homePage.DoSomething();

dotnet test      

      // Assert

# Specific framework      result.Should().Be(expected);

dotnet test GitHubCopilotDocsPlaywright/  }

dotnet test GitHubCopilotDocsSelenium/  ```

- **Assertions**: Use FluentAssertions for readability

# By category  ```csharp

dotnet test --filter Category=Smoke  result.Should().NotBeNullOrWhiteSpace("because page title must be present");

  ```

# Different browser (Playwright: chromium/firefox/webkit)- **Test Independence**: NO shared state between tests; each test must be idempotent

$env:TEST_Browser__Type="firefox"; dotnet test GitHubCopilotDocsPlaywright/- **Categories**: Use `[Category("Smoke")]`, `[Category("E2E")]`, `[Category("DataDriven")]`



# Different browser (Selenium: chrome/firefox/edge)### .NET Modern Practices

$env:TEST_Browser__Type="firefox"; dotnet test GitHubCopilotDocsSelenium/- Enable **nullable reference types** (enabled in .NET 9)

```- Use `ArgumentNullException.ThrowIfNull()` for parameter validation

- Async methods: Always accept `CancellationToken` for long operations

## When to Use Which File- DI via constructor injection (see `BaseTest`, `BasePage`)

- Configuration: Use Options pattern via `ConfigLoader.Instance.Settings`

| Situation | Read This File |- **Playwright**: Always use `await` - never `Task.Result` or `.Wait()`

|-----------|----------------|- **Selenium**: Synchronous methods - no async/await needed

| **General project setup** | [`AGENTS.md`](../AGENTS.md) |

| **Build/test commands** | [`AGENTS.md`](../AGENTS.md) |### Framework-Specific Patterns

| **Architecture overview** | [`AGENTS.md`](../AGENTS.md) |- **Inherit from BaseTest**: All test classes must inherit from `BaseTest`

| **Adding new tests** | [`AGENTS.md`](../AGENTS.md) |  ```csharp

| **.NET coding standards** | [`.github/instructions/dotnet-framework.instructions.md`](instructions/dotnet-framework.instructions.md) |  // Playwright

| **AI safety/prompts** | [`.github/instructions/ai-prompt-engineering.instructions.md`](instructions/ai-prompt-engineering.instructions.md) |  public class MyTests : BaseTest

| **Framework comparison** | [`docs/ADR-001-Playwright-vs-Selenium.md`](../docs/ADR-001-Playwright-vs-Selenium.md) |  

| **Troubleshooting** | [`docs/TROUBLESHOOTING.md`](../docs/TROUBLESHOOTING.md) |  // Selenium

  public class MyTests : BaseTest

## Maintainer Note  ```

- **Call base methods**: 

**To update AI instructions:**  - Playwright: `await base.SetUp()` and `await base.TearDown()`

1. Edit [`AGENTS.md`](../AGENTS.md) (primary source of truth)  - Selenium: `base.SetUp()` and `base.TearDown()`

2. This file (`.github/copilot-instructions.md`) is a reference wrapper - update only if adding GitHub Copilot-specific notes- **Use LogStep()**: For test readability

3. Specialized topics go in `.github/instructions/*.instructions.md`  ```csharp

4. Architecture decisions go in `docs/ADR-*.md`  LogStep("Navigate to page");

  ```

**No sync script needed** - this file intentionally references `AGENTS.md` instead of duplicating content.- **Configuration**: Access via `Settings` property in BaseTest

  ```csharp

---  var baseUrl = Settings.BaseUrl;

  ```

**For GitHub Copilot**: When suggesting code or answering questions, consult [`AGENTS.md`](../AGENTS.md) first for project-specific patterns and standards. Use the specialized instruction files (`.github/instructions/`) for detailed .NET and AI safety guidance.- **Page Objects**: 

  - Playwright: `_homePage = new CopilotDocsHomePage(Page, Logger!);`
  - Selenium: `_homePage = new CopilotDocsHomePage(Driver, Wait, Logger!);`
- **Retry Logic**: Use built-in retry methods
  - Playwright: `await RetryAsync(async () => await SomeOperationAsync());`
  - Selenium: `Retry(() => SomeOperation());`

### Locator Strategy (Priority Order)
1. **data-test-id** attributes (most stable)
2. **Semantic HTML** with text: 
   - Playwright: `button:has-text('Submit')`
   - Selenium: `By.XPath("//button[contains(text(),'Submit')]")`
3. **ARIA labels**: `[aria-label='Search']` / `By.CssSelector("[aria-label='Search']")`
4. **Stable CSS**: `[role='navigation']` / `By.CssSelector("[role='navigation']")`
5. **Avoid**: Index-based XPath, brittle CSS like `div > div > span`

### Resource Management
- **Playwright**: `PlaywrightDriverManager` implements `IAsyncDisposable` - proper async disposal
- **Selenium**: `SeleniumDriverManager` implements `IDisposable` - synchronous disposal
- `using` declarations not needed - `BaseTest.TearDown()` handles cleanup
- All screenshots/traces/HTML auto-captured on failure

## Key Conventions from Existing Files

### .gitignore Exclusions
- Build outputs: `bin/`, `obj/`, `Debug/`, `Release/`
- Test results: `TestResult.xml`, `[Tt]est[Rr]esult*/`
- Coverage: `CodeCoverage/`
- Logs: `[Ll]og/`, `[Ll]ogs/`

### AI Guidance Files
- **`.github/instructions/dotnet-framework.instructions.md`**: .NET coding standards for this project
  - Covers: DI, async patterns, testing, security, performance, logging
  - Modern .NET 9 features (nullable types, required properties, collection expressions)
- **`.github/instructions/ai-prompt-engineering.instructions.md`**: AI safety and best practices

## Critical Architecture Details

### Configuration Hierarchy (Highest to Lowest Priority)
1. Environment variables (prefix: `TEST_`)
2. `appsettings.{TEST_ENV}.json` (e.g., `appsettings.Development.json`)
3. `appsettings.json`

**Example**:
```powershell
# Override browser type
$env:TEST_Browser__Type="firefox"
$env:TEST_ENV="Development"  # Uses appsettings.Development.json
```

### Test Lifecycle Hooks (NUnit)
```csharp
[OneTimeSetUp]    // Once before all tests in fixture
[SetUp]           // Before each test
[Test]            // The actual test
[TearDown]        // After each test (captures diagnostics on failure)
[OneTimeTearDown] // Once after all tests in fixture
```

### Automatic Failure Diagnostics
On test failure, `BaseTest.TearDown()` automatically captures:
- Screenshot → `TestResults/Screenshots/`
- Playwright trace → `TestResults/Traces/` (view with `playwright show-trace`)
- Page HTML → `TestResults/Screenshots/`
- All attached to NUnit test results

## Common Operations

### Creating New Page Object
```csharp
// Playwright (async)
public class MyNewPage : BasePage
{
    private const string SubmitButtonSelector = "[data-test-id='submit']";
    
    public MyNewPage(IPage page, TestLogger logger) : base(page, logger) { }
    
    public async Task ClickSubmitAsync()
    {
        Logger.Debug("Clicking submit button");
        await ClickAsync(SubmitButtonSelector);
    }
    
    public async Task<string> GetResultTextAsync()
    {
        return await GetTextAsync("[data-test-id='result']");
    }
}

// Selenium (sync)
public class MyNewPage : BasePage
{
    private static readonly By SubmitButtonSelector = By.CssSelector("[data-test-id='submit']");
    
    public MyNewPage(IWebDriver driver, WebDriverWait wait, TestLogger logger) 
        : base(driver, wait, logger) { }
    
    public void ClickSubmit()
    {
        Logger.Debug("Clicking submit button");
        Click(SubmitButtonSelector);
    }
    
    public string GetResultText()
    {
        return GetText(By.CssSelector("[data-test-id='result']"));
    }
}
```

### Creating New Test Class
```csharp
// Playwright (async)
[TestFixture]
[Category("E2E")]
public class MyNewTests : BaseTest
{
    private MyNewPage? _myPage;
    
    [SetUp]
    public override async Task SetUp()
    {
        await base.SetUp();  // MUST call base
        _myPage = new MyNewPage(Page, Logger!);
    }
    
    [Test]
    public async Task MyTest_InitialState_ExpectedBehavior()
    {
        // Arrange
        LogStep("Setup test data");
        
        // Act
        await NavigateToAsync("/some-path");
        await _myPage!.ClickSubmitAsync();
        var result = await _myPage.GetResultTextAsync();
        
        // Assert
        LogStep("Verify result");
        result.Should().Contain("expected text");
    }
}

// Selenium (sync)
[TestFixture]
[Category("E2E")]
public class MyNewTests : BaseTest
{
    private MyNewPage? _myPage;
    
    [SetUp]
    public override void SetUp()
    {
        base.SetUp();  // MUST call base
        var wait = new WebDriverWait(Driver, TimeSpan.FromMilliseconds(Settings.Execution.DefaultTimeout));
        _myPage = new MyNewPage(Driver, wait, Logger!);
    }
    
    [Test]
    public void MyTest_InitialState_ExpectedBehavior()
    {
        // Arrange
        LogStep("Setup test data");
        
        // Act
        NavigateTo("/some-path");
        _myPage!.ClickSubmit();
        var result = _myPage.GetResultText();
        
        // Assert
        LogStep("Verify result");
        result.Should().Contain("expected text");
    }
}
```

### Adding Data-Driven Test
```csharp
// Playwright (async)
[Test]
[TestCase("value1", "expected1")]
[TestCase("value2", "expected2")]
public async Task MyTest_WithData_ProducesResult(string input, string expected)
{
    // Arrange
    LogStep($"Testing with input: {input}");
    
    // Act
    var result = await _page.DoSomethingAsync(input);
    
    // Assert
    result.Should().Be(expected);
}

// Selenium (sync)
[Test]
[TestCase("value1", "expected1")]
[TestCase("value2", "expected2")]
public void MyTest_WithData_ProducesResult(string input, string expected)
{
    // Arrange
    LogStep($"Testing with input: {input}");
    
    // Act
    var result = _page.DoSomething(input);
    
    // Assert
    result.Should().Be(expected);
}
```

## Troubleshooting Quick Reference

| Issue | Solution |
|-------|----------|
| **Playwright**: `Playwright executable doesn't exist` | `cd GitHubCopilotDocsPlaywright; pwsh bin/Debug/net9.0/playwright.ps1 install chromium` |
| **Selenium**: ChromeDriver version mismatch | WebDriverManager auto-downloads correct version; check Chrome browser version |
| Config not loading | Check `appsettings.json` has `CopyToOutputDirectory=PreserveNewest` in .csproj |
| Tests timeout | Increase `TEST_Execution__DefaultTimeout` env var (default: 30000ms) |
| **Playwright**: Browser not closing | Ensure `await base.TearDown()` is called in test cleanup |
| **Selenium**: Browser not closing | Ensure `base.TearDown()` is called in test cleanup |
| Screenshots not saved | Check `Settings.Browser.ScreenshotOnFailure = true` in appsettings.json |
| "Driver not initialized" error | Verify `base.SetUp()` is called before accessing Driver/Page property |

**Full troubleshooting**: See `docs/TROUBLESHOOTING.md`

## ADRs (Architecture Decision Records)
- `docs/ADR-001-Playwright-vs-Selenium.md` - Framework comparison and decision guidance
- `docs/ADR-002-NUnit-Test-Framework.md` - Why NUnit over xUnit/MSTest

## Setup Workflow for New Features
1. **Clarify scope**: Understand requirements and choose framework (Playwright for modern async, Selenium for legacy/sync compatibility)
2. **Create Page Object**: If new page, inherit from `BasePage` in correct project
   - Playwright: Use async methods, accept `IPage` in constructor
   - Selenium: Use sync methods, accept `IWebDriver` and `WebDriverWait` in constructor
3. **Write tests**: Follow AAA pattern, use `LogStep()`, inherit from `BaseTest`
   - Playwright: Use `async Task` methods with `await`
   - Selenium: Use `void` or sync methods
4. **Add test data**: Use `TestDataProvider` for data-driven scenarios (shared across both frameworks)
5. **Run locally**: 
   - Playwright: `dotnet test GitHubCopilotDocsPlaywright/ --filter Category=Smoke`
   - Selenium: `dotnet test GitHubCopilotDocsSelenium/ --filter Category=Smoke`
6. **Validate**: Run `dotnet build` and full `dotnet test` before commit
