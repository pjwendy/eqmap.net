# Logging Configuration

This document describes the NLog configuration for the EQMap.NET project.

## Overview

Each project now uses external NLog.config files for flexible logging configuration. This allows you to easily turn logging on/off and adjust log levels without recompiling.

**All log files are stored in a shared logs folder at the solution root: `C:\Users\stecoc\git\eqmap.net\logs\`**

## Combined Logging Strategy

The logging system now uses **combined log files** to make it easier to follow the complete flow of operations:

### EQConsole Combined Log
- **Config File**: `EQConsole/NLog.config`
- **Main Log File**: `EQConsole-Combined-YYYY-MM-DD.log`
- **Contains**: EQConsole + EQProtocol logs combined
- **Additional Features**: Console output (INFO+), Debug output

### EQMap Combined Log
- **Config File**: `EQMap/NLog.config`
- **Main Log File**: `EQMap-Combined-YYYY-MM-DD.log`
- **Contains**: EQMap + Lua + EQProtocol logs combined
- **Additional Features**: Console output (INFO+), Debug output

### EQLogs
- **Config File**: `EQLogs/NLog.config`
- **Log Files** (in solution root `/logs/`):
  - `EQLogs-YYYY-MM-DD.log` - Main application log (DEBUG+)
  - `PacketAnalysis-YYYY-MM-DD.log` - Packet analysis logs
  - `Services-YYYY-MM-DD.log` - Docker/service logs

## Optional Individual Component Logs

For detailed debugging, individual component logs are also available:

### EQConsole Individual Logs
- `EQConsole-Only-YYYY-MM-DD.log` - EQConsole application only
- `Network-YYYY-MM-DD.log` - Network/packet logs only
- `Movement-YYYY-MM-DD.log` - Movement/navigation logs only

### EQMap Individual Logs
- `EQMap-Only-YYYY-MM-DD.log` - EQMap application only
- `Lua-Only-YYYY-MM-DD.log` - Lua script logs only

### EQProtocol Standalone
- `EQProtocol-Standalone-YYYY-MM-DD.log` - Only when testing EQProtocol library alone

## Log Features

### Automatic Archiving
- Logs rotate daily
- Keep 7 days of archives
- Archives stored in solution root `/logs/archive/` directory

### Log Levels
- **DEBUG**: Detailed diagnostic information
- **INFO**: General application information
- **WARN**: Warning messages
- **ERROR**: Error conditions
- **FATAL**: Critical errors

### Console Output
- Only INFO+ messages shown on console to reduce noise
- Can be adjusted by changing the `minlevel` in config files

## Customizing Logging

### Turning Logging On/Off
Edit the respective `NLog.config` file and:

1. **Disable all logging**: Comment out the logger rules:
```xml
<!-- <logger name="*" minlevel="Debug" writeTo="logfile" /> -->
```

2. **Change log levels**: Modify the `minlevel` attribute:
```xml
<!-- Only show warnings and errors -->
<logger name="*" minlevel="Warn" writeTo="logfile" />
```

3. **Disable console output**: Comment out console logger:
```xml
<!-- <logger name="*" minlevel="Info" writeTo="logconsole" /> -->
```

### Movement/Navigation Debugging
To enable detailed movement logging, the configs include specific rules:
```xml
<!-- Movement and Navigation logging -->
<logger name="*Movement*" minlevel="Debug" writeTo="movementfile" />
<logger name="*Navigation*" minlevel="Debug" writeTo="movementfile" />
```

Change `minlevel` to `Trace` for even more detail, or `Info` for less detail.

### Adding Custom Log Targets
You can add additional targets to the config files:

```xml
<targets>
  <!-- Add a new file target -->
  <target xsi:type="File" name="errorfile" fileName="logs/Errors-${shortdate}.log"
          layout="${longdate} [${level:uppercase=true}] ${message} ${exception:format=tostring}" />
</targets>

<rules>
  <!-- Log only errors to the error file -->
  <logger name="*" minlevel="Error" writeTo="errorfile" />
</rules>
```

## Fallback Configuration

If an NLog.config file is missing, each project will:
1. Log a warning message
2. Use a fallback programmatic configuration
3. Create logs with `-fallback` suffix in the filename

This ensures logging continues to work even if config files are missing.

## File Locations

Config files are automatically copied to the output directory during build. **All log files are created in the shared solution root logs directory:**

```
C:\Users\stecoc\git\eqmap.net\
├── logs/
│   ├── EQConsole-Combined-2024-09-22.log      # ⭐ MAIN: EQConsole + EQProtocol
│   ├── EQMap-Combined-2024-09-22.log          # ⭐ MAIN: EQMap + Lua + EQProtocol
│   ├── EQLogs-2024-09-22.log                  # EQLogs application
│   ├── PacketAnalysis-2024-09-22.log          # EQLogs packet analysis
│   ├── Services-2024-09-22.log                # EQLogs services
│   │
│   ├── EQConsole-Only-2024-09-22.log          # Optional: EQConsole only
│   ├── EQMap-Only-2024-09-22.log              # Optional: EQMap only
│   ├── Lua-Only-2024-09-22.log                # Optional: Lua only
│   ├── Network-2024-09-22.log                 # Optional: Network debugging
│   ├── Movement-2024-09-22.log                # Optional: Movement debugging
│   ├── EQProtocol-Standalone-2024-09-22.log   # Optional: EQProtocol standalone
│   │
│   └── archive/
│       ├── EQConsole-Combined-1.log
│       ├── EQMap-Combined-1.log
│       └── ...
├── EQConsole/
│   └── bin/Debug/net8.0/NLog.config
├── EQMap/
│   └── bin/Debug/net8.0-windows/NLog.config
└── ...
```

## Internal NLog Logging

If NLog itself encounters issues, internal logs are written to:
- `c:\temp\internal-nlog-{ProjectName}.txt`

This helps diagnose NLog configuration problems.