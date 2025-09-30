using EQLogs.Models;
using System;
using System.Linq;

namespace EQLogs.Services
{
    /// <summary>
    /// Simple test class to verify the generic packet parser is working correctly
    /// </summary>
    public static class PacketParserTest
    {
        public static void RunBasicTest()
        {
            try
            {
                Console.WriteLine("=== Enhanced Packet Parser and Session Filtering Test ===");

                // Test session parsing
                TestSessionParsing();
                Console.WriteLine();

                // Test generic parser
                TestGenericParser();
                Console.WriteLine();

                Console.WriteLine("=== Test Complete ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed with error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Direct test method that can be called from console apps for debugging
        /// </summary>
        public static void TestStructureDiscovery()
        {
            Console.WriteLine("=== Structure Discovery Debug Test ===");

            try
            {
                // Create the parser first to trigger assembly loading
                Console.WriteLine("=== Creating GenericPacketParserService to load assemblies ===");
                var parser = new GenericPacketParserService();

                // Now examine what types we can find directly
                Console.WriteLine("=== Examining EQProtocol Assembly Types ===");
                var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();

                Console.WriteLine("All loaded assemblies:");
                foreach (var assembly in assemblies)
                {
                    Console.WriteLine($"  • {assembly.GetName().Name} (FullName: {assembly.FullName})");
                }
                Console.WriteLine();

                var eqProtocolAssembly = assemblies.FirstOrDefault(a => a.GetName().Name == "EQProtocol");

                if (eqProtocolAssembly != null)
                {
                    var structTypes = eqProtocolAssembly.GetTypes()
                        .Where(t => typeof(EQProtocol.Streams.Common.IEQStruct).IsAssignableFrom(t))
                        .Where(t => !t.IsAbstract && !t.IsInterface)
                        .ToList();

                    Console.WriteLine($"Found {structTypes.Count} IEQStruct types in EQProtocol assembly");

                    // Show first 20 types for inspection
                    Console.WriteLine("First 20 types:");
                    foreach (var type in structTypes.Take(20))
                    {
                        Console.WriteLine($"  • {type.Name} (Full: {type.FullName})");
                        if (type.Name.Contains("ClientUpdate"))
                        {
                            Console.WriteLine($"    *** FOUND ClientUpdate type: {type.Name} ***");
                        }
                    }

                    // Test first few types from EQProtocol with regex patterns
                    Console.WriteLine("\nTesting first 5 EQProtocol types with regex patterns:");
                    foreach (var type in structTypes.Take(5))
                    {
                        Console.WriteLine($"Testing {type.Name}:");

                        var patterns = new[]
                        {
                            @"^([A-Z][a-zA-Z]+?)(?:ToServer|FromServer)$",
                            @"^OP_([A-Z][a-zA-Z]+)(?:ToServer|FromServer)?$",
                            @"^([A-Z][a-zA-Z]+)(?:ToServer|FromServer)?$"
                        };

                        bool matched = false;
                        foreach (var pattern in patterns)
                        {
                            var match = System.Text.RegularExpressions.Regex.Match(type.Name, pattern);
                            if (match.Success)
                            {
                                var baseName = match.Groups[1].Value;
                                var opcode = baseName.StartsWith("OP_") ? baseName : $"OP_{baseName}";
                                Console.WriteLine($"  ✓ Pattern '{pattern}' matched: baseName='{baseName}' opcode='{opcode}'");
                                matched = true;
                                break;
                            }
                        }

                        if (!matched)
                        {
                            Console.WriteLine($"  ❌ No patterns matched for {type.Name}");
                        }
                    }

                    // Look specifically for ClientUpdate types
                    var clientUpdateTypes = structTypes.Where(t => t.Name.Contains("ClientUpdate")).ToList();
                    Console.WriteLine($"\nFound {clientUpdateTypes.Count} ClientUpdate types:");
                    foreach (var type in clientUpdateTypes)
                    {
                        Console.WriteLine($"  *** {type.Name} (Full: {type.FullName}) ***");

                        // Test the ExtractOpcodeFromTypeName method directly
                        Console.WriteLine($"      Testing regex patterns for {type.Name}:");

                        var patterns = new[]
                        {
                            @"^([A-Z][a-zA-Z]+?)(?:ToServer|FromServer)$",
                            @"^OP_([A-Z][a-zA-Z]+)(?:ToServer|FromServer)?$",
                            @"^([A-Z][a-zA-Z]+)(?:ToServer|FromServer)?$"
                        };

                        bool matched = false;
                        foreach (var pattern in patterns)
                        {
                            var match = System.Text.RegularExpressions.Regex.Match(type.Name, pattern);
                            if (match.Success)
                            {
                                var baseName = match.Groups[1].Value;
                                var opcode = baseName.StartsWith("OP_") ? baseName : $"OP_{baseName}";
                                Console.WriteLine($"        ✓ Pattern '{pattern}' matched: baseName='{baseName}' opcode='{opcode}'");
                                matched = true;
                                break;
                            }
                            else
                            {
                                Console.WriteLine($"        ❌ Pattern '{pattern}' did not match");
                            }
                        }

                        if (!matched)
                        {
                            Console.WriteLine($"        ❌ No patterns matched for {type.Name}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("EQProtocol assembly not found!");
                }

                Console.WriteLine("\n=== Now testing GenericPacketParserService ===");

                // Get diagnostic info
                Console.WriteLine(parser.GetDiagnosticInfo());
                Console.WriteLine();

                // Test OP_ClientUpdate specifically
                Console.WriteLine("=== OP_ClientUpdate Specific Test ===");
                Console.WriteLine(parser.GetOpcodeReport("OP_ClientUpdate", PacketDirection.ServerToClient));
                Console.WriteLine();
                Console.WriteLine(parser.GetOpcodeReport("OP_ClientUpdate", PacketDirection.ClientToServer));

                Console.WriteLine("\n=== Structure Discovery Debug Complete ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Structure discovery test failed: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        private static void TestSessionParsing()
        {
            Console.WriteLine("Testing Session Information Parsing:");
            Console.WriteLine("=====================================");

            // Test the ACTUAL log format you're seeing with Session number from log
            var actualLogLine = "[09-29-2025 10:39:31] [Zone] [Packet C->S] [OP_ReqNewZone] [0x4118] Size [2] Session [2] Account [2:bifar] Player [Bifar]";
            var actualPacket = PacketData.ParseFromLogLine(actualLogLine);
            if (actualPacket != null)
            {
                Console.WriteLine($"✅ ACTUAL log format parsed successfully:");
                Console.WriteLine($"  Original Line: {actualLogLine}");
                Console.WriteLine($"  Account ID: '{actualPacket.AccountId}'");
                Console.WriteLine($"  Account Name: '{actualPacket.AccountName}'");
                Console.WriteLine($"  Player Name: '{actualPacket.PlayerName}'");
                Console.WriteLine($"  Session Key: '{actualPacket.SessionKey}' (Session {actualPacket.SessionNumber})");
                Console.WriteLine($"  Session Number: {actualPacket.SessionNumber} (from log file)");
                Console.WriteLine($"  Display Text: {actualPacket.DisplayText}");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"❌ FAILED to parse actual log line: {actualLogLine}");
                Console.WriteLine();
            }

            // Test Zone/World format with Account and Player (older format without session in log)
            var zoneLogLine = "[2025-01-15 10:30:45] [Zone] [Packet S->C] [OP_ClientUpdate] [0x0019] Size [22] Account [1:bakak] Player [Bakak]";
            var packet1 = PacketData.ParseFromLogLine(zoneLogLine);
            if (packet1 != null)
            {
                packet1.SessionNumber = 1; // Simulate session number assignment
                Console.WriteLine($"Zone packet parsed successfully:");
                Console.WriteLine($"  Original Line: {zoneLogLine}");
                Console.WriteLine($"  Account ID: '{packet1.AccountId}'");
                Console.WriteLine($"  Account Name: '{packet1.AccountName}'");
                Console.WriteLine($"  Player Name: '{packet1.PlayerName}'");
                Console.WriteLine($"  Session Key: '{packet1.SessionKey}'");
                Console.WriteLine($"  Session Number: {packet1.SessionNumber}");
                Console.WriteLine($"  Display Text: {packet1.DisplayText}");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"❌ Failed to parse Zone line: {zoneLogLine}");
                Console.WriteLine();
            }

            // Test Login format with Account only
            var loginLogLine = "[2025-01-15 10:25:12] [Login] [Packet C->S] [OP_LoginRequest] [0x0001] Size [64] Account [1:bakak]";
            var packet2 = PacketData.ParseFromLogLine(loginLogLine);
            if (packet2 != null)
            {
                packet2.SessionNumber = 1; // Simulate session number assignment
                Console.WriteLine($"Login packet parsed successfully:");
                Console.WriteLine($"  Original Line: {loginLogLine}");
                Console.WriteLine($"  Account ID: '{packet2.AccountId}'");
                Console.WriteLine($"  Account Name: '{packet2.AccountName}'");
                Console.WriteLine($"  Player Name: '{packet2.PlayerName}'");
                Console.WriteLine($"  Session Key: '{packet2.SessionKey}'");
                Console.WriteLine($"  Session Number: {packet2.SessionNumber}");
                Console.WriteLine($"  Display Text: {packet2.DisplayText}");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"❌ Failed to parse Login line: {loginLogLine}");
                Console.WriteLine();
            }

            // Test legacy format (backward compatibility)
            var legacyLogLine = "[2025-01-15 10:20:30] [Zone] [Packet S->C] [OP_ZoneEntry] [0x0061] Size [68]";
            var packet3 = PacketData.ParseFromLogLine(legacyLogLine);
            if (packet3 != null)
            {
                packet3.SessionNumber = 0; // No session info
                Console.WriteLine($"Legacy packet parsed successfully:");
                Console.WriteLine($"  Original Line: {legacyLogLine}");
                Console.WriteLine($"  Session Key: '{packet3.SessionKey}' (should be empty)");
                Console.WriteLine($"  Session Number: {packet3.SessionNumber}");
                Console.WriteLine($"  Display Text: {packet3.DisplayText}");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"❌ Failed to parse Legacy line: {legacyLogLine}");
                Console.WriteLine();
            }

            // Test Login format with Session number
            var loginWithSessionLine = "[09-29-2025 10:39:31] [Login] [Packet C->S] [OP_LoginRequest] [0x0001] Size [64] Session [2] Account [2:bifar]";
            var loginWithSession = PacketData.ParseFromLogLine(loginWithSessionLine);
            if (loginWithSession != null)
            {
                Console.WriteLine($"✅ Login with session parsed successfully:");
                Console.WriteLine($"  Original Line: {loginWithSessionLine}");
                Console.WriteLine($"  Account ID: '{loginWithSession.AccountId}'");
                Console.WriteLine($"  Account Name: '{loginWithSession.AccountName}'");
                Console.WriteLine($"  Session Number: {loginWithSession.SessionNumber} (from log file)");
                Console.WriteLine($"  Display Text: {loginWithSession.DisplayText}");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"❌ FAILED to parse login with session line: {loginWithSessionLine}");
                Console.WriteLine();
            }

            // Test problematic case that might be causing Account [:] issue
            Console.WriteLine("Testing edge cases that might cause Account [:] issue:");
            var edgeCases = new[]
            {
                "[2025-01-15 10:30:45] [Zone] [Packet S->C] [OP_ClientUpdate] [0x0019] Size [22] Account [] Player [Bakak]",
                "[2025-01-15 10:30:45] [Zone] [Packet S->C] [OP_ClientUpdate] [0x0019] Size [22] Account [:] Player [Bakak]",
                "[2025-01-15 10:30:45] [Zone] [Packet S->C] [OP_ClientUpdate] [0x0019] Size [22]",
                // Test the exact format from your server
                "[09-29-2025 10:39:31] [Zone] [Packet C->S] [OP_ReqNewZone] [0x4118] Size [2] Session [2] Account [2:bifar] Player [Bifar]"
            };

            foreach (var testLine in edgeCases)
            {
                var testPacket = PacketData.ParseFromLogLine(testLine);
                if (testPacket != null)
                {
                    Console.WriteLine($"  ✅ Edge case parsed: AccountId='{testPacket.AccountId}', AccountName='{testPacket.AccountName}', SessionNum={testPacket.SessionNumber}");
                    Console.WriteLine($"      Display: {testPacket.DisplayText}");
                }
                else
                {
                    Console.WriteLine($"  ❌ Edge case failed to parse: {testLine}");
                }
                Console.WriteLine();
            }
        }

        private static void TestGenericParser()
        {
            Console.WriteLine("Testing Generic Packet Parser:");
            Console.WriteLine("==============================");

            var parser = new GenericPacketParserService();
            Console.WriteLine(parser.GetDiagnosticInfo());
            Console.WriteLine();

            // Test specific OP_ClientUpdate opcode report
            Console.WriteLine("=== OP_ClientUpdate Opcode Report ===");
            Console.WriteLine(parser.GetOpcodeReport("OP_ClientUpdate", PacketDirection.ServerToClient));
            Console.WriteLine();
            Console.WriteLine(parser.GetOpcodeReport("OP_ClientUpdate", PacketDirection.ClientToServer));
            Console.WriteLine();

            // Test with a known structure
            var testPacket1 = new PacketData
            {
                OpcodeName = "OP_ClientUpdate",
                Direction = PacketDirection.ServerToClient,
                HexDump = @"0: 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F 10 | ................
16: 11 12 13 14 15 16 17 18 19 1A 1B 1C 1D 1E 1F 20 | ............... ",
                Size = 32,
                AccountId = "1",
                AccountName = "bakak",
                PlayerName = "Bakak"
            };

            Console.WriteLine("Test parsing OP_ClientUpdate with session info:");
            var result1 = parser.ParsePacketStructure(testPacket1);
            Console.WriteLine(result1);
            Console.WriteLine();

            // Test with an unknown structure to show enhanced error reporting
            var testPacket2 = new PacketData
            {
                OpcodeName = "OP_UnknownPacket",
                Direction = PacketDirection.ClientToServer,
                HexDump = @"0: AA BB CC DD EE FF 11 22 33 44 55 66 77 88 99 00 | ................",
                Size = 16,
                AccountId = "2",
                AccountName = "testuser"
            };

            Console.WriteLine("Test parsing unknown packet (enhanced error reporting):");
            var result2 = parser.ParsePacketStructure(testPacket2);
            Console.WriteLine(result2);
            Console.WriteLine();
        }
    }
}