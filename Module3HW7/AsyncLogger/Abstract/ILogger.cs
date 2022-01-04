using System;
using System.Threading.Tasks;
using AsyncLogger.Helper;

namespace AsyncLogger.Abstract
{
    public interface ILogger
    {
        event Action<string> StartBackup;
        LoggerConfig LoggerConfig { get; set; }
        IDisposable LoggerStream { get; set; }
        Task LogInfo(LogType logType, string message);
        Task Run();
    }
}