. "Pester\helpers.ps1"

Describe "metapack.cli.core" {
  
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

    # # spmeta2 provider runs for O365
    # It "Can instsall SPMeta2.CI package" {

    #     $args = @("install", 
    #               "--id  $m2PackageId",
    #               "--url  $siteUrl"
    #               "--username", $userName,
    #               "--userpassword", $userPassword
    #               "--spversion", "o365",
    #               "--source", $nugetSource
    #               "--verbose"
    #               )

    #     $result = (RunMetaPackCLI $args)

    #     $output = $result.Output
    #     $exitCode = $result.ExitCode
        
    #     # no exception in output
    #     (OutputHasError $output) | Should Be $false
        
    #     # exist code, please 0
    #     ($exitCode) | Should Be 0
    # }

    # It "Can instsall SPMeta2.CI package --force" {
        
    #     $args = @("install", 
    #               "--id  $m2PackageId",
    #               "--url  $siteUrl"
    #               "--username", $userName,
    #               "--userpassword", $userPassword
    #               "--spversion", "o365",
    #               "--source", $nugetSource
    #               "--verbose",
    #               "--force"
    #               )

    #     $result = (RunMetaPackCLI $args)

    #     $output = $result.Output
    #     $exitCode = $result.ExitCode

    #     # no exception in output
    #     (OutputHasError $output) | Should Be $false
        
    #     # exist code, please 0
    #     ($exitCode) | Should Be 0
    # }

    # # SharePointPno provider runs for O365

    # It "Can instsall SharePointPnP.CI package" {

    #     $args = @("install", 
    #               "--id  $spPnPPackageId",
    #               "--url  $siteUrl"
    #               "--username", $userName,
    #               "--userpassword", $userPassword
    #               "--spversion", "o365",
    #               "--source", $nugetSource
    #               "--verbose"
    #               )

    #     $result = (RunMetaPackCLI $args)

    #     $output = $result.Output
    #     $exitCode = $result.ExitCode
        
    #     # no exception in output
    #     (OutputHasError $output) | Should Be $false
        
    #     # exist code, please 0
    #     ($exitCode) | Should Be 0
    # }

    # It "Can instsall SharePointPnP.CI package --force" {
        
    #     $args = @("install", 
    #               "--id  $spPnPPackageId",
    #               "--url  $siteUrl"
    #               "--username", $userName,
    #               "--userpassword", $userPassword
    #               "--spversion", "o365",
    #               "--source", $nugetSource
    #               "--verbose",
    #               "--force"
    #               )

    #     $result = (RunMetaPackCLI $args)

    #     $output = $result.Output
    #     $exitCode = $result.ExitCode

    #     # no exception in output
    #     (OutputHasError $output) | Should Be $false
        
    #     # exist code, please 0
    #     ($exitCode) | Should Be 0
    # }
}