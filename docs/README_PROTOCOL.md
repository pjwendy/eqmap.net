# EverQuest Protocol Implementation

## Overview

This repository contains a C# implementation of the EverQuest network protocol, specifically targeting the Underfoot (UF) client version. The implementation is based on reverse engineering of the EQEmu server code and packet analysis.

## Project Structure

```
eqmap.net/
├── eqprotocol/         # Core protocol implementation
│   ├── EQStream.cs     # Base UDP stream handler
│   ├── LoginStream.cs  # Login server communication
│   ├── WorldStream.cs  # World server communication
│   ├── ZoneStream.cs   # Zone server communication
│   ├── Packet.cs       # Packet structures
│   └── LoginPackets.cs # Login-specific packet definitions
├── EQBot/              # Bot client implementation
│   └── Program.cs      # Main bot entry point
├── docs/               # Documentation
│   └── PACKET_STRUCTURES.md
└── DEVELOPMENT_LOG.md  # Development history and findings
```

## Key Components

### EQStream
Base class for all network communication. Handles:
- UDP socket management
- Session establishment
- Packet sequencing and acknowledgment
- Compression and CRC validation
- Packet fragmentation and reassembly

### Packet Types

#### Session Layer
- `SessionOp.Request` (0x0001): Initiate session
- `SessionOp.Response` (0x0002): Session acknowledgment
- `SessionOp.Single` (0x0009): Single packet
- `SessionOp.Fragment` (0x000D): Fragmented packet
- `SessionOp.Combined` (0x0003): Combined packets
- `SessionOp.Ack` (0x0015): Acknowledge receipt

#### Application Layer (Login)
- `LoginOp.SessionReady` (0x0001): Client ready
- `LoginOp.Login` (0x0002): Authentication request
- `LoginOp.ServerListRequest` (0x0004): Request server list
- `LoginOp.PlayEverquestRequest` (0x000D): Select server
- `LoginOp.ChatMessage` (0x0017): Server messages
- `LoginOp.LoginAccepted` (0x0018): Login success
- `LoginOp.ServerListResponse` (0x0019): Available servers
- `LoginOp.PlayEverquestResponse` (0x0022): Server selection result

## Protocol Flow

### 1. Login Server Connection
```
Client                    Login Server
  |                            |
  |---- SESSION_Request ------>|
  |<--- SESSION_Response ------|
  |---- OP_SessionReady ------>|
  |<--- OP_ChatMessage --------|
  |---- OP_Login ------------->|
  |<--- OP_LoginAccepted ------|
  |---- OP_ServerListRequest ->|
  |<--- OP_ServerListResponse -|
  |---- OP_PlayEverquestReq -->|
  |<--- OP_PlayEverquestResp --|
```

### 2. World Server Connection
```
Client                    World Server
  |                            |
  |---- SESSION_Request ------>|
  |<--- SESSION_Response ------|
  |---- OP_SendLoginInfo ----->|
  |<--- OP_GuildsList ---------|
  |<--- OP_LogServer ----------|
  |<--- OP_ApproveWorld -------|
  |<--- OP_SendCharInfo -------|
  |---- OP_EnterWorld -------->|
  |<--- OP_ZoneServerInfo -----|
```

### 3. Zone Server Connection
```
Client                    Zone Server
  |                            |
  |---- SESSION_Request ------>|
  |<--- SESSION_Response ------|
  |---- OP_SetDataRate ------->|
  |---- OP_ZoneEntry --------->|
  |<--- OP_PlayerProfile ------|
  |<--- OP_ZoneSpawns ---------|
```

## Configuration

### appsettings.json
```json
{
  "EQServer": {
    "LoginServer": "127.0.0.1",
    "LoginServerPort": 5999,
    "Username": "your_username",
    "Password": "your_password",
    "WorldName": "World Server Name",
    "CharacterName": "Character Name",
    "LogKeepalives": false
  }
}
```

### NLog.config
Logging configuration with automatic log rotation and cleanup.

## Building

```bash
# Build all projects
dotnet build

# Run the bot
cd EQBot
dotnet run
```

## Debugging Tips

1. **Enable packet logging**: Set `Debug = true` in stream classes
2. **Compare with server logs**: Use matching format for easy comparison
3. **Check packet sizes**: Mismatches indicate structure problems
4. **Verify byte order**: All values are little-endian
5. **Watch for padding**: Server uses `#pragma pack(1)`

## Known Issues

1. World server connection may timeout after OP_SendLoginInfo
2. Zone server handshake incomplete
3. Character movement not implemented
4. Combat system not implemented

## Critical Findings

### Structure Packing
The server uses `#pragma pack(1)` which eliminates padding between struct members. This is critical for correct packet structure.

### LoginBaseMessage
All login packets include a 10-byte header:
- 4 bytes: sequence
- 1 byte: compressed flag
- 1 byte: encryption type
- 4 bytes: unused

### Packet Size Reporting
Server logs report data size only (excluding opcode), while raw packets include the 2-byte opcode.

## References

- [EQEmu Server Source](https://github.com/EQEmu/Server)
- [Project 1999 Wiki](https://wiki.project1999.com/)
- [EQEmu Forums](https://www.eqemulator.org/forums/)

## License

This project is for educational purposes only. EverQuest is a registered trademark of Darkpaw Games.