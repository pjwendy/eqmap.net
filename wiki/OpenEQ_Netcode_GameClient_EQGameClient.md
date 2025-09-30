# OpenEQ.Netcode.GameClient.EQGameClient Namespace

## Methods

### LoginAsync(System.String,System.String,System.String,System.String)

Complete login sequence - connects to login server, selects world, selects character

### SendChat(System.String,OpenEQ.Netcode.ChatChannel)

Send a chat message

### MoveTo(System.Single,System.Single,System.Single,System.Single)

Move character to a specific location

### GetNearbyNPCs(System.Single)

Get all NPCs within a certain radius of the character

### InitializePacketCapture

Initialize packet capture system

### GetPacketEventEmitter

Get the current packet event emitter (for stream integration)

### Disconnect

Disconnect from the game

### MoveTo(System.Single,System.Single,System.Single)

Moves the character to the specified coordinates using pathfinding

### StopMovement

Stops any current movement

### DistanceTo(System.Double,System.Double,System.Double)

Get distance between current position and target coordinates

**Parameters:**

- $paramName: Target X coordinate
- $paramName: Target Y coordinate
- $paramName: Target Z coordinate (optional, defaults to current Z)

**Returns:** 3D distance if Z provided, 2D distance otherwise

### IsWithinDistance(System.Double,System.Double,System.Double,System.Double)

Check if player is within a certain distance of target coordinates

**Parameters:**

- $paramName: Target X coordinate
- $paramName: Target Y coordinate
- $paramName: Maximum distance
- $paramName: Target Z coordinate (optional)

**Returns:** True if within distance

### Sleep(System.Double)

Sleep for specified milliseconds - useful in Lua scripts for delays

**Parameters:**

- $paramName: Number of milliseconds to sleep

### SleepSeconds(System.Double)

Sleep for specified seconds - convenience method for Lua scripts

**Parameters:**

- $paramName: Number of seconds to sleep


