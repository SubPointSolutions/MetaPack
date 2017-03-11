$m2PackageId = "MetaPack.SPMeta2.CI"
$spPnPPackageId = "MetaPack.SharePointPnP.CI"

$nugetSource = "https://www.myget.org/F/subpointsolutions-ci/api/v2"

Write-Host "Reading environment variables..." -fore Green

function GetGlobalEnvironmentVariable($name) {
    
	$result = [System.Environment]::GetEnvironmentVariable($name, "Process");

    if([String]::IsNullOrEmpty($result) -eq $true) 
	{
        $result = System.Environment.GetEnvironmentVariable($name, "User");
	}

    if([String]::IsNullOrEmpty($result) -eq $true)
	{
        $result = System.Environment.GetEnvironmentVariable($name, "Machine");
	}

    return $result;
}

$siteUrl = GetGlobalEnvironmentVariable "MetaPack.SharePoint.O365.RootWebUrl"
if([string]::IsNullOrEmpty($siteUrl) -eq $true) {
    throw "o365 siteUrl is null or empty"
}

$userName =  GetGlobalEnvironmentVariable "MetaPack.SharePoint.O365.UserName"
if([string]::IsNullOrEmpty($userName) -eq $true) {
    throw "o365 userName is null or empty"
}

$userPassword =  GetGlobalEnvironmentVariable "MetaPack.SharePoint.O365.UserPassword"
if([string]::IsNullOrEmpty($userPassword) -eq $true) {
    throw "o365 userPassword is null or empty"
}

$sp2013SiteUrl = GetGlobalEnvironmentVariable "MetaPack.SharePoint.SP2013.RootWebUrl"
if([string]::IsNullOrEmpty($sp2013SiteUrl) -eq $true) {
    throw "sp2013SiteUrl user name is null or empty"
}

$sp2013UserName = GetGlobalEnvironmentVariable "MetaPack.SharePoint.SP2013.UserName"
if([string]::IsNullOrEmpty($sp2013UserName) -eq $true) {
    throw "sp2013 user name is null or empty"
}

$sp2013UserPassword = GetGlobalEnvironmentVariable "MetaPack.SharePoint.SP2013.UserPassword"
if([string]::IsNullOrEmpty($sp2013UserPassword) -eq $true) {
    throw "sp2013 user password is null or empty"
}

function OutputHasError($output) {

    # not empty
    if([string]::IsNullOrEmpty($output) -eq $true) {
        return $true
    }

    # has no "Exception" message
    $result = $output.ToString().ToLower().Contains("exception") -eq $true

    return $result ;
}

function RunMetaPackCLI($processArgs, $useShellExecute) 
{
    $ProcessInfo = New-Object System.Diagnostics.ProcessStartInfo 
        $ProcessInfo.FileName = "metapack" 
        #$ProcessInfo.RedirectStandardError = $true 
        
        if($useShellExecute -eq $true)
        {
            $ProcessInfo.UseShellExecute = $true
        }            
        else
        {
            $ProcessInfo.RedirectStandardOutput = $true 
            $ProcessInfo.UseShellExecute = $false
        }
        
        $ProcessInfo.Arguments = $processArgs 
        $Process = New-Object System.Diagnostics.Process 
        $Process.StartInfo = $ProcessInfo 
        $Process.Start() | Out-Null 
        $Process.WaitForExit() 
        
        if($useShellExecute -eq $true)
        {
            
        }
        else
        {
            $output = $Process.StandardOutput.ReadToEnd() 
            Write-Host $output 
        }

        return @{
            Output = $output
            ExitCode = $Process.ExitCode
            UseShellExecute = $useShellExecute
        }
}
