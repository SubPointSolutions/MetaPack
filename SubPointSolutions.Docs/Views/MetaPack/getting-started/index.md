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
In this day and age there are many ways to create and deliver SharePoint solutions.
As development cycle goes on, not only more solutions are created but more versions of the same solutions is done. 
Most of the times, every solution has its own way to deploy and upgrade, it is usually tighty coupled and hard to reuse. 
At some point it becomes extremely hard to scale, introduce automatation and reuse already created solutions.

### Modulalize your solutions
Now, if you ever worked with .NET, think about SharePoint solutions same way as you think about assemblies and NuGet packages. 
Every assembly has version, it can reference other assemblies via NuGet packages and NuGet takes care if all versining and dependency resolution.

MetaPack does the same for SharePoint solutions. 
It turnes your customizations into a NuGet package so that you can think at "NuGet package level" with versioning and referencies to other packages.
Now all your customizations can be turned into small NuGet packages, versioned, have referencies to other packages. 

### Simplify delivery
It does not stops here. As MetaPack works on top of NuGet, all your packages can be pblished to any NuGet Gallery including shared folders or priveta galleries.


### Deploy the way you like
Once packages are in NuGet gallery, MetaPack can discover them and deploy straight to SharePoint. 
Actual provision is abstracted as well - MetaPack has out of the box integrations with SharePointPnP and SPMeta2. 

## Creating MetaPack package with C# 
The easiest way to start with MetaPack API is use NuGet packages. MetaPack integrates with both SPMeta2 and SharePointPnp so that you can package and deploy both type of cuszo

<a href="_samples/index-Create_Package_SPMeta2.sample-ref"></a>

## Deploying MetaPack package with C#

<a href="_samples/index-Create_Package_PnP.sample-ref"></a>

## Next steps
This and that and something else