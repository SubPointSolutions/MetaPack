. "Pester\helpers.ps1"

Write-Host "Running CLI provisioning tests..." -fore Green

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
}