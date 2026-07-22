using OpenQA.Selenium;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace TRS.Web.Automation.Utilities
{
    // Not thread-safe by design: CaptureFrame must only be called from the same
    // thread that drives the WebDriver session (Selenium sessions do not support
    // concurrent commands from multiple threads).
    public sealed class ScreenRecorder
    {
        private const int FrameDelayCentiseconds = 60;
        private const int MaxFrameWidth = 960;

        private readonly IWebDriver _driver;
        private readonly List<Image<Rgba32>> _frames = new();

        public ScreenRecorder(IWebDriver driver)
        {
            _driver = driver;
        }

        public void CaptureFrame()
        {
            try
            {
                var bytes = ((ITakesScreenshot)_driver).GetScreenshot().AsByteArray;
                var image = Image.Load<Rgba32>(bytes);
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(MaxFrameWidth, 0),
                    Mode = ResizeMode.Max
                }));
                _frames.Add(image);
            }
            catch
            {
                // The page may be mid-navigation; skip this frame rather than fail the recording.
            }
        }

        public async Task<string?> SaveAsync(string outputPath)
        {
            if (_frames.Count == 0)
                return null;

            try
            {
                using var gif = _frames[0].Clone();
                for (var i = 1; i < _frames.Count; i++)
                {
                    gif.Frames.AddFrame(_frames[i].Frames.RootFrame);
                }

                foreach (var frame in gif.Frames)
                {
                    frame.Metadata.GetGifMetadata().FrameDelay = FrameDelayCentiseconds;
                }

                gif.Metadata.GetGifMetadata().RepeatCount = 0;

                Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
                await gif.SaveAsGifAsync(outputPath);
                return outputPath;
            }
            finally
            {
                foreach (var frame in _frames)
                {
                    frame.Dispose();
                }
            }
        }
    }
}
