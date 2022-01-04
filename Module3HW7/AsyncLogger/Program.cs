using System;
using AsyncLogger.Abstract;
using AsyncLogger.Services;
using AsyncLogger.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace AsyncLogger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfigService, ConfigService>()
                .AddSingleton<ILogger, Logger>()
                .AddTransient<IFileService, FileService>()
                .AddTransient<IBackupService, BackupService>()
                .AddTransient<Starter>()
                .BuildServiceProvider();

            var start = serviceProvider.GetService<Starter>();
            start?.Run().GetAwaiter().GetResult();
        }
    }
}