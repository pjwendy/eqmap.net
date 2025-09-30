# Generate API Documentation using DocFX
# DocFX handles complex dependencies better than xmldocmd

Write-Host "Generating EQProtocol API Documentation with DocFX..."

# Ensure we're in the root directory
$rootDir = Split-Path -Parent $PSScriptRoot
Set-Location $rootDir

# Build the EQProtocol project to generate XML documentation
Write-Host "Building EQProtocol project..."
dotnet build EQProtocol/EQProtocol.csproj -c Release --verbosity minimal

if ($LASTEXITCODE -ne 0) {
    Write-Error "Build failed. Cannot generate documentation."
    exit 1
}

# Check if XML documentation was generated
$xmlDocPath = "EQProtocol/bin/Release/netstandard2.1/EQProtocol.xml"
if (-not (Test-Path $xmlDocPath)) {
    Write-Error "XML documentation file not found at: $xmlDocPath"
    exit 1
}

# Create docs/api directory
$docsApiDir = "docs/api"
if (-not (Test-Path $docsApiDir)) {
    New-Item -ItemType Directory -Path $docsApiDir -Force | Out-Null
}

# Install DocFX globally if not already installed
Write-Host "Checking for DocFX..."
$docfxInstalled = Get-Command "docfx" -ErrorAction SilentlyContinue
if (-not $docfxInstalled) {
    Write-Host "Installing DocFX..."
    dotnet tool install -g docfx
}

# Create DocFX configuration
$docfxConfig = @"
{
  "metadata": [
    {
      "src": [
        {
          "files": ["EQProtocol/**/*.csproj"],
          "exclude": ["**/bin/**", "**/obj/**"]
        }
      ],
      "dest": "docs/api",
      "properties": {
        "TargetFramework": "netstandard2.1"
      }
    }
  ],
  "build": {
    "content": [
      {
        "files": ["docs/api/*.yml"],
        "dest": "api"
      },
      {
        "files": ["docs/*.md", "README.md"]
      }
    ],
    "resource": [
      {
        "files": ["images/**"]
      }
    ],
    "dest": "docs/_site",
    "globalMetadataFiles": [],
    "fileMetadataFiles": [],
    "template": ["default"],
    "postProcessors": [],
    "keepFileLink": false,
    "disableGitFeatures": false
  }
}
"@

# Write DocFX configuration
$docfxConfig | Out-File -FilePath "docfx.json" -Encoding UTF8

Write-Host "Generating API metadata..."
docfx metadata docfx.json

Write-Host "Building documentation site..."
docfx build docfx.json

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Documentation generated successfully!"
    Write-Host "📁 Output location: docs/_site/"
    Write-Host "🌐 Open docs/_site/index.html in a browser to view"
} else {
    Write-Warning "❌ DocFX generation failed. XML documentation is still available at: $xmlDocPath"
}

Write-Host ""
Write-Host "Documentation generation complete!"
Write-Host "- XML Documentation: $xmlDocPath"
Write-Host "- DocFX Site: docs/_site/"