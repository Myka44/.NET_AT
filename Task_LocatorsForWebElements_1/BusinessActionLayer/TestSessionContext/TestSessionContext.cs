using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessActionLayer.Configuration;
using TestFramework.Core.BrowserUtils;
using TestProject.Configuration;
using TestProject.PageObjects;

namespace TestLayer.SessionContext
{
    public class TestSessionContext
    {
        public TestUISettings Settings => UiTestConfig.Settings;
        public CustomWebDriver Driver { get; set; } = null!;
        public ScreenshotMaker ScreenshotMaker { get; set; } = null!;
        public MainPage MainPage { get; set; } = null!;
        public string DownloadDirectory { get; set; } = string.Empty;
    }
}
