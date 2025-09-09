# EQProtocol API Documentation

This document explains how to generate and publish API documentation for the EQProtocol project.

## Overview

The EQProtocol project is configured to automatically generate XML documentation during build. This documentation can then be converted to Markdown format for publication on the GitHub wiki.

## XML Documentation Generation

XML documentation is automatically generated when building the EQProtocol project:

```bash
dotnet build EQProtocol/EQProtocol.csproj -c Release
```

The XML file is generated at:
- **Debug**: `EQProtocol/bin/Debug/netstandard2.0/EQProtocol.xml`
- **Release**: `EQProtocol/bin/Release/netstandard2.0/EQProtocol.xml`

## Generating Markdown Documentation

### Automated Script

Run the provided PowerShell script to generate markdown documentation:

```powershell
.\scripts\generate-docs.ps1
```

This script will:
1. Build the EQProtocol project in Release mode
2. Generate XML documentation
3. Attempt to convert XML to Markdown using XMLDocMarkdown
4. Output results to `docs/api/` directory

### Manual Generation

If the automated script fails due to dependency issues, you can manually generate documentation:

```bash
cd EQProtocol
dotnet build -c Release
dotnet tool restore
dotnet xmldocmd "bin/Release/netstandard2.0/EQProtocol.dll" "../docs/api" --source https://github.com/pjwendy/eqmap.net --newline lf --clean --namespace OpenEQ
```

## Publishing to GitHub Wiki

### Option 1: Automated (if markdown generation works)
If the markdown generation is successful, copy the files from `docs/api/` to your GitHub wiki.

### Option 2: Manual Documentation
Use the generated XML file (`EQProtocol.xml`) with other documentation tools or manually create wiki pages.

### Option 3: GitHub Wiki Upload
1. Navigate to your GitHub repository's wiki: https://github.com/pjwendy/eqmap.net/wiki
2. Create new pages for each class/namespace
3. Copy the content from generated markdown files or XML documentation

## Documentation Structure

The generated documentation includes:

- **Classes**: All public classes in the OpenEQ.Netcode namespace
- **Methods**: Public methods with parameter and return type information
- **Properties**: Public properties with type information
- **Events**: Public events with delegate signatures
- **Enums**: Public enumerations with values

## Key Classes Documented

- `EQGameClient`: High-level game client for connecting to EverQuest servers
- `EQStream`: Low-level EQ protocol stream handling
- `LoginStream`: Login server connection handling
- `WorldStream`: World server connection handling  
- `ZoneStream`: Zone server connection handling
- `Packet`: EQ protocol packet structure
- Game client models (Character, Zone, NPC, Player, etc.)

## Maintaining Documentation

To keep documentation up to date:

1. Add XML documentation comments to new public APIs:
   ```csharp
   /// <summary>
   /// Brief description of the method or class
   /// </summary>
   /// <param name="paramName">Description of parameter</param>
   /// <returns>Description of return value</returns>
   public void MyMethod(string paramName) { }
   ```

2. Build the project to generate updated XML documentation
3. Re-run the documentation generation script
4. Update the GitHub wiki with new content

## Troubleshooting

### XMLDocMarkdown Dependency Issues
If `xmldocmd` fails with dependency resolution errors:
- Use the generated XML file directly with other documentation tools
- Consider using Visual Studio's built-in XML documentation features
- Manually create documentation from the XML file

### Missing Documentation Warnings
The project is configured to show warnings for missing XML documentation. Add documentation comments to resolve these warnings.

### Build Failures
Ensure all dependencies are properly restored:
```bash
dotnet restore
dotnet build
```