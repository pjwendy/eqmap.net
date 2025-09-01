# EverQuest Bot Opcode Mapping Strategy

## Overview

Opcodes are the fundamental communication mechanism between EverQuest clients and servers. Each packet sent has an opcode that identifies what type of data it contains (login request, movement update, chat message, etc.). Since EverQuest has evolved through many expansions, these opcodes have changed over time, requiring different mappings for different client versions.

---

## Current Implementation

### Static Opcode Mapping (Current)

Our bot currently uses **hardcoded Underfoot (UF) opcodes** defined in `C:\Users\stecoc\git\eqmap.net\eqprotocol\Opcodes.cs`:

```csharp
// All opcodes listed below are for the Underfoot client
public enum SessionOp : ushort {
    Request = 0x0001,
    Response = 0x0002,
    Disconnect = 0x0005,
    // ... etc
}

public enum WorldOp : ushort {
    SendLoginInfo = 0x13da,
    ApproveWorld = 0x86c7,
    EnterWorld = 0x51b9,
    // ... etc
}
```

**Advantages:**
- Simple and performant (no runtime loading)
- Type-safe with compile-time checking
- No configuration errors
- Clear and readable code

**Disadvantages:**
- Fixed to one client version
- Requires recompilation to change versions
- Cannot adapt to different servers dynamically

---

## Understanding Opcode Flow

### 1. Connection Establishment
```
Bot → Server: SessionOp.Request (0x0001)
Server → Bot: SessionOp.Response (0x0002)
Bot → Server: LoginOp.Login (0x0002)
Server → Bot: LoginOp.LoginAccepted (0x0018)
```

### 2. World Server Connection
```
Bot → World: WorldOp.SendLoginInfo (0x13da)
World → Bot: WorldOp.ApproveWorld (0x86c7)
Bot → World: WorldOp.SendCharInfo (0x4200)
World → Bot: WorldOp.MessageOfTheDay (0x7629)
```

### 3. Zone Entry
```
Bot → Zone: ZoneOp.ZoneEntry (0x4b61)
Zone → Bot: ZoneOp.PlayerProfile (0x6022)
Zone → Bot: ZoneOp.ZoneSpawns (0x7114)
```

---

## Opcode Mapping Files

The server stores opcode mappings in configuration files:

### File Structure
```
C:\Users\stecoc\git\server\utils\patches\
├── patch_Titanium.conf (545 opcodes)
├── patch_SoF.conf      (549 opcodes)
├── patch_SoD.conf      (592 opcodes)
├── patch_UF.conf       (628 opcodes) ← Currently Used
├── patch_RoF.conf      (596 opcodes)
└── patch_RoF2.conf     (644 opcodes)
```

### Configuration Format
```conf
# From patch_UF.conf
OP_SendLoginInfo=0x13da
OP_ApproveWorld=0x86c7
OP_EnterWorld=0x51b9
OP_ZoneEntry=0x4b61
# ... hundreds more
```

---

## Proposed Dynamic Mapping System

### Architecture
```csharp
public interface IOpcodeMapper {
    ushort GetOpcode(string operationName, ClientVersion version);
    void LoadMappings(string configPath, ClientVersion version);
    bool IsVersionSupported(ClientVersion version);
}

public class DynamicOpcodeMapper : IOpcodeMapper {
    private Dictionary<ClientVersion, Dictionary<string, ushort>> _mappings;
    
    public void LoadMappings(string configPath, ClientVersion version) {
        var opcodes = new Dictionary<string, ushort>();
        foreach (var line in File.ReadAllLines(configPath)) {
            if (line.StartsWith("OP_")) {
                var parts = line.Split('=');
                var name = parts[0].Substring(3); // Remove "OP_"
                var value = Convert.ToUInt16(parts[1], 16);
                opcodes[name] = value;
            }
        }
        _mappings[version] = opcodes;
    }
}
```

### Usage Pattern
```csharp
public class AdaptiveBot {
    private IOpcodeMapper _mapper;
    private ClientVersion _version;
    
    public void Connect(ServerInfo server) {
        // Detect or configure client version
        _version = DetectOptimalVersion(server);
        
        // Load appropriate opcodes
        _mapper.LoadMappings($"patches/patch_{_version}.conf", _version);
        
        // Use dynamic opcodes
        var loginOpcode = _mapper.GetOpcode("Login", _version);
        SendPacket(loginOpcode, loginData);
    }
}
```

---

## Version Detection Strategy

### Manual Configuration (Simple)
```json
{
  "EQServer": {
    "ClientVersion": "UF",
    "LoginServer": "172.29.179.249",
    "CustomOpcodeFile": "custom_opcodes.conf"
  }
}
```

### Automatic Detection (Advanced)
```csharp
public ClientVersion DetectServerVersion(string serverAddress) {
    // Try connecting with each version's Session Request
    // Server will only respond correctly to matching version
    foreach (var version in Enum.GetValues<ClientVersion>()) {
        if (TryHandshake(serverAddress, version)) {
            return version;
        }
    }
    return ClientVersion.UF; // Default fallback
}
```

### Server Negotiation (Future)
```csharp
// Proposed protocol extension
Bot → Server: "SUPPORTED_VERSIONS=UF,RoF,RoF2"
Server → Bot: "USE_VERSION=UF"
```

---

## Implementation Phases

### Phase 1: Document Current UF Opcodes ✅
- Map all UF opcodes we use
- Document packet structures
- Create reference documentation

### Phase 2: Refactor for Flexibility (Future)
- Extract opcode constants to configuration
- Create opcode provider interface
- Maintain backward compatibility

### Phase 3: Multi-Version Support (Future)
- Implement dynamic opcode loading
- Add version detection logic
- Test with multiple server types

### Phase 4: Optimization (Future)
- Cache loaded opcodes
- Precompile common versions
- Runtime JIT for performance

---

## Debugging Opcode Issues

### Common Problems and Solutions

1. **Wrong Opcode Value**
   - Symptom: Server doesn't respond or sends error
   - Debug: Compare with server's patch file
   - Fix: Update opcode value to match server

2. **Version Mismatch**
   - Symptom: All packets fail after session
   - Debug: Check server's expected client version
   - Fix: Use correct patch file for server

3. **Packet Structure Changes**
   - Symptom: Opcodes correct but data corrupted
   - Debug: Check structure definitions between versions
   - Fix: Adjust packet serialization for version

### Debugging Tools
```csharp
public class OpcodeDebugger {
    public void LogPacket(ushort opcode, byte[] data, Direction dir) {
        var opName = LookupOpcodeName(opcode);
        Console.WriteLine($"{dir}: {opName} (0x{opcode:X4}) - {data.Length} bytes");
        if (VerboseMode) {
            Console.WriteLine(BitConverter.ToString(data));
        }
    }
}
```

---

## Best Practices

### 1. Version Consistency
- Always ensure bot and server use same client version
- Document which version each server expects
- Test version compatibility before deployment

### 2. Opcode Documentation
- Comment opcodes with their purpose
- Link to server source for reference
- Document any custom modifications

### 3. Error Handling
```csharp
try {
    var opcode = GetOpcode("EnterWorld");
    SendPacket(opcode, data);
} catch (OpcodeNotFoundException ex) {
    Logger.Error($"Opcode not found for version {_version}: {ex.OpcodeName}");
    // Fallback or abort
}
```

### 4. Performance Considerations
- Cache opcode lookups (they don't change at runtime)
- Use static mappings for production bots
- Only use dynamic loading for development/testing

---

## Security Considerations

### Opcode Validation
- Never trust opcode values from external sources
- Validate opcodes are within expected ranges
- Sanitize configuration file inputs

### Version Spoofing
- Servers may detect version mismatches
- Don't claim capabilities not supported by version
- Match behavior to claimed client version

---

## Conclusion

While our current static UF opcode implementation is sufficient for initial development, implementing a dynamic opcode mapping system will provide the flexibility needed for:

1. Supporting multiple EQEmu servers with different configurations
2. Testing bots across different client versions
3. Adapting to server updates without recompilation
4. A/B testing protocol optimizations

The strategy outlined here provides a clear path from our current hardcoded implementation to a flexible, maintainable system that can support our bot ecosystem as it scales to hundreds of concurrent connections across diverse server environments.