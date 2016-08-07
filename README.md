# MetaPack
The package manager for SPMeta2 based SharePoint customizations.

Deploying, updating and managing SharePoint customizations takes effort. Not only deployment but also versioning, modularization and dependency management require too much effort slowing down development, expanding cost and the delivery dates. Time spent on writing plumbing code, scripts or other means to handle all these areas should be spend on really important things. 

MetaPack introduces a complete solution for packaging, versioning, deploying and updating SPMeta2 based customizations. It is built on top of NuGet platform and offers a smooth API for developers, a CLI for better CI/CD scenarios and a handy GUI app for IT-pros.

### Build status
[![Build status](https://ci.appveyor.com/api/projects/status/metapack?svg=true)](https://ci.appveyor.com/project/SubPointSupport/metapack)

### MetaPack in details

#### Introduces solution packaging
Packaging SharePoint customization is never an easy task. Should it be console app? Should it be provider hosted app? A PowerShell script?

Don't worry. MetaPack packages your solution as a self-contained NuGet package. As simple as that.

#### Handles solution versioning
Version history is another pain point while delivering SharePoint customizations. It is not easy to keep track of all customizations deployed not to talk about versioning them.

MetaPack uses NuGet Gallery infrastructure to provide solution version tracking. Semantic versioning naturally comes along.

#### Makes dependency management possible
Did you ever want to reuse and modularize your customizations so that you can compose bigger building blocks? We know it's hard to implement.

MetaPack brings the best of NuGet platform: package dependency management, versioning and easy modularization.

#### Simplifies deployment and updates
Solution life-cycle does not end with the first deployment. New features are built, new versions are released so that a smooth update process is a must.

MetaPack offers the best experience ever to deploy and update your models. It handles all the details and even shows if updates are available.


#### Offers API, a CLI and user friendly GUI
Modern software development blurs the boundaries between developers, IT-pros and business. Team needs to work closely having a solid, smooth delivery workflow.

MetaPack offers ultimate experience for all team: developers leverage API, IT-pros have a CLI and business have a friendly GUI based application.


#### Improves CI/CD story
Ultimately, MetaPack not only helps to ship SharePoint customizations to the client but also helps to improve continues integration and deployment story.

Create SPMeta2 models in Visual Studios, use API to create NuGet packages, ship them the way you like: API, CLI or GUI - it's all yours.


As for the API, that's how we see it happening:
```cs


```

#### Feature requests, support and contributions
In case you have unexpected issues or keen to see new features please contact support on SPMeta2 Yammer or here at github:

* [https://www.yammer.com/spmeta2feedback](https://www.yammer.com/spmeta2feedback/#/threads/inGroup?type=in_group&feedId=7897894&view=all)
