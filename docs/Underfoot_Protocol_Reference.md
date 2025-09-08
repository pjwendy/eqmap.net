# EverQuest Underfoot (UF) Protocol Reference

## Table of Contents
1. [Overview](#overview)
2. [Opcode Categories](#opcode-categories)
3. [Critical Connection Flow](#critical-connection-flow)
4. [Packet Structure Definitions](#packet-structure-definitions)
5. [Complete Opcode Reference](#complete-opcode-reference)
6. [Implementation Notes](#implementation-notes)

---

## Overview

This document provides a comprehensive reference for the EverQuest Underfoot (UF) client protocol, essential for implementing bot connections to EQEmu servers. The UF client uses 628 unique opcodes for client-server communication.

### Key Files
- **Opcode Definitions**: `C:\Users\stecoc\git\server\utils\patches\patch_UF.conf`
- **Packet Structures**: `C:\Users\stecoc\git\server\common\patches\uf_structs.h`
- **Encoding/Decoding**: `C:\Users\stecoc\git\server\common\patches\uf.cpp`
- **Our Implementation**: `C:\Users\stecoc\git\eqmap.net\eqprotocol\Opcodes.cs`

---

## Opcode Categories

### Session Management (0x0001-0x0015)
Core protocol opcodes for managing the EQStream session layer.

| Opcode | Value | Purpose | Direction |
|--------|-------|---------|-----------|
| SessionOp.Request | 0x0001 | Initiate session | Client → Server |
| SessionOp.Response | 0x0002 | Session acknowledgment | Server → Client |
| SessionOp.Disconnect | 0x0005 | Terminate session | Bidirectional |
| SessionOp.KeepAlive | 0x0006 | Keep connection alive | Bidirectional |
| SessionOp.Stats | 0x0007 | Connection statistics | Server → Client |
| SessionOp.Ack | 0x0015 | Acknowledge packets | Bidirectional |
| SessionOp.OutOfOrder | 0x0011 | Out of order packet | Server → Client |
| SessionOp.Single | 0x0009 | Single packet | Bidirectional |
| SessionOp.Fragment | 0x000d | Fragmented packet | Bidirectional |
| SessionOp.Combined | 0x0003 | Combined packets | Bidirectional |

### Login Server Operations
Initial authentication and server discovery.

| Opcode | Value | Purpose | Direction |
|--------|-------|---------|-----------|
| LoginOp.SessionReady | 0x0001 | Session established | Server → Client |
| LoginOp.Login | 0x0002 | Send credentials | Client → Server |
| LoginOp.ServerListRequest | 0x0004 | Request server list | Client → Server |
| LoginOp.PlayEverquestRequest | 0x000d | Select world server | Client → Server |
| LoginOp.LoginAccepted | 0x0018 | Login successful | Server → Client |
| LoginOp.ServerListResponse | 0x0019 | Available servers | Server → Client |

### World Server Connection
Character selection and world entry.

| Opcode | Value | Purpose | Direction |
|--------|-------|---------|-----------|
| OP_SendLoginInfo | 0x13da | Send login credentials | Client → World |
| OP_ApproveWorld | 0x86c7 | World approval | World → Client |
| OP_SendCharInfo | 0x4200 | Character list | World → Client |
| OP_EnterWorld | 0x51b9 | Enter with character | Client → World |
| OP_PostEnterWorld | 0x5d32 | World entry complete | World → Client |
| OP_ExpansionInfo | 0x7e4d | Expansion flags | World → Client |
| OP_GuildsList | 0x5b0b | Guild list | World → Client |
| OP_MOTD | 0x7629 | Message of the day | World → Client |
| OP_SetChatServer | 0x7d90 | Chat server info | World → Client |
| OP_ZoneServerInfo | 0x1190 | Zone server details | World → Client |
| OP_WorldComplete | 0x441c | World loading done | World → Client |

### Zone Server Operations
Gameplay within zones.

| Opcode | Value | Purpose | Direction |
|--------|-------|---------|-----------|
| OP_ZoneEntry | 0x4b61 | Enter zone | Client → Zone |
| OP_PlayerProfile | 0x6022 | Character data | Zone → Client |
| OP_ZoneSpawns | 0x7114 | All zone spawns | Zone → Client |
| OP_TimeOfDay | 0x6015 | Game time | Zone → Client |
| OP_Weather | 0x4658 | Weather state | Zone → Client |
| OP_ClientUpdate | 0x7062 | Position update | Client → Zone |
| OP_SpawnAppearance | 0x3e17 | Spawn appearance | Zone → Client |
| OP_NewSpawn | 0x429b | New entity spawn | Zone → Client |
| OP_DeleteSpawn | 0x58c5 | Remove entity | Zone → Client |

---

## Critical Connection Flow

### Phase 1: Login Server Authentication
```
1. [Session Establishment]
   Client → Login: SessionOp.Request (0x0001)
   Login → Client: SessionOp.Response (0x0002)
   
2. [Authentication]
   Client → Login: LoginOp.Login (0x0002)
   Packet: Username + Password
   
   Login → Client: LoginOp.LoginAccepted (0x0018)
   Returns: AccountID, SessionKey
   
3. [Server Discovery]
   Client → Login: LoginOp.ServerListRequest (0x0004)
   Login → Client: LoginOp.ServerListResponse (0x0019)
   Returns: List of available world servers
   
4. [World Selection]
   Client → Login: LoginOp.PlayEverquestRequest (0x000d)
   Packet: Selected ServerID
   
   Login → Client: LoginOp.PlayEverquestResponse (0x0022)
   Returns: World server connection info
```

### Phase 2: World Server Connection
```
1. [World Authentication]
   Client → World: OP_SendLoginInfo (0x13da)
   Packet: LoginInfo_Struct (488 bytes)
   
   World → Client: OP_ApproveWorld (0x86c7)
   Response: 544-byte approval packet
   
2. [Character Selection]
   World → Client: OP_SendCharInfo (0x4200)
   Returns: List of characters
   
   World → Client: OP_ExpansionInfo (0x7e4d)
   World → Client: OP_GuildsList (0x5b0b)
   World → Client: OP_MOTD (0x7629)
   
3. [World Entry]
   Client → World: OP_EnterWorld (0x51b9)
   Packet: EnterWorld_Struct with character name
   
   World → Client: OP_PostEnterWorld (0x5d32)
   World → Client: OP_ZoneServerInfo (0x1190)
   Returns: Zone server IP, port, and zone ID
```

### Phase 3: Zone Server Connection
```
1. [Zone Entry]
   Client → Zone: OP_ZoneEntry (0x4b61)
   Packet: ClientZoneEntry_Struct
   
2. [Character Setup]
   Zone → Client: OP_PlayerProfile (0x6022)
   Packet: PlayerProfile_Struct (8KB+)
   
   Zone → Client: OP_ZoneSpawns (0x7114)
   Zone → Client: OP_TimeOfDay (0x6015)
   Zone → Client: OP_Weather (0x4658)
   
3. [Ready State]
   Client → Zone: OP_ClientReady (0x6cdc)
   
4. [Active Play]
   Client ↔ Zone: OP_ClientUpdate (0x7062) - Position updates
   Zone → Client: OP_MobUpdate (0x4656) - NPC positions
   Client ↔ Zone: Various gameplay packets
```

---

## Packet Structure Definitions

### LoginInfo_Struct (OP_SendLoginInfo - 0x13da)
```cpp
Size: 488 bytes
struct LoginInfo_Struct {
    /*000*/ char login_info[64];     // "username\0password"
    /*064*/ uint8 unknown064[124];   // Padding/flags
    /*188*/ uint8 zoning;            // 01 if zoning, 00 if not
    /*189*/ uint8 unknown189[275];   // Additional data
    /*464*/ uint32 unknown464;       // Usually 0
    /*468*/ uint8 unknown468[20];    // Final padding
    /*488*/
};
```

### EnterWorld_Struct (OP_EnterWorld - 0x51b9)
```cpp
Size: 72 bytes
struct EnterWorld_Struct {
    /*000*/ char name[64];           // Character name (null-terminated)
    /*064*/ uint32 tutorial;         // 01 for tutorial, 00 normal
    /*068*/ uint32 return_home;      // 01 for return home, 00 normal
    /*072*/
};
```

### ClientZoneEntry_Struct (OP_ZoneEntry - 0x4b61)
```cpp
Size: 76 bytes
struct ClientZoneEntry_Struct {
    /*000*/ uint32 unknown0000;      // Usually 0
    /*004*/ char char_name[64];      // Character name
    /*068*/ uint32 unknown0068;      // Zone-specific data
    /*072*/ uint32 unknown0072;      // Additional flags
    /*076*/
};
```

### PlayerPositionUpdateClient_Struct (OP_ClientUpdate - 0x7062)
```cpp
Size: 40 bytes (Client to Server)
struct PlayerPositionUpdateClient_Struct {
    /*0000*/ uint16 spawn_id;        // Entity ID
    /*0002*/ uint16 sequence;        // Packet sequence number
    /*0004*/ uint8 unknown0004[4];   // Padding
    /*0008*/ float x_pos;            // X coordinate
    /*0012*/ float y_pos;            // Y coordinate
    /*0016*/ uint32 packed_data1;    // Bitfield: delta_heading, animation, padding
    /*0020*/ float delta_x;          // X velocity
    /*0024*/ float delta_y;          // Y velocity
    /*0028*/ float z_pos;            // Z coordinate
    /*0032*/ float delta_z;          // Z velocity
    /*0036*/ uint32 packed_data2;    // Bitfield: animation, heading, padding
    /*0040*/
};
```

### PlayerProfile_Struct (OP_PlayerProfile - 0x6022)
```cpp
Size: 26584 bytes (truncated for brevity)
struct PlayerProfile_Struct {
    /*00000*/ uint32 checksum;
    /*00004*/ uint32 gender;         // 0=Male, 1=Female
    /*00008*/ uint32 race;           // Race ID
    /*00012*/ uint32 class_;         // Class ID
    /*00056*/ uint8 level;           // Character level
    /*00057*/ uint8 level1;          // Level again (verification)
    /*00060*/ BindStruct binds[5];   // Bind point locations
    /*00160*/ uint32 deity;          // Deity ID
    /*00164*/ uint32 intoxication;   // Drunk level
    /*00168*/ uint32 spellSlotRefresh[13]; // Spell refresh timers
    /*00220*/ uint8 unknown00220[6];
    /*00226*/ uint8 haircolor;       // Hair color
    /*00227*/ uint8 beardcolor;      // Beard color
    /*00228*/ uint8 eyecolor1;       // Left eye
    /*00229*/ uint8 eyecolor2;       // Right eye
    /*00230*/ uint8 hairstyle;       // Hair style
    /*00231*/ uint8 beard;           // Beard style
    /*00232*/ uint8 unknown00232[4];
    /*00236*/ TextureProfile equipment; // Equipment appearance
    /*00512*/ TintProfile item_tint;    // Equipment dye colors
    /*00716*/ AA_Array aa_array[MAX_PP_AA_ARRAY]; // AA abilities
    /*04148*/ uint32 points;         // Unspent skill points
    /*04152*/ uint32 mana;           // Current mana
    /*04156*/ uint32 cur_hp;         // Current HP
    /*04160*/ uint32 STR;            // Strength
    /*04164*/ uint32 STA;            // Stamina
    /*04168*/ uint32 CHA;            // Charisma
    /*04172*/ uint32 DEX;            // Dexterity
    /*04176*/ uint32 INT;            // Intelligence
    /*04180*/ uint32 AGI;            // Agility
    /*04184*/ uint32 WIS;            // Wisdom
    // ... continues with skills, inventory, buffs, etc.
    /*26584*/
};
```

---

## Complete Opcode Reference

### World Server Opcodes (Required for Character Select)

| Opcode Name | Hex Value | Status | Purpose |
|-------------|-----------|---------|---------|
| OP_SendLoginInfo | 0x13da | Verified | Send login credentials to world |
| OP_ApproveWorld | 0x86c7 | Correct | World server approval response |
| OP_LogServer | 0x6f79 | Correct | Logging server info |
| OP_SendCharInfo | 0x4200 | Correct | Character list for selection |
| OP_ExpansionInfo | 0x7e4d | Correct | Expansion flags/features |
| OP_GuildsList | 0x5b0b | Correct | Guild list data |
| OP_EnterWorld | 0x51b9 | Correct | Enter world with character |
| OP_PostEnterWorld | 0x5d32 | Correct | Post-enter setup |
| OP_World_Client_CRC1 | 0x3a18 | Correct | Client file CRC check 1 |
| OP_World_Client_CRC2 | 0x3e50 | Correct | Client file CRC check 2 |
| OP_SendSpellChecksum | 0x46d3 | Correct | Spell file checksum |
| OP_SendSkillCapsChecksum | 0x040b | Correct | Skill caps checksum |

### Character Management

| Opcode Name | Hex Value | Status | Purpose |
|-------------|-----------|---------|---------|
| OP_DeleteCharacter | 0x5ca5 | Correct | Delete character request |
| OP_CharacterCreateRequest | 0x53a3 | Correct | Create character request |
| OP_CharacterCreate | 0x1b85 | Correct | Character creation response |
| OP_RandomNameGenerator | 0x647a | Correct | Random name generation |
| OP_ApproveName | 0x4f1f | Correct | Name approval check |

### Zone Entry Opcodes

| Opcode Name | Hex Value | Status | Purpose |
|-------------|-----------|---------|---------|
| OP_AckPacket | 0x3594 | Verified | Acknowledge packet receipt |
| OP_ZoneEntry | 0x4b61 | Verified | Zone entry request |
| OP_ReqNewZone | 0x4118 | Verified | Request new zone |
| OP_NewZone | 0x43ac | Verified | New zone info |
| OP_ZoneSpawns | 0x7114 | Verified | All spawns in zoOP_HPUpdatene |
| OP_PlayerProfile | 0x6022 | Verified | Complete character data |
| OP_TimeOfDay | 0x6015 | Verified | Game time |
| OP_LevelUpdate | 0x6a99 | Verified | Level change |
| OP_Stamina | 0x3d86 | Verified | Stamina update |
| OP_RequestClientZoneChange | 0x18ea | Correct | Zone change request |
| OP_ZoneChange | 0x6d37 | Correct | Zone change notification |

### Character State Updates

| Opcode Name | Hex Value | Status | Purpose |
|-------------|-----------|---------|---------|
| OP_SpawnAppearance | 0x3e17 | Verified | Spawn appearance change |
| OP_ChangeSize | 0x6942 | Unknown | Size change |
| OP_TributeUpdate | 0x684c | Verified | Tribute system update |
| OP_TributeTimer | 0x4895 | Correct | Tribute timer |
| OP_Weather | 0x4658 | Verified | Weather update |
| OP_ClientUpdate | 0x7062 | Verified | Client position update |
| OP_ClientReady | 0x6cdc | Verified | Client ready signal |

### Health/Mana/Stats

| Opcode Name | Hex Value | Status | Purpose |
|-------------|-----------|---------|---------|
| OP_HPUpdate | 0x6145 | Verified | HP update |
| OP_ManaChange | 0x569a | Correct | Mana change |
| OP_ManaUpdate | 0x0433 | Correct | Mana update |
| OP_EnduranceUpdate | 0x6b76 | Correct | Endurance update |
| OP_ExpUpdate | 0x0555 | Verified | Experience update |
| OP_AAExpUpdate | 0x4aa2 | Verified | AA experience update |

### Movement and Positioning

| Opcode Name | Hex Value | Status | Purpose |
|-------------|-----------|---------|---------|
| OP_MobUpdate | 0x4656 | Correct | NPC position update |
| OP_SpawnPositionUpdate | 0x4656 | Correct | Spawn position update |
| OP_NPCMoveUpdate | 0x0f3e | Unknown | NPC movement |
| OP_Jump | 0x083b | Correct | Jump action |
| OP_SetRunMode | 0x3d06 | Correct | Walk/run toggle |

### Combat Operations

| Opcode Name | Hex Value | Status | Purpose |
|-------------|-----------|---------|---------|
| OP_AutoAttack | 0x1df9 | Correct | Auto attack toggle |
| OP_AutoAttack2 | 0x517b | Correct | Secondary auto attack |
| OP_Damage | 0x631a | Correct | Damage packet |
| OP_Death | 0x7f9e | Correct | Death notification |
| OP_Consider | 0x3c2d | Correct | Consider target |
| OP_Assist | 0x35b1 | Correct | Assist target |
| OP_Taunt | 0x30e2 | Correct | Taunt ability |
| OP_CombatAbility | 0x36f8 | Correct | Combat ability use |

### Spell System

| Opcode Name | Hex Value | Status | Purpose |
|-------------|-----------|---------|---------|
| OP_CastSpell | 0x50c2 | Correct | Cast spell |
| OP_BeginCast | 0x0d5a | Correct | Begin casting |
| OP_MemorizeSpell | 0x3887 | Correct | Memorize spell |
| OP_SwapSpell | 0x5805 | Correct | Swap spell slots |
| OP_DeleteSpell | 0x0698 | Correct | Delete memorized spell |
| OP_InterruptCast | 0x7566 | Correct | Interrupt casting |
| OP_SpellEffect | 0x57a3 | Verified | Spell effect visual |

### Inventory and Items

| Opcode Name | Hex Value | Status | Purpose |
|-------------|-----------|---------|---------|
| OP_CharInventory | 0x47ae | Verified | Full inventory |
| OP_WearChange | 0x0400 | Verified | Equipment change |
| OP_MoveItem | 0x2641 | Correct | Move item |
| OP_DeleteItem | 0x66e0 | Correct | Delete item |
| OP_ItemPacket | 0x7b6e | Correct | Item data |
| OP_Consume | 0x24c5 | Verified | Consume item |

### Chat and Communication

| Opcode Name | Hex Value | Status | Purpose |
|-------------|-----------|---------|---------|
| OP_ChannelMessage | 0x2e79 | Correct | Channel message |
| OP_FormattedMessage | 0x3b52 | Correct | Formatted message |
| OP_SimpleMessage | 0x1f4d | Correct | Simple message |
| OP_SpecialMesg | 0x016c | Verified | Special message |
| OP_Emote | 0x3164 | Correct | Emote action |

### Guild Operations

| Opcode Name | Hex Value | Status | Purpose |
|-------------|-----------|---------|---------|
| OP_GuildMemberList | 0x51bc | Correct | Guild member list |
| OP_GuildMOTD | 0x5658 | Verified | Guild MOTD |
| OP_GuildInvite | 0x1a58 | Correct | Guild invite |
| OP_GuildRemove | 0x3c02 | Correct | Remove from guild |
| OP_GuildLeader | 0x0598 | Correct | Guild leader change |
| OP_GuildUpdate | 0x5232 | Correct | Guild update |

### Group Operations

| Opcode Name | Hex Value | Status | Purpose |
|-------------|-----------|---------|---------|
| OP_GroupInvite | 0x4f60 | Correct | Group invite |
| OP_GroupUpdate | 0x5331 | Correct | Group update |
| OP_GroupDisband | 0x54e8 | Correct | Disband group |
| OP_GroupFollow | 0x7f2b | Correct | Follow group member |
| OP_GroupLeaderChange | 0x0c33 | Correct | Group leader change |

### Trading

| Opcode Name | Hex Value | Status | Purpose |
|-------------|-----------|---------|---------|
| OP_TradeRequest | 0x7113 | Correct | Trade request |
| OP_TradeAcceptClick | 0x064a | Correct | Accept trade |
| OP_TradeCoins | 0x0149 | Correct | Trade money |
| OP_FinishTrade | 0x3ff6 | Correct | Complete trade |
| OP_CancelTrade | 0x527e | Correct | Cancel trade |

### Merchant/Shop

| Opcode Name | Hex Value | Status | Purpose |
|-------------|-----------|---------|---------|
| OP_ShopRequest | 0x442a | Correct | Open merchant |
| OP_ShopEnd | 0x3753 | Correct | Close merchant |
| OP_ShopPlayerBuy | 0x436a | Correct | Buy from merchant |
| OP_ShopPlayerSell | 0x0b27 | Correct | Sell to merchant |

### Doors and Objects

| Opcode Name | Hex Value | Status | Purpose |
|-------------|-----------|---------|---------|
| OP_ClickDoor | 0x6e97 | Correct | Click door |
| OP_MoveDoor | 0x3154 | Correct | Door movement |
| OP_SpawnDoor | 0x6f2b | Verified | Spawn door object |
| OP_ClickObject | 0x33e5 | Verified | Click world object |

---

## Implementation Notes

### Critical Implementation Requirements

1. **Packet Alignment**: All structures use `#pragma pack(1)` - no padding
2. **Endianness**: Little-endian byte order for all multi-byte values
3. **String Encoding**: ASCII null-terminated strings
4. **Sequence Numbers**: ClientUpdate packets must increment sequence
5. **Checksums**: PlayerProfile checksum field can be 0 for outbound

### Common Issues and Solutions

#### Issue: World Server Connection Timeout
**Symptoms**: Bot connects to world but times out after OP_SendLoginInfo
**Likely Causes**:
- Incorrect LoginInfo_Struct size (must be exactly 488 bytes)
- Missing or incorrect zoning flag (byte 188)
- Wrong opcode value for OP_SendLoginInfo

**Debug Steps**:
1. Verify packet size matches exactly 488 bytes
2. Check zoning flag is set to 0x00 for initial login
3. Confirm opcode 0x13da is being sent
4. Monitor server logs for authentication errors

#### Issue: Zone Entry Failure
**Symptoms**: Character selection works but zone entry fails
**Likely Causes**:
- Missing OP_ClientReady packet after zone entry
- Incorrect ClientZoneEntry_Struct format
- Sequence number not incrementing in ClientUpdate

**Debug Steps**:
1. Send OP_ClientReady (0x6cdc) after receiving OP_PlayerProfile
2. Verify ClientZoneEntry_Struct is 76 bytes
3. Implement sequence counter for ClientUpdate packets

### Performance Optimization

1. **Batch Packet Processing**: Combine multiple packets using SessionOp.Combined
2. **Delta Compression**: Use delta values in position updates
3. **Selective Updates**: Only send ClientUpdate when position changes
4. **Fragment Large Packets**: Use SessionOp.Fragment for packets > 512 bytes

### Security Considerations

1. **Input Validation**: Validate all incoming packet sizes
2. **Bounds Checking**: Check array indices in variable-length packets
3. **Authentication**: Never trust client-provided account IDs
4. **Rate Limiting**: Implement rate limits on position updates

---

## Debugging Checklist

When debugging connection issues, verify:

- [ ] Session establishment (Request/Response exchange)
- [ ] Login credentials format in LoginInfo_Struct
- [ ] Correct opcode values from patch_UF.conf
- [ ] Exact packet structure sizes
- [ ] Proper null-termination of strings
- [ ] Sequence number incrementation
- [ ] Zoning flag state (0x00 for login, 0x01 for zoning)
- [ ] ClientReady sent after zone entry
- [ ] All required zone setup packets received

---

## Next Steps for Bot Implementation

1. **Fix LoginInfo_Struct**: Ensure exact 488-byte structure
2. **Implement ApproveWorld Handler**: Process 544-byte response
3. **Add Character Selection**: Parse OP_SendCharInfo response
4. **Complete Zone Entry**: Implement full zone entry sequence
5. **Add Position Updates**: Implement ClientUpdate with sequence numbers
6. **Handle State Updates**: Process HP/Mana/Exp updates
7. **Implement Combat**: Add combat packet handlers
8. **Add Chat System**: Implement chat message handling

This reference provides the complete foundation needed to implement a fully functional UF client bot that can connect to EQEmu servers and participate in gameplay.