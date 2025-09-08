# EverQuest Bot Ecosystem - Repository References

## Core Dependencies

Our bot ecosystem depends on several key repositories that provide the EverQuest server infrastructure and client implementation references. We maintain forked versions to ensure stability and protect against breaking changes.

---

## Client Implementation References

### OpenEQ - Original Open Source EverQuest Client
**URL**: https://github.com/daeken/OpenEQ  
**Author**: Serafina Brocious (@daeken)  
**Purpose**: Open source EverQuest client implementation in C#  
**Role in Project**: **This is the original project that our current implementation was based on**

**Key Components**:
- `NetClient/` - Network client implementation
- `Netcode/` - Core networking protocol (foundation for our eqprotocol)
- `Engine/` - Game engine components
- `CollisionManager/` - Collision detection system

**Technologies**:
- Primary Language: C# (94.1%)
- Secondary Language: Python (5.9%)
- Solution File: OpenEQ.sln

**Historical Significance**:
- Original source for our EverQuest protocol implementation
- Reference for packet structures and client behavior
- Foundation for understanding EQ networking in C#
- Many of our core protocol classes evolved from this codebase

---

## EQEmu Server Repositories

### Original EQEmu Server
**URL**: https://github.com/EQEmu/Server  
**Purpose**: The official open-source EverQuest server emulator  
**Role in Project**: Reference implementation for understanding server behavior and protocol

**Key Components**:
- Core server engine (world, zone, login servers)
- Database schemas and game data
- Network protocol implementations
- Game mechanics and rules engine

### Our Forked EQEmu Server (Stable)
**URL**: https://github.com/pjwendy/Server  
**Purpose**: Our stable fork for bot development  
**Role in Project**: Primary server codebase for bot testing and integration

**Fork Benefits**:
- Protected from breaking changes in upstream
- Ability to add bot-specific server modifications if needed
- Consistent testing environment
- Version control for our specific server configuration

**Important Directories for Bot Development**:
```
Server/
├── common/           # Shared protocol definitions and utilities
│   ├── net/         # Network protocol implementations
│   ├── patches/     # Client version-specific opcodes
│   └── structs/     # Packet structure definitions
├── world/           # World server (handles login, character select)
├── zone/            # Zone server (handles gameplay)
└── loginserver/     # Login server (authentication)
```

---

## AkkStack Docker Implementation

### Original AkkStack
**URL**: https://github.com/Akkadius/akk-stack  
**Purpose**: Complete dockerized EQEmu server environment  
**Role in Project**: Reference for containerized deployment

**Stack Components**:
- EQEmu server (world, zone, login servers)
- MariaDB database
- PEQ database content
- Web-based server management tools
- Automatic backup systems
- Development tools

### Our Forked AkkStack (Stable)
**URL**: https://github.com/pjwendy/akk-stack  
**Purpose**: Our stable fork for consistent bot testing environment  
**Role in Project**: Primary development and testing platform

**Fork Benefits**:
- Consistent development environment across team
- Pre-configured for bot testing scenarios
- Isolated from production server changes
- Easy deployment for scaling tests

**Docker Services Architecture**:
```yaml
services:
  eqemu-server:     # Main game server
  mariadb:          # Game database
  phpmyadmin:       # Database management
  peq-editor:       # Quest and content editing
  eqemu-maps:       # Map file serving
  backup:           # Automated backup service
```

---

## How These Repositories Work Together

```
┌─────────────────────────────────────────────────┐
│           Bot Ecosystem (Our Code)              │
│                                                 │
│  ┌──────────────┐      ┌──────────────┐        │
│  │   EQBot      │      │  Management  │        │
│  │   Client     │      │     API      │        │
│  └──────┬───────┘      └──────────────┘        │
│         │                                       │
└─────────┼───────────────────────────────────────┘
          │ EQEmu Protocol
┌─────────▼───────────────────────────────────────┐
│         AkkStack Docker Environment             │
│         (github.com/pjwendy/akk-stack)         │
│                                                 │
│  ┌────────────────────────────────────┐        │
│  │    EQEmu Server                    │        │
│  │ (github.com/pjwendy/Server)        │        │
│  │                                    │        │
│  │  • Login Server (Auth)             │        │
│  │  • World Server (Character/Zone)   │        │
│  │  • Zone Server (Gameplay)          │        │
│  └────────────────────────────────────┘        │
│                                                 │
│  ┌──────────────┐  ┌──────────────┐            │
│  │   MariaDB    │  │   PEQ DB     │            │
│  │  (Game Data) │  │  (Content)    │            │
│  └──────────────┘  └──────────────┘            │
└─────────────────────────────────────────────────┘
```

---

## Important Files for Bot Development

### From EQEmu Server Repository

**Protocol Definitions**:
- `common/net/packet_structs.h` - Core packet structures
- `common/net/eqstream.cpp` - EQStream protocol implementation
- `common/patches/*/opcodes.conf` - Opcode mappings per client version

**Login Flow**:
- `world/client.cpp` - World server client handling
- `world/clientlist.cpp` - Client connection management
- `loginserver/client.cpp` - Login authentication

**Game Mechanics**:
- `zone/client.cpp` - Main client game logic
- `zone/mob.cpp` - Mobile entity behaviors (NPCs, players)
- `zone/combat.cpp` - Combat system implementation

### From AkkStack Repository

**Configuration**:
- `docker-compose.yml` - Service definitions
- `.env` - Environment configuration
- `scripts/` - Utility scripts for server management

**Database**:
- `peq-database/` - Game content and configuration
- `backups/` - Database backup location

---

## Version Compatibility

### Current Versions (as of fork)
- **EQEmu Server**: Check commit hash in forked repo
- **AkkStack**: Check release tag in forked repo
- **Database Schema**: PEQ latest stable
- **Client Compatibility**: Titanium, RoF2, others

### Protocol Compatibility
Our bot client must match the server's expected protocol version:
- Opcode mappings must align with server patch files
- Packet structures must match server expectations
- Authentication flow must follow server implementation

---

## Development Workflow

### Local Development Setup
1. **Clone AkkStack Fork**: 
   ```bash
   git clone https://github.com/pjwendy/akk-stack.git
   cd akk-stack
   ```

2. **Configure Environment**:
   ```bash
   cp .env.example .env
   # Edit .env with your settings
   ```

3. **Start Services**:
   ```bash
   docker-compose up -d
   ```

4. **Verify Server**:
   - Login server: Port 5998/5999
   - World server: Port 9000
   - Database: Port 3306

### Testing Bot Connections
1. Point bot to local AkkStack instance
2. Use test accounts created in database
3. Monitor server logs for connection issues
4. Debug protocol mismatches using server source

### Contributing Back
While we work from forked repositories for stability, valuable improvements can be contributed back:
- Bug fixes that benefit the community
- Protocol documentation improvements
- Performance optimizations
- New features that don't break existing functionality

---

## Security Considerations

### Private Server Usage
- These repositories are for private server use only
- Not affiliated with official EverQuest
- Respect intellectual property rights
- Use for educational and development purposes

### Fork Security
- Keep forks private if containing sensitive modifications
- Regularly review upstream security patches
- Apply critical security fixes even if staying on stable fork
- Monitor for vulnerabilities in dependencies

---

## Maintenance Strategy

### Keeping Forks Updated
1. **Quarterly Review**: Check upstream for critical updates
2. **Selective Merging**: Only merge non-breaking changes
3. **Testing Protocol**: Full regression testing before updates
4. **Rollback Plan**: Tag stable versions before updates

### Documentation Sync
- Document any divergence from upstream
- Note custom modifications in fork
- Keep compatibility matrix updated
- Track tested client/server version combinations

---

*This document serves as the authoritative reference for understanding the server infrastructure our bot ecosystem depends upon.*