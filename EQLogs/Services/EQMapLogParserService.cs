using EQLogs.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EQLogs.Services
{
    public class EQMapLogParserService
    {
        private static readonly Regex LogLineRegex = new Regex(
            @"^(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{4})\s+(\w+)\s+\[([^\]]+)\]\s+(.*)$",
            RegexOptions.Compiled);

        private static readonly Regex PacketRegex = new Regex(
            @"(\d{2}-\d{2}-\d{4} \d{2}:\d{2}:\d{2})\s+\|\s+(\w+)\s+\|\s+Packet\s+([CS])->([CS])\s+\|\s+\[([^\]]+)\]\s+\[([^\]]+)\]\s+Size\s+\[(\d+)\]",
            RegexOptions.Compiled);

        public List<EQMapLogEntry> ParseLogFile(string filePath)
        {
            var entries = new List<EQMapLogEntry>();
            var lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length; i++)
            {
                var entry = ParseLogLine(lines[i], i, lines);
                if (entry != null)
                {
                    entries.Add(entry);
                }
            }

            return entries;
        }

        public List<EQMapLogEntry> ParseLogContent(string logContent)
        {
            var entries = new List<EQMapLogEntry>();
            var lines = logContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++)
            {
                var entry = ParseLogLine(lines[i], i, lines);
                if (entry != null)
                {
                    entries.Add(entry);
                }
            }

            return entries;
        }

        private EQMapLogEntry? ParseLogLine(string line, int lineIndex, string[] allLines)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;

            var match = LogLineRegex.Match(line);
            if (!match.Success)
                return null;

            var timestamp = DateTime.ParseExact(match.Groups[1].Value, "yyyy-MM-dd HH:mm:ss.ffff", CultureInfo.InvariantCulture);
            var logLevel = match.Groups[2].Value;
            var source = match.Groups[3].Value;
            var message = match.Groups[4].Value;

            var entry = new EQMapLogEntry
            {
                Timestamp = timestamp,
                LogLevel = logLevel,
                Source = source,
                Message = message,
                RawLogText = line
            };

            // Check if this is a packet entry
            if (source == "EQStream" && message.Contains("Packet"))
            {
                ParsePacketInfo(message, entry);

                // Look for hex dump in following lines
                ParseHexDump(lineIndex, allLines, entry);
            }
            else if (source == "ZoneStream" && message.Contains("struct"))
            {
                // Look for structure data in current and following lines
                ParseStructureData(lineIndex, allLines, entry);
            }

            return entry;
        }

        private void ParsePacketInfo(string message, EQMapLogEntry entry)
        {
            var packetMatch = PacketRegex.Match(message);
            if (packetMatch.Success)
            {
                entry.StreamType = packetMatch.Groups[2].Value; // Login, Zone, etc.
                entry.Direction = $"{packetMatch.Groups[3].Value}->{packetMatch.Groups[4].Value}";
                entry.PacketName = packetMatch.Groups[5].Value;
                entry.OpCode = packetMatch.Groups[6].Value;

                if (int.TryParse(packetMatch.Groups[7].Value, out int size))
                {
                    entry.PacketSize = size;
                }
            }
        }

        private void ParseHexDump(int startIndex, string[] allLines, EQMapLogEntry entry)
        {
            var hexDumpBuilder = new StringBuilder();
            var nextLineIndex = startIndex + 1;

            // Look for hex dump lines that start with spaces and contain hex data
            while (nextLineIndex < allLines.Length)
            {
                var nextLine = allLines[nextLineIndex];

                // Check if this looks like a hex dump line
                if (IsHexDumpLine(nextLine))
                {
                    hexDumpBuilder.AppendLine(nextLine.Trim());
                    nextLineIndex++;
                }
                else if (string.IsNullOrWhiteSpace(nextLine))
                {
                    // Skip empty lines but continue looking
                    nextLineIndex++;
                }
                else
                {
                    // Non-hex line found, stop
                    break;
                }
            }

            if (hexDumpBuilder.Length > 0)
            {
                entry.HexDump = hexDumpBuilder.ToString().Trim();
            }
        }

        private void ParseStructureData(int startIndex, string[] allLines, EQMapLogEntry entry)
        {
            var structureBuilder = new StringBuilder();
            var currentLine = allLines[startIndex];

            // Add the current struct line
            structureBuilder.AppendLine(currentLine.Trim());

            var nextLineIndex = startIndex + 1;
            int braceCount = currentLine.Count(c => c == '{') - currentLine.Count(c => c == '}');

            // Continue reading lines until we find the closing brace or run out of lines
            while (nextLineIndex < allLines.Length && braceCount > 0)
            {
                var nextLine = allLines[nextLineIndex];

                // If the line starts with a timestamp, it's a new log entry
                if (LogLineRegex.IsMatch(nextLine))
                {
                    break;
                }

                structureBuilder.AppendLine(nextLine);
                braceCount += nextLine.Count(c => c == '{') - nextLine.Count(c => c == '}');
                nextLineIndex++;
            }

            if (structureBuilder.Length > 0)
            {
                entry.StructureData = structureBuilder.ToString().Trim();
            }
        }

        private bool IsHexDumpLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return false;

            // Hex dump lines typically start with spaces and contain patterns like:
            // "   0: 0B 00 00 00 00 00 00 00 - 00 00 78 E0 07 00 00 00 | ........ ..x....."
            var trimmed = line.Trim();

            // Check for hex dump pattern: number: hex bytes - hex bytes | ascii
            return Regex.IsMatch(trimmed, @"^\s*\d+:\s+[0-9A-Fa-f\s-]+\|\s*.*$") ||
                   Regex.IsMatch(trimmed, @"^\s*\d+:\s+[0-9A-Fa-f\s-]+$");
        }

        public List<EQMapLogEntry> GetPacketEntries(List<EQMapLogEntry> entries)
        {
            return entries.Where(e => e.IsPacketEntry).ToList();
        }

        public List<EQMapLogEntry> FilterByLogLevel(List<EQMapLogEntry> entries, string logLevel)
        {
            return entries.Where(e => e.LogLevel.Equals(logLevel, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<EQMapLogEntry> FilterBySource(List<EQMapLogEntry> entries, string source)
        {
            return entries.Where(e => e.Source.Equals(source, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<EQMapLogEntry> FilterByPacketName(List<EQMapLogEntry> entries, string packetName)
        {
            return entries.Where(e => e.PacketName?.Equals(packetName, StringComparison.OrdinalIgnoreCase) == true).ToList();
        }
    }
}