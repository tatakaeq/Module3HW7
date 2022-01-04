using System.Threading.Tasks;
using AsyncLogger.Helper;

namespace AsyncLogger.Services.Abstract
{
    public interface IBackupService
    {
        BackupConfig BackupConfig { get; set; }
        string BackupDirectory { get; set; }
        Task Backup(string content);
    }
}