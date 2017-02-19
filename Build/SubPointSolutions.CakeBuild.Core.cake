// common tooling
// always version to avoid breaking change with new releases
#addin nuget:https://www.nuget.org/api/v2/?package=Cake.Powershell&Version=0.2.9
#addin nuget:https://www.nuget.org/api/v2/?package=newtonsoft.json&Version=9.0.1
#addin nuget:https://www.nuget.org/api/v2/?package=NuGet.Core&Version=2.12.0

// variables
// * defaultXXX - shared, common settings from json config
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

Verbose("Reading build.json");
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

string GetFullPath(string path) {
    return System.IO.Path.GetFullPath(path);
}

string ResolveFullPathFromSolutionRelativePath(string solutionRelativePath) {
    return System.IO.Path.GetFullPath(System.IO.Path.Combine(defaultSolutionDirectory, solutionRelativePath));
}

List<DirectoryPath> GetAllProjectDirectories(string solutionDirectory) {
    
    Verbose("Looking for *.csproj files in dir: " + solutionDirectory);

    var result = new List<DirectoryPath>();

    var csPrjFilePaths = System.IO.Directory.GetFiles(solutionDirectory, "*.csproj", System.IO.SearchOption.AllDirectories);

    foreach(var filePath in csPrjFilePaths)
    {
        var dirPath = System.IO.Path.GetDirectoryName(filePath);
        var binDirPath = System.IO.Path.Combine(dirPath, "bin");

        Verbose("- translated to bin path: " + binDirPath);
        result.Add(new DirectoryPath(binDirPath));
    }
    return result;
}

string ResolveVersionForPackage(string id) {

    Verbose(String.Format("Resolving deps for package id:[{0}]", id));

    var result = new List<NuSpecDependency>();
    var specs = jsonConfig["customNuspecs"];

    foreach(var spec in specs) {
        var specId = (string)spec["Id"];

        if(specId == id) {

            return (string)spec["Version"];
        }
    }

    throw new Exception(String.Format("Cannot resolve version for package:[{0}]", id));
}

ChocolateyPackSettings[] ResolveChocolateyPackSettings() {

    Verbose("Resolving Chocolatey specs..");
    var result = new List<ChocolateyPackSettings>();

    var specs = jsonConfig["customChocolateySpecs"];

    foreach(var spec in specs) {

        var packSettings = new ChocolateyPackSettings();

        packSettings.Id = (string)spec["Id"];
        packSettings.Version = (string)spec["Version"];

        if(spec["Authors"] == null)
            packSettings.Authors =  new [] { "SubPoint Solutions" };
        else
            packSettings.Authors = spec["Authors"].Select(t => (string)t).ToArray();

        if(spec["Owners"] == null)
            packSettings.Owners =  new [] { "SubPoint Solutions" };
        else
            packSettings.Owners = spec["Owners"].Select(t => (string)t).ToArray();
        
        packSettings.LicenseUrl = new System.Uri((string)spec["LicenseUrl"]);
        packSettings.ProjectUrl = new System.Uri((string)spec["ProjectUrl"]);
        packSettings.IconUrl = new System.Uri((string)spec["IconUrl"]);

        packSettings.Description = (string)spec["Description"];
        packSettings.Copyright = (string)spec["Copyright"];

        packSettings.Tags = spec["Tags"].Select(t => (string)t).ToArray();

        packSettings.RequireLicenseAcceptance = false;

        var files = spec["Files"].Select(t => t).ToArray();

        Verbose(String.Format("- resolving files [{0}]", files.Count())); 
        packSettings.Files = files.SelectMany(target => {
            
               Verbose(String.Format("Processing file set...")); 

               var result1 = new List<ChocolateyNuSpecContent>();
               
               var targetName = (string)target["Source"];
               Verbose(String.Format("- target name:[{0}]", targetName)); 

               var targetFilesFolder = (string)target["SolutionRelativeSourceFilesFolder"];
               Verbose(String.Format("- target files folder:[{0}]", targetFilesFolder)); 

               var targetFiles = target["SourceFiles"].Select(t => (string)t).ToArray();
               Verbose(String.Format("- target files:[{0}]", targetFiles.Count())); 

               var absFileFolder = System.IO.Path.GetFullPath(System.IO.Path.Combine(defaultSolutionDirectory, targetFilesFolder));

                foreach(var f in targetFiles)
                {
                    Verbose(String.Format("     - resolving file:[{0}]", f)); 

                    if(f.Contains("**")) {

                        var dstFolder = f.Replace("**", String.Empty).TrimEnd('\\').Replace('\\', '/');
                        var srcFolder = System.IO.Path.GetFullPath(
                                                System.IO.Path.Combine(defaultSolutionDirectory,
                                                    System.IO.Path.Combine(targetFilesFolder, dstFolder)));

                        if(!System.IO.Directory.Exists(srcFolder))
                            throw new Exception(String.Format("Directory does not exist: [{0}]"));

                        var chAbsSrcDir = srcFolder +  @"\**";
                        var chDstDir = targetName + "/" + dstFolder.TrimEnd('/');

                        Verbose(String.Format("     - resolved as:[{0}]", chAbsSrcDir)); 

                        result1.Add( new ChocolateyNuSpecContent{
                            Source = chAbsSrcDir,
                            Target = chDstDir
                        });
                    }
                    else{
                        
                        
                        var singleFileAbsolutePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(absFileFolder, f));
                        
                        Verbose(String.Format("     - resolved as:[{0}]", singleFileAbsolutePath)); 

                        if(!System.IO.File.Exists(singleFileAbsolutePath))
                            throw new Exception(String.Format("File does not exist: [{0}]", singleFileAbsolutePath));

                        result1.Add( new ChocolateyNuSpecContent{
                            Source = singleFileAbsolutePath,
                            Target = targetName
                        });
                    }
                }

                return result1;

            }).ToList();

        packSettings.OutputDirectory = new DirectoryPath(defaultChocolateyPackagesDirectory);

        result.Add(packSettings);
    }

    return result.ToArray();
}

NuGetPackSettings[] ResolveNuGetPackSettings() {

    var result = new List<NuGetPackSettings>();

    var specs = jsonConfig["customNuspecs"];

    foreach(var spec in specs) {

        var packSettings = new NuGetPackSettings();

        packSettings.Id = (string)spec["Id"];
        packSettings.Version = (string)spec["Version"];

        packSettings.Dependencies = ResolveDependenciesForPackage(packSettings.Id);

        if(spec["Authors"] == null)
            packSettings.Authors =  new [] { "SubPoint Solutions" };
        else
            packSettings.Authors = spec["Authors"].Select(t => (string)t).ToArray();

        if(spec["Owners"] == null)
            packSettings.Owners =  new [] { "SubPoint Solutions" };
        else
            packSettings.Owners = spec["Owners"].Select(t => (string)t).ToArray();
        
        packSettings.LicenseUrl = new System.Uri((string)spec["LicenseUrl"]);
        packSettings.ProjectUrl = new System.Uri((string)spec["ProjectUrl"]);
        packSettings.IconUrl = new System.Uri((string)spec["IconUrl"]);

        packSettings.Description = (string)spec["Description"];
        packSettings.Copyright = (string)spec["Copyright"];

        packSettings.Tags = spec["Tags"].Select(t => (string)t).ToArray();

        packSettings.RequireLicenseAcceptance = false;
        packSettings.Symbols = false;
        packSettings.NoPackageAnalysis = false;

        var projectPath = System.IO.Path.Combine(defaultSolutionDirectory, packSettings.Id);
        var projectBinPath = System.IO.Path.Combine(projectPath, "bin/debug");

        packSettings.BasePath = projectBinPath;

        packSettings.Files = new [] {
                new NuSpecContent {
                    Source = packSettings.Id + ".dll",
                    Target = "lib/net45"
                },
                new NuSpecContent {
                    Source = packSettings.Id + ".xml",
                    Target = "lib/net45"
                }
        };

        packSettings.OutputDirectory = new DirectoryPath(defaultNuGetPackagesDirectory);

        result.Add(packSettings);
    }

    return result.ToArray();
}

NuSpecDependency[] ResolveDependenciesForPackage(string id) {

    Verbose(String.Format("Resolving deps for package id:[{0}]", id));

    var result = new List<NuSpecDependency>();
    var specs = jsonConfig["customNuspecs"];

    foreach(var spec in specs) {
        var specId = (string)spec["Id"];

        if(specId == id) {

            var deps = spec["Dependencies"];

            foreach(var dep in deps) {
                result.Add(new NuSpecDependency() {
                    Id = (string)dep["Id"],
                    Version = (string)dep["Version"],
                });
            }

            break;
        }
    }

    foreach(var dep in result) {
        Information(String.Format(" - ID:[{0}] Version:[{1}]", dep.Id, dep.Version));
    }

    return result.ToArray();
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

// source solution dir and file
var defaultSolutionDirectory =  GetFullPath((string)jsonConfig["defaultSolutionDirectory"]); 
var defaultSolutionFilePath = GetFullPath((string)jsonConfig["defaultSolutionFilePath"]);

// nuget packages
var defaultNuGetPackagesDirectory = GetFullPath((string)jsonConfig["defaultNuGetPackagesDirectory"]);
System.IO.Directory.CreateDirectory(defaultNuGetPackagesDirectory);

var defaultNuspecVersion = (string)jsonConfig["defaultNuspecVersion"];

// chocolatey
var defaultChocolateyPackagesDirectory = GetFullPath((string)jsonConfig["defaultChocolateyPackagesDirectory"]);
System.IO.Directory.CreateDirectory(defaultChocolateyPackagesDirectory);

// test settings
var defaultTestCategories = jsonConfig["defaultTestCategories"].Select(t => (string)t).ToList();
var defaultTestAssemblyPaths = jsonConfig["defaultTestAssemblyPaths"].Select(t => GetFullPath(defaultSolutionDirectory + "/" + (string)t)).ToList();

// build settings
var defaultBuildDirs = jsonConfig["defaultBuildDirs"].Select(t => new DirectoryPath(GetFullPath((string)t))).ToList();
var defaultEnvironmentVariables =  jsonConfig["defaultEnvironmentVariables"].Select(t => (string)t).ToList();

// refine defaultBuildDirs - everything with *.csprj in the folder + /bin
//effectively, looking for all cs projects within solution
defaultBuildDirs.AddRange(GetAllProjectDirectories(defaultSolutionDirectory));

// default dirs for chocol and nuget packages
defaultBuildDirs.Add(ResolveFullPathFromSolutionRelativePath(defaultChocolateyPackagesDirectory));
defaultBuildDirs.Add(ResolveFullPathFromSolutionRelativePath(defaultNuGetPackagesDirectory));

Information("Starting build...");
Information(string.Format(" -target:[{0}]",target));
Information(string.Format(" -configuration:[{0}]", configuration));
Information(string.Format(" -activeBranch:[{0}]", ciBranch));

var defaultNuspecs = new List<NuGetPackSettings>();
defaultNuspecs.AddRange(ResolveNuGetPackSettings());

var defaulChocolateySpecs = new List<ChocolateyPackSettings>();
defaulChocolateySpecs.AddRange(ResolveChocolateyPackSettings());

// validates that all defaultEnvironmentVariables exist
Task("Action-Validate-Environment")
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

// cleans everything from defaultBuildDirs
Task("Action-Clean")
    .Does(() =>
{
    foreach(var dirPath in defaultBuildDirs) {
        CleanDirectory(dirPath);
    }        
});


// restores NuGet packages for default solution
Task("Action-Restore-NuGet-Packages")
    .Does(() =>
{
    NuGetRestore(defaultSolutionFilePath);
});

// buulds current solution
Task("Action-Build")
    .Does(() =>
{
      MSBuild(defaultSolutionFilePath, settings => {
            settings.SetVerbosity(Verbosity.Quiet);
            settings.SetConfiguration(configuration);
      });
});

// runs unit tests for the giving projects and categories
Task("Action-Run-UnitTests")
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

// creates NuGet packages for default NuSpecs
Task("Action-API-NuGet-Packaging")
    .Does(() =>
{
    Information("Creating NuGet packages in directory:[{0}]", new []{
        defaultNuGetPackagesDirectory,
        defaultNuspecVersion
    });

    CreateDirectory(defaultNuGetPackagesDirectory);
    CleanDirectory(defaultNuGetPackagesDirectory);

    var currentIndex = 1;
    var totalCount = defaultNuspecs.Count;

    foreach(var nuspec in defaultNuspecs)
    {   
        nuspec.Version = GetVersionForNuGetPackage(nuspec.Id, defaultNuspecVersion, ciBranch);

        // update deps versions to a correct one during build
        foreach(var dep in nuspec.Dependencies) {
            if(dep.Id.StartsWith("MetaPack")) {
                dep.Version = GetVersionForNuGetPackage(dep.Id, defaultNuspecVersion, ciBranch); 
            }
        }

        Information(string.Format("[{2}/{3}] - Creating NuGet package for [{0}] of version:[{1}]", 
        new object[] {
                nuspec.Id, 
                nuspec.Version,
                currentIndex,
                currentIndex
        }));

        NuGetPack(nuspec);
        currentIndex++;
    }        
});

Task("Action-API-NuGet-Publishing")
    // all packaged should be compiled by NuGet-Packaging task into 'defaultNuGetPackagesDirectory' folder
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

 Task("Action-CLI-Chocolatey-Packaging")
    .Does(() =>
{
      Information("Building CLI - Chocolatey package...");

      foreach(var chocoSpec in defaulChocolateySpecs) {

           chocoSpec.Version = GetVersionForNuGetPackage(chocoSpec.Id, defaultNuspecVersion, ciBranch);

           Information(string.Format("Creating Chocolatey package [{0}] version:[{1}]", chocoSpec.Id, chocoSpec.Version));
           ChocolateyPack(chocoSpec);
       }

      Information(string.Format("Completed creating chocolatey package"));
});

Task("Action-CLI-Zip-Packaging")
    .Does(() =>
{
      Information("Building CLI - Zip package...");

      var cliId = String.Empty;
      var cliVersion = String.Empty;

      foreach(var chocoSpec in defaulChocolateySpecs) {

           cliVersion = GetVersionForNuGetPackage(chocoSpec.Id, defaultNuspecVersion, ciBranch);
           cliId = chocoSpec.Id;
           break;
      }

      var tmpFolderPath = System.IO.Path.Combine( System.IO.Path.GetTempPath(), Guid.NewGuid().ToString("N"));
      System.IO.Directory.CreateDirectory(tmpFolderPath);

    //   var cliFilePaths = prjMetaPackCLIFiles.SelectMany(f => {
                
    //              if(f.Contains("**")) {

    //                   var dstFolder = f.Replace("**", String.Empty).TrimEnd('\\').Replace('\\', '/');
    //                   return System.IO.Directory.GetFiles(System.IO.Path.GetFullPath(prjMetaPackCLIBinPath + @"\" + dstFolder), "*.*");
    //              }

    //            return new []{ System.IO.Path.GetFullPath(prjMetaPackCLIBinPath + @"\" + f) };

    //         }).ToList();

   

    //   Information(string.Format("Creating CLI Zip package [{0}] version:[{1}]", cliId, cliVersion));

    //   var originalFileBase = System.IO.Path.GetFullPath(prjMetaPackCLIBinPath);

    //   Information(string.Format("Copying distr files to TMP folder [{0}]", tmpFolderPath));

    //   foreach(var filePath in cliFilePaths) {
        
    //     Information(string.Format("src file:[{0}]", filePath));    
        
    //     var absPath = filePath
    //                     .Replace(originalFileBase, "")
    //                     .Replace(System.IO.Path.GetFileName(filePath), "")
    //                     .Trim('\\')
    //                     .Trim('/');
        
    //     var dstDirectory = System.IO.Path.Combine(tmpFolderPath, absPath);
    //     var dstFilePath = System.IO.Path.Combine(dstDirectory,  System.IO.Path.GetFileName(filePath));

    //     System.IO.Directory.CreateDirectory(dstDirectory);

    //     Information("- copy from: " + filePath);
    //     Information("- copy to  : " + dstFilePath);
    //     System.IO.File.Copy(filePath, dstFilePath);
    //   }

    //   var cliZipPackageFileName = String.Format("{0}.{1}.zip", cliId, cliVersion);
    //   Information(string.Format("Creating ZIP file [{0}]", cliZipPackageFileName));
    //   Zip(tmpFolderPath, cliZipPackageFileName);

    //   Information(string.Format("Calculating checksum..."));

    //   var md5 = CalculateFileHash(cliZipPackageFileName, HashAlgorithm.MD5);
    //   var sha256 = CalculateFileHash(cliZipPackageFileName, HashAlgorithm.SHA256);
    //   var sha512 = CalculateFileHash(cliZipPackageFileName, HashAlgorithm.SHA512);

    //   Information(string.Format("-md5    :{0}", md5.ToHex()));
    //   Information(string.Format("-sha256 :{0}", sha256.ToHex()));
    //   Information(string.Format("-sha512 :{0}", sha512.ToHex()));

    //   var md5File = cliZipPackageFileName + ".MD5SUM";
    //   var sha256File = cliZipPackageFileName + ".SHA256SUM";
    //   var sha512File = cliZipPackageFileName + ".SHA512SUM";

    //   System.IO.File.WriteAllLines(cliZipPackageFileName + ".MD5SUM", new string[]{
    //       String.Format("{0} {1}", md5.ToHex(), md5File)
    //   });

    //   System.IO.File.WriteAllLines(cliZipPackageFileName + ".SHA256SUM", new string[]{
    //       String.Format("{0} {1}", sha256.ToHex(), sha256File)
    //   });

    //   System.IO.File.WriteAllLines(cliZipPackageFileName + ".SHA512SUM", new string[]{
    //       String.Format("{0} {1}", sha512.ToHex(), sha512File)
    //   });

    //   var distrFiles = new string[] {
    //       cliZipPackageFileName,
    //       md5File,
    //       sha256File,
    //       sha512File
    //   };

    //   Information("Final distributive:");
    //   foreach(var filePath in distrFiles)
    //   {
    //       Information(string.Format("- {0}", filePath));
    //   }
});

// Action-XXX - common tasks
// * Action-Validate-Environment
// * Action-Clean
// * Action-Restore-NuGet-Packages
// * Action-Build
// * Action-Run-Unit-Tests

// * Action-API-NuGet-Packaging
// * Action-API-NuGet-Publishing

// * Action-CLI-Zip-Packaging
// * Action-CLI-Zip-Publishing

// * Action-CLI-Chocolatey-Packaging
// * Action-CLI-Chocolatey-Publishing

// basic common targets
Task("Default")
    .IsDependentOn("Default-Run-UnitTests");

Task("Default-Clean")
    .IsDependentOn("Action-Validate-Environment")
    .IsDependentOn("Action-Clean");

Task("Default-Build")
    .IsDependentOn("Default-Clean")
    .IsDependentOn("Action-Build");

Task("Default-Run-UnitTests")
    .IsDependentOn("Default-Build")
    .IsDependentOn("Action-Run-UnitTests");    

// API related targets
Task("Default-API-NuGet-Packaging")
    .IsDependentOn("Default-Run-UnitTests")
    .IsDependentOn("Action-API-NuGet-Packaging");

Task("Default-API-NuGet-Publishing")
    .IsDependentOn("Default-Run-UnitTests")
    .IsDependentOn("Action-API-NuGet-Packaging")
    .IsDependentOn("Action-API-NuGet-Publishing");  

// CLI related targets
Task("Default-CLI-Packaging")
    .IsDependentOn("Default-Run-UnitTests")
    .IsDependentOn("Action-API-NuGet-Packaging")

    .IsDependentOn("Action-CLI-Zip-Packaging")
    .IsDependentOn("Action-CLI-Chocolatey-Packaging");  

Task("Default-CLI-Publishing")
    .IsDependentOn("Default-CLI-Packaging");

// CI related targets
Task("Default-CI")
    .IsDependentOn("Default-Run-UnitTests")
    .IsDependentOn("Action-API-NuGet-Packaging")
    .IsDependentOn("Action-CLI-Zip-Packaging")
    .IsDependentOn("Action-CLI-Chocolatey-Packaging");