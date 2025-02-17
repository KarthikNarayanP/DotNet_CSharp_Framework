using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;
using ReqNollSeleniumTest.Utilities;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace ReqNollSeleniumTest.Hooks
{
    [Binding]
    [Parallelizable(ParallelScope.All)] // Enables full parallel execution
    public class TestHooks
    {
        private static readonly ExtentReports _extentReports = ExtentReportManager.GetExtentReports();
        private static readonly ConcurrentDictionary<string, ExtentTest> _featureReports = new();
        private static readonly ConcurrentDictionary<Guid, ExtentTest> _scenarioReports = new();
        private static readonly ThreadLocal<IWebDriver> _driver = new();
        private static readonly object _lock = new();

        [BeforeTestRun]
        public static void SetupExtentReports()
        {
            _extentReports.Flush(); // Ensure fresh report on every test run
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            string featureTitle = featureContext.FeatureInfo.Title;

            lock (_lock)
            {
                if (!_featureReports.ContainsKey(featureTitle))
                {
                    var featureTest = _extentReports.CreateTest(featureTitle);
                    _featureReports[featureTitle] = featureTest;
                }
            }

            featureContext["FeatureTitle"] = featureTitle;
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            try
            {
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArgument("--headless");
                _driver.Value = new ChromeDriver(chromeOptions);

                scenarioContext["WebDriver"] = _driver.Value;
                Console.WriteLine($"WebDriver initialized on Thread: {Thread.CurrentThread.ManagedThreadId}");

                string featureTitle = (string)featureContext["FeatureTitle"];
                var featureTest = _featureReports[featureTitle];

                var scenarioId = Guid.NewGuid();
                scenarioContext["ScenarioId"] = scenarioId;

                ExtentTest scenarioTest;
                lock (_lock) // ✅ Ensure thread safety
                {
                    scenarioTest = featureTest.CreateNode(scenarioContext.ScenarioInfo.Title);
                    _scenarioReports[scenarioId] = scenarioTest;
                }
                
                scenarioContext["ExtentTest"] = scenarioTest; // ✅ Store it properly
                var webdriverFactory = new WebdriverFactory(scenarioContext, scenarioTest);
                scenarioContext["WebdriverFactory"] = webdriverFactory;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing WebDriver: {ex.Message}");
                throw;
            }
        }

        [AfterStep]
        public void AfterEachStep(ScenarioContext scenarioContext)
        {
            var stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            var stepName = scenarioContext.StepContext.StepInfo.Text;
            var scenarioId = (Guid)scenarioContext["ScenarioId"];

            if (!_scenarioReports.TryGetValue(scenarioId, out var scenarioTest))
                return; // Avoid errors if scenario is not tracked

            lock (_lock)
            {
                if (scenarioContext.TestError == null)
                {
                    scenarioTest.Log(Status.Pass, $"{stepType}: {stepName}");
                }
                else
                {
                    scenarioTest.Log(Status.Fail, $"{stepType}: {stepName} - {scenarioContext.TestError.Message}");
                    if (scenarioContext.TryGetValue("WebDriver", out IWebDriver driver))
                    {
                        string screenshotPath = CaptureScreenshot(driver, stepName);
                        scenarioTest.AddScreenCaptureFromPath(screenshotPath);
                    }
                }
            }
        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext scenarioContext)
        {
            if (scenarioContext.TestError != null)
            {
                var scenarioId = (Guid)scenarioContext["ScenarioId"];
                if (_scenarioReports.TryGetValue(scenarioId, out var scenarioTest))
                {
                    lock (_lock)
                    {
                        scenarioTest.Log(Status.Fail, $"Scenario Failed: {scenarioContext.TestError.Message}");
                    }
                }
            }

            if (scenarioContext.TryGetValue("WebDriver", out IWebDriver driver))
            {
                driver.Quit();
                Console.WriteLine($"WebDriver closed on Thread: {Thread.CurrentThread.ManagedThreadId}");
            }
        }

        [AfterTestRun]
        public static void FlushReports()
        {
            lock (_lock)
            {
                _extentReports.Flush();
            }
        }

        private string CaptureScreenshot(IWebDriver driver, string stepName)
        {
            string screenshotDir = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "Screenshots");
            Directory.CreateDirectory(screenshotDir);
            string filePath = Path.Combine(screenshotDir, $"{stepName}.png");

            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(filePath);
            return filePath;
        }
    }
}