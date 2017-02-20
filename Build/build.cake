// load up common tools
#load SubPointSolutions.CakeBuild.Core.cake

Task("Action-CLI-Pester")
    .Does(() => {

        Information("Running CLI Pester testing...");

        Information("- ensuring peter is installed...");
        StartPowershellFile("Pester/_install.ps1", args =>
        {
            // args.Append("Username", "admin")
            //     .AppendSecret("Password", "pass1");
        });

        Information("- installing the latest Chocolatey package...");
        StartPowershellFile("build-choco-install-local.ps1", args =>
        {
            // args.Append("Username", "admin")
            //     .AppendSecret("Password", "pass1");
        });

        Information("- running CLI regression...");
        StartPowershellFile("Pester/metapack.cli.Tests.ps1", args =>
        {
            // args.Append("Username", "admin")
            //     .AppendSecret("Password", "pass1");
        });

    });

// default targets
RunTarget(target);