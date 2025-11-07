# ADR-002: NUnit as the Test Framework

**Status:** Accepted  
**Date:** 2025-11-05  
**Decision Makers:** QA Automation Team  
**Technical Story:** Test framework selection for .NET automation

## Context

We needed to select a test framework for our .NET Playwright automation suite. The main options were:
- NUnit
- xUnit
- MSTest

## Decision

We chose **NUnit** as the test framework.

## Rationale

### Comparison Matrix

| Criterion | NUnit | xUnit | MSTest | Weight | Winner |
|-----------|-------|-------|--------|--------|--------|
| **Maturity** | Very mature | Mature | Very mature | 10 | Tie |
| **Community** | Large | Large | Moderate | 15 | NUnit/xUnit |
| **Attributes** | Rich set | Minimal | Basic | 15 | NUnit |
| **Test Setup** | `[SetUp]`, `[OneTimeSetUp]` | Constructor/IDisposable | `[TestInitialize]` | 20 | NUnit |
| **Parallel Execution** | Excellent | Excellent | Good | 15 | NUnit/xUnit |
| **Assertions** | FluentAssertions compatible | FluentAssertions compatible | Basic | 10 | Tie |
| **TestCase Support** | `[TestCase]` | `[Theory]` + `[InlineData]` | `[DataRow]` | 10 | Tie |
| **Playwright Integration** | Excellent | Excellent | Good | 5 | Tie |
| **Total** | | | | **100** | **NUnit: 90, xUnit: 85, MSTest: 70** |

### Key Decision Factors

1. **Rich Attribute System**: NUnit's `[SetUp]`, `[TearDown]`, `[OneTimeSetUp]`, `[OneTimeTearDown]` provide granular control
2. **TestCase Simplicity**: `[TestCase]` is more intuitive than xUnit's `[Theory]` + `[InlineData]` combination
3. **Category Support**: `[Category]` for easy test organization (Smoke, Regression, etc.)
4. **Test Lifecycle**: Clear separation of test-level and fixture-level setup
5. **Industry Standard**: Widely used in .NET test automation
6. **Microsoft Playwright Examples**: Official Playwright .NET docs use NUnit

### Code Comparison

**NUnit** (chosen):
```csharp
[TestFixture]
public class MyTests : BaseTest
{
    [SetUp]
    public async Task SetUp() { /* Per-test setup */ }
    
    [Test]
    [Category("Smoke")]
    [TestCase("data1")]
    [TestCase("data2")]
    public async Task MyTest(string data) { }
}
```

**xUnit** (alternative):
```csharp
public class MyTests : BaseTest, IAsyncLifetime
{
    public async Task InitializeAsync() { /* Per-test setup */ }
    
    [Fact]
    [Trait("Category", "Smoke")]
    public async Task MyTest() { }
    
    [Theory]
    [InlineData("data1")]
    [InlineData("data2")]
    public async Task MyParameterizedTest(string data) { }
}
```

## Consequences

### Positive
- ✅ Familiar syntax for team members with testing background
- ✅ Clear test lifecycle with explicit setup/teardown methods
- ✅ `[TestCase]` is more readable than `[Theory]` + `[InlineData]`
- ✅ Built-in `[Category]` for test organization
- ✅ Excellent integration with Playwright NUnit package
- ✅ Rich assertion messages with FluentAssertions

### Negative
- ❌ Not as "modern" as xUnit (acceptable trade-off)
- ❌ Slight verbosity compared to xUnit's implicit setup

### Neutral
- ⚪ Similar parallel execution capabilities to xUnit
- ⚪ Both require FluentAssertions for modern assertion syntax

## Alternatives Considered

### xUnit
**Pros**: 
- Modern, opinionated design
- No hidden test state (no `[SetUp]`)
- Cleaner parallel execution

**Cons**:
- More verbose data-driven tests
- Constructor-based setup less intuitive for UI tests
- No `[Category]` attribute (uses `[Trait]`)

**Verdict**: Rejected due to less intuitive setup pattern for browser automation

### MSTest
**Pros**:
- Official Microsoft framework
- Good Visual Studio integration

**Cons**:
- Less feature-rich than NUnit/xUnit
- Slower community adoption of new features
- Weaker parallel execution support historically

**Verdict**: Rejected due to limited features compared to NUnit

## Implementation Patterns

### Test Lifecycle
```csharp
[TestFixture]
public class ExampleTests : BaseTest
{
    // Runs once before all tests in fixture
    [OneTimeSetUp]
    public override void OneTimeSetUp() { }
    
    // Runs before each test
    [SetUp]
    public override async Task SetUp() { }
    
    // Actual test
    [Test]
    public async Task MyTest() { }
    
    // Runs after each test
    [TearDown]
    public override async Task TearDown() { }
    
    // Runs once after all tests in fixture
    [OneTimeTearDown]
    public override void OneTimeTearDown() { }
}
```

### Data-Driven Tests
```csharp
[Test]
[TestCase("Chrome", "https://example.com")]
[TestCase("Firefox", "https://example.org")]
public async Task BrowserTest(string browser, string url) { }

// Or with TestCaseSource for complex data
[Test]
[TestCaseSource(typeof(TestData), nameof(TestData.Scenarios))]
public async Task ComplexTest(Scenario scenario) { }
```

## Migration Considerations

If switching to xUnit in future:
1. Replace `[TestFixture]` with class-level fixture
2. Convert `[SetUp]` to constructor or `IAsyncLifetime.InitializeAsync()`
3. Change `[TestCase]` to `[Theory]` + `[InlineData]`
4. Update `[Category]` to `[Trait]`
5. Estimated effort: 2-3 days

## References

- [NUnit Documentation](https://docs.nunit.org/)
- [Microsoft Playwright NUnit Package](https://www.nuget.org/packages/Microsoft.Playwright.NUnit)
- [xUnit vs NUnit Comparison](https://xunit.net/docs/comparisons)

## Review

**Next Review Date:** 2026-05-05 (6 months)  
**Review Triggers:**
- Major NUnit issues
- Team productivity concerns
- xUnit adds compelling features

---

**Approved by:** QA Automation Architect  
**Implementation Status:** ✅ Complete
