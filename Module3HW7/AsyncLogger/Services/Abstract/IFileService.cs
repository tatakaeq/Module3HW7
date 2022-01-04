using System;
using System.Threading.Tasks;

namespace AsyncLogger.Services.Abstract
{
    public interface IFileService
    {
        Task CloseFile(IDisposable stream);
        IDisposable CreateFile(string directoryPath, string fileName);
        Task<string> ReadAllFile(string directoryPath, string fileName);
        Task WriteToFile(IDisposable stream, string text);
    }
}