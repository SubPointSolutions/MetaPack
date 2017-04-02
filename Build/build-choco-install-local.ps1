Param(
  [Parameter(Mandatory=$true)]
  [string]$packageName
)

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

$latestPackage = $packages | Where-Object { $_.Name.StartsWith($packageName + ".") -eq $true } | Sort-Object { $_.Name } -Descending | Select-Object -First 1
$latestPackageVersion = [System.IO.Path]::GetFileNameWithoutExtension($latestPackage).Replace($packageName + ".", "")

Write-Host "Uninstalling [$packageName] package..." -fore Green    
choco uninstall $packageName --force

Write-Host "Updating [$packageName] package to [$latestPackageVersion]" -fore Green
choco install $packageName --source $packagesFolder --force --pre --version $latestPackageVersion

Write-Host "Running [$packageName version] command.." -fore Green
& $packageName version

if($lastexitcode -ne 0) {
	throw "Exit code:[$lastexitcode]. Expected [0]"
} else {
	Write-Host "Exit code:[$lastexitcode]. All good."
}