Param(
  [string]$workingFolderPath
)

if([string]::IsNullOrEmpty($workingFolderPath) -eq $true) {
	Write-Host "Installing the latest choco package globally"
}
else {
	Write-Host "Installing the latest choco package in folder:[$workingFolderPath]"
}

$PSScriptRoot = ""

Write-Host "Looking for the current folder..."

if( $MyInvocation -ne $null -and $MyInvocation.MyCommand -ne $null)
{
    $PSScriptRoot = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition
}

Write-Host "Current folder is:[$PSScriptRoot]"

$packagesFolder = ($PSScriptRoot + "\" + "build-artifact-cli-packages")
Write-Host "Looking for package in folder:[$packagesFolder]"
$packages = Get-ChildItem -Path $packagesFolder -Filter *.nupkg

Write-Host "Found [$($packages.Count)] packages"

if($packages.Count -eq 0) {
    throw "No packages to install"
}

$latestPackage = $packages | Sort-Object { $_.Name } -Descending | Select-Object -First 1
$latestPackageVersion = [System.IO.Path]::GetFileNameWithoutExtension($latestPackage).Replace("MetaPack.", "")


Write-Host "Uninstalling MetaPack CLI..." -fore Green    
choco uninstall metapack --force

Write-Host "Updating MetaPack CLI package to [$latestPackageVersion]" -fore Green
choco install metapack --source $packagesFolder --force --pre --version $latestPackageVersion

metapack version