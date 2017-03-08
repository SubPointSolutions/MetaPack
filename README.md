# MetaPack
MetaPack is a platform for packaging, delivering and deploying SharePoint customization. MetaPack abstracts away packaging, versioning, dependency management, deploying and updating pipelines offering a complete and consistent workflow instead.

See it that way: there are many ways to build and deliver SharePoint solutions - from using old fashioned *.wsp packages, custom PowerShell scripts, to modern remote provisioning approaches using SharePoint PnP or SPMeta2 libraries. But building actual "installation package" takes effort, a fair share of a pluming code while the actual outcome, such as a console application or PowerShell script, is highly inconsistent, hard to scale and support.

That is where MetaPack comes! It offers a common, standardized and extensible platform for packaging, delivering and deploying SharePoint solutions regardless of the actual framework or provisioning library used.

### Build status
| Branch  | Status |
| ------------- | ------------- |
| dev   | [![Build status](https://ci.appveyor.com/api/projects/status/1weq7g33dfp3xi6i/branch/dev?svg=true)](https://ci.appveyor.com/project/SubPointSupport/metapack/branch/dev)  |
| beta  | [![Build status](https://ci.appveyor.com/api/projects/status/1weq7g33dfp3xi6i/branch/beta?svg=true)](https://ci.appveyor.com/project/SubPointSupport/metapack/branch/beta)  |
| master| [![Build status](https://ci.appveyor.com/api/projects/status/1weq7g33dfp3xi6i/branch/master?svg=true)](https://ci.appveyor.com/project/SubPointSupport/metapack/branch/master) |

## How does MetaPack work?
MetaPack is built on top of NuGet platform so that it leverages all NuGet features and concepts: from packaging, versioning, dependency management to NuGet Galleries. First of all, MetaPack API can create a NuGet package out of SharePoint customizations. Next, once target NuGet package is published to NuGet Gallery or local file system, MetaPack API can deploy target NuGet package straight to SharePoint. No more *.wsp, console apps or PowerShell scripts - all customizations is a NuGet package!

MetaPack integrates with the most popular provisioning libraries for SharePoint: SharePointPnP and SPMeta2. Regardless of the library you use, MetaPack creates a NuGet package out of SPMeta2 or PnP solutions. That said, taking care of packaging, versioning and dependencies, MetaPack delegates actual provision of a package to SPMeta2 or SharePointPnP.

Here is how it works for both SharePointPnP and SPMeta2: 
![MetaPack Vision](https://subpointsolutions-dev.netlify.com/content/img/products/metapack/metapack-vision.png)

## Key features of MetaPack
Below are some of the key features of MetaPack and even more scenarios on how MetaPack can be used:

* **Standardized solution packaging**: MetaPack abstracts away packaging of your SharePoint customizations. You don't have to think about how to package and deliver your SharePoint customization - you always have a NuGet package as a smallest "module"
* **Standardized versioning**: version your SharePoint customizations same way as you version you APIs: use semantic versioning across the board, use NuGet packages and NuGet Galleries
* **Dependency management**: modularize your SharePoint customizations in the most natural, industry-adopted way - using NuGet versioning and its dependency management. MetaPack understands dependencies: it resolves and deploys required packages same way as NuGet does
* **Extensible API**: MetaPack provides extensible API so that it is possible to implement a custom packaging and deployment workflows. For instance, you can implement your own licensing asking people a license key before deploying SharePoint customizations with MetaPack.
* **MetaPack CLI**: built for developers, MetaPack also offers a command-line interface for both developers and IT professionals. That enables easy integration of MetaPack with existing CI/CD pipelines as well as IT professionals can deploy SharePoint solutions with a single line command
* **MetaPack GUI** (coming soon): we would like to enable business users and IT professionals with a friendly GUI application to manage and deploy SharePoint customization. It is work in progress but let us know what you think of it

## How MetaPack can be used?
Standardized packages, versioning and dependency management coupled with out of the box support for SharePointPnP/SPMeta2 and extensible API makes other interesting scenarios possible:

* **A global, community-driven solution catalog** -  why not to create a public NuGet gallery devoted to open-source, community-driving SharePoint solutions? Let us know if you want one!
* **A private, corporate solution catalog** - why not deploy NuGet gallery internally in your company and deliver your solutions for SharePoint that way?
* **A private solution catalog for your customers** -  are you a SharePoint consultancy? Why not deploy a private NuGet gallery to deliver solutions for your customers? You can use http://myget.org to get one as cheap as $7 a month
* **You own packaging and licensing** - are you an ISV company? Why not implement your own packaging provider asking people for a commercial license key before actually deploying your solution?
* **API-independent solutions provision orchestrations** - why not to mix few SharePointPnP and SPMeta2 solutions in a single batch? MetaPack takes care of all details so that we can focus on WHAT to deploy rather HOW to deploy

## Next steps
* [MetaPack Documentation](http://docs.subpointsolutions.com/metapack)
* [Getting started guide](http://docs.subpointsolutions.com/metapack/getting-started)
* [MetaPack CLI](http://docs.subpointsolutions.com/metapack/cli)
* [DefinitelyPacked - a repository for common, community-driven MetaPack packages](https://github.com/SubPointSolutions/DefinitelyPacked)

## Feature requests, support and contributions
MetaPack is a part of the SPMeta2 ecosystem. In case you have unexpected issues or keen to see new features please contact support on SPMeta2 Yammer or here at github:

* [Yammer Community](https://www.yammer.com/spmeta2feedback)
* [MetaPack @ GitHub](https://github.com/SubPointSolutions/MetaPack)