$PSScriptRoot = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition
cd $PSScriptRoot

$packages = Get-ChildItem -Path $PSScriptRoot -Filter *.nupkg

$latestPackage = $packages | Sort-Object { $_.Name } -Descending | Select-Object -First 1
$latestPackageVersion = [System.IO.Path]::GetFileNameWithoutExtension($latestPackage).Replace("MetaPack.", "")

Write-Host "Updating MetaPack CLI package to [$latestPackageVersion]" -fore Green
choco install metapack --source $PSScriptRoot --force --pre --version $latestPackageVersion