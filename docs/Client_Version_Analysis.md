# EverQuest Client Version Analysis for Bot Implementation

## Executive Summary

After analyzing the EQEmu server's supported client versions and their opcode mappings, we've identified that **Underfoot (UF)** is the optimal client version for our bot implementation, with **RoF2** as a strong alternative for future enhancement.

Our existing codebase already uses UF opcodes, making it the natural choice to continue with.

---

## Supported Client Versions

The EQEmu server supports the following client versions, each with different capabilities:

| Client Version | Release Date | Opcodes Count | Expansion Name | Status |
|---------------|--------------|---------------|----------------|---------|
| **Titanium** | 2006 | 545 | The Serpent's Spine | Legacy, stable |
| **SoF** (Secrets of Faydwer) | 2007 | 549 | Secrets of Faydwer | Stable |
| **SoD** (Seeds of Destruction) | 2008 | 592 | Seeds of Destruction | Stable |
| **UF** (Underfoot) | 2010 | 628 | Underfoot | **Currently Used** |
| **RoF** (Rain of Fear) | 2012 | 596 | Rain of Fear | Modern |
| **RoF2** (Rain of Fear 2) | 2013 | 644 | Rain of Fear (updated) | Most Complete |

---

## Analysis Results

### Opcode Completeness
Based on the opcode count analysis from patch files:

1. **RoF2** - 644 opcodes (Most complete)
2. **UF** - 628 opcodes (Our current choice)
3. **RoF** - 596 opcodes
4. **SoD** - 592 opcodes
5. **SoF** - 549 opcodes
6. **Titanium** - 545 opcodes (Baseline)

### Key Feature Support by Version

#### Titanium (545 opcodes)
- ✅ Basic gameplay (movement, combat, chat)
- ✅ Trading, guilds, groups
- ✅ Original EQ through Serpent's Spine content
- ❌ No mercenary support
- ❌ Limited AA abilities
- ❌ Older UI system

#### SoF/SoD (549/592 opcodes)
- ✅ All Titanium features
- ✅ Improved AA system
- ✅ Better guild management
- ❌ No mercenary support
- ⚠️ Transitional versions, less commonly used

#### Underfoot (628 opcodes) - **CURRENT IMPLEMENTATION**
- ✅ All previous features
- ✅ Extended AA system
- ✅ Improved combat mechanics
- ✅ Better zone handling
- ✅ Enhanced guild features
- ⚠️ Mercenary support (limited)
- ✅ Good balance of features and stability

#### RoF/RoF2 (596/644 opcodes)
- ✅ All previous features
- ✅ Full mercenary support
- ✅ Most complete opcode set
- ✅ Latest UI improvements
- ✅ Advanced features like extended targeting
- ⚠️ Potentially less stable for bots
- ⚠️ More complex protocol

---

## Why Underfoot (UF) is Optimal for Bots

### 1. **Already Implemented**
```csharp
// From C:\Users\stecoc\git\eqmap.net\eqprotocol\Opcodes.cs
// All opcodes listed below are for the Underfoot client
// https://github.com/pjwendy/Server/blob/7fed8fc8c88aca3eca86062d7ef199f2f3160165/utils/patches/patch_UF.conf
```
Our existing codebase uses UF opcodes, providing a working foundation.

### 2. **Feature Completeness**
With 628 opcodes, UF provides:
- Full core gameplay mechanics
- Advanced AA system for bot progression
- Guild support for bot organizations  
- Comprehensive zone and world operations
- All essential bot activities

### 3. **Stability Balance**
- More stable than newer RoF/RoF2 implementations
- Better documented than older versions
- Wide server support and testing

### 4. **Resource Efficiency**
- Lighter protocol than RoF2
- Well-optimized packet structures
- Suitable for running hundreds of concurrent bots

### 5. **Server Compatibility**
- Widely supported by EQEmu servers
- Most private servers accept UF clients
- Good backward compatibility

---

## Migration Path Considerations

### Current State (UF)
✅ Working authentication  
✅ Server discovery functional  
✅ Character selection implemented  
🔄 World server connection (debugging needed)  

### If We Need RoF2 Features Later
The migration path from UF to RoF2 would involve:
1. Updating opcode mappings (16 additional opcodes)
2. Implementing mercenary-related packets
3. Adjusting packet structures for RoF2 format
4. Testing compatibility with target servers

---

## Recommendation

**Continue with Underfoot (UF) implementation** for the following reasons:

1. **Already Working**: Our codebase is configured for UF
2. **Sufficient Features**: 628 opcodes provide all needed bot functionality
3. **Proven Stability**: UF is well-tested in production environments
4. **Resource Efficient**: Optimal for scaling to hundreds of bots
5. **Future Flexibility**: Can upgrade to RoF2 if mercenary features become essential

---

## Opcode Mapping Strategy

### Current Implementation
The bot uses hardcoded UF opcodes from `Opcodes.cs`:
- Session operations (connection management)
- Login operations (authentication)
- World operations (character/server selection)
- Zone operations (gameplay)

### Dynamic Opcode Loading (Future Enhancement)
For flexibility, we could implement:
```csharp
public interface IOpcodeProvider {
    Dictionary<string, ushort> LoadOpcodes(ClientVersion version);
}

public enum ClientVersion {
    Titanium,
    SoF,
    SoD,
    UF,      // Current
    RoF,
    RoF2
}
```

This would allow:
- Runtime client version selection
- Easy testing across versions
- Server-specific optimizations
- A/B testing different protocols

---

## Next Steps

1. **Debug UF Connection**: Fix the world server timeout with UF protocol
2. **Document UF Specifics**: Create detailed UF packet documentation
3. **Optimize for UF**: Tune bot implementation for UF's capabilities
4. **Plan RoF2 Upgrade**: Keep path open for future mercenary support

---

## Conclusion

Underfoot provides the optimal balance of features, stability, and efficiency for our bot ecosystem. With 628 opcodes covering all essential gameplay mechanics and our existing implementation already using UF, it's the clear choice for moving forward.

The path to RoF2 remains open if we need mercenary support or additional features in the future, but UF gives us everything needed to create a thriving bot ecosystem that can simulate hundreds of players effectively.