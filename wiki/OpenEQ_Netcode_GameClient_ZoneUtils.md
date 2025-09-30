# OpenEQ.Netcode.GameClient.ZoneUtils Namespace

## Methods

### ZoneNameToNumber(System.String)

Convert zone name to zone ID

**Parameters:**

- $paramName: Zone short name (e.g., "arena", "qeynos")

**Returns:** Zone ID or 0 if not found

### ZoneNumberToName(System.UInt32)

Convert zone ID to zone name

**Parameters:**

- $paramName: Zone ID

**Returns:** Zone short name or "UNKNOWNZONE" if not found

### IsValidZoneId(System.UInt32)

Check if a zone ID exists in the mapping

**Parameters:**

- $paramName: Zone ID to check

**Returns:** True if zone ID is valid

### IsValidZoneName(System.String)

Check if a zone name exists in the mapping

**Parameters:**

- $paramName: Zone name to check

**Returns:** True if zone name is valid

### GetAllZoneNames

Get all valid zone names

**Returns:** Collection of all zone names

### GetAllZoneIds

Get all valid zone IDs

**Returns:** Collection of all zone IDs


