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

		Information("- installing the latest Chocolatey package...");
        StartPowershellFile("build-choco-install-local.ps1", args =>
        {
            args.Append("WorkingFolderPath", workingFolderPath);
        });

        Information("- running regression...");

        StartPowershellFile("Pester/pester.run.ps1", args =>
        {
            args.Append("WorkingFolderPath", workingFolderPath);
        });
    });

// add one more for taskDefaultCLIPackaging
// testing that CLI from chocolatey works
// https://github.com/SubPointSolutions/CakeBuildTools
taskDefaultCI
    .IsDependentOn("Action-CLI-Regression");

// default targets
RunTarget(target);