# Simple XML Documentation to Markdown converter
# Extracts documentation from the generated XML file and creates basic markdown

Write-Host "Converting XML Documentation to Markdown..."

# Ensure we're in the root directory
$rootDir = Split-Path -Parent $PSScriptRoot
Set-Location $rootDir

# Check if XML documentation exists
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

# Load the XML documentation
[xml]$xmlDoc = Get-Content $xmlDocPath

Write-Host "Processing XML documentation..."

# Create the main API index
$indexContent = @"
# EQProtocol API Documentation

Auto-generated from XML documentation comments.

## Namespaces

"@

# Group members by namespace
$namespaces = @{}
foreach ($member in $xmlDoc.doc.members.member) {
    $memberName = $member.name
    if ($memberName -match '^[TMF]:([\w\.]+)') {
        $fullName = $matches[1]
        $namespaceName = ($fullName -split '\.')[0..($fullName.Split('.').Length-2)] -join '.'
        if (-not $namespaces.ContainsKey($namespaceName)) {
            $namespaces[$namespaceName] = @()
        }
        $namespaces[$namespaceName] += $member
    }
}

# Generate namespace documentation
foreach ($namespace in $namespaces.Keys | Sort-Object) {
    $safeNamespace = $namespace -replace '[^\w]', '_'
    $namespaceFile = "$docsApiDir/$safeNamespace.md"

    $namespaceContent = @"
# $namespace Namespace

"@

    # Group by type (T:), Method (M:), Property (P:), etc.
    $types = @()
    $methods = @()
    $properties = @()
    $fields = @()

    foreach ($member in $namespaces[$namespace]) {
        $memberName = $member.name
        switch -Regex ($memberName) {
            '^T:' { $types += $member }
            '^M:' { $methods += $member }
            '^P:' { $properties += $member }
            '^F:' { $fields += $member }
        }
    }

    # Add types section
    if ($types.Count -gt 0) {
        $namespaceContent += "`n## Types`n`n"
        foreach ($type in $types) {
            $typeName = ($type.name -replace '^T:', '') -replace "^$namespace\.", ''
            $summary = if ($type.summary) { $type.summary.Trim() } else { "No description available." }
            $namespaceContent += "### $typeName`n`n$summary`n`n"
        }
    }

    # Add properties section
    if ($properties.Count -gt 0) {
        $namespaceContent += "`n## Properties`n`n"
        foreach ($prop in $properties) {
            $propName = ($prop.name -replace '^P:', '') -replace "^$namespace\.", ''
            $summary = if ($prop.summary) { $prop.summary.Trim() } else { "No description available." }
            $namespaceContent += "### $propName`n`n$summary`n`n"
        }
    }

    # Add methods section
    if ($methods.Count -gt 0) {
        $namespaceContent += "`n## Methods`n`n"
        foreach ($method in $methods) {
            $methodName = ($method.name -replace '^M:', '') -replace "^$namespace\.", ''
            $summary = if ($method.summary) { $method.summary.Trim() } else { "No description available." }
            $namespaceContent += "### $methodName`n`n$summary`n`n"

            # Add parameters if available
            if ($method.param) {
                $namespaceContent += "**Parameters:**`n`n"
                foreach ($param in $method.param) {
                    $paramName = $param.name
                    $paramDesc = if ($param.'#text') { $param.'#text'.Trim() } else { "No description" }
                    $namespaceContent += "- `$paramName`: $paramDesc`n"
                }
                $namespaceContent += "`n"
            }

            # Add returns if available
            if ($method.returns) {
                $returns = $method.returns.Trim()
                $namespaceContent += "**Returns:** $returns`n`n"
            }
        }
    }

    # Write namespace file
    $namespaceContent | Out-File -FilePath $namespaceFile -Encoding UTF8
    Write-Host "Generated: $namespaceFile"

    # Add to index
    $indexContent += "- [$namespace](./$safeNamespace.md)`n"
}

# Write index file
$indexFile = "$docsApiDir/README.md"
$indexContent | Out-File -FilePath $indexFile -Encoding UTF8

Write-Host ""
Write-Host "✅ Markdown documentation generated successfully!"
Write-Host "📁 Output location: $docsApiDir/"
Write-Host "📋 Index file: $indexFile"
Write-Host ""
Write-Host "Files generated:"
Get-ChildItem $docsApiDir -Name "*.md" | ForEach-Object { Write-Host "  - $_" }