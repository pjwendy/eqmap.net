# PowerShell script to extract Underfoot client opcodes and structures
# Converts bash script to PowerShell for Windows environment

param(
    [string[]]$OpcodeList = @(),          # Process only specific opcodes
    [switch]$ExtractAll = $false,         # Extract all opcodes (required for full extraction)
    [switch]$Help = $false                # Show help
)

# Show help if requested
if ($Help) {
    Write-Host "UF Opcode Extractor Script Parameters:"
    Write-Host ""
    Write-Host "  -ExtractAll        Extract all opcodes (REQUIRED for full extraction)"
    Write-Host "  -OpcodeList        Process only specific opcodes (comma-separated list)"
    Write-Host "  -Help              Show this help message"
    Write-Host ""
    Write-Host "Safety: Script requires either -ExtractAll or -OpcodeList to prevent accidental runs"
    Write-Host ""
    Write-Host "Examples:"
    Write-Host "  .\uf_opcode_extractor.ps1 -ExtractAll                       # Extract all opcodes"
    Write-Host "  .\uf_opcode_extractor.ps1 -OpcodeList 'OP_Action','OP_Buff' # Extract specific opcodes"
    Write-Host "  .\uf_opcode_extractor.ps1                                   # Does nothing (safe default)"
    exit 0
}

# Safety check - require explicit action to prevent accidental runs
if (-not $ExtractAll -and $OpcodeList.Count -eq 0) {
    Write-Host "UF Opcode Extractor - No action specified"
    Write-Host ""
    Write-Host "To prevent accidental overwriting of C# files, you must specify:"
    Write-Host "  -ExtractAll         to process all opcodes"
    Write-Host "  -OpcodeList         to process specific opcodes"
    Write-Host "  -Help               to show help"
    Write-Host ""
    Write-Host "Example: .\uf_opcode_extractor.ps1 -ExtractAll"
    exit 0
}

# Validate that both ExtractAll and OpcodeList aren't specified together
if ($ExtractAll -and $OpcodeList.Count -gt 0) {
    Write-Host "Error: Cannot specify both -ExtractAll and -OpcodeList"
    Write-Host "Use -ExtractAll for all opcodes OR -OpcodeList for specific opcodes"
    exit 1
}

# Define file paths
$UF_OPS_H = "C:\Users\stecoc\git\Server\common\patches\uf_ops.h"
$UF_CPP = "C:\Users\stecoc\git\Server\common\patches\uf.cpp"
$UF_STRUCTS_H = "C:\Users\stecoc\git\Server\common\patches\uf_structs.h"
$UF_CONF = "C:\Users\stecoc\git\Server\utils\patches\patch_UF.conf"
$OUTFILE = "C:\Users\stecoc\git\eqmap.net\docs\uf_opcode_structs.md"
$LOGFILE = "C:\Users\stecoc\git\eqmap.net\docs\uf_opcode_extractor.log"
$CSHARP_PACKAGES_DIR = "C:\Users\stecoc\git\eqmap.net\EQProtocol\Packages"
$CSPROJ_FILE = "C:\Users\stecoc\git\eqmap.net\EQProtocol\eqprotocol.csproj"
$TEMPLATE_FILE = "C:\Users\stecoc\Documents\class.txt"

# Function to write to both console and log file
function Write-Log {
    param(
        [string]$Message,
        [string]$Level = "INFO"
    )

    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $logEntry = "[$timestamp] [$Level] $Message"

    # Write to console
    Write-Host $Message

    # Write to log file
    Add-Content -Path $LOGFILE -Value $logEntry -Encoding UTF8
}


function Extract-Block {
    param(
        [string]$FilePath,
        [string]$Keyword,
        [string]$Opcode
    )

    $content = Get-Content $FilePath
    $inBlock = $false
    $braceCount = 0
    $started = $false
    $result = @()
    $inComment = $false

    foreach ($line in $content) {
        # Track block comments
        if ($line -match '/\*') { $inComment = $true }
        if ($line -match '\*/') { $inComment = $false }

        # Check for macro at start of line (not in comments)
        if (!$inComment -and $line -notmatch '^\s*//' -and $line -match "^\s*$Keyword\($Opcode\)") {
            $inBlock = $true
            $braceCount = 0
            $started = $false
        }

        if ($inBlock) {
            $openBraces = ($line -split '\{').Count - 1
            $closeBraces = ($line -split '\}').Count - 1

            $braceCount += $openBraces
            if ($openBraces -gt 0) { $started = $true }
            $braceCount -= $closeBraces

            $result += $line

            if ($started -and $braceCount -eq 0) {
                $inBlock = $false
                break
            }
        }
    }

    return $result -join "`n"
}

function Convert-CppTypeToCSharp {
    param([string]$CppType)

    switch ($CppType) {
        "uint32" { return "uint" }
        "uint32_t" { return "uint" }
        "int32" { return "int" }
        "int32_t" { return "int" }
        "uint16" { return "ushort" }
        "uint16_t" { return "ushort" }
        "int16" { return "short" }
        "int16_t" { return "short" }
        "uint8" { return "byte" }
        "uint8_t" { return "byte" }
        "int8" { return "sbyte" }
        "int8_t" { return "sbyte" }
        "float" { return "float" }
        "double" { return "double" }
        "char" { return "byte" }
        default { return "uint" }  # Default to uint for unknown types
    }
}

function Convert-CppFieldToCSharp {
    param(
        [string]$FieldLine,
        [string]$StructName = ""
    )

    # Remove comments and trim - handle both /* */ and // comments
    $cleanLine = $FieldLine
    if ($cleanLine -match '/\*.*?\*/') {
        $cleanLine = $cleanLine -replace '/\*.*?\*/', ''
    }
    $cleanLine = ($cleanLine -split '//')[0].Trim()

    if ([string]::IsNullOrWhiteSpace($cleanLine) -or $cleanLine -match '^\s*[{}];?\s*$' -or $cleanLine -match '^\s*struct\s+' -or $cleanLine -match '^\s*/\*') {
        return $null
    }

    # Handle various C++ field patterns
    # Pattern 1: /*offset*/ type field_name;
    if ($cleanLine -match '^\s*(/\*[^*]*\*/)?\s*([a-zA-Z_][a-zA-Z0-9_]*)\s+([a-zA-Z_][a-zA-Z0-9_]*)\s*(\[[^\]]*\])?\s*;?\s*$') {
        $cppType = $matches[2]
        $fieldName = $matches[3]
        $arrayPart = $matches[4]
    }
    # Pattern 2: signed/unsigned with bitfields: signed delta_x : 13;
    elseif ($cleanLine -match '^\s*(/\*[^*]*\*/)?\s*(signed|unsigned)\s+([a-zA-Z_][a-zA-Z0-9_]*)\s*:\s*\d+\s*;?\s*$') {
        if ($matches[2] -eq "signed") {
            $cppType = "int32"
        } else {
            $cppType = "uint32"
        }
        $fieldName = $matches[3]
        $arrayPart = ""
    }
    # Pattern 3: Basic type with bitfield: uint32 field : 12;
    elseif ($cleanLine -match '^\s*(/\*[^*]*\*/)?\s*([a-zA-Z_][a-zA-Z0-9_]*)\s+([a-zA-Z_][a-zA-Z0-9_]*)\s*:\s*\d+\s*;?\s*$') {
        $cppType = $matches[2]
        $fieldName = $matches[3]
        $arrayPart = ""
    }
    else {
        return $null
    }

    # Skip padding fields
    if ($fieldName -match '^padding' -or $fieldName -match '^unknown' -and $fieldName.Length -gt 10) {
        return $null
    }

    # Convert to PascalCase
    $propertyName = (Get-Culture).TextInfo.ToTitleCase($fieldName.Replace('_', ' ')).Replace(' ', '')

    # Avoid naming conflicts with struct name
    if (![string]::IsNullOrWhiteSpace($StructName) -and $propertyName -eq $StructName) {
        $propertyName = $propertyName + "Value"
    }

    # Convert C++ type to C#
    $csharpType = Convert-CppTypeToCSharp $cppType

    # Handle arrays
    if (![string]::IsNullOrWhiteSpace($arrayPart)) {
        if ($arrayPart -match '\[(\d+)\]') {
            $arraySize = [int]$matches[1]
            if ($arraySize -gt 1) {
                # For now, just use array notation - could be enhanced to use fixed buffers later
                $csharpType = "$csharpType[]"
            }
        }
    }

    return @{
        PropertyName = $propertyName
        Type = $csharpType
        OriginalName = $fieldName
        OriginalType = $cppType
        IsArray = ![string]::IsNullOrWhiteSpace($arrayPart)
    }
}

function Generate-CSharpStruct {
    param(
        [string]$OpcodeName,
        [string]$StructDefinition,
        [string]$TemplateContent,
        [string]$EncodeDecodeSection = ""
    )

    # Remove OP_ prefix and convert to PascalCase
    $className = $OpcodeName -replace '^OP_', ''
    $className = $className -replace '_', ''

    # Parse struct fields
    $fields = @()
    if ($StructDefinition -ne "Structure definition not found." -and ![string]::IsNullOrWhiteSpace($StructDefinition)) {
        $lines = $StructDefinition -split "`n"

        foreach ($line in $lines) {
            $field = Convert-CppFieldToCSharp $line $className
            if ($field) {
                $fields += $field
            }
        }
    }

    # Start with template
    $csharpCode = $TemplateContent

    # Add comments at the top with C++ structure and encode/decode sections
    $comments = ""
    if (![string]::IsNullOrWhiteSpace($StructDefinition) -and $StructDefinition -ne "Structure definition not found.") {
        $comments += "// C++ Structure Definition:`r`n"
        $structLines = $StructDefinition -split "`n"
        foreach ($line in $structLines) {
            $comments += "// " + $line.Trim() + "`r`n"
        }
    }
    else {
        $comments += "// C++ Structure Definition: Not found or not applicable`r`n"
    }

    $comments += "`r`n"

    if (![string]::IsNullOrWhiteSpace($EncodeDecodeSection) -and $EncodeDecodeSection -ne "Section not found.") {
        $comments += "// ENCODE/DECODE Section:`r`n"
        $encodeLines = $EncodeDecodeSection -split "`n"
        foreach ($line in $encodeLines) {
            $comments += "// " + $line.Trim() + "`r`n"
        }
    }
    else {
        $comments += "// ENCODE/DECODE Section: Not found or not applicable`r`n"
    }

    # Insert comments after all using statements, before the namespace or public struct line
    $namespaceStart = $csharpCode.IndexOf("namespace ")
    $structStart = $csharpCode.IndexOf("public struct ")

    if ($namespaceStart -ge 0) {
        # Insert before namespace
        $beforeNamespace = $csharpCode.Substring(0, $namespaceStart)
        $afterNamespace = $csharpCode.Substring($namespaceStart)
        $csharpCode = $beforeNamespace + $comments + "`r`n" + $afterNamespace
    } elseif ($structStart -ge 0) {
        # Insert before struct (no namespace)
        $beforeStruct = $csharpCode.Substring(0, $structStart)
        $afterStruct = $csharpCode.Substring($structStart)
        $csharpCode = $beforeStruct + $comments + "`r`n" + $afterStruct
    }

    # Replace placeholders
    $csharpCode = $csharpCode -replace '\*\*opcode\*\*', $className

    if ($fields.Count -eq 0) {
        # Handle empty struct - no properties
        # Check if using namespace template and adjust indentation accordingly
        if ($csharpCode.Contains("namespace ")) {
            $csharpCode = $csharpCode -replace '\*\*add structure\*\*', "`t`t// No properties - structure definition not found or empty"
        } else {
            $csharpCode = $csharpCode -replace '\*\*add structure\*\*', "`t// No properties - structure definition not found or empty"
        }
        # For C# 8.0 compatibility, comment out parameterless constructor for empty structs
        # Handle both namespace and non-namespace templates
        if ($csharpCode.Contains("namespace ")) {
            $constructorPattern = '(\s*public\s+' + [regex]::Escape($className) + '\s*\(\s*\)\s*:\s*this\s*\(\s*\)\s*\{[\s\S]*?\})'
            $replacement = "`r`n`t`t// C# 8.0: Parameterless struct constructors not supported - uncomment if using C# 10.0+`r`n`t`t// public $className() : this() {`r`n`t`t// `t`t// No assignments needed`r`n`t`t// }"
        } else {
            $constructorPattern = '(\s*public\s+' + [regex]::Escape($className) + '\s*\(\s*\)\s*:\s*this\s*\(\s*\)\s*\{[\s\S]*?\})'
            $replacement = "`r`n`t// C# 8.0: Parameterless struct constructors not supported - uncomment if using C# 10.0+`r`n`t// public $className() : this() {`r`n`t// `t`t// No assignments needed`r`n`t// }"
        }
        # Replace placeholders for empty structs
        $csharpCode = $csharpCode -replace '\*\*structure fields\*\*', ""
        $csharpCode = $csharpCode -replace '\s*\*\*constructor params docs\*\*', ""
        # THEN comment out the parameterless constructor
        $csharpCode = $csharpCode -replace $constructorPattern, $replacement
        if ($csharpCode.Contains("namespace ")) {
            $csharpCode = $csharpCode -replace '\*\*assign structure\*\*', "`t`t`t// No assignments needed"
            $csharpCode = $csharpCode -replace '\*\*read buffer\*\*', "`t`t`t// No data to read"
            $csharpCode = $csharpCode -replace '\*\*write buffer\*\*', "`t`t`t// No data to write"
        } else {
            $csharpCode = $csharpCode -replace '\*\*assign structure\*\*', "`t`t// No assignments needed"
            $csharpCode = $csharpCode -replace '\*\*read buffer\*\*', "`t`t// No data to read"
            $csharpCode = $csharpCode -replace '\*\*write buffer\*\*', "`t`t// No data to write"
        }

        # Handle empty ToString
        $templateStart = $csharpCode.IndexOf('**property start**')
        $templateEnd = $csharpCode.IndexOf('**property end**')
        if ($templateStart -ge 0 -and $templateEnd -ge 0) {
            $beforeTemplate = $csharpCode.Substring(0, $templateStart)
            $afterTemplate = $csharpCode.Substring($templateEnd + '**property end**'.Length)
            if ($csharpCode.Contains("namespace ")) {
                $csharpCode = $beforeTemplate + "`t`t`tret += `"`t// No properties`\n`";`r`n`t`t`tret += `"}`";`r`n`t`t`t" + $afterTemplate
            } else {
                $csharpCode = $beforeTemplate + "`t`tret += `"`t// No properties`\n`";`r`n`t`tret += `"}`";`r`n`t`t" + $afterTemplate
            }
        }
    }
    else {
        # Generate properties for non-empty structs
        $properties = ""
        foreach ($field in $fields) {
            # Add XML documentation for the property
            $properties += "`t`t/// <summary>`r`n"
            $properties += "`t`t/// Gets or sets the $($field.PropertyName.ToLower()) value.`r`n"
            $properties += "`t`t/// </summary>`r`n"
            $properties += "`t`tpublic $($field.Type) $($field.PropertyName) { get; set; }`r`n`r`n"
        }
        $csharpCode = $csharpCode -replace '\s*\*\*add structure\*\*', "`r`n$($properties.TrimEnd())"

        # Generate constructor parameters
        $constructorParams = ($fields | ForEach-Object { "$($_.Type.ToLower()) $($_.OriginalName)" }) -join ', '
        $csharpCode = $csharpCode -replace '\*\*structure fields\*\*', $constructorParams

        # Generate constructor parameter documentation
        $constructorParamDocs = ""
        foreach ($field in $fields) {
            $constructorParamDocs += "`t`t/// <param name=`"$($field.OriginalName)`">The $($field.PropertyName.ToLower()) value.</param>`r`n"
        }
        $csharpCode = $csharpCode -replace '\s*\*\*constructor params docs\*\*', "`r`n$($constructorParamDocs.TrimEnd())"

        # Generate constructor assignments
        $assignments = ""
        foreach ($field in $fields) {
            $assignments += "`t`t`t$($field.PropertyName) = $($field.OriginalName);`r`n"
        }
        $csharpCode = $csharpCode -replace '\s*\*\*assign structure\*\*', "`r`n$($assignments.TrimEnd())"

        # Generate read buffer (Unpack)
        $readBuffer = ""
        foreach ($field in $fields) {
            if ($field.IsArray -and $field.Type.EndsWith("[]")) {
                $baseType = $field.Type.Replace("[]", "")
                $readBuffer += "`t`t`t// TODO: Array reading for $($field.PropertyName) - implement based on actual array size`r`n"
                $readBuffer += "`t`t`t// $($field.PropertyName) = new $baseType[size];`r`n"
            }
            else {
                switch ($field.Type) {
                    "uint" { $readBuffer += "`t`t`t$($field.PropertyName) = br.ReadUInt32();`r`n" }
                    "int" { $readBuffer += "`t`t`t$($field.PropertyName) = br.ReadInt32();`r`n" }
                    "ushort" { $readBuffer += "`t`t`t$($field.PropertyName) = br.ReadUInt16();`r`n" }
                    "short" { $readBuffer += "`t`t`t$($field.PropertyName) = br.ReadInt16();`r`n" }
                    "byte" { $readBuffer += "`t`t`t$($field.PropertyName) = br.ReadByte();`r`n" }
                    "sbyte" { $readBuffer += "`t`t`t$($field.PropertyName) = br.ReadSByte();`r`n" }
                    "float" { $readBuffer += "`t`t`t$($field.PropertyName) = br.ReadSingle();`r`n" }
                    "double" { $readBuffer += "`t`t`t$($field.PropertyName) = br.ReadDouble();`r`n" }
                    default { $readBuffer += "`t`t`t$($field.PropertyName) = br.ReadUInt32();`r`n" }
                }
            }
        }
        $csharpCode = $csharpCode -replace '\s*\*\*read buffer\*\*', "`r`n$($readBuffer.TrimEnd())"

        # Generate write buffer (Pack)
        $writeBuffer = ""
        foreach ($field in $fields) {
            if ($field.IsArray -and $field.Type.EndsWith("[]")) {
                $writeBuffer += "`t`t`t// TODO: Array writing for $($field.PropertyName) - implement based on actual array size`r`n"
                $writeBuffer += "`t`t`t// foreach(var item in $($field.PropertyName)) bw.Write(item);`r`n"
            }
            else {
                $writeBuffer += "`t`t`tbw.Write($($field.PropertyName));`r`n"
            }
        }
        $csharpCode = $csharpCode -replace '\s*\*\*write buffer\*\*', "`r`n$($writeBuffer.TrimEnd())"

        # Generate ToString properties
        $toStringProperties = ""
        foreach ($field in $fields) {
            $toStringProperties += "`t`t`tret += `"`t$($field.PropertyName) = `";`r`n"
            $toStringProperties += "`t`t`ttry {`r`n"
            $toStringProperties += "`t`t`t`tret += `$`"{ Indentify($($field.PropertyName)) },\n`";`r`n"
            $toStringProperties += "`t`t`t} catch(NullReferenceException) {`r`n"
            $toStringProperties += "`t`t`t`tret += `"!!NULL!!\n`";`r`n"
            $toStringProperties += "`t`t`t}`r`n"
        }

        # Replace property template section - find and replace including surrounding whitespace
        $csharpCode = $csharpCode -replace '\s*\*\*property start\*\*[\s\S]*?\*\*property end\*\*', "`r`n$($toStringProperties.TrimEnd())`r`n`t`t`t"
    }

    return $csharpCode
}

function Add-FileToProject {
    param(
        [string]$FilePath,
        [string]$ProjectFile
    )

    $relativePath = "Packages\$([System.IO.Path]::GetFileName($FilePath))"

    # Read project file
    $projectContent = Get-Content $ProjectFile -Raw

    # Check if file already exists in project
    if ($projectContent -match [regex]::Escape($relativePath)) {
        Write-Log "    File already exists in project: $relativePath"
        return
    }

    # Find the Packages ItemGroup or create it
    if ($projectContent -match '(\s*<ItemGroup>\s*<Compile Include="Packages\\[^"]*" />\s*</ItemGroup>)') {
        # Add to existing Packages ItemGroup
        $existingGroup = $matches[1]
        $newEntry = "    <Compile Include=`"$relativePath`" />"
        $updatedGroup = $existingGroup -replace '(\s*</ItemGroup>)', "`r`n$newEntry`r`n`$1"
        $projectContent = $projectContent -replace [regex]::Escape($existingGroup), $updatedGroup
    }
    elseif ($projectContent -match '(\s*<ItemGroup>\s*<Compile Include="Packages\\Death\.cs" />\s*</ItemGroup>)') {
        # Add to existing ItemGroup with Death.cs
        $existingGroup = $matches[1]
        $newEntry = "    <Compile Include=`"$relativePath`" />"
        $updatedGroup = $existingGroup -replace '(\s*</ItemGroup>)', "`r`n$newEntry`r`n`$1"
        $projectContent = $projectContent -replace [regex]::Escape($existingGroup), $updatedGroup
    }
    else {
        # Create new ItemGroup (before the last </Project>)
        $newItemGroup = "  <ItemGroup>`r`n    <Compile Include=`"$relativePath`" />`r`n  </ItemGroup>`r`n"
        $projectContent = $projectContent -replace '(\s*</Project>)', "`r`n$newItemGroup`$1"
    }

    # Write updated project file
    [System.IO.File]::WriteAllText($ProjectFile, $projectContent, [System.Text.Encoding]::UTF8)
    Write-Log "    Added to project: $relativePath"
}

# Create output directories if they don't exist
$outputDir = Split-Path $OUTFILE -Parent
if (!(Test-Path $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir -Force
}

if (!(Test-Path $CSHARP_PACKAGES_DIR)) {
    New-Item -ItemType Directory -Path $CSHARP_PACKAGES_DIR -Force
}

# Initialize log file
"" | Out-File -FilePath $LOGFILE -Encoding UTF8
Write-Log "=== UF Opcode Extractor Script Started ===" "START"
Write-Log "Processing opcodes from $UF_OPS_H..."

# Load template file
if (!(Test-Path $TEMPLATE_FILE)) {
    Write-Log "Template file not found: $TEMPLATE_FILE" "ERROR"
    exit 1
}

$templateContent = Get-Content $TEMPLATE_FILE -Raw
Write-Log "Loaded template from: $TEMPLATE_FILE"

# Process opcodes
$opcodePattern = 'E\(OP_|D\(OP_'
$content = Get-Content $UF_OPS_H
$opcodes = $content | Select-String -Pattern $opcodePattern

$processedCount = 0
$outputContent = @()
$jsonData = @()
$generatedFiles = @()

# Add header to output content
$outputContent += "# Underfoot Client Opcodes and Structures"
$outputContent += ""
$outputContent += "This file lists all opcodes, their direction, numerical value, associated structure, the full structure definition, and the full ENCODE or DECODE section for the Underfoot client protocol."
$outputContent += ""

foreach ($match in $opcodes) {
    $line = $match.Line

    # Extract opcode and direction
    if ($line -match 'E\(OP_([A-Za-z0-9_]+)\)') {
        $OPCODE = "OP_$($matches[1])"
        $DIRECTION = "outgoing"
    }
    elseif ($line -match 'D\(OP_([A-Za-z0-9_]+)\)') {
        $OPCODE = "OP_$($matches[1])"
        $DIRECTION = "incoming"
    }
    else { continue }

    # Filter opcodes based on mode
    if ($OpcodeList.Count -gt 0) {
        # OpcodeList mode - only process specified opcodes
        if ($OPCODE -notin $OpcodeList) {
            continue
        }
    } elseif ($ExtractAll) {
        # ExtractAll mode - process all opcodes (no filtering)
        # Continue with processing
    }

    Write-Log "Processing $OPCODE ($DIRECTION)..."

    # Find opcode value from patch file
    $hexPattern = "^$OPCODE\s*="
    $hexLine = Get-Content $UF_CONF | Select-String -Pattern $hexPattern | Select-Object -First 1
    if ($hexLine -and $hexLine.Line -match '=(0x[0-9a-fA-F]+)') {
        $OPCODE_VALUE = $matches[1]
    }
    else {
        $OPCODE_VALUE = "(unknown)"
    }

    # Find structure name in uf.cpp
    $cppContent = Get-Content $UF_CPP -Raw
    $encodeDecodePattern = "(ENCODE\($OPCODE\)|DECODE\($OPCODE\)).*?\}"
    $codeBlock = [regex]::Match($cppContent, $encodeDecodePattern, [System.Text.RegularExpressions.RegexOptions]::Singleline)

    $STRUCT_NAME = "(unknown)"
    if ($codeBlock.Success) {
        $blockText = $codeBlock.Value
        # Look for structs:: pattern first
        if ($blockText -match 'structs::([A-Za-z0-9_]+)') {
            $STRUCT_NAME = $matches[1]
        }
        # Look for _Struct pattern
        elseif ($blockText -match '([A-Za-z0-9_]+_Struct)') {
            $STRUCT_NAME = $matches[1]
        }
    }

    # Extract structure definition
    $STRUCT_DEF = "Structure definition not found."
    $STRUCT_SOURCE_FILE = ""
    if ($STRUCT_NAME -ne "(unknown)") {
        $structsContent = Get-Content $UF_STRUCTS_H
        $structPattern = "struct\s+$STRUCT_NAME(\s|$)"

        for ($i = 0; $i -lt $structsContent.Count; $i++) {
            $line = $structsContent[$i]

            # Skip commented out lines to avoid confusion with multiple struct versions
            if ($line -match '^\s*//' -or $line -match '^\s*/\*') {
                continue
            }

            if ($line -match $structPattern) {
                $startLine = $i
                $STRUCT_SOURCE_FILE = $UF_STRUCTS_H

                # Find opening brace
                $braceIndex = $i
                if ($line -notmatch 'struct\s+' + [regex]::Escape($STRUCT_NAME) + '\s*\{') {
                    for ($j = $i + 1; $j -lt $structsContent.Count; $j++) {
                        if ($structsContent[$j] -match '^\s*\{') {
                            $braceIndex = $j
                            break
                        }
                    }
                }

                # Find closing brace, ensuring we don't include commented sections
                $braceCount = 0
                $endLine = -1
                for ($j = $braceIndex; $j -lt $structsContent.Count; $j++) {
                    $currentLine = $structsContent[$j]

                    # Skip commented lines
                    if ($currentLine -match '^\s*//' -or $currentLine -match '^\s*/\*') {
                        continue
                    }

                    # Count braces
                    $openBraces = ($currentLine -split '\{').Count - 1
                    $closeBraces = ($currentLine -split '\}').Count - 1
                    $braceCount += $openBraces - $closeBraces

                    if ($j -eq $braceIndex) { $braceCount = 1 } # Initialize for first line with opening brace

                    if ($braceCount -eq 0 -and $j -gt $braceIndex) {
                        $endLine = $j
                        break
                    }
                }

                if ($endLine -ge 0) {
                    $STRUCT_DEF = ($structsContent[$startLine..$endLine] -join "`n")
                }
                break
            }
        }
    }

    # Extract ENCODE/DECODE section
    if ($DIRECTION -eq "outgoing") {
        $CODE_SECTION = Extract-Block $UF_CPP "ENCODE" $OPCODE
    }
    else {
        $CODE_SECTION = Extract-Block $UF_CPP "DECODE" $OPCODE
    }

    if ([string]::IsNullOrWhiteSpace($CODE_SECTION)) {
        $CODE_SECTION = "Section not found."
    }


    # Generate C# struct file for ALL opcodes
    Write-Log "    Generating C# struct for $OPCODE..."
    $csharpCode = Generate-CSharpStruct $OPCODE $STRUCT_DEF $templateContent $CODE_SECTION

    if ($csharpCode) {
        $className = $OPCODE -replace '^OP_', ''
        $className = $className -replace '_', ''
        $fileName = "$className.cs"
        $filePath = Join-Path $CSHARP_PACKAGES_DIR $fileName

        try {
            [System.IO.File]::WriteAllText($filePath, $csharpCode, [System.Text.Encoding]::UTF8)
            Write-Log "    Created C# file: $fileName"
            $generatedFiles += $filePath

            # Note: .NET SDK automatically includes .cs files, no need to add to project file
        }
        catch {
            Write-Log "    Error creating C# file: $($_.Exception.Message)" "ERROR"
        }
    }
    else {
        Write-Log "    Could not generate C# code for $OPCODE" "ERROR"
    }

    # Add to JSON data array for reporting
    $opcodeObj = @{
        "opcode" = $OPCODE
        "direction" = $DIRECTION
        "value" = $OPCODE_VALUE
        "structure_name" = $STRUCT_NAME
        "structure_definition" = $STRUCT_DEF
        "encode_decode_section" = $CODE_SECTION
        "csharp_file_generated" = if ($STRUCT_NAME -ne "(unknown)" -and $STRUCT_DEF -ne "Structure definition not found.") { $true } else { $false }
        "source_files" = @{
            "opcode_definition" = $UF_OPS_H
            "opcode_value" = $UF_CONF
            "structure_definition" = if (![string]::IsNullOrWhiteSpace($STRUCT_SOURCE_FILE)) { $STRUCT_SOURCE_FILE } else { "not found" }
            "encode_decode_section" = $UF_CPP
        }
    }
    $jsonData += $opcodeObj

    # Add to output content array
    $outputContent += "## $OPCODE ($OPCODE_VALUE)"
    $outputContent += "- **Direction:** $DIRECTION"
    $outputContent += "- **Structure:** $STRUCT_NAME"
    $outputContent += ""
    $outputContent += "**Source Files:**"
    $outputContent += "- Opcode Definition: $UF_OPS_H"
    $outputContent += "- Opcode Value: $UF_CONF"
    $outputContent += "- Structure Definition: $UF_STRUCTS_H"
    $outputContent += "- Encode/Decode Section: $UF_CPP"
    $outputContent += ""
    $outputContent += "**Structure Definition:**"
    $outputContent += "``````cpp"
    $outputContent += $STRUCT_DEF
    $outputContent += "``````"
    $outputContent += ""
    $outputContent += "**Full $DIRECTION Section:**"
    $outputContent += "``````cpp"
    $outputContent += $CODE_SECTION
    $outputContent += "``````"
    $outputContent += ""
    $outputContent += "---"
    $outputContent += ""

    $processedCount++
}

Write-Log "Processing complete! Processed $processedCount opcodes."

if ($processedCount -eq 0) {
    Write-Log "No opcodes were processed. Check input files." "ERROR"
    exit
}

# Check for unused opcodes in patch file
Write-Log "Checking for unused opcodes in patch file..."
$patchContent = Get-Content $UF_CONF
$allPatchOpcodes = @()
$usedOpcodes = @()

foreach ($line in $patchContent) {
    if ($line -match '^(OP_[A-Za-z0-9_]+)\s*=') {
        $allPatchOpcodes += $matches[1]
    }
}

foreach ($item in $jsonData) {
    $usedOpcodes += $item.opcode
}

$unusedOpcodes = $allPatchOpcodes | Where-Object { $_ -notin $usedOpcodes }

Write-Log "Found $($allPatchOpcodes.Count) total opcodes in patch file"
Write-Log "Used $($usedOpcodes.Count) opcodes in encode/decode sections"
Write-Log "Found $($unusedOpcodes.Count) unused opcodes"

if ($unusedOpcodes.Count -gt 0) {
    Write-Log "Processing direct packet handler opcodes..."

    # Define additional source files to search
    $CLIENT_PACKET_CPP = "C:\Users\stecoc\git\Server\zone\client_packet.cpp"
    $CLIENT_PACKET_H = "C:\Users\stecoc\git\Server\zone\client_packet.h"
    $CLIENT_CPP = "C:\Users\stecoc\git\Server\zone\client.cpp"

    # Get client_packet.cpp content for searching
    $clientPacketContent = Get-Content $CLIENT_PACKET_CPP -Raw
    $clientPacketHeaderContent = Get-Content $CLIENT_PACKET_H -Raw
    $clientContent = Get-Content $CLIENT_CPP -Raw

    # Add direct handler opcodes to JSON data
    foreach ($unusedOpcode in $unusedOpcodes) {
        # Get the opcode value from patch file
        $hexLine = Get-Content $UF_CONF | Select-String -Pattern "^$unusedOpcode\s*=" | Select-Object -First 1
        if ($hexLine -and $hexLine.Line -match '=(0x[0-9a-fA-F]+)') {
            $opcodeValue = $matches[1]
        }
        else {
            $opcodeValue = "(unknown)"
        }

        # Skip opcodes with value 0x0000 (placeholders/disabled)
        if ($opcodeValue -eq "0x0000") {
            Write-Log "  Skipping $unusedOpcode (value: $opcodeValue - placeholder/disabled)"
            continue
        }

        Write-Log "  Processing direct handler opcode: $unusedOpcode (value: $opcodeValue)"

        # Look for handler function by checking opcode assignments first
        $handlerFunction = ""
        $handlerDirection = "unknown"
        $actualHandlerName = ""

        # Check ConnectedOpcodes array for handler assignment
        if ($clientPacketContent -match "ConnectedOpcodes\[\s*$unusedOpcode\s*\]\s*=\s*&Client::([A-Za-z0-9_]+)") {
            $actualHandlerName = $matches[1]
            $handlerDirection = "incoming (connected)"
            Write-Log "    Found ConnectedOpcodes assignment: $actualHandlerName"
        }
        # Check ConnectingOpcodes array for handler assignment
        elseif ($clientPacketContent -match "ConnectingOpcodes\[\s*$unusedOpcode\s*\]\s*=\s*&Client::([A-Za-z0-9_]+)") {
            $actualHandlerName = $matches[1]
            $handlerDirection = "incoming (connecting)"
            Write-Log "    Found ConnectingOpcodes assignment: $actualHandlerName"
        }
        else {
            # Fall back to standard naming convention
            $actualHandlerName = "Handle_$unusedOpcode"
            Write-Log "    No opcode assignment found, trying standard naming: $actualHandlerName"
        }

        # Now extract the actual handler function
        if (![string]::IsNullOrWhiteSpace($actualHandlerName)) {
            # Check if handler exists in client_packet.h
            if ($clientPacketHeaderContent -match "void\s+$actualHandlerName\s*\(") {
                Write-Log "    Found handler declaration for $actualHandlerName"
                # Extract the handler function from client_packet.cpp
                $handlerFunction = Extract-Block $CLIENT_PACKET_CPP "void Client::$actualHandlerName" ""

                if (![string]::IsNullOrWhiteSpace($handlerFunction)) {
                    Write-Log "    Handler function extracted successfully"
                }
                else {
                    Write-Log "    Handler function not found in implementation"
                }
            }
            else {
                Write-Log "    No handler declaration found for $actualHandlerName"
            }
        }

        # If still no direction found, try to determine from context
        if ($handlerDirection -eq "unknown") {
            if ($clientPacketContent -match "ConnectingOpcodes\[\s*$unusedOpcode\s*\]") {
                $handlerDirection = "incoming (connecting)"
                Write-Host "    Direction: incoming (connecting)"
            }
            elseif ($clientPacketContent -match "ConnectedOpcodes\[\s*$unusedOpcode\s*\]") {
                $handlerDirection = "incoming (connected)"
                Write-Host "    Direction: incoming (connected)"
            }
            else {
                $handlerDirection = "not assigned"
                Write-Host "    Direction: not assigned to opcode arrays"
            }
        }
        else {
            Write-Host "    Direction: $handlerDirection"
        }

        # Look for structures used with this opcode across the codebase
        $structureInfo = ""
        $structureName = "(no specific structure)"
        $sourceFile = ""

        # Search for EQApplicationPacket creation with this opcode in client_packet.cpp first
        $packetCreationPattern = "EQApplicationPacket.*$unusedOpcode.*sizeof\(([^)]+)\)"
        if ($clientPacketContent -match $packetCreationPattern) {
            $structureName = $matches[1]
            $sourceFile = $CLIENT_PACKET_CPP
            Write-Host "    Found structure in client_packet.cpp: $structureName"
        }

        # Search for struct casts in handler
        if (![string]::IsNullOrWhiteSpace($handlerFunction)) {
            # Look for app->pBuffer casts
            if ($handlerFunction -match "\(\s*([A-Za-z0-9_]+)\s*\*\s*\)\s*app->pBuffer") {
                $structureName = $matches[1]
                $sourceFile = $CLIENT_PACKET_CPP
                Write-Host "    Found structure in handler app->pBuffer cast: $structureName"
            }
            # Look for other structure patterns in handler
            elseif ($handlerFunction -match "([A-Za-z0-9_]+_Struct)\s+\*\s*[a-zA-Z_][a-zA-Z0-9_]*\s*=") {
                $structureName = $matches[1]
                $sourceFile = $CLIENT_PACKET_CPP
                Write-Host "    Found structure variable in handler: $structureName"
            }
            # Look for sizeof patterns in handler
            elseif ($handlerFunction -match "sizeof\s*\(\s*([A-Za-z0-9_]+_Struct)\s*\)") {
                $structureName = $matches[1]
                $sourceFile = $CLIENT_PACKET_CPP
                Write-Host "    Found structure in sizeof in handler: $structureName"
            }
            # Look for EQApplicationPacket creation in handler
            elseif ($handlerFunction -match "EQApplicationPacket.*sizeof\s*\(\s*([A-Za-z0-9_]+)\s*\)") {
                $structureName = $matches[1]
                $sourceFile = $CLIENT_PACKET_CPP
                Write-Host "    Found structure in packet creation in handler: $structureName"
            }
        }

        # If no structure found yet, search in client.cpp specifically
        if ($structureName -eq "(no specific structure)") {
            Write-Host "    Searching client.cpp for $unusedOpcode usage..."

            # Look for EQApplicationPacket creation patterns in client.cpp
            if ($clientContent -match "EQApplicationPacket.*$unusedOpcode.*sizeof\(([^)]+)\)") {
                $structureName = $matches[1]
                $sourceFile = $CLIENT_CPP
                Write-Host "    Found structure in client.cpp: $structureName"
            }
            # Look for direct opcode usage with structures in client.cpp
            elseif ($clientContent -match "$unusedOpcode.*\{[^}]*([A-Za-z0-9_]+_Struct)[^}]*\}") {
                $structureName = $matches[1]
                $sourceFile = $CLIENT_CPP
                Write-Host "    Found structure pattern in client.cpp: $structureName"
            }
            # Look for struct definitions near opcode usage in client.cpp
            elseif ($clientContent -match "$unusedOpcode") {
                Write-Host "    Found opcode usage in client.cpp, looking for nearby structures..."
                # Found opcode usage, look for nearby struct casts or definitions
                $lines = $clientContent -split "`n"
                for ($i = 0; $i -lt $lines.Count; $i++) {
                    if ($lines[$i] -match $unusedOpcode) {
                        # Look in surrounding lines for struct patterns
                        $startSearch = [Math]::Max(0, $i - 15)
                        $endSearch = [Math]::Min($lines.Count - 1, $i + 15)

                        for ($j = $startSearch; $j -le $endSearch; $j++) {
                            if ($lines[$j] -match "\(\s*([A-Za-z0-9_]+_Struct)\s*\*\s*\)") {
                                $structureName = $matches[1]
                                $sourceFile = $CLIENT_CPP
                                Write-Host "    Found structure near opcode usage in client.cpp: $structureName"
                                break
                            }
                            if ($lines[$j] -match "([A-Za-z0-9_]+_Struct)\s+\*?\s*[a-zA-Z_][a-zA-Z0-9_]*\s*=") {
                                $structureName = $matches[1]
                                $sourceFile = $CLIENT_CPP
                                Write-Host "    Found structure variable in client.cpp: $structureName"
                                break
                            }
                            if ($lines[$j] -match "sizeof\s*\(\s*([A-Za-z0-9_]+_Struct)\s*\)") {
                                $structureName = $matches[1]
                                $sourceFile = $CLIENT_CPP
                                Write-Host "    Found structure in sizeof in client.cpp: $structureName"
                                break
                            }
                        }
                        if ($structureName -ne "(no specific structure)") { break }
                    }
                }
            }
        }

        # If still no structure found, search across entire Server codebase
        if ($structureName -eq "(no specific structure)") {
            Write-Host "    Searching entire codebase for $unusedOpcode usage..."

            # Get all .cpp and .h files in Server directory
            $serverFiles = Get-ChildItem "C:\Users\stecoc\git\Server" -Recurse -Include "*.cpp", "*.h" | Where-Object { $_.Name -notlike ".*" }

            foreach ($file in $serverFiles) {
                try {
                    $fileContent = Get-Content $file.FullName -Raw -ErrorAction SilentlyContinue

                    # Look for EQApplicationPacket creation patterns
                    if ($fileContent -match "EQApplicationPacket.*$unusedOpcode.*sizeof\(([^)]+)\)") {
                        $structureName = $matches[1]
                        $sourceFile = $file.FullName
                        Write-Host "    Found structure in $($file.Name): $structureName"
                        break
                    }

                    # Look for direct opcode usage with structures
                    if ($fileContent -match "$unusedOpcode.*\{[^}]*([A-Za-z0-9_]+_Struct)[^}]*\}") {
                        $structureName = $matches[1]
                        $sourceFile = $file.FullName
                        Write-Host "    Found structure pattern in $($file.Name): $structureName"
                        break
                    }

                    # Look for struct definitions near opcode usage
                    if ($fileContent -match "$unusedOpcode") {
                        # Found opcode usage, look for nearby struct casts or definitions
                        $lines = $fileContent -split "`n"
                        for ($i = 0; $i -lt $lines.Count; $i++) {
                            if ($lines[$i] -match $unusedOpcode) {
                                # Look in surrounding lines for struct patterns
                                $startSearch = [Math]::Max(0, $i - 10)
                                $endSearch = [Math]::Min($lines.Count - 1, $i + 10)

                                for ($j = $startSearch; $j -le $endSearch; $j++) {
                                    if ($lines[$j] -match "\(\s*([A-Za-z0-9_]+_Struct)\s*\*\s*\)") {
                                        $structureName = $matches[1]
                                        $sourceFile = $file.FullName
                                        Write-Host "    Found structure near opcode usage in $($file.Name): $structureName"
                                        break
                                    }
                                    if ($lines[$j] -match "([A-Za-z0-9_]+_Struct)\s+\*?\s*[a-zA-Z_][a-zA-Z0-9_]*\s*=") {
                                        $structureName = $matches[1]
                                        $sourceFile = $file.FullName
                                        Write-Host "    Found structure variable in $($file.Name): $structureName"
                                        break
                                    }
                                }
                                if ($structureName -ne "(no specific structure)") { break }
                            }
                        }
                        if ($structureName -ne "(no specific structure)") { break }
                    }
                }
                catch {
                    # Skip files that can't be read
                    continue
                }
            }
        }

        if ($structureName -eq "(no specific structure)") {
            Write-Host "    No specific structure found after comprehensive search"
        }

        # Find structure definition if we found a structure name
        $structureDefinition = "Structure definition not found."
        $structSourceFile = ""
        if ($structureName -ne "(no specific structure)" -and $structureName -ne "(not implemented)") {
            Write-Host "    Searching for structure definition: $structureName"
            # Search in uf_structs.h and common eq_packet_structs.h
            $structsFiles = @($UF_STRUCTS_H, "C:\Users\stecoc\git\Server\common\eq_packet_structs.h")

            foreach ($structFile in $structsFiles) {
                if (Test-Path $structFile) {
                    $structsContent = Get-Content $structFile
                    $structPattern = "struct\s+$structureName(\s|\{|$)"

                    for ($i = 0; $i -lt $structsContent.Count; $i++) {
                        # Skip commented lines when looking for struct definitions
                        if ($structsContent[$i] -match '^\s*//.*struct\s+' + [regex]::Escape($structureName) + '(\s|$)') {
                            continue
                        }

                        if ($structsContent[$i] -match $structPattern) {
                            $startLine = $i

                            # Find opening brace
                            $braceIndex = $i
                            if ($structsContent[$i] -notmatch 'struct\s+' + [regex]::Escape($structureName) + '\s*\{') {
                                for ($j = $i + 1; $j -lt $structsContent.Count; $j++) {
                                    if ($structsContent[$j] -match '^\s*\{') {
                                        $braceIndex = $j
                                        break
                                    }
                                }
                            }

                            # Find closing brace
                            for ($j = $braceIndex + 1; $j -lt $structsContent.Count; $j++) {
                                if ($structsContent[$j] -match '^\s*\};') {
                                    $endLine = $j
                                    $structureDefinition = ($structsContent[$startLine..$endLine] -join "`n")
                                    $structSourceFile = $structFile
                                    Write-Host "    Structure definition found in: $structFile"
                                    break
                                }
                            }
                            break
                        }
                    }
                    if ($structureDefinition -ne "Structure definition not found.") { break }
                }
            }
        }

        if ([string]::IsNullOrWhiteSpace($handlerFunction)) {
            $handlerFunction = "Handler function not found."
        }

        # Generate C# struct file for ALL direct handler opcodes
        Write-Log "    Generating C# struct for direct handler $unusedOpcode..."
        $csharpCode = Generate-CSharpStruct $unusedOpcode $structureDefinition $templateContent $handlerFunction

        if ($csharpCode) {
            $className = $unusedOpcode -replace '^OP_', ''
            $className = $className -replace '_', ''
            $fileName = "$className.cs"
            $filePath = Join-Path $CSHARP_PACKAGES_DIR $fileName

            try {
                [System.IO.File]::WriteAllText($filePath, $csharpCode, [System.Text.Encoding]::UTF8)
                Write-Log "    Created C# file: $fileName"
                $generatedFiles += $filePath

                # Note: .NET SDK automatically includes .cs files, no need to add to project file
            }
            catch {
                Write-Log "    Error creating C# file: $($_.Exception.Message)" "ERROR"
            }
        }
        else {
            Write-Log "    Could not generate C# code for $unusedOpcode" "ERROR"
        }

        $directHandlerObj = @{
            "opcode" = $unusedOpcode
            "direction" = $handlerDirection
            "value" = $opcodeValue
            "structure_name" = $structureName
            "structure_definition" = $structureDefinition
            "handler_function" = $handlerFunction
            "implementation_type" = "direct_packet_handler"
            "csharp_file_generated" = if ($structureName -ne "(no specific structure)" -and $structureName -ne "(not implemented)" -and $structureDefinition -ne "Structure definition not found.") { $true } else { $false }
            "source_files" = @{
                "opcode_definition" = $CLIENT_PACKET_H
                "opcode_value" = $UF_CONF
                "structure_definition" = if ($structureDefinition -ne "Structure definition not found." -and ![string]::IsNullOrWhiteSpace($structSourceFile)) { $structSourceFile } else { "(not found)" }
                "handler_function" = $CLIENT_PACKET_CPP
                "structure_usage" = if (![string]::IsNullOrWhiteSpace($sourceFile)) { $sourceFile } else { "(not found)" }
            }
        }
        $jsonData += $directHandlerObj
    }

    # Add direct handler opcodes section to markdown
    $outputContent += ""
    $outputContent += "# Direct Packet Handler Opcodes"
    $outputContent += ""
    $outputContent += "The following opcodes are defined in the patch file and use direct packet handling instead of ENCODE/DECODE:"
    $outputContent += ""

    foreach ($unusedOpcode in $unusedOpcodes) {
        # Get the opcode value from patch file
        $hexLine = Get-Content $UF_CONF | Select-String -Pattern "^$unusedOpcode\s*=" | Select-Object -First 1
        if ($hexLine -and $hexLine.Line -match '=(0x[0-9a-fA-F]+)') {
            $opcodeValue = $matches[1]
        }
        else {
            $opcodeValue = "(unknown)"
        }

        # Skip opcodes with value 0x0000 (placeholders/disabled)
        if ($opcodeValue -eq "0x0000") {
            continue
        }

        # Find the corresponding entry in jsonData for this opcode
        $opcodeData = $jsonData | Where-Object { $_.opcode -eq $unusedOpcode -and $_.implementation_type -eq "direct_packet_handler" } | Select-Object -First 1

        if ($opcodeData) {
            $outputContent += "## $unusedOpcode ($opcodeValue)"
            $outputContent += "- **Direction:** $($opcodeData.direction)"
            $outputContent += "- **Structure:** $($opcodeData.structure_name)"
            $outputContent += "- **Implementation:** Direct packet handler"
            $outputContent += ""
            $outputContent += "**Source Files:**"
            $outputContent += "- Opcode Definition: $($opcodeData.source_files.opcode_definition)"
            $outputContent += "- Opcode Value: $($opcodeData.source_files.opcode_value)"
            $outputContent += "- Structure Definition: $($opcodeData.source_files.structure_definition)"
            $outputContent += "- Handler Function: $($opcodeData.source_files.handler_function)"
            $outputContent += ""
            $outputContent += "**Structure Definition:**"
            $outputContent += "``````cpp"
            $outputContent += $opcodeData.structure_definition
            $outputContent += "``````"
            $outputContent += ""
            $outputContent += "**Handler Function:**"
            $outputContent += "``````cpp"
            $outputContent += $opcodeData.handler_function
            $outputContent += "``````"
            $outputContent += ""
            $outputContent += "---"
            $outputContent += ""
        }
    }
}

Write-Log "Writing markdown output to: $OUTFILE"

try {
    # Write markdown content to file with Windows line endings
    $markdownText = ($outputContent -join "`r`n")
    [System.IO.File]::WriteAllText($OUTFILE, $markdownText, [System.Text.Encoding]::UTF8)
    Write-Log "Markdown file written successfully!"
}
catch {
    Write-Log "Error writing markdown file: $($_.Exception.Message)" "ERROR"
}

Write-Log "C# File Generation Summary:"
Write-Log "Generated $($generatedFiles.Count) C# struct files"
foreach ($file in $generatedFiles) {
    $fileName = [System.IO.Path]::GetFileName($file)
    Write-Log "  - $fileName"
}

# Analyze and report missing information
Write-Log ""
Write-Log "=== MISSING INFORMATION ANALYSIS ===" "ANALYSIS"

$noStructures = @()
$noHandlers = @()
$completelyMissing = @()

foreach ($item in $jsonData) {
    if ($item.implementation_type -eq "direct_packet_handler") {
        $hasStructure = $item.structure_name -ne "(no specific structure)" -and $item.structure_name -ne "(not implemented)"
        $hasHandler = $item.handler_function -ne "Handler function not found." -and ![string]::IsNullOrWhiteSpace($item.handler_function)

        if (!$hasStructure -and !$hasHandler) {
            $completelyMissing += "$($item.opcode) ($($item.value))"
        }
        elseif (!$hasStructure) {
            $noStructures += "$($item.opcode) ($($item.value))"
        }
        elseif (!$hasHandler) {
            $noHandlers += "$($item.opcode) ($($item.value))"
        }
    }
}

Write-Log ""
Write-Log "SUMMARY OF MISSING INFORMATION:"
Write-Log "=============================="
Write-Log "Total direct handler opcodes processed: $(($jsonData | Where-Object { $_.implementation_type -eq "direct_packet_handler" }).Count)"
Write-Log ""

if ($completelyMissing.Count -gt 0) {
    Write-Log "OPCODES WITH NO HANDLER AND NO STRUCTURE ($($completelyMissing.Count)):" "WARN"
    Write-Log "These opcodes have no detectable implementation:"
    foreach ($opcode in $completelyMissing) {
        Write-Log "  - $opcode"
    }
    Write-Log ""
}

if ($noStructures.Count -gt 0) {
    Write-Log "OPCODES WITH HANDLER BUT NO STRUCTURE ($($noStructures.Count)):" "WARN"
    Write-Log "These opcodes have handlers but no detectable structures:"
    foreach ($opcode in $noStructures) {
        Write-Log "  - $opcode"
    }
    Write-Log ""
}

if ($noHandlers.Count -gt 0) {
    Write-Log "OPCODES WITH STRUCTURE BUT NO HANDLER ($($noHandlers.Count)):" "WARN"
    Write-Log "These opcodes have structures but no detectable handlers:"
    foreach ($opcode in $noHandlers) {
        Write-Log "  - $opcode"
    }
    Write-Log ""
}

if ($completelyMissing.Count -eq 0 -and $noStructures.Count -eq 0 -and $noHandlers.Count -eq 0) {
    Write-Log "All processed opcodes have either handlers or structures (or both)!" "INFO"
}

Write-Log ""
Write-Log "INVESTIGATION PRIORITY:"
Write-Log "====================="
Write-Log "1. HIGH PRIORITY - Completely missing opcodes: $($completelyMissing.Count)"
Write-Log "2. MEDIUM PRIORITY - Missing structures: $($noStructures.Count)"
Write-Log "3. LOW PRIORITY - Missing handlers: $($noHandlers.Count)"
Write-Log ""

# Add missing information section to markdown output
$outputContent += ""
$outputContent += "# Missing Information Summary"
$outputContent += ""
$outputContent += "This section identifies opcodes that may need investigation for missing implementations."
$outputContent += ""

if ($completelyMissing.Count -gt 0) {
    $outputContent += "## High Priority - No Handler and No Structure"
    $outputContent += ""
    $outputContent += "These opcodes have no detectable implementation and should be investigated first:"
    $outputContent += ""
    foreach ($opcode in $completelyMissing) {
        $outputContent += "- $opcode"
    }
    $outputContent += ""
}

if ($noStructures.Count -gt 0) {
    $outputContent += "## Medium Priority - Missing Structures"
    $outputContent += ""
    $outputContent += "These opcodes have handlers but no detectable structures:"
    $outputContent += ""
    foreach ($opcode in $noStructures) {
        $outputContent += "- $opcode"
    }
    $outputContent += ""
}

if ($noHandlers.Count -gt 0) {
    $outputContent += "## Low Priority - Missing Handlers"
    $outputContent += ""
    $outputContent += "These opcodes have structures but no detectable handlers:"
    $outputContent += ""
    foreach ($opcode in $noHandlers) {
        $outputContent += "- $opcode"
    }
    $outputContent += ""
}


Write-Log "=== Script Completed Successfully ===" "END"
Write-Log "Output locations:"
Write-Log "Markdown: $OUTFILE"
Write-Log "C# Files: $CSHARP_PACKAGES_DIR"
Write-Log "Project Updated: $CSPROJ_FILE"
Write-Log "Log: $LOGFILE"
Write-Log ""
Write-Log "Generated $($generatedFiles.Count) C# struct files in total"

# Report on processing mode
if ($OpcodeList.Count -gt 0) {
    Write-Log "Selective mode: Processed $($OpcodeList.Count) specified opcodes: $($OpcodeList -join ', ')"
} elseif ($ExtractAll) {
    Write-Log "Extract-all mode: Processed all available opcodes"
}