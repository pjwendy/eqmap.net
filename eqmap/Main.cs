using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using NLua;
using OpenEQ.Netcode;
using Newtonsoft.Json;
using System.Drawing;
using NLog;

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
        private LoginStream loginStream;
        private WorldStream worldStream;
        private ZoneStream zoneStream;
        private PlayerProfile player;
        private Map map = new Map();
        private Dictionary<string, string> zones = new Dictionary<string, string>();
        private ConcurrentDictionary<uint, Spawn> spawns = new ConcurrentDictionary<uint, Spawn>();
        public Main()
        {
            InitializeComponent();
            InitialiseZoneList();

            map.pb = pictureBox1;

            lua = new Lua();
            log = new Log(this);
            account = new Account();
            account.OnLogon += Account_OnLogon;
            lua["account"] = account;
            lua["log"] = log;
            lua.RegisterFunction("SetLogonResultHandler", this, GetType().GetMethod("SetLogonResultHandler"));
           
            this.button1.Click += Button1_Click;
            this.FormClosing += Form1_FormClosing;
            this.textBox1.DragEnter += TextBox1_DragEnter;
            this.textBox1.DragDrop += TextBox1_DragDrop;
            this.pictureBox1.MouseMove += PictureBox1_MouseMove;
            this.pictureBox1.Paint += PictureBox1_Paint;

            // Create config for NLog so we can log to file as well as to file
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "log.txt" };

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;
        }

        #region Draw Map
        private void loadMap()
        {
            map.zone = zones[Convert.ToString(player.ZoneID)];
            map.zoom = 1f;
            map.maxX = 0;
            map.maxY = 0;
            map.minX = 0;
            map.minY = 0;
            map.lines = new List<Line>();
            
            FileInfo[] maps = new DirectoryInfo("maps").GetFiles($"{map.zone}.txt", SearchOption.AllDirectories);

            foreach (string line in File.ReadAllLines(maps[0].FullName))
            {
                if (line.StartsWith("L"))
                {                   
                    Line l = new Line(line);
                    map.lines.Add(l);
                    map.maxX = l.fromX > l.toX ? (map.maxX < l.fromX ? l.fromX : map.maxX) : (map.maxX < l.toX ? l.toX : map.maxX);
                    map.maxY = l.fromY > l.toY ? (map.maxY < l.fromY ? l.fromY : map.maxY) : (map.maxY < l.toY ? l.toY : map.maxY);
                    map.minX = l.fromX < l.toX ? (map.minX > l.fromX ? l.fromX : map.minX) : (map.minX > l.toX ? l.toX : map.minX);
                    map.minY = l.fromY < l.toY ? (map.minY > l.fromY ? l.fromY : map.minY) : (map.minY > l.toY ? l.toY : map.minY);
                }
            }           
            MapReady = true;            
        }
        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (mapReady)
            {
                Graphics g = e.Graphics;                
                g.Clear(Color.White);
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
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (map.zone != String.Empty)
            {
                label1.Visible = true;
                button2.Visible = true;
                button3.Visible = true;
                label1.Text = $"{map.zone} x:({map.minX},{map.maxX}) y:({map.minY},{map.maxY}) pos:({map.adjustedX(-e.X)},{map.adjustedY(-e.Y)})";
            }
        }
        #endregion

        #region Run Lua Script 
        private void TextBox1_DragDrop(object sender, DragEventArgs e)
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
        private void TextBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }
        #endregion

        #region Account Logon
        private void Account_OnLogon()
        {
            Info($"Logon: {account.User} {account.Server} {account.Character}");

            loginStream = new LoginStream(account.LoginServer, account.LoginServerPort);
            loginStream.LoginSuccess += (_, success) =>
            {
                if (success)
                {
                    Info($"Login succeeded (accountID={loginStream.AccountID}). Requesting server list");
                    loginStream.RequestServerList();
                }
                else
                {
                    Error("Login failed");
                    CallLogonResult(false, "Login failed");
                    loginStream.Disconnect();
                }
            };

            loginStream.ServerList += (_, servers) =>
            {
                foreach (ServerListElement serv in servers)
                {
                    string shortName = serv.Longname.Substring(serv.Longname.LastIndexOf("] ") + 1).Trim();
                    if (shortName == account.Server)
                    {
                        loginStream.Play(serv);
                        return;
                    }

                }
                Error("Server not found");
                CallLogonResult(false, "Server not found");
                loginStream.Disconnect();
            };

            loginStream.PlaySuccess += (_, server) =>
            {
                if (server == null)
                {
                    Error("Failed to connect to server");
                    CallLogonResult(false, "Failed to connect to server");
                    loginStream.Disconnect();
                    return;
                }
                ConnectWorld(loginStream, server.Value);
            };

            loginStream.Login(account.User, account.Password);
        }
        #endregion

        #region Connect To World
        void ConnectWorld(LoginStream ls, ServerListElement server)
        {
            Info($"Selected {server}.  Connecting");
            worldStream = new WorldStream(server.WorldIP, 9000, ls.AccountID, ls.SessionKey);

            string charName = null;
            worldStream.CharacterList += (_, chars) =>
            {
                foreach (CharacterSelectEntry @char in chars)
                {
                    if (@char.Name == account.Character)
                    {
                        worldStream.SendEnterWorld(new EnterWorld(charName = @char.Name, false, false));
                        return;
                    }
                }
                Error("Character not found");
                CallLogonResult(false, "Character not found");
                ls.Disconnect();
            };

            worldStream.ZoneServer += (_, zs) =>
            {
                Info($"Got zone server at {zs.Host}:{zs.Port}.  Connecting");
                ConnectZone(charName, zs.Host, zs.Port);
            };

            worldStream.MOTD += (_, motd) =>
            {
                Info($"Message of the day : {motd.Replace("\0", "")}");
            };

            worldStream.ChatServerList += (_, chats) =>
            {
                Info($"ChatServerList : {chats.Replace("\0", "")}");
            };
        }
        #endregion

        #region Connect Zone
        void ConnectZone(string charName, string host, ushort port)
        {
            Info("Connected");
            spawns.Clear();
            CallLogonResult(true, "Connected");
            zoneStream = new ZoneStream(host, port, charName);
            zoneStream.Spawned += (_, mob) =>
            {
                Info($"Spawn {mob.Name} X:{mob.Position.X} Y:{mob.Position.Y}");               
                
                if (mob.CharType == CharType.NPC || mob.CharType == CharType.PC)
                {
                    spawns.TryAdd(mob.SpawnID, mob);
                    pictureBox1.BeginInvoke((Action)delegate { pictureBox1.Refresh(); });
                }
            };
            zoneStream.PositionUpdated += (_, update) =>
            {   
                if (!spawns.ContainsKey(update.ID))
                {
                    Info($"Position updated : {update.ID} not found"); 
                    return;
                }
                Spawn tmpSpawn = spawns[update.ID];
                tmpSpawn.Position.X = update.Position.X;
                tmpSpawn.Position.Y = update.Position.Y;
                tmpSpawn.Position.Z = update.Position.Z;
                tmpSpawn.Position.Heading = update.Position.Heading;
                tmpSpawn.Position.DeltaX = update.Position.DeltaX;
                tmpSpawn.Position.DeltaY = update.Position.DeltaY;
                tmpSpawn.Position.DeltaZ = update.Position.DeltaZ;
                tmpSpawn.Position.DeltaHeading = update.Position.DeltaHeading;
                tmpSpawn.Position.Animation = update.Position.Animation;
                spawns[update.ID] = tmpSpawn;
                pictureBox1.BeginInvoke((Action)delegate { pictureBox1.Refresh(); });
            };
            zoneStream.Death += (_, death) =>
            {
                if (!spawns.ContainsKey(death.SpawnId))
                {
                    Info($"Death : {death.SpawnId} not found");
                    return;
                }
                Info($"Death {spawns[death.SpawnId].Name}");
                Spawn sp;
                spawns.TryRemove(death.SpawnId, out sp);
                pictureBox1.BeginInvoke((Action)delegate { pictureBox1.Refresh(); });
            };
            zoneStream.PlayerProfile += (_, player) =>
            {
                UpdateCharacter(player);               
                MapReady = false;
                loadMap();
                button1.BeginInvoke((Action)delegate { button1.Enabled = true; });                
                pictureBox1.BeginInvoke((Action)delegate { pictureBox1.Refresh(); });                
            };            
        }
        #endregion

        #region Various Form Initialisation Methods
        private void InitialiseZoneList()
        {
            string server = "192.168.38.50";
            string database = "peq";
            string uid = "peq";
            string password = "!PEQTea4TwoBye4Now";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

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
                    listBox1.Items.Add(zone);
        }
        private void Form1_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            try
            {
                if (loginStream != null)
                    loginStream.Disconnect();
            }
            catch (Exception)
            {
            }                 
        }
        #endregion

        #region Zone
        private void Button1_Click(object sender, EventArgs e)
        {
            var zone = listBox1.SelectedItem.ToString();
            zoneStream.SendChatMessage(string.Empty, string.Empty, $"#zone {zone}");
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
            if (this.textBox1.InvokeRequired)
            {
                SetLogCallback d = new SetLogCallback(Info);
                this.Invoke(d, new object[] { text, source });
            }
            else
            {
                string ds = $"{DateTime.Now}";
                switch (source)
                {
                    case LogSource.engine:
                        this.textBox1.AppendText($"{ds}:{text}{Environment.NewLine}");
                        break;
                    case LogSource.lua:
                        this.textBox3.AppendText($"{ds}:{text}{Environment.NewLine}");
                        break;                    
                }
                Logger.Info($"{source}:{text}");                
            }
            Application.DoEvents();
        }
        public void Error(string text)
        {
            Error(text, LogSource.engine);
        }
        public void Error(string text, LogSource source)
        {
            if (this.textBox1.InvokeRequired)
            {
                SetLogCallback d = new SetLogCallback(Error);
                this.Invoke(d, new object[] { text, source });
            }
            else
            {
                string ds = $"{DateTime.Now}";
                switch (source)
                {
                    case LogSource.engine:
                        this.textBox1.AppendText($"Error:{ds}:{text}{Environment.NewLine}");
                        break;
                    case LogSource.lua:
                        this.textBox3.AppendText($"Error:{ds}:{text}{Environment.NewLine}");
                        break;                    
                }
                Logger.Error($"{source}:{text}");
            }
            Application.DoEvents();
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
            if (this.textBox2.InvokeRequired)
            {
                SetPlayerProfileCallback d = new SetPlayerProfileCallback(UpdateCharacter);
                this.Invoke(d, new object[] { player });
            }
            else
            {
                this.textBox2.AppendText(player.ToString().Replace("\n", Environment.NewLine));
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
            _LogonResultEventHandler(success, reason);
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

        private void button2_Click(object sender, EventArgs e)
        {
            map.zoom = map.zoom + 0.1f;
            if (map.zoom > 2.0f)
                map.zoom = 2.0f;
            pictureBox1.BeginInvoke((Action)delegate { pictureBox1.Refresh(); });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            map.zoom = map.zoom - 0.1f;
            if (map.zoom == 0.0f)
                map.zoom = 0.1f;
            pictureBox1.BeginInvoke((Action)delegate { pictureBox1.Refresh(); });
        }
    }
}
