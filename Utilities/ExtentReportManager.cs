using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.IO;

namespace ReqNollSeleniumTest.Utilities
{
    public static class ExtentReportManager
    {
        private static ExtentReports? _extentReports;
        private static ExtentSparkReporter? _sparkReporter;
        private static readonly object _lock = new(); // ✅ Thread safety

        static ExtentReportManager()
        {
            InitializeReport();
        }

        private static void InitializeReport()
        {
            lock (_lock)
            {
                if (_extentReports == null)
                {
                    string rootDir = GetRootDirectory();
                    if (string.IsNullOrEmpty(rootDir))
                    {
                        throw new InvalidOperationException("Could not determine the project root directory.");
                    }

                    string reportPath = Path.Combine(rootDir, "Reports", "ExtentReport.html");

                    // ✅ Ensure directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(reportPath) ?? throw new InvalidOperationException("Invalid report path"));

                    _sparkReporter = new ExtentSparkReporter(reportPath);
                    _sparkReporter.Config.DocumentTitle = "Automation Test Report";
                    _sparkReporter.Config.ReportName = "ReqNoll Selenium Test Execution";

                    _extentReports = new ExtentReports();
                    _extentReports.AttachReporter(_sparkReporter);
                }
            }
        }

        public static ExtentReports GetExtentReports()
        {
            if (_extentReports == null)
            {
                InitializeReport(); // ✅ Ensures report instance is available
            }
            return _extentReports!;
        }

        public static void FlushReports()
        {
            lock (_lock)
            {
                _extentReports?.Flush(); // ✅ Safe null-check before flushing
            }
        }

        private static string GetRootDirectory()
        {
            string? currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo? rootDir = new DirectoryInfo(currentDir);

            while (rootDir?.Parent != null && !Directory.Exists(Path.Combine(rootDir.FullName, ".git")))
            {
                rootDir = rootDir.Parent;
            }

            return rootDir?.FullName ?? currentDir; // ✅ Ensures it never returns null
        }
    }
}