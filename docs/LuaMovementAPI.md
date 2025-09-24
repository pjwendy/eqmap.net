# Lua Movement API Documentation

## Overview
The EQMap bot framework now supports automated character movement through Lua scripts using the navigation system. This allows bots to pathfind and move to specific coordinates within zones.

## Zone Object Methods

### MoveTo(x, y, z)
Moves the character to the specified coordinates using automated pathfinding.

**Parameters:**
- `x` (number): X coordinate in EverQuest world units
- `y` (number): Y coordinate in EverQuest world units  
- `z` (number): Z coordinate in EverQuest world units

**Example:**
```lua
-- Move to coordinates 100.5, -200.3, 10.0
zone:MoveTo(100.5, -200.3, 10.0)
```

### Properties
- `zone.CurrentZoneName` (string): Name of the current zone
- `zone.CurrentZoneId` (number): Numeric ID of the current zone

## Movement System Features

### Automatic Pathfinding
- Uses navigation mesh data for the current zone
- Automatically calculates safe paths around obstacles
- Sends position updates to the server at regular intervals

### Speed Control
- Base movement speed configurable (default: 0.7 - EQ running speed)
- Speed multipliers for buffs/debuffs (planned feature)
- Smooth movement with proper heading calculations

### Zone Navigation Files
- Navigation data loaded from `.nav` files in `GameClient/Maps/Nav/`
- Falls back to straight-line movement if no nav data available
- Supports user-customized navigation files

## Usage Examples

### Basic Movement Command Bot
```lua
SetMessageEventHandler(
    function(message)
        if string.find(string.lower(message.Message), "moveto") then
            local x, y, z = string.match(message.Message, "([%-]?%d+%.?%d*)[%s,]+([%-]?%d+%.?%d*)[%s,]+([%-]?%d+%.?%d*)")
            if x and y and z then
                zone:MoveTo(tonumber(x), tonumber(y), tonumber(z))
                chat:Say(string.format("Moving to %.2f, %.2f, %.2f", x, y, z))
            end
        end
    end
)
```

### Auto-Follow Bot
```lua
SetSpawnEventHandler(
    function(spawn)
        if spawn.Name == "PlayerToFollow" then
            local x = spawn.Position.X / 8.0  -- Convert EQ coordinates
            local y = spawn.Position.Y / 8.0
            local z = spawn.Position.Z / 8.0
            zone:MoveTo(x, y, z)
        end
    end
)
```

### Patrol Bot
```lua
local patrolPoints = {
    {100, 200, 0},
    {150, 250, 5},
    {75, 175, -2}
}
local currentPoint = 1

function patrol()
    local point = patrolPoints[currentPoint]
    zone:MoveTo(point[1], point[2], point[3])
    currentPoint = currentPoint + 1
    if currentPoint > #patrolPoints then
        currentPoint = 1
    end
end
```

## Technical Details

### Coordinate System
- Uses EverQuest's native coordinate system
- Y-axis is typically north/south
- X-axis is typically east/west  
- Z-axis is vertical (up/down)

### Movement Updates
- Position updates sent every 100ms by default
- Waypoint threshold: 2.0 units
- Destination threshold: 0.5 units
- Heading automatically calculated from movement direction

### Error Handling
- Returns `false` if movement cannot be initiated
- Logs errors to both NLog and Lua log system
- Stops movement on path calculation failures

## Planned Features
- `zone:StopMovement()` method
- Speed multiplier controls
- Movement event callbacks
- Path visualization
- Obstacle avoidance improvements
- Multi-zone pathfinding

## Files
- Navigation implementation: `EQProtocol/GameClient/Navigation/`
- Lua integration: `EQMap/Zone.cs`
- Example scripts: `scripts/movement.example`
- Nav data: `GameClient/Maps/Nav/*.nav`