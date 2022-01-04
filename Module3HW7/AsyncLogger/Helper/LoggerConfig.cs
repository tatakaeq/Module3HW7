namespace AsyncLogger.Helper
{
    public class LoggerConfig
    {
        public string LogDirectory { get; set; }
        public string FileName { get; set; }
        public int BackupRecordsCount { get; set; }
    }
}