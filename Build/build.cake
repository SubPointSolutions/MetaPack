// load up common tools
#load tools/SubPointSolutions.CakeBuildTools/scripts/SubPointSolutions.CakeBuild.Core.cake

Task("Action-CLI-Regression")
    .Does(() => {

        Information("Running Pester testing...");

		var tmpPath = System.IO.Path.GetTempPath();
		
		var workingFolderName = string.Format("metapack-cli-{0}", Guid.NewGuid().ToString("N"));
		var workingFolderPath = System.IO.Path.Combine(tmpPath, workingFolderName);

		System.IO.Directory.CreateDirectory(workingFolderPath);

        Information("- ensuring peter is installed...");
        StartPowershellFile("Pester/_install.ps1", args =>
        {
             args.Append("WorkingFolderPath", workingFolderPath);
        });

		Information("- installing the latest Chocolatey package [metapack]");
        StartPowershellFile("build-choco-install-local.ps1", args =>
        {
            args.Append("packageName", "metapack");
        });

		Information("- installing the latest Chocolatey package [metapack-ui]");
        StartPowershellFile("build-choco-install-local.ps1", args =>
        {
            args.Append("packageName", "metapack-ui");
        });
		
        Information("- running CLI core regression...");
        var coreRegressionResult = StartPowershellFile("Pester/pester.cli.core.ps1", args =>
        {
            args.Append("WorkingFolderPath", workingFolderPath);
        });

		var coreRegressionResultCode = int.Parse(coreRegressionResult[0].BaseObject.ToString());
        if (coreRegressionResultCode != 0) 
            throw new ApplicationException("Failed CLI core regression");

		Information("- running CLI core provision, SPMeta2/PnP with O365");
        var provisionRegressionResult = StartPowershellFile("Pester/pester.cli.provision.ps1", args =>
        {
            args.Append("WorkingFolderPath", workingFolderPath);
        });

		var provisionRegressionResultCode = int.Parse(provisionRegressionResult[0].BaseObject.ToString());
        if (provisionRegressionResultCode != 0) 
            throw new ApplicationException("Failed CLI provision regression");
    });

// add one more for taskDefaultCLIPackaging
// testing that CLI from chocolatey works
// https://github.com/SubPointSolutions/CakeBuildTools
taskDefaultCI
    .IsDependentOn("Action-CLI-Regression");

// default targets
RunTarget(target);