using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Reqnroll;

namespace ReqNollSeleniumTest.Utilities
{
    public class WebdriverFactory
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly ExtentTest _test;
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public WebdriverFactory(ScenarioContext scenarioContext, ExtentTest test)
        {
            _scenarioContext = scenarioContext;
            _test = test;

            if (!_scenarioContext.TryGetValue("WebDriver", out object? driverObj) || driverObj is not IWebDriver driver)
            {
                throw new Exception("WebDriver is not initialized in ScenarioContext.");
            }

            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20)); // ✅ Explicit wait setup
        }

        public IWebDriver GetDriver() => _driver;

        // ✅ Click Element
        public void ClickElement(IWebElement element , string description)
        {
            try
            {
                 element.Click();
                _test.Log(Status.Pass, $"Clicked on {description}");
            }
            catch (Exception e)
            {
                _test.Log(Status.Fail, $"Failed to click {description}: {e.Message}");
                throw;
            }
        }

        public void ClickByLocator(By locator, string description)
        {
            try
            {
                _wait.Until(driver => driver.FindElement(locator)).Click();
                _test.Log(Status.Pass, $"Clicked on {description}");
            }
            catch (Exception e)
            {
                _test.Log(Status.Fail, $"Failed to click {description}: {e.Message}");
                throw;
            }
        }

        // ✅ Enter Text
        public void EnterText(By locator, string text, string description)
        {
            try
            {
                var element = _wait.Until(driver => driver.FindElement(locator));
                element.Clear();
                element.SendKeys(text);
                _test.Log(Status.Pass, $"Entered '{text}' in {description}");
            }
            catch (Exception e)
            {
                _test.Log(Status.Fail, $"Failed to enter text in {description}: {e.Message}");
                throw;
            }
        }

        public void EnterTextSlowly(By locator, string text, string description)
            {
                try
                {
                    var element = _wait.Until(driver => driver.FindElement(locator));
                    element.Clear();
                    
                    foreach (char c in text)
                    {
                        element.SendKeys(c.ToString()); // Sending one character at a time
                        Thread.Sleep(100); // Adding a slight delay to mimic typing speed
                    }

                    _test.Log(Status.Pass, $"Typed '{text}' in {description} one character at a time");
                }
                catch (Exception e)
                {
                    _test.Log(Status.Fail, $"Failed to type text in {description}: {e.Message}");
                    throw;
                }
            }

        // ✅ Get Text
        public string GetText(By locator, string description)
        {
            try
            {
                string text = _wait.Until(driver => driver.FindElement(locator)).Text;
                _test.Log(Status.Info, $"Retrieved text from {description}: {text}");
                return text;
            }
            catch (Exception e)
            {
                _test.Log(Status.Fail, $"Failed to retrieve text from {description}: {e.Message}");
                throw;
            }
        }

        // ✅ Assert Attribute Value
        public void AssertAttributeValue(By locator, string attribute, string expectedValue, string description)
        {
            try
            {
                string actualValue = _driver.FindElement(locator).GetAttribute(attribute);
                Assert.That(actualValue, Is.EqualTo(expectedValue), $"Expected '{expectedValue}' but found '{actualValue}' for {description}");
                _test.Log(Status.Pass, $"Verified attribute '{attribute}' of {description} successfully.");
            }
            catch (Exception e)
            {
                _test.Log(Status.Fail, $"Failed to verify attribute value of {description}: {e.Message}");
                throw;
            }
        }

        // ✅ Assert InnerText
        public void AssertInnerText(By locator, string expectedText, string description)
        {
            try
            {
                string actualText = _driver.FindElement(locator).Text.Trim();
                Assert.That(actualText, Is.EqualTo(expectedText), $"Expected '{expectedText}' but found '{actualText}' for {description}");
                _test.Log(Status.Pass, $"Verified inner text of {description} successfully.");
            }
            catch (Exception e)
            {
                _test.Log(Status.Fail, $"Failed to verify inner text of {description}: {e.Message}");
                throw;
            }
        }

        // ✅ Wait for Element Text to Change
        public void WaitForText(By locator, string expectedText, string description)
        {
            try
            {
                _wait.Until(driver => driver.FindElement(locator).Text.Contains(expectedText));
                _test.Log(Status.Pass, $"{description} contains expected text: {expectedText}");
            }
            catch (Exception e)
            {
                _test.Log(Status.Fail, $"{description} does not contain expected text: {expectedText}. Error: {e.Message}");
                throw;
            }
        }
         public void AssertAttributeContains(By locator, string attribute, string expectedValue, string description)
        {
            try
            {
                string actualValue = _driver.FindElement(locator).GetAttribute(attribute);
                Assert.That(actualValue, Does.Contain(expectedValue), 
                    $"Expected '{expectedValue}' to be part of '{actualValue}' for {description}");
                _test.Log(Status.Pass, $"Verified attribute '{attribute}' of {description} contains '{expectedValue}'");
            }
            catch (Exception e)
            {
                _test.Log(Status.Fail, $"Failed to verify attribute value of {description}: {e.Message}");
                throw;
            }
        }

        // ✅ Assert InnerText Contains Specific Text
        public void AssertInnerTextContains(By locator, string expectedText, string description)
        {
            try
            {
                string actualText = _driver.FindElement(locator).Text.Trim();
                Assert.That(actualText, Does.Contain(expectedText), 
                    $"Expected '{expectedText}' to be part of '{actualText}' for {description}");
                _test.Log(Status.Pass, $"Verified inner text of {description} contains '{expectedText}'");
            }
            catch (Exception e)
            {
                _test.Log(Status.Fail, $"Failed to verify inner text of {description}: {e.Message}");
                throw;
            }
        }

        // ✅ Wait for Attribute Value to Change
        public void WaitForAttributeValue(By locator, string attribute, string expectedValue, string description)
        {
            try
            {
                _wait.Until(driver => driver.FindElement(locator).GetAttribute(attribute) == expectedValue);
                _test.Log(Status.Pass, $"{description} has expected attribute value: {expectedValue}");
            }
            catch (Exception e)
            {
                _test.Log(Status.Fail, $"{description} does not have expected attribute value: {expectedValue}. Error: {e.Message}");
                throw;
            }
        }

        // ✅ Wait for Specific URL
        public void WaitForUrl(string expectedUrl)
        {
            try
            {
                _wait.Until(driver => driver.Url.Contains(expectedUrl));
                _test.Log(Status.Pass, $"Navigated to expected URL: {expectedUrl}");
            }
            catch (Exception e)
            {
                _test.Log(Status.Fail, $"Failed to navigate to expected URL: {expectedUrl}. Error: {e.Message}");
                throw;
            }
        }

        // ✅ Get Count of Elements
        public int GetElementCount(By locator, string description)
        {
            try
            {
                int count = _driver.FindElements(locator).Count;
                _test.Log(Status.Info, $"Found {count} elements for {description}");
                return count;
            }
            catch (Exception e)
            {
                _test.Log(Status.Fail, $"Failed to count elements for {description}: {e.Message}");
                throw;
            }
        }

        // ✅ Switch to Frame
        public void SwitchToFrame(By locator)
        {
            try
            {
                _driver.SwitchTo().Frame(_driver.FindElement(locator));
                _test.Log(Status.Info, "Switched to frame");
            }
            catch (Exception e)
            {
                _test.Log(Status.Fail, "Failed to switch to frame: " + e.Message);
                throw;
            }
        }

        // ✅ Switch to Default Content
        public void SwitchToDefaultContent()
        {
            _driver.SwitchTo().DefaultContent();
            _test.Log(Status.Info, "Switched to default content");
        }

        // ✅ Switch to Latest Window
        public void SwitchToLatestWindow()
        {
            _driver.SwitchTo().Window(_driver.WindowHandles.Last());
            _test.Log(Status.Info, "Switched to latest window");
        }

        // ✅ Scroll to Element
        public void ScrollToElement(By locator)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            IWebElement element = _driver.FindElement(locator);
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
            _test.Log(Status.Info, "Scrolled to element");
        }

        // ✅ JavaScript Click
        public void ClickElementJS(By locator, string description)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            IWebElement element = _driver.FindElement(locator);
            js.ExecuteScript("arguments[0].click();", element);
            _test.Log(Status.Info, $"Clicked on {description} using JavaScript");
        }

        // ✅ Capture Screenshot
        public string CaptureScreenshot(string stepName)
        {
            string screenshotDir = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "Screenshots");
            Directory.CreateDirectory(screenshotDir);
            string filePath = Path.Combine(screenshotDir, $"{stepName}.png");

            Screenshot screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            screenshot.SaveAsFile(filePath);
            return filePath;
        }

        // ✅ Close Browser
        public void CloseBrowser()
        {
            _driver.Quit();
            _test.Log(Status.Info, "Browser closed");
        }
    }
}