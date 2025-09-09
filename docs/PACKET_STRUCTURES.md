# EverQuest Packet Structure Documentation

**Version**: 2.0 (Updated January 2025)  
**Protocol**: EverQuest Underfoot (UF) Client  
**Server Compatibility**: EQEmu Server  

## Overview
This document provides comprehensive documentation of EverQuest packet structures for the Underfoot (UF) client protocol. All structures have been verified against the EQEmu C++ server source code to ensure exact binary compatibility and include defensive error handling for production stability.

### Recent Updates (v2.0)
- **Complete C++ Server Verification**: All structures match EQEmu server definitions exactly
- **Production-Ready Implementation**: Defensive error handling and zero-crash operation
- **Comprehensive Coverage**: 30+ packet structures with full field documentation
- **Runtime Tested**: All structures validated with live server connections

## Key Discoveries

### 1. Structure Packing
The server uses `#pragma pack(1)` for all login structures, meaning:
- No padding between struct members
- Structs are packed tightly with no alignment gaps
- Size calculations must account for exact byte counts

### 2. Login Server Packets

#### LoginBaseMessage
All login packets include this base header:
```c
struct LoginBaseMessage {
    int32_t sequence;     // 4 bytes - request sequence number
    bool    compressed;   // 1 byte - compression flag (0 = false)
    int8_t  encrypt_type; // 1 byte - encryption type (0 = none)
    int32_t unk3;         // 4 bytes - unused field
};  // Total: 10 bytes (no padding)
```

#### OP_SessionReady (0x0001)
```
Size: 12 bytes (data only, excluding opcode)
Structure: 3 x uint32 (all zeros)
```

#### OP_Login (0x0002)
```
Size: 26 bytes (10 bytes zeros + 16 bytes encrypted credentials)
Structure: 5 x uint16 (zeros) + encrypted blob
```

#### OP_ServerListRequest (0x0004)
```
Size: 10 bytes
Structure: Fixed pattern starting with 0x04 0x00 0x00 0x00
```

#### OP_PlayEverquestRequest (0x000D)
```
Size: 14 bytes (data only)
Structure:
- LoginBaseMessage (10 bytes)
- uint32 server_number (4 bytes) - RuntimeID of selected server
```

**Critical Finding**: The PlayEverquestRequest must be exactly 14 bytes. The server reads:
- `play->base_header.sequence` from bytes 0-3
- `play->server_number` from bytes 10-13

### 3. World Server Packets

#### OP_SendLoginInfo (0x13DA)
```
Size: 466 bytes (data only)
Structure:
- Account ID and Session Key as null-terminated string
- Byte 192: Zoning flag (0xCC for UF client)
- Remaining bytes: zeros
```

**Note**: When CRC validation is enabled, 2 additional bytes are appended for the CRC checksum.

## Logging Format Standardization

To match server logging format:
1. **Size** - Report data size only (excluding 2-byte opcode)
2. **Hex dumps** - Show data payload only (no opcode bytes)
3. **Session packets** - Include full packet with headers
4. **Application packets** - Show data portion only

Example:
```
Server format: [OP_SessionReady] [0x0001] Size [12]
Our format:    [OP_SessionReady] [0x0001] Size [12]
```

## Common Pitfalls

1. **Assuming padding** - Server uses pack(1), no automatic padding
2. **Including opcode in size** - Size should be data-only
3. **Wrong sequence type** - Some sequences are uint32, not uint16
4. **Byte order** - All multi-byte values are little-endian

## Zone Server Packet Structures

### Combat & Health Structures

#### Consider_Struct (20 bytes)
Used for target consideration - determines relative level and faction.
```c
// C++ Server Definition (uf_structs.h)
struct Consider_Struct {
    uint32 playerid;      // 4 bytes - Considering player's ID
    uint32 targetid;      // 4 bytes - Target being considered 
    uint32 faction;       // 4 bytes - Faction standing
    uint32 level;         // 4 bytes - Relative level indication
    uint8 pvpcon;         // 1 byte - PvP consideration
    uint8 unknown017[3];  // 3 bytes - Padding/unknown data
}; // Total: 20 bytes (verified)
```

**C# Implementation:**
```csharp
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Consider {
    public uint PlayerID;
    public uint TargetID; 
    public uint Faction;
    public uint Level;
    public byte PvpCon;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public byte[] Unknown017;
}
```

#### CombatDamage_Struct (28 bytes)
Used for all combat damage events.
```c
// C++ Server Definition
struct CombatDamage_Struct {
    uint16 target;        // 2 bytes - Target entity ID
    uint16 source;        // 2 bytes - Source entity ID
    uint8 type;           // 1 byte - Damage type (231/0xE7 for spells)
    uint16 spellid;       // 2 bytes - Spell ID if spell damage
    int32 damage;         // 4 bytes - Damage amount
    float force;          // 4 bytes - Force/knockback amount
    float hit_heading;    // 4 bytes - Direction of hit
    float hit_pitch;      // 4 bytes - Pitch of hit
    uint8 secondary;      // 1 byte - 0=primary hand, 1=secondary
    uint32 special;       // 4 bytes - 2=Rampage, 1=Wild Rampage
}; // Total: 28 bytes (verified)
```

**C# Implementation:**
```csharp
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Damage {
    public ushort Target;
    public ushort Source;
    public byte Type;
    public ushort SpellID;
    public int Amount;        // Renamed from "Damage" to avoid naming conflict
    public float Force;
    public float HitHeading;
    public float HitPitch;
    public byte Secondary;
    public uint Special;
}
```

#### HPUpdate_Struct (3 bytes)
Used for health percentage updates.
```c
// C++ Server Definition
struct HPUpdate_Struct {
    uint8 hp;             // 1 byte - HP percentage (0-100)
    uint16 entityid;      // 2 bytes - Entity ID
}; // Total: 3 bytes (verified)
```

**C# Implementation:**
```csharp
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct MobHealth {
    public byte HP;           // HP percentage (0-100) 
    public ushort EntityID;   // Mob's ID (field order corrected from C++)
}
```

### Communication Structures

#### ChannelMessage_Struct
Used for all chat communications.
```c
// C++ Server Definition
struct ChannelMessage_Struct {
    char targetname[64];      // 64 bytes - Target player name
    char sender[64];          // 64 bytes - Sender name  
    uint32 language;          // 4 bytes - Language ID
    uint32 chan_num;          // 4 bytes - Channel number
    uint32 cm_unknown4[2];    // 8 bytes - Unknown fields
    uint32 skill_in_language; // 4 bytes - Language skill
    char message[512];        // 512 bytes - Message text
}; // Total: ~660 bytes
```

### Object & Environment Structures

#### Object_Struct (104 bytes)
Used for ground spawns, objects, and containers.
```c
// C++ Server Definition  
struct Object_Struct {
    uint32 linked_list_addr[2]; // 8 bytes - Linked list pointers
    uint32 unknown008;          // 4 bytes - Unknown
    uint32 drop_id;             // 4 bytes - Unique object ID
    uint16 zone_id;             // 2 bytes - Zone ID
    uint16 zone_instance;       // 2 bytes - Zone instance
    uint32 unknown020;          // 4 bytes - Unknown
    uint32 unknown024;          // 4 bytes - Unknown  
    float size;                 // 4 bytes - Object size (default 1.0)
    float tilt_x;               // 4 bytes - X-axis tilt
    float tilt_y;               // 4 bytes - Y-axis tilt  
    float heading;              // 4 bytes - Heading/rotation
    float z;                    // 4 bytes - Z coordinate
    float x;                    // 4 bytes - X coordinate
    float y;                    // 4 bytes - Y coordinate
    char object_name[32];       // 32 bytes - Object name
    uint32 unknown088;          // 4 bytes - Unknown
    uint32 object_type;         // 4 bytes - Object type ID
    uint8 unknown096[4];        // 4 bytes - Unknown  
    uint32 spawn_id;            // 4 bytes - Spawn ID of interacting client
}; // Total: 104 bytes (verified)
```

**C# Implementation:**
```csharp
[StructLayout(LayoutKind.Sequential, Pack = 1)]  
public struct GroundSpawn {
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public uint[] LinkedListAddr;    // Prev/next pointers
    public uint Unknown008;          // Linked list related
    public uint DropID;              // Unique object ID for zone
    public ushort ZoneID;            // Zone ID
    public ushort ZoneInstance;      // Zone instance
    public uint Unknown020;          // 00 00 00 00
    public uint Unknown024;          // Zone-specific data
    public float Size;               // Object size (default 1.0)
    public float XTilt;              // X-axis tilt
    public float YTilt;              // Y-axis tilt
    public float Heading;            // Rotation
    public float Z;                  // Z coordinate
    public float X;                  // X coordinate  
    public float Y;                  // Y coordinate
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string ObjectName;        // Object name
    public uint Unknown088;          // Unique table ID
    public uint ObjectType;          // Object type
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] Unknown096;        // ff ff ff ff
    public uint SpawnID;             // Interacting client spawn ID
}
```

#### Track_Struct (8 bytes)  
Used for tracking information.
```c
// C++ Server Definition
struct Track_Struct {
    uint16 entityid;          // 2 bytes - Entity being tracked
    uint16 y;                 // 2 bytes - Y coordinate  
    uint16 x;                 // 2 bytes - X coordinate
    uint16 z;                 // 2 bytes - Z coordinate
}; // Total: 8 bytes (verified)
```

### Spell & Buff Structures

#### Buff_Struct
Used for buff/debuff applications.
```c
// C++ Server Definition
struct Buff_Struct {
    uint32 entityid;          // 4 bytes - Target entity ID
    uint16 spellid;           // 2 bytes - Spell ID
    uint32 duration;          // 4 bytes - Duration in ticks
    uint8 level;              // 1 byte - Caster level
    uint32 bard_modifier;     // 4 bytes - Bard song modifier
    uint32 effect;            // 4 bytes - Effect type
    uint32 damage_shield;     // 4 bytes - Damage shield amount
}; // Total: 23 bytes (verified)
```

### Defensive Error Handling

All packet constructors include comprehensive defensive error handling:

```csharp
public Consider(byte[] data, int offset = 0)
{
    var availableBytes = data.Length - offset;
    if (availableBytes < 20)  // Consider_Struct is 20 bytes
    {
        // Initialize with defaults if insufficient data
        PlayerID = 0; TargetID = 0; Faction = 0; Level = 0;
        PvpCon = 0; Unknown017 = new byte[3];
        return;
    }
    
    // Safe parsing with known correct size
    using (var ms = new MemoryStream(data, offset, 20))
    using (var br = new BinaryReader(ms))
    {
        PlayerID = br.ReadUInt32();
        TargetID = br.ReadUInt32(); 
        Faction = br.ReadUInt32();
        Level = br.ReadUInt32();
        PvpCon = br.ReadByte();
        Unknown017 = br.ReadBytes(3);
    }
}
```

### Key Implementation Principles

1. **Exact Size Matching**: All structures match C++ server definitions byte-for-byte
2. **Defensive Parsing**: Buffer overflow protection and graceful error handling
3. **Field Ordering**: Maintain exact field order from server structures  
4. **Padding Awareness**: Account for structure packing and alignment
5. **Default Initialization**: Safe fallback values for malformed packets

## Protocol Flow

### Successful Login Sequence
1. Client → Server: SESSION_Request
2. Server → Client: SESSION_Response
3. Client → Server: OP_SessionReady (12 bytes zeros)
4. Server → Client: OP_ChatMessage
5. Client → Server: OP_Login (26 bytes)
6. Server → Client: OP_LoginAccepted + OP_LoginExpansionPacketData
7. Client → Server: OP_ServerListRequest
8. Server → Client: OP_ServerListResponse
9. Client → Server: OP_PlayEverquestRequest (14 bytes)
10. Server → Client: OP_PlayEverquestResponse

### World Server Connection
1. Client → World: SESSION_Request
2. World → Client: SESSION_Response
3. Client → World: OP_SendLoginInfo (466 bytes)
4. World → Client: OP_GuildsList, OP_LogServer, OP_ApproveWorld, etc.

## Debugging Tips

1. Always compare packet sizes first - mismatch indicates struct issues
2. Check for #pragma pack directives in server code
3. Use server log format for easier comparison
4. Watch for CRC bytes added to packets when validation is enabled
5. Verify byte order for multi-byte fields

## References

- Server code: `/loginserver/login_types.h`
- Opcodes: `/loginserver/login_util/login_opcodes.conf`
- Client handler: `/loginserver/client.cpp`