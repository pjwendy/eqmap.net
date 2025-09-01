using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenEQ.Netcode;
using EQBot.GameClient.Models;
using System.Collections.Generic;

namespace EQBot.GameClient
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
        
        private TaskCompletionSource<bool>? _loginCompletionSource;
        private bool _disposed = false;
        
        // Game State
        public Character? Character { get; private set; }
        public Zone? CurrentZone { get; private set; }
        public ConnectionState State { get; private set; } = ConnectionState.Disconnected;
        
        // Configuration
        public string LoginServer { get; set; } = "";
        public int LoginServerPort { get; set; } = 5999;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string WorldName { get; set; } = "";
        public string CharacterName { get; set; } = "";
        
        // Events - High level game events
        public event EventHandler<ConnectionState>? ConnectionStateChanged;
        public event EventHandler<Character>? CharacterLoaded;
        public event EventHandler<Zone>? ZoneChanged;
        public event EventHandler<NPC>? NPCSpawned;
        public event EventHandler<uint>? NPCDespawned; // SpawnID
        public event EventHandler<Player>? PlayerSpawned;
        public event EventHandler<uint>? PlayerDespawned; // SpawnID
        public event EventHandler<ChatMessage>? ChatMessageReceived;
        public event EventHandler<string>? LoginFailed;
        public event EventHandler? Disconnected;
        
        public EQGameClient(ILogger<EQGameClient> logger)
        {
            _logger = logger;
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
            
            _zoneStream.SendChatMessage("", "", message);
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
            _loginStream.Debug = false; // Reduce noise
            
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
                
                if (server.Longname.Contains(WorldName, StringComparison.OrdinalIgnoreCase))
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
            _worldStream.Debug = false; // Reduce noise
            
            // Set up world event handlers
            _worldStream.CharacterList += OnCharacterListReceived;
            _worldStream.ZoneServer += OnZoneServerReceived;
            _worldStream.MOTD += OnMOTDReceived;
            
            // Wait for character list
            await Task.Delay(3000);
        }
        
        private async void OnCharacterListReceived(object? sender, List<CharacterSelectEntry> characters)
        {
            _logger.LogInformation("Received character list with {Count} characters", characters.Count);
            
            CharacterSelectEntry? targetCharacter = null;
            foreach (var character in characters)
            {
                _logger.LogDebug("Available character: {CharacterName} (Level {Level})", character.Name, character.Level);
                
                if (character.Name.Equals(CharacterName, StringComparison.OrdinalIgnoreCase))
                {
                    targetCharacter = character;
                    break;
                }
            }
            
            if (targetCharacter == null)
            {
                _logger.LogError("Target character '{CharacterName}' not found", CharacterName);
                _loginCompletionSource?.SetResult(false);
                return;
            }
            
            _logger.LogInformation("Found target character: {CharacterName}", targetCharacter.Value.Name);
            
            // Create our Character model from the character select data
            Character = new Character
            {
                Name = targetCharacter.Value.Name,
                Level = targetCharacter.Value.Level,
                Race = targetCharacter.Value.Race,
                Class = targetCharacter.Value.Class,
                Zone = targetCharacter.Value.Zone
            };
            
            // Enter world with selected character
            var enterWorld = new EnterWorld(targetCharacter.Value.Name, false, false);
            _worldStream?.SendEnterWorld(enterWorld);
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
            _zoneStream.Debug = false; // Reduce noise
            
            // Set up zone event handlers
            _zoneStream.PlayerProfile += OnPlayerProfileReceived;
            _zoneStream.Spawned += OnSpawnReceived;
            _zoneStream.Message += OnMessageReceived;
            _zoneStream.Death += OnDeathReceived;
            _zoneStream.Zoned += OnZonedReceived;
            
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
            _loginCompletionSource?.SetResult(true);
        }
        
        private void OnSpawnReceived(object? sender, Spawn spawn)
        {
            if (CurrentZone == null) return;
            
            // Don't process our own character spawn
            if (spawn.Name == Character?.Name) return;
            
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
                NPCSpawned?.Invoke(this, npc);
            }
        }
        
        private void OnMessageReceived(object? sender, ChannelMessage message)
        {
            var chatMsg = new ChatMessage
            {
                Channel = (ChatChannel)message.ChanNum,
                From = message.From,
                To = message.To,
                Message = message.Message,
                Language = message.Language
            };
            
            _logger.LogInformation("Chat: {Message}", chatMsg.ToString());
            ChatMessageReceived?.Invoke(this, chatMsg);
        }
        
        private void OnDeathReceived(object? sender, Death death)
        {
            if (CurrentZone == null) return;
            
            // Remove dead spawns from tracking
            CurrentZone.RemoveNPC(death.SpawnId);
            CurrentZone.RemovePlayer(death.SpawnId);
            
            _logger.LogDebug("Death event: SpawnID {SpawnID}", death.SpawnId);
        }
        
        private void OnZonedReceived(object? sender, object? zone)
        {
            _logger.LogInformation("Zone change detected");
            // Handle zone transitions
        }
    }
}