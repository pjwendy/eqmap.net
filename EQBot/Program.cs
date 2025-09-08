using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using OpenEQ.Netcode;
using System.Collections.Generic;

namespace EQBot
{
    public class Program
    {
        private static ILogger<Program>? _logger;
        private static IConfiguration? _config;

        public static async Task MainOriginal(string[] args)
        {
            // Use the new SimpleBot instead of the original complex bot
            await SimpleBotProgram.Main(args);
            
            // Original complex bot implementation - COMMENTED OUT
            /*
            var host = CreateHostBuilder(args).Build();
            
            _logger = host.Services.GetRequiredService<ILogger<Program>>();
            _config = host.Services.GetRequiredService<IConfiguration>();

            _logger.LogInformation("Starting EverQuest Bot");

            var bot = new EQBotClient(_config, _logger);
            await bot.StartAsync();

            _logger.LogInformation("Bot has exited");
            */
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.Configure<EQServerConfig>(
                        context.Configuration.GetSection("EQServer"));
                });
    }


    // Original EQBotClient - COMMENTED OUT (replaced by EQGameClient abstraction)
    /*
    public class EQBotClient
    {
        private readonly IConfiguration _config;
        private readonly ILogger<Program> _logger;
        private LoginStream? _loginStream;
        private WorldStream? _worldStream;
        private ZoneStream? _zoneStream;
        private string _characterName = "";
        private bool _isRunning = true;
        
        private TaskCompletionSource<bool>? _loginCompletionSource;
        private TaskCompletionSource<bool>? _worldConnectionCompletionSource;

        public EQBotClient(IConfiguration config, ILogger<Program> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task StartAsync()
        {
            var serverConfig = _config.GetSection("EQServer").Get<EQServerConfig>();
            if (serverConfig == null)
            {
                _logger.LogError("No EQServer configuration found");
                return;
            }

            _logger.LogInformation("Connecting to EverQuest server...");
            _logger.LogInformation("Login Server: {LoginServer}:{LoginPort}", 
                serverConfig.LoginServer, serverConfig.LoginServerPort);
            _logger.LogInformation("Username: {Username}, Character: {Character}", 
                serverConfig.Username, serverConfig.CharacterName);

            try
            {
                await ConnectToLoginServerAsync(serverConfig);
                
                // Keep the bot running
                while (_isRunning)
                {
                    await Task.Delay(1000);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during bot execution");
            }
            finally
            {
                Disconnect();
            }
        }

        private async Task ConnectToLoginServerAsync(EQServerConfig config)
        {
            _logger.LogInformation("Connecting to login server...");
            
            _loginCompletionSource = new TaskCompletionSource<bool>();
            _worldConnectionCompletionSource = new TaskCompletionSource<bool>();
            
            _loginStream = new LoginStream(config.LoginServer, config.LoginServerPort);
            _loginStream.Debug = true; // Enable detailed packet logging

            // Set up login event handlers
            _loginStream.LoginSuccess += async (sender, success) =>
            {
                if (success)
                {
                    _logger.LogInformation("Login successful! AccountID: {AccountID}", _loginStream.AccountID);
                    _logger.LogInformation("Requesting server list...");
                    _loginStream.RequestServerList();
                }
                else
                {
                    _logger.LogError("Login failed");
                    _isRunning = false;
                    _loginCompletionSource?.SetResult(false);
                }
            };

            _loginStream.ServerList += async (sender, servers) =>
            {
                _logger.LogInformation("=== RECEIVED SERVER LIST ===");
                _logger.LogInformation("Total servers available: {Count}", servers.Count);
                
                // List all available servers with detailed info
                for (int i = 0; i < servers.Count; i++)
                {
                    var server = servers[i];
                    _logger.LogInformation("Server {Index}: '{ServerName}' - Runtime ID: {RuntimeID}, World IP: {WorldIP}", 
                        i + 1, server.Longname, server.RuntimeID, server.WorldIP);
                }
                
                _logger.LogInformation("Looking for server matching: '{WorldName}'", config.WorldName);
                
                ServerListElement? targetServer = null;
                foreach (var server in servers)
                {
                    _logger.LogInformation("Checking server: '{ServerName}' against '{WorldName}'", 
                        server.Longname, config.WorldName);
                    
                    if (server.Longname.Contains(config.WorldName, StringComparison.OrdinalIgnoreCase))
                    {
                        _logger.LogInformation("✅ MATCH FOUND: Server '{ServerName}' matches '{WorldName}'", 
                            server.Longname, config.WorldName);
                        targetServer = server;
                        break;
                    }
                    else
                    {
                        _logger.LogInformation("❌ No match: '{ServerName}' != '{WorldName}'", 
                            server.Longname, config.WorldName);
                    }
                }

                if (targetServer != null)
                {
                    _logger.LogInformation("✅ SELECTING SERVER: '{ServerName}' - Sending PlayEverquestRequest", targetServer.Value.Longname);
                    
                    // Send the PlayEverquestRequest to select this world server
                    _loginStream.Play(targetServer.Value);
                }
                else
                {
                    _logger.LogError("Target world server '{WorldName}' not found", config.WorldName);
                    _isRunning = false;
                    _loginCompletionSource?.SetResult(false);
                }
            };

            _loginStream.PlaySuccess += async (sender, selectedServer) =>
            {
                if (selectedServer.HasValue)
                {
                    _logger.LogInformation("✅ PlayEverquestResponse SUCCESS: Server '{ServerName}' accepted connection", 
                        selectedServer.Value.Longname);
                    _logger.LogInformation("Ready to connect to world server at: {WorldIP}:9000", 
                        selectedServer.Value.WorldIP);
                    
                    // Now actually connect to world server
                    _logger.LogInformation("Connecting to world server: {ServerName}", selectedServer.Value.Longname);
                    await ConnectToWorldServerAsync(selectedServer.Value, config);
                    _loginCompletionSource?.SetResult(true);
                }
                else
                {
                    _logger.LogError("❌ PlayEverquestResponse FAILED: Server rejected connection");
                    _isRunning = false;
                    _loginCompletionSource?.SetResult(false);
                }
            };

            // Start login process
            _logger.LogInformation("Sending login request...");
            _loginStream.Login(config.Username, config.Password);

            // Wait for the entire login and world connection process to complete
            var loginSuccess = await _loginCompletionSource.Task;
            if (!loginSuccess)
            {
                _logger.LogError("Login process failed");
                _isRunning = false;
            }
        }

        private async Task ConnectToWorldServerAsync(ServerListElement server, EQServerConfig config)
        {
            _logger.LogInformation("Connecting to world server at {IP}:9000...", server.WorldIP);
            
            if (_loginStream == null)
            {
                _logger.LogError("Login stream is null");
                return;
            }

            _worldStream = new WorldStream(server.WorldIP, 9000, _loginStream.AccountID, _loginStream.SessionKey);
            _worldStream.Debug = true; // Enable detailed packet logging

            // Set up world event handlers
            _worldStream.CharacterList += async (sender, characters) =>
            {
                _logger.LogInformation("Received character list with {Count} characters", characters.Count);
                
                CharacterSelectEntry? targetCharacter = null;
                foreach (var character in characters)
                {
                    _logger.LogInformation("Available character: {CharacterName} (Level {Level})", character.Name, character.Level);
                    if (character.Name.Equals(config.CharacterName, StringComparison.OrdinalIgnoreCase))
                    {
                        targetCharacter = character;
                        break;
                    }
                }

                if (targetCharacter != null)
                {
                    _logger.LogInformation("Entering world with character: {CharacterName}", targetCharacter.Value.Name);
                    _characterName = targetCharacter.Value.Name;
                    _worldStream.SendEnterWorld(new EnterWorld(targetCharacter.Value.Name, false, false));
                }
                else
                {
                    _logger.LogError("Target character '{CharacterName}' not found", config.CharacterName);
                    _isRunning = false;
                }
            };

            _worldStream.ZoneServer += async (sender, zoneServer) =>
            {
                _logger.LogInformation("Received zone server info: {Host}:{Port}", zoneServer.Host, zoneServer.Port);
                await ConnectToZoneServerAsync(zoneServer, config);
            };

            _worldStream.MOTD += (sender, motd) =>
            {
                _logger.LogInformation("Message of the Day: {MOTD}", motd?.Replace("\0", "").Trim());
            };

            // Wait for world server connection
            await Task.Delay(5000);
        }

        private async Task ConnectToZoneServerAsync(ZoneServerInfo zoneServer, EQServerConfig config)
        {
            _logger.LogInformation("Connecting to zone server...");
            
            _zoneStream = new ZoneStream(zoneServer.Host, zoneServer.Port, _characterName);
            _zoneStream.Debug = true; // Enable detailed packet logging

            // Set up zone event handlers
            _zoneStream.PlayerProfile += (sender, player) =>
            {
                _logger.LogInformation("Player profile loaded: {Name} Level {Level} in Zone {ZoneID}", 
                    player.Name, player.Level, player.ZoneID);
                _logger.LogInformation("Position: X={X}, Y={Y}, Z={Z}", 
                    player.X, player.Y, player.Z);
                
                // Bot is now fully connected and in-game!
                OnBotReady();
            };

            _zoneStream.Spawned += (sender, spawn) =>
            {
                if (spawn.Name != _characterName) // Don't log our own character
                {
                    _logger.LogInformation("Spawn detected: {Name} ({Type}) at X={X}, Y={Y}", 
                        spawn.Name, spawn.CharType, spawn.Position.X, spawn.Position.Y);
                }
            };

            _zoneStream.Message += (sender, message) =>
            {
                _logger.LogInformation("Message: [{ChanNum}] {From}: {Message}", 
                    message.ChanNum, message.From, message.Message);
            };

            _zoneStream.Death += (sender, death) =>
            {
                _logger.LogInformation("Death event: SpawnID {SpawnID}", death.SpawnId);
            };

            _zoneStream.Zoned += (sender, zone) =>
            {
                _logger.LogInformation("Zone change detected: {ZoneID}", zone);
            };

            // Wait for zone connection
            await Task.Delay(3000);
        }

        private void OnBotReady()
        {
            _logger.LogInformation("=== BOT IS READY AND IN-GAME ===");
            
            // Example bot behavior - say hello
            Task.Run(async () =>
            {
                await Task.Delay(2000);
                // Temporarily disable chat message to prevent ACK timeout
                // _zoneStream?.SendChatMessage("", "", "Hello! I'm an EverQuest bot!");
                
                // Add more bot behavior here
                await RunBotBehaviorAsync();
            });
        }

        private async Task RunBotBehaviorAsync()
        {
            _logger.LogInformation("Starting bot behavior loop...");
            
            while (_isRunning && _zoneStream != null)
            {
                // Example bot behaviors:
                
                // 1. Send a message every 30 seconds
                await Task.Delay(30000);
                if (_zoneStream != null && _isRunning)
                {
                    // Temporarily disable recurring chat messages
                    // _zoneStream.SendChatMessage("", "", "Bot is still running...");
                }
                
                // Add more sophisticated bot logic here:
                // - Combat logic
                // - Movement
                // - Inventory management
                // - etc.
            }
        }

        private void Disconnect()
        {
            _logger.LogInformation("Disconnecting from EverQuest...");
            
            _zoneStream?.Disconnect();
            _worldStream?.Disconnect();
            _loginStream?.Disconnect();
            
            _zoneStream = null;
            _worldStream = null;
            _loginStream = null;
        }
    }
    */
}