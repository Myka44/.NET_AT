using log4net;
using log4net.Config;
using log4net.Core;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLayer.Utils
{
    public static class LoggingUtils
    {
        public static void Configure(string minLevel)
        {
            XmlConfigurator.Configure(new FileInfo(Path.Combine(AppContext.BaseDirectory, "Log.config")));

            var hierarchy = (Hierarchy)LogManager.GetRepository();
            var level = hierarchy.LevelMap[minLevel.ToUpperInvariant()] ?? Level.Info;

            hierarchy.Root.Level = level;
            hierarchy.RaiseConfigurationChanged(EventArgs.Empty);
        }
    }
}
