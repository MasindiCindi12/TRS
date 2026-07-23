namespace TRS.Web.Automation.TestData
{
    internal static class TestDataFactory
    {
        public const string DefaultPassword = "TestPass123!";
        public const string InvalidEmail = "invalid.user@example.com";
        public const string InvalidPassword = "IncorrectPassword123";

        public static class HobbyTypes
        {
            public const string Sports = "Sports";
            public const string Music = "Music";
        }

        public static string UniqueEmail(string label)
        {
            var sixDigitSuffix = Math.Abs(Guid.NewGuid().GetHashCode() % 1_000_000).ToString("D6");
            return $"trs.test.{label}.{sixDigitSuffix}@example.com";
        }

        public static string UniqueName(string label)
        {
            var suffix = Guid.NewGuid().ToString("N")[..3];
            return $"{label} {suffix}";
        }
    }
}
