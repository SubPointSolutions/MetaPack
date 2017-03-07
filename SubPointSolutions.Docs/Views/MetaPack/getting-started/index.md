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

Once done, use the following code to create a solution package for your SPMeta2 models: 

<a href="_samples/index-Create_Package_SPMeta2.sample-ref"></a>

And the following code to create a solution package for SharePointPnP solutions:
<a href="_samples/index-Create_Package_PnP.sample-ref"></a>


### Deploying MetaPack package with API



## Next steps
This and that and something else