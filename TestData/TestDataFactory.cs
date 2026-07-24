namespace TRS.Web.Automation.TestData
{
    internal static class TestDataFactory
    {
        public const string DefaultPassword = "TestPass123!";
        public const string InvalidEmail = "invalid.user@example.com";
        public const string InvalidPassword = "IncorrectPassword123";

        public static class Passwords
        {
            // Confirmed live against trs-web.vercel.app/auth/register: registration actually requires
            // length >= 8, a number, and a special character, but does NOT require both letter cases —
            // despite the on-page message claiming "a combination of lowercase, numbers, and uppercase".
            public const string MeetsAllRequirements = "Abcdef1!";
            public const string TooShort = "Ab1cde!";
            public const string NoUppercase = "abcdefgh1!";
            public const string NoLowercase = "ABCDEFGH1!";
            public const string NoSpecialCharacter = "Abcdefgh1";
            public const string NoNumber = "Abcdefgh!";
            public const string NoLetterAtAll = "12345678!";
        }

        public static class HobbyTypes
        {
            public const string Sports = "Sports";
            public const string Music = "Music";
        }

        public static class HobbyNames
        {
            public const string Default = "Automation Hobby";
            public const string DefaultEdited = "Automation Hobby Edited";
            public const string EndToEndOwn = "E2E Own Hobby";
            public const string EndToEndLinked = "E2E Linked Hobby";
        }

        public static class Names
        {
            public const string PersonFirstName = "Automation";
            public const string PersonLastName = "Person";

            public const string LinkedPersonFirstName = "Automation";
            public const string LinkedPersonLastName = "Linked";

            public const string SignUpFirstName = "Trs";
            public const string SignUpLastName = "Tester";

            public const string EndToEndFirstName = "Trs";
            public const string EndToEndLastName = "E2E";
        }

        public static class EmailLabels
        {
            public const string Person = "person";
            public const string SignUp = "signup";
            public const string EndToEnd = "e2e";
            public const string LinkedPerson = "linked";
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
