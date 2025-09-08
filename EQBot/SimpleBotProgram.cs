using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using OpenEQ.Netcode.GameClient;
using OpenEQ.Netcode.GameClient.Models;

namespace EQBot
{
    public class SimpleBotProgram
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            var logger = host.Services.GetRequiredService<ILogger<SimpleBotProgram>>();
            var config = host.Services.GetRequiredService<IConfiguration>();
            var gameClientLogger = host.Services.GetRequiredService<ILogger<EQGameClient>>();

            logger.LogInformation("=== EverQuest Simple Bot ===");

            var gameClient = new EQGameClient(gameClientLogger);
            var simpleBot = new SimpleBot(gameClient, logger);

            try
            {
                // Debug configuration loading
                logger.LogDebug("Looking for EQServer configuration...");
                var eqServerSection = config.GetSection("EQServer");
                logger.LogDebug("EQServer section exists: {Exists}", eqServerSection.Exists());
                
                // Get connection settings
                var serverConfig = eqServerSection.Get<EQServerConfig>();
                if (serverConfig == null)
                {
                    logger.LogError("No EQServer configuration found");
                    logger.LogDebug("All config keys: {Keys}", string.Join(", ", config.AsEnumerable().Select(kv => kv.Key)));
                    return;
                }
                
                logger.LogDebug("Found EQServer config - LoginServer: {LoginServer}, Username: {Username}", 
                    serverConfig.LoginServer, serverConfig.Username);

                // Configure game client
                gameClient.LoginServer = serverConfig.LoginServer;
                gameClient.LoginServerPort = serverConfig.LoginServerPort;

                // Start the bot
                await simpleBot.RunAsync(
                    serverConfig.Username, 
                    serverConfig.Password, 
                    serverConfig.WorldName, 
                    serverConfig.CharacterName);

                logger.LogInformation("Bot has exited");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during bot execution");
            }
            finally
            {
                gameClient.Dispose();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    // Ensure we load appsettings.json from the project directory
                    var projectDir = System.IO.Path.GetDirectoryName(typeof(SimpleBotProgram).Assembly.Location);
                    config.SetBasePath(projectDir);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Debug))
                .ConfigureServices((context, services) =>
                {
                    services.Configure<EQServerConfig>(
                        context.Configuration.GetSection("EQServer"));
                });
    }

    public class SimpleBot
    {
        private readonly EQGameClient _gameClient;
        private readonly ILogger<SimpleBotProgram> _logger;
        private bool _isRunning = true;

        public SimpleBot(EQGameClient gameClient, ILogger<SimpleBotProgram> logger)
        {
            _gameClient = gameClient;
            _logger = logger;

            // Set up event handlers for high-level game events
            _gameClient.ConnectionStateChanged += OnConnectionStateChanged;
            _gameClient.CharacterLoaded += OnCharacterLoaded;
            _gameClient.NPCSpawned += OnNPCSpawned;
            _gameClient.PlayerSpawned += OnPlayerSpawned;
            _gameClient.ChatMessageReceived += OnChatMessageReceived;
            _gameClient.LoginFailed += OnLoginFailed;
            _gameClient.Disconnected += OnDisconnected;
        }

        public async Task RunAsync(string username, string password, string worldName, string characterName)
        {
            _logger.LogInformation("Starting bot for {Username} -> {WorldName} -> {CharacterName}", 
                username, worldName, characterName);

            // Single method call handles entire login sequence!
            var loginSuccess = await _gameClient.LoginAsync(username, password, worldName, characterName);
            
            if (!loginSuccess)
            {
                _logger.LogError("Failed to login to game");
                return;
            }

            // Bot is now in-game and ready!
            await RunBotBehaviorAsync();
        }

        private async Task RunBotBehaviorAsync()
        {
            _logger.LogInformation("Bot behavior started");

            while (_isRunning && _gameClient.State == ConnectionState.InGame)
            {
                // Example bot behaviors using the simple API:

                // 1. Check character status
                if (_gameClient.Character != null)
                {
                    _logger.LogDebug("Character status: HP {HP}/{MaxHP}, Pos [{X:F1}, {Y:F1}, {Z:F1}]",
                        _gameClient.Character.HP, _gameClient.Character.MaxHP,
                        _gameClient.Character.X, _gameClient.Character.Y, _gameClient.Character.Z);
                }

                // 2. Look for nearby NPCs
                var nearbyNPCs = _gameClient.GetNearbyNPCs(50.0f);
                foreach (var npc in nearbyNPCs)
                {
                    if (!npc.IsAlive) continue;

                    _logger.LogDebug("Nearby NPC: {NPCName} (Level {Level}) at distance {Distance:F1}",
                        npc.Name, npc.Level, 
                        _gameClient.Character?.DistanceTo(npc) ?? 0);
                }

                // 3. Send periodic messages
                if (_gameClient.CurrentZone != null)
                {
                    var playerCount = _gameClient.CurrentZone.Players.Count;
                    var npcCount = _gameClient.CurrentZone.NPCs.Count;
                    
                    _logger.LogInformation("Zone status: {PlayerCount} players, {NPCCount} NPCs", 
                        playerCount, npcCount);
                }

                // 4. Example: Say hello every 60 seconds
                _gameClient.SendChat($"Bot online! Zone has {_gameClient.CurrentZone?.NPCs.Count ?? 0} NPCs");

                await Task.Delay(60000); // Wait 60 seconds
            }

            _logger.LogInformation("Bot behavior loop ended");
        }

        #region Event Handlers

        private void OnConnectionStateChanged(object? sender, ConnectionState state)
        {
            _logger.LogInformation("Connection state: {State}", state);
        }

        private void OnCharacterLoaded(object? sender, Character character)
        {
            _logger.LogInformation("=== CHARACTER READY ===");
            _logger.LogInformation("Playing as: {Character}", character);
            _logger.LogInformation("Zone: {ZoneName} (ID: {ZoneID})", 
                _gameClient.CurrentZone?.Name, _gameClient.CurrentZone?.ZoneID);
        }

        private void OnNPCSpawned(object? sender, NPC npc)
        {
            _logger.LogDebug("NPC spawned: {NPC}", npc);
        }

        private void OnPlayerSpawned(object? sender, Player player)
        {
            _logger.LogInformation("Player joined zone: {Player}", player);
        }

        private void OnChatMessageReceived(object? sender, ChatMessage message)
        {
            _logger.LogInformation("Chat: {Message}", message);
            
            // Example: Respond to tells
            if (message.IsTell && message.Message.Contains("hello", StringComparison.OrdinalIgnoreCase))
            {
                // Note: This would need additional tell functionality in EQGameClient
                _logger.LogInformation("Would respond to tell from {From}", message.From);
            }
        }

        private void OnLoginFailed(object? sender, string reason)
        {
            _logger.LogError("Login failed: {Reason}", reason);
            _isRunning = false;
        }

        private void OnDisconnected(object? sender, EventArgs e)
        {
            _logger.LogWarning("Disconnected from game");
            _isRunning = false;
        }

        #endregion
    }

    public class EQServerConfig
    {
        public string LoginServer { get; set; } = "";
        public int LoginServerPort { get; set; } = 5999;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string WorldName { get; set; } = "";
        public string CharacterName { get; set; } = "";
    }
}