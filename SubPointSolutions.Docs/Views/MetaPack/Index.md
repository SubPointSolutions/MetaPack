---
Title: 'MetaPack Home'
Tile: true
TileTitle: 'MetaPack'
TileOrder: 50
TileLink: true
TileLinkOrder: 9
TileDescription: 'The package manager for SharePoint customizations.'
---

Welcome to the introduction guide to MetaPack! It is the best place to start with basics of MetaPack project. We explain what MetaPack is, what kind of problems it solves, how to get started with MetaPack.

If you are already familiar with the basics, the documentation provides reference for all available API and features.

## What is MetaPack?

MetaPack is a platform for packaging, delivering and deploying SharePoint customization. MetaPack abstracts away packaging, versioning, dependency management, deploying and updating pipelines offering a complete and consistent workflow instead. 

See it that way: there are many ways to build and deliver SharePoint solutions - from using old fashioned  *.wsp packages, custom PowerShell scripts, to modern remote provisioning approaches using SharePoint PnP or SPMeta2 libraries. Building actual "installation package" takes effort, a fair share of pluming code while the actual outcome, such as a console application or PowerShell script is highly inconsistent.

That's where MetaPack shines! It offers a common, standardized and extensible way to package, deliver and deploy SharePoint solutions regardless of the actual framework or provisioning library used. 

## How does MetaPack work?

MetaPack is built on top of NuGet platform leveraging all packaging, versioning and dependency management features of NuGet itself. MetaPack API allows us to create a NuGet package out of our customizations. Next, once target NuGet package is published to NuGet Gallery (or local file system), target NuGet package can be deployed by MetaPack straight to SharePoint. 

MetaPack integrates with SPMeta2 and SharePoint PnP offering a way to create a NuGet package out of SPMeta2 or PnP models. Taking care of packaging, versioning and dependencies, MetaPack delegates actual provision of a package to SPMeta2 or SharePoint PnP.

Here is how it works:
![MetaPack Vision](https://subpointsolutions-dev.netlify.com/content/img/products/metapack/metapack-vision.png)

## Key features of MetaPack
The key features of MetaPack are:

	• Standardized solution packaging: MetaPack abstracts away packaging of your SharePoint customizations. Neither you have to write another console application or PowerShell script, nor you have to think on how to package your customization - you always have a NuGet package as a smallest "solution package"
	• Standardized versioning: version your SharePoint customizations same way as you version you APIs: use semantic versioning across the board.
	• Dependency management: with MetaPack not only you can package your SharePoint solutions but you can also modularize them in most natural, industry adopted way - using NuGet versioning and dependency management. MetaPack understands dependencies, resolves and deploys required packages same way as NuGet would
	• Extensible API: MetaPack offers extensible API so that you can write your own packaging and deployment providers. You can implement your own licensing asking people a license key before deploying your solution.
	• MetaPack CLI: MetaPack offers a command-line interface. That enables easy integration of MetaPack with existing CI/CD pipelines as well as IT professionals can deploy SharePoint solutions with a single line command
	• MetaPack GUI (coming soon): we would like to enable business used and IT professionals with a friendly GUI application to manage and deploy SharePoint customization. It's work in progress but let us know what you think of it

## How MetaPack can be used?
Standardized packages, versioning and dependency management coupled with out of the box support  for SharePointPnP/SPMeta2 and extensible API makes other interesting scenarios possible:

	• A global, community-driven solution catalog -  why not to create a public NuGet gallery devoted to open-source, community-driving solutions? Let us know if you want one!
	• A private, corporate solution catalog  - why not to deploy NuGet gallery internally in your company and deliver your solutions for SharePoint that way?
	• A private solution catalog for your customers -  are you a SharePoint consultancy? Why not to deploy a private NuGet gallery to deliver solutions for your customers? You can use http://myget.org to get one as cheap as $7 a month
	• You own packaging and licensing  - are you an ISV company? Why not to implement your own packaging provider asking people for a commercial license key before actually deploying your solution?
	• API-independent solutions provision orchestrations  - why not to mix few SharePointPnP and SPMeta2 solutions in a single batch? MetaPack takes care on all details and we can focus on WHAT to deploy rather HOW to deploy

## Next steps
Have more question or keen to learn more about MetaPack? Continue with getting started guide,  API reference and use cases page. Don't forget to join the community and share your ideas as well. Also, MetaPack is an open source project hosted at github, it's all yours.
