using System;

namespace EQLogs.Models
{
    public class EQMapLogEntry
    {
        public DateTime Timestamp { get; set; }
        public string LogLevel { get; set; } = "";
        public string Source { get; set; } = "";
        public string Message { get; set; } = "";
        public string? PacketName { get; set; }
        public string? OpCode { get; set; }
        public int? PacketSize { get; set; }
        public string? Direction { get; set; } // S->C or C->S
        public string? StreamType { get; set; } // Login, Zone, etc.
        public string? HexDump { get; set; }
        public string? StructureData { get; set; }
        public string RawLogText { get; set; } = "";

        // For display purposes
        public string DisplayText => $"{Timestamp:HH:mm:ss.fff} [{Source}] {Message}";

        public bool IsPacketEntry => !string.IsNullOrEmpty(PacketName);
        public bool HasHexDump => !string.IsNullOrEmpty(HexDump);
        public bool HasStructure => !string.IsNullOrEmpty(StructureData);
    }
}