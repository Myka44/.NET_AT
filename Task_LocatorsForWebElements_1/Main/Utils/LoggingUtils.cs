using log4net;
using log4net.Config;
using log4net.Core;
using log4net.Repository.Hierarchy;
namespace TestLayer.Utils
{
    public static class LoggingUtils
    {
        private static readonly object ConfigurationLock = new();
        private static bool _isConfigured;

        public static void Configure(string minLevel)
        {
            lock (ConfigurationLock)
            {
                if (!_isConfigured)
                {
                    XmlConfigurator.Configure(
                        new FileInfo(Path.Combine(AppContext.BaseDirectory, "Log.config")));

                    _isConfigured = true;
                }

                var hierarchy = (Hierarchy)LogManager.GetRepository();
                var level = hierarchy.LevelMap[minLevel.ToUpperInvariant()] ?? Level.Info;

                hierarchy.Root.Level = level;
                hierarchy.RaiseConfigurationChanged(EventArgs.Empty);
            }
        }
    }
}
