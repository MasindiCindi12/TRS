using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace TRS.Web.Automation.Utilities
{
    internal static class ExtentReportManager
    {
        private static readonly Lazy<ExtentReports> LazyExtent = new(Create);

        public static ExtentReports Instance => LazyExtent.Value;

        public static void Flush() => LazyExtent.Value.Flush();

        private static ExtentReports Create()
        {
            var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
            var reportsDir = Path.Combine(projectRoot, "Reports");
            Directory.CreateDirectory(reportsDir);

            var reportPath = Path.Combine(reportsDir, $"TestReport_{DateTime.Now:yyyyMMdd_HHmmss}.html");
            var htmlReporter = new ExtentSparkReporter(reportPath);

            var extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
            return extent;
        }
    }
}
