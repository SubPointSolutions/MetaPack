----
Title: CLI
Order: 600
TileLink: true
TileLinkOrder: 600
Hidden: false
----

Welcome to MetaPack CLI guide. MetaPack comes in different flavors: a set of C# API targets SharePoint developers, and the command-line interface takes away all the coding routines enabling IT professionals to deliver and deploy SharePoint solutions in a single line.

MetaPack CLI connects NuGet Gallery and SharePoint within a commend-line interface. No coding is required, and it's got its own mission - "SharePoint customizations should be one line away" - meaning that it should be really easy to install and update SharePoint solutions within a single line or so. For instance, a solution package "Contoso.Intranet.SiteFiels" can be installed to SharePoint site "http://contoso-intranet.local" with the following command:

```ps
metapack install -id Contoso.Intranet.SiteFiels -url http://contoso-intranet.local
```

That said, MetaPack CLI can be used in several scenarios:
* Enhance continuous integration and continuous deployment pipelines 
* Provide IT profession with easy way to install and update SharePoint solutions

## Installing MetaPack CLI

There are two ways to get MetaPack CLI installed - using a single *.zip package or using Chocolatey package manager.

First of all, ensure that you've got [Chocolatey](https://chocolatey.org/install) installed. Next, run the following command to get the latest MetaPack CLI installed:

```ps
choco install metapack 
```

You can also install the recent MetaPack CLI from the build feed:
```ps
choco install metapack -source https://ci.appveyor.com/nuget/metapack-ci --force --prerelese
```

Once done, check that metapack was installed running one of the following commands:

```ps
metapack
metapack version
```

The outcome should look as following:
```ps
[Information]: Metapack client v1.0.17049.1225
[Information]: Working directory: [C:\ProgramData\chocolatey\lib\MetaPack\lib\metapack]
[Information]: Using NuGet galleries:[

    http://metapackgallery.com/api/v2
    https://packages.nuget.org/api/v2
    http://metapackgallery.com/api/v2
    https://www.myget.org/F/subpointsolutions-staging/api/v2
]
[Information]: MetaPack.Client 1.0.0.0
Copyright ©  2016

  --install    Install package to SharePoint web site. Use 'install --help' for
               more information.

  --update     Update package on SharePoint web site. Use 'update --help' for
               more information.

  --list       List installed packaged on SharePoit web site. Use 'list --help'
               for more information.

  --push       Push a packaged to NuGet gallery. Use 'push --help' for more
               information.

  --version    Shows current version of CLI.

  --help       Display this help screen.


[Information]: Cannot find arguments. Exiting.
```

You can also get metapack zip package. Check the following link and get *.zip file whith looks as "MetaPack.0.1.0-alpha170660508.zip":
* https://ci.appveyor.com/project/SubPointSupport/metapack/build/artifacts

Download it, right clicn -> Properties -> Unblock it, and then extract all the content into a new folder.
Checksums are also avialable via build links and looks as following:
* MetaPack.0.1.0-alpha170660508.MD5SUM
* MetaPack.0.1.0-alpha170660508.SHA256SUM
* MetaPack.0.1.0-alpha170660508.SHA512SUM

## Installing packages with MetaPack CLI

MetaPack CLI is heavility nfluence by nuget.exe and provides similar subset of commands to manage and install solutions packages but in a context of SharePoint:

* **instal** - installs a target package to SharePoint web site
* **update** - updates target package installing the latest version available
* **list** - shows packages installed on a target SharePoint web site 
* **push** - pushes package to NuGet Gallery
* **version** - shows the current version of the CLI
* **help** - shows help

Every command has additional parameters to address the complexity of a particular deployment scenario. Use "help" switch  to get more information regarding additional parameters available:
```ps
metapack install help
metapack list help
```

Most of the command require id of the package and target SharePoint url. You may also provide a target version of the package and source of the NuGet Gallery to use:
* **--id** - ID of the target NuGet package
* **--version** - version of the target package
* **--source** - url of the NuGet gallery to use
* **--url** - url of the target SharePoint web site

By default, in beta version the following NuGet Galleries are used:
* http://metapackgallery.com/api/v2
* https://packages.nuget.org/api/v2
* https://www.myget.org/F/subpointsolutions-staging/api/v2

Depending of the auth used, you may be required to provide user name and passoword using the following switches:
* **--username** - user name, a simple string
* **--userpassword** - user password, a simple string

Next, there are several parameters to suggest which SharePoint version and runtime to use - SharePoint 2013, 2016, O365 and CSOM/SSOM. By default O365 and CSOM are used.
* **--spversion** - target version of SharePoint: o365, sp2013, sp2016
* **--spapi** - target SharePoint API to use: CSOM/SSOM


Finally, some additonal trace is available with the verbose swith:
* **--verbose** - shows verbose trace 

Finally, here are some scenarios and examples on how to deploy solution packages with MetaPack CLI.

### Install solution package to O365

```ps
metapack install `
        --id "Contoso.Intranet.SiteFields" `
        --url "http://contoso-intranet.sharepoint.com" `
        --username "user@contoso.com" `
        --userpassword "pass@word1" `
        --spversion "o365"
```

### Install solution package to SP2013

```ps
metapack install `
        --id "Contoso.Intranet.SiteFields" `
        --url "http://contoso-intranet.sharepoint.com" `
        --username "user@contoso.com" `
        --userpassword "pass@word1" `
        --spversion "sp2013"
```


### Install solution package to SP2013 via CSOM

```ps
metapack install `
        --id "Contoso.Intranet.SiteFields" `
        --url "http://contoso-intranet.sharepoint.com" `
        --username "user@contoso.com" `
        --userpassword "pass@word1" `
        --spversion "sp2013"
```

### Install solution package to SP2013 via SSOM

```ps
metapack install `
        --id "Contoso.Intranet.SiteFields" `
        --url "http://contoso-intranet.sharepoint.com" `
        --username "user@contoso.com" `
        --userpassword "pass@word1" `
        --spversion "sp2013" `
        --spapi "SSOM"
```

### Install solution package from custom NuGet Gallery
```ps
metapack install `
        --id "Contoso.Intranet.SiteFields" `
        --source "https://www.myget.org/F/subpointsolutions-staging/api/v2" `
        --url "http://contoso-intranet.sharepoint.com" `
        --username "user@contoso.com" `
        --userpassword "pass@word1" `
        --spversion "sp2013"
```

### Install solution package from shared folder
```ps
metapack install `
        --id "Contoso.Intranet.SiteFields" `
        --source "\\shared-drives\sp-solutions-repository" `
        --url "http://contoso-intranet.sharepoint.com" `
        --username "user@contoso.com" `
        --userpassword "pass@word1" `
        --spversion "sp2013"
```

### Show installed packages on SharePoint web site
```ps
metapack list `
        --url "http://contoso-intranet.sharepoint.com" `
        --username "user@contoso.com" `
        --userpassword "pass@word1" 

```

## Next steps
Hope the current guide give some understanding of MetaPack CLI. Let us know how it works for you, don't forget to [join the community](https://www.yammer.com/spmeta2feedback) and share your ideas as well.
