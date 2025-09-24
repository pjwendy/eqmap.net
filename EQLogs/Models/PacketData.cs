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
        
        // For comparison
        public bool IsMatched { get; set; } = false;
        public PacketData? MatchedPacket { get; set; }
        
        public string DisplayText => $"[{Timestamp:HH:mm:ss.fff}] [{Source}] [{Direction}] [{OpcodeName}] [{OpcodeHex}] Size [{Size}]";
        
        public static PacketData? ParseFromLogLine(string logLine)
        {
            // Try multiple formats for packet headers

            // Format 1: [09-11-2025 08:38:40] [Zone] [Packet S->C] [OP_PlayerProfile] [0x6022] Size [26634]
            var headerMatch = Regex.Match(logLine, @"\[(\d{2}-\d{2}-\d{4} \d{2}:\d{2}:\d{2})\] \[(\w+)\] \[Packet ([SC])->([SC])\] \[(\w+)\] \[(0x[A-Fa-f0-9]+)\] Size \[(\d+)\]");

            if (!headerMatch.Success)
            {
                // Format 2: Try with different timestamp format [yyyy-MM-dd HH:mm:ss.fff]
                headerMatch = Regex.Match(logLine, @"\[(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}(?:\.\d{3})?)\] \[(\w+)\] \[Packet ([SC])->([SC])\] \[(\w+)\] \[(0x[A-Fa-f0-9]+)\] Size \[(\d+)\]");
            }

            if (!headerMatch.Success)
            {
                // Format 3: Try without "Packet" prefix: [timestamp] [Zone] [S->C] [OP_Name] [0xHex] Size [number]
                headerMatch = Regex.Match(logLine, @"\[([^\]]+)\] \[(\w+)\] \[([SC])->([SC])\] \[(\w+)\] \[(0x[A-Fa-f0-9]+)\] Size \[(\d+)\]");
            }

            if (!headerMatch.Success)
            {
                // Format 4: Try simpler format: timestamp [Zone] S->C OP_Name 0xHex Size number
                headerMatch = Regex.Match(logLine, @"([^\[]*)\[(\w+)\] ([SC])->([SC]) (\w+) (0x[A-Fa-f0-9]+) Size (\d+)");
            }

            if (!headerMatch.Success)
            {
                // Format 5: Debug any line containing OP_ClientUpdate
                if (logLine.Contains("OP_ClientUpdate"))
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to parse OP_ClientUpdate line: {logLine}");
                }
                return null;
            }
                
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
}