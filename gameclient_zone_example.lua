-- GameClient Zone Utils Example for EQMap Lua Scripts
-- This script demonstrates the new GameClient-based zone and position functionality
-- All zone/position data now comes from EQProtocol GameClient for better architecture
-- Drop this file onto the EQMap window to execute

function Main()
    log:Info("=== GameClient Zone Utils Test Started ===")
    log:Info("NOTE: Zone and position data now comes from EQProtocol GameClient")

    -- Test the zone utility functions (now from GameClient ZoneUtils)
    log:Info("=== Zone Utility Function Tests ===")

    -- Test zone name to number conversion
    local arenaId = ZoneNameToNumber("arena")
    log:Info("Zone 'arena' has ID: " .. arenaId)

    local qeynosId = ZoneNameToNumber("qeynos")
    log:Info("Zone 'qeynos' has ID: " .. qeynosId)

    -- Test zone number to name conversion
    local zone77Name = ZoneNumberToName(77)
    log:Info("Zone ID 77 is: " .. zone77Name)

    local zone1Name = ZoneNumberToName(1)
    log:Info("Zone ID 1 is: " .. zone1Name)

    -- Test invalid zone
    local invalidZone = ZoneNumberToName(99999)
    log:Info("Invalid zone ID 99999 returns: " .. invalidZone)

    -- Test validation functions
    log:Info("=== Zone Validation Tests ===")
    log:Info("Is zone ID 77 valid? " .. tostring(IsValidZoneId(77)))
    log:Info("Is zone ID 99999 valid? " .. tostring(IsValidZoneId(99999)))
    log:Info("Is zone name 'arena' valid? " .. tostring(IsValidZoneName("arena")))
    log:Info("Is zone name 'invalidzone' valid? " .. tostring(IsValidZoneName("invalidzone")))

    -- Test current zone information if zone object is available
    if zone then
        log:Info("=== Current Zone Information (via GameClient) ===")
        log:Info("Current Zone Name: " .. (zone.Name or "Unknown"))
        log:Info("Current Zone ID: " .. tostring(zone.Id or 0))
        log:Info("Raw Zone Name from Client: " .. (zone.CurrentZoneName or "Unknown"))
        log:Info("Zone ID from Client: " .. tostring(zone.CurrentZoneId or 0))

        -- Verify the zone name/ID consistency
        local expectedName = ZoneNumberToName(zone.Id)
        if zone.Name == expectedName then
            log:Info("✓ Zone name/ID mapping is consistent")
        else
            log:Info("✗ Zone name/ID mapping mismatch!")
            log:Info("  Expected: " .. expectedName .. ", Got: " .. zone.Name)
        end

        log:Info("=== Player Location (GameClient Data) ===")
        log:Info("Player Name: " .. (zone.PlayerName or "Unknown"))
        log:Info("Position: " .. zone.Position)
        log:Info("Full Location: " .. zone.Location)
        log:Info("Player Heading: " .. string.format("%.1f", zone.Heading or 0))
        log:Info("Is Moving: " .. tostring(zone.IsMoving))

        -- Test some zone lookup examples
        log:Info("=== Zone Lookup Examples ===")
        local commonZones = {"qeynos", "freeporte", "arena", "gfaydark", "crushbone", "blackburrow"}

        for i, zoneName in ipairs(commonZones) do
            local zoneId = ZoneNameToNumber(zoneName)
            if zoneId > 0 then
                log:Info(string.format("Zone '%s' = ID %d", zoneName, zoneId))
            else
                log:Info(string.format("Zone '%s' not found", zoneName))
            end
        end

        -- Distance examples with proper zone names
        if zone.X and zone.Y and zone.Z then
            log:Info("=== Distance Calculation Examples (GameClient) ===")

            -- Example: distance to zone center (0, 0)
            local distanceToCenter = zone:DistanceTo(0, 0)
            log:Info(string.format("Distance to %s center (0, 0): %.2f units", zone.Name, distanceToCenter))

            -- Example: check if within certain distance of spawn point
            local withinSpawnRange = zone:IsWithinDistance(0, 0, 1000)
            log:Info(string.format("Within 1000 units of %s center: %s", zone.Name, tostring(withinSpawnRange)))

            -- 3D distance example
            local distance3D = zone:DistanceTo(100, 200, zone.Z + 50)
            log:Info(string.format("3D Distance to (100, 200, %.1f): %.2f units", zone.Z + 50, distance3D))
        end

        -- Test movement capabilities
        log:Info("=== Movement System Tests ===")
        log:Info("Movement system available via zone:MoveTo() and zone:StopMovement()")
        log:Info("Current movement state: " .. (zone.IsMoving and "Moving" or "Stationary"))

        -- Note: Uncomment the following lines to test actual movement
        -- log:Info("Testing movement - moving 10 units forward...")
        -- zone:MoveTo(zone.X, zone.Y + 10, zone.Z)
        -- sleepSeconds(2)
        -- log:Info("Stopping movement...")
        -- zone:StopMovement()

        -- Test sleep functions (delegated to GameClient)
        log:Info("=== Sleep Function Tests (GameClient) ===")
        log:Info("Testing 0.5 second sleep via zone:Sleep()...")
        zone:Sleep(500)  -- 500ms sleep via GameClient
        log:Info("Sleep completed")

    else
        log:Info("Zone object not available - you need to be connected to EverQuest")
        log:Info("Connect to EQ first to see current zone information")
        log:Info("The zone object is now a lightweight wrapper around EQGameClient")
    end

    log:Info("=== Architecture Notes ===")
    log:Info("* Zone class now delegates to EQGameClient in EQProtocol")
    log:Info("* All position/movement data comes from GameClient")
    log:Info("* ZoneUtils moved to OpenEQ.Netcode.GameClient namespace")
    log:Info("* EQMap is now just UI/graphics and Lua integration")
    log:Info("* GameClient handles all game logic and state")

    log:Info("=== GameClient Zone Utils Test Completed ===")
    return "GameClient zone utils test completed successfully"
end

-- Example function for zone-specific scripting using GameClient data
function ZoneSpecificAction()
    if not zone then
        log:Info("Zone not available for zone-specific actions")
        return false
    end

    local currentZone = zone.Name
    log:Info("Executing zone-specific action for: " .. currentZone)

    -- Example zone-specific logic using proper zone names from GameClient
    if currentZone == "arena" then
        log:Info("Arena detected - could implement PvP bot behavior here")
        log:Info("Current position: " .. zone.Position)
        log:Info("Player heading: " .. string.format("%.1f", zone.Heading))
    elseif currentZone == "qeynos" then
        log:Info("Qeynos detected - could implement city/trading behavior here")
    elseif currentZone == "crushbone" then
        log:Info("Crushbone detected - could implement dungeon crawling here")
    elseif currentZone == "gfaydark" then
        log:Info("Greater Faydark detected - could implement outdoor hunting here")
    else
        log:Info("Zone '" .. currentZone .. "' - using default behavior")
    end

    return true
end

-- Example function to validate zone before performing actions
function ValidateZoneAndExecute(requiredZone, actionFunction)
    if not zone then
        log:Info("Zone not available - cannot validate zone")
        return false
    end

    local currentZone = zone.Name

    if currentZone ~= requiredZone then
        log:Info(string.format("Zone mismatch: Expected '%s', currently in '%s'", requiredZone, currentZone))
        return false
    end

    log:Info(string.format("Zone validated: Currently in '%s' as expected", currentZone))

    if actionFunction then
        return actionFunction()
    end

    return true
end

-- Example usage of GameClient-based zone validation
function ExampleZoneValidation()
    log:Info("=== Zone Validation Example (GameClient) ===")

    -- This would only execute if we're in the arena
    ValidateZoneAndExecute("arena", function()
        log:Info("Arena-specific action executed!")
        log:Info("Arena position: " .. zone.Position)
        return true
    end)

    -- This would only execute if we're in qeynos
    ValidateZoneAndExecute("qeynos", function()
        log:Info("Qeynos-specific action executed!")
        log:Info("Qeynos position: " .. zone.Position)
        return true
    end)
end

-- Uncomment the next line to run zone-specific examples
-- ZoneSpecificAction()
-- ExampleZoneValidation()