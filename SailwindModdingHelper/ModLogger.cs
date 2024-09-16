using BepInEx;
using BepInEx.Logging;

namespace SailwindModdingHelper
{
    public static class ModLogger
    {
        public static event LogEventHandler OnLog;


        public static void Log(LogLevel logLevel, PluginInfo pluginInfo, object message)
        {
            OnLog?.Invoke(logLevel, pluginInfo, message);
        }

        public static void LogFatal(this PluginInfo pluginInfo, object data)
        {
            Log(LogLevel.Fatal, pluginInfo, data);
        }

        public static void LogError(this PluginInfo pluginInfo, object data)
        {
            Log(LogLevel.Error, pluginInfo, data);
        }

        public static void LogWarning(this PluginInfo pluginInfo, object data)
        {
            Log(LogLevel.Warning, pluginInfo, data);
        }

        public static void LogMessage(this PluginInfo pluginInfo, object data)
        {
            Log(LogLevel.Message, pluginInfo, data);
        }

        public static void LogInfo(this PluginInfo pluginInfo, object data)
        {
            Log(LogLevel.Info, pluginInfo, data);
        }

        public static void LogDebug(this PluginInfo pluginInfo, object data)
        {
            Log(LogLevel.Debug, pluginInfo, data);
        }

        public delegate void LogEventHandler(LogLevel logType, PluginInfo pluginInfo, object message);

    }
}
