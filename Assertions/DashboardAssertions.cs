using AventStack.ExtentReports;
using TRS.Web.Automation.Models;

namespace TRS.Web.Automation.Assertions
{
    internal static class DashboardAssertions
    {
        // This is a public demo environment shared with other concurrent users, so global
        // totals can move in either direction between the two reads independently of this
        // test's own actions. We only sanity-check that the dashboard renders plausible,
        // non-negative figures — the specific outcomes of this test's own actions (person
        // added, hobbies added/linked) are verified precisely elsewhere via test-owned data.
        public static void AssertStatsAreWellFormed(ExtentTest test, DashboardStats stats)
        {
            test.Info("Expected: Dashboard should render well-formed, non-negative statistics.");
            test.Info($"Actual: Total Users: {stats.TotalUsers}, Total Hobbies: {stats.TotalHobbies}.");

            Assert.That(stats.TotalUsers, Is.GreaterThan(0), "Expected Total Users to be a positive number.");
            Assert.That(stats.TotalHobbies, Is.GreaterThanOrEqualTo(0), "Expected Total Hobbies to be a non-negative number.");
        }

        // Same shared-environment caveat as above: other concurrent users could also add hobbies of this
        // type between the two reads, so we only assert the count moved by at least the one we added.
        public static void AssertHobbyDistributionCountIncreased(ExtentTest test, string hobbyType, int countBefore, int countAfter)
        {
            test.Info($"Expected: The Hobby Distribution chart's '{hobbyType}' count should increase by at least 1 after adding a new '{hobbyType}' hobby.");
            test.Info($"Actual: '{hobbyType}' count was {countBefore} before, {countAfter} after.");

            Assert.That(countAfter, Is.GreaterThanOrEqualTo(countBefore + 1),
                $"Expected the '{hobbyType}' segment count to increase by at least 1 (from {countBefore}), but it was {countAfter}.");
        }
    }
}
