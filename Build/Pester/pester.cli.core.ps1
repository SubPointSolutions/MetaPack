Param(
  [string]$workingFolderPath
)

. "Pester\helpers.ps1"

$fullRegressionResult = $true
$testFiles = @(

    # default regression
    @{
        ProfileName = "cli.default"
        FileName = "Pester/metapack.cli.core.ps1"
        Enable = $true
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

return 0