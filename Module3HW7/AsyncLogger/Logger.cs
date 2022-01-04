using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncLogger.Abstract;
using AsyncLogger.Helper;
using AsyncLogger.Services.Abstract;

namespace AsyncLogger
{
    public class Logger : ILogger
    {
        private readonly object _lock = new object();
        private Queue<LogMessage> _messages = new Queue<LogMessage>();
        private IFileService _fileService;
        public Logger(IFileService fileService)
        {
            _fileService = fileService;
        }

        public event Action<string> StartBackup;
        public LoggerConfig LoggerConfig { get; set; }
        public IDisposable LoggerStream { get; set; }
        public async Task Run()
        {
            LogMessage message = null;
            var counter = 0;
            var limit = LoggerConfig.BackupRecordsCount;
            while (true)
            {
                lock (_messages)
                {
                    if (_messages.Count != 0)
                    {
                        message = _messages.Dequeue();
                    }
                }

                if (message != null)
                {
                    await InternalLogInfo(message.LogType, message.Message);
                    message = null;
                    counter++;
                    if (counter >= limit)
                    {
                        await _fileService.CloseFile(LoggerStream);
                        var content = await _fileService.ReadAllFile(LoggerConfig.LogDirectory, LoggerConfig.FileName);
                        await Task.Run(() => StartBackup?.Invoke(content));
                        LoggerStream = _fileService.CreateFile(LoggerConfig.LogDirectory, LoggerConfig.FileName);
                        counter = 0;
                    }
                }
            }
        }

        public async Task LogInfo(LogType type, string message)
        {
            await Task.Run(() =>
            {
                var newMessage = new LogMessage { LogType = type, Message = message };
                lock (_messages)
                {
                    _messages.Enqueue(newMessage);
                }
            });
        }

        private async Task InternalLogInfo(LogType type, string message)
        {
            string report = $"{DateTime.UtcNow.ToString()} : {type.ToString()} : {message}";
            Console.WriteLine(report);
            await _fileService.WriteToFile(LoggerStream, report);
        }
    }
}