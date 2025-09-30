using EQLogs.Models;
using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace EQLogs.Services
{
    /// <summary>
    /// Generic packet parser service that uses reflection to automatically map opcodes to EQProtocol structures
    /// and handle packet parsing with ToServer/FromServer suffix handling for bidirectional packets.
    /// </summary>
    public class GenericPacketParserService
    {
        private readonly Dictionary<string, Type> _opcodeToTypeMapping;
        private readonly Dictionary<string, (Type toServer, Type fromServer)> _bidirectionalOpcodes;

        public GenericPacketParserService()
        {
            _opcodeToTypeMapping = new Dictionary<string, Type>();
            _bidirectionalOpcodes = new Dictionary<string, (Type, Type)>();


            BuildOpcodeToTypeMappings();
        }

        /// <summary>
        /// Parse the structure data for a packet using reflection-based opcode mapping
        /// </summary>
        public string ParsePacketStructure(PacketData packet)
        {
            if (string.IsNullOrEmpty(packet.HexDump))
                return "No hex data available";

            try
            {
                // Convert hex dump to byte array
                var hexData = ConvertHexDumpToByteArray(packet.HexDump);
                if (hexData.Length == 0)
                    return "No valid hex data found";

                var sb = new StringBuilder();
                sb.AppendLine($"Packet: {packet.OpcodeName} ({packet.OpcodeHex})");
                sb.AppendLine($"Size: {packet.Size} bytes");
                sb.AppendLine($"Direction: {packet.Direction}");
                sb.AppendLine();

                // Try to find and parse the appropriate structure
                if (TryParseWithReflection(packet.OpcodeName, hexData, packet.Direction, sb))
                {
                    return sb.ToString();
                }

                // Fallback to generic analysis if no structure found
                AppendGenericAnalysis(hexData, sb);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return $"Error parsing packet structure: {ex.Message}";
            }
        }

        /// <summary>
        /// Build mappings from opcodes to structure types using reflection
        /// </summary>
        private void BuildOpcodeToTypeMappings()
        {
            try
            {
                // Get all loaded assemblies that might contain EQProtocol structures
                var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => a.FullName?.Contains("EQProtocol") == true || a.GetName().Name == "EQProtocol")
                    .ToList();

                // Also check the current assembly (EQLogs) since it references EQProtocol types
                var currentAssembly = Assembly.GetExecutingAssembly();
                if (!assemblies.Contains(currentAssembly))
                {
                    assemblies.Add(currentAssembly);
                }

                // Get all assemblies that reference types implementing IEQStruct
                var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                Console.WriteLine($"[DEBUG] Checking {allAssemblies.Length} loaded assemblies for IEQStruct types");
                foreach (var assembly in allAssemblies)
                {
                    try
                    {
                        // Check if this assembly has any types that implement IEQStruct
                        var structTypes = assembly.GetTypes().Where(t => typeof(IEQStruct).IsAssignableFrom(t)).ToList();
                        var hasEQStructs = structTypes.Any();

                        Console.WriteLine($"[DEBUG] Assembly {assembly.GetName().Name}: {structTypes.Count} IEQStruct types");

                        if (hasEQStructs && !assemblies.Contains(assembly))
                        {
                            Console.WriteLine($"[DEBUG] Adding assembly with IEQStruct types: {assembly.GetName().Name}");
                            assemblies.Add(assembly);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[DEBUG] Error checking assembly {assembly.GetName().Name}: {ex.Message}");
                    }
                }

                Console.WriteLine($"[DEBUG] Total assemblies to process: {assemblies.Count}");

                // If no EQProtocol assembly found, try to load it
                if (!assemblies.Any())
                {
                    try
                    {
                        var eqProtocolAssembly = Assembly.LoadFrom("EQProtocol.dll");
                        assemblies.Add(eqProtocolAssembly);
                    }
                    catch
                    {
                        // Assembly not found, continue with what we have
                    }
                }

                foreach (var assembly in assemblies)
                {
                    try
                    {
                        Console.WriteLine($"[DEBUG] Processing assembly: {assembly.GetName().Name}");

                        // Find all types that implement IEQStruct
                        var structTypes = assembly.GetTypes()
                            .Where(t => t.IsClass || t.IsValueType)
                            .Where(t => typeof(IEQStruct).IsAssignableFrom(t))
                            .Where(t => !t.IsAbstract && !t.IsInterface)
                            .ToList();

                        Console.WriteLine($"[DEBUG] Found {structTypes.Count} IEQStruct types in {assembly.GetName().Name}");

                        int processedCount = 0;
                        int mappedCount = 0;

                        foreach (var structType in structTypes)
                        {
                            var typeName = structType.Name;
                            processedCount++;

                            // Extract opcode from type name using various patterns
                            var opcode = ExtractOpcodeFromTypeName(typeName);

                            // Debug first few and ClientUpdate types
                            if (processedCount <= 5 || typeName.Contains("ClientUpdate"))
                            {
                                Console.WriteLine($"[DEBUG] Processing {processedCount}/{structTypes.Count}: {typeName} -> {opcode ?? "NULL"}");
                            }

                            // Debug logging for all types in EQProtocol assembly - show first few
                            if (assembly.GetName().Name == "EQProtocol" && structTypes.IndexOf(structType) < 5)
                            {
                                System.Diagnostics.Debug.WriteLine($"Processing type: {typeName} -> {opcode ?? "NULL"}");
                            }

                            // Specifically log ClientUpdate types
                            if (typeName.Contains("ClientUpdate"))
                            {
                                System.Diagnostics.Debug.WriteLine($"*** FOUND ClientUpdate type in assembly {assembly.GetName().Name}: {typeName} -> {opcode ?? "NULL"}");
                            }

                            if (opcode != null)
                            {
                                mappedCount++;

                                // Debug logging for ClientUpdate types
                                if (typeName.Contains("ClientUpdate"))
                                {
                                    Console.WriteLine($"[DEBUG] Found ClientUpdate type: {typeName} -> {opcode}");
                                }

                                // Check for bidirectional packet suffixes
                                if (typeName.EndsWith("ToServer") || typeName.EndsWith("FromServer"))
                                {
                                    var baseOpcode = opcode;
                                    var isToServer = typeName.EndsWith("ToServer");

                                    if (typeName.Contains("ClientUpdate"))
                                    {
                                        Console.WriteLine($"[DEBUG] Bidirectional mapping: {typeName} -> {baseOpcode} ({(isToServer ? "ToServer" : "FromServer")})");
                                    }

                                    if (!_bidirectionalOpcodes.ContainsKey(baseOpcode))
                                    {
                                        _bidirectionalOpcodes[baseOpcode] = (null, null);
                                        if (typeName.Contains("ClientUpdate"))
                                        {
                                            Console.WriteLine($"[DEBUG] Created new bidirectional entry for {baseOpcode}");
                                        }
                                    }

                                    var current = _bidirectionalOpcodes[baseOpcode];
                                    if (isToServer)
                                    {
                                        _bidirectionalOpcodes[baseOpcode] = (structType, current.fromServer);
                                        if (typeName.Contains("ClientUpdate"))
                                        {
                                            Console.WriteLine($"[DEBUG] Set ToServer for {baseOpcode}: {structType.Name}");
                                        }
                                    }
                                    else
                                    {
                                        _bidirectionalOpcodes[baseOpcode] = (current.toServer, structType);
                                        if (typeName.Contains("ClientUpdate"))
                                        {
                                            Console.WriteLine($"[DEBUG] Set FromServer for {baseOpcode}: {structType.Name}");
                                        }
                                    }

                                    if (typeName.Contains("ClientUpdate"))
                                    {
                                        Console.WriteLine($"[DEBUG] Current bidirectional count: {_bidirectionalOpcodes.Count}");
                                    }
                                }
                                else
                                {
                                    // Single direction packet
                                    _opcodeToTypeMapping[opcode] = structType;
                                    System.Diagnostics.Debug.WriteLine($"Single direction mapping: {typeName} -> {opcode}");
                                }
                            }
                            else if (typeName.Contains("ClientUpdate"))
                            {
                                System.Diagnostics.Debug.WriteLine($"Failed to extract opcode from ClientUpdate type: {typeName}");
                            }
                        }

                        Console.WriteLine($"[DEBUG] Assembly {assembly.GetName().Name}: processed {processedCount}, mapped {mappedCount} structures");
                    }
                    catch (Exception ex)
                    {
                        // Log error but continue with other assemblies
                        System.Diagnostics.Debug.WriteLine($"Error processing assembly {assembly.FullName}: {ex.Message}");
                    }
                }

                System.Diagnostics.Debug.WriteLine($"Found {_opcodeToTypeMapping.Count} single-direction opcodes and {_bidirectionalOpcodes.Count} bidirectional opcodes");

                // Console output for debugging
                Console.WriteLine($"[DEBUG] Before final summary: {_opcodeToTypeMapping.Count} single-direction, {_bidirectionalOpcodes.Count} bidirectional");

                // Check if ClientUpdate is in the mappings
                if (_bidirectionalOpcodes.ContainsKey("OP_ClientUpdate"))
                {
                    var clientUpdate = _bidirectionalOpcodes["OP_ClientUpdate"];
                    Console.WriteLine($"[DEBUG] OP_ClientUpdate bidirectional: ToServer={clientUpdate.toServer?.Name}, FromServer={clientUpdate.fromServer?.Name}");
                }
                else
                {
                    Console.WriteLine($"[DEBUG] OP_ClientUpdate not found in bidirectional mappings");
                }

                Console.WriteLine($"[DEBUG] BuildOpcodeToTypeMappings complete: {_opcodeToTypeMapping.Count} single-direction, {_bidirectionalOpcodes.Count} bidirectional");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error building opcode mappings: {ex.Message}");
            }
        }

        /// <summary>
        /// Generate possible structure names that we would look for based on opcode and direction
        /// </summary>
        private List<string> GeneratePossibleStructureNames(string opcodeName, PacketDirection direction)
        {
            var possibleNames = new List<string>();

            // Remove OP_ prefix for base name generation
            var baseName = opcodeName.StartsWith("OP_") ? opcodeName.Substring(3) : opcodeName;

            // Bidirectional structure names
            if (direction == PacketDirection.ClientToServer)
            {
                possibleNames.Add($"{baseName}ToServer");
                possibleNames.Add($"{opcodeName}ToServer");
                possibleNames.Add($"{baseName}ClientToServer");
            }
            else
            {
                possibleNames.Add($"{baseName}FromServer");
                possibleNames.Add($"{opcodeName}FromServer");
                possibleNames.Add($"{baseName}ServerToClient");
            }

            // Single direction structure names
            possibleNames.Add(baseName);
            possibleNames.Add(opcodeName);

            // Common variations
            possibleNames.Add($"{baseName}Struct");
            possibleNames.Add($"{baseName}_Struct");
            possibleNames.Add($"{opcodeName}Struct");
            possibleNames.Add($"{opcodeName}_Struct");

            // Packet suffix variations
            possibleNames.Add($"{baseName}Packet");
            possibleNames.Add($"{baseName}Data");

            return possibleNames.Distinct().ToList();
        }

        /// <summary>
        /// Extract opcode name from type name using various patterns
        /// </summary>
        private string ExtractOpcodeFromTypeName(string typeName)
        {
            // Common patterns for EQ structure names
            var patterns = new[]
            {
                // With suffixes: "ClientUpdateToServer" -> "OP_ClientUpdate" (check this first)
                @"^([A-Z][a-zA-Z]+?)(?:ToServer|FromServer)$",
                // With "OP_" prefix: "OP_ClientUpdate" -> "OP_ClientUpdate"
                @"^OP_([A-Z][a-zA-Z]+)(?:ToServer|FromServer)?$",
                // Direct match: "ZoneEntry" -> "OP_ZoneEntry"
                @"^([A-Z][a-zA-Z]+)(?:ToServer|FromServer)?$"
            };

            // Debug logging specifically for ClientUpdate types
            if (typeName.Contains("ClientUpdate"))
            {
                System.Diagnostics.Debug.WriteLine($"ExtractOpcodeFromTypeName: Processing {typeName}");
                foreach (var pattern in patterns)
                {
                    var match = Regex.Match(typeName, pattern);
                    if (match.Success)
                    {
                        var baseName = match.Groups[1].Value;
                        var opcode = baseName.StartsWith("OP_") ? baseName : $"OP_{baseName}";
                        System.Diagnostics.Debug.WriteLine($"  Pattern '{pattern}' matched: baseName='{baseName}' opcode='{opcode}'");
                        return opcode;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"  Pattern '{pattern}' did not match");
                    }
                }
                System.Diagnostics.Debug.WriteLine($"  No patterns matched for {typeName}");
            }

            foreach (var pattern in patterns)
            {
                var match = Regex.Match(typeName, pattern);
                if (match.Success)
                {
                    var baseName = match.Groups[1].Value;
                    // Ensure it starts with OP_ for consistency
                    return baseName.StartsWith("OP_") ? baseName : $"OP_{baseName}";
                }
            }

            return null;
        }

        /// <summary>
        /// Try to parse packet using reflection to find the appropriate structure
        /// </summary>
        private bool TryParseWithReflection(string opcodeName, byte[] hexData, PacketDirection direction, StringBuilder sb)
        {
            Type structureType = null;
            var attemptedStructures = new List<string>();

            // First check for bidirectional packets
            if (_bidirectionalOpcodes.TryGetValue(opcodeName, out var bidirectional))
            {
                if (direction == PacketDirection.ClientToServer)
                {
                    structureType = bidirectional.toServer;
                    attemptedStructures.Add($"{opcodeName} -> {bidirectional.toServer?.Name ?? "null"} (ToServer)");
                }
                else
                {
                    structureType = bidirectional.fromServer;
                    attemptedStructures.Add($"{opcodeName} -> {bidirectional.fromServer?.Name ?? "null"} (FromServer)");
                }
            }

            // Fall back to single direction mapping
            if (structureType == null)
            {
                _opcodeToTypeMapping.TryGetValue(opcodeName, out structureType);
                if (structureType != null)
                {
                    attemptedStructures.Add($"{opcodeName} -> {structureType.Name} (Single direction)");
                }
                else
                {
                    attemptedStructures.Add($"{opcodeName} -> not found in single direction mappings");
                }
            }

            if (structureType == null)
            {
                sb.AppendLine($"No matching structure found for {opcodeName} ({direction})");
                sb.AppendLine();
                sb.AppendLine("Structure lookup attempts:");
                foreach (var attempt in attemptedStructures)
                {
                    sb.AppendLine($"  • {attempt}");
                }

                // Add suggestions for common naming patterns
                sb.AppendLine();
                sb.AppendLine("Possible structure names we looked for:");
                var possibleNames = GeneratePossibleStructureNames(opcodeName, direction);
                foreach (var name in possibleNames)
                {
                    sb.AppendLine($"  • {name}");
                }

                return false;
            }

            try
            {
                // Create instance of the structure
                var structInstance = Activator.CreateInstance(structureType) as IEQStruct;
                if (structInstance == null)
                {
                    return false;
                }

                // Unpack the data
                structInstance.Unpack(hexData);

                // Use the structure's ToString method
                sb.AppendLine($"{structureType.Name} structure (using EQProtocol reflection):");
                sb.AppendLine();
                sb.AppendLine(structInstance.ToString());

                return true;
            }
            catch (Exception ex)
            {
                sb.AppendLine($"Error parsing with {structureType.Name}: {ex.Message}");
                sb.AppendLine($"Data length: {hexData.Length} bytes");

                // Show some hex data for debugging
                sb.AppendLine();
                sb.AppendLine("Raw hex data (first 64 bytes):");
                AppendHexDump(hexData, sb, 64);

                return false;
            }
        }

        /// <summary>
        /// Convert hex dump string to byte array with proper parsing
        /// </summary>
        private byte[] ConvertHexDumpToByteArray(string hexDump)
        {
            var bytes = new List<byte>();
            var lines = hexDump.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                // Match hex dump format: "  OFFSET: HEX_BYTES | ASCII"
                var match = Regex.Match(line.Trim(), @"^\s*\d+:\s+([0-9A-Fa-f\s\-]+)\s+\|");
                if (match.Success)
                {
                    var hexPart = match.Groups[1].Value;
                    // Extract individual hex bytes
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

        /// <summary>
        /// Append generic packet analysis when no structure is found
        /// </summary>
        private void AppendGenericAnalysis(byte[] data, StringBuilder sb)
        {
            if (data.Length == 0) return;

            sb.AppendLine("Generic Analysis:");

            // Basic type analysis
            if (data.Length >= 2)
            {
                var uint16_0 = BitConverter.ToUInt16(data, 0);
                sb.AppendLine($"  Offset 0x0000: 0x{uint16_0:X4} ({uint16_0}) [UInt16]");

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

            // Look for coordinate patterns
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
            var strings = FindStrings(data);
            if (strings.Count > 0)
            {
                sb.AppendLine("  Possible strings:");
                foreach (var (offset, str) in strings.Take(3))
                {
                    sb.AppendLine($"    0x{offset:X4}: \"{str}\"");
                }
            }

            sb.AppendLine();
            sb.AppendLine("Raw hex data (first 128 bytes):");
            AppendHexDump(data, sb, 128);
        }

        /// <summary>
        /// Find possible strings in binary data
        /// </summary>
        private List<(int offset, string text)> FindStrings(byte[] data)
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

        /// <summary>
        /// Append formatted hex dump to StringBuilder
        /// </summary>
        private void AppendHexDump(byte[] data, StringBuilder sb, int maxBytes)
        {
            for (int i = 0; i < Math.Min(data.Length, maxBytes); i += 16)
            {
                var chunk = data.Skip(i).Take(16).ToArray();
                var hex = string.Join(" ", chunk.Select(b => b.ToString("X2")));
                var ascii = string.Join("", chunk.Select(b => b >= 32 && b < 127 ? (char)b : '.'));
                sb.AppendLine($"  {i:X4}: {hex.PadRight(47)} | {ascii}");
            }

            if (data.Length > maxBytes)
                sb.AppendLine($"  ... ({data.Length - maxBytes} more bytes)");
        }

        /// <summary>
        /// Get diagnostic information about the loaded opcodes
        /// </summary>
        public string GetDiagnosticInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Generic Packet Parser Service - Diagnostic Info");
            sb.AppendLine($"================================================");
            sb.AppendLine($"Single-direction opcodes: {_opcodeToTypeMapping.Count}");
            sb.AppendLine($"Bidirectional opcodes: {_bidirectionalOpcodes.Count}");

            // Count total available structures
            var totalStructures = _opcodeToTypeMapping.Count +
                                 _bidirectionalOpcodes.Values.Count(x => x.toServer != null) +
                                 _bidirectionalOpcodes.Values.Count(x => x.fromServer != null);
            sb.AppendLine($"Total available structures: {totalStructures}");
            sb.AppendLine();

            // Show assembly information
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var eqProtocolAssemblies = assemblies.Where(a => a.FullName?.Contains("EQProtocol") == true || a.GetName().Name == "EQProtocol").ToList();
            var structAssemblies = assemblies.Where(a =>
            {
                try
                {
                    return a.GetTypes().Any(t => typeof(IEQStruct).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);
                }
                catch { return false; }
            }).ToList();

            sb.AppendLine($"EQProtocol assemblies: {eqProtocolAssemblies.Count}");
            foreach (var assembly in eqProtocolAssemblies)
            {
                sb.AppendLine($"  • {assembly.GetName().Name} v{assembly.GetName().Version}");
            }

            sb.AppendLine($"Assemblies with IEQStruct types: {structAssemblies.Count}");
            foreach (var assembly in structAssemblies)
            {
                var structCount = assembly.GetTypes().Count(t => typeof(IEQStruct).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);
                sb.AppendLine($"  • {assembly.GetName().Name} ({structCount} structures)");
            }
            sb.AppendLine();

            sb.AppendLine("Single-direction mappings:");
            foreach (var kvp in _opcodeToTypeMapping.OrderBy(x => x.Key))
            {
                sb.AppendLine($"  ✓ {kvp.Key} -> {kvp.Value.Name}");
            }

            sb.AppendLine();
            sb.AppendLine("Bidirectional mappings:");
            foreach (var kvp in _bidirectionalOpcodes.OrderBy(x => x.Key))
            {
                var toServerName = kvp.Value.toServer?.Name ?? "❌ missing";
                var fromServerName = kvp.Value.fromServer?.Name ?? "❌ missing";
                sb.AppendLine($"  {kvp.Key}:");
                sb.AppendLine($"    → ToServer:   {toServerName}");
                sb.AppendLine($"    → FromServer: {fromServerName}");
            }

            // Show some statistics about incomplete mappings
            var incompleteBidirectional = _bidirectionalOpcodes.Count(kvp =>
                kvp.Value.toServer == null || kvp.Value.fromServer == null);

            if (incompleteBidirectional > 0)
            {
                sb.AppendLine();
                sb.AppendLine($"⚠️  Incomplete bidirectional mappings: {incompleteBidirectional}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Get a detailed report for a specific opcode (useful for debugging)
        /// </summary>
        public string GetOpcodeReport(string opcodeName, PacketDirection? direction = null)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Opcode Report: {opcodeName}");
            sb.AppendLine($"====================");

            // Check single direction mapping
            if (_opcodeToTypeMapping.TryGetValue(opcodeName, out var singleType))
            {
                sb.AppendLine($"✓ Single-direction mapping found: {singleType.Name}");
                sb.AppendLine($"  Assembly: {singleType.Assembly.GetName().Name}");
                sb.AppendLine($"  Namespace: {singleType.Namespace}");
            }
            else
            {
                sb.AppendLine($"❌ No single-direction mapping found");
            }

            // Check bidirectional mapping
            if (_bidirectionalOpcodes.TryGetValue(opcodeName, out var bidirectional))
            {
                sb.AppendLine($"Bidirectional mapping found:");
                if (bidirectional.toServer != null)
                {
                    sb.AppendLine($"  ✓ ToServer: {bidirectional.toServer.Name}");
                    sb.AppendLine($"    Assembly: {bidirectional.toServer.Assembly.GetName().Name}");
                }
                else
                {
                    sb.AppendLine($"  ❌ ToServer: missing");
                }

                if (bidirectional.fromServer != null)
                {
                    sb.AppendLine($"  ✓ FromServer: {bidirectional.fromServer.Name}");
                    sb.AppendLine($"    Assembly: {bidirectional.fromServer.Assembly.GetName().Name}");
                }
                else
                {
                    sb.AppendLine($"  ❌ FromServer: missing");
                }
            }
            else
            {
                sb.AppendLine($"❌ No bidirectional mapping found");
            }

            // Show what we would look for
            if (direction.HasValue)
            {
                sb.AppendLine();
                sb.AppendLine($"For direction {direction.Value}, we would look for:");
                var possibleNames = GeneratePossibleStructureNames(opcodeName, direction.Value);
                foreach (var name in possibleNames)
                {
                    sb.AppendLine($"  • {name}");
                }
            }

            return sb.ToString();
        }
    }
}