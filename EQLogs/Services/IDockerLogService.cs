using EQLogs.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EQLogs.Services
{
    /// <summary>
    /// Interface for Docker log service operations
    /// </summary>
    public interface IDockerLogService
    {
        Task<List<LogFileInfo>> GetLogFilesAsync();
        Task<string> ReadLogFileAsync(string filePath);

        // New methods for chunked reading
        Task<string> ExecuteCommandAsync(string command, CancellationToken cancellationToken = default);
        Task<string> ReadFileRangeAsync(string filePath, long offset, int length, CancellationToken cancellationToken = default);
    }
}