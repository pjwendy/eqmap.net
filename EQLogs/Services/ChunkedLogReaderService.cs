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
    /// <summary>
    /// Efficiently handles large log files by reading them in chunks and providing virtual access
    /// </summary>
    public class ChunkedLogReaderService : IDisposable
    {
        private readonly IDockerLogService _dockerService;
        private readonly PacketParserService _packetParser;

        // Configuration
        private const int ChunkSize = 1024 * 1024; // 1MB chunks
        private const int MaxCachedChunks = 10; // Maximum chunks to keep in memory
        private const int IndexInterval = 1000; // Index every 1000 packets

        // Current file state
        private string? _currentFilePath;
        private long _totalFileSize;
        private readonly Dictionary<int, LogChunk> _chunkCache = new();
        private readonly LogFileIndex _fileIndex = new();

        // Threading
        private readonly SemaphoreSlim _readSemaphore = new(1, 1);
        private CancellationTokenSource? _cancellationTokenSource;

        public ChunkedLogReaderService(IDockerLogService dockerService, PacketParserService packetParser)
        {
            _dockerService = dockerService;
            _packetParser = packetParser;
        }

        /// <summary>
        /// Initialize the chunked reader for a specific log file
        /// </summary>
        public async Task<LogFileInfo> InitializeAsync(string filePath, CancellationToken cancellationToken = default)
        {
            System.Diagnostics.Debug.WriteLine($"ChunkedLogReaderService: InitializeAsync called for {filePath}");

            await _readSemaphore.WaitAsync(cancellationToken);
            try
            {
                _currentFilePath = filePath;
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource = new CancellationTokenSource();

                // Clear previous state
                _chunkCache.Clear();
                _fileIndex.Clear();

                System.Diagnostics.Debug.WriteLine("ChunkedLogReaderService: Getting file size");
                // Get file size using ls -l instead of loading the whole file
                _totalFileSize = await GetFileSizeAsync(filePath, cancellationToken);
                System.Diagnostics.Debug.WriteLine($"ChunkedLogReaderService: File size = {_totalFileSize} bytes");

                if (_totalFileSize <= 0)
                {
                    System.Diagnostics.Debug.WriteLine("ChunkedLogReaderService: File size is 0 or negative, file may not exist or be accessible");
                }

                // Build index in background
                System.Diagnostics.Debug.WriteLine("ChunkedLogReaderService: Starting background indexing");
                _ = Task.Run(() => BuildIndexAsync(filePath, _cancellationTokenSource.Token), cancellationToken);

                var result = new LogFileInfo
                {
                    FilePath = filePath,
                    TotalSize = _totalFileSize,
                    EstimatedPacketCount = EstimatePacketCount(_totalFileSize)
                };

                System.Diagnostics.Debug.WriteLine($"ChunkedLogReaderService: InitializeAsync completed, estimated {result.EstimatedPacketCount} packets");
                return result;
            }
            finally
            {
                _readSemaphore.Release();
            }
        }

        /// <summary>
        /// Get a range of packets efficiently using virtual scrolling
        /// </summary>
        public async Task<LogPageResult> GetPacketRangeAsync(int startIndex, int count, CancellationToken cancellationToken = default)
        {
            if (_currentFilePath == null)
                throw new InvalidOperationException("No file initialized");

            await _readSemaphore.WaitAsync(cancellationToken);
            try
            {
                var packets = new List<PacketData>();
                var endIndex = Math.Min(startIndex + count, _fileIndex.TotalPacketCount);

                // If we have complete index, use it for fast access
                if (_fileIndex.IsComplete)
                {
                    for (int i = startIndex; i < endIndex; i++)
                    {
                        var packet = await GetPacketByIndexAsync(i, cancellationToken);
                        if (packet != null)
                            packets.Add(packet);
                    }
                }
                else
                {
                    // Fallback: stream through file until we reach desired range
                    packets = await GetPacketRangeStreamingAsync(startIndex, count, cancellationToken);
                }

                return new LogPageResult
                {
                    Packets = packets,
                    StartIndex = startIndex,
                    TotalCount = _fileIndex.TotalPacketCount,
                    IsComplete = _fileIndex.IsComplete
                };
            }
            finally
            {
                _readSemaphore.Release();
            }
        }

        /// <summary>
        /// Search for packets matching criteria efficiently
        /// </summary>
        public async Task<List<PacketSearchResult>> SearchAsync(PacketSearchCriteria criteria, CancellationToken cancellationToken = default)
        {
            var results = new List<PacketSearchResult>();

            if (_fileIndex.IsComplete)
            {
                // Use index for fast searching
                var matchingIndices = _fileIndex.FindPacketIndices(criteria);
                foreach (var index in matchingIndices)
                {
                    var packet = await GetPacketByIndexAsync(index, cancellationToken);
                    if (packet != null && MatchesCriteria(packet, criteria))
                    {
                        results.Add(new PacketSearchResult
                        {
                            PacketIndex = index,
                            Packet = packet,
                            FileOffset = _fileIndex.GetFileOffset(index)
                        });
                    }
                }
            }
            else
            {
                // Stream search through file
                results = await SearchStreamingAsync(criteria, cancellationToken);
            }

            return results;
        }

        private async Task<long> GetFileSizeAsync(string filePath, CancellationToken cancellationToken)
        {
            try
            {
                // Use 'stat' command to get file size without reading the file
                var sizeResult = await _dockerService.ExecuteCommandAsync($"stat -c %s \"{filePath}\"", cancellationToken);
                if (long.TryParse(sizeResult.Trim(), out long size))
                    return size;

                // Fallback to ls -l
                var lsResult = await _dockerService.ExecuteCommandAsync($"ls -l \"{filePath}\"", cancellationToken);
                var parts = lsResult.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 4 && long.TryParse(parts[4], out size))
                    return size;

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        private async Task BuildIndexAsync(string filePath, CancellationToken cancellationToken)
        {
            try
            {
                var chunkIndex = 0;
                long fileOffset = 0;
                var packetCount = 0;

                while (fileOffset < _totalFileSize && !cancellationToken.IsCancellationRequested)
                {
                    // Read chunk from file
                    var chunkData = await ReadFileChunkAsync(filePath, fileOffset, ChunkSize, cancellationToken);
                    if (string.IsNullOrEmpty(chunkData))
                        break;

                    // Parse packets in this chunk
                    var chunkPackets = _packetParser.ParseLogContent(chunkData);

                    // Update index
                    foreach (var packet in chunkPackets)
                    {
                        if (packetCount % IndexInterval == 0)
                        {
                            _fileIndex.AddIndexPoint(packetCount, fileOffset, packet.Timestamp, packet.OpcodeName);
                        }
                        packetCount++;
                    }

                    // Cache this chunk if we have space
                    if (_chunkCache.Count < MaxCachedChunks)
                    {
                        _chunkCache[chunkIndex] = new LogChunk
                        {
                            Index = chunkIndex,
                            FileOffset = fileOffset,
                            Data = chunkData,
                            Packets = chunkPackets,
                            LastAccessed = DateTime.UtcNow
                        };
                    }

                    fileOffset += ChunkSize;
                    chunkIndex++;

                    // Cleanup old chunks if cache is full
                    CleanupOldChunks();

                    // Allow UI updates
                    await Task.Delay(10, cancellationToken);
                }

                _fileIndex.TotalPacketCount = packetCount;
                _fileIndex.IsComplete = true;
            }
            catch (OperationCanceledException)
            {
                // Expected when switching files
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error building index: {ex.Message}");
            }
        }

        private async Task<string> ReadFileChunkAsync(string filePath, long offset, int size, CancellationToken cancellationToken)
        {
            try
            {
                // Use 'dd' command to read specific byte range efficiently
                var command = $"dd if=\"{filePath}\" bs=1 skip={offset} count={size} 2>/dev/null";
                return await _dockerService.ExecuteCommandAsync(command, cancellationToken);
            }
            catch
            {
                return string.Empty;
            }
        }

        private async Task<PacketData?> GetPacketByIndexAsync(int packetIndex, CancellationToken cancellationToken)
        {
            var indexPoint = _fileIndex.GetNearestIndexPoint(packetIndex);
            if (indexPoint == null)
                return null;

            // Load the chunk containing this packet
            var chunkIndex = (int)(indexPoint.FileOffset / ChunkSize);
            var chunk = await LoadChunkAsync(chunkIndex, cancellationToken);

            if (chunk?.Packets != null)
            {
                var localIndex = packetIndex - indexPoint.PacketIndex;
                if (localIndex >= 0 && localIndex < chunk.Packets.Count)
                    return chunk.Packets[localIndex];
            }

            return null;
        }

        private async Task<LogChunk?> LoadChunkAsync(int chunkIndex, CancellationToken cancellationToken)
        {
            if (_chunkCache.TryGetValue(chunkIndex, out var cachedChunk))
            {
                cachedChunk.LastAccessed = DateTime.UtcNow;
                return cachedChunk;
            }

            // Load chunk from file
            var offset = chunkIndex * ChunkSize;
            var chunkData = await ReadFileChunkAsync(_currentFilePath!, offset, ChunkSize, cancellationToken);

            if (!string.IsNullOrEmpty(chunkData))
            {
                var packets = _packetParser.ParseLogContent(chunkData);
                var chunk = new LogChunk
                {
                    Index = chunkIndex,
                    FileOffset = offset,
                    Data = chunkData,
                    Packets = packets,
                    LastAccessed = DateTime.UtcNow
                };

                // Add to cache
                _chunkCache[chunkIndex] = chunk;
                CleanupOldChunks();

                return chunk;
            }

            return null;
        }

        private void CleanupOldChunks()
        {
            while (_chunkCache.Count > MaxCachedChunks)
            {
                var oldestChunk = _chunkCache.Values.OrderBy(c => c.LastAccessed).First();
                _chunkCache.Remove(oldestChunk.Index);
            }
        }

        private async Task<List<PacketData>> GetPacketRangeStreamingAsync(int startIndex, int count, CancellationToken cancellationToken)
        {
            // Fallback implementation for when index is not complete
            var packets = new List<PacketData>();

            // This is simplified - in practice, we'd stream through chunks
            // and skip until we reach the start index, then collect 'count' packets
            var allPackets = new List<PacketData>();

            // Stream through file in chunks
            long offset = 0;
            while (offset < _totalFileSize && allPackets.Count < startIndex + count)
            {
                var chunkData = await ReadFileChunkAsync(_currentFilePath!, offset, ChunkSize, cancellationToken);
                if (string.IsNullOrEmpty(chunkData))
                    break;

                var chunkPackets = _packetParser.ParseLogContent(chunkData);
                allPackets.AddRange(chunkPackets);
                offset += ChunkSize;
            }

            return allPackets.Skip(startIndex).Take(count).ToList();
        }

        private async Task<List<PacketSearchResult>> SearchStreamingAsync(PacketSearchCriteria criteria, CancellationToken cancellationToken)
        {
            var results = new List<PacketSearchResult>();
            long offset = 0;
            int packetIndex = 0;

            while (offset < _totalFileSize)
            {
                var chunkData = await ReadFileChunkAsync(_currentFilePath!, offset, ChunkSize, cancellationToken);
                if (string.IsNullOrEmpty(chunkData))
                    break;

                var chunkPackets = _packetParser.ParseLogContent(chunkData);

                foreach (var packet in chunkPackets)
                {
                    if (MatchesCriteria(packet, criteria))
                    {
                        results.Add(new PacketSearchResult
                        {
                            PacketIndex = packetIndex,
                            Packet = packet,
                            FileOffset = offset
                        });
                    }
                    packetIndex++;
                }

                offset += ChunkSize;
            }

            return results;
        }

        private static int EstimatePacketCount(long fileSize)
        {
            // Rough estimate: average 200 bytes per packet entry in log
            return (int)(fileSize / 200);
        }

        private static bool MatchesCriteria(PacketData packet, PacketSearchCriteria criteria)
        {
            if (!string.IsNullOrEmpty(criteria.OpcodeName) &&
                !packet.OpcodeName.Contains(criteria.OpcodeName, StringComparison.OrdinalIgnoreCase))
                return false;

            if (criteria.Direction.HasValue && packet.Direction != criteria.Direction.Value)
                return false;

            if (criteria.MinTimestamp.HasValue && packet.Timestamp < criteria.MinTimestamp.Value)
                return false;

            if (criteria.MaxTimestamp.HasValue && packet.Timestamp > criteria.MaxTimestamp.Value)
                return false;

            return true;
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _readSemaphore.Dispose();
            _chunkCache.Clear();
        }
    }

    // Supporting classes

    public class LogChunk
    {
        public int Index { get; set; }
        public long FileOffset { get; set; }
        public string Data { get; set; } = string.Empty;
        public List<PacketData> Packets { get; set; } = new();
        public DateTime LastAccessed { get; set; }
    }

    public class LogFileIndex
    {
        private readonly List<IndexPoint> _indexPoints = new();

        public int TotalPacketCount { get; set; }
        public bool IsComplete { get; set; }

        public void AddIndexPoint(int packetIndex, long fileOffset, DateTime timestamp, string opcode)
        {
            _indexPoints.Add(new IndexPoint
            {
                PacketIndex = packetIndex,
                FileOffset = fileOffset,
                Timestamp = timestamp,
                OpcodeName = opcode
            });
        }

        public IndexPoint? GetNearestIndexPoint(int packetIndex)
        {
            return _indexPoints
                .Where(p => p.PacketIndex <= packetIndex)
                .OrderByDescending(p => p.PacketIndex)
                .FirstOrDefault();
        }

        public long GetFileOffset(int packetIndex)
        {
            var point = GetNearestIndexPoint(packetIndex);
            return point?.FileOffset ?? 0;
        }

        public List<int> FindPacketIndices(PacketSearchCriteria criteria)
        {
            return _indexPoints
                .Where(p => string.IsNullOrEmpty(criteria.OpcodeName) ||
                           p.OpcodeName.Contains(criteria.OpcodeName, StringComparison.OrdinalIgnoreCase))
                .Where(p => !criteria.MinTimestamp.HasValue || p.Timestamp >= criteria.MinTimestamp.Value)
                .Where(p => !criteria.MaxTimestamp.HasValue || p.Timestamp <= criteria.MaxTimestamp.Value)
                .Select(p => p.PacketIndex)
                .ToList();
        }

        public void Clear()
        {
            _indexPoints.Clear();
            TotalPacketCount = 0;
            IsComplete = false;
        }
    }

    public class IndexPoint
    {
        public int PacketIndex { get; set; }
        public long FileOffset { get; set; }
        public DateTime Timestamp { get; set; }
        public string OpcodeName { get; set; } = string.Empty;
    }

}