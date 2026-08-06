namespace CoreLayer.WebDriver
{
    public class BrowserSettings
    {
        public string Name { get; set; } = "chrome";
        public int ExplicitWaitSeconds { get; set; } = 15;
        public bool Maximize { get; set; } = true;
        public DownloadSettings Downloads { get; set; } = new();
        public BrowserOptionsSettings Options { get; set; } = new();
    }

    public class DownloadSettings
    {
        public string Directory { get; set; } = "Downloads";
    }

    public class BrowserOptionsSettings
    {
        public List<string> Arguments { get; set; } = new();
        public Dictionary<string, string> Preferences { get; set; } = new();
    }
}
