# EverQuest Bot Ecosystem - Technical Architecture

## System Overview

The EverQuest Bot Ecosystem is designed as a distributed, microservices-based system that can scale from dozens to thousands of concurrent bots while providing comprehensive management and monitoring capabilities.

## Core Principles

### 1. **Separation of Concerns**
Each component has a single, well-defined responsibility:
- **Bot Runtime**: Pure game client functionality
- **Behavior Engine**: AI/scripted decision making
- **Management Layer**: Orchestration and configuration
- **Monitoring System**: Observability and analytics
- **User Interface**: Visualization and control

### 2. **Horizontal Scalability**
All components designed to scale horizontally:
- Multiple bot processes per machine
- Load-balanced management services
- Distributed data storage
- Elastic resource allocation

### 3. **Fault Tolerance**
System continues operating despite individual component failures:
- Bot auto-restart on crashes
- Graceful degradation of services
- Circuit breakers for external dependencies
- Comprehensive health checks

### 4. **Resource Optimization**
Efficient resource usage to maximize bot density:
- Minimal memory footprint per bot
- CPU optimization for concurrent operations
- Network connection pooling
- Smart caching strategies

## Component Architecture

```
┌─────────────────────────────────────────────────────────┐
│                  Web Dashboard                          │
│              (React/Vue Frontend)                       │
└─────────────────┬───────────────────────────────────────┘
                  │ HTTP/WebSocket
┌─────────────────▼───────────────────────────────────────┐
│                Management API                           │
│              (ASP.NET Core)                            │
├─────────────────┬───────────────────────────────────────┤
│     Bot Manager │ Analytics Engine │ Config Service    │
└─────────────────┬───────────────────────────────────────┘
                  │ Message Queue (Redis/RabbitMQ)
┌─────────────────▼───────────────────────────────────────┐
│                Bot Orchestrator                         │
│           (Manages Bot Lifecycle)                       │
└─────────────────┬───────────────────────────────────────┘
                  │ Process Management
┌─────────────────▼───────────────────────────────────────┐
│                Bot Runtime Layer                        │
├─────────────────┬─────────────────┬───────────────────┤
│   Bot Instance  │  Bot Instance   │  Bot Instance     │
│  ┌─────────────┐│ ┌─────────────┐ │ ┌─────────────┐   │
│  │ EQ Client   ││ │ EQ Client   │ │ │ EQ Client   │   │
│  │ Behavior AI ││ │ Behavior AI │ │ │ Behavior AI │   │
│  │ State Mgmt  ││ │ State Mgmt  │ │ │ State Mgmt  │   │
│  └─────────────┘│ └─────────────┘ │ └─────────────┘   │
└─────────────────┴─────────────────┴───────────────────┘
                  │ EQEmu Protocol
┌─────────────────▼───────────────────────────────────────┐
│              EverQuest Emulator Server                  │
└─────────────────────────────────────────────────────────┘
```

## Detailed Component Design

### Bot Runtime (Lightweight Process)

**Responsibilities**:
- EQEmu server connection and protocol handling
- Game state synchronization
- Basic event processing
- Behavior execution

**Design Goals**:
- <50MB RAM per bot instance
- <1% CPU utilization during idle
- Fast startup (<5 seconds)
- Graceful shutdown and restart

**Key Components**:
```csharp
class BotInstance
{
    LoginStream loginStream;           // Login server connection
    WorldStream worldStream;           // World server connection  
    ZoneStream zoneStream;            // Zone server connection
    BehaviorEngine behaviorEngine;    // Decision making
    GameState gameState;              // Current world state
    EventProcessor eventProcessor;    // Handle game events
    HealthMonitor healthMonitor;      // Self-monitoring
}
```

**Protocol Flow Implementation**:

#### 1. Login Server Connection
```csharp
// Successful implementation using SESSION protocol
LoginStream -> SESSION_Request
           <- SESSION_Response
           -> SessionReady + Login(username, password)
           <- LoginAccepted
           -> ServerListRequest
           <- ServerListResponse
           -> PlayEverquestRequest(serverId)
           <- PlayEverquestResponse(serverInfo)
```

#### 2. World Server Connection
```csharp
// Complete authentication chain
WorldStream -> SESSION_Request
           <- SESSION_Response  
           -> SendLoginInfo(accountId + sessionKey)
           <- ApproveWorld (544 bytes, fragmented)
           <- EnterWorld + PostEnterWorld
           -> ApproveWorld (empty)      // Critical response
           -> WorldClientReady          // Critical response
           <- SendCharInfo
           -> EnterWorld(characterName)
           <- ZoneServerInfo
```

#### 3. Zone Server Connection
```csharp
// Full initialization sequence
ZoneStream -> SESSION_Request
          <- SESSION_Response
          -> ZoneEntry(characterName)   // 68 bytes
          <- Multiple spawn packets
          <- PlayerProfile (26KB+, fragmented)
          <- Weather
          -> ReqNewZone
          <- NewZone (944 bytes)        // Zone initialization
          -> ReqClientSpawn
          <- SendExpZonein
          -> ClientReady
          // Character appears in UI!
```

### Behavior Engine (AI/Scripting)

**Responsibilities**:
- Decision making for bot actions
- Personality and behavior variation
- Learning and adaptation
- Script execution fallbacks

**AI Integration Points**:
```csharp
interface IBehaviorProvider
{
    BotAction DecideNextAction(GameState state, BotPersonality personality);
    void ProcessGameEvent(GameEvent gameEvent);
    void UpdateFromLearning(ExperienceData experience);
}

// Implementations:
class ScriptedBehavior : IBehaviorProvider    // Deterministic scripts
class AIBehavior : IBehaviorProvider          // ML/LLM integration  
class HybridBehavior : IBehaviorProvider      // AI with script fallbacks
```

### Management API (Orchestration Hub)

**Endpoints**:
```
POST   /api/bots                    # Create new bot
GET    /api/bots                    # List all bots
GET    /api/bots/{id}               # Get bot details
PUT    /api/bots/{id}               # Update bot config
DELETE /api/bots/{id}               # Remove bot
POST   /api/bots/{id}/actions       # Send bot command

GET    /api/zones                   # Zone information
GET    /api/zones/{id}/bots         # Bots in specific zone
GET    /api/analytics/performance   # System metrics
GET    /api/analytics/population    # Bot distribution
```

**Real-time Events**:
```
WebSocket /ws/bots/{id}             # Individual bot updates
WebSocket /ws/zones/{id}            # Zone activity feed  
WebSocket /ws/system                # Global status updates
```

### Data Storage Strategy

**Bot Configuration** (PostgreSQL):
```sql
CREATE TABLE bots (
    id UUID PRIMARY KEY,
    name VARCHAR(64) UNIQUE,
    character_class VARCHAR(32),
    level INTEGER,
    zone_id INTEGER,
    behavior_type VARCHAR(32),
    personality_config JSONB,
    status VARCHAR(16),
    created_at TIMESTAMP,
    last_seen TIMESTAMP
);
```

**Time-Series Analytics** (InfluxDB):
```
measurement: bot_metrics
tags: bot_id, zone, activity_type
fields: cpu_percent, memory_mb, actions_per_minute, ping_ms
time: timestamp

measurement: zone_population  
tags: zone_id, zone_name
fields: bot_count, activity_level
time: timestamp
```

**Real-time State** (Redis):
```
bot:{id}:state          # Current bot game state
bot:{id}:location       # Position and zone  
zone:{id}:bots          # Set of bots in zone
system:health           # Overall system status
```

## Deployment Architecture

### Container Strategy
```dockerfile
# Lightweight bot runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0-alpine
COPY BotRuntime ./
ENTRYPOINT ["./BotRuntime"]

# Resource limits per container
# Memory: 64MB limit, 32MB request
# CPU: 100m limit, 50m request  
```

### Orchestration (Docker Compose / Kubernetes)
```yaml
services:
  bot-manager:
    image: eqbot/manager
    scale: 2
    
  bot-runtime:
    image: eqbot/runtime
    scale: 50  # 50 containers = ~500 potential bots
    
  redis:
    image: redis:alpine
    
  postgres:
    image: postgres:15
    
  influxdb:
    image: influxdb:2.7
```

## Performance Targets

### Resource Efficiency
- **Memory**: 32MB base + 16MB per active behavior
- **CPU**: 0.5% idle, 2% during active gameplay
- **Network**: 1KB/s average, 10KB/s during combat
- **Startup**: <3 seconds from container creation to game login

### Scalability Metrics
- **Single Machine**: 100+ bots on 8GB/4-core system
- **Cluster**: 1000+ bots across multiple nodes
- **Response Time**: <100ms for management operations
- **Throughput**: 10,000+ bot actions per second

### Reliability Standards  
- **Bot Uptime**: 99.5% (automated restart on failure)
- **Service Uptime**: 99.9% (redundant management services)
- **Data Integrity**: Zero loss of bot configurations
- **Recovery Time**: <30 seconds for bot restart

## Critical Protocol Implementation Details

### Fragment Handling (EQStream.cs)
The fragment handling must match the server's expectations exactly:

```csharp
case SessionOp.Fragment:
    var tlen = packet.Data.NetU32(0);  // Total length in first fragment
    var rlen = -4;
    // Check if we have all fragments
    for(var i = packet.Sequence; futurePackets[i] != null && rlen < tlen; ++i)
        rlen += futurePackets[i].Data.Length;
    
    if(rlen < tlen) {
        // Store fragment but DON'T increment InSequence
        futurePackets[packet.Sequence] = packet;
        return false;  // Wait for more fragments
    }
    // Reassemble when complete
```

**Critical**: Do NOT increment InSequence for incomplete fragments - this breaks reassembly!

### World Authentication Sequence
The PostEnterWorld handler MUST send these responses:

```csharp
case WorldOp.PostEnterWorld:
    // These responses are MANDATORY for authentication
    Send(AppPacket.Create(WorldOp.ApproveWorld));
    Send(AppPacket.Create(WorldOp.WorldClientReady));
    break;
```

### Packet Structure Specifications

#### ClientZoneEntry (68 bytes)
```csharp
public struct ClientZoneEntry {
    uint unk;           // 4 bytes - must be 0
    char[64] CharName;  // 64 bytes - null-terminated string
}
```

#### Key Packet Sizes
- **LoginInfo**: 464 bytes (accountID + sessionKey)
- **ApproveWorld**: 544 bytes (fragmented from server)
- **EnterWorld Request**: 72 bytes (character name)
- **NewZone**: 944 bytes (zone initialization data)
- **PlayerProfile**: 26KB+ (heavily fragmented)

### Session Management
Each stream maintains independent sequence numbers:
- `OutSequence`: Increments for each sent packet
- `InSequence`: Increments ONLY for complete packets/fragments
- `lastAckReceived`: Tracks acknowledgments from server
- `lastAckSent`: Tracks our acknowledgments to server

## Security & Safety

### Bot Detection Mitigation
- **Behavior Randomization**: Vary timings and actions
- **Human-like Patterns**: Model real player behaviors
- **Response Variability**: Avoid perfectly consistent reactions
- **Social Interactions**: Engage naturally with other players

### System Security
- **API Authentication**: JWT tokens for management operations
- **Rate Limiting**: Prevent abuse of management endpoints
- **Input Validation**: Sanitize all configuration inputs
- **Audit Logging**: Track all bot management actions

### Resource Protection
- **Memory Limits**: Hard caps to prevent runaway processes
- **CPU Throttling**: Ensure fair resource sharing
- **Network Quotas**: Prevent bandwidth abuse
- **Connection Limits**: Respect server capacity

## Monitoring & Observability

### Health Checks
- **Bot Level**: Game connection, AI responsiveness, resource usage
- **Service Level**: API availability, database connections, queue health  
- **System Level**: CPU, memory, disk, network utilization

### Metrics Collection
- **Performance**: Response times, throughput, error rates
- **Business**: Bot population, activity distribution, player interactions
- **Infrastructure**: Resource utilization, container health, scaling events

### Alerting Strategy
- **Critical**: Service outages, data corruption, security breaches
- **Warning**: High resource usage, performance degradation, bot failures
- **Info**: Scaling events, configuration changes, routine maintenance

---

This architecture provides the foundation for a scalable, maintainable bot ecosystem while keeping individual components focused and optimized for their specific roles.