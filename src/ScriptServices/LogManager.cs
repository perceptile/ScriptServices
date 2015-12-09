using log4net;

namespace ScriptServices
{
    public static class LogManager
    {
        public static ILog Logger
        {
            get { return log4net.LogManager.GetLogger("default"); }
        }
    }
}
