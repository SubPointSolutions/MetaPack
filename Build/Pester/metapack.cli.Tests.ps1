Write-Host "Reading environment variables..." -fore Green

$userName =  [environment]::GetEnvironmentVariable("MetaPack.SharePoint.O365.UserName", "machine")
if([string]::IsNullOrEmpty($userName) -eq $true) {
    throw "userName is null or empty"
}

$userPassword =  [environment]::GetEnvironmentVariable("MetaPack.SharePoint.O365.UserPassword", "machine")
if([string]::IsNullOrEmpty($userPassword) -eq $true) {
    throw "userPassword is null or empty"
}

$siteUrl = [environment]::GetEnvironmentVariable("MetaPack.SharePoint.O365.RootWebUrl", "machine")
if([string]::IsNullOrEmpty($userPassword) -eq $true) {
    throw "siteUrl is null or empty"
}

$m2PackageId = "MetaPack.SPMeta2.CI"
$spPnPPackageId = "MetaPack.SharePointPnP.CI"

$nugetSource = "https://www.myget.org/F/subpointsolutions-ci/api/v2"

function OutputHasError($output) {

    # not empty
    if([string]::IsNullOrEmpty($output) -eq $true) {
        return $true
    }

    # has no "Exception" message
    $result = $output.ToString().ToLower().Contains("exception") -eq $true

    return $result ;
}

function RunMetaPackCLI($processArgs) 
{
    $ProcessInfo = New-Object System.Diagnostics.ProcessStartInfo 
        $ProcessInfo.FileName = "metapack" 
        #$ProcessInfo.RedirectStandardError = $true 
        $ProcessInfo.RedirectStandardOutput = $true 
        $ProcessInfo.UseShellExecute = $false
        $ProcessInfo.Arguments = $processArgs 
        $Process = New-Object System.Diagnostics.Process 
        $Process.StartInfo = $ProcessInfo 
        $Process.Start() | Out-Null 
        $Process.WaitForExit() 
        
        $output = $Process.StandardOutput.ReadToEnd() 
       
        Write-Host $output 

        return @{
            Output = $output
            ExitCode = $Process.ExitCode
        }
}

Describe "metapack.cli" {
  
    # generic tests
    It "Can run with no arguments" {
        
        $args = @()

        $result = (RunMetaPackCLI $args)

        $output = $result.Output
        $exitCode = $result.ExitCode

        # no exception in output
        (OutputHasError $output) | Should Be $false
        
        # exist code, please 0
        ($exitCode) | Should Be 1       

    }

    It "Can get version" {
        
        $args = @("version")

        $result = (RunMetaPackCLI $args)

        $output = $result.Output
        $exitCode = $result.ExitCode

        # no exception in output
        (OutputHasError $output) | Should Be $false
        
        # exist code, please 0
        ($exitCode) | Should Be 0
    }

    # spmeta2 provider runs for O365
    It "Can instsall SPMeta2.CI package" {

        $args = @("install", 
                  "--id  $m2PackageId",
                  "--url  $siteUrl"
                  "--username", $userName,
                  "--userpassword", $userPassword
                  "--spversion", "o365",
                  "--source", $nugetSource
                  "--verbose"
                  )

        $result = (RunMetaPackCLI $args)

        $output = $result.Output
        $exitCode = $result.ExitCode
        
        # no exception in output
        (OutputHasError $output) | Should Be $false
        
        # exist code, please 0
        ($exitCode) | Should Be 0
    }

    It "Can instsall SPMeta2.CI package --force" {
        
        $args = @("install", 
                  "--id  $m2PackageId",
                  "--url  $siteUrl"
                  "--username", $userName,
                  "--userpassword", $userPassword
                  "--spversion", "o365",
                  "--source", $nugetSource
                  "--verbose",
                  "--force"
                  )

        $result = (RunMetaPackCLI $args)

        $output = $result.Output
        $exitCode = $result.ExitCode

        # no exception in output
        (OutputHasError $output) | Should Be $false
        
        # exist code, please 0
        ($exitCode) | Should Be 0
    }

    # SharePointPno provider runs for O365

    It "Can instsall SharePointPnP.CI package" {

        $args = @("install", 
                  "--id  $spPnPPackageId",
                  "--url  $siteUrl"
                  "--username", $userName,
                  "--userpassword", $userPassword
                  "--spversion", "o365",
                  "--source", $nugetSource
                  "--verbose"
                  )

        $result = (RunMetaPackCLI $args)

        $output = $result.Output
        $exitCode = $result.ExitCode
        
        # no exception in output
        (OutputHasError $output) | Should Be $false
        
        # exist code, please 0
        ($exitCode) | Should Be 0
    }

    It "Can instsall SharePointPnP.CI package --force" {
        
        $args = @("install", 
                  "--id  $spPnPPackageId",
                  "--url  $siteUrl"
                  "--username", $userName,
                  "--userpassword", $userPassword
                  "--spversion", "o365",
                  "--source", $nugetSource
                  "--verbose",
                  "--force"
                  )

        $result = (RunMetaPackCLI $args)

        $output = $result.Output
        $exitCode = $result.ExitCode

        # no exception in output
        (OutputHasError $output) | Should Be $false
        
        # exist code, please 0
        ($exitCode) | Should Be 0
    }
}