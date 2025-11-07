using GitHubCopilotDocs.Selenium.Tests.Core.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace GitHubCopilotDocs.Selenium.Tests.Pages;

/// <summary>
/// Base page object providing common functionality for all pages.
/// </summary>
public abstract class BasePage
{
    protected readonly IWebDriver Driver;
    protected readonly WebDriverWait Wait;
    protected readonly TestLogger Logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="BasePage"/> class.
    /// </summary>
    protected BasePage(IWebDriver driver, WebDriverWait wait, TestLogger logger)
    {
        ArgumentNullException.ThrowIfNull(driver);
        ArgumentNullException.ThrowIfNull(wait);
        ArgumentNullException.ThrowIfNull(logger);

        Driver = driver;
        Wait = wait;
        Logger = logger;
    }

    #region Element Interaction

    /// <summary>
    /// Clicks an element identified by the selector.
    /// </summary>
    protected void Click(By selector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        Logger.Debug($"Clicking element: {selector}");
        WaitForVisible(selector).Click();
    }

    /// <summary>
    /// Types text into an input field.
    /// </summary>
    protected void Type(By selector, string text)
    {
        ArgumentNullException.ThrowIfNull(selector);
        ArgumentNullException.ThrowIfNull(text);
        Logger.Debug($"Typing into element: {selector}");
        var element = WaitForVisible(selector);
        element.Clear();
        element.SendKeys(text);
    }

    /// <summary>
    /// Presses a key on an element.
    /// </summary>
    protected void PressKey(By selector, string key)
    {
        ArgumentNullException.ThrowIfNull(selector);
        ArgumentNullException.ThrowIfNull(key);
        Logger.Debug($"Pressing key '{key}' on element: {selector}");
        WaitForVisible(selector).SendKeys(key);
    }

    /// <summary>
    /// Hovers over an element.
    /// </summary>
    protected void Hover(By selector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        Logger.Debug($"Hovering over element: {selector}");
        var element = WaitForVisible(selector);
        var actions = new OpenQA.Selenium.Interactions.Actions(Driver);
        actions.MoveToElement(element).Perform();
    }

    /// <summary>
    /// Scrolls an element into view.
    /// </summary>
    protected void ScrollIntoView(By selector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        Logger.Debug($"Scrolling element into view: {selector}");
        var element = Driver.FindElement(selector);
        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
    }

    #endregion

    #region Element State

    /// <summary>
    /// Checks if an element is visible.
    /// </summary>
    protected bool IsVisible(By selector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        try
        {
            return Driver.FindElement(selector).Displayed;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    /// <summary>
    /// Checks if an element is enabled.
    /// </summary>
    protected bool IsEnabled(By selector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        try
        {
            return Driver.FindElement(selector).Enabled;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the text content of an element.
    /// </summary>
    protected string GetText(By selector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        Logger.Debug($"Getting text from element: {selector}");
        return WaitForVisible(selector).Text;
    }

    /// <summary>
    /// Gets an attribute value from an element.
    /// </summary>
    protected string? GetAttribute(By selector, string attributeName)
    {
        ArgumentNullException.ThrowIfNull(selector);
        ArgumentNullException.ThrowIfNull(attributeName);
        Logger.Debug($"Getting attribute '{attributeName}' from element: {selector}");
        return Driver.FindElement(selector).GetDomAttribute(attributeName);
    }

    /// <summary>
    /// Gets the count of elements matching the selector.
    /// </summary>
    protected int GetCount(By selector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        return Driver.FindElements(selector).Count;
    }

    #endregion

    #region Waiting

    /// <summary>
    /// Waits for an element to be visible.
    /// </summary>
    protected IWebElement WaitForVisible(By selector, int? timeoutMs = null)
    {
        ArgumentNullException.ThrowIfNull(selector);
        Logger.Debug($"Waiting for element to be visible: {selector}");
        
        if (timeoutMs.HasValue)
        {
            var customWait = new WebDriverWait(Driver, TimeSpan.FromMilliseconds(timeoutMs.Value));
            return customWait.Until(ExpectedConditions.ElementIsVisible(selector));
        }
        
        return Wait.Until(ExpectedConditions.ElementIsVisible(selector));
    }

    /// <summary>
    /// Waits for an element to be clickable.
    /// </summary>
    protected IWebElement WaitForClickable(By selector, int? timeoutMs = null)
    {
        ArgumentNullException.ThrowIfNull(selector);
        Logger.Debug($"Waiting for element to be clickable: {selector}");
        
        if (timeoutMs.HasValue)
        {
            var customWait = new WebDriverWait(Driver, TimeSpan.FromMilliseconds(timeoutMs.Value));
            return customWait.Until(ExpectedConditions.ElementToBeClickable(selector));
        }
        
        return Wait.Until(ExpectedConditions.ElementToBeClickable(selector));
    }

    /// <summary>
    /// Waits for an element to be hidden or removed.
    /// </summary>
    protected bool WaitForHidden(By selector, int? timeoutMs = null)
    {
        ArgumentNullException.ThrowIfNull(selector);
        Logger.Debug($"Waiting for element to be hidden: {selector}");
        
        if (timeoutMs.HasValue)
        {
            var customWait = new WebDriverWait(Driver, TimeSpan.FromMilliseconds(timeoutMs.Value));
            return customWait.Until(ExpectedConditions.InvisibilityOfElementLocated(selector));
        }
        
        return Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(selector));
    }

    #endregion

    #region Form Controls

    /// <summary>
    /// Selects an option from a dropdown by visible text.
    /// </summary>
    protected void SelectOption(By selector, string optionText)
    {
        ArgumentNullException.ThrowIfNull(selector);
        ArgumentNullException.ThrowIfNull(optionText);
        Logger.Debug($"Selecting option '{optionText}' from dropdown: {selector}");
        var element = WaitForVisible(selector);
        var select = new SelectElement(element);
        select.SelectByText(optionText);
    }

    /// <summary>
    /// Checks a checkbox or radio button.
    /// </summary>
    protected void Check(By selector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        Logger.Debug($"Checking element: {selector}");
        var element = WaitForVisible(selector);
        if (!element.Selected)
        {
            element.Click();
        }
    }

    /// <summary>
    /// Unchecks a checkbox.
    /// </summary>
    protected void Uncheck(By selector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        Logger.Debug($"Unchecking element: {selector}");
        var element = WaitForVisible(selector);
        if (element.Selected)
        {
            element.Click();
        }
    }

    #endregion

    #region Page Information

    /// <summary>
    /// Gets the current page URL.
    /// </summary>
    public string GetCurrentUrl() => Driver.Url;

    /// <summary>
    /// Gets the page title.
    /// </summary>
    public string GetTitle() => Driver.Title;

    /// <summary>
    /// Takes a screenshot of the current page.
    /// </summary>
    public byte[] TakeScreenshot()
    {
        Logger.Debug("Taking screenshot");
        return ((ITakesScreenshot)Driver).GetScreenshot().AsByteArray;
    }

    #endregion
}
