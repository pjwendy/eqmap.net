# Generate API Documentation from EQProtocol XML Documentation
# This script generates markdown documentation for the GitHub wiki

Write-Host "Generating EQProtocol API Documentation..."

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
$xmlDocPath = "EQProtocol/bin/Release/netstandard2.0/EQProtocol.xml"
if (-not (Test-Path $xmlDocPath)) {
    Write-Error "XML documentation file not found at: $xmlDocPath"
    exit 1
}

# Create docs/api directory
$docsApiDir = "docs/api"
if (-not (Test-Path $docsApiDir)) {
    New-Item -ItemType Directory -Path $docsApiDir -Force | Out-Null
}

# Restore xmldocmd tool
Write-Host "Restoring xmldocmd tool..."
Set-Location EQProtocol
dotnet tool restore

# Generate markdown documentation (if dependencies can be resolved)
Write-Host "Attempting to generate markdown documentation..."
$dllPath = "bin/Release/netstandard2.0/EQProtocol.dll"
$outputPath = "../docs/api"

# Try to generate documentation, but continue if it fails
try {
    dotnet xmldocmd $dllPath $outputPath --source "https://github.com/pjwendy/eqmap.net" --newline lf --clean --namespace OpenEQ 2>$null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Markdown documentation generated successfully in docs/api/"
    } else {
        Write-Warning "❌ xmldocmd failed due to dependency issues. XML documentation is available at: $xmlDocPath"
    }
} catch {
    Write-Warning "❌ xmldocmd failed due to dependency issues. XML documentation is available at: $xmlDocPath"
}

Write-Host ""
Write-Host "Documentation generation complete!"
Write-Host "- XML Documentation: $xmlDocPath"
Write-Host "- Target Location: docs/api/"
Write-Host "- GitHub Wiki: https://github.com/pjwendy/eqmap.net/wiki"
Write-Host ""
Write-Host "Note: If markdown generation failed, you can:"
Write-Host "1. Copy docs/api/*.md files to the GitHub wiki manually, or"
Write-Host "2. Use the XML file with other documentation tools"