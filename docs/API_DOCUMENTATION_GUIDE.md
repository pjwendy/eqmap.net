# EQProtocol API Documentation Guide

The EQProtocol library includes comprehensive XML documentation that can be used to generate API documentation in various formats.

## Available Options

### Option 1: Use the Generated XML Documentation Directly

The XML documentation file is automatically generated during build:
```
EQProtocol/bin/Release/netstandard2.1/EQProtocol.xml
```

This file contains all the XML comments and can be used by:
- **Visual Studio/VS Code**: Provides IntelliSense and tooltips
- **JetBrains Rider**: Shows documentation in IDE
- **Other IDEs**: Most .NET IDEs can consume the XML file

### Option 2: Generate Markdown with Custom Script

Run the custom XML to Markdown converter:
```powershell
.\scripts\xml-to-markdown.ps1
```

This creates:
- `docs/api/README.md` - Main API index
- `docs/api/[Namespace].md` - Documentation for each namespace
- Organized by types, properties, and methods

### Option 3: Use DocFX (Advanced)

For a full documentation website:
```powershell
.\scripts\generate-docs-docfx.ps1
```

This creates a complete documentation site at `docs/_site/`.

### Option 4: GitHub Wiki Integration

1. Generate markdown files using Option 2
2. Copy the generated `.md` files to your GitHub Wiki
3. The wiki will automatically render the documentation

## Documentation Standards

The EQProtocol XML documentation includes:

### Class Documentation
```xml
/// <summary>
/// Represents a network packet used in EverQuest protocol communication.
/// Handles packet parsing, compression, encryption, and sequencing.
/// </summary>
public class Packet
```

### Method Documentation
```xml
/// <summary>
/// Unpacks binary data into the structure using a BinaryReader.
/// </summary>
/// <param name="br">The BinaryReader positioned at the start of the structure data</param>
void Unpack(BinaryReader br);
```

### Property Documentation
```xml
/// <summary>
/// Gets or sets the spawn ID of the entity being updated (player or NPC)
/// </summary>
public ushort ID { get; set; }
```

## Key Documented Components

### Core Interfaces
- **`IEQStruct`**: Base interface for all packet structures
- **`EQStream`**: Abstract base class for network streams

### Packet Classes
- **`Packet`**: Core network packet handling
- **`ClientUpdateFromServer`**: Position update packets
- **`UpdatePositionFromServer`**: Movement data structures

### Game Models
- **`Player`**: Player character representation
- **`NPC`**: Non-player character data
- **`Zone`**: Zone/area information

### Utility Classes
- **`Utility`**: Debug helpers and data formatting
- **`NavigationManager`**: Pathfinding and movement
- **`PacketParserService`**: Packet analysis tools

## Integration Notes

The XML documentation is designed to work with:
- **IntelliSense**: Auto-completion and tooltips in IDEs
- **API Documentation**: Generation of reference documentation
- **Code Analysis**: Static analysis tools and linters
- **Package Publishing**: NuGet package documentation

## Build Configuration

The project is configured to generate XML documentation automatically:
```xml
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
  <DocumentationFile>bin\$(Configuration)\$(TargetFramework)\$(AssemblyName).xml</DocumentationFile>
</PropertyGroup>
```

This ensures documentation is always up-to-date with the code.