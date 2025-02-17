using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using ReqNollSeleniumTest.Pages;
using ReqNollSeleniumTest.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReqNollSeleniumTest.StepDefinitions
{
    [Binding]
    [Parallelizable(ParallelScope.Self)] // Runs each scenario independently
    public class SeleniumWebsiteSteps
    {
        private readonly WebdriverFactory _webdriverFactory;
        private readonly SeleniumPage _seleniumPage;
        private readonly ExtentTest _test;

        public SeleniumWebsiteSteps(ScenarioContext scenarioContext)
        {
            if (!scenarioContext.TryGetValue("WebdriverFactory", out _webdriverFactory))
            {
                throw new Exception("WebdriverFactory is not initialized in ScenarioContext.");
            }
            
            if (!scenarioContext.TryGetValue("ExtentTest", out _test))
            {
                throw new Exception("ExtentTest is not initialized in ScenarioContext.");
            }
             _seleniumPage = new SeleniumPage(_webdriverFactory, _test);
        }

        [Given("I navigate to {string}")]
        public void GivenINavigateTo(string url)
        {
            _webdriverFactory.GetDriver().Navigate().GoToUrl(url);
            Console.WriteLine($"Navigating to {url}");
            _test.Log(Status.Info, $"Navigated to {url}");
        }

        [Then("the page title should be {string}")]
        public void ThenThePageTitleShouldBe(string expectedTitle)
        {
            string actualTitle = _seleniumPage.GetPageTitle();
            Assert.That(actualTitle, Does.Contain(expectedTitle), $"Expected '{expectedTitle}' but found '{actualTitle}'");
            _test.Log(Status.Pass, $"Page title validated successfully: {actualTitle}");
        }

        [When("I click on {string}")]
        public void WhenIClickOn(string linkText)
        {
            _seleniumPage.ClickNavigationLink(linkText);
            _test.Log(Status.Info, $"Clicked on '{linkText}'");
        }

        [When("I enter {string} in the search bar")]
        public void WhenIEnterInTheSearchBar(string searchText)
        {
            _seleniumPage.EnterSearchQuery(searchText);
            _test.Log(Status.Info, $"Entered '{searchText}' in the search bar");
        }

        [Then("the search results should contain {string}")]
        public void ThenTheSearchResultsShouldContain(string expectedText)
        {
            string searchResults = _seleniumPage.GetSearchResults();
            Assert.That(searchResults, Does.Contain(expectedText), $"Search results do not contain '{expectedText}'");
            _test.Log(Status.Pass, "Search results validated successfully.");
        }

        [Then("the following navigation links should be present:")]
        public void ThenTheFollowingNavigationLinksShouldBePresent(Table table)
        {
            List<string> expectedLinks = table.Rows.Select(row => row["Link Text"]).ToList();
            List<string> actualLinks = _seleniumPage.GetNavigationLinks();

            Assert.That(actualLinks, Is.SupersetOf(expectedLinks), "Some navigation links are missing.");
            _test.Log(Status.Pass, "Navigation links validated successfully.");
        }
    }
}