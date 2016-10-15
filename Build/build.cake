#addin "Cake.Powershell"
#addin nuget:?package=Cake.Git

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

var solutionDirectory = "./../"; 
var solutionFilePath = "./../MetaPack.sln";

var defaultTestCategory = "CI.Core";
var defaultTestAssemblyPaths = new string[] {
    "./../MetaPack.Tests/bin/debug/MetaPack.Tests.dll"
};

var useNuGetPackaging = true;
var useNuGetPublishing = true;

var useCIBuildVersion = true;

var g_hardcoreVersionBase = "1.2.70";
var g_SPMeta2VersionBase = "1.2.70";

var g_hardcoreVersion = g_hardcoreVersionBase + "-beta1";

var date = DateTime.Now;
var stamp = (date.ToString("yy") + date.DayOfYear.ToString("000") + date.ToString("HHmm"));

if(useCIBuildVersion) {
    g_hardcoreVersion = g_hardcoreVersionBase + "-alpha" + stamp;
}

var nuGetPackagesDirectory = "./packages";

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var buildDirs = new [] {

    new DirectoryPath("./../MetaPack.Client.Common/bin/"),
    new DirectoryPath("./../MetaPack.Client.Console/bin/"),

    new DirectoryPath("./../MetaPack.Core/bin/"),
    new DirectoryPath("./../MetaPack.NuGet/bin/"),

    new DirectoryPath("./../MetaPack.SPMeta2/bin/"),

    new DirectoryPath("./../MetaPack.Tests/bin/"),

    new DirectoryPath(nuGetPackagesDirectory)
}; 

var environmentVariables = new string[] {
    //"metapack-nuget-source",
    //"metapack-nuget-key",
    //"subpointsolutions-docs-username",
    //"subpointsolutions-docs-userpassword",
};

 var nuspecs = new [] {
        new NuGetPackSettings()
        {
            Id = "MetaPack.Core",
            Version = g_hardcoreVersion,

            Dependencies = new []
            {
                new NuSpecDependency() { Id = "SPMeta2.Core", Version = g_SPMeta2VersionBase }
            },

            Authors = new [] { "SubPoint Solutions" },
            Owners = new [] { "SubPoint Solutions" },
            LicenseUrl = new Uri("http://docs.subpointsolutions.com/metapack/license"),
            ProjectUrl = new Uri("https://github.com/SubPointSolutions/metapack"),
            
            Description = "MetaPack common infrastructure. Provides high level abstraction for packaging.",
            Copyright = "Copyright 2016",
            Tags = new [] { "SPMeta2", "Provisoin", "SharePoint", "Office365Dev", "Office365", "metapack", "nuget" },

            RequireLicenseAcceptance = false,
            Symbols = false,
            NoPackageAnalysis = true,
            BasePath  = "./../MetaPack.Core/bin/debug",
            
            Files = new [] {
                new NuSpecContent {
                    Source = "MetaPack.Core.dll",
                    Target = "lib/net45"
                },
                new NuSpecContent {
                    Source = "MetaPack.Core.xml",
                    Target = "lib/net45"
                }
            },
            
            OutputDirectory = new DirectoryPath(nuGetPackagesDirectory)
        },

        new NuGetPackSettings()
        {
            Id = "MetaPack.NuGet",
            Version = g_hardcoreVersion,

            Dependencies = new []
            {
                new NuSpecDependency() { Id = "MetaPack.Core", Version = g_SPMeta2VersionBase },
                new NuSpecDependency() { Id = "NuGet.Core", Version = "2.11.1" },
            },

            Authors = new [] { "SubPoint Solutions" },
            Owners = new [] { "SubPoint Solutions" },
            LicenseUrl = new Uri("http://docs.subpointsolutions.com/metapack/license"),
            ProjectUrl = new Uri("https://github.com/SubPointSolutions/metapack"),
            
            Description = "MetaPack implementation for NuGet protocol. Provides NuGet file-system and other NuGet related services.",
            Copyright = "Copyright 2016",
            Tags = new [] { "SPMeta2", "Provisoin", "SharePoint", "Office365Dev", "Office365", "metapack", "nuget" },

            RequireLicenseAcceptance = false,
            Symbols = false,
            NoPackageAnalysis = true,
            BasePath  = "./../MetaPack.Nuget/bin/debug",
            
            Files = new [] {
                new NuSpecContent {
                    Source = "MetaPack.Nuget.dll",
                    Target = "lib/net45"
                },
                new NuSpecContent {
                    Source = "MetaPack.Nuget.xml",
                    Target = "lib/net45"
                }
            },
            
            OutputDirectory = new DirectoryPath(nuGetPackagesDirectory)
        },

        new NuGetPackSettings()
        {
            Id = "MetaPack.SPMeta2",
            Version = g_hardcoreVersion,

            Dependencies = new []
            {
                new NuSpecDependency() { Id = "MetaPack.Core", Version = g_SPMeta2VersionBase },
                new NuSpecDependency() { Id = "MetaPack.NuGet", Version = g_SPMeta2VersionBase },
            },

             Authors = new [] { "SubPoint Solutions" },
            Owners = new [] { "SubPoint Solutions" },
            LicenseUrl = new Uri("http://docs.subpointsolutions.com/metapack/license"),
            ProjectUrl = new Uri("https://github.com/SubPointSolutions/metapack"),
            
            Description = "MetaPack implementation for SPMeta2 model packaging, delivery and updates with NuGet protocol. Enables SPMeta2 model provision from NuGet galleries.",
            Copyright = "Copyright 2016",
            Tags = new [] { "SPMeta2", "Provisoin", "SharePoint", "Office365Dev", "Office365", "metapack", "nuget" },

            RequireLicenseAcceptance = false,
            Symbols = false,
            NoPackageAnalysis = true,
            BasePath  = "./../MetaPack.SPMeta2/bin/debug",
            
            Files = new [] {
                new NuSpecContent {
                    Source = "MetaPack.SPMeta2.dll",
                    Target = "lib/net45"
                },
                new NuSpecContent {
                    Source = "MetaPack.SPMeta2.xml",
                    Target = "lib/net45"
                }
            },
            
            OutputDirectory = new DirectoryPath(nuGetPackagesDirectory)
        }
    };  

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Validate-Environment")
    .Does(() =>
{
    foreach(var name in environmentVariables)
    {
        Information(string.Format("HasEnvironmentVariable - [{0}]", name));
        if(!HasEnvironmentVariable(name)) {
            Information(string.Format("Cannot find environment variable:[{0}]", name));
            throw new ArgumentException(string.Format("Cannot find environment variable:[{0}]", name));
        }
    }
});

Task("Clean")
    .IsDependentOn("Validate-Environment")
    .Does(() =>
{
    foreach(var dirPath in buildDirs) {
        CleanDirectory(dirPath);
    }        
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solutionFilePath);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
      MSBuild(solutionFilePath, settings => {
        
            settings.SetVerbosity(Verbosity.Quiet);
            settings.SetConfiguration(configuration);
      });
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    foreach(var assemblyPath in defaultTestAssemblyPaths) {
        
        Information(string.Format("Running test for assembly:[{0}]", assemblyPath));
        
        MSTest(new [] { new FilePath(assemblyPath) }, new MSTestSettings {
                Category = defaultTestCategory
            });
    }
});

Task("NuGet-Packaging")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    if(!useNuGetPackaging) {
        Information("Skipping NuGet packaging...");
        return;
    }

    Information("Creating NuGet packages for version:[{0}] and SPMeta2 version:[{1}]", new []{
        g_hardcoreVersion,
        g_SPMeta2VersionBase
    });

    CreateDirectory(nuGetPackagesDirectory);
    CleanDirectory(nuGetPackagesDirectory);

    foreach(var nuspec in nuspecs)
    {   
        Information(string.Format("Creating NuGet package for [{0}]", nuspec.Id));
        NuGetPack(nuspec);
    }        
});

Task("NuGet-Publishing")
    .IsDependentOn("NuGet-Packaging")
    .Does(() =>
{
    if(!useNuGetPublishing) {
        Information("Skipping NuGet publishing...");
        return;
    }

    Information("Publishing NuGet packages for version:[{0}] and SPMeta2 version:[{1}]", new []{
        g_hardcoreVersion,
        g_SPMeta2VersionBase
    });

    foreach(var nuspec in nuspecs)
    {   
        Information(string.Format("Publishing NuGet package for [{0}]", nuspec.Id));

        var packageFileName = string.Format("{0}.{1}.nupkg", nuspec.Id, nuspec.Version);
        var packageFilePath = string.Format("{0}/{1}", nuGetPackagesDirectory, packageFileName);
        
		var nugetSource = EnvironmentVariable("spmeta2-reverse-nuget-source");
		var nugetKey = EnvironmentVariable("spmeta2-reverse-nuget-key");

        if(System.IO.File.Exists(packageFilePath)) {
            
			Information(string.Format("Publishing NuGet package [{0}]...", packageFileName));

            NuGetPush(packageFilePath, new NuGetPushSettings {
                Source = nugetSource,
                ApiKey = nugetKey
            });
            
        } else {
            Information(string.Format("NuGet package does not exist:[{0}]", packageFilePath));
            throw new ArgumentException(string.Format("NuGet package does not exist:[{0}]", packageFilePath));
        }
    }        
});


Task("Docs-Publishing")
    .Does(() =>
{
    var environmentVariables = new [] {
        "subpointsolutions-docs-username",
        "subpointsolutions-docs-userpassword",
    };

    foreach(var name in environmentVariables)
    {
        Information(string.Format("HasEnvironmentVariable - [{0}]", name));
        if(!HasEnvironmentVariable(name)) {
            Information(string.Format("Cannot find environment variable:[{0}]", name));
            throw new ArgumentException(string.Format("Cannot find environment variable:[{0}]", name));
        }
    }

     var docsRepoUserName = EnvironmentVariable("subpointsolutions-docs-username");
	 var docsRepoUserPassword = EnvironmentVariable("subpointsolutions-docs-userpassword");

     var docsRepoFolder = string.Format(@"{0}/m2-r",  "c:/__m2");
     var docsRepoUrl = @"https://github.com/SubPointSolutions/subpointsolutions-docs";
     var docsRepoPushUrl = string.Format(@"https://{0}:{1}@github.com/SubPointSolutions/subpointsolutions-docs", docsRepoUserName, docsRepoUserPassword);

     var srcDocsPath = System.IO.Path.GetFullPath(@"./../SubPointSolutions.Docs/Views/SPMeta2-Reverse");
     var dstDocsPath = string.Format(@"{0}/subpointsolutions-docs/SubPointSolutions.Docs/Views", docsRepoFolder);

     var commitName = string.Format(@"SPMeta2 Reverse - CI docs update {0}", DateTime.Now.ToString("yyyyMMdd_HHmmssfff"));

     Information(string.Format("Merging documentation wiht commit:[{0}]", commitName));

     Information(string.Format("Cloning repo [{0}] to [{1}]", docsRepoUrl, docsRepoFolder));

     if(!System.IO.Directory.Exists(docsRepoFolder))
     {   
        System.IO.Directory.CreateDirectory(docsRepoFolder);   

        var cloneCmd = new []{
            string.Format("cd '{0}'", docsRepoFolder),
            string.Format("git clone -b wyam-dev {0}", docsRepoUrl)
        };

        StartPowershellScript(string.Join(Environment.NewLine, cloneCmd));  
     }                            

     docsRepoFolder = docsRepoFolder + "/subpointsolutions-docs"; 

     Information(string.Format("Checkout..."));
     var checkoutCmd = new []{
            string.Format("cd '{0}'", docsRepoFolder),
            string.Format("git checkout wyam-dev"),
            string.Format("git pull")
      };

      StartPowershellScript(string.Join(Environment.NewLine, checkoutCmd));  

      Information(string.Format("Merge and commit..."));
      var mergeCmd = new []{
            string.Format("cd '{0}'", docsRepoFolder),
            string.Format("copy-item  '{0}' '{1}' -Recurse -Force", srcDocsPath,  dstDocsPath),
            string.Format("git add *.md"),
            string.Format("git add *.cs"),
            string.Format("git commit -m '{0}'", commitName),
            
            
      };

      StartPowershellScript(string.Join(Environment.NewLine, mergeCmd)); 

      Information(string.Format("Push to the main repo..."));
      var pushCmd = new []{
            string.Format("cd '{0}'", docsRepoFolder),
            string.Format("git config http.sslVerify false"),
            string.Format("git push {0}", docsRepoPushUrl)
      };

      var res = StartPowershellScript(string.Join(Environment.NewLine, pushCmd), new PowershellSettings()
      {
            LogOutput = false,
            OutputToAppConsole  = false
      });

      Information(string.Format("Completed docs merge.")); 
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Run-Unit-Tests");

Task("Default-NuGet")
    .IsDependentOn("NuGet-Publishing");

Task("Default-Docs")
    .IsDependentOn("Docs-Publishing");

Task("Default-Appveyor")
    .IsDependentOn("NuGet-Publishing")
    .IsDependentOn("Docs-Publishing");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);