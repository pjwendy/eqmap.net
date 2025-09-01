# EQGameClient API Documentation

## Overview

The EQGameClient is a high-level abstraction layer for EverQuest bot development that hides the complexity of the underlying EverQuest protocol (LoginStream, WorldStream, ZoneStream). It provides a clean, event-driven API that allows developers to focus on bot logic rather than packet handling and network protocols.

## Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    Bot Application Layer                     │
│  (SimpleBot, custom bots using high-level game events)      │
└─────────────────────────────────────────────────────────────┘
                              │
┌─────────────────────────────────────────────────────────────┐
│                     EQGameClient                            │
│  • LoginAsync() - Single method login sequence              │
│  • High-level game state (Character, CurrentZone)          │
│  • Simple API methods (SendChat, MoveTo, GetNearbyNPCs)    │
│  • Event-driven architecture (CharacterLoaded, NPCSpawned) │
└─────────────────────────────────────────────────────────────┘
                              │
┌─────────────────────────────────────────────────────────────┐
│                  EverQuest Protocol Layer                   │
│     LoginStream → WorldStream → ZoneStream                  │
│  • Packet handling, authentication, session management      │
│  • Low-level protocol implementation (Underfoot/UF)         │
└─────────────────────────────────────────────────────────────┘
```

## Key Benefits

1. **Simplified Development**: Reduced bot complexity from 380+ lines to 60 lines
2. **Single Login Method**: `LoginAsync()` handles entire login sequence automatically
3. **Event-Driven**: Clean separation between protocol events and bot logic
4. **High-Level State**: Access game state through properties like `Character` and `CurrentZone`
5. **Type Safety**: Strongly-typed models for all game entities
6. **Maintainable**: Protocol changes isolated from bot logic

## API Reference

### Core Classes

#### EQGameClient
Main client class providing high-level game interface.

**Properties:**
- `Character? Character` - Current player character state
- `Zone? CurrentZone` - Current zone with NPCs, players, doors
- `ConnectionState State` - Current connection state
- `string LoginServer` - Login server hostname/IP
- `int LoginServerPort` - Login server port (default: 5999)
- `string Username` - Account username
- `string Password` - Account password  
- `string WorldName` - Target world server name
- `string CharacterName` - Target character name

**Methods:**
- `Task<bool> LoginAsync(username, password, worldName, characterName)` - Complete login sequence
- `void SendChat(message, channel = Say)` - Send chat message
- `void MoveTo(x, y, z, heading = 0)` - Move character to location
- `IEnumerable<NPC> GetNearbyNPCs(radius = 100f)` - Get NPCs within radius
- `void Disconnect()` - Disconnect from game
- `void Dispose()` - Clean up resources

**Events:**
- `EventHandler<ConnectionState> ConnectionStateChanged` - Connection state updates
- `EventHandler<Character> CharacterLoaded` - Character enters game
- `EventHandler<Zone> ZoneChanged` - Zone transitions
- `EventHandler<NPC> NPCSpawned` - NPC appears in zone
- `EventHandler<uint> NPCDespawned` - NPC removed (SpawnID)
- `EventHandler<Player> PlayerSpawned` - Player enters zone
- `EventHandler<uint> PlayerDespawned` - Player leaves (SpawnID)
- `EventHandler<ChatMessage> ChatMessageReceived` - Chat messages
- `EventHandler<string> LoginFailed` - Login failure reasons
- `EventHandler Disconnected` - Connection lost

### Data Models

#### Character : Entity
Player character with stats and attributes.

**Properties:**
- `uint Level` - Character level
- `uint Zone` - Current zone ID
- `string ZoneName` - Current zone name
- `uint HP, MaxHP` - Health points
- `uint Mana, MaxMana` - Mana points  
- `uint Endurance, MaxEndurance` - Endurance points
- `uint Strength, Stamina, Agility, Dexterity, Wisdom, Intelligence, Charisma` - Attributes
- `uint GuildID` - Guild identifier
- `string GuildName` - Guild name
- `uint Experience, AAExperience` - Experience points

#### Zone
Current zone state with entities.

**Properties:**
- `uint ZoneID` - Zone identifier
- `string Name` - Zone name
- `ConcurrentDictionary<uint, NPC> NPCs` - All NPCs by SpawnID
- `ConcurrentDictionary<uint, Player> Players` - All players by SpawnID
- `ConcurrentDictionary<uint, Door> Doors` - All doors by ID

**Methods:**
- `IEnumerable<NPC> GetNearbyNPCs(x, y, radius)` - NPCs within radius
- `IEnumerable<Player> GetNearbyPlayers(x, y, radius)` - Players within radius
- `void AddNPC(npc), RemoveNPC(spawnId)` - NPC management
- `void AddPlayer(player), RemovePlayer(spawnId)` - Player management
- `void AddDoor(door), RemoveDoor(doorId)` - Door management

#### Entity (Base Class)
Base class for all positioned game entities.

**Properties:**
- `uint SpawnID` - Unique spawn identifier
- `string Name` - Entity name
- `uint Level` - Entity level
- `uint Race` - Race ID
- `uint Gender` - Gender (0=Male, 1=Female, 2=Neuter)
- `uint Class` - Class ID
- `float X, Y, Z` - World coordinates
- `float Heading` - Facing direction (0-255)

**Methods:**
- `float DistanceTo(Entity other)` - Calculate distance to another entity
- `string ToString()` - Formatted entity description

#### NPC : Entity
Non-player character with combat stats.

**Properties:**
- `uint BodyType` - Body type for combat calculations
- `uint Size` - NPC size/scale
- `bool IsAlive` - Whether NPC is alive
- `bool IsAttackable` - Whether NPC can be attacked

#### Player : Entity  
Other player characters in the zone.

**Properties:**
- `bool IsAlive` - Whether player is alive
- `string GuildName` - Player's guild name

#### Door
Zone doors and portals.

**Properties:**
- `uint DoorID` - Door identifier  
- `string Name` - Door name
- `float X, Y, Z` - Door location
- `float Heading` - Door orientation
- `uint ZoneID` - Destination zone ID
- `bool IsOpen` - Door state

#### ChatMessage
Chat communication events.

**Properties:**
- `ChatChannel Channel` - Message channel (Say, Tell, Guild, etc.)
- `string From` - Sender name
- `string To` - Recipient name (for tells)
- `string Message` - Message content
- `uint Language` - Language ID
- `bool IsTell` - Whether message is a private tell

**Methods:**
- `string ToString()` - Formatted chat message

### Connection States

```csharp
public enum ConnectionState
{
    Disconnected,    // Not connected
    Connecting,      // Initiating connection
    LoginServer,     // Authenticating with login server
    WorldServer,     // Connected to world server
    ZoneServer,      // Connected to zone server
    InGame          // Fully connected and in-game
}
```

### Chat Channels

```csharp
public enum ChatChannel
{
    Say = 0,         // Local say
    Tell = 1,        // Private message
    Group = 2,       // Group chat
    Guild = 3,       // Guild chat
    OOC = 4,         // Out of character
    Auction = 5,     // Auction channel
    Shout = 6,       // Zone shout
    // ... additional channels
}
```

## Usage Examples

### Basic Bot Implementation

```csharp
public class SimpleBot
{
    private readonly EQGameClient _gameClient;
    private readonly ILogger _logger;

    public SimpleBot(EQGameClient gameClient, ILogger logger)
    {
        _gameClient = gameClient;
        _logger = logger;

        // Set up event handlers
        _gameClient.CharacterLoaded += OnCharacterLoaded;
        _gameClient.NPCSpawned += OnNPCSpawned;
        _gameClient.ChatMessageReceived += OnChatMessageReceived;
    }

    public async Task RunAsync(string username, string password, 
                               string worldName, string characterName)
    {
        // Single method call handles entire login sequence!
        var loginSuccess = await _gameClient.LoginAsync(username, password, 
                                                       worldName, characterName);
        
        if (!loginSuccess)
        {
            _logger.LogError("Failed to login");
            return;
        }

        // Bot is now in-game and ready!
        await RunBotBehaviorAsync();
    }

    private async Task RunBotBehaviorAsync()
    {
        while (_gameClient.State == ConnectionState.InGame)
        {
            // Check character status
            if (_gameClient.Character != null)
            {
                _logger.LogInfo("HP: {HP}/{MaxHP}", 
                    _gameClient.Character.HP, _gameClient.Character.MaxHP);
            }

            // Look for nearby NPCs
            foreach (var npc in _gameClient.GetNearbyNPCs(50.0f))
            {
                if (npc.IsAlive && npc.IsAttackable)
                {
                    _logger.LogInfo("Found target: {Name} at distance {Distance:F1}",
                        npc.Name, _gameClient.Character?.DistanceTo(npc) ?? 0);
                }
            }

            // Send periodic messages
            _gameClient.SendChat($"Bot online! Zone has {_gameClient.CurrentZone?.NPCs.Count ?? 0} NPCs");

            await Task.Delay(60000); // Wait 60 seconds
        }
    }

    private void OnCharacterLoaded(object? sender, Character character)
    {
        _logger.LogInfo("=== CHARACTER READY ===");
        _logger.LogInfo("Playing as: {Character}", character);
    }

    private void OnNPCSpawned(object? sender, NPC npc)
    {
        _logger.LogDebug("NPC spawned: {NPC}", npc);
    }

    private void OnChatMessageReceived(object? sender, ChatMessage message)
    {
        _logger.LogInfo("Chat: {Message}", message);
        
        // Respond to tells
        if (message.IsTell && message.Message.Contains("hello"))
        {
            // Future: _gameClient.SendTell(message.From, "Hello there!");
        }
    }
}
```

### Configuration Setup

**appsettings.json:**
```json
{
  "EQServer": {
    "LoginServer": "172.29.179.249",
    "LoginServerPort": 5999,
    "Username": "your_username",
    "Password": "your_password", 
    "WorldName": "Honeytree",
    "CharacterName": "YourCharacter"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "OpenEQ.Netcode": "Debug"
    }
  }
}
```

**Program.cs:**
```csharp
public static async Task Main(string[] args)
{
    var host = CreateHostBuilder(args).Build();
    
    var logger = host.Services.GetRequiredService<ILogger<Program>>();
    var config = host.Services.GetRequiredService<IConfiguration>();
    var gameClient = new EQGameClient(host.Services.GetRequiredService<ILogger<EQGameClient>>());

    var serverConfig = config.GetSection("EQServer").Get<EQServerConfig>();
    
    // Configure connection
    gameClient.LoginServer = serverConfig.LoginServer;
    gameClient.LoginServerPort = serverConfig.LoginServerPort;

    var bot = new SimpleBot(gameClient, logger);
    await bot.RunAsync(serverConfig.Username, serverConfig.Password, 
                      serverConfig.WorldName, serverConfig.CharacterName);
}
```

## Advanced Usage

### Custom Event Handling
```csharp
// Monitor zone population
_gameClient.PlayerSpawned += (sender, player) => 
{
    Console.WriteLine($"Player {player.Name} entered zone");
};

_gameClient.PlayerDespawned += (sender, spawnId) =>
{
    Console.WriteLine($"Player {spawnId} left zone");
};

// Track connection state
_gameClient.ConnectionStateChanged += (sender, state) =>
{
    Console.WriteLine($"Connection: {state}");
    if (state == ConnectionState.InGame)
    {
        Console.WriteLine("Bot is ready for commands!");
    }
};
```

### Zone Analysis
```csharp
private void AnalyzeZone()
{
    if (_gameClient.CurrentZone == null) return;

    var zone = _gameClient.CurrentZone;
    
    Console.WriteLine($"Zone: {zone.Name} (ID: {zone.ZoneID})");
    Console.WriteLine($"Players: {zone.Players.Count}");
    Console.WriteLine($"NPCs: {zone.NPCs.Count}");
    Console.WriteLine($"Doors: {zone.Doors.Count}");

    // Find hostile NPCs near character
    if (_gameClient.Character != null)
    {
        var hostileNPCs = zone.GetNearbyNPCs(_gameClient.Character.X, 
                                           _gameClient.Character.Y, 100.0f)
                              .Where(npc => npc.IsAlive && npc.IsAttackable);
                              
        Console.WriteLine($"Hostile NPCs nearby: {hostileNPCs.Count()}");
    }
}
```

### Movement and Navigation
```csharp
// Move to specific coordinates
_gameClient.MoveTo(100.5f, -200.3f, 25.0f, 128); // heading 128 = south

// Move toward an NPC
var targetNPC = _gameClient.GetNearbyNPCs(200f).FirstOrDefault(npc => npc.Name == "orc pawn");
if (targetNPC != null)
{
    _gameClient.MoveTo(targetNPC.X, targetNPC.Y, targetNPC.Z);
    Console.WriteLine($"Moving to {targetNPC.Name} at [{targetNPC.X:F1}, {targetNPC.Y:F1}]");
}
```

## Error Handling

```csharp
try
{
    var loginSuccess = await _gameClient.LoginAsync(username, password, worldName, characterName);
    if (!loginSuccess)
    {
        _logger.LogError("Login failed");
        return;
    }
}
catch (InvalidOperationException ex)
{
    _logger.LogError("Cannot login while already connected: {Message}", ex.Message);
}
catch (Exception ex)
{
    _logger.LogError(ex, "Unexpected error during login");
}

// Handle disconnections
_gameClient.Disconnected += (sender, e) =>
{
    _logger.LogWarning("Lost connection to game server");
    // Implement reconnection logic here
};

_gameClient.LoginFailed += (sender, reason) =>
{
    _logger.LogError("Login failed: {Reason}", reason);
    // Handle login failures (bad credentials, server down, etc.)
};
```

## Performance Considerations

1. **Event Handler Efficiency**: Keep event handlers fast to avoid blocking packet processing
2. **NPC Queries**: Use radius limits when calling `GetNearbyNPCs()` to avoid performance issues in crowded zones
3. **Chat Frequency**: Limit chat message frequency to avoid server rate limiting
4. **Memory Management**: The client automatically manages entity lifecycle, but custom event handlers should avoid memory leaks

## Thread Safety

- All public methods and properties are thread-safe
- Event handlers are called on the packet processing thread
- Use proper synchronization if accessing shared state from event handlers
- Zone collections (NPCs, Players, Doors) use `ConcurrentDictionary` for thread safety

## Protocol Implementation Status

### Supported Features ✅
- Complete login sequence (Login → World → Zone)
- Character loading and state management
- NPC/Player spawn tracking
- Chat message handling  
- Zone transitions
- World server opcodes: GuildsList, LogServer, ApproveWorld, EnterWorld, PostEnterWorld, ExpansionInfo, WorldComplete
- Movement commands
- Basic game state queries

### Future Enhancements 🚧
- Combat system integration
- Inventory management
- Spell casting
- Trading/merchant interactions
- Group/raid management
- Advanced movement (pathfinding)
- Tell responses
- Item inspection

## Migration from Direct Protocol Usage

### Before (Direct Protocol - 380+ lines)
```csharp
// Complex setup with multiple streams
_loginStream = new LoginStream(server, port);
_worldStream = new WorldStream(worldIP, 9000, accountId, sessionKey);
_zoneStream = new ZoneStream(zoneHost, zonePort, characterName);

// Manual event handler setup for each stream
_loginStream.LoginSuccess += OnLoginSuccess;
_loginStream.ServerList += OnServerListReceived; 
_loginStream.PlaySuccess += OnPlaySuccess;
_worldStream.CharacterList += OnCharacterListReceived;
_worldStream.ZoneServer += OnZoneServerReceived;
// ... dozens more event handlers

// Manual login sequence coordination
await ConnectToLoginServerAsync();
// Wait for login success
await ConnectToWorldServerAsync();  
// Wait for character list
await ConnectToZoneServerAsync();
// Finally ready!
```

### After (EQGameClient - 60 lines)
```csharp
// Simple setup
var gameClient = new EQGameClient(logger);
gameClient.LoginServer = "172.29.179.249";

// Single high-level event handlers
gameClient.CharacterLoaded += OnCharacterLoaded;
gameClient.NPCSpawned += OnNPCSpawned;

// One method call for entire login sequence!
var success = await gameClient.LoginAsync(username, password, worldName, characterName);
// Bot is now in-game and ready!
```

## Troubleshooting

### Common Issues

1. **"No EQServer configuration found"**
   - Ensure `appsettings.json` exists and contains EQServer section
   - Verify file is copied to output directory (should be automatic)

2. **Login timeouts**
   - Check server connectivity
   - Verify username/password
   - Ensure world server name matches exactly (case-sensitive)

3. **Character not found**
   - Verify character name matches exactly (case-sensitive)  
   - Ensure character exists on the specified world server

4. **Zone connection failures**
   - Usually indicates authentication issues
   - Check server logs for more details
   - May need to wait longer between connection attempts

### Debug Logging
Enable debug logging to see detailed packet information:

```json
{
  "Logging": {
    "LogLevel": {
      "OpenEQ.Netcode": "Debug"
    }
  }
}
```

This will show all packet exchanges and help diagnose connection issues.

---

**Generated on:** September 1, 2025  
**EQGameClient Version:** 1.0  
**Compatible with:** EverQuest Underfoot (UF) Protocol  
**Dependencies:** eqprotocol, zlib, Microsoft.Extensions.*