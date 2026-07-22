using System.Collections.Concurrent;
using OpenQA.Selenium;

namespace TRS.Web.Automation.Utilities
{
    internal sealed class NetworkCapture
    {
        private readonly INetwork _network;
        private readonly ConcurrentBag<(string Url, long StatusCode)> _responses = new();

        public NetworkCapture(IWebDriver driver)
        {
            _network = driver.Manage().Network;
            _network.NetworkResponseReceived += OnResponseReceived;
        }

        public Task StartAsync() => _network.StartMonitoring();

        public async Task StopAsync()
        {
            await _network.StopMonitoring();
            _network.NetworkResponseReceived -= OnResponseReceived;
        }

        public long? FindStatusCode(string urlSubstring) =>
            _responses
                .Where(r => r.Url.Contains(urlSubstring, StringComparison.OrdinalIgnoreCase))
                .Select(r => (long?)r.StatusCode)
                .LastOrDefault();

        public async Task<long?> WaitForStatusCodeAsync(string urlSubstring, TimeSpan timeout)
        {
            var deadline = DateTime.UtcNow + timeout;
            long? statusCode;
            while ((statusCode = FindStatusCode(urlSubstring)) is null && DateTime.UtcNow < deadline)
            {
                await Task.Delay(200);
            }
            return statusCode;
        }

        private void OnResponseReceived(object? sender, NetworkResponseReceivedEventArgs e)
        {
            if (e.ResponseUrl is not null)
                _responses.Add((e.ResponseUrl, e.ResponseStatusCode));
        }
    }
}
