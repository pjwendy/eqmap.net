using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NLua;
using OpenEQ.Netcode;
using OpenEQ.Netcode.GameClient;
using OpenEQ.Netcode.GameClient.Models;
using System.Drawing;
using NLog;
using MySqlConnector;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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
        private ConcurrentDictionary<uint, Spawn> spawns = new ConcurrentDictionary<uint, Spawn>();
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
            account.OnLogon += Account_OnLogon;
            lua["account"] = account;
            lua["log"] = log;            
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

            // Create config for NLog so we can log to file as well as to file
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = @"logs\log.txt" };

            // Rules for mapping loggers to targets            
            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;

            ServerLogs serverLogs = new ServerLogs();
            serverLogs.Show();
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
                map.maxX = 0;
                map.maxY = 0;
                map.minX = 0;
                map.minY = 0;
                map.lines = new List<Line>();
                
                if (!Directory.Exists("maps"))
                {
                    Info("Maps directory not found");
                    return;
                }
                
                FileInfo[] maps = new DirectoryInfo("maps").GetFiles($"{map.zone}.txt", SearchOption.AllDirectories);
                
                if (maps.Length == 0)
                {
                    Info($"No map file found for zone: {map.zone}.txt");
                    return;
                }

                Info($"Loading map file: {maps[0].FullName}");
                int lineCount = 0;
                
                foreach (string line in File.ReadAllLines(maps[0].FullName))
                {
                    if (line.StartsWith("L"))
                    {                   
                        Line l = new Line(line);
                        map.lines.Add(l);
                        lineCount++;
                        map.maxX = l.fromX > l.toX ? (map.maxX < l.fromX ? l.fromX : map.maxX) : (map.maxX < l.toX ? l.toX : map.maxX);
                        map.maxY = l.fromY > l.toY ? (map.maxY < l.fromY ? l.fromY : map.maxY) : (map.maxY < l.toY ? l.toY : map.maxY);
                        map.minX = l.fromX < l.toX ? (map.minX > l.fromX ? l.fromX : map.minX) : (map.minX > l.toX ? l.toX : map.minX);
                        map.minY = l.fromY < l.toY ? (map.minY > l.fromY ? l.fromY : map.minY) : (map.minY > l.toY ? l.toY : map.minY);
                    }
                }
                
                Info($"Map loaded with {lineCount} lines. Bounds: X({map.minX},{map.maxX}) Y({map.minY},{map.maxY})");
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
            
                foreach(Spawn spawn in spawns.Values)
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
            gameClient.PositionUpdated += OnPositionUpdated;

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

        private void OnChatMessageReceived(object sender, ChatMessage message)
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
            var spawn = new Spawn
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
            var spawn = new Spawn
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
            Spawn sp;
            spawns.TryRemove(spawnId, out sp);
            BeginInvoke((Action)delegate { pictureBox1.Refresh(); });
        }

        private void OnNPCDespawned(object sender, uint spawnId)
        {
            Info($"NPC despawned: {spawnId}");
            Spawn sp;
            spawns.TryRemove(spawnId, out sp);
            BeginInvoke((Action)delegate { pictureBox1.Refresh(); });
        }

        private void OnPositionUpdated(object sender, PlayerPositionUpdate update)
        {
            // Update the position of the spawn if it exists
            if (spawns.TryGetValue(update.ID, out Spawn spawn))
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
                Info($"Position update for unknown spawn ID {update.ID}");
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

            List<string> fis = new List<string>();
            foreach (FileInfo fi in new DirectoryInfo(@"maps").GetFiles())
                fis.Add(fi.Name.Replace(".txt", ""));

            //return list to be displayed
            foreach (string zone in list)
                if (fis.Contains(zone.Trim()))
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
