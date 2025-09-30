using Docker.DotNet;
using Docker.DotNet.Models;
using EQLogs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EQLogs.Services
{
    public class DockerLogService : IDockerLogService
    {
        private readonly DockerClient _dockerClient;
        private const string CONTAINER_NAME = "honeytree-eqemu-server-1";
        private const string LOG_BASE_PATH = "/home/eqemu/server/logs";
        
        public DockerLogService()
        {
            _dockerClient = new DockerClientConfiguration().CreateClient();
        }
        
        public async Task<List<LogFileInfo>> GetLogFilesAsync()
        {
            var logFiles = await GetAvailableLogFilesAsync();
            return logFiles.Select(lf => new LogFileInfo
            {
                FilePath = lf.Path,
                TotalSize = lf.Size,
                EstimatedPacketCount = (int)(lf.Size / 200) // Rough estimate
            }).ToList();
        }

        public async Task<List<LogFile>> GetAvailableLogFilesAsync()
        {
            var logFiles = new List<LogFile>();

            try
            {
                // Get container ID
                var containers = await _dockerClient.Containers.ListContainersAsync(new ContainersListParameters
                {
                    All = true
                });

                var container = containers.FirstOrDefault(c => c.Names.Any(name => name.Contains(CONTAINER_NAME)));
                if (container == null)
                {
                    throw new InvalidOperationException($"Container {CONTAINER_NAME} not found");
                }

                // List files in the logs directory
                await ListDirectoryFiles(container.ID, LOG_BASE_PATH, logFiles, LogType.Login);
                await ListDirectoryFiles(container.ID, $"{LOG_BASE_PATH}/zone", logFiles, LogType.Zone);

                // Remove duplicates based on file path
                var uniqueLogFiles = logFiles
                    .GroupBy(f => f.Path)
                    .Select(g => g.First())
                    .OrderByDescending(f => f.LastModified)
                    .ToList();

                return uniqueLogFiles;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve log files: {ex.Message}", ex);
            }
        }
        
        private async Task ListDirectoryFiles(string containerId, string path, List<LogFile> logFiles, LogType defaultType)
        {
            try
            {
                var execConfig = new ContainerExecCreateParameters
                {
                    Cmd = new[] { "find", path, "-name", "*.log", "-type", "f", "-exec", "stat", "-c", "%n|%s|%Y", "{}", ";" },
                    AttachStdout = true,
                    AttachStderr = true
                };
                
                var execCreateResponse = await _dockerClient.Exec.ExecCreateContainerAsync(containerId, execConfig);
                var execStartResponse = await _dockerClient.Exec.StartAndAttachContainerExecAsync(execCreateResponse.ID, false);
                
                var output = await ReadStreamAsync(execStartResponse);
                var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 3)
                    {
                        var filePath = parts[0];
                        var size = long.Parse(parts[1]);
                        var timestamp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(parts[2])).DateTime;
                        
                        var fileName = Path.GetFileName(filePath);
                        var logType = DetermineLogType(fileName, defaultType);
                        
                        logFiles.Add(new LogFile
                        {
                            Name = fileName,
                            Path = filePath,
                            Size = size,
                            LastModified = timestamp,
                            Type = logType
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Directory might not exist, continue
                Console.WriteLine($"Warning: Could not list files in {path}: {ex.Message}");
            }
        }
        
        private static LogType DetermineLogType(string fileName, LogType defaultType)
        {
            var lower = fileName.ToLower();
            if (lower.Contains("login")) return LogType.Login;
            if (lower.Contains("world")) return LogType.World;
            if (lower.Contains("zone") || defaultType == LogType.Zone) return LogType.Zone;
            return defaultType;
        }
        
        public async Task<string> ReadLogFileAsync(string filePath)
        {
            try
            {
                var containers = await _dockerClient.Containers.ListContainersAsync(new ContainersListParameters
                {
                    All = true
                });
                
                var container = containers.FirstOrDefault(c => c.Names.Any(name => name.Contains(CONTAINER_NAME)));
                if (container == null)
                {
                    throw new InvalidOperationException($"Container {CONTAINER_NAME} not found");
                }
                
                var execConfig = new ContainerExecCreateParameters
                {
                    Cmd = new[] { "cat", filePath },
                    AttachStdout = true,
                    AttachStderr = true
                };
                
                var execCreateResponse = await _dockerClient.Exec.ExecCreateContainerAsync(container.ID, execConfig);
                var execStartResponse = await _dockerClient.Exec.StartAndAttachContainerExecAsync(execCreateResponse.ID, false);
                
                return await ReadStreamAsync(execStartResponse);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to read log file {filePath}: {ex.Message}", ex);
            }
        }
        
        private static async Task<string> ReadStreamAsync(MultiplexedStream stream)
        {
            var output = new StringBuilder();
            var buffer = new byte[4096];
            
            while (true)
            {
                var readResult = await stream.ReadOutputAsync(buffer, 0, buffer.Length, default);
                if (readResult.Count == 0)
                    break;
                    
                output.Append(Encoding.UTF8.GetString(buffer, 0, readResult.Count));
            }
            
            return output.ToString();
        }
        
        // New methods for chunked reading
        public async Task<string> ExecuteCommandAsync(string command, CancellationToken cancellationToken = default)
        {
            try
            {
                var containers = await _dockerClient.Containers.ListContainersAsync(new ContainersListParameters
                {
                    All = true
                });

                var container = containers.FirstOrDefault(c => c.Names.Any(name => name.Contains(CONTAINER_NAME)));
                if (container == null)
                {
                    throw new InvalidOperationException($"Container {CONTAINER_NAME} not found");
                }

                // Split command into parts
                var commandParts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                var execConfig = new ContainerExecCreateParameters
                {
                    Cmd = commandParts,
                    AttachStdout = true,
                    AttachStderr = true
                };

                var execCreateResponse = await _dockerClient.Exec.ExecCreateContainerAsync(container.ID, execConfig);
                var execStartResponse = await _dockerClient.Exec.StartAndAttachContainerExecAsync(execCreateResponse.ID, false);

                return await ReadStreamAsync(execStartResponse);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to execute command '{command}': {ex.Message}", ex);
            }
        }

        public async Task<string> ReadFileRangeAsync(string filePath, long offset, int length, CancellationToken cancellationToken = default)
        {
            var command = $"dd if=\"{filePath}\" bs=1 skip={offset} count={length} 2>/dev/null";
            return await ExecuteCommandAsync(command, cancellationToken);
        }

        public void Dispose()
        {
            _dockerClient?.Dispose();
        }
    }
}