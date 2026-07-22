using System.Text.Json;
using TRS.Web.Automation.Models;

namespace TRS.Web.Automation.Configuration
{
    internal static class AppSettingsProvider
    {
        private static AppSettings? _current;

        public static AppSettings Current => _current ??= Load();

        private static AppSettings Load()
        {
            var settings = new AppSettings();
            MergeFrom(settings, Path.Combine(AppContext.BaseDirectory, "Configuration", "appsettings.json"));
            MergeFrom(settings, Path.Combine(AppContext.BaseDirectory, "Configuration", "appsettings.local.json"));
            return settings;
        }

        private static void MergeFrom(AppSettings settings, string path)
        {
            if (!File.Exists(path))
                return;

            var loaded = JsonSerializer.Deserialize<AppSettings>(
                File.ReadAllText(path),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (loaded is null)
                return;

            if (!string.IsNullOrWhiteSpace(loaded.BaseUrl)) settings.BaseUrl = loaded.BaseUrl;
            if (!string.IsNullOrWhiteSpace(loaded.LoginPath)) settings.LoginPath = loaded.LoginPath;
            if (!string.IsNullOrWhiteSpace(loaded.LoginEmail)) settings.LoginEmail = loaded.LoginEmail;
            if (!string.IsNullOrWhiteSpace(loaded.LoginPassword)) settings.LoginPassword = loaded.LoginPassword;
        }
    }
}
