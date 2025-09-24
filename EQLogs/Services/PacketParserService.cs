using EQLogs.Models;
using EQProtocol.Streams.Common;
using OpenEQ.Netcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EQLogs.Services
{
    public class PacketParserService
    {
        public List<PacketData> ParseLogContent(string logContent)
        {
            var packets = new List<PacketData>();
            var lines = logContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            PacketData? currentPacket = null;
            var hexLines = new List<string>();
            var skippedLines = new List<string>(); // For debugging

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();

                // Check if this is a packet header line
                var packetHeader = PacketData.ParseFromLogLine(trimmedLine);
                if (packetHeader != null)
                {
                    // Save previous packet if exists
                    if (currentPacket != null)
                    {
                        currentPacket.HexDump = string.Join("\n", hexLines);
                        currentPacket.StructureData = ParseStructureData(currentPacket);
                        packets.Add(currentPacket);
                    }

                    // Start new packet
                    currentPacket = packetHeader;
                    hexLines.Clear();
                }
                else if (currentPacket != null)
                {
                    // Check if this is a hex dump line
                    var hexMatch = Regex.Match(trimmedLine, @"^\s*(\d+):\s+([0-9A-Fa-f\s\-]+)\s+\|\s*(.*)$");
                    if (hexMatch.Success)
                    {
                        // Format hex dump line with proper alignment
                        var offset = int.Parse(hexMatch.Groups[1].Value);
                        var hexBytes = hexMatch.Groups[2].Value.Trim();
                        var asciiPart = hexMatch.Groups[3].Value;

                        // Create properly aligned hex dump line
                        var formattedLine = FormatHexDumpLine(offset, hexBytes, asciiPart);
                        hexLines.Add(formattedLine);
                    }
                    else if (trimmedLine.StartsWith("struct ") || trimmedLine.Contains("{"))
                    {
                        // Structure data - add to structure data
                        currentPacket.StructureData += trimmedLine + "\n";
                    }
                }
                else
                {
                    // Track potential packet header lines that didn't match
                    if (trimmedLine.Contains("OP_ClientUpdate") ||
                        trimmedLine.Contains("Packet") ||
                        trimmedLine.Contains("[") && trimmedLine.Contains("]"))
                    {
                        skippedLines.Add(trimmedLine);
                    }
                }
            }

            // Don't forget the last packet
            if (currentPacket != null)
            {
                currentPacket.HexDump = string.Join("\n", hexLines);
                if (string.IsNullOrEmpty(currentPacket.StructureData))
                {
                    currentPacket.StructureData = ParseStructureData(currentPacket);
                }
                packets.Add(currentPacket);
            }

            // Log skipped lines for debugging (first 10)
            if (skippedLines.Count > 0)
            {
                System.Diagnostics.Debug.WriteLine($"Skipped {skippedLines.Count} potential packet lines:");
                foreach (var skipped in skippedLines.Take(10))
                {
                    System.Diagnostics.Debug.WriteLine($"  SKIPPED: {skipped}");
                }
            }

            return packets;
        }
        
        private string ParseStructureData(PacketData packet)
        {
            if (string.IsNullOrEmpty(packet.HexDump))
                return "No hex data available";
                
            try
            {
                // Try to parse the packet using EQProtocol structures
                var hexData = ExtractHexBytes(packet.HexDump);
                if (hexData.Length == 0)
                    return "No valid hex data found";
                
                var sb = new StringBuilder();
                sb.AppendLine($"Packet: {packet.OpcodeName} ({packet.OpcodeHex})");
                sb.AppendLine($"Size: {packet.Size} bytes");
                sb.AppendLine($"Direction: {packet.Direction}");
                sb.AppendLine();
                
                // Attempt to decode known packet structures
                if (TryDecodeKnownPacket(packet.OpcodeName, hexData, sb, packet.Direction))
                {
                    return sb.ToString();
                }
                
                // Enhanced generic packet analysis
                sb.AppendLine($"Unknown packet structure for {packet.OpcodeName}:");
                sb.AppendLine();
                
                // Analyze packet content
                AnalyzePacketStructure(hexData, sb);
                
                sb.AppendLine();
                sb.AppendLine("Raw hex data (first 128 bytes):");
                for (int i = 0; i < Math.Min(hexData.Length, 128); i += 16)
                {
                    var chunk = hexData.Skip(i).Take(16).ToArray();
                    var hex = string.Join(" ", chunk.Select(b => b.ToString("X2")));
                    var ascii = string.Join("", chunk.Select(b => b >= 32 && b < 127 ? (char)b : '.'));
                    sb.AppendLine($"  {i:X4}: {hex.PadRight(47)} | {ascii}");
                }
                
                if (hexData.Length > 128)
                    sb.AppendLine($"  ... ({hexData.Length - 128} more bytes)");
                
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return $"Error parsing packet structure: {ex.Message}";
            }
        }

        // TODO: Cater for NPC Movements and check Mob Movements
        private bool TryDecodeKnownPacket(string opcodeName, byte[] data, StringBuilder sb, PacketDirection direction)
        {
            try
            {
                // Try to use EQProtocol classes first, then fall back to manual parsing
                return opcodeName switch
                {
                    "OP_ClientUpdate" => DecodeClientUpdate(data, sb, direction),
                    "OP_PlayerProfile" => DecodePlayerProfile(data, sb),
                    "OP_ZoneEntry" => DecodeZoneEntry(data, sb),
                    "OP_NewZone" => DecodeNewZone(data, sb),
                    "OP_HP_Update" => DecodeHPUpdate(data, sb),
                    "OP_TargetMouse" => DecodeTargetMouse(data, sb),
                    "OP_Consider" => DecodeConsider(data, sb),
                    "OP_SpawnAppearance" => DecodeSpawnAppearance(data, sb),
                    "OP_MobUpdate" => DecodeMobUpdate(data, sb),
                    "OP_NewSpawn" => DecodeNewSpawn(data, sb),
                    _ => false
                };
            }
            catch
            {
                return false;
            }
        }

        private bool DecodeClientUpdate(byte[] data, StringBuilder sb, PacketDirection direction)
        {
            try
            {
                if (direction == PacketDirection.ClientToServer)
                {
                    return DecodeUsingEQProtocol<ClientUpdateToServer>(data, sb, "ClientUpdateToServer");
                }
                else // ServerToClient
                {
                    return DecodeUsingEQProtocol<ClientUpdateFromServer>(data, sb, "ClientUpdateFromServer");
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine($"Error decoding OP_ClientUpdate: {ex.Message}");
                return false;
            }
        }

        private bool DecodePlayerProfile(byte[] data, StringBuilder sb)
        {
            sb.AppendLine("PlayerProfile structure:");
            sb.AppendLine($"  Size: {data.Length} bytes");
            sb.AppendLine("  (Full player profile data - too large to decode completely)");
            
            if (data.Length >= 64)
            {
                sb.AppendLine("  First 64 bytes:");
                for (int i = 0; i < 64; i += 16)
                {
                    var chunk = data.Skip(i).Take(16).ToArray();
                    sb.AppendLine($"    {i:X4}: {string.Join(" ", chunk.Select(b => b.ToString("X2")))}");
                }
            }
            
            return true;
        }
        
        private byte[] ExtractHexBytes(string hexDump)
        {
            var bytes = new List<byte>();
            var lines = hexDump.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var line in lines)
            {
                var match = Regex.Match(line.Trim(), @"^\s*\d+:\s+([0-9A-Fa-f\s\-]+)\s+\|");
                if (match.Success)
                {
                    var hexPart = match.Groups[1].Value;
                    var hexNumbers = Regex.Matches(hexPart, @"[0-9A-Fa-f]{2}");
                    
                    foreach (Match hexMatch in hexNumbers)
                    {
                        if (byte.TryParse(hexMatch.Value, System.Globalization.NumberStyles.HexNumber, null, out byte b))
                        {
                            bytes.Add(b);
                        }
                    }
                }
            }
            
            return bytes.ToArray();
        }
        
        public List<PacketData> FindMatchingPackets(List<PacketData> serverPackets, List<PacketData> botPackets)
        {
            var unmatched = new List<PacketData>();
            
            foreach (var botPacket in botPackets)
            {
                var match = serverPackets.FirstOrDefault(sp => 
                    sp.OpcodeName == botPacket.OpcodeName &&
                    sp.Direction == botPacket.Direction &&
                    Math.Abs((sp.Timestamp - botPacket.Timestamp).TotalSeconds) < 5); // Within 5 seconds
                
                if (match != null)
                {
                    botPacket.IsMatched = true;
                    botPacket.MatchedPacket = match;
                    match.IsMatched = true;
                    match.MatchedPacket = botPacket;
                }
                else
                {
                    unmatched.Add(botPacket);
                }
            }
            
            // Add unmatched server packets too
            unmatched.AddRange(serverPackets.Where(sp => !sp.IsMatched));
            
            return unmatched;
        }
        
        private bool DecodeUsingEQProtocol<T>(byte[] data, StringBuilder sb, string structName) where T : IEQStruct, new()
        {
            try
            {
                sb.AppendLine($"{structName} structure (using EQProtocol):");
                sb.AppendLine();
                
                // Create instance and unpack the data
                var packet = new T();
                packet.Unpack(data);
                
                // Use the built-in ToString() method for detailed output
                sb.AppendLine(packet.ToString());
                
                return true;
            }
            catch (Exception ex)
            {
                sb.AppendLine($"Error decoding {structName} using EQProtocol: {ex.Message}");
                sb.AppendLine($"Data length: {data.Length} bytes");
                
                // Fallback to hex dump for debugging
                sb.AppendLine("Raw hex data:");
                for (int i = 0; i < Math.Min(data.Length, 64); i += 16)
                {
                    var chunk = data.Skip(i).Take(16).ToArray();
                    var hex = string.Join(" ", chunk.Select(b => b.ToString("X2")));
                    var ascii = string.Join("", chunk.Select(b => b >= 32 && b < 127 ? (char)b : '.'));
                    sb.AppendLine($"  {i:X4}: {hex.PadRight(47)} | {ascii}");
                }
                
                return false;
            }
        }
        
        private void AnalyzePacketStructure(byte[] data, StringBuilder sb)
        {
            if (data.Length == 0) return;
            
            sb.AppendLine("Structural Analysis:");
            
            // Look for common patterns
            if (data.Length >= 2)
            {
                var uint16_0 = BitConverter.ToUInt16(data, 0);
                sb.AppendLine($"  Offset 0x0000: 0x{uint16_0:X4} ({uint16_0}) [UInt16]");
                
                // Check if it might be an entity ID
                if (uint16_0 > 0 && uint16_0 < 65535)
                    sb.AppendLine($"    → Possible entity/spawn ID");
            }
            
            if (data.Length >= 4)
            {
                var uint32_0 = BitConverter.ToUInt32(data, 0);
                var float_0 = BitConverter.ToSingle(data, 0);
                sb.AppendLine($"  Offset 0x0000: 0x{uint32_0:X8} ({uint32_0}) [UInt32]");
                if (!float.IsNaN(float_0) && !float.IsInfinity(float_0) && Math.Abs(float_0) < 100000)
                    sb.AppendLine($"  Offset 0x0000: {float_0:F2} [Float - possible coordinate]");
            }
            
            // Look for coordinates (common EQ pattern)
            if (data.Length >= 12)
            {
                var x = BitConverter.ToSingle(data, 0);
                var y = BitConverter.ToSingle(data, 4);
                var z = BitConverter.ToSingle(data, 8);
                
                if (!float.IsNaN(x) && !float.IsNaN(y) && !float.IsNaN(z) &&
                    Math.Abs(x) < 100000 && Math.Abs(y) < 100000 && Math.Abs(z) < 100000)
                {
                    sb.AppendLine($"  Possible coordinates at 0x0000-0x000B:");
                    sb.AppendLine($"    X: {x:F2}, Y: {y:F2}, Z: {z:F2}");
                }
            }
            
            // Look for strings
            var strings = FindPossibleStrings(data);
            if (strings.Count > 0)
            {
                sb.AppendLine("  Possible strings:");
                foreach (var (offset, str) in strings.Take(3))
                {
                    sb.AppendLine($"    0x{offset:X4}: \"{str}\"");
                }
            }
        }
        
        private List<(int offset, string text)> FindPossibleStrings(byte[] data)
        {
            var strings = new List<(int, string)>();
            var currentString = new StringBuilder();
            int stringStart = -1;
            
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= 32 && data[i] < 127) // Printable ASCII
                {
                    if (stringStart == -1)
                        stringStart = i;
                    currentString.Append((char)data[i]);
                }
                else
                {
                    if (currentString.Length >= 4) // At least 4 chars to be considered a string
                    {
                        strings.Add((stringStart, currentString.ToString()));
                    }
                    currentString.Clear();
                    stringStart = -1;
                }
            }
            
            // Don't forget the last string if the data ends with one
            if (currentString.Length >= 4)
                strings.Add((stringStart, currentString.ToString()));
                
            return strings;
        }
        
        private bool DecodeZoneEntry(byte[] data, StringBuilder sb)
        {
            sb.AppendLine("ZoneEntry structure:");
            if (data.Length >= 64)
            {
                sb.AppendLine("  (Zone entry data - complex structure)");
                sb.AppendLine($"  Size: {data.Length} bytes");
                if (data.Length >= 4)
                {
                    var playerId = BitConverter.ToUInt32(data, 0);
                    sb.AppendLine($"  Possible Player ID: {playerId}");
                }
            }
            return true;
        }
        
        private bool DecodeNewZone(byte[] data, StringBuilder sb)
        {
            sb.AppendLine("NewZone structure:");
            if (data.Length >= 16)
            {
                sb.AppendLine($"  Size: {data.Length} bytes");
                // Zone name often starts around offset 4-8
                var zoneName = FindPossibleStrings(data.Skip(4).Take(32).ToArray());
                if (zoneName.Count > 0)
                {
                    sb.AppendLine($"  Possible zone name: \"{zoneName[0].text}\"");
                }
            }
            return true;
        }
        
        private bool DecodeHPUpdate(byte[] data, StringBuilder sb)
        {
            if (data.Length < 8) return false;
            
            sb.AppendLine("HP Update structure:");
            var spawnId = BitConverter.ToUInt32(data, 0);
            var hpPercent = BitConverter.ToUInt32(data, 4);
            
            sb.AppendLine($"  Spawn ID: {spawnId}");
            sb.AppendLine($"  HP Percentage: {hpPercent}%");
            return true;
        }
        
        private bool DecodeTargetMouse(byte[] data, StringBuilder sb)
        {
            if (data.Length < 4) return false;
            
            sb.AppendLine("Target Mouse structure:");
            var targetId = BitConverter.ToUInt32(data, 0);
            sb.AppendLine($"  Target ID: {targetId}");
            return true;
        }
        
        private bool DecodeConsider(byte[] data, StringBuilder sb)
        {
            if (data.Length < 8) return false;
            
            sb.AppendLine("Consider structure:");
            var playerId = BitConverter.ToUInt32(data, 0);
            var targetId = BitConverter.ToUInt32(data, 4);
            
            sb.AppendLine($"  Player ID: {playerId}");
            sb.AppendLine($"  Target ID: {targetId}");
            
            if (data.Length >= 12)
            {
                var considerLevel = BitConverter.ToUInt32(data, 8);
                sb.AppendLine($"  Consider Level: {considerLevel}");
            }
            return true;
        }
        
        private bool DecodeSpawnAppearance(byte[] data, StringBuilder sb)
        {
            if (data.Length < 8) return false;
            
            sb.AppendLine("Spawn Appearance structure:");
            var spawnId = BitConverter.ToUInt16(data, 0);
            var type = BitConverter.ToUInt16(data, 2);
            var parameter = BitConverter.ToUInt32(data, 4);
            
            sb.AppendLine($"  Spawn ID: {spawnId}");
            sb.AppendLine($"  Type: {type}");
            sb.AppendLine($"  Parameter: {parameter}");
            
            // Decode appearance types
            switch (type)
            {
                case 0: sb.AppendLine("    → Animation/Activity"); break;
                case 1: sb.AppendLine("    → HP (0=alive, 1=dead)"); break;
                case 3: sb.AppendLine("    → Invisibility/See Invis"); break;
                case 14: sb.AppendLine("    → Levitation"); break;
                case 15: sb.AppendLine("    → GM Flag"); break;
                case 16: sb.AppendLine("    → Anonymous"); break;
                case 17: sb.AppendLine("    → Guild ID"); break;
                case 29: sb.AppendLine("    → NPC Name"); break;
            }
            
            return true;
        }
        
        private bool DecodeMobUpdate(byte[] data, StringBuilder sb)
        {
            if (data.Length < 20) return false;
            
            sb.AppendLine("Mob Update structure:");
            var spawnId = BitConverter.ToUInt16(data, 0);
            var x = BitConverter.ToSingle(data, 4);
            var y = BitConverter.ToSingle(data, 8);
            var z = BitConverter.ToSingle(data, 12);
            var heading = BitConverter.ToSingle(data, 16);
            
            sb.AppendLine($"  Spawn ID: {spawnId}");
            sb.AppendLine($"  Position: X={x:F2}, Y={y:F2}, Z={z:F2}");
            sb.AppendLine($"  Heading: {heading:F2}");
            return true;
        }
        
        private bool DecodeNewSpawn(byte[] data, StringBuilder sb)
        {
            if (data.Length < 100) return false; // NewSpawn is typically quite large
            
            sb.AppendLine("New Spawn structure:");
            sb.AppendLine($"  Size: {data.Length} bytes");
            
            // Basic spawn info is usually at the beginning
            if (data.Length >= 4)
            {
                var spawnId = BitConverter.ToUInt32(data, 0);
                sb.AppendLine($"  Spawn ID: {spawnId}");
            }
            
            // Look for name strings
            var strings = FindPossibleStrings(data);
            if (strings.Count > 0)
            {
                sb.AppendLine("  Possible spawn names:");
                foreach (var (offset, name) in strings.Take(2))
                {
                    if (name.Length < 20) // Reasonable name length
                        sb.AppendLine($"    \"{name}\" at offset 0x{offset:X4}");
                }
            }
            
            return true;
        }
        
        private string FormatHexDumpLine(int offset, string hexBytes, string asciiPart)
        {
            // Determine the maximum offset we might encounter to calculate padding
            // For most packet dumps, we rarely go beyond 65535 bytes, so 5 digits should be enough
            const int maxOffsetWidth = 5;
            
            // Format the offset with proper padding (right-aligned)
            string offsetStr = offset.ToString().PadLeft(maxOffsetWidth, ' ');
            
            // Clean up and format the hex bytes - ensure consistent spacing
            var hexParts = hexBytes.Split(new char[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
            var formattedHexBytes = new List<string>();
            
            for (int i = 0; i < hexParts.Length; i++)
            {
                if (hexParts[i].Length == 2)
                {
                    formattedHexBytes.Add(hexParts[i].ToUpper());
                }
                else if (hexParts[i].Length == 1)
                {
                    formattedHexBytes.Add("0" + hexParts[i].ToUpper());
                }
            }
            
            // Ensure we have up to 16 hex bytes per line for proper alignment
            // If we have fewer, pad with spaces to maintain alignment
            const int bytesPerLine = 16;
            int actualBytes = formattedHexBytes.Count;
            while (formattedHexBytes.Count < bytesPerLine)
            {
                formattedHexBytes.Add("  "); // Two spaces for missing byte
            }
            
            // Group bytes in sets of 8 for readability (common hex dump format)
            var firstHalf = string.Join(" ", formattedHexBytes.Take(8));
            var secondHalf = string.Join(" ", formattedHexBytes.Skip(8));
            
            // Format: "  OFFSET: FIRST_8_BYTES  SECOND_8_BYTES | ASCII"
            var formattedLine = $"{offsetStr}: {firstHalf}  {secondHalf} | {asciiPart}";
            
            return formattedLine;
        }
    }
}