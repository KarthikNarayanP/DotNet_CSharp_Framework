using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReqNollSeleniumTest.Utilities;

namespace ReqNollSeleniumTest.Pages
{
    public class SeleniumPage
    {
        private readonly WebdriverFactory _webdriverFactory;
        private readonly ExtentTest _test;

        public SeleniumPage(WebdriverFactory webdriverFactory, ExtentTest test)
        {
            _webdriverFactory = webdriverFactory;
            _test = test;
        }

         private By SearchBarLink => By.XPath("//*[@id='docsearch-1']/button");
        private By SearchBarLocator => By.CssSelector("input[type='search']");
        private By NavigationLinksLocator => By.CssSelector("nav a");
        private By SearchResultsLocator => By.XPath("//*[contains(@class,'DocSearch-Hit-title')][1]");

        public string GetPageTitle()
        {
            string title = _webdriverFactory.GetDriver().Title;
            _test.Log(Status.Info, "Page title retrieved: " + title);
            return title;
        }

        public List<string> GetNavigationLinks()
        {
            var elements = _webdriverFactory.GetDriver().FindElements(NavigationLinksLocator);
            var links = elements.Select(link => link.Text).ToList();
            _test.Log(Status.Info, "Navigation links retrieved: " + string.Join(", ", links));
            return links;
        }

        public void ClickNavigationLink(string linkText)
        {
            var elements = _webdriverFactory.GetDriver().FindElements(NavigationLinksLocator);
            foreach (var element in elements)
            {
                if (element.Text.Equals(linkText, StringComparison.OrdinalIgnoreCase))
                {
                    _webdriverFactory.ClickElement(element, linkText);
                    _test.Log(Status.Pass, $"Clicked on navigation link: {linkText}");
                    return;
                }
            }
            throw new NoSuchElementException($"Navigation link '{linkText}' not found.");
        }

        public void EnterSearchQuery(string query)
        {
            _webdriverFactory.ClickByLocator(SearchBarLink, "Search Bar");
            _webdriverFactory.EnterTextSlowly(SearchBarLocator, query, "Search Bar");
            // _webdriverFactory.GetDriver().FindElement(SearchBarLocator).SendKeys(Keys.Enter);
            _test.Log(Status.Info, $"Entered search query: {query}");
        }

        public string GetSearchResults()
        {
            string results = _webdriverFactory.GetDriver().FindElement(SearchResultsLocator).Text;
            _test.Log(Status.Info, "Search results retrieved: " + results);
            return results;
        }
    }
}