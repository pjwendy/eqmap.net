using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NLua;
using OpenEQ.Netcode;
using OpenEQ.Netcode.GameClient;
using OpenEQ.Netcode.GameClient.Models;
using OpenEQ.Netcode.GameClient.Maps;
using System.Drawing;
using NLog;
using MySqlConnector;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Action = System.Action;

namespace eqmap
{
    public partial class Main : Form
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public enum LogSource
        {
            lua = 0,
            engine = 1           
        }        
        private Account account;
        private Log log;
        private Lua lua;
        private Chat chat;
        private Zone zone;
        private EQGameClient gameClient;
        private PlayerProfile player;
        private Map map = new Map();
        private Dictionary<string, string> zones = new Dictionary<string, string>();
        private ConcurrentDictionary<uint, ZoneEntry> spawns = new ConcurrentDictionary<uint, ZoneEntry>();
        private dynamic settings;
        public Main()
        {
            InitializeComponent();
            InitialiseSettings();
            InitialiseZoneList();

            map.pb = pictureBox1;

            lua = new Lua();
            account = new Account();
            log = new Log(this);
            chat = null;  // Will be initialized when game client connects
            account.OnLogon += Account_OnLogon;
            lua["account"] = account;
            lua["log"] = log;
            lua["chat"] = chat;
            lua.RegisterFunction("SetLogonResultHandler", this, GetType().GetMethod("SetLogonResultHandler"));
            lua.RegisterFunction("SetMessageEventHandler", this, GetType().GetMethod("SetMessageEventHandler"));
            lua.RegisterFunction("SetSpawnEventHandler", this, GetType().GetMethod("SetSpawnEventHandler"));

            this.buttonZone.Click += Button1_Click;
            this.FormClosing += Form1_FormClosing;
            this.pictureBox1.AllowDrop = true;
            this.pictureBox1.DragEnter += PictureBox1_DragEnter;
            this.pictureBox1.DragDrop += PictureBox1_DragDrop;
            this.pictureBox1.MouseMove += PictureBox1_MouseMove;
            this.pictureBox1.Paint += PictureBox1_Paint;

            if (!Directory.Exists("logs"))
                Directory.CreateDirectory("logs");
            if (File.Exists(@"logs\log.txt"))
                File.Delete(@"logs\log.txt");
            if (File.Exists(@"logs\handled.txt"))
                File.Delete(@"logs\handled.txt");
            if (File.Exists(@"logs\unhandled.txt"))
                File.Delete(@"logs\unhandled.txt");

            // Create shared logs directory in solution root if it doesn't exist
            string solutionLogsDir = Path.Combine("..", "logs");
            string solutionArchiveDir = Path.Combine("..", "logs", "archive");

            if (!Directory.Exists(solutionLogsDir))
                Directory.CreateDirectory(solutionLogsDir);

            if (!Directory.Exists(solutionArchiveDir))
                Directory.CreateDirectory(solutionArchiveDir);

            // NLog will automatically load NLog.config from the application directory
            // If the config file doesn't exist, use fallback configuration
            if (!File.Exists("NLog.config"))
            {
                Logger.Warn("NLog.config file not found. Using fallback configuration.");

                // Fallback to programmatic configuration if config file is missing
                var config = new NLog.Config.LoggingConfiguration();
                var logfile = new NLog.Targets.FileTarget("logfile") { FileName = @"..\logs\EQMap-Combined-fallback.log" };
                config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, logfile);
                NLog.LogManager.Configuration = config;
            }

            // Update log status display
            UpdateLogStatusDisplay();

            // ServerLogs functionality removed - server logs accessed through alternative method
        }

        private void UpdateLogStatusDisplay()
        {
            try
            {
                string baseDir = Application.StartupPath;
                string solutionRoot = Path.GetFullPath(Path.Combine(baseDir, ".."));
                string combinedLogPath = Path.Combine(solutionRoot, "logs", $"EQMap-Combined-{DateTime.Now:yyyy-MM-dd}.log");
                string mainLogPath = Path.Combine(solutionRoot, "logs", $"EQMap-Only-{DateTime.Now:yyyy-MM-dd}.log");
                string luaLogPath = Path.Combine(solutionRoot, "logs", $"Lua-Only-{DateTime.Now:yyyy-MM-dd}.log");

                string statusText = $"Logs: COMBINED={Path.GetFileName(combinedLogPath)} (EQMap+Lua+EQProtocol) - Click to open logs folder";

                // Update the status label safely from UI thread
                if (labelLogStatus.InvokeRequired)
                {
                    labelLogStatus.Invoke(new Action(() => labelLogStatus.Text = statusText));
                }
                else
                {
                    labelLogStatus.Text = statusText;
                }

                // Also log the paths to the main log for reference
                Logger.Info($"EQMap log file locations:");
                Logger.Info($"  COMBINED (EQMap + Lua + EQProtocol): {combinedLogPath}");
                Logger.Info($"  Optional - EQMap Only: {mainLogPath}");
                Logger.Info($"  Optional - Lua Only: {luaLogPath}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to update log status display");
            }
        }

        private void labelLogStatus_Click(object sender, EventArgs e)
        {
            try
            {
                string baseDir = Application.StartupPath;
                string solutionRoot = Path.GetFullPath(Path.Combine(baseDir, ".."));
                string logsDir = Path.Combine(solutionRoot, "logs");

                // Create logs directory if it doesn't exist
                if (!Directory.Exists(logsDir))
                {
                    Directory.CreateDirectory(logsDir);
                }

                // Open the logs folder in Windows Explorer
                System.Diagnostics.Process.Start("explorer.exe", logsDir);
                Logger.Info("Opened logs folder in Windows Explorer");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to open logs folder");
                MessageBox.Show($"Failed to open logs folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Draw Map
        private void loadMap()
        {
            try
            {
                string zoneIdStr = Convert.ToString(player.ZoneID);
                if (!zones.ContainsKey(zoneIdStr))
                {
                    Info($"Zone ID {zoneIdStr} not found in zones dictionary");
                    return;
                }
                
                map.zone = zones[zoneIdStr];
                Info($"Loading map for zone: {map.zone} (ID: {zoneIdStr})");
                
                map.zoom = 1f;
                map.lines = new List<Line>();
                
                // Use the new MapReader from EQProtocol
                var mapData = MapReader.ReadMapFile(map.zone);
                
                if (mapData == null)
                {
                    Info($"No map file found for zone: {map.zone}");
                    return;
                }

                Info($"Loading map for zone: {map.zone}");
                
                // Convert MapLines to the local Line format
                foreach (var mapLine in mapData.Lines)
                {
                    Line l = new Line(mapLine);
                    map.lines.Add(l);
                }
                
                // Use the bounds calculated by MapReader
                map.maxX = mapData.MaxX;
                map.maxY = mapData.MaxY;
                map.minX = mapData.MinX;
                map.minY = mapData.MinY;
                
                Info($"Map loaded with {mapData.Lines.Count} lines. Bounds: X({map.minX},{map.maxX}) Y({map.minY},{map.maxY})");
                MapReady = true;            
            }
            catch (Exception ex)
            {
                Error($"Error loading map: {ex.Message}");
                MapReady = false;
            }
        }
        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.LightGray); // Changed to light gray so we can see if it's painting
            
            // Always draw status info
            int y = 10;
            g.DrawString($"MapReady: {mapReady}", SystemFonts.DefaultFont, Brushes.Black, 10, y);
            y += 20;
            
            if (map != null)
            {
                g.DrawString($"Zone: {map.zone ?? "null"}", SystemFonts.DefaultFont, Brushes.Black, 10, y);
                y += 20;
                g.DrawString($"Lines: {map.lines?.Count ?? 0}", SystemFonts.DefaultFont, Brushes.Black, 10, y);
                y += 20;
            }
            
            g.DrawString($"Spawns: {spawns?.Count ?? 0}", SystemFonts.DefaultFont, Brushes.Black, 10, y);
            y += 20;
            
            if (!mapReady)
            {
                g.DrawString("Map not ready", SystemFonts.DefaultFont, Brushes.Red, 10, y);
                return;
            }
            
            if (map.lines == null || map.lines.Count == 0)
            {
                g.DrawString("No map lines loaded", SystemFonts.DefaultFont, Brushes.Red, 10, y);
                return;
            }
            
            // Draw the map
            foreach (Line line in map.lines)
            {
                g.DrawLine(
                    line.pen,
                    new Point(map.adjustedX(line.fromX), map.adjustedY(line.fromY)),
                    new Point(map.adjustedX(line.toX), map.adjustedY(line.toY))
                );                        
            }
            
            using (Font font = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Pixel))
            {                    
                TextRenderer.DrawText(g, "(0,0)", font, new Point(map.adjustedX(0), map.adjustedY(0)), Color.Black);
            
                foreach(ZoneEntry spawn in spawns.Values)
                {
                    TextRenderer.DrawText(
                        g, $"{spawn.Name}", 
                        font, 
                        new Point(map.adjustedX(-spawn.Position.X / 8) + 6, map.adjustedY(-spawn.Position.Y / 8)), 
                        spawn.CharType == CharType.PC ? Color.Blue : Color.Red
                    );

                    g.DrawEllipse(
                        new Pen(spawn.CharType == CharType.PC ? Color.Blue : Color.Red,5),
                        map.adjustedX(-spawn.Position.X / 8),
                        map.adjustedY(-spawn.Position.Y / 8),
                        5,
                        5
                    );                        
                }
            }
            pictureBox1.Height = Convert.ToInt32((map.maxY + (map.minY > 0 ? map.minY : -map.minY) + 1) * map.zoom);
            pictureBox1.Width = Convert.ToInt32((map.maxX + (map.minX > 0 ? map.minX : -map.minX) + 1) * map.zoom);
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (map.zone != String.Empty)
            {
                labelMapCoordinates.Visible = true;
                labelMapCoordinates.Text = $"{map.zone} x:({map.minX},{map.maxX}) y:({map.minY},{map.maxY}) pos:({map.adjustedX(-e.X)},{map.adjustedY(-e.Y)})";
            }
        }
        #endregion

        #region Run Lua Script 
        private void PictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 1)
                Info("Only drop 1 file");
            else
            {
                try
                {
                    Info($"Loading {files[0]}");
                    lua.DoFile(files[0]);
                    lua.DoString("return Main()");
                }
                catch (Exception ex)
                {
                    Error(ex.ToString());
                }
            }
        }
        private void PictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }
        #endregion

        #region Account Logon
        private async void Account_OnLogon()
        {
            Info($"Logon: {account.User} {account.Server} {account.Character}");

            // Create a simple logger adapter that wraps NLog
            var gameClientLogger = new NLogLoggerAdapter();
            
            // Create and configure the game client with proper logger
            gameClient = new EQGameClient(gameClientLogger);
            gameClient.PacketRecordingMode = OpenEQ.Netcode.GameClient.Events.RecordingMode.Full;

            // Subscribe to events
            gameClient.ConnectionStateChanged += OnConnectionStateChanged;
            gameClient.Disconnected += OnGameClientDisconnected;
            gameClient.ChatMessageReceived += OnChatMessageReceived;
            gameClient.PlayerSpawned += OnPlayerSpawned;
            gameClient.NPCSpawned += OnNPCSpawned;
            gameClient.PlayerDespawned += OnPlayerDespawned;
            gameClient.NPCDespawned += OnNPCDespawned;
            gameClient.ZoneChanged += OnZoneChanged;
            gameClient.LoginFailed += OnLoginFailed;
            gameClient.ClientUpdated += OnClientUpdated;
            gameClient.MobUpdated += OnMobUpdated;
            gameClient.NPCMoveUpdated += OnNPCMoveUpdated;

            try
            {
                // Set the login server info BEFORE connecting
                gameClient.LoginServer = account.LoginServer;
                gameClient.LoginServerPort = account.LoginServerPort;
                
                // Connect to the server using LoginAsync
                bool connected = await gameClient.LoginAsync(
                    account.User,
                    account.Password,
                    account.Server,
                    account.Character
                );

                if (connected)
                {
                    Info("Successfully connected to game server");
                    CallLogonResult(true, "Connected");
                    
                    // Update player info
                    UpdateCharacterFromGameClient();
                }
                else
                {
                    Error("Failed to connect to game server");
                    CallLogonResult(false, "Connection failed");
                }
            }
            catch (Exception ex)
            {
                Error($"Connection error: {ex.Message}");
                CallLogonResult(false, ex.Message);
            }
        }
        #endregion

        #region EQGameClient Event Handlers
        private void OnConnectionStateChanged(object sender, ConnectionState state)
        {
            Info($"Connection state changed to: {state}");
            
            if (state == ConnectionState.InGame)
            {
                spawns.Clear();
                
                // Update chat and zone with game client
                chat = new Chat(account, gameClient);
                zone = new Zone(gameClient);
                
                // Force initial refresh to show status
                BeginInvoke((Action)delegate { pictureBox1.Refresh(); });
                lua["chat"] = chat;
                lua["zone"] = zone;
                
                // Enable UI elements
                BeginInvoke((Action)delegate { buttonZone.Enabled = true; });
            }
        }

        private void OnGameClientDisconnected(object sender, EventArgs e)
        {
            Info("Game client disconnected");
            BeginInvoke((Action)delegate { buttonZone.Enabled = false; });
        }

        private void OnLoginFailed(object sender, string error)
        {
            Error($"Login failed: {error}");
            CallLogonResult(false, error);
        }

        private void OnChatMessageReceived(object sender, OpenEQ.Netcode.GameClient.Models.ChatMessage message)
        {
            Info($"[{message.Channel}] {message.From}: {message.Message}");
            // Convert to old ChannelMessage format for Lua compatibility
            var oldMessage = new ChannelMessage
            {
                ChanNum = (uint)message.Channel,
                From = message.From,
                Message = message.Message
            };
            CallMessageEvent(oldMessage);
        }

        private void OnPlayerSpawned(object sender, Player player)
        {
            Info($"Player spawned: {player.Name} at X:{player.X} Y:{player.Y}");
            
            // Convert Player to Spawn for compatibility
            var spawn = new ZoneEntry
            {
                SpawnID = player.SpawnID,
                Name = player.Name,
                CharType = CharType.PC,
                Position = new SpawnPosition
                {
                    X = (int)player.X,
                    Y = (int)player.Y,
                    Z = (int)player.Z,
                    Heading = (ushort)player.Heading
                }
            };
            
            spawns.TryAdd(player.SpawnID, spawn);
            Info($"Added player spawn to map. Total spawns: {spawns.Count}");
            BeginInvoke((Action)delegate { pictureBox1.Refresh(); });
            CallSpawnEvent(spawn);
        }

        private void OnNPCSpawned(object sender, NPC npc)
        {
            Info($"NPC spawned: {npc.Name} at X:{npc.X} Y:{npc.Y}");
            
            // Convert NPC to Spawn for compatibility
            var spawn = new ZoneEntry   
            {
                SpawnID = npc.SpawnID,
                Name = npc.Name,
                CharType = CharType.NPC,
                Position = new SpawnPosition
                {
                    X = (int)npc.X,
                    Y = (int)npc.Y,
                    Z = (int)npc.Z,
                    Heading = (ushort)npc.Heading
                }
            };
            
            spawns.TryAdd(npc.SpawnID, spawn);
            Info($"Added NPC spawn to map. Total spawns: {spawns.Count}");
            BeginInvoke((Action)delegate { pictureBox1.Refresh(); });
            CallSpawnEvent(spawn);
        }

        private void OnPlayerDespawned(object sender, uint spawnId)
        {
            Info($"Player despawned: {spawnId}");
            ZoneEntry sp;
            spawns.TryRemove(spawnId, out sp);
            BeginInvoke((Action)delegate { pictureBox1.Refresh(); });
        }

        private void OnNPCDespawned(object sender, uint spawnId)
        {
            Info($"NPC despawned: {spawnId}");
            ZoneEntry sp;
            spawns.TryRemove(spawnId, out sp);
            BeginInvoke((Action)delegate { pictureBox1.Refresh(); });
        }

        private void OnClientUpdated(object sender, ClientUpdateFromServer update)
        {
            // Update the position of the spawn if it exists
            if (spawns.TryGetValue(update.ID, out ZoneEntry spawn))
            {
                // Since Spawn is a struct, we need to update the position and put it back
                spawn.Position = new SpawnPosition
                {
                    X = (int)update.Position.X,
                    Y = (int)update.Position.Y,
                    Z = (int)update.Position.Z,
                    Heading = (ushort)update.Position.Heading
                };
                
                // Put the updated spawn back in the dictionary
                spawns[update.ID] = spawn;
                
                Info($"Position updated for spawn {update.ID}: ({spawn.Position.X:F1}, {spawn.Position.Y:F1})");
                
                // Refresh the map to show the new position
                BeginInvoke((Action)delegate { pictureBox1.Refresh(); });
            }
            else
            {
                // Log if we're getting updates for unknown spawns
                Info($"Position update for unknown client ID {update.ID}");
            }
        }

        private void OnMobUpdated(object sender, MobUpdate update)
        {
            // Update the position of the spawn if it exists
            if (spawns.TryGetValue(update.ID, out ZoneEntry spawn))
            {
                // Since Spawn is a struct, we need to update the position and put it back
                spawn.Position = new SpawnPosition
                {
                    X = (int)update.Position.X,
                    Y = (int)update.Position.Y,
                    Z = (int)update.Position.Z,
                    Heading = (ushort)update.Position.Heading
                };

                // Put the updated spawn back in the dictionary
                spawns[update.ID] = spawn;

                Info($"Position updated for spawn {update.ID}: ({spawn.Position.X:F1}, {spawn.Position.Y:F1})");

                // Refresh the map to show the new position
                BeginInvoke((Action)delegate { pictureBox1.Refresh(); });
            }
            else
            {
                // Log if we're getting updates for unknown spawns
                Info($"Position update for unknown mob ID {update.ID}");
            }
        }

        private void OnNPCMoveUpdated(object sender, NPCMoveUpdate update)
        {
            // Update the position of the spawn if it exists
            if (spawns.TryGetValue(update.ID, out ZoneEntry spawn))
            {
                // Since Spawn is a struct, we need to update the position and put it back
                spawn.Position = new SpawnPosition
                {
                    X = (int)update.Position.X,
                    Y = (int)update.Position.Y,
                    Z = (int)update.Position.Z,
                    Heading = (ushort)update.Position.Heading
                };

                // Put the updated spawn back in the dictionary
                spawns[update.ID] = spawn;

                Info($"Position updated for spawn {update.ID}: ({spawn.Position.X:F1}, {spawn.Position.Y:F1})");

                // Refresh the map to show the new position
                BeginInvoke((Action)delegate { pictureBox1.Refresh(); });
            }
            else
            {
                // Log if we're getting updates for unknown spawns
                Info($"Position update for unknown NPC ID {update.ID}");
            }
        }

        private void OnZoneChanged(object sender, OpenEQ.Netcode.GameClient.Models.Zone zone)
        {
            Info($"Zone changed to: {zone.Name} ({zone.ZoneID})");
            spawns.Clear();
            MapReady = false;
            
            // Update player from game client
            UpdateCharacterFromGameClient();
            loadMap();
            
            BeginInvoke((Action)delegate { pictureBox1.Refresh(); });
        }

        private void UpdateCharacterFromGameClient()
        {
            if (gameClient?.CurrentZone != null)
            {
                // For now, we'll create a basic player profile from zone info
                // The actual player data would need to come from tracking our own spawn
                player = new PlayerProfile
                {
                    Name = account.Character,
                    Level = 1, // Default, should be tracked from spawn info
                    Class = 1, // Default
                    Race = 1, // Default
                    ZoneID = (ushort)gameClient.CurrentZone.ZoneID,
                    X = 0,
                    Y = 0,
                    Z = 0
                };
                UpdateCharacter(player);
            }
        }
        #endregion

        private void InitialiseSettings()
        {
            string file = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\.eqmap.net.settings.json";
            if (!File.Exists(file)) {
                File.Copy("settings.json", file);
                throw new Exception("You are missing your settings.json file, one has been created for you from settings.json" +
                    @"\n\nTake a look at the settings and update and\or add missing values so asd to reflect your instance of eqemu" +
                    $@"\n\nYou settings can be found at {file}");
            } 
            else
            {
                settings = JObject.Parse(File.ReadAllText(file));
            }
        }

        #region Various Form Initialisation Methods
        private void InitialiseZoneList()
        {
            string server = settings.database.ip;
            string database = settings.database.name;
            string port = settings.database.port;
            string uid = settings.database.uid;
            string password = settings.database.password;
            string connectionString = $"SERVER={server};Port={port};DATABASE={database};UID={uid};PASSWORD={password};";

            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "SELECT * FROM zone WHERE version=0";

            //Create a list to store the result
            List<string> list = new List<string>();

            //Create Command
            MySqlCommand cmd = new MySqlCommand(query, connection);
            //Create a data reader and Execute the command
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //Read the data and store them in the list
            while (dataReader.Read())
            {
                list.Add(dataReader["short_name"] + "");
                if (!zones.ContainsKey(Convert.ToString(dataReader["zoneidnumber"])))
                    zones.Add(Convert.ToString(dataReader["zoneidnumber"]), dataReader["short_name"] + "");
            }

            list.Sort();

            //close Data Reader
            dataReader.Close();

            //close Connection
            connection.Close();

            // Get available zones from EQProtocol MapReader
            var availableZones = MapReader.GetAvailableZones();
            
            //return list to be displayed
            foreach (string zone in list)
                if (availableZones.Contains(zone.Trim()))
                    listboxZone.Items.Add(zone);
        }
        private void Form1_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            try
            {
                if (gameClient != null)
                {
                    gameClient.Disconnect();
                    gameClient.Dispose();
                }
            }
            catch (Exception)
            {
            }                 
        }
        #endregion

        #region Zone
        private void Button1_Click(object sender, EventArgs e)
        {            
            var zone = listboxZone.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(zone) && gameClient != null)
            {
                // Send zone command through chat
                gameClient.SendChat($"#zone {zone}", ChatChannel.Say);
            }
        }
        #endregion

        #region Logging
        delegate void SetLogCallback(string text, LogSource source);
        public void Info(string text)
        {
            Info(text, LogSource.engine);
        }
        public void Info(string text, LogSource source)
        {
            Logger.Info($"{source}:{text}");                 
        }
        public void Error(string text)
        {
            Error(text, LogSource.engine);
        }
        public void Error(string text, LogSource source)
        {
            Logger.Error($"{source}:{text}");            
        }
        #endregion

        #region Map Ready
        private bool mapReady = false;

        public bool MapReady
        {
            get => mapReady;
            set => mapReady = value; 
        }
        #endregion

        delegate void SetPlayerProfileCallback(PlayerProfile player);
        private void UpdateCharacter(PlayerProfile player)
        {
            this.player = player;
            if (this.textBoxCharacterInfo.InvokeRequired)
            {
                SetPlayerProfileCallback d = new SetPlayerProfileCallback(UpdateCharacter);
                this.Invoke(d, new object[] { player });
            }
            else
            {
                this.textBoxCharacterInfo.AppendText(player.ToString().Replace("\n", Environment.NewLine));
            }
            Application.DoEvents();
        }

        public delegate void LogonResultEventHandler(bool success, string reason);

        private LogonResultEventHandler _LogonResultEventHandler;
        
        public void SetLogonResultHandler(LogonResultEventHandler eventHandler)
        {
            _LogonResultEventHandler = eventHandler;
        }

        public void CallLogonResult(bool success, string reason)
        {
            _LogonResultEventHandler?.Invoke(success, reason);
        }

        public delegate void SpawnEventHandler(object mob);

        private SpawnEventHandler _SpawnEventHandler;

        public void SetSpawnEventHandler(SpawnEventHandler eventHandler)
        {
            _SpawnEventHandler = eventHandler;
        }

        public void CallSpawnEvent(object mob)
        {
            _SpawnEventHandler?.Invoke(mob);
        }

        public delegate void MessageEventHandler(object mob);

        private MessageEventHandler _MessageEventHandler;

        public void SetMessageEventHandler(MessageEventHandler eventHandler)
        {
            _MessageEventHandler = eventHandler;
        }

        public void CallMessageEvent(ChannelMessage message)
        {
            _MessageEventHandler?.Invoke(message);
        }

        #region Internal Classes

        #region Map Class
        class Line
        {
            public int fromX;
            public int fromY;
            public int fromZ;
            public int toX;
            public int toY;
            public int toZ;
            public Pen pen; 
            public Line(string line)
            {
                string[] l = line.Substring(1).Split(',');
                this.fromX = Convert.ToInt32(l[0].Split('.')[0].Trim());
                this.fromY = Convert.ToInt32(l[1].Split('.')[0].Trim());
                this.fromZ = Convert.ToInt32(l[2].Split('.')[0].Trim());
                this.toX = Convert.ToInt32(l[3].Split('.')[0].Trim());
                this.toY = Convert.ToInt32(l[4].Split('.')[0].Trim());
                this.toZ = Convert.ToInt32(l[5].Split('.')[0].Trim());
                this.pen = new Pen(Color.FromArgb(255, Convert.ToInt32(l[6].Trim()), Convert.ToInt32(l[7].Trim()), Convert.ToInt32(l[8].Trim())));
            }
            
            public Line(MapLine mapLine)
            {
                this.fromX = mapLine.FromX;
                this.fromY = mapLine.FromY;
                this.fromZ = mapLine.FromZ;
                this.toX = mapLine.ToX;
                this.toY = mapLine.ToY;
                this.toZ = mapLine.ToZ;
                this.pen = new Pen(Color.FromArgb(255, mapLine.Red, mapLine.Green, mapLine.Blue));
            }
        }

        class Chat
        {
            private Account account;
            private EQGameClient gameClient;
            
            public Chat(Account account, EQGameClient gameClient)
            {
                this.account = account;
                this.gameClient = gameClient;
            }
            
            public void Say(string message)
            {
                if (gameClient != null && gameClient.State == ConnectionState.InGame)
                {
                    gameClient.SendChat(message, ChatChannel.Say);
                    Logger.Info($"Said: {message}");
                }
                else
                {
                    Logger.Error("Cannot send chat - not connected to game");
                }
            }
            
            public void Group(string message)
            {
                if (gameClient != null && gameClient.State == ConnectionState.InGame)
                {
                    gameClient.SendChat(message, ChatChannel.Group);
                    Logger.Info($"Group: {message}");
                }
                else
                {
                    Logger.Error("Cannot send group chat - not connected to game");
                }
            }
            
            public void Guild(string message)
            {
                if (gameClient != null && gameClient.State == ConnectionState.InGame)
                {
                    gameClient.SendChat(message, ChatChannel.Guild);
                    Logger.Info($"Guild: {message}");
                }
                else
                {
                    Logger.Error("Cannot send guild chat - not connected to game");
                }
            }
            
            public void Tell(string target, string message)
            {
                if (gameClient != null && gameClient.State == ConnectionState.InGame)
                {
                    // For tells, we might need to implement a different method or use a different channel
                    gameClient.SendChat($"{target}, {message}", ChatChannel.Tell);
                    Logger.Info($"Tell to {target}: {message}");
                }
                else
                {
                    Logger.Error("Cannot send tell - not connected to game");
                }
            }
        }

        class Map
        {
            public PictureBox pb;
            public string zone = string.Empty;
            public int maxX = 0;
            public int maxY = 0;
            public int minX = 0;
            public int minY = 0;
            public float zoom = 1f;
            public List<Line> lines = new List<Line>();
            public int adjustedX(int x) { return Convert.ToInt32(x + (minX > 0 ? minX : -minX) * zoom); }
            public int adjustedY(int y) { return Convert.ToInt32(y + (minY > 0 ? minY : -minY) * zoom); }            
        }
        #endregion

        #endregion
    }
}
