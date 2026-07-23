using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace TRS.Web.Automation.Utilities
{
    internal static class ExtentReportManager
    {
        private static readonly Lazy<ExtentReports> LazyExtent = new(Create);

        public static ExtentReports Instance => LazyExtent.Value;

        public static string ReportsDirectory { get; } = Path.Combine(
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..")),
            "Reports",
            DateTime.Now.ToString("yyyy-MM-dd"));

        public static void Flush() => LazyExtent.Value.Flush();

        private static ExtentReports Create()
        {
            Directory.CreateDirectory(ReportsDirectory);

            var reportPath = Path.Combine(ReportsDirectory, $"TestReport_{DateTime.Now:yyyyMMdd_HHmmss}.html");
            var htmlReporter = new ExtentSparkReporter(reportPath);

            var extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
            return extent;
        }
    }
}
