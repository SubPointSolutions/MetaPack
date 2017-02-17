$PSScriptRoot = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition
cd $PSScriptRoot

.\build.ps1 -Verbosity Minimal -Target "Default-Appveyor"