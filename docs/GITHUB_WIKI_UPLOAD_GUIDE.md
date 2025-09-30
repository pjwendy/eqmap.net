# GitHub Wiki Upload Guide for EQProtocol API Documentation

The EQProtocol API documentation has been successfully generated as Markdown files. This guide provides step-by-step instructions for uploading them to the GitHub wiki at https://github.com/pjwendy/eqmap.net/wiki.

## 📁 Generated Documentation Files

The following files have been created in `docs/api/`:

### Core Documentation
- **`README.md`** - Main API index with namespace links
- **`EQProtocol_Streams_Common_IEQStruct.md`** - Core packet structure interface
- **`EQProtocol_Streams_Common_Packet.md`** - Network packet handling
- **`EQProtocol_Streams_Common_EQStream.md`** - Base stream class
- **`OpenEQ_Netcode_Utility.md`** - Debug and utility functions

### Game Client Documentation
- **`OpenEQ_Netcode_GameClient_EQGameClient.md`** - Main game client class
- **`OpenEQ_Netcode_GameClient_Models.md`** - Player, NPC, and game models
- **`OpenEQ_Netcode_ClientUpdateFromServer.md`** - Position update packets
- **`OpenEQ_Netcode_UpdatePositionFromServer.md`** - Movement data structures

### Packet Structure Documentation
- **300+ packet structure files** organized by namespace and functionality

## 🚀 Upload Methods

### Method 1: GitHub Web Interface (Recommended)

1. **Navigate to the GitHub Wiki**:
   - Go to https://github.com/pjwendy/eqmap.net/wiki
   - Click "New Page" or edit existing pages

2. **Create the Main API Index**:
   - Create a new page called "API Documentation"
   - Copy the contents of `docs/api/README.md`
   - Paste into the wiki page editor
   - Save the page

3. **Upload Individual Documentation Pages**:
   - For each `.md` file in `docs/api/`:
     - Create a new wiki page with the same name (without .md extension)
     - Copy the file contents
     - Paste into the wiki editor
     - Save the page

4. **Update Wiki Sidebar** (Optional):
   - Edit the wiki sidebar to include links to key documentation pages
   - Add links to "API Documentation" and major namespace pages

### Method 2: Git Clone Method (Advanced)

1. **Clone the Wiki Repository**:
   ```bash
   git clone https://github.com/pjwendy/eqmap.net.wiki.git
   cd eqmap.net.wiki
   ```

2. **Copy Documentation Files**:
   ```bash
   # Copy all documentation files
   cp ../docs/api/*.md .
   ```

3. **Commit and Push**:
   ```bash
   git add *.md
   git commit -m "Add comprehensive EQProtocol API documentation

   - Complete XML documentation for all public APIs
   - Organized by namespace and functionality
   - Includes examples and parameter details
   - Generated from XML comments in source code"
   git push origin master
   ```

### Method 3: Batch Upload Script

Create a PowerShell script to automate the upload process:

```powershell
# Note: This requires GitHub CLI (gh) to be installed
$apiDocsPath = "docs/api"
$files = Get-ChildItem "$apiDocsPath/*.md" -Name

foreach ($file in $files) {
    $pageName = $file -replace '\.md$', ''
    $content = Get-Content "$apiDocsPath/$file" -Raw

    # Use GitHub CLI to create wiki pages
    # (This is a conceptual example - actual implementation may vary)
    Write-Host "Uploading: $pageName"
    # gh api repos/pjwendy/eqmap.net/wiki --method POST --field title="$pageName" --field content="$content"
}
```

## 📋 Recommended Upload Priority

Upload in this order for best user experience:

1. **`README.md`** → **"API-Documentation"** (Main index page)
2. **Core interfaces**:
   - `EQProtocol_Streams_Common_IEQStruct.md` → **"IEQStruct-Interface"**
   - `EQProtocol_Streams_Common_Packet.md` → **"Packet-Class"**
   - `EQProtocol_Streams_Common_EQStream.md` → **"EQStream-Base-Class"**

3. **Game client documentation**:
   - `OpenEQ_Netcode_GameClient_EQGameClient.md` → **"EQGameClient"**
   - `OpenEQ_Netcode_GameClient_Models.md` → **"Game-Models"**

4. **Key packet structures**:
   - `OpenEQ_Netcode_ClientUpdateFromServer.md` → **"ClientUpdate-Packets"**
   - `OpenEQ_Netcode_UpdatePositionFromServer.md` → **"Position-Updates"**

5. **Utility classes**:
   - `OpenEQ_Netcode_Utility.md` → **"Utility-Functions"**

6. **Remaining packet structures** (300+ files)

## 🔗 Wiki Organization

### Suggested Wiki Structure:

```
📁 EQMap.NET Wiki
├── 🏠 Home
├── 📖 API Documentation (main index)
├── 🔧 Core Interfaces
│   ├── IEQStruct Interface
│   ├── Packet Class
│   └── EQStream Base Class
├── 🎮 Game Client
│   ├── EQGameClient
│   ├── Game Models
│   └── Navigation
├── 📦 Packet Structures
│   ├── ClientUpdate Packets
│   ├── Position Updates
│   ├── Login Packets
│   ├── World Packets
│   └── Zone Packets
├── 🛠️ Utilities
│   └── Utility Functions
└── 📚 Development Guides
    ├── Getting Started
    ├── Bot Development
    └── Contributing
```

## ✨ Enhanced Wiki Features

### 1. Cross-References
Link related pages using wiki syntax:
```markdown
See also: [[IEQStruct-Interface]] for the base interface
```

### 2. Code Examples
The generated documentation includes code examples that will render nicely:
```csharp
var packet = new ClientUpdateFromServer(spawnID, positionData);
```

### 3. Navigation
Update the wiki sidebar (`_Sidebar.md`) to include:
```markdown
## API Documentation
- [[API-Documentation|Main Index]]
- [[IEQStruct-Interface|Core Interface]]
- [[EQGameClient|Game Client]]
- [[ClientUpdate-Packets|Position Updates]]
```

## 🎯 Benefits of This Documentation

Once uploaded, the wiki will provide:

- **✅ Complete API Reference**: All public classes, methods, and properties documented
- **✅ IntelliSense Integration**: XML documentation provides IDE tooltips and autocomplete
- **✅ Developer-Friendly**: Clear examples and parameter descriptions
- **✅ Searchable**: GitHub wiki search will find relevant documentation quickly
- **✅ Version Control**: Documentation updates with code changes
- **✅ Community Contribution**: Others can contribute to and improve the documentation

## 📝 Post-Upload Tasks

After uploading:

1. **Test Navigation**: Verify all internal links work correctly
2. **Update Main README**: Add links to the wiki documentation
3. **Announce**: Let the community know about the new comprehensive API documentation
4. **Maintain**: Keep documentation updated as the codebase evolves

## 🔄 Automation for Future Updates

Consider setting up GitHub Actions to automatically regenerate and update wiki documentation when the codebase changes:

```yaml
name: Update API Documentation
on:
  push:
    paths: ['EQProtocol/**/*.cs']
jobs:
  update-docs:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v3
      - name: Generate Documentation
        run: .\scripts\xml-to-markdown.ps1
      - name: Update Wiki
        # Upload generated files to wiki
```

---

**The comprehensive EQProtocol API documentation is now ready for upload to make the library accessible to all developers!** 🚀