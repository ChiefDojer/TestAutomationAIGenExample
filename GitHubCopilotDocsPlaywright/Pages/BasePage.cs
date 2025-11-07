using GitHubCopilotDocs.Tests.Core.Logging;
using Microsoft.Playwright;

namespace GitHubCopilotDocs.Tests.Pages;

/// <summary>
/// Base class for all Page Object Model classes.
/// Provides common utilities for interacting with web elements.
/// </summary>
public abstract class BasePage
{
    protected IPage Page { get; }
    protected TestLogger Logger { get; }

    protected BasePage(IPage page, TestLogger logger)
    {
        Page = page;
        Logger = logger;
    }

    /// <summary>
    /// Clicks an element identified by selector
    /// </summary>
    protected async Task ClickAsync(string selector)
    {
        Logger.Debug($"Clicking element: {selector}");
        await Page.Locator(selector).ClickAsync();
    }

    /// <summary>
    /// Types text into an input field
    /// </summary>
    protected async Task TypeAsync(string selector, string text)
    {
        Logger.Debug($"Typing into {selector}: {text}");
        await Page.Locator(selector).FillAsync(text);
    }

    /// <summary>
    /// Gets text content of an element
    /// </summary>
    protected async Task<string> GetTextAsync(string selector)
    {
        Logger.Debug($"Getting text from: {selector}");
        var text = await Page.Locator(selector).TextContentAsync();
        return text ?? string.Empty;
    }

    /// <summary>
    /// Gets the inner text of an element
    /// </summary>
    protected async Task<string> GetInnerTextAsync(string selector)
    {
        Logger.Debug($"Getting inner text from: {selector}");
        var text = await Page.Locator(selector).InnerTextAsync();
        return text ?? string.Empty;
    }

    /// <summary>
    /// Checks if an element is visible
    /// </summary>
    protected async Task<bool> IsVisibleAsync(string selector)
    {
        Logger.Debug($"Checking visibility: {selector}");
        return await Page.Locator(selector).IsVisibleAsync();
    }

    /// <summary>
    /// Checks if an element is enabled
    /// </summary>
    protected async Task<bool> IsEnabledAsync(string selector)
    {
        Logger.Debug($"Checking if enabled: {selector}");
        return await Page.Locator(selector).IsEnabledAsync();
    }

    /// <summary>
    /// Waits for an element to be visible
    /// </summary>
    protected async Task WaitForVisibleAsync(string selector, int? timeout = null)
    {
        Logger.Debug($"Waiting for element to be visible: {selector}");
        var options = timeout.HasValue ? new LocatorWaitForOptions { Timeout = timeout.Value } : null;
        await Page.Locator(selector).WaitForAsync(options);
    }

    /// <summary>
    /// Waits for an element to be hidden
    /// </summary>
    protected async Task WaitForHiddenAsync(string selector, int? timeout = null)
    {
        Logger.Debug($"Waiting for element to be hidden: {selector}");
        var options = new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Hidden,
            Timeout = timeout
        };
        await Page.Locator(selector).WaitForAsync(options);
    }

    /// <summary>
    /// Gets the count of elements matching selector
    /// </summary>
    protected async Task<int> GetCountAsync(string selector)
    {
        Logger.Debug($"Counting elements: {selector}");
        return await Page.Locator(selector).CountAsync();
    }

    /// <summary>
    /// Gets attribute value of an element
    /// </summary>
    protected async Task<string?> GetAttributeAsync(string selector, string attributeName)
    {
        Logger.Debug($"Getting attribute '{attributeName}' from: {selector}");
        return await Page.Locator(selector).GetAttributeAsync(attributeName);
    }

    /// <summary>
    /// Presses a keyboard key
    /// </summary>
    protected async Task PressKeyAsync(string key)
    {
        Logger.Debug($"Pressing key: {key}");
        await Page.Keyboard.PressAsync(key);
    }

    /// <summary>
    /// Scrolls an element into view
    /// </summary>
    protected async Task ScrollIntoViewAsync(string selector)
    {
        Logger.Debug($"Scrolling element into view: {selector}");
        await Page.Locator(selector).ScrollIntoViewIfNeededAsync();
    }

    /// <summary>
    /// Selects an option from a dropdown
    /// </summary>
    protected async Task SelectOptionAsync(string selector, string value)
    {
        Logger.Debug($"Selecting option '{value}' from: {selector}");
        await Page.Locator(selector).SelectOptionAsync(value);
    }

    /// <summary>
    /// Checks a checkbox or radio button
    /// </summary>
    protected async Task CheckAsync(string selector)
    {
        Logger.Debug($"Checking element: {selector}");
        await Page.Locator(selector).CheckAsync();
    }

    /// <summary>
    /// Unchecks a checkbox
    /// </summary>
    protected async Task UncheckAsync(string selector)
    {
        Logger.Debug($"Unchecking element: {selector}");
        await Page.Locator(selector).UncheckAsync();
    }

    /// <summary>
    /// Hovers over an element
    /// </summary>
    protected async Task HoverAsync(string selector)
    {
        Logger.Debug($"Hovering over element: {selector}");
        await Page.Locator(selector).HoverAsync();
    }

    /// <summary>
    /// Gets the current page URL
    /// </summary>
    protected string GetCurrentUrl()
    {
        var url = Page.Url;
        Logger.Debug($"Current URL: {url}");
        return url;
    }

    /// <summary>
    /// Gets the page title
    /// </summary>
    protected async Task<string> GetTitleAsync()
    {
        var title = await Page.TitleAsync();
        Logger.Debug($"Page title: {title}");
        return title;
    }

    /// <summary>
    /// Waits for page navigation to complete
    /// </summary>
    protected async Task WaitForNavigationAsync(Func<Task> action)
    {
        Logger.Debug("Waiting for navigation...");
        await Page.RunAndWaitForNavigationAsync(action);
    }

    /// <summary>
    /// Takes a screenshot of the current page
    /// </summary>
    protected async Task<byte[]> ScreenshotAsync(string? path = null)
    {
        Logger.Debug($"Taking screenshot{(path != null ? $": {path}" : "")}");
        var options = new PageScreenshotOptions { FullPage = true };
        if (path != null)
            options.Path = path;
        return await Page.ScreenshotAsync(options);
    }
}
