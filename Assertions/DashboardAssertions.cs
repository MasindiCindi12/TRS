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
        public static void AssertStatsAreWellFormed(DashboardStats stats)
        {
            Assert.That(stats.TotalUsers, Is.GreaterThan(0), "Expected Total Users to be a positive number.");
            Assert.That(stats.TotalHobbies, Is.GreaterThanOrEqualTo(0), "Expected Total Hobbies to be a non-negative number.");
        }
    }
}
