using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenEQ.Netcode.GameClient.Models;
using OpenEQ.Netcode.GameClient.Events;
using OpenEQ.Netcode.GameClient.Navigation;
using System.Collections.Generic;
using EQProtocol.Streams.Login;
using EQProtocol.Streams.World;
using EQProtocol.Streams.Zone;
using EQProtocol.Streams.World.Packets;

namespace OpenEQ.Netcode.GameClient
{
    public enum ConnectionState
    {
        Disconnected,
        Connecting,
        LoginServer,
        WorldServer,
        ZoneServer,
        InGame
    }
    
    public class EQGameClient : IDisposable
    {
        private readonly ILogger<EQGameClient> _logger;
        private LoginStream? _loginStream;
        private WorldStream? _worldStream;
        private ZoneStream? _zoneStream;
        private IPacketEventEmitter? _packetEventEmitter;
        
        private TaskCompletionSource<bool>? _loginCompletionSource;
        private bool _disposed = false;
        
        // Navigation components
        private NavigationManager? _navigationManager;
        private MovementManager? _movementManager;
        private uint _positionSequence = 0;
        
        // Game State
        /// <summary>
        /// Gets the current character information once logged in
        /// </summary>
        public Character? Character { get; private set; }
        
        /// <summary>
        /// Gets the current zone the character is in
        /// </summary>
        public Zone? CurrentZone { get; private set; }
        
        /// <summary>
        /// Gets the current connection state of the game client
        /// </summary>
        public ConnectionState State { get; private set; } = ConnectionState.Disconnected;
        
        /// <summary>
        /// Gets the navigation manager for pathfinding and nav mesh operations
        /// </summary>
        public NavigationManager? Navigation => _navigationManager;
        
        /// <summary>
        /// Gets the movement manager for controlling character movement
        /// </summary>
        public MovementManager? Movement => _movementManager;
        
        // Configuration
        /// <summary>
        /// Gets or sets the login server hostname or IP address
        /// </summary>
        public string LoginServer { get; set; } = "";
        
        /// <summary>
        /// Gets or sets the login server port (default: 5999)
        /// </summary>
        public int LoginServerPort { get; set; } = 5999;
        
        /// <summary>
        /// Gets or sets the account username
        /// </summary>
        public string Username { get; set; } = "";
        
        /// <summary>
        /// Gets or sets the account password
        /// </summary>
        public string Password { get; set; } = "";
        
        /// <summary>
        /// Gets or sets the target world/server name
        /// </summary>
        public string WorldName { get; set; } = "";
        
        /// <summary>
        /// Gets or sets the character name to play
        /// </summary>
        public string CharacterName { get; set; } = "";
        
        /// <summary>
        /// Gets or sets the packet recording mode for narration engine
        /// </summary>
        public RecordingMode PacketRecordingMode { get; set; } = RecordingMode.Off;
        
        /// <summary>
        /// Gets or sets the base directory for packet capture files
        /// </summary>
        public string PacketCaptureDirectory { get; set; } = "";
        
        // Events - High level game events
        /// <summary>
        /// Raised when the connection state changes
        /// </summary>
        public event EventHandler<ConnectionState>? ConnectionStateChanged;
        
        /// <summary>
        /// Raised when the character has been loaded into the game
        /// </summary>
        public event EventHandler<Character>? CharacterLoaded;
        
        /// <summary>
        /// Raised when the character changes zones
        /// </summary>
        public event EventHandler<Zone>? ZoneChanged;
        
        /// <summary>
        /// Raised when an NPC spawns in the zone
        /// </summary>
        public event EventHandler<NPC>? NPCSpawned;
        
        /// <summary>
        /// Raised when an NPC despawns from the zone (provides SpawnID)
        /// </summary>
        public event EventHandler<uint>? NPCDespawned;
        
        /// <summary>
        /// Raised when another player spawns in the zone
        /// </summary>
        public event EventHandler<Player>? PlayerSpawned;
        
        /// <summary>
        /// Raised when another player despawns from the zone (provides SpawnID)
        /// </summary>
        public event EventHandler<uint>? PlayerDespawned;
        
        /// <summary>
        /// Raised when a chat message is received
        /// </summary>
        public event EventHandler<OpenEQ.Netcode.GameClient.Models.ChatMessage>? ChatMessageReceived;
        
        /// <summary>
        /// Raised when login fails with an error message
        /// </summary>
        public event EventHandler<string>? LoginFailed;
        
        /// <summary>
        /// Raised when the client is disconnected from the server
        /// </summary>
        public event EventHandler? Disconnected;
        /// <summary>
        /// Raised when a player position update is received
        /// </summary>
        public event EventHandler<ClientUpdateFromServer>? ClientUpdated;
        /// <summary>
        /// Raised when a mob position update is received
        /// </summary>
        public event EventHandler<MobUpdate>? MobUpdated;
        /// <summary>
        /// Raised when a NPC position update is received
        /// </summary>
        public event EventHandler<NPCMoveUpdate>? NPCMoveUpdated;
        /// <summary>
        /// Raised when a death event occurs
        /// </summary>
        public event EventHandler<Death>? DeathReceived;
        
        /// <summary>
        /// Raised when an entity is deleted from the zone
        /// </summary>
        public event EventHandler<uint>? EntityDeleted;
        
        /// <summary>
        /// Raised when a consider response is received
        /// </summary>
        public event EventHandler<Consider>? ConsiderReceived;
        
        /// <summary>
        /// Raised when mob health information is received
        /// </summary>
        public event EventHandler<MobHealth>? MobHealthReceived;
        
        /// <summary>
        /// Raised when damage is dealt or received
        /// </summary>
        public event EventHandler<Damage>? DamageReceived;
        
        /// <summary>
        /// Raised when a spell is cast
        /// </summary>
        public event EventHandler<CastSpell>? SpellCast;
        
        /// <summary>
        /// Raised when a spell cast is interrupted
        /// </summary>
        public event EventHandler<InterruptCast>? SpellInterrupted;
        
        /// <summary>
        /// Raised when an animation is played
        /// </summary>
        public event EventHandler<Animation>? AnimationReceived;
        
        /// <summary>
        /// Raised when a buff is applied or updated
        /// </summary>
        public event EventHandler<Buff>? BuffReceived;
        
        /// <summary>
        /// Raised when a ground spawn is detected
        /// </summary>
        public event EventHandler<GroundSpawn>? GroundSpawnReceived;
        
        /// <summary>
        /// Raised when tracking information is received
        /// </summary>
        public event EventHandler<Track>? TrackingReceived;
        
        /// <summary>
        /// Raised when an emote is received
        /// </summary>
        public event EventHandler<Emote>? EmoteReceived;
        /// <summary>
        /// Raised when experience points are gained or lost
        /// </summary>
        public event EventHandler<ExpUpdate>? ExperienceUpdated;
        
        /// <summary>
        /// Raised when the character's level changes
        /// </summary>
        public event EventHandler<LevelUpdate>? LevelChanged;
        
        /// <summary>
        /// Raised when a skill is updated
        /// </summary>
        public event EventHandler<SkillUpdate>? SkillUpdated;
        
        /// <summary>
        /// Raised when equipment is changed
        /// </summary>
        public event EventHandler<WearChange>? WearChanged;
        
        /// <summary>
        /// Raised when an item is moved in inventory
        /// </summary>
        public event EventHandler<MoveItem>? ItemMoved;
        
        /// <summary>
        /// Raised when the assist target changes
        /// </summary>
        public event EventHandler<Assist>? TargetAssisted;
        
        /// <summary>
        /// Raised when auto-attack is toggled on or off
        /// </summary>
        public event EventHandler<byte[]>? AutoAttackToggled;
        
        /// <summary>
        /// Raised when a charm effect is applied
        /// </summary>
        public event EventHandler<Charm>? CharmReceived;
        
        /// <summary>
        /// Raised when a stun effect is applied
        /// </summary>
        public event EventHandler<Stun>? StunReceived;
        
        /// <summary>
        /// Raised when an illusion is applied
        /// </summary>
        public event EventHandler<Illusion>? IllusionReceived;
        
        /// <summary>
        /// Raised when a sound effect is played
        /// </summary>
        public event EventHandler<Sound>? SoundReceived;
        
        public EQGameClient(ILogger<EQGameClient> logger)
        {
            _logger = logger;
            
            // Initialize navigation components
            var navigationLogger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<NavigationManager>();
            var movementLogger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<MovementManager>();
            
            _navigationManager = new NavigationManager(navigationLogger);
            _movementManager = new MovementManager(_navigationManager, movementLogger, SendPositionToServer);
            
            // Wire up movement events
            _movementManager.OnPositionUpdate += OnMovementPositionUpdate;
        }
        
        /// <summary>
        /// Complete login sequence - connects to login server, selects world, selects character
        /// </summary>
        public async Task<bool> LoginAsync(string username, string password, string worldName, string characterName)
        {
            if (State != ConnectionState.Disconnected)
            {
                throw new InvalidOperationException($"Cannot login while in state: {State}");
            }
            
            Username = username;
            Password = password;
            WorldName = worldName;
            CharacterName = characterName;
            
            _logger.LogInformation("Starting login sequence for {Username} -> {WorldName} -> {CharacterName}", 
                username, worldName, characterName);
            
            try
            {
                SetConnectionState(ConnectionState.Connecting);
                
                // Initialize packet capture if enabled
                InitializePacketCapture();
                
                _loginCompletionSource = new TaskCompletionSource<bool>();
                
                await ConnectToLoginServerAsync();
                
                // Wait for the entire login sequence to complete
                var success = await _loginCompletionSource.Task;
                
                if (success)
                {
                    _logger.LogInformation("Login sequence completed successfully");
                    return true;
                }
                else
                {
                    _logger.LogError("Login sequence failed");
                    SetConnectionState(ConnectionState.Disconnected);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login sequence");
                SetConnectionState(ConnectionState.Disconnected);
                LoginFailed?.Invoke(this, ex.Message);
                return false;
            }
        }
        
        /// <summary>
        /// Send a chat message
        /// </summary>
        public void SendChat(string message, ChatChannel channel = ChatChannel.Say)
        {
            if (_zoneStream == null || State != ConnectionState.InGame)
            {
                throw new InvalidOperationException("Not connected to game");
            }
            
            _zoneStream.SendChatMessage("", Character?.Name ?? "", message, (uint)channel);
            _logger.LogDebug("Sent chat message: {Message}", message);
        }
        
        /// <summary>
        /// Move character to a specific location
        /// </summary>
        public void MoveTo(float x, float y, float z, float heading = 0)
        {
            if (_zoneStream == null || Character == null || State != ConnectionState.InGame)
            {
                throw new InvalidOperationException("Not connected to game");
            }
            
            Character.X = x;
            Character.Y = y;
            Character.Z = z;
            Character.Heading = heading;
            
            _zoneStream.UpdatePosition(Tuple.Create(x, y, z, heading));
            _logger.LogDebug("Moving character to [{X:F1}, {Y:F1}, {Z:F1}]", x, y, z);
        }
        
        /// <summary>
        /// Get all NPCs within a certain radius of the character
        /// </summary>
        public IEnumerable<NPC> GetNearbyNPCs(float radius = 100.0f)
        {
            if (CurrentZone == null || Character == null)
                yield break;
                
            foreach (var npc in CurrentZone.GetNearbyNPCs(Character.X, Character.Y, radius))
            {
                yield return npc;
            }
        }
        
        /// <summary>
        /// Initialize packet capture system
        /// </summary>
        public void InitializePacketCapture()
        {
            if (PacketRecordingMode == RecordingMode.Off)
            {
                _packetEventEmitter?.Dispose();
                _packetEventEmitter = null;
                return;
            }
            
            var captureDir = string.IsNullOrEmpty(PacketCaptureDirectory) 
                ? null 
                : PacketCaptureDirectory;
                
            _packetEventEmitter?.Dispose();
            _packetEventEmitter = new FilePacketEventEmitter(captureDir, PacketRecordingMode);
            
            _logger.LogInformation("Packet capture initialized: Mode={Mode}, Directory={Directory}", 
                PacketRecordingMode, captureDir ?? "default");
        }
        
        /// <summary>
        /// Get the current packet event emitter (for stream integration)
        /// </summary>
        internal IPacketEventEmitter? GetPacketEventEmitter() => _packetEventEmitter;
        
        /// <summary>
        /// Disconnect from the game
        /// </summary>
        public void Disconnect()
        {
            _logger.LogInformation("Disconnecting from game");
            
            _zoneStream?.Disconnect();
            _worldStream?.Disconnect();
            _loginStream?.Disconnect();
            
            _zoneStream = null;
            _worldStream = null;
            _loginStream = null;
            
            Character = null;
            CurrentZone = null;
            
            SetConnectionState(ConnectionState.Disconnected);
            Disconnected?.Invoke(this, EventArgs.Empty);
        }
        
        private void SetConnectionState(ConnectionState newState)
        {
            if (State != newState)
            {
                var oldState = State;
                State = newState;
                _logger.LogDebug("Connection state changed: {OldState} -> {NewState}", oldState, newState);
                ConnectionStateChanged?.Invoke(this, newState);
            }
        }
        
        public void Dispose()
        {
            if (!_disposed)
            {
                _movementManager?.Dispose();
                _navigationManager?.ClearCache();
                
                _packetEventEmitter?.Dispose();
                Disconnect();
                _disposed = true;
            }
        }
        
        // Private implementation methods
        private async Task ConnectToLoginServerAsync()
        {
            _logger.LogInformation("Connecting to login server {Server}:{Port}", LoginServer, LoginServerPort);
            SetConnectionState(ConnectionState.LoginServer);
            
            _loginStream = new LoginStream(LoginServer, LoginServerPort);
            _loginStream.Debug = true; // Enable debug to see login packets
            _loginStream.SetPacketEmitter(_packetEventEmitter);
            
            // Set up login event handlers
            _loginStream.LoginSuccess += OnLoginSuccess;
            _loginStream.ServerList += OnServerListReceived;
            _loginStream.PlaySuccess += OnPlaySuccess;
            
            // Start login process
            _loginStream.Login(Username, Password);
            
            // Give some time for login to process
            await Task.Delay(5000);
        }
        
        private async void OnLoginSuccess(object? sender, bool success)
        {
            if (!success)
            {
                _logger.LogError("Login failed - invalid credentials");
                _loginCompletionSource?.SetResult(false);
                return;
            }
            
            _logger.LogInformation("Login successful, requesting server list");
            _loginStream?.RequestServerList();
        }
        
        private async void OnServerListReceived(object? sender, List<ServerListElement> servers)
        {
            _logger.LogInformation("Received {Count} servers from login server", servers.Count);
            
            // Find the target world server
            ServerListElement? targetServer = null;
            foreach (var server in servers)
            {
                _logger.LogDebug("Available server: {ServerName}", server.Longname);
                
                if (server.Longname.IndexOf(WorldName, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    targetServer = server;
                    break;
                }
            }
            
            if (targetServer == null)
            {
                _logger.LogError("Target world server '{WorldName}' not found", WorldName);
                _loginCompletionSource?.SetResult(false);
                return;
            }
            
            _logger.LogInformation("Found target server: {ServerName}", targetServer.Value.Longname);
            _loginStream?.Play(targetServer.Value);
        }
        
        private async void OnPlaySuccess(object? sender, ServerListElement? selectedServer)
        {
            if (!selectedServer.HasValue)
            {
                _logger.LogError("PlayEverquestResponse failed");
                _loginCompletionSource?.SetResult(false);
                return;
            }
            
            _logger.LogInformation("World server selected, connecting to world server");
            await ConnectToWorldServerAsync(selectedServer.Value);
        }
        
        private async Task ConnectToWorldServerAsync(ServerListElement server)
        {
            if (_loginStream == null)
            {
                _logger.LogError("Login stream is null");
                _loginCompletionSource?.SetResult(false);
                return;
            }
            
            _logger.LogInformation("Connecting to world server {IP}:9000", server.WorldIP);
            SetConnectionState(ConnectionState.WorldServer);
            
            _worldStream = new WorldStream(server.WorldIP, 9000, _loginStream.AccountID, _loginStream.SessionKey);
            _worldStream.Debug = true; // Enable debug to see world packets
            _worldStream.SetPacketEmitter(_packetEventEmitter);
            
            // Set up world event handlers
            _worldStream.CharacterList += OnCharacterListReceived;
            _worldStream.ZoneServer += OnZoneServerReceived;
            _worldStream.MOTD += OnMOTDReceived;
            _worldStream.EnterWorld += OnEnterWorldResponse;
            _worldStream.PostEnterWorld += OnPostEnterWorldReceived;
            
            // Wait for character list
            await Task.Delay(3000);
        }
        
        private async void OnCharacterListReceived(object? sender, List<CharacterSelectEntry> characters)
        {
            _logger.LogInformation("Received character list with {Count} characters", characters.Count);
            
            // Log all available characters
            _logger.LogInformation("=== AVAILABLE CHARACTERS ===");
            for (int i = 0; i < characters.Count; i++)
            {
                var character = characters[i];
                _logger.LogInformation("Available character #{Index}: {CharacterName} (Level {Level}, Race: {Race}, Class: {Class}, Zone: {Zone})",
                    i + 1, character.Name, character.Level, character.Race, character.Class, character.Zone);
            }
            _logger.LogInformation("============================");
            
            // Find the target character we're supposed to play
            CharacterSelectEntry? targetCharacter = null;
            foreach (var character in characters)
            {
                if (character.Name.Equals(CharacterName, StringComparison.OrdinalIgnoreCase))
                {
                    targetCharacter = character;
                    break;
                }
            }
            
            if (targetCharacter == null)
            {
                _logger.LogError("Target character '{CharacterName}' not found in character list", CharacterName);
                _loginCompletionSource?.SetResult(false);
                return;
            }
            
            _logger.LogInformation("=== SELECTING CHARACTER ===");
            _logger.LogInformation("Target character found: {CharacterName} (Level {Level})", 
                targetCharacter.Value.Name, targetCharacter.Value.Level);
            _logger.LogInformation("Selecting character: {CharacterName} (Level {Level})", targetCharacter.Value.Name, targetCharacter.Value.Level);
            
            // Create our Character model from the character select data
            Character = new Character
            {
                Name = targetCharacter.Value.Name,
                Level = targetCharacter.Value.Level,
                Race = targetCharacter.Value.Race,
                Class = targetCharacter.Value.Class,
                Zone = targetCharacter.Value.Zone
            };
            
            // Send OP_EnterWorld to start session with selected character
            _logger.LogInformation("Sending OP_EnterWorld packet for character: {CharacterName}", targetCharacter.Value.Name);
            var enterWorld = new EnterWorld(targetCharacter.Value.Name, false, false);
            _worldStream?.SendEnterWorld(enterWorld);            
            _logger.LogInformation("Sent OP_EnterWorld request for character: {CharacterName}", targetCharacter.Value.Name);
        }
        
        private async void OnZoneServerReceived(object? sender, ZoneServerInfo zoneServer)
        {
            _logger.LogInformation("Received zone server info: {Host}:{Port}", zoneServer.Host, zoneServer.Port);
            await ConnectToZoneServerAsync(zoneServer);
        }
        
        private void OnMOTDReceived(object? sender, string? motd)
        {
            if (!string.IsNullOrEmpty(motd))
            {
                _logger.LogInformation("Server MOTD: {MOTD}", motd.Replace("\0", "").Trim());
            }
        }
        
        private void OnEnterWorldResponse(object? sender, EnterWorld enterWorldStatus)
        {
            _logger.LogInformation("=== ENTER WORLD RESPONSE RECEIVED ===");
            _logger.LogInformation("EnterWorld response received - character selection acknowledged by server");
            _logger.LogInformation("Server acknowledged character selection - entering world...");            
        }
        
        private void OnPostEnterWorldReceived(object? sender, PostEnterWorld postEnterWorld)
        {
            _logger.LogInformation("=== POST ENTER WORLD RECEIVED ===");
            _logger.LogInformation("PostEnterWorld received - character setup complete, ready for zone server");
            _logger.LogInformation("Character setup complete - ready to connect to zone server");
        }
        
        private async Task ConnectToZoneServerAsync(ZoneServerInfo zoneServer)
        {
            if (Character == null)
            {
                _logger.LogError("Character is null when connecting to zone server");
                _loginCompletionSource?.SetResult(false);
                return;
            }
            
            _logger.LogInformation("Connecting to zone server {Host}:{Port}", zoneServer.Host, zoneServer.Port);
            SetConnectionState(ConnectionState.ZoneServer);
            
            _zoneStream = new ZoneStream(zoneServer.Host, zoneServer.Port, Character.Name);
            _zoneStream.Debug = true; // Enable debug to see zone packets
            _zoneStream.SetPacketEmitter(_packetEventEmitter);
            
            // Set up zone event handlers
            _zoneStream.PlayerProfile += OnPlayerProfileReceived;
            _zoneStream.ZoneEntry += OnSpawnReceived;
            _zoneStream.Message += OnMessageReceived;
            _zoneStream.Death += OnDeathReceived;
            //_zoneStream.Zoned += OnZonedReceived;
            _zoneStream.ClientUpdate += OnClientUpdated;
            _zoneStream.MobUpdate += OnMobUpdated;
            _zoneStream.NPCMoveUpdate += OnNPCMoveUpdated;

            // Wire up additional events
            _zoneStream.DeleteSpawn += OnDeleteSpawn;
            _zoneStream.Consider += (s, e) => ConsiderReceived?.Invoke(this, e);
            _zoneStream.MobHealth += (s, e) => MobHealthReceived?.Invoke(this, e);
            _zoneStream.Damage += (s, e) => DamageReceived?.Invoke(this, e);
            _zoneStream.CastSpell += (s, e) => SpellCast?.Invoke(this, e);
            _zoneStream.InterruptCast += (s, e) => SpellInterrupted?.Invoke(this, e);
            _zoneStream.Animation += (s, e) => AnimationReceived?.Invoke(this, e);
            _zoneStream.Buff += (s, e) => BuffReceived?.Invoke(this, e);
            _zoneStream.GroundSpawn += (s, e) => GroundSpawnReceived?.Invoke(this, e);
            _zoneStream.Track += (s, e) => TrackingReceived?.Invoke(this, e);
            _zoneStream.Emote += (s, e) => EmoteReceived?.Invoke(this, e);
            _zoneStream.ExpUpdate += (s, e) => ExperienceUpdated?.Invoke(this, e);
            _zoneStream.LevelUpdate += (s, e) => LevelChanged?.Invoke(this, e);
            _zoneStream.SkillUpdate += (s, e) => SkillUpdated?.Invoke(this, e);
            _zoneStream.WearChange += (s, e) => WearChanged?.Invoke(this, e);
            _zoneStream.MoveItem += (s, e) => ItemMoved?.Invoke(this, e);
            _zoneStream.Assist += (s, e) => TargetAssisted?.Invoke(this, e);
            _zoneStream.AutoAttack += (s, e) => AutoAttackToggled?.Invoke(this, e);
            _zoneStream.Charm += (s, e) => CharmReceived?.Invoke(this, e);
            _zoneStream.Stun += (s, e) => StunReceived?.Invoke(this, e);
            _zoneStream.Illusion += (s, e) => IllusionReceived?.Invoke(this, e);
            _zoneStream.Sound += (s, e) => SoundReceived?.Invoke(this, e);
            
            await Task.Delay(3000);
        }
        
        private void OnPlayerProfileReceived(object? sender, PlayerProfile profile)
        {
            if (Character == null) return;
            
            _logger.LogInformation("Player profile loaded: {Name} Level {Level} in Zone {Zone}", 
                profile.Name, profile.Level, profile.ZoneID);
            
            // Update character with full profile data
            Character.Name = profile.Name;
            Character.Level = profile.Level;
            Character.HP = profile.CurHP;
            Character.MaxHP = profile.CurHP; // We'll need to find MaxHP elsewhere
            Character.Mana = profile.Mana;
            Character.MaxMana = profile.Mana; // We'll need to find MaxMana elsewhere
            Character.X = profile.X;
            Character.Y = profile.Y;
            Character.Z = profile.Z;
            Character.Zone = profile.ZoneID;
            
            // Initialize current zone
            CurrentZone = new Zone
            {
                ZoneID = profile.ZoneID,
                Name = $"Zone_{profile.ZoneID}" // We'll get the real name later
            };
            
            SetConnectionState(ConnectionState.InGame);
            
            _logger.LogInformation("=== LOGIN COMPLETE - CHARACTER IN GAME ===");
            CharacterLoaded?.Invoke(this, Character);
            ZoneChanged?.Invoke(this, CurrentZone);  // Fire the zone changed event
            
            // Don't send spawn appearance here - we don't have SpawnID yet!
            // It will be sent when we receive our spawn in OnSpawnReceived
            
            _loginCompletionSource?.SetResult(true);
        }
        
        private void OnSpawnReceived(object? sender, ZoneEntry spawn)
        {
            if (CurrentZone == null) return;
            
            // Process ALL spawns including our own character
            // The UI needs to see our character spawn to display the bot
            
            // Check if this is our own character spawn
            if (spawn.Name == Character?.Name)
            {
                Character.SpawnID = spawn.SpawnID;
                _logger.LogInformation("Our character spawn received: {Name} with SpawnID {SpawnID}", 
                    spawn.Name, spawn.SpawnID);
                    
                // Don't send SpawnAppearance - OpenEQ doesn't send it!
                // The server handles our visibility
            }
            
            // Determine if this is an NPC or Player based on spawn data
            if (spawn.CharType == CharType.PC) // Player
            {
                var player = new Player
                {
                    SpawnID = spawn.SpawnID,
                    Name = spawn.Name,
                    Level = spawn.Level,
                    Race = (uint)spawn.Race,
                    Gender = 0, // Not available in spawn data
                    Class = spawn.Class,
                    X = spawn.Position.X,
                    Y = spawn.Position.Y,
                    Z = spawn.Position.Z,
                    Heading = spawn.Position.Heading
                };
                
                CurrentZone.AddPlayer(player);
                _logger.LogDebug("Player spawned: {PlayerName}", player.Name);
                
                // Emit game event for narration engine
                _packetEventEmitter?.EmitGameEvent(GameEventHelper.CreateSpawnEvent(
                    spawn.SpawnID.ToString(), 
                    spawn.Name, 
                    "entity.player_spawn", 
                    CurrentZone.Name, 
                    spawn.Position.X, 
                    spawn.Position.Y, 
                    spawn.Position.Z,
                    new Dictionary<string, object>
                    {
                        {"level", spawn.Level},
                        {"race", spawn.Race.ToString()},
                        {"class", spawn.Class.ToString()}
                    }));
                
                PlayerSpawned?.Invoke(this, player);
            }
            else // NPC
            {
                var npc = new NPC
                {
                    SpawnID = spawn.SpawnID,
                    Name = spawn.Name,
                    Level = spawn.Level,
                    Race = (uint)spawn.Race,
                    Gender = 0, // Not available in spawn data
                    Class = spawn.Class,
                    X = spawn.Position.X,
                    Y = spawn.Position.Y,
                    Z = spawn.Position.Z,
                    Heading = spawn.Position.Heading,
                    BodyType = spawn.BodyType,
                    Size = (uint)spawn.Size
                };
                
                CurrentZone.AddNPC(npc);
                _logger.LogDebug("NPC spawned: {NPCName}", npc.Name);
                
                // Emit game event for narration engine
                _packetEventEmitter?.EmitGameEvent(GameEventHelper.CreateSpawnEvent(
                    spawn.SpawnID.ToString(), 
                    spawn.Name, 
                    "entity.npc_spawn", 
                    CurrentZone.Name, 
                    spawn.Position.X, 
                    spawn.Position.Y, 
                    spawn.Position.Z,
                    new Dictionary<string, object>
                    {
                        {"level", spawn.Level},
                        {"race", spawn.Race.ToString()},
                        {"class", spawn.Class.ToString()},
                        {"body_type", spawn.BodyType.ToString()},
                        {"size", spawn.Size}
                    }));
                
                NPCSpawned?.Invoke(this, npc);
            }
        }
        
        void OnMessageReceived(object? sender, ChannelMessage message)
        {
            var chatMsg = new OpenEQ.Netcode.GameClient.Models.ChatMessage
            {
                Channel = (ChatChannel)message.ChanNum,
                From = message.From,
                To = message.To, 
                Message = message.Message, 
                Language = message.Language
            };

            _logger.LogInformation("Chat: {Message}", chatMsg.ToString());

            // Emit game event for narration engine
            _packetEventEmitter?.EmitGameEvent(GameEventHelper.CreateChatEvent(
                "unknown", // We don't have actor ID from chat message
                chatMsg.From,
                chatMsg.Channel.ToString(),
                chatMsg.Message,
                CurrentZone?.Name ?? "unknown"));

            ChatMessageReceived?.Invoke(this, chatMsg);
        }
        
        private void OnDeathReceived(object? sender, Death death)
        {
            if (CurrentZone == null) return;
            
            // Check if it was an NPC or Player and fire appropriate despawn events
            bool wasNPC = CurrentZone.NPCs.ContainsKey(death.SpawnId);
            bool wasPlayer = CurrentZone.Players.ContainsKey(death.SpawnId);
            
            // Remove dead spawns from tracking
            CurrentZone.RemoveNPC(death.SpawnId);
            CurrentZone.RemovePlayer(death.SpawnId);
            
            // Fire despawn events so map gets updated
            if (wasNPC)
            {
                NPCDespawned?.Invoke(this, death.SpawnId);
                _logger.LogDebug("NPC died and despawned: SpawnID {SpawnID}", death.SpawnId);
            }
            if (wasPlayer)
            {
                PlayerDespawned?.Invoke(this, death.SpawnId);
                _logger.LogDebug("Player died and despawned: SpawnID {SpawnID}", death.SpawnId);
            }
            
            // Forward the death event
            DeathReceived?.Invoke(this, death);
            
            _logger.LogDebug("Death event: SpawnID {SpawnID}", death.SpawnId);
        }
        
        private void OnDeleteSpawn(object? sender, uint spawnId)
        {
            if (CurrentZone == null) return;
            
            // Check if it was an NPC or Player and fire appropriate despawn events
            bool wasNPC = CurrentZone.NPCs.ContainsKey(spawnId);
            bool wasPlayer = CurrentZone.Players.ContainsKey(spawnId);
            
            // Remove from tracking
            CurrentZone.RemoveNPC(spawnId);
            CurrentZone.RemovePlayer(spawnId);
            
            // Fire despawn events so map gets updated
            if (wasNPC)
            {
                NPCDespawned?.Invoke(this, spawnId);
                _logger.LogDebug("NPC deleted and despawned: SpawnID {SpawnID}", spawnId);
            }
            if (wasPlayer)
            {
                PlayerDespawned?.Invoke(this, spawnId);
                _logger.LogDebug("Player deleted and despawned: SpawnID {SpawnID}", spawnId);
            }
            
            // Forward the entity deleted event
            EntityDeleted?.Invoke(this, spawnId);
            
            _logger.LogDebug("Delete spawn event: SpawnID {SpawnID}", spawnId);
        }

        private void OnZonedReceived(object? sender, object? zone)
        {
            _logger.LogInformation("Zone change detected");
            
            // Load navigation mesh for the new zone
            if (CurrentZone?.Name != null)
            {
                _ = Task.Run(() => LoadNavMeshForCurrentZone());
            }
        }
        
        private void OnClientUpdated(object? sender, ClientUpdateFromServer update)
        {
            _logger.LogDebug("ClientUpdate received for SpawnID {SpawnID} - Position: ({X}, {Y}, {Z}) Heading: {Heading}",
                update.ID, update.Position.X, update.Position.Y, update.Position.Z, update.Position.Heading);

            // Update movement manager and character position if this is our character
            if (update.ID == Character?.SpawnID && Character != null)
            {
                _logger.LogDebug("Updating character and movement manager with current position");

                // Update character position
                Character.X = update.Position.X;
                Character.Y = update.Position.Y;
                Character.Z = update.Position.Z;
                Character.Heading = update.Position.Heading;

                // Update movement manager with current position
                _movementManager?.SetCurrentPosition(update.Position.X, update.Position.Y, update.Position.Z);
            }

            // Forward the position update event
            ClientUpdated?.Invoke(this, update);
        }
        private void OnMobUpdated(object? sender, MobUpdate update)
        {
            _logger.LogDebug("MobUpdate received for SpawnID {SpawnID} - Position: ({X}, {Y}, {Z}) Heading: {Heading}",
                update.ID, update.Position.X, update.Position.Y, update.Position.Z, update.Position.Heading);
            // Forward the position update event
            MobUpdated?.Invoke(this, update);
        }

        private void OnNPCMoveUpdated(object? sender, NPCMoveUpdate update)
        {
            _logger.LogDebug("NPCMoveUpdate received for SpawnID {SpawnID} - Position: ({X}, {Y}, {Z}) Heading: {Heading}",
                update.ID, update.Position.X, update.Position.Y, update.Position.Z, update.Position.Heading);   
            // Forward the position update event
            NPCMoveUpdated?.Invoke(this, update);
        }

        /// <summary>
        /// Moves the character to the specified coordinates using pathfinding
        /// </summary>
        public async Task<bool> MoveTo(float x, float y, float z)
        {
            if (_movementManager == null)
            {
                _logger.LogWarning("Cannot move - movement manager not initialized");
                return false;
            }

            if (State != ConnectionState.InGame)
            {
                _logger.LogWarning("Cannot move - not in game");
                return false;
            }
            _logger.LogDebug("Move to X:{},Y:{},Z:{}", x, y, z);
            return await _movementManager.MoveTo(x, y, z);
        }

        /// <summary>
        /// Stops any current movement
        /// </summary>
        public void StopMovement()
        {
            _movementManager?.StopMovement();
        }

        /// <summary>
        /// Gets or sets the movement speed multiplier (for buffs, debuffs, etc.)
        /// </summary>
        public float MovementSpeedMultiplier
        {
            get => _movementManager?.Speed.SpeedMultiplier ?? 1.0f;
            set
            {
                if (_movementManager != null)
                {
                    _movementManager.Speed.SpeedMultiplier = value;
                }
            }
        }

        /// <summary>
        /// Gets whether the character is currently moving
        /// </summary>
        public bool IsMoving => _movementManager?.IsMoving ?? false;

        #region Zone and Position Information

        /// <summary>
        /// Get the current zone name using proper zone mapping
        /// </summary>
        public string ZoneName => CurrentZone != null ? ZoneUtils.ZoneNumberToName(CurrentZone.ZoneID) : "";

        /// <summary>
        /// Get the current zone ID
        /// </summary>
        public uint ZoneId => CurrentZone?.ZoneID ?? 0;

        /// <summary>
        /// Get player's current X coordinate
        /// </summary>
        public float X => Character?.X ?? 0;

        /// <summary>
        /// Get player's current Y coordinate
        /// </summary>
        public float Y => Character?.Y ?? 0;

        /// <summary>
        /// Get player's current Z coordinate
        /// </summary>
        public float Z => Character?.Z ?? 0;

        /// <summary>
        /// Get player's current heading
        /// </summary>
        public float Heading => Character?.Heading ?? 0;

        /// <summary>
        /// Get player's name
        /// </summary>
        public string PlayerName => Character?.Name ?? "";

        /// <summary>
        /// Get all position information as a formatted string
        /// </summary>
        public string Position => $"({Y:F1}, {X:F1}, {Z:F1})";

        /// <summary>
        /// Get full location information as a formatted string
        /// </summary>
        public string Location => $"{ZoneName} {Position}";

        /// <summary>
        /// Get distance between current position and target coordinates
        /// </summary>
        /// <param name="targetX">Target X coordinate</param>
        /// <param name="targetY">Target Y coordinate</param>
        /// <param name="targetZ">Target Z coordinate (optional, defaults to current Z)</param>
        /// <returns>3D distance if Z provided, 2D distance otherwise</returns>
        public double DistanceTo(double targetX, double targetY, double targetZ = double.NaN)
        {
            double dx = X - targetX;
            double dy = Y - targetY;

            if (double.IsNaN(targetZ))
            {
                // 2D distance
                return Math.Sqrt(dx * dx + dy * dy);
            }
            else
            {
                // 3D distance
                double dz = Z - targetZ;
                return Math.Sqrt(dx * dx + dy * dy + dz * dz);
            }
        }

        /// <summary>
        /// Check if player is within a certain distance of target coordinates
        /// </summary>
        /// <param name="targetX">Target X coordinate</param>
        /// <param name="targetY">Target Y coordinate</param>
        /// <param name="maxDistance">Maximum distance</param>
        /// <param name="targetZ">Target Z coordinate (optional)</param>
        /// <returns>True if within distance</returns>
        public bool IsWithinDistance(double targetX, double targetY, double maxDistance, double targetZ = double.NaN)
        {
            return DistanceTo(targetX, targetY, targetZ) <= maxDistance;
        }

        /// <summary>
        /// Sleep for specified milliseconds - useful in Lua scripts for delays
        /// </summary>
        /// <param name="milliseconds">Number of milliseconds to sleep</param>
        public void Sleep(double milliseconds)
        {
            if (milliseconds <= 0) return;
            System.Threading.Thread.Sleep((int)Math.Round(milliseconds));
        }

        /// <summary>
        /// Sleep for specified seconds - convenience method for Lua scripts
        /// </summary>
        /// <param name="seconds">Number of seconds to sleep</param>
        public void SleepSeconds(double seconds)
        {
            Sleep(seconds * 1000);
        }

        #endregion

        private async Task LoadNavMeshForCurrentZone()
        {
            if (_navigationManager == null || CurrentZone?.Name == null) return;

            _logger.LogInformation("Loading nav mesh for zone: {ZoneName}", CurrentZone.Name);
            
            var success = await Task.Run(() => _navigationManager.LoadNavMeshForZone(CurrentZone.Name));
            
            if (success)
            {
                _logger.LogInformation("Nav mesh loaded successfully for zone: {ZoneName}", CurrentZone.Name);
            }
            else
            {
                _logger.LogWarning("Failed to load nav mesh for zone: {ZoneName}", CurrentZone.Name);
            }
        }

        private void SendPositionToServer(float x, float y, float z, float heading)
        {
            try
            {
                if (_zoneStream == null || State != ConnectionState.InGame)
                {
                    return;
                }

                _logger.LogDebug("Sending position to server: ({}, {}, {}) Heading: {}", x, y, z, heading);

                // Send position update directly to server via ZoneStream
                var position = new Tuple<float, float, float, float>(x, y, z, heading);
                _zoneStream.UpdatePosition(position);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending position to server");
            }
        }

        private void OnMovementPositionUpdate(float x, float y, float z, float heading)
        {
            try
            {
                _logger.LogDebug("GameClient received local movement position update: (X: {}, Y: {}, Z: {}) Heading: {}", x, y, z, heading);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing movement position update");
            }
        }


    }
}