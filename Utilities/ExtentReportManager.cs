using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.IO;

namespace ReqNollSeleniumTest.Utilities
{
    public static class ExtentReportManager
    {
        private static readonly ExtentReports _extentReports;
        private static readonly ExtentSparkReporter _sparkReporter;

        static ExtentReportManager()
        {
            var rootDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            var reportPath = Path.Combine(rootDir, "Reports", "ExtentReport.html");
            Directory.CreateDirectory(Path.GetDirectoryName(reportPath) ?? throw new InvalidOperationException("Invalid report path"));

            _sparkReporter = new ExtentSparkReporter(reportPath);
            _sparkReporter.Config.DocumentTitle = "Automation Test Report";
            _sparkReporter.Config.ReportName = "ReqNoll Selenium Test Execution";

            _extentReports = new ExtentReports();
            _extentReports.AttachReporter(_sparkReporter);
        }

        public static ExtentReports GetExtentReports()
        {
            return _extentReports;
        }

        public static void FlushReports()
        {
            _extentReports.Flush();
        }
    }
}