# ADR-001: Choice of Playwright over Selenium for Browser Automation

**Status:** Accepted  
**Date:** 2025-11-05  
**Decision Makers:** QA Automation Team  
**Technical Story:** Initial test automation framework setup

## Context

We needed to choose a browser automation tool for testing the GitHub Copilot documentation website. The main candidates were:
- Selenium WebDriver
- Playwright
- Cypress
- Puppeteer

## Decision

We chose **Microsoft Playwright** as the browser automation framework.

## Rationale

### Technical Advantages

| Criterion | Playwright | Selenium | Weight | Score |
|-----------|-----------|----------|--------|-------|
| **Auto-wait** | Built-in smart waiting | Manual waits needed | 20 | P: 20, S: 10 |
| **Speed** | Very fast | Moderate | 15 | P: 15, S: 10 |
| **Multi-browser** | Chromium, Firefox, WebKit | All major browsers | 15 | P: 15, S: 15 |
| **API Design** | Modern async/await | Mixed sync/async | 15 | P: 15, S: 10 |
| **Debugging** | Trace viewer, inspector | Basic screenshots | 10 | P: 10, S: 5 |
| **Parallelization** | Native support | Requires setup | 10 | P: 10, S: 7 |
| **Network Interception** | Built-in | Requires proxies | 10 | P: 10, S: 5 |
| **Community** | Growing rapidly | Mature | 5 | P: 4, S: 5 |
| **Total Score** | | | **100** | **P: 99, S: 67** |

### Key Benefits

1. **Auto-waiting**: Playwright automatically waits for elements to be actionable, reducing flaky tests
2. **Modern API**: Clean async/await pattern fits .NET's async model perfectly
3. **Multiple Browsers**: Single API for Chromium, Firefox, and WebKit
4. **Trace Viewer**: Post-mortem debugging with full trace of test execution
5. **Network Control**: Built-in request/response interception
6. **Faster Execution**: Significantly faster than Selenium due to direct browser protocol
7. **Official .NET Support**: First-class .NET SDK maintained by Microsoft

### Cons Considered

- **Newer Tool**: Less mature than Selenium (acceptable risk)
- **Smaller Community**: Growing but not as large as Selenium
- **IE Support**: No IE11 support (not needed for our use case)

## Consequences

### Positive
- ✅ Faster test execution (30-50% faster than Selenium)
- ✅ More reliable tests with auto-waiting
- ✅ Better debugging with trace viewer
- ✅ Simpler test code without explicit waits
- ✅ Native network mocking capabilities

### Negative
- ❌ Team needs to learn new API (minimal due to modern design)
- ❌ Smaller community for troubleshooting
- ❌ Cannot test IE11 (not a requirement)

### Neutral
- ⚪ Requires .NET 6+ (we're using .NET 9)
- ⚪ Browser installation via CLI (automated in CI)

## Implementation Notes

```csharp
// Playwright's clean API example
await page.ClickAsync("button#submit"); // Auto-waits for element
await page.WaitForURLAsync("**/success");

// vs Selenium's approach
var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
var button = wait.Until(d => d.FindElement(By.Id("submit")));
button.Click();
```

## Alternatives Considered

### Selenium WebDriver
- **Pros**: Mature, large community, broad browser support
- **Cons**: Slower, requires explicit waits, complex setup
- **Verdict**: Rejected due to speed and developer experience concerns

### Cypress
- **Pros**: Great DX, built-in retry logic
- **Cons**: JavaScript-only, runs in browser, no multi-tab support
- **Verdict**: Rejected as we need .NET integration

### Puppeteer
- **Pros**: Fast, Chromium-only optimization
- **Cons**: Chromium-only, JavaScript-only
- **Verdict**: Rejected due to single-browser limitation

## Migration Path

If we need to switch to Selenium later:
1. Page Object Model architecture remains the same
2. Replace `IPage` with `IWebDriver` in page constructors
3. Update element interaction methods in `BasePage`
4. Estimated effort: 3-5 days for framework, 1-2 weeks for all tests

## References

- [Playwright .NET Documentation](https://playwright.dev/dotnet/)
- [Why Playwright](https://playwright.dev/docs/why-playwright)
- [Selenium vs Playwright Comparison](https://www.browserstack.com/guide/playwright-vs-selenium)

## Review

**Next Review Date:** 2026-05-05 (6 months)  
**Review Triggers:**
- Major framework issues or instability
- Change in browser support requirements
- Significant community shift

---

**Approved by:** QA Automation Architect  
**Implementation Status:** ✅ Complete
