using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace EQLogs.Models
{
    public class PacketData
    {
        public DateTime Timestamp { get; set; }
        public string Source { get; set; } = string.Empty; // Zone, Login, World
        public PacketDirection Direction { get; set; }
        public string OpcodeName { get; set; } = string.Empty;
        public string OpcodeHex { get; set; } = string.Empty;
        public int Size { get; set; }
        public string HexDump { get; set; } = string.Empty;
        public string StructureData { get; set; } = string.Empty;
        public string OriginalLogLine { get; set; } = string.Empty;

        // Session information
        public string AccountId { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string PlayerName { get; set; } = string.Empty;
        public int SessionNumber { get; set; } = 0; // Sequence number for this session
        public string SessionKey => SessionNumber > 0 ? $"Session {SessionNumber}" : string.Empty;

        // For comparison
        public bool IsMatched { get; set; } = false;
        public PacketData? MatchedPacket { get; set; }

        public string DisplayText
        {
            get
            {
                var baseText = $"[{Timestamp:HH:mm:ss.fff}] [{Source}] [{Direction}] [{OpcodeName}] [{OpcodeHex}] Size [{Size}]";

                // Only add session info if we have account information
                if (!string.IsNullOrEmpty(AccountId) && !string.IsNullOrEmpty(AccountName))
                {
                    var sessionText = $" Session[{SessionNumber}] Account[{AccountId}:{AccountName}]";
                    if (!string.IsNullOrEmpty(PlayerName))
                    {
                        sessionText += $" Player[{PlayerName}]";
                    }
                    return baseText + sessionText;
                }

                return baseText;
            }
        }
        
        public static PacketData? ParseFromLogLine(string logLine)
        {
            // Try multiple formats for packet headers with session information

            // Enhanced formats to capture session information:
            // World/Zone: [timestamp] [Source] [Packet Direction] [Opcode] [Hex] Size [number] Account [id:name] Player [name]
            // Login: [timestamp] [Source] [Packet Direction] [Opcode] [Hex] Size [number] Account [id:name]

            Match? headerMatch = null;

            // Format 1: Full format with Session, Account and Player (Zone/World)
            // [timestamp] [Source] [Packet Direction] [Opcode] [Hex] Size [number] Session [session] Account [id:name] Player [name]
            headerMatch = Regex.Match(logLine, @"\[([^\]]+)\] \[(\w+)\] \[Packet ([SC])->([SC])\] \[(\w+)\] \[(0x[A-Fa-f0-9]+)\] Size \[(\d+)\] Session \[(\d+)\] Account \[(\d+):([^\]]+)\] Player \[([^\]]+)\]");
            if (headerMatch.Success)
            {
                return CreatePacketDataWithSession(headerMatch, logLine, hasPlayer: true);
            }

            // Format 2: Account and Player without Session (Zone/World - older format)
            headerMatch = Regex.Match(logLine, @"\[([^\]]+)\] \[(\w+)\] \[Packet ([SC])->([SC])\] \[(\w+)\] \[(0x[A-Fa-f0-9]+)\] Size \[(\d+)\] Account \[(\d+):([^\]]+)\] Player \[([^\]]+)\]");
            if (headerMatch.Success)
            {
                return CreatePacketData(headerMatch, logLine, hasPlayer: true);
            }

            // Format 3: Session and Account only (Login)
            // [timestamp] [Source] [Packet Direction] [Opcode] [Hex] Size [number] Session [session] Account [id:name]
            headerMatch = Regex.Match(logLine, @"\[([^\]]+)\] \[(\w+)\] \[Packet ([SC])->([SC])\] \[(\w+)\] \[(0x[A-Fa-f0-9]+)\] Size \[(\d+)\] Session \[(\d+)\] Account \[(\d+):([^\]]+)\]");
            if (headerMatch.Success)
            {
                return CreatePacketDataWithSession(headerMatch, logLine, hasPlayer: false);
            }

            // Format 4: Account only format (Login - older format)
            headerMatch = Regex.Match(logLine, @"\[([^\]]+)\] \[(\w+)\] \[Packet ([SC])->([SC])\] \[(\w+)\] \[(0x[A-Fa-f0-9]+)\] Size \[(\d+)\] Account \[(\d+):([^\]]+)\]");
            if (headerMatch.Success)
            {
                return CreatePacketData(headerMatch, logLine, hasPlayer: false);
            }

            // Legacy formats (without session info) for backward compatibility

            // Format 5: [timestamp] [Source] [Packet Direction] [Opcode] [Hex] Size [number]
            headerMatch = Regex.Match(logLine, @"\[([^\]]+)\] \[(\w+)\] \[Packet ([SC])->([SC])\] \[(\w+)\] \[(0x[A-Fa-f0-9]+)\] Size \[(\d+)\]");
            if (headerMatch.Success)
            {
                return CreatePacketData(headerMatch, logLine, hasPlayer: null);
            }

            // Format 6: Without "Packet" prefix
            headerMatch = Regex.Match(logLine, @"\[([^\]]+)\] \[(\w+)\] \[([SC])->([SC])\] \[(\w+)\] \[(0x[A-Fa-f0-9]+)\] Size \[(\d+)\]");
            if (headerMatch.Success)
            {
                return CreatePacketData(headerMatch, logLine, hasPlayer: null);
            }

            // Format 7: Simple format
            headerMatch = Regex.Match(logLine, @"([^\[]*)\[(\w+)\] ([SC])->([SC]) (\w+) (0x[A-Fa-f0-9]+) Size (\d+)");
            if (headerMatch.Success)
            {
                return CreatePacketData(headerMatch, logLine, hasPlayer: null);
            }

            // Debug logging for unmatched lines
            if (logLine.Contains("OP_ClientUpdate") || logLine.Contains("Account [") || logLine.Contains("Player ["))
            {
                System.Diagnostics.Debug.WriteLine($"Failed to parse enhanced log line: {logLine}");
            }

            return null;
        }

        private static PacketData? CreatePacketData(Match headerMatch, string logLine, bool? hasPlayer)
        {
            // Try to parse the timestamp with multiple possible formats
            DateTime timestamp;
            string timestampStr = headerMatch.Groups[1].Value.Trim();
            string[] formats = {
                "MM-dd-yyyy HH:mm:ss",
                "dd-MM-yyyy HH:mm:ss",
                "yyyy-MM-dd HH:mm:ss",
                "yyyy-MM-dd HH:mm:ss.fff",
                "MM-dd-yyyy HH:mm:ss.fff"
            };

            if (!DateTime.TryParseExact(timestampStr, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out timestamp))
            {
                // If all specific formats fail, try general parsing
                if (!DateTime.TryParse(timestampStr, out timestamp))
                {
                    // If everything fails, use current time and log the issue
                    timestamp = DateTime.Now;
                    System.Diagnostics.Debug.WriteLine($"Failed to parse timestamp: '{timestampStr}' - using current time");
                }
            }

            // Extract values based on regex groups (accounting for different formats)
            string source = headerMatch.Groups[2].Value;
            string directionFrom = headerMatch.Groups[3].Value;
            string directionTo = headerMatch.Groups[4].Value;
            string opcodeName = headerMatch.Groups[5].Value;
            string opcodeHex = headerMatch.Groups[6].Value;
            string sizeStr = headerMatch.Groups[7].Value;

            var packet = new PacketData
            {
                Timestamp = timestamp,
                Source = source,
                Direction = directionFrom == "S" ? PacketDirection.ServerToClient : PacketDirection.ClientToServer,
                OpcodeName = opcodeName,
                OpcodeHex = opcodeHex,
                Size = int.Parse(sizeStr),
                OriginalLogLine = logLine
            };

            // Extract session information based on format
            if (hasPlayer == true && headerMatch.Groups.Count >= 10)
            {
                // Format with Account and Player: groups[8] = accountId, groups[9] = accountName, groups[10] = playerName
                packet.AccountId = headerMatch.Groups[8].Value;
                packet.AccountName = headerMatch.Groups[9].Value;
                packet.PlayerName = headerMatch.Groups[10].Value;
            }
            else if (hasPlayer == false && headerMatch.Groups.Count >= 9)
            {
                // Format with Account only: groups[8] = accountId, groups[9] = accountName
                packet.AccountId = headerMatch.Groups[8].Value;
                packet.AccountName = headerMatch.Groups[9].Value;
                packet.PlayerName = string.Empty;
            }
            // For legacy formats (hasPlayer == null), leave session info empty

            return packet;
        }

        private static PacketData? CreatePacketDataWithSession(Match headerMatch, string logLine, bool hasPlayer)
        {
            // Try to parse the timestamp with multiple possible formats
            DateTime timestamp;
            string timestampStr = headerMatch.Groups[1].Value.Trim();
            string[] formats = {
                "MM-dd-yyyy HH:mm:ss",
                "dd-MM-yyyy HH:mm:ss",
                "yyyy-MM-dd HH:mm:ss",
                "yyyy-MM-dd HH:mm:ss.fff",
                "MM-dd-yyyy HH:mm:ss.fff"
            };

            if (!DateTime.TryParseExact(timestampStr, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out timestamp))
            {
                // If all specific formats fail, try general parsing
                if (!DateTime.TryParse(timestampStr, out timestamp))
                {
                    // If everything fails, use current time and log the issue
                    timestamp = DateTime.Now;
                    System.Diagnostics.Debug.WriteLine($"Failed to parse timestamp: '{timestampStr}' - using current time");
                }
            }

            // Extract values based on regex groups (with Session included)
            string source = headerMatch.Groups[2].Value;
            string directionFrom = headerMatch.Groups[3].Value;
            string directionTo = headerMatch.Groups[4].Value;
            string opcodeName = headerMatch.Groups[5].Value;
            string opcodeHex = headerMatch.Groups[6].Value;
            string sizeStr = headerMatch.Groups[7].Value;
            string sessionNumberStr = headerMatch.Groups[8].Value; // Session number from log
            string accountId = headerMatch.Groups[9].Value;
            string accountName = headerMatch.Groups[10].Value;

            var packet = new PacketData
            {
                Timestamp = timestamp,
                Source = source,
                Direction = directionFrom == "S" ? PacketDirection.ServerToClient : PacketDirection.ClientToServer,
                OpcodeName = opcodeName,
                OpcodeHex = opcodeHex,
                Size = int.Parse(sizeStr),
                OriginalLogLine = logLine,
                AccountId = accountId,
                AccountName = accountName,
                SessionNumber = int.TryParse(sessionNumberStr, out int sessionNum) ? sessionNum : 0
            };

            // Extract player name if available
            if (hasPlayer && headerMatch.Groups.Count >= 11)
            {
                packet.PlayerName = headerMatch.Groups[11].Value;
            }

            return packet;
        }
    }
    
    public enum PacketDirection
    {
        ServerToClient,
        ClientToServer
    }
    
    public class LogFile
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public long Size { get; set; }
        public DateTime LastModified { get; set; }
        public LogType Type { get; set; }
    }
    
    public enum LogType
    {
        Login,
        World,
        Zone,
        Bot
    }

    // Classes for large log file handling
    public class LogFileInfo
    {
        public string FilePath { get; set; } = string.Empty;
        public long TotalSize { get; set; }
        public int EstimatedPacketCount { get; set; }
    }

    public class LogPageResult
    {
        public List<PacketData> Packets { get; set; } = new();
        public int StartIndex { get; set; }
        public int TotalCount { get; set; }
        public bool IsComplete { get; set; }
    }

    public class PacketSearchCriteria
    {
        public string? OpcodeName { get; set; }
        public PacketDirection? Direction { get; set; }
        public DateTime? MinTimestamp { get; set; }
        public DateTime? MaxTimestamp { get; set; }
    }

    public class PacketSearchResult
    {
        public int PacketIndex { get; set; }
        public PacketData Packet { get; set; } = null!;
        public long FileOffset { get; set; }
    }
}