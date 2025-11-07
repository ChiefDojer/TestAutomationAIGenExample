---
mode: 'agent'
description: 'Ensure .NET/C# code meets best practices for the solution/project.'
---
# .NET/C# Best Practices

Your task is to ensure .NET/C# code in ${selection} meets the best practices specific to this solution/project. This includes:

## Documentation & Structure

- Create comprehensive XML documentation comments for:
  - All public and protected classes, interfaces, structs, and enums
  - Public and protected methods and properties
  - Complex internal APIs that will be consumed by other teams
- Include `<summary>`, `<param>`, `<returns>`, `<exception>`, and `<example>` tags where appropriate
- Document why, not just what (explain business logic and non-obvious decisions)
- Follow established namespace conventions:
  - Organize by feature/domain, not technical layers
  - Use consistent depth (avoid excessive nesting beyond 4-5 levels)
  - Example: `CompanyName.ProductName.Feature.SubFeature`

## Design Patterns & Architecture

- Use primary constructor syntax for simple dependency injection scenarios
  - Avoid for classes with complex initialization logic
  - Consider traditional constructors when you need validation or field initialization
- Implement CQRS patterns where appropriate:
  - Command Handler pattern for write operations
  - Query Handler pattern for read operations
  - Use MediatR or similar libraries for request/response patterns
- Follow SOLID principles:
  - Single Responsibility: One class, one reason to change
  - Interface Segregation: Many specific interfaces over one general-purpose interface
  - Dependency Inversion: Depend on abstractions, not concretions
- Use common patterns appropriately:
  - Repository pattern for data access abstraction
  - Unit of Work for transactional operations
  - Factory/Builder patterns for complex object construction
  - Strategy pattern for interchangeable algorithms
  - Options pattern for configuration

## Dependency Injection & Services

- Use constructor dependency injection as the primary DI method
- Leverage nullable reference types (enabled by default in .NET 8+) instead of manual null checks
- For explicit null validation, use `ArgumentNullException.ThrowIfNull()` (C# 11+)
- Register services with appropriate lifetimes:
  - Singleton: Stateless services, shared across application lifetime
  - Scoped: Per-request state (web apps), per-operation state (console/services)
  - Transient: Lightweight, stateless services instantiated per use
- Avoid the Service Locator anti-pattern (don't inject `IServiceProvider` directly)
- Use keyed services (.NET 8+) when you need multiple implementations of the same interface
- Implement interfaces for all services to enable testability and loose coupling
- Consider using factory patterns for services requiring runtime parameters

## Resource Management & Localization

- Use `IStringLocalizer<T>` from `Microsoft.Extensions.Localization` (preferred modern approach)
- Fallback to `ResourceManager` for legacy compatibility
- Organize resources logically:
  - Separate files: LogMessages, ErrorMessages, ValidationMessages, UIStrings
  - Use consistent naming: Feature.Action.Context (e.g., "User.Create.Success")
- Support multiple cultures with satellite resource assemblies
- Use string interpolation safely: `localizer["Welcome {0}", userName]`
- Implement `IDisposable` for classes holding unmanaged resources
- Use `using` statements or declarations for automatic disposal
- Prefer `IAsyncDisposable` and `DisposeAsync()` for async cleanup operations

## Async/Await Patterns

- Use async/await for all I/O-bound operations (file, network, database)
- Return `Task` or `Task<T>` from async methods; avoid `async void` except for event handlers
- **Note on ConfigureAwait**: In .NET 8+ applications (not libraries), `ConfigureAwait(false)` is typically unnecessary
  - Use it in library code where you don't need to return to the original context
  - ASP.NET Core apps don't have a SynchronizationContext by default
- Always accept and pass `CancellationToken` parameters for long-running operations
- Use `ValueTask<T>` for hot paths and frequently-completed operations to reduce allocations
- Avoid mixing blocking calls (`Task.Result`, `Task.Wait`) with async code - causes deadlocks
- Use `Task.WhenAll()` for parallel async operations, not `Task.WaitAll()`
- Handle `OperationCanceledException` gracefully for cancellation scenarios

## Testing Standards

- Use xUnit (industry standard) or MSTest framework with FluentAssertions
- Follow AAA pattern (Arrange, Act, Assert) or Given-When-Then for BDD
- Use Moq, NSubstitute, or FakeItEasy for mocking dependencies
- Test Coverage Guidelines:
  - Unit tests: All public methods, edge cases, error conditions
  - Integration tests: Database operations, external service calls, API endpoints
  - Test both success paths and failure scenarios
  - Include boundary condition and null/empty input tests
- Naming convention: `MethodName_StateUnderTest_ExpectedBehavior`
- Use Test Data Builders or AutoFixture for complex object creation
- Avoid testing implementation details; focus on behavior
- Keep tests independent and idempotent (no shared state)
- Use `[Theory]` with `[InlineData]` or `[MemberData]` for parameterized tests
- Consider snapshot testing for complex object comparisons
- Achieve 70-80%+ code coverage, but prioritize meaningful tests over coverage metrics

## Configuration & Settings

- Use strongly-typed configuration with the Options pattern:
  - `IOptions<T>` for singleton settings
  - `IOptionsSnapshot<T>` for scoped, reloadable settings
  - `IOptionsMonitor<T>` for real-time configuration updates
- Implement validation:
  - Data annotations: `[Required]`, `[Range]`, `[RegularExpression]`
  - `IValidateOptions<T>` for complex validation logic
  - Enable `ValidateOnStart()` in .NET 8+ to fail fast
- Configuration hierarchy (highest to lowest precedence):
  - Command-line arguments
  - Environment variables
  - User secrets (development only)
  - appsettings.{Environment}.json
  - appsettings.json
- Never commit secrets to source control:
  - Use User Secrets for local development
  - Use Azure Key Vault, AWS Secrets Manager, or similar in production
  - Consider environment variables for container deployments
- Use `IConfiguration` binding with `Configure<T>()` or `Bind()` methods
- Implement configuration sections with clear hierarchies

## AI Integration & Semantic Kernel (if applicable)

- Use Microsoft.SemanticKernel for AI orchestration when appropriate
- Implement proper kernel configuration:
  - Register AI services (ChatCompletion, TextEmbedding, etc.)
  - Configure model settings (temperature, max tokens, top-p)
  - Use dependency injection for kernel instances
- Handle AI-specific concerns:
  - Implement retry policies for transient failures
  - Set appropriate timeouts
  - Use streaming responses for long-running operations
  - Handle rate limiting and quota management
- Responsible AI practices:
  - Implement content filtering and safety checks
  - Log AI interactions for auditing
  - Provide clear user disclosure when AI is involved
  - Handle bias and fairness considerations
  - Implement human-in-the-loop for critical decisions
- Use structured outputs (JSON mode, function calling) for reliable parsing
- Consider costs and implement usage tracking/limits

## Error Handling & Logging

- Use structured logging with `ILogger<T>` from `Microsoft.Extensions.Logging`
- Logging best practices:
  - Use log levels appropriately: Trace, Debug, Information, Warning, Error, Critical
  - Use high-performance logging with `LoggerMessage.Define` or source generators
  - Include structured data: `logger.LogInformation("User {UserId} completed action {Action}", userId, action)`
  - Use log scopes for contextual information: `using (logger.BeginScope(...))`
  - Avoid logging sensitive data (PII, passwords, tokens)
- Exception handling:
  - Catch specific exceptions, not generic `Exception` (unless at application boundaries)
  - Use exception filters when appropriate: `catch (Exception ex) when (ex is not OutOfMemoryException)`
  - Include meaningful error messages with context
  - Use custom exceptions for domain-specific errors
  - Don't swallow exceptions without logging
  - Re-throw with `throw;` not `throw ex;` to preserve stack traces
- Implement global exception handling:
  - Use middleware in ASP.NET Core
  - Use try-catch at application entry points for console apps
- Consider using Problem Details (RFC 7807) for API error responses

## Performance Optimization

- Use modern C# features for better performance:
  - `Span<T>` and `Memory<T>` for memory-efficient operations
  - `ValueTask<T>` for hot paths with frequent synchronous completion
  - `ArrayPool<T>` for reducing allocations in high-throughput scenarios
  - Collection expressions (C# 12) for cleaner initialization
- Async optimization:
  - Avoid async over sync (don't make synchronous operations async)
  - Use `IAsyncEnumerable<T>` for streaming large datasets
- Database performance:
  - Use compiled queries in Entity Framework Core
  - Implement pagination for large result sets
  - Use `AsNoTracking()` for read-only queries
  - Consider caching strategies (memory, distributed)
- Profile before optimizing - measure actual bottlenecks

## Security Best Practices

- Input validation:
  - Validate all user inputs at API boundaries
  - Use data annotations and FluentValidation
  - Sanitize inputs to prevent injection attacks
- Use parameterized queries/ORM to prevent SQL injection
- Authentication & Authorization:
  - Use ASP.NET Core Identity or IdentityServer/Duende
  - Implement role-based (RBAC) or policy-based authorization
  - Never roll your own crypto or authentication
- Secure data handling:
  - Encrypt sensitive data at rest and in transit (TLS 1.2+)
  - Use Data Protection API for encryption/decryption
  - Hash passwords with bcrypt, Argon2, or PBKDF2
  - Never log sensitive information
- Dependency security:
  - Regularly update NuGet packages
  - Use tools like OWASP Dependency-Check or GitHub Dependabot
  - Review security advisories
- Follow OWASP Top 10 guidelines
- Implement rate limiting and throttling for APIs
- Use HTTPS everywhere (HSTS headers)

## Code Quality & Maintainability

- SOLID principles:
  - Single Responsibility: Classes should have one reason to change
  - Open/Closed: Open for extension, closed for modification
  - Liskov Substitution: Derived classes must be substitutable for base classes
  - Interface Segregation: Many specific interfaces over one general interface
  - Dependency Inversion: Depend on abstractions, not concretions
- Code organization:
  - Keep methods short (< 20-30 lines ideally)
  - Limit class size (< 300-400 lines as a guideline)
  - Use meaningful names that express intent
  - Avoid deep nesting (> 3-4 levels)
  - Extract complex conditions into well-named methods
- Avoid code smells:
  - Duplicate code (DRY principle)
  - Large classes/methods (God objects)
  - Long parameter lists (use objects)
  - Feature envy (method using another class's data more than its own)
- Use static analysis tools:
  - Enable nullable reference types
  - Configure .editorconfig for consistent style
  - Use analyzers: StyleCop, Roslynator, SonarAnalyzer
  - Treat warnings as errors in CI/CD pipelines
- Code review practices:
  - Review for logic correctness first
  - Check for security vulnerabilities
  - Ensure test coverage
  - Verify adherence to these standards

## Modern .NET 8+ Features to Leverage

- Use required properties for mandatory initialization: `public required string Name { get; init; }`
- Leverage collection expressions: `int[] numbers = [1, 2, 3, 4, 5];`
- Use primary constructors for classes with simple initialization
- Take advantage of improved pattern matching and switch expressions
- Use file-scoped namespaces to reduce indentation
- Implement interceptors for advanced scenarios (preview feature)
- Use `TimeProvider` for testable time-dependent code
- Leverage frozen collections for immutable, high-performance lookups
- Consider native AOT compilation for startup-critical applications