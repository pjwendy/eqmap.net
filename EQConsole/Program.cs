using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using NLog;
using OpenEQ.Netcode;
using OpenEQ.Netcode.GameClient;
using OpenEQ.Netcode.GameClient.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static System.Console;
using NLogLevel = NLog.LogLevel;

namespace eqconsole
{
    public class PatrolPoint
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }

    public class AccountSettings
    {
        public string Host { get; set; } = "172.29.179.249";
        public int LoginPort { get; set; } = 5999;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string ServerName { get; set; } = "";
        public string CharacterName { get; set; } = "";
        public int PositionUpdateIntervalMs { get; set; } = 100;
        public PatrolPoint[] PatrolPoints { get; set; } = new PatrolPoint[0];
    }

    internal class Program
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static EQGameClient gameClient;
        private static AccountSettings settings;
        private static bool isConnected = false;
        private static bool isMoving = false;
        private static int currentPatrolPoint = 0;
        private static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        static async Task Main(string[] args)
        {
            SetupLogging();
            Logger.Info("Starting EQConsole Bot");

            // Load settings
            settings = LoadSettings();

            // Get connection parameters (use settings as defaults)
            var host = GetUserInputWithDefault("Login Server", settings.Host);
            var loginPort = GetUserInputWithDefault("Login Port", settings.LoginPort.ToString());
            var username = GetUserInputWithDefault("Username", settings.Username);
            var password = GetUserInputWithDefault("Password", settings.Password);
            var serverName = GetUserInputWithDefault("Server Name", settings.ServerName);
            var characterName = GetUserInputWithDefault("Character Name", settings.CharacterName);

            Logger.Info($"Connecting to LoginServer @ {host}:{loginPort}");

            // Create game client with logger
            var gameClientLogger = new NLogLoggerAdapter<EQGameClient>();
            gameClient = new EQGameClient(gameClientLogger);
            
            // Set login server info
            gameClient.LoginServer = host;
            gameClient.LoginServerPort = int.Parse(loginPort);

            // Subscribe to events
            SetupEventHandlers();

            try
            {
                // Connect to the server
                bool connected = await gameClient.LoginAsync(username, password, serverName, characterName);

                if (connected)
                {
                    Logger.Info("Successfully connected to game server");
                    
                    // Keep the application running
                    while (!cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        await Task.Delay(1000, cancellationTokenSource.Token);
                        
                        // Handle patrol movement if connected and not already moving
                        if (isConnected && !isMoving && settings.PatrolPoints.Length > 0)
                        {
                            await StartPatrolMovement();
                        }
                    }
                }
                else
                {
                    Logger.Error("Failed to connect to game server");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Connection error");
            }
            finally
            {
                gameClient?.Dispose();
            }
        }

        private static AccountSettings LoadSettings()
        {
            try
            {
                if (File.Exists("Account.json"))
                {
                    var json = File.ReadAllText("Account.json");
                    return JsonConvert.DeserializeObject<AccountSettings>(json) ?? new AccountSettings();
                }
            }
            catch (Exception ex)
            {
                Logger.Warn(ex, "Could not load Account.json, using defaults");
            }
            
            return new AccountSettings();
        }

        private static void SetupLogging()
        {
            // Create shared logs directory in solution root if it doesn't exist
            string solutionLogsDir = Path.Combine("..", "logs");
            string solutionArchiveDir = Path.Combine("..", "logs", "archive");

            if (!Directory.Exists(solutionLogsDir))
                Directory.CreateDirectory(solutionLogsDir);

            if (!Directory.Exists(solutionArchiveDir))
                Directory.CreateDirectory(solutionArchiveDir);

            // NLog will automatically load NLog.config from the application directory
            // If the config file doesn't exist, log a warning
            if (!File.Exists("NLog.config"))
            {
                Console.WriteLine("Warning: NLog.config file not found. Using default configuration.");

                // Fallback to programmatic configuration if config file is missing
                var config = new NLog.Config.LoggingConfiguration();

                var logfile = new NLog.Targets.FileTarget("logfile")
                {
                    FileName = "../logs/EQConsole-Combined-fallback.log",
                    Layout = "${longdate} ${level:uppercase=true} [${logger:shortName=true}] ${message} ${exception:format=tostring}"
                };

                var logconsole = new NLog.Targets.ConsoleTarget("logconsole")
                {
                    Layout = "${time} [${level:uppercase=true}] ${logger:shortName=true} ${message} ${exception:format=tostring}"
                };

                config.AddRule(NLogLevel.Debug, NLogLevel.Fatal, logfile);
                config.AddRule(NLogLevel.Info, NLogLevel.Fatal, logconsole);

                NLog.LogManager.Configuration = config;
            }

            // Print log file locations to console
            PrintLogFileLocations();
        }

        private static void PrintLogFileLocations()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string solutionRoot = Path.GetFullPath(Path.Combine(baseDir, ".."));
                string mainLogPath = Path.Combine(solutionRoot, "logs", $"EQConsole-{DateTime.Now:yyyy-MM-dd}.log");
                string protocolLogPath = Path.Combine(solutionRoot, "logs", $"EQProtocol-{DateTime.Now:yyyy-MM-dd}.log");
                string networkLogPath = Path.Combine(solutionRoot, "logs", $"Network-{DateTime.Now:yyyy-MM-dd}.log");
                string movementLogPath = Path.Combine(solutionRoot, "logs", $"Movement-{DateTime.Now:yyyy-MM-dd}.log");

                string combinedLogPath = Path.Combine(solutionRoot, "logs", $"EQConsole-Combined-{DateTime.Now:yyyy-MM-dd}.log");

                Console.WriteLine("=== LOG FILE LOCATIONS ===");
                Console.WriteLine($"COMBINED Log (EQConsole + EQProtocol): {combinedLogPath}");
                Console.WriteLine("");
                Console.WriteLine("Optional detailed logs:");
                Console.WriteLine($"  EQConsole Only: {mainLogPath}");
                Console.WriteLine($"  Network Only: {networkLogPath}");
                Console.WriteLine($"  Movement Only: {movementLogPath}");
                Console.WriteLine("===========================");
                Console.WriteLine();

                // Also log to the actual log file for reference
                Logger.Info($"EQConsole log file locations:");
                Logger.Info($"  COMBINED (EQConsole + EQProtocol): {combinedLogPath}");
                Logger.Info($"  Optional - EQConsole Only: {mainLogPath}");
                Logger.Info($"  Optional - Network Only: {networkLogPath}");
                Logger.Info($"  Optional - Movement Only: {movementLogPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error displaying log file locations: {ex.Message}");
            }
        }

        private static void SetupEventHandlers()
        {
            gameClient.ConnectionStateChanged += OnConnectionStateChanged;
            gameClient.Disconnected += OnDisconnected;
            gameClient.LoginFailed += OnLoginFailed;
            gameClient.ChatMessageReceived += OnChatMessageReceived;
            gameClient.PlayerSpawned += OnPlayerSpawned;
            gameClient.NPCSpawned += OnNPCSpawned;
            gameClient.ZoneChanged += OnZoneChanged;
        }

        private static void OnConnectionStateChanged(object sender, ConnectionState state)
        {
            Logger.Info($"Connection state: {state}");

            if (state == ConnectionState.InGame)
            {
                isConnected = true;
                
                // Send hello message
                Task.Run(async () =>
                {
                    // Wait a moment for everything to stabilize
                    await Task.Delay(2000);
                    gameClient.SendChat("Hello from EQConsole LUA Bot", ChatChannel.Say);
                    Logger.Info("Bot is now active and ready for patrol");
                });
            }
            else
            {
                isConnected = false;
                isMoving = false;
            }
        }

        private static void OnDisconnected(object sender, EventArgs e)
        {
            Logger.Info("Game client disconnected");
            isConnected = false;
            isMoving = false;
        }

        private static void OnLoginFailed(object sender, string error)
        {
            Logger.Error($"Login failed: {error}");
        }

        private static void OnChatMessageReceived(object sender, OpenEQ.Netcode.GameClient.Models.ChatMessage message)
        {
            Logger.Info($"[{message.Channel}] {message.From}: {message.Message}");
        }

        private static void OnPlayerSpawned(object sender, Player player)
        {
            Logger.Info($"Player spawned: {player.Name}");
        }

        private static void OnNPCSpawned(object sender, NPC npc)
        {
            // Only log NPC spawns at debug level to reduce console noise
            Logger.Debug($"NPC spawned: {npc.Name}");
        }

        private static void OnZoneChanged(object sender, Zone zone)
        {
            Logger.Info($"Zone changed to: {zone.Name} ({zone.ZoneID})");
            isMoving = false; // Reset movement when changing zones
            currentPatrolPoint = 0; // Reset patrol to first point
        }

        private static async Task StartPatrolMovement()
        {
            if (!isConnected || isMoving || settings.PatrolPoints.Length == 0)
                return;

            isMoving = true;
            
            try
            {
                var targetPoint = settings.PatrolPoints[currentPatrolPoint];
                Logger.Info($"Moving to patrol point {currentPatrolPoint + 1}: ({targetPoint.X:F2}, {targetPoint.Y:F2}, {targetPoint.Z:F2})");

                // Send chat message about movement
                gameClient.SendChat($"Moving to point {currentPatrolPoint + 1}", ChatChannel.Say);

                // Start movement
                bool success = await gameClient.MoveTo(targetPoint.X, targetPoint.Y, targetPoint.Z);
                
                if (success)
                {
                    Logger.Info($"Successfully reached patrol point {currentPatrolPoint + 1}");
                    
                    // Move to next patrol point
                    currentPatrolPoint = (currentPatrolPoint + 1) % settings.PatrolPoints.Length;
                    
                    // Wait a bit before moving to next point
                    await Task.Delay(3000);
                }
                else
                {
                    Logger.Warn($"Failed to reach patrol point {currentPatrolPoint + 1}");
                    
                    // Still advance to next point and try again
                    currentPatrolPoint = (currentPatrolPoint + 1) % settings.PatrolPoints.Length;
                    await Task.Delay(5000);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error during patrol movement");
                await Task.Delay(5000);
            }
            finally
            {
                isMoving = false;
            }
        }

        private static string GetUserInputWithDefault(string prompt, string defaultValue)
        {
            if (!string.IsNullOrEmpty(defaultValue))
            {
                Write($"{prompt} (default: {defaultValue}): ");
            }
            else
            {
                Write($"{prompt}: ");
            }
            
            var input = ReadLine()?.TrimEnd();
            return string.IsNullOrEmpty(input) ? defaultValue : input;
        }
    }
}