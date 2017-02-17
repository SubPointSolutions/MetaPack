$PSScriptRoot = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition
cd $PSScriptRoot

.\build.ps1 -Target "Default-NuGet-Packaging" -Verbosity Minimal