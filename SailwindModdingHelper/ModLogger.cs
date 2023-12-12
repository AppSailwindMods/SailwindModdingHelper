namespace SailwindModdingHelper
{
    public static class ModLogger
    {
        public static event LogEventHandler OnLog;


        private static void Log(LogType logType, string modId, string message)
        {
            OnLog?.Invoke(logType, modId, message);
        }

        public static void Log(string modId, string message)
        {
            Log(LogType.Log, modId, message);
        }

        public static void Error(string modId, string message)
        {
            Log(LogType.Error, modId, message);
        }

        public delegate void LogEventHandler(LogType logType, string modId, string message);

    }
    public enum LogType
    {
        Log,
        Error
    }
}
