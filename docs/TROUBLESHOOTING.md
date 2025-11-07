# Troubleshooting Guide

This guide covers common issues and their solutions for the Test Automation Framework.

## Table of Contents
- [Installation Issues](#installation-issues)
- [Configuration Issues](#configuration-issues)
- [Test Execution Issues](#test-execution-issues)
- [Browser Issues](#browser-issues)
- [Performance Issues](#performance-issues)
- [CI/CD Issues](#cicd-issues)

---

## Installation Issues

### Playwright Browsers Not Found

**Symptoms:**
```
Playwright executable doesn't exist at <path>
```

**Solutions:**

```powershell
# Option 1: Install browsers from project directory
cd Tests
dotnet build
pwsh bin/Debug/net9.0/playwright.ps1 install

# Option 2: Install specific browser
pwsh bin/Debug/net9.0/playwright.ps1 install chromium

# Option 3: Install all browsers
pwsh bin/Debug/net9.0/playwright.ps1 install
```

**Linux/Mac:**
```bash
cd Tests
dotnet build
pwsh bin/Debug/net9.0/playwright.ps1 install
```

### PowerShell Not Found (Linux/Mac)

**Symptoms:**
```
pwsh: command not found
```

**Solutions:**

```bash
# Install PowerShell on Ubuntu/Debian
sudo apt-get update
sudo apt-get install -y powershell

# Install on macOS
brew install --cask powershell

# Alternative: Use bash script
bash bin/Debug/net9.0/playwright.sh install
```

### NuGet Package Restore Failures

**Symptoms:**
```
Unable to find package 'Microsoft.Playwright.NUnit'
```

**Solutions:**

```powershell
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore

# If still failing, check NuGet sources
dotnet nuget list source

# Add NuGet.org if missing
dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget.org
```

---

## Configuration Issues

### Configuration File Not Found

**Symptoms:**
```
Required configuration value 'BaseUrl' is missing or empty.
```

**Solutions:**

1. **Verify file location:**
   ```powershell
   # Ensure appsettings.json is in Tests/ directory
   Get-ChildItem Tests -Filter "appsettings.json"
   ```

2. **Check .csproj file:**
   ```xml
   <ItemGroup>
     <None Update="appsettings.json">
       <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
     </None>
   </ItemGroup>
   ```

3. **Rebuild project:**
   ```powershell
   dotnet clean
   dotnet build
   ```

### Environment Variables Not Working

**Symptoms:**
Configuration values don't change when setting environment variables.

**Solutions:**

1. **Check variable naming** (must be prefixed with `TEST_`):
   ```powershell
   # Correct
   $env:TEST_Browser__Type="firefox"
   
   # Incorrect
   $env:Browser__Type="firefox"
   ```

2. **Use double underscores** for nested properties:
   ```powershell
   # Browser.Type property
   $env:TEST_Browser__Type="chromium"
   
   # Execution.MaxRetries property
   $env:TEST_Execution__MaxRetries="5"
   ```

3. **Verify environment variable is set:**
   ```powershell
   # Windows PowerShell
   Get-ChildItem Env: | Where-Object { $_.Name -like "TEST_*" }
   
   # Linux/Mac
   printenv | grep TEST_
   ```

### Invalid Browser Type Error

**Symptoms:**
```
Browser type 'chrome' is invalid. Valid types: chromium, firefox, webkit
```

**Solution:**

Use correct browser type names:
- `chromium` (not "chrome")
- `firefox`
- `webkit` (not "safari")

Update `appsettings.json`:
```json
{
  "Browser": {
    "Type": "chromium"
  }
}
```

---

## Test Execution Issues

### Tests Timeout

**Symptoms:**
```
Timeout 30000ms exceeded.
```

**Solutions:**

1. **Increase timeout in configuration:**
   ```json
   {
     "Execution": {
       "DefaultTimeout": 60000,
       "NavigationTimeout": 90000
     }
   }
   ```

2. **Or via environment variable:**
   ```powershell
   $env:TEST_Execution__DefaultTimeout="60000"
   dotnet test
   ```

3. **For specific test, use CancelAfter:**
   ```csharp
   [Test]
   [CancelAfter(120000)] // 2 minutes
   public async Task LongRunningTest() { }
   ```

### Tests Are Flaky

**Symptoms:**
Tests pass sometimes, fail other times.

**Solutions:**

1. **Check for hardcoded waits** - Replace with smart waits:
   ```csharp
   // ❌ Bad - hardcoded delay
   await Task.Delay(5000);
   
   // ✅ Good - wait for condition
   await page.Locator("selector").WaitForAsync();
   ```

2. **Use data-test-id selectors:**
   ```csharp
   // ❌ Fragile
   await page.ClickAsync("div > button.submit");
   
   // ✅ Stable
   await page.ClickAsync("[data-test-id='submit-button']");
   ```

3. **Increase retry count:**
   ```json
   {
     "Execution": {
       "MaxRetries": 5,
       "RetryDelay": 2000
     }
   }
   ```

### Null Reference Exceptions

**Symptoms:**
```
System.NullReferenceException: Object reference not set to an instance of an object.
```

**Solutions:**

1. **Ensure BaseTest.SetUp() is called:**
   ```csharp
   [SetUp]
   public override async Task SetUp()
   {
       await base.SetUp(); // ← Must call base
       _homePage = new CopilotDocsHomePage(Page, Logger!);
   }
   ```

2. **Check for null-conditional operators:**
   ```csharp
   // Use null-conditional to avoid null reference
   await _homePage!.WaitForLoadAsync();
   Logger?.Info("Test started");
   ```

---

## Browser Issues

### Browser Launches But Is Not Visible

**Symptoms:**
Browser runs in headless mode even when set to headed.

**Solution:**

1. **Set headless to false:**
   ```json
   {
     "Browser": {
       "Headless": false
     }
   }
   ```

2. **Or via environment:**
   ```powershell
   $env:TEST_Browser__Headless="false"
   dotnet test
   ```

### Browser Doesn't Close After Tests

**Symptoms:**
Browser processes remain running after test completion.

**Solutions:**

1. **Ensure proper disposal:**
   ```csharp
   [TearDown]
   public override async Task TearDown()
   {
       await base.TearDown(); // ← Calls DisposeAsync()
   }
   ```

2. **Manually kill processes** (temporary fix):
   ```powershell
   # Windows
   Stop-Process -Name "chrome", "firefox", "webkit" -Force
   
   # Linux/Mac
   pkill -f "chrome|firefox|webkit"
   ```

### Screenshot/Trace Not Captured

**Symptoms:**
No screenshots or traces in TestResults folder after test failure.

**Solutions:**

1. **Verify settings:**
   ```json
   {
     "Browser": {
       "ScreenshotOnFailure": true,
       "CaptureTrace": true
     }
   }
   ```

2. **Check BaseTest.TearDown():**
   ```csharp
   [TearDown]
   public override async Task TearDown()
   {
       // This should call CaptureFailureDiagnosticsAsync() on failure
       await base.TearDown();
   }
   ```

3. **Check TestResults directory permissions:**
   ```powershell
   # Ensure directory exists and is writable
   New-Item -ItemType Directory -Force -Path "TestResults/Screenshots"
   New-Item -ItemType Directory -Force -Path "TestResults/Traces"
   ```

---

## Performance Issues

### Tests Run Slowly

**Solutions:**

1. **Enable parallel execution:**
   ```json
   {
     "Execution": {
       "Parallel": true,
       "Workers": 0  // 0 = auto-detect CPU cores
     }
   }
   ```

2. **Use headless mode for faster execution:**
   ```json
   {
     "Browser": {
       "Headless": true
     }
   }
   ```

3. **Optimize selectors** - Use efficient locators:
   ```csharp
   // ❌ Slow - complex XPath
   await page.Locator("//div[@class='container']//span[contains(text(), 'Submit')]").ClickAsync();
   
   // ✅ Fast - simple CSS or test-id
   await page.Locator("[data-test-id='submit']").ClickAsync();
   ```

4. **Reduce navigation timeout for fast pages:**
   ```json
   {
     "Execution": {
       "NavigationTimeout": 30000
     }
   }
   ```

### High Memory Usage

**Solutions:**

1. **Reduce parallel workers:**
   ```json
   {
     "Execution": {
       "Workers": 2  // Limit concurrent browsers
     }
   }
   ```

2. **Disable video recording:**
   ```json
   {
     "Browser": {
       "RecordVideo": false
     }
   }
   ```

3. **Clear TestResults periodically:**
   ```powershell
   Remove-Item -Recurse -Force TestResults/*
   ```

---

## CI/CD Issues

### GitHub Actions: Browser Installation Fails

**Symptoms:**
```
Error: Browser installation failed
```

**Solution:**

Update workflow to install system dependencies:
```yaml
- name: Install Playwright browsers
  run: |
    cd Tests
    dotnet build
    pwsh bin/Release/net9.0/playwright.ps1 install --with-deps chromium
```

### GitHub Actions: Tests Fail Locally Pass

**Common Causes:**

1. **Different environment variables:**
   ```yaml
   env:
     TEST_Browser__Headless: true
     TEST_Execution__DefaultTimeout: 60000
   ```

2. **Missing appsettings files:**
   ```yaml
   - name: Verify config files
     run: |
       ls Tests/appsettings*.json
   ```

3. **Browser-specific issues - Test with matrix:**
   ```yaml
   strategy:
     matrix:
       browser: [chromium, firefox]
   ```

### Artifacts Not Uploaded

**Solution:**

Ensure paths are correct in workflow:
```yaml
- name: Upload test results
  if: always()  # ← Important: upload even if tests fail
  uses: actions/upload-artifact@v4
  with:
    name: test-results
    path: |
      **/TestResults/**/*.trx
      **/TestResults/Screenshots/**
      **/TestResults/Traces/**
```

---

## Logging and Debugging

### Enable Detailed Logging

```powershell
# Set log level to Debug
$env:TEST_Logging__MinimumLevel="Debug"
dotnet test --logger "console;verbosity=detailed"
```

### View Playwright Trace

```powershell
# After test failure, open trace
playwright show-trace TestResults/Traces/<test-name>_<timestamp>.zip
```

### Access Test Logs

```powershell
# View most recent log file
Get-Content TestResults/Logs/TestLog_$(Get-Date -Format yyyy-MM-dd).log -Tail 50
```

---

## Getting Help

If issues persist:

1. **Check logs** in `TestResults/Logs/`
2. **Review screenshots** in `TestResults/Screenshots/`
3. **Analyze traces** using `playwright show-trace`
4. **Search GitHub Issues**: [Playwright Issues](https://github.com/microsoft/playwright-dotnet/issues)
5. **Create issue** with:
   - Full error message
   - Test code snippet
   - Configuration file
   - Log output
   - Trace file (if available)

---

**Last Updated:** 2025-11-05  
**Maintainer:** QA Automation Team
