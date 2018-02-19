----
Title: Getting started
Order: 400
TileLink: true
TileLinkOrder: 400
----

Welcome to MetaPack getting started guide. 
We cover here basics ideas on which MetaPack exists, simple scenarios to get some sense of MetaPack and then you can explore full [C# API reference](/metapack/reference), think about advanced options with [extendibility API](/metapack/extensibility/) 
and, finally, try out [command-line interface](/metapack/cli/).

## Thinking MetaPack way
In this day and age, there are many ways to create and deliver SharePoint solutions. As development cycle goes on, not only more solutions are created, but more versions of the same solutions are done. 
Most of the times, every solution has its own, unique way to deploy and upgrade, it is usually tightly coupled and hard to reuse. 
At some point, it becomes extremely hard to scale, introduce automation or reuse already created solutions.

### 1 - Modulalize your solutions
If you ever worked with .NET, think about SharePoint solutions same way as you think about assemblies and NuGet packages. 
Every assembly has version, it can reference other assemblies via NuGet packages and NuGet takes care if all versining and dependency resolution.

MetaPack does the same for SharePoint solutions. 
It turns your SharePoint customizations into a NuGet package so that you can start thinking "NuGet package level": with versioning and references to other packages concepts applied to SharePoint solutions.
Now all your customizations can be turned into NuGet packages, versioned, have references to other packages. 

The fact that SharePoint customizations can be broken down into small, self-contained modules and then packed as NuGet packages is the first step of the "thinking MetaPack way".

### 2 - Use NuGet Galleries as a solution delivery platform
MetaPack is built on top of NuGet platform so that all MetaPack packages can be pushed to any NuGet Gallery. In that case, NuGet Gallery plays an "application marketplace" role storing all SharePoint customizations you created.

There are many ways to get your own NuGet Gallery up and running - it can be anything from a folder on a shared drive, an IIS based NuGet deployment, it can also be Azure/AWS hosted NuGet Gallery or you can get a ready-to-go, hosted NuGet Gallery powered by http://myget.org - they are free to use and there are cheap commercial plans available for hosting private NuGet Galleries.

### 3 - Deploy the way you like
Once packages are in NuGet gallery, MetaPack can discover them and deploy straight to SharePoint. It works the same way as NuGet does - installing not only the target package but also resolving and installing all dependencies if any.

MetaPack abstract actual provisioning process. Out of the box, MetaPack has integrations with SharePointPnP and SPMeta2 provisioning libraries. These are the most popular libraries for modern "remote provisioning" pattern to deliver SharePoint solutions. If that's not enough, extensible API allows a creation of a custom packaging and deployment pipelines so that you can control every aspect of it.

MetaPack comes in several flavours: API for developers, a command-line interface for both developers and IT professionals, and a friendly GUI application for power users. It aims to cover most of the delivery and deployment scenarios exist in SharePoint space.

## Get started with MetaPack API
Let's get started with MetaPack API and see how it can be used for packaging and deploying your SharePoint customizations for both SPMeta2 and SharePointPnP libraries. We cover a high-level overview enough to get started, and the rest of the scenarios and technical details can be found in [API reference](/metapack/reference).

### Creating MetaPack package with API
The easiest way to get started with solution packaging is to use MetaPack's integrations for SPMeta2 and SharePointPnP. Both integrations are C# libraries which provide specific PnP/SPMeta2 implementations for model packaging and deployment.

* MetaPack.SharePointPnP - enables SharePointPnP model packaging and deployment 
* MetaPack.SPMeta2 - enables SPMeta2 model packaging and deployment

Both packages are available in NuGet so that you can install them via "Package Manager Console" as simple as following:
* Install-Package MetaPack.SPMeta2 
* Install-Package MetaPack.SharePointPnP 

For the beta versions of the packages use -IncludePrerelease flag:
* Install-Package MetaPack.SPMeta2 -Pre
* Install-Package MetaPack.SharePointPnP -Pre

MetaPack is notmally available via NuGet but the latest dev builds can also be found via myget feed:
* https://www.myget.org/F/subpointsolutions-staging/api/v2/package

Once done, use the following code to create a solution package for your SPMeta2 models and SharePointPnP solutions.
In both cases, you need to create a new solution package first, and then use appropriate packaging service to create a NuGet package. Here is how it goes:
* Create new instance of SolutionPackageBase class
* Fill out package metadata, such as Id, Title and so on
* Add PnP/SPMeta2 models to newly created package
* Use SharePointPnPSolutionPackageService or SPMeta2SolutionPackageService packaging service to create actual NuGet package

SolutionPackageBase is a wrapper over a NuGet package, and it fully follows NuGet package spec design - https://docs.nuget.org/ndocs/schema/nuspec

Both PnP and SPMeta2 models are added to a package as byte[] arrays. It is done intentionally to eliminate a hard dependency on either of the libraries as well as to avoid library versioning issues.

Both models and solution package can have additional options such as name-value pairs which are used by deployment services to understand how the models have to be provisioned. 

Currently, one of the options for SPMeta2/PnP models is "Order" - so that we can control the order in which models are deployed and "ModelType" - which is a type of the model as add. By default, SPMeta2 models have to be added as serialized XML and PnP solutions have to be added as OpenXML packages. These are only supported types for the time being, we'll add more flexibility supporting other PnP solutions down the road.

Once you created a solution package, use either SharePointPnPSolutionPackageService or SPMeta2SolutionPackageService to create an actual NuGet package. Both services support various scenarios as following:
* Pack as Stream
* Pack to file
* Push to NuGet Gallery

The following example illustrates SPMeta2 models packaging:
<a href="_samples/index-Create_Package_SPMeta2.sample-ref"></a>

The following example illustrates SharePointPnP solution packaging:
<a href="_samples/index-Create_Package_PnP.sample-ref"></a>

### Deploying MetaPack package with API
The previous paragraph covers solution packaging so that by this time you should already have your package as a *.nupkg file or pushed to NuGet Gallery. It is time to deploy your package to SharePoint.

MetaPack provides "DefaultMetaPackSolutionPackageManager" which helps connect NuGet Gallery with SharePoint and deploy solution packages. Provisioning flow looks as following:

* Connect to SharePoint using ClientContext  
* Connect to NuGet Gallery creating new instance of IPackageRepository
* Create new instance of DefaultMetaPackSolutionPackageManager()
* Find your package with IPackageRepository
* Deploy your package with DefaultMetaPackSolutionPackageManager

MetaPack actually implements a special type of NuGet package manager which understands SharePoint. By default, NuGet package manager work with the file system (such as the one within your Visual Studio, for instance). MetaPack implements a special file system provider for NuGet so that package manager can work with SharePoint and store installed packages in SharePoint library. That's how it keeps tracking of all packages installed - they are all stored in a special SharePoint library MetaPack creates.

Once you find your NuGet package, you can call .InstallPackage() method passing actual package. ClientContext is used to connect to SharePoint, and then MetaPack checks if SharePoint has "metapack-gallery" library exists creating once if it does not. Next, MetaPack resolved all package dependencies installing each of them before installing your package - exactly the same way as NuGet does.

Finally, MetaPack unpacks solution packages using the right provider, fetches models and deployed them using the right deployment provider. That's why you had to setup either  "MetaPack.SharePointPnP" or "MetaPack.SPMeta2" as a solution tool package id while packing your solution early. MetaPack installs the right providers, looks for the deployment service and then passed unpacked model to be provisioned to SharePoint.

A set of options is available to tune deployment provider behavior. All of the options are passed as name-value collection via .SolutionOptions property. Here are the basic options passed down to a deployment provider:

SharePoint environment:
* **DefaultOptions.SharePoint.Api.CSOM** - CSOM/SSOM   
* **DefaultOptions.SharePoint.Api.Edition** - Foundation/Standard
* **DefaultOptions.SharePoint.Version.O365** - O365/SP2013/S2016

SharePoint credentials:
* **UserName** - actual SharePoint user name
* **UserPassword** - actual SharePoint user password

Be aware that If you use O365, then username and password are used with SharePointOnlineCredentials. For SP2013/2016, NetworkCredential will be used if non-empty username and password were provided (so that you can provision remotely on-remises) and default credentials will be used if no username/password were provided (as if you were on actual SharePoint box).

Deployment API is a little bit verbose so that we suggest to create some high-level wrappers to tune MetaPack as per your business needs. Otherwise, you can use [MetaPack CLI](/metapack/cli) to deploy solution packages from command-line.

Here is how package installation looks like with MetaPack API:
<a href="_samples/Index-Install_Package.sample-ref"></a>

## Next steps
Hope the current guide give some understanding of MetaPac API. You can continue with API reference and CLI. Don't forget to [join the community](https://www.yammer.com/spmeta2feedback) and share your ideas as well.