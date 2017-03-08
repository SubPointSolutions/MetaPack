Param(
  [string]$workingFolderPath
)

# if([string]::IsNullOrEmpty($workingFolderPath) -eq $true) {
# 	throw "workingFolderPath is null"
# }
# else {
# 	Write-Host "Running regresison in folder:[$workingFolderPath]"
# }

function Analyse-Results($results) {

    $failedCount = 0;
    $passedCount = 0;

    foreach($r in $results.TestResult) {

    Write-Host "- Test [$($r.Name)] with result:[$($r.Result)]"

    switch($r.Result) {
        "Failed"  {
            $failedCount++
        }; 
        "Passed"  {
            $passedCount++
        }; 
    }
    }

    Write-Host "- Failed count:[$failedCount]"
    Write-Host "- Passed count:[$passedCount]"

    if($failedCount -gt 0) {
        Write-Host "[FAIL] Didn't pass Pester regression. Failed tests count:[$failedCount]"
        return $false
    }
    else{
        if($passedCount -le 0) {
            Write-Host "[FAIL] Weird. No tests were passed. Check it, please?"
            return $false
        }
    } 

    return $true
}

$fullRegressionResult = $true
$testFiles = @(

    # default regression
    @{
        ProfileName = "cli.default"
        FileName = "Pester/metapack.cli.core.ps1"
        Enable = $false
    },

    # O365 regresssion
    # SPMeta2 + 0365
    @{
        ProfileName = "cli.m2.csom.o365"
        FileName = "Pester/metapack.cli.provision.ps1"
        TestName = "metapack.cli.provision.spmeta2.o365"
        Enable = $true
    },

    # PnP + O365
    @{
        ProfileName = "cli.pnp.csom.o365"
        FileName = "Pester/metapack.cli.provision.ps1"
        TestName = "metapack.cli.provision.pnp.o365"
        Enable = $false
    }

    # SP2013 regresssion
    # SPMeta2 + SP2013
    @{
        ProfileName = "cli.m2.csom.sp2013"
        FileName = "Pester/metapack.cli.provision.ps1"
        TestName = "metapack.cli.provision.spmeta2.sp2013"
        Enable = $false
    },

    # PnP + SP2013
    @{
        ProfileName = "cli.pnp.csom.sp2013"
        FileName = "Pester/metapack.cli.provision.ps1"
        TestName = "metapack.cli.provision.pnp.sp2013"
        Enable = $false
    },

    # SPMeta2 + SP2013 SSOM
    @{
        ProfileName = "cli.m2.csom.sp2013.ssom"
        FileName = "Pester/metapack.cli.provision.ps1"
        TestName = "metapack.cli.provision.spmeta2.sp2013.ssom"
        Enable = $false
    }
)

foreach($testFile in $testFiles) {

    $fileName = $testFile.FileName
    $testName = $testFile.TestName

    $enabled = $testFile.Enable

    if($enabled  -eq $false) {
        Write-Host "Skipping test file:[$fileName]"
        continue;
    }

    Write-Host "Running test file:[$fileName] with test name:[$testName]"
    $result = $true

    if([string]::IsNullOrEmpty($testName)) {
        $result = Analyse-Results (Invoke-Pester -Script $fileName -PassThru)
    } else {
        $result = Analyse-Results (Invoke-Pester -Script $fileName -TestName $testName  -PassThru)
    }        

    if($result -eq $false) {
         $fullRegressionResult  = $false
    }
}

if($fullRegressionResult -eq $false)
{
    throw "Failed Pester regression."
}