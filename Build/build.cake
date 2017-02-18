// common tooling
// always version to avoid breaking change with new releases
#addin nuget:?package=Cake.Powershell&Version=0.2.9
#addin nuget:?package=newtonsoft.json&Version=9.0.1
#addin nuget:?package=NuGet.Core&Version=2.12.0

// defaultXXX - shared, common settings
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

var jsonConfig = Newtonsoft.Json.Linq.JObject.Parse(System.IO.File.ReadAllText("build.json"));

// default helpers
var currentTimeStamp = String.Empty;

string GetGlobalEnvironmentVariable(string name) {
    var result = System.Environment.GetEnvironmentVariable(name, System.EnvironmentVariableTarget.Process);

    if(String.IsNullOrEmpty(result))
        result = System.Environment.GetEnvironmentVariable(name, System.EnvironmentVariableTarget.User);

    if(String.IsNullOrEmpty(result))
        result = System.Environment.GetEnvironmentVariable(name, System.EnvironmentVariableTarget.Machine);

    return result;
}

string GetVersionForNuGetPackage(string id, string defaultVersion, string branch) {
    
    var resultVersion = string.Empty;
    
	if(branch != "master" && branch != "beta")
		branch = "dev";

    var now = DateTime.Now;

    Information(string.Format("Building nuget package version for branch:[{0}]", branch));

    switch(branch) {
       
        case "dev" : {

            var year = now.ToString("yy");
            var dayOfYear = now.DayOfYear.ToString("000");
            var timeOfDay = now.ToString("HHmm");

            var stamp = int.Parse(year + dayOfYear + timeOfDay);

            // save it to a static var to avoid flicks between NuGet package builds
            if(String.IsNullOrEmpty(currentTimeStamp))
                currentTimeStamp = stamp.ToString();

            var latestNuGetPackageVersion = GetLatestPackageFromNuget(id);

            if(String.IsNullOrEmpty(latestNuGetPackageVersion))
                latestNuGetPackageVersion = defaultVersion;

            Information(String.Format("- latest nuget package [{0}] version [{1}]", id, latestNuGetPackageVersion));

            var versionParts = latestNuGetPackageVersion.Split('-');

            var packageVersion = String.Empty;
            var packageBetaVersion = 0;

            if(versionParts.Count() > 1) {
                packageVersion = versionParts[0];
                packageBetaVersion = int.Parse(versionParts[1].Replace("beta", String.Empty));
            } else {
                packageVersion = versionParts[0];
            }

            var currentVersion = new Version(packageVersion);

            Information(String.Format("- currentVersion package [{0}] version [{1}]", id, currentVersion));
            Information(String.Format("- packageBetaVersion package [{0}] version [{1}]", id, packageBetaVersion));

            var buildIncrement = 5;

            if(packageBetaVersion == 1) {
                resultVersion = string.Format("{0}.{1}.{2}",
                        new object[] {
                                currentVersion.Major,
                                currentVersion.Minor,
                                currentVersion.Build + buildIncrement
                }); 
            }

            var packageSemanticVersion = packageVersion + "-alpha" + (currentTimeStamp);
            resultVersion = packageSemanticVersion;

        }; break;

        case "beta" : {

            var latestNuGetPackageVersion = GetLatestPackageFromNuget(id);

            if(String.IsNullOrEmpty(latestNuGetPackageVersion))
                latestNuGetPackageVersion = defaultVersion;

            Information(String.Format("- latest nuget package [{0}] version [{1}]", id, latestNuGetPackageVersion));

            var versionParts = latestNuGetPackageVersion.Split('-');

            var packageVersion = String.Empty;
            var packageBetaVersion = 0;

            if(versionParts.Count() > 1) {
                packageVersion = versionParts[0];
                packageBetaVersion = int.Parse(versionParts[1].Replace("beta", String.Empty));
            } else {
                packageVersion = versionParts[0];
            }

            var currentVersion = new Version(packageVersion);

            Information(String.Format("- currentVersion package [{0}] version [{1}]", id, currentVersion));
            Information(String.Format("- packageBetaVersion package [{0}] version [{1}]", id, packageBetaVersion));

            var buildIncrement = 5;

            if(packageBetaVersion == 1) {
                resultVersion = string.Format("{0}.{1}.{2}",
                        new object[] {
                                currentVersion.Major,
                                currentVersion.Minor,
                                currentVersion.Build + buildIncrement
                }); 
            }

            var packageSemanticVersion = packageVersion + "-beta" + (++packageBetaVersion);
            resultVersion = packageSemanticVersion;

         }; break;

         case "master" : {

            var latestNuGetPackageVersion = GetLatestPackageFromNuget(id);

            if(String.IsNullOrEmpty(latestNuGetPackageVersion))
                latestNuGetPackageVersion = defaultVersion;

            Information(String.Format("- latest nuget package [{0}] version [{1}]", id, latestNuGetPackageVersion));

            var versionParts = latestNuGetPackageVersion.Split('-');

            var packageVersion = String.Empty;
            var packageBetaVersion = 0;

            if(versionParts.Count() > 1) {
                packageVersion = versionParts[0];
                packageBetaVersion = int.Parse(versionParts[1].Replace("beta", String.Empty));
            } else {
                packageVersion = versionParts[0];
            }

            var currentVersion = new Version(packageVersion);
            
            Information(String.Format("- currentVersion package [{0}] version [{1}]", id, currentVersion));
            Information(String.Format("- packageBetaVersion package [{0}] version [{1}]", id, packageBetaVersion));

            var buildIncrement = 5;

            if(packageBetaVersion == 0)
                buildIncrement = 10;
            
            resultVersion = string.Format("{0}.{1}.{2}",
                        new object[] {
                                currentVersion.Major,
                                currentVersion.Minor,
                                currentVersion.Build + buildIncrement
            }); 

            return resultVersion;

         }; break;
    }

    return resultVersion;
}

string GetLatestPackageFromNuget(string packageId) {
    return GetLatestPackageFromNuget("https://packages.nuget.org/api/v2",packageId);
}

string GetLatestPackageFromNuget(string nugetRepoUrl, string packageId) {
    
    var repo =  NuGet.PackageRepositoryFactory.Default.CreateRepository(nugetRepoUrl);
    var package =  NuGet.PackageRepositoryExtensions.FindPackage(repo, packageId);

    if(package == null)
        return String.Empty;

    return package.Version.ToString();
}

// CI related environment
// * dev / beta / master versioning and publishing
var ciBranch = GetGlobalEnvironmentVariable("ci.activebranch") ?? "dev";

// override under CI run
var ciBranchOverride = GetGlobalEnvironmentVariable("APPVEYOR_REPO_BRANCH");
if(!String.IsNullOrEmpty(ciBranchOverride))
	ciBranch = ciBranchOverride;

ciBranch = "dev";

var ciNuGetSource = GetGlobalEnvironmentVariable("ci.nuget.source") ?? String.Empty;
var ciNuGetKey = GetGlobalEnvironmentVariable("ci.nuget.key") ?? String.Empty;
var ciNuGetShouldPublish = bool.Parse(GetGlobalEnvironmentVariable("ci.nuget.shouldpublish") ?? "FALSE");

Information(string.Format(" -target:[{0}]",target));
Information(string.Format(" -configuration:[{0}]", configuration));
Information(string.Format(" -activeBranch:[{0}]", ciBranch));

// source solution dir and file
var defaultSolutionDirectory = (string)jsonConfig["defaultSolutionDirectory"]; 
var defaultSolutionFilePath = (string)jsonConfig["defaultSolutionFilePath"];

// nuget packages
var defaultNuGetPackagesDirectory = (string)jsonConfig["defaultNuGetPackagesDirectory"];
var defaultNuspecVersion = (string)jsonConfig["defaultNuspecVersion"];

// test settings
var defaultTestCategories = jsonConfig["defaultTestCategories"].Select(t => (string)t).ToList();
var defaultTestAssemblyPaths = jsonConfig["defaultTestAssemblyPaths"].Select(t => (string)t).ToList();

// build settings
var defaultBuildDirs = jsonConfig["defaultBuildDirs"].Select(t => new DirectoryPath((string)t)).ToList();
var defaultEnvironmentVariables =  jsonConfig["defaultEnvironmentVariables"].Select(t => (string)t).ToList();

var defaultNuspecs = new List<NuGetPackSettings>();

// common tasks
// * Validate-Environment
// * Clean
// * Restore-NuGet-Packages
// * Build
// * Run-Unit-Tests
// * NuGet-Publishing

Task("Validate-Environment")
    .Does(() =>
{
    foreach(var name in defaultEnvironmentVariables)
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
    foreach(var dirPath in defaultBuildDirs) {
        CleanDirectory(dirPath);
    }        
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(defaultSolutionFilePath);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
      MSBuild(defaultSolutionFilePath, settings => {
            settings.SetVerbosity(Verbosity.Quiet);
            settings.SetConfiguration(configuration);
      });
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    foreach(var assemblyPath in defaultTestAssemblyPaths) {
        
        foreach(var testCategory in defaultTestCategories) {
            Information(string.Format("Running test category [{0}] for assembly:[{1}]", testCategory, assemblyPath));

            MSTest(new [] { new FilePath(assemblyPath) }, new MSTestSettings {
                    Category = testCategory
                });
        }
    }        
});

Task("NuGet-Packaging")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    Information("Creating NuGet packages of version [{0}] in directory:[{1}]", new []{
        defaultNuGetPackagesDirectory,
        defaultNuspecVersion
    });

    CreateDirectory(defaultNuGetPackagesDirectory);
    CleanDirectory(defaultNuGetPackagesDirectory);

    foreach(var nuspec in defaultNuspecs)
    {   
        nuspec.Version = GetVersionForNuGetPackage(nuspec.Id, defaultNuspecVersion, ciBranch);

        // update deps versions to a correct one during build
        foreach(var dep in nuspec.Dependencies) {
            if(dep.Id.StartsWith("MetaPack")) {
                dep.Version = GetVersionForNuGetPackage(dep.Id, defaultNuspecVersion, ciBranch); 
            }
        }

        Information(string.Format("Creating NuGet package for [{0}] of version:[{1}]", nuspec.Id, nuspec.Version));

        NuGetPack(nuspec);
    }        
});

Task("NuGet-Publishing")
    // all packaged should be compiled by NuGet-Packaging task into 'defaultNuGetPackagesDirectory' folder
    .IsDependentOn("NuGet-Packaging")
    .Does(() =>
{
    if(!ciNuGetShouldPublish) {
        Information("Skipping NuGet publishing as ciNuGetShouldPublish is false.");
        return;
    }

    Information("Publishing NuGet packages to repository: [{0}]", new []{
        ciNuGetSource
    });

    var nugetSource = ciNuGetSource;
	var nugetKey = ciNuGetKey;

    var nuGetPackages = System.IO.Directory.GetFiles(defaultNuGetPackagesDirectory, "*.nupkg");

    foreach(var packageFilePath in nuGetPackages)
        {
            var packageFileName = System.IO.Path.GetFileName(packageFilePath);

            if(System.IO.File.Exists(packageFilePath)) {
                
                // checking is publushed
                Information(string.Format("Checking if NuGet package [{0}] is already published", packageFileName));
                
                // TODO
                var isNuGetPackagePublished = false;
                if(!isNuGetPackagePublished)
                {
                    Information(string.Format("Publishing NuGet package [{0}]...", packageFileName));
                
                    NuGetPush(packageFilePath, new NuGetPushSettings {
                        Source = nugetSource,
                        ApiKey = nugetKey
                    });
                }
                else
                {
                    Information(string.Format("NuGet package [{0}] was already published", packageFileName));
                }                 
                
            } else {
                Information(string.Format("NuGet package does not exist:[{0}]", packageFilePath));
                throw new ArgumentException(string.Format("NuGet package does not exist:[{0}]", packageFilePath));
            }
        }           
});

// common targets
Task("Default")
    .IsDependentOn("Run-Unit-Tests");

Task("Default-Clean")
    .IsDependentOn("Clean");

Task("Default-Build")
    .IsDependentOn("Build");

Task("Default-NuGet-Packaging")
    .IsDependentOn("NuGet-Packaging");

Task("Default-NuGet-Publishing")
    .IsDependentOn("NuGet-Publishing");  

Task("Default-CI")
    .IsDependentOn("NuGet-Publishing");  

// project specific things



// prjXXX - project specific vars
var prjNuspecSPMeta2DependencyVersion = "1.2.100";
var prjNuspecNuGetCoreDependencyVersion  = "2.12.0";

var prjTestCategories = new string []{
     
};

var prjChocoPackagesDirectory = "./build";
var prjMetaPackCLIBinPath = "./../MetaPack.Client.Console/bin/debug/";
var prjMetaPackCLIFiles = new string[] {
    
    // console itself
    "metapack.exe",
    "metapack.exe.config",
    
    // console helpers
    "CommandLine.dll",

    // matapack core assemblies
    "MetaPack.Core.dll",
    "MetaPack.NuGet.dll",

    // other refs
    "NuGet.Core.dll",
    "Microsoft.Web.XmlTransform.dll",
    "AppDomainToolkit.dll",

    // metapack common client services
    "MetaPack.Client.Common.dll",

    "deps/sp2013-csom/**"
    //"deps/sp2013-csom/Microsoft.SharePoint.Client.dll",
    //"deps/sp2013-csom/Microsoft.SharePoint.Client.Runtime.dll"
};

var metapackCorePackage = new NuGetPackSettings()
        {
            Id = "MetaPack.Core",
            Version = defaultNuspecVersion,

            Dependencies = new NuSpecDependency[]
            {
                
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
            
            OutputDirectory = new DirectoryPath(defaultNuGetPackagesDirectory)
};

// project specific NuGet packages
defaultNuspecs.Add(metapackCorePackage);

var metapackNuGetPackage = new NuGetPackSettings()
        {
            Id = "MetaPack.NuGet",
            Version = defaultNuspecVersion,

            Dependencies = new []
            {
                new NuSpecDependency() { Id = "MetaPack.Core", Version = metapackCorePackage.Version },
                new NuSpecDependency() { Id = "NuGet.Core", Version = prjNuspecNuGetCoreDependencyVersion },
                new NuSpecDependency() { Id = "AppDomainToolkit", Version = "1.0.4.3" },
            },

            Authors = new [] { "SubPoint Solutions" },
            Owners = new [] { "SubPoint Solutions" },
            LicenseUrl = new Uri("http://docs.subpointsolutions.com/metapack/license"),
            ProjectUrl = new Uri("https://github.com/SubPointSolutions/metapack"),
            
            Description = "MetaPack implementation for NuGet protocol. Provides NuGet file-system and other NuGet related services.",
            Copyright = "Copyright 2016",
            Tags = new [] { "SPMeta2", "Provision", "SharePoint", "Office365Dev", "Office365", "metapack", "nuget" },

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
            
            OutputDirectory = new DirectoryPath(defaultNuGetPackagesDirectory)
};

defaultNuspecs.Add(metapackNuGetPackage);

defaultNuspecs.Add(new NuGetPackSettings()
        {
            Id = "MetaPack.SPMeta2",
            Version = defaultNuspecVersion,

            Dependencies = new []
            {
                new NuSpecDependency() { Id = "MetaPack.Core", Version = metapackCorePackage.Version },
                new NuSpecDependency() { Id = "MetaPack.NuGet", Version = metapackNuGetPackage.Version }               
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
            
            OutputDirectory = new DirectoryPath(defaultNuGetPackagesDirectory)
});

defaultNuspecs.Add(new NuGetPackSettings()
        {
            Id = "MetaPack.SharePointPnP",
            Version = defaultNuspecVersion,

            Dependencies = new []
            {
                new NuSpecDependency() { Id = "MetaPack.Core", Version = metapackCorePackage.Version },
                new NuSpecDependency() { Id = "MetaPack.NuGet", Version = metapackNuGetPackage.Version }
            },

            Authors = new [] { "SubPoint Solutions" },
            Owners = new [] { "SubPoint Solutions" },
            LicenseUrl = new Uri("http://docs.subpointsolutions.com/metapack/license"),
            ProjectUrl = new Uri("https://github.com/SubPointSolutions/metapack"),
            
            Description = "MetaPack implementation for SharePointPnP model packaging, delivery and updates with NuGet protocol. Enables SharePointPnP model provision from NuGet galleries.",
            Copyright = "Copyright 2016",
            Tags = new [] { "SPMeta2", "Provisoin", "SharePoint", "Office365Dev", "Office365", "metapack", "nuget" },

            RequireLicenseAcceptance = false,
            Symbols = false,
            NoPackageAnalysis = true,
            BasePath  = "./../MetaPack.SharePointPnP/bin/debug",
            
            Files = new [] {
                new NuSpecContent {
                    Source = "MetaPack.SharePointPnP.dll",
                    Target = "lib/net45"
                },
                new NuSpecContent {
                    Source = "MetaPack.SharePointPnP.xml",
                    Target = "lib/net45"
                }
            },
            
            OutputDirectory = new DirectoryPath(defaultNuGetPackagesDirectory)
});

defaultNuspecs.Add(new NuGetPackSettings()
        {
            Id = "MetaPack.Client.Common",
            Version = defaultNuspecVersion,

            Dependencies = new []
            {
                new NuSpecDependency() { Id = "MetaPack.Core", Version = metapackCorePackage.Version },
                new NuSpecDependency() { Id = "NuGet.Core", Version = prjNuspecNuGetCoreDependencyVersion },
            },

             Authors = new [] { "SubPoint Solutions" },
            Owners = new [] { "SubPoint Solutions" },
            LicenseUrl = new Uri("http://docs.subpointsolutions.com/metapack/license"),
            ProjectUrl = new Uri("https://github.com/SubPointSolutions/metapack"),
            
            Description = "MetaPack implementation for common solution package operations such as list, update and install",
            Copyright = "Copyright 2016",
            Tags = new [] { "SPMeta2", "Provisoin", "SharePoint", "Office365Dev", "Office365", "metapack", "nuget" },

            RequireLicenseAcceptance = false,
            Symbols = false,
            NoPackageAnalysis = true,
            BasePath  = "./../MetaPack.Client.Common/bin/debug",
            
            Files = new [] {
                new NuSpecContent {
                    Source = "MetaPack.Client.Common.dll",
                    Target = "lib/net45"
                },
                new NuSpecContent {
                    Source = "MetaPack.Client.Common.xml",
                    Target = "lib/net45"
                }
            },
            
            OutputDirectory = new DirectoryPath(defaultNuGetPackagesDirectory)
});

// prject specific Chocolatey packaging
var prjChocolateySpecs = new [] {
        new ChocolateyPackSettings()
        {
            Id = "MetaPack",
            Title = "MetaPack",
            Version = defaultNuspecVersion,

            Authors = new [] { "SubPoint Solutions" },
            Owners = new [] { "SubPoint Solutions" },
            LicenseUrl = new Uri("http://docs.subpointsolutions.com/metapack/license"),
            ProjectUrl = new Uri("https://github.com/SubPointSolutions/metapack"),
            
            IconUrl = new Uri("https://raw.githubusercontent.com/SubPointSolutions/spmeta2/dev/SPMeta2/SPMeta2.Dependencies/Images/SPMeta2_64_64.png"),

            Description = "MetaPack CLI. Provides a command line interface to manage matapack packages.",
            Copyright = "Copyright 2016",
            Tags = new [] { "SPMeta2", "Provision", "SharePoint", "Office365Dev", "Office365", "metapack", "nuget" },

            RequireLicenseAcceptance = false,

            Files = prjMetaPackCLIFiles.Select(f => {
                
                 if(f.Contains("**")) {

                     var dstFolder = f.Replace("**", String.Empty).TrimEnd('\\').Replace('\\', '/');
                     var srcFolder = System.IO.Path.GetFullPath(prjMetaPackCLIBinPath +  dstFolder);

                     var chSrcDir = srcFolder + @"**";
                     var chDstDir = "lib/metapack" + "/" + dstFolder.TrimEnd('/');

                     return new ChocolateyNuSpecContent{
                         Source = chSrcDir,
                         Target = chDstDir
                     };
                 }

                return new ChocolateyNuSpecContent{
                    Source = System.IO.Path.Combine(prjMetaPackCLIBinPath, f),
                    Target = "lib/metapack"
                };

            }).ToList(),
            
            AllowUnofficial = false
        }
 };
 
 Task("Default-Chocolatey-Packaging")
     //.IsDependentOn("Run-Unit-Tests")
     //.IsDependentOn("Build")
    .Does(() =>
{
      Information("Building Chocolatey package...");

      foreach(var chocoSpec in prjChocolateySpecs) {

           chocoSpec.Version = GetVersionForNuGetPackage(chocoSpec.Id, defaultNuspecVersion, ciBranch);

           Information(string.Format("Creating Chocolatey package [{0}] version:[{1}]", chocoSpec.Id, chocoSpec.Version));
           ChocolateyPack(chocoSpec);
       }

      Information(string.Format("Completed creating chocolatey package"));
});

RunTarget(target);