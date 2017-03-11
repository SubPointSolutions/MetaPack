
. "Pester\helpers.ps1"


Write-Host "Running CLI provisioning tests..." -fore Green

# O365
Describe "metapack.cli.provision.spmeta2.o365" {
  
    It "Can install [$m2PackageId] package" {

        $args = @("install", 
                  "--id  $m2PackageId",
                  "--url  $siteUrl"
                  "--username", $userName,
                  "--userpassword", $userPassword
                  "--spversion", "o365",
                  "--source", $nugetSource
                  "--verbose"
                  )

        $result = (RunMetaPackCLI $args $false)

        $output = $result.Output
        $exitCode = $result.ExitCode
        
        # no exception in output
        if($result.UseShellExecute -eq $false) {
            (OutputHasError $output) | Should Be $false
			#Write-Host $output
        }            
        
        # exist code, please 0
        ($exitCode) | Should Be 0
    }

    #It "Can install [$m2PackageId] package --force" {
        
    #    $args = @("install", 
    #              "--id  $m2PackageId",
    #              "--url  $siteUrl"
    #              "--username", $userName,
    #              "--userpassword", $userPassword
    #              "--spversion", "o365",
    #              "--source", $nugetSource
    #              "--verbose",
    #              "--force"
    #              )

    #    $result = (RunMetaPackCLI $args $true)

    #    $output = $result.Output
    #    $exitCode = $result.ExitCode

    #   # no exception in output
    #    if($result.UseShellExecute -eq $false) {
    #        (OutputHasError $output) | Should Be $false
    #    }     
        
    #    # exist code, please 0
    #    ($exitCode) | Should Be 0
    #}
}

Describe "metapack.cli.provision.pnp.o365" {
  
    It "Can install [$spPnPPackageId] package" {

        $args = @("install", 
                  "--id  $spPnPPackageId",
                  "--url  $siteUrl"
                  "--username", $userName,
                  "--userpassword", $userPassword
                  "--spversion", "o365",
                  "--source", $nugetSource
                  "--verbose"
                  )

        $result = (RunMetaPackCLI $args $true)

        $output = $result.Output
        $exitCode = $result.ExitCode
        
        # no exception in output
        if($result.UseShellExecute -eq $false) {
            (OutputHasError $output) | Should Be $false
        }            
        
        # exist code, please 0
        ($exitCode) | Should Be 0
    }

    It "Can install [$spPnPPackageId] package --force" {
        
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

        $result = (RunMetaPackCLI $args $true)

        $output = $result.Output
        $exitCode = $result.ExitCode

       # no exception in output
        if($result.UseShellExecute -eq $false) {
            (OutputHasError $output) | Should Be $false
        }     
        
        # exist code, please 0
        ($exitCode) | Should Be 0
    }
}

# SP2013 CSOM
Describe "metapack.cli.provision.spmeta2.sp2013" {
  
    It "Can install [$m2PackageId] package" {

        $args = @("install", 
                  "--id  $m2PackageId",
                  "--url  $sp2013SiteUrl",
                  "--username", $sp2013UserName,
                  "--userpassword", $sp2013UserPassword
                  "--spversion", "sp2013",
                  "--source", $nugetSource
                  "--verbose"
                  )

        $result = (RunMetaPackCLI $args $true)

        $output = $result.Output
        $exitCode = $result.ExitCode
        
        # no exception in output
        if($result.UseShellExecute -eq $false) {
            (OutputHasError $output) | Should Be $false
        }            
        
        # exist code, please 0
        ($exitCode) | Should Be 0
    }

    It "Can install [$m2PackageId] package --force" {
        
        $args = @("install", 
                  "--id  $m2PackageId",
                  "--url  $sp2013SiteUrl",
                  "--username", $sp2013UserName,
                  "--userpassword", $sp2013UserPassword
                  "--spversion", "sp2013",
                  "--source", $nugetSource
                  "--verbose",
                  "--force"
                  )

        $result = (RunMetaPackCLI $args $true)

        $output = $result.Output
        $exitCode = $result.ExitCode

       # no exception in output
        if($result.UseShellExecute -eq $false) {
            (OutputHasError $output) | Should Be $false
        }     
        
        # exist code, please 0
        ($exitCode) | Should Be 0
    }
}

Describe "metapack.cli.provision.pnp.sp2013" {
  
    It "Can install [$spPnPPackageId] package" {

        $args = @("install", 
                  "--id  $spPnPPackageId",
                  "--url  $sp2013SiteUrl"
                  "--username", $sp2013UserName,
                  "--userpassword", $sp2013UserPassword
                  "--spversion", "sp2013",
                  "--source", $nugetSource
                  "--verbose"
                  )

        $result = (RunMetaPackCLI $args $true)

        $output = $result.Output
        $exitCode = $result.ExitCode
        
        # no exception in output
        if($result.UseShellExecute -eq $false) {
            (OutputHasError $output) | Should Be $false
        }            
        
        # exist code, please 0
        ($exitCode) | Should Be 0
    }

    It "Can install [$spPnPPackageId] package --force" {
        
        $args = @("install", 
                  "--id  $spPnPPackageId",
                  "--url  $siteUrl"
                  "--username", $sp2013UserName,
                  "--userpassword", $sp2013UserPassword
                  "--spversion", "sp2013",
                  "--source", $nugetSource
                  "--verbose",
                  "--force"
                  )

        $result = (RunMetaPackCLI $args $true)

        $output = $result.Output
        $exitCode = $result.ExitCode

       # no exception in output
        if($result.UseShellExecute -eq $false) {
            (OutputHasError $output) | Should Be $false
        }     
        
        # exist code, please 0
        ($exitCode) | Should Be 0
    }
}

# SP2013 SSOM
Describe "metapack.cli.provision.spmeta2.sp2013.ssom" {
  
    It "Can install [$m2PackageId] package [SSOM]" {

        $args = @("install", 
                  "--id  $m2PackageId",
                  "--url  $sp2013SiteUrl",
                  "--username", $sp2013UserName,
                  "--userpassword", $sp2013UserPassword
                  "--spversion", "sp2013",
                  "--spapi", "SSOM",
                  "--source", $nugetSource
                  "--verbose"
                  )

        $result = (RunMetaPackCLI $args $true)

        $output = $result.Output
        $exitCode = $result.ExitCode
        
        # no exception in output
        if($result.UseShellExecute -eq $false) {
            (OutputHasError $output) | Should Be $false
        }            
        
        # exist code, please 0
        ($exitCode) | Should Be 0
    }

    It "Can install [$m2PackageId] package --force [SSOM]" {
        
        $args = @("install", 
                  "--id  $m2PackageId",
                  "--url  $sp2013SiteUrl",
                  "--username", $sp2013UserName,
                  "--userpassword", $sp2013UserPassword
                  "--spversion", "sp2013",
                  "--spapi", "SSOM",
                  "--source", $nugetSource
                  "--verbose",
                  "--force"
                  )

        $result = (RunMetaPackCLI $args $true)

        $output = $result.Output
        $exitCode = $result.ExitCode

       # no exception in output
        if($result.UseShellExecute -eq $false) {
            (OutputHasError $output) | Should Be $false
        }     
        
        # exist code, please 0
        ($exitCode) | Should Be 0
    }
}