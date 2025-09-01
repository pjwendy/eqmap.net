# EverQuest Packet Structure Documentation

## Overview
This document details the packet structures used in the EverQuest protocol, specifically for the Underfoot (UF) client version. These findings are based on analysis of the EQEmu server code and packet traces.

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