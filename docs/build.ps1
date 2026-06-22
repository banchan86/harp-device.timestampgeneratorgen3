[CmdletBinding()] param (
    [string[]]$docfxArgs
)
Set-StrictMode -Version 3.0
$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $true

Push-Location $PSScriptRoot
try {
    # Harp schema processor
    $deviceYml = "..\device.yml"

    Write-Output "Generating schema tables for $deviceYml..."
    dotnet run --project .\harp-schema-processor $deviceYml .\apidoc

    $libPaths = @()
    $libPaths += Get-ChildItem "..\artifacts\bin\*\release_net4*" -Directory | Select-Object -Expand FullName
    $libPaths += "..\artifacts\package\release"

    ./export-images.ps1 $libPaths
    dotnet docfx metadata
    dotnet docfx build $docfxArgs
} finally {
    Pop-Location
}
