﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Core.Packaging;
using MetaPack.Core.Services;
using NuGet;
using System.IO;
using System.Reflection;
using System.Security;
using MetaPack.Core.Common;
using MetaPack.Core.Exceptions;
using MetaPack.NuGet.Common;
using MetaPack.NuGet.Services;
using MetaPack.NuGet.Utils;
using MetaPack.Core.Utils;
using System.Net;

namespace MetaPack.SPMeta2.Services
{
    /// <summary>
    /// Solution package deployment service for SPMeta2 provision.
    /// </summary>
    public class SPMeta2SolutionPackageDeploymentService : SolutionPackageDeploymentService
    {
        private bool Compare(string v1, string v2, bool irnoreCase)
        {
            return string.Compare(v1, v2, true) == 0;
        }

        public override IEnumerable<SolutionToolPackage> GetAdditionalToolPackages(SolutionPackageProvisionOptions options)
        {
            var result = new List<SolutionToolPackage>();

            var mainToolPackageId = "SPMeta2";

            var spVersion = options.GetOptionValue(DefaultOptions.SharePoint.Version.Id);
            var spEdition = options.GetOptionValue(DefaultOptions.SharePoint.Edition.Id);
            var spApi = options.GetOptionValue(DefaultOptions.SharePoint.Api.Id);

            // ensure m2 assemblies
            if (Compare(spApi, DefaultOptions.SharePoint.Api.CSOM.Value, true))
                mainToolPackageId += ".CSOM";
            else if (Compare(spApi, DefaultOptions.SharePoint.Api.SSOM.Value, true))
                mainToolPackageId += ".SSOM";
            else
                throw new MetaPackException(String.Format("Unsuported SharePoint Api:[{0}]", spApi));

            if (Compare(spEdition, DefaultOptions.SharePoint.Edition.Foundation.Value, true))
                mainToolPackageId += ".Foundation";
            else if (Compare(spEdition, DefaultOptions.SharePoint.Edition.Standard.Value, true))
            {
                mainToolPackageId += ".Standard";

                if (Compare(spApi, DefaultOptions.SharePoint.Api.CSOM.Value, true))
                {
                    if (Compare(spVersion, DefaultOptions.SharePoint.Version.O365.Value, true))
                    {
                        result.Add(new SolutionToolPackage
                        {
                            Id = "SPMeta2.CSOM.Foundation-v16",
                            AssemblyNameHint = "SPMeta2.CSOM.dll"
                        });
                    }
                    else
                    {
                        result.Add(new SolutionToolPackage
                        {
                            Id = "SPMeta2.CSOM.Foundation",
                            AssemblyNameHint = "SPMeta2.CSOM.dll"
                        });
                    }
                }

                if (Compare(spApi, DefaultOptions.SharePoint.Api.SSOM.Value, true))
                {
                    result.Add(new SolutionToolPackage
                    {
                        Id = "SPMeta2.SSOM.Foundation",
                        AssemblyNameHint = "SPMeta2.SSOM.dll"
                    });
                }
            }
            else
                throw new MetaPackException(String.Format("Unsuported SharePoint Edition:[{0}]", spEdition));

            if (Compare(spVersion, DefaultOptions.SharePoint.Version.O365.Value, true))
            {
                mainToolPackageId += "-v16";

                // adding main toolpackage
                result.Add(new SolutionToolPackage
                {
                    Id = "Microsoft.SharePointOnline.CSOM",
                    AssemblyNameHint = "Microsoft.SharePoint.Client.dll"
                });

                result.Add(new SolutionToolPackage
                {
                    Id = "Microsoft.SharePointOnline.CSOM",
                    AssemblyNameHint = "Microsoft.SharePoint.Client.Runtime.dll"
                });
            }
            else if (Compare(spVersion, DefaultOptions.SharePoint.Version.SP2013.Value, true))
            {
                if (Compare(spApi, DefaultOptions.SharePoint.Api.CSOM.Value, true))
                {
                    // download stuff from NuGet
                    // loading from GAC is not suported yet

                    // ideally we'd like to get SP1 version

                    // 15.0.4569.1000*	​Service Pack 1	​SharePoint Foundation 2013	​KB2817439	​Download	​Bugs, Notes, and Regressions
                    // ​15.0.4711.1000	​April 2015 CU	​SharePoint Foundation 2013	​KB2965261	​Download	​Bugs, Notes, and Regressions

                    // adding main toolpackage
                    result.Add(new SolutionToolPackage
                    {
                        Id = "Microsoft.SharePoint2013.CSOM",
                        Version = "15.0.4711.1000",
                        AssemblyNameHint = "Microsoft.SharePoint.Client.dll"
                    });

                    result.Add(new SolutionToolPackage
                    {
                        Id = "Microsoft.SharePoint2013.CSOM",
                        Version = "15.0.4711.1000",
                        AssemblyNameHint = "Microsoft.SharePoint.Client.Runtime.dll"
                    });
                }
            }
            else if (Compare(spVersion, DefaultOptions.SharePoint.Version.SP2016.Value, true))
            {
                // same as 2013 yet
                if (Compare(spApi, DefaultOptions.SharePoint.Api.CSOM.Value, true))
                {
                    result.Add(new SolutionToolPackage
                    {
                        Id = "Microsoft.SharePoint2013.CSOM",
                        Version = "15.0.4711.1000",
                        AssemblyNameHint = "Microsoft.SharePoint.Client.dll"
                    });

                    result.Add(new SolutionToolPackage
                    {
                        Id = "Microsoft.SharePoint2013.CSOM",
                        Version = "15.0.4711.1000",
                        AssemblyNameHint = "Microsoft.SharePoint.Client.Runtime.dll"
                    });
                }
            }
            else
            {
                throw new MetaPackException(String.Format("Unsuported SharePoint Version:[{0}]", spVersion));
            }

            // adding main toolpackage
            result.Add(new SolutionToolPackage
            {
                Id = mainToolPackageId
            });

            return result;
        }

        protected virtual string GetProvisionServiceClassName(SolutionPackageProvisionOptions options)
        {
            var provisionServiceClassName = string.Empty;

            var spVersion = options.GetOptionValue(DefaultOptions.SharePoint.Version.Id);
            var spEdition = options.GetOptionValue(DefaultOptions.SharePoint.Edition.Id);
            var spApi = options.GetOptionValue(DefaultOptions.SharePoint.Api.Id);

            if (Compare(spEdition, DefaultOptions.SharePoint.Edition.Foundation.Value, true))
            {
                if (Compare(spApi, DefaultOptions.SharePoint.Api.CSOM.Value, true))
                    provisionServiceClassName = "SPMeta2.CSOM.Services.CSOMProvisionService";
                else if (Compare(spApi, DefaultOptions.SharePoint.Api.SSOM.Value, true))
                    provisionServiceClassName = "SPMeta2.SSOM.Services.SSOMProvisionService";
                else
                    throw new MetaPackException(String.Format("Unsuported SharePoint Api:[{0}]", spApi));
            }
            else if (Compare(spEdition, DefaultOptions.SharePoint.Edition.Standard.Value, true))
            {
                if (Compare(spApi, DefaultOptions.SharePoint.Api.CSOM.Value, true))
                    provisionServiceClassName = "SPMeta2.CSOM.Standard.Services.StandardCSOMProvisionService";
                else if (Compare(spApi, DefaultOptions.SharePoint.Api.SSOM.Value, true))
                    provisionServiceClassName = "SPMeta2.SSOM.Standard.Services.StandardSSOMProvisionService";
                else
                    throw new MetaPackException(String.Format("Unsuported SharePoint Api:[{0}]", spApi));
            }
            else
                throw new MetaPackException(String.Format("Unsuported SharePoint Edition:[{0}]", spEdition));

            return provisionServiceClassName;
        }

        protected virtual Type ResolveModelHostType(IEnumerable<Type> allClasses,
            string rootDefinitionClassName, string spApi)
        {
            Type modelHostType = null;

            MetaPackTrace.Verbose(string.Format(
                        "Revolving model host for API:[{0}] and root definition:[{1}]",
                        spApi, rootDefinitionClassName));

            if (Compare(spApi, DefaultOptions.SharePoint.Api.SSOM.Value, true))
            {
                MetaPackTrace.Verbose("Searching for SSOM impl...");

                if (rootDefinitionClassName == "FarmDefinition")
                    modelHostType = allClasses.FirstOrDefault(t => t.FullName == "SPMeta2.SSOM.ModelHosts.FarmModelHost");
                else if (rootDefinitionClassName == "WebApplicationDefinition")
                    modelHostType = allClasses.FirstOrDefault(t => t.FullName == "SPMeta2.SSOM.ModelHosts.WebApplicationModelHost");
                else if (rootDefinitionClassName == "SiteDefinition")
                    modelHostType = allClasses.FirstOrDefault(t => t.FullName == "SPMeta2.SSOM.ModelHosts.SiteModelHost");
                else if (rootDefinitionClassName == "WebDefinition")
                    modelHostType = allClasses.FirstOrDefault(t => t.FullName == "SPMeta2.SSOM.ModelHosts.WebModelHost");
                else
                {
                    throw new MetaPackException(
                        string.Format("Unsupported model with root definition type:[{0}]",
                        rootDefinitionClassName));
                }
            }
            else if (Compare(spApi, DefaultOptions.SharePoint.Api.CSOM.Value, true))
            {
                MetaPackTrace.Verbose("Searching for CSOM impl...");

                if (rootDefinitionClassName == "SiteDefinition")
                    modelHostType = allClasses.FirstOrDefault(t => t.FullName == "SPMeta2.CSOM.ModelHosts.SiteModelHost");
                else if (rootDefinitionClassName == "WebDefinition")
                    modelHostType = allClasses.FirstOrDefault(t => t.FullName == "SPMeta2.CSOM.ModelHosts.WebModelHost");
                else
                {
                    throw new MetaPackException(
                        string.Format("Unsupported model with root definition type:[{0}]",
                        rootDefinitionClassName));
                }
            }

            if (modelHostType == null)
            {
                throw new MetaPackException(string.Format(
                        "Cannot find model host for API:[{0}] and root definition:[{1}]",
                        spApi, rootDefinitionClassName));
            }


            return modelHostType;
        }

        public override void Deploy(SolutionPackageProvisionOptions options)
        {
            MetaPackTrace.Verbose("Resolving provision class...");

            var provisionServiceClassName = GetProvisionServiceClassName(options);
            MetaPackTrace.Verbose(string.Format("Resolved as:[{0}]", provisionServiceClassName));

            MetaPackTrace.Verbose("Resolving provision class type...");

            var allAssemblies = ReflectionUtils.GetAllAssembliesFromCurrentAppDomain();
            var allClasses = ReflectionUtils.GetAllTypesFromCurrentAppDomain();

            MetaPackTrace.Verbose("All assemblies");
            foreach (var assembly in allAssemblies)
                MetaPackTrace.Verbose("    " + assembly.FullName);

            var provisionServiceClass = ReflectionUtils.FindTypeByFullName(allClasses, provisionServiceClassName);

            if (provisionServiceClass == null)
                throw new MetaPackException(string.Format("Cannot find provision service imp by name:[{0}]", provisionServiceClassName));
            else
                MetaPackTrace.Verbose(string.Format("Found as type as:[{0}]", provisionServiceClass));

            MetaPackTrace.Verbose(string.Format("Creating provision service instance..."));
            var provisionService = Activator.CreateInstance(provisionServiceClass);

            // reoad, cause SPMeta2.dll will be loaded only once provisionServiceClass is created
            allAssemblies = ReflectionUtils.GetAllAssembliesFromCurrentAppDomain();
            var m2CoreAssembly = ReflectionUtils.FindAssemblyByFullName(allAssemblies, "SPMeta2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a");

            var m2coreClasses = m2CoreAssembly.GetTypes().OrderBy(s => s.Name);
            var spMeta2ModelClass = m2coreClasses.FirstOrDefault(c => c.Name == "SPMeta2Model");
            var fromXMLMethod = spMeta2ModelClass.GetMethods().FirstOrDefault(m => m.Name == "FromXML"
                                                                        && m.ReturnType.Name == "ModelNode");

            var solutionPackage = options.SolutionPackage as SolutionPackageBase;

            if (solutionPackage == null)
                throw new MetaPackException("SolutionPackage is not of type SPMeta2SolutionPackage");

            if (provisionService == null)
                throw new MetaPackException(string.Format("provisionService is null. Tried to create from:[{0}]", provisionServiceClassName));

            var spVersion = options.GetOptionValue(DefaultOptions.SharePoint.Version.Id);
            var spEdition = options.GetOptionValue(DefaultOptions.SharePoint.Edition.Id);
            var spApi = options.GetOptionValue(DefaultOptions.SharePoint.Api.Id);

            MetaPackTrace.Verbose(string.Format("spVersion:[{0}]", spVersion));
            MetaPackTrace.Verbose(string.Format("spEdition:[{0}]", spEdition));
            MetaPackTrace.Verbose(string.Format("spApi:[{0}]", spApi));

            var models = solutionPackage.GetModels();
            MetaPackTrace.Verbose(string.Format("Starting provision for [{0}] models", models.Count()));

            foreach (var modelContainer in models)
            {
                if (modelContainer.Model == null)
                    throw new MetaPackException("model is null or empty");

                var modelContent = UTF8Encoding.UTF8.GetString(modelContainer.Model);

                var model = fromXMLMethod.Invoke(null, new object[] { modelContent });
                var rootDefinitionValue = model.GetType().GetProperty("Value")
                                               .GetValue(model);

                var rootDefinitionClassName = rootDefinitionValue.GetType().Name;

                MetaPackTrace.Verbose(string.Format("Provisioning model [{0}]", rootDefinitionValue.GetType().Name));
                MetaPackTrace.Verbose(string.Format("Resolving model host type..."));

                var modelHostType = ResolveModelHostType(allClasses, rootDefinitionClassName, spApi);

                MetaPackTrace.Verbose(string.Format("Resolved as [{0}]", modelHostType));

                if (modelHostType == null)
                    throw new MetaPackException(string.Format("Cannot resolve model host type"));

                if (Compare(spApi, DefaultOptions.SharePoint.Api.CSOM.Value, true))
                {
                    ProcessCSOMDeployment(options,
                                          provisionServiceClassName,
                                          allAssemblies,
                                          allClasses,
                                          provisionService,
                                          spVersion,
                                          model,
                                          modelHostType);
                }
                else
                {
                    ProcessSSOMDeployment(options,
                                         provisionServiceClassName,
                                         allAssemblies,
                                         allClasses,
                                         provisionService,
                                         spVersion,
                                         model,
                                         modelHostType);
                }
            }
        }

        private void ProcessSSOMDeployment(SolutionPackageProvisionOptions options,
            string provisionServiceClassName,
            IEnumerable<Assembly> allAssemblies,
            IEnumerable<Type> allClasses,
            object provisionService,
            string spVersion,
            object model,
            Type modelHostType)
        {
            MetaPackTrace.Verbose(string.Format("Detected SSOM provision."));

            var m2SSOMAssembly = ReflectionUtils.FindAssemblyByFullName(allAssemblies, "SPMeta2.SSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a");

            if (m2SSOMAssembly == null)
                throw new MetaPackException(string.Format("Cannot find assembly:[SPMeta2.SSOM]"));

            var siteUrl = options.GetOptionValue(DefaultOptions.Site.Url.Id);

            MethodInfo modelHostFromXXXMethod = null;

            if (modelHostType.Name == "SiteModelHost")
                modelHostFromXXXMethod = modelHostType.GetMethod("FromSite");
            else if (modelHostType.Name == "WebModelHost")
                modelHostFromXXXMethod = modelHostType.GetMethod("FromWeb");
            else if (modelHostType.Name == "WebApplicationModelHost")
                modelHostFromXXXMethod = modelHostType.GetMethod("FromWebApplication");
            else if (modelHostType.Name == "FarmModelHost")
                modelHostFromXXXMethod = modelHostType.GetMethod("FromFarm");
            else
            {
                throw new MetaPackException(string.Format("Unknown model host type:[{0}]", modelHostType.Name));
            }

            if (modelHostFromXXXMethod == null)
                throw new MetaPackException(string.Format("Cannot find FromXXX for model host of type:[{0}]", modelHostType));

            object spHost = new object();


            // load up sharepoint assembly if not there yet
            var spAssembly = allAssemblies.FirstOrDefault(a => a.FullName.Contains("Microsoft.SharePoint,"));

            if (spAssembly == null)
            {
                // fall back t SP2010 but 15-16 must work as well
                var spAssemblyName = "Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c";
                spAssembly = Assembly.Load(spAssemblyName);

                if (spAssembly == null)
                {
                    throw new MetaPackException(
                      string.Format("Cannot load assembly:[{0}]",
                      spAssemblyName));
                }
            }

            if (modelHostType.Name == "FarmModelHost")
            {
                var spHostTypeName = "Microsoft.SharePoint.Administration.SPFarm";
                var spHostType = spAssembly.GetType(spHostTypeName);

                if (spHostType == null)
                {
                    throw new MetaPackException(
                      string.Format("Cannot find type:[{0}]",
                      spHostTypeName));
                }

                MetaPackTrace.Verbose(string.Format("SPFarm: creating new SharePoint host of type:[{0}]", spHostTypeName));
                spHost = ReflectionUtils.GetStaticPropertyValue(spHostType, "Local");

                if (spHost == null)
                {
                    throw new MetaPackException(
                      string.Format("Cannot create spHost of type type:[{0}]",
                      spHostType));
                }
            }
            else if (modelHostType.Name == "WebApplicationModelHost")
            {
                var spHostTypeName = "Microsoft.SharePoint.Administration.SPWebApplication";
                var spHostType = spAssembly.GetType(spHostTypeName);

                if (spHostType == null)
                {
                    throw new MetaPackException(
                      string.Format("Cannot find type:[{0}]",
                      spHostTypeName));
                }

                MetaPackTrace.Verbose(string.Format("SPWebApplication: creating new SharePoint host of type:[{0}] for Url:[{1}]", spHostTypeName, siteUrl));

                spHost = ReflectionUtils.InvokeStaticMethod(spHostType, "Lookup", new object[] { siteUrl });

                if (spHost == null)
                {
                    throw new MetaPackException(
                      string.Format("Cannot create spHost of type type:[{0}]",
                      spHostType));
                }
            }
            else if (modelHostType.Name == "SiteModelHost" || modelHostType.Name == "WebModelHost")
            {
                var spHostTypeName = "Microsoft.SharePoint.SPSite";
                var spHostType = spAssembly.GetType(spHostTypeName);

                if (spHostType == null)
                {
                    throw new MetaPackException(
                      string.Format("Cannot find type:[{0}]",
                      spHostTypeName));
                }

                MetaPackTrace.Verbose(string.Format("SPSite: new SharePoint host of type:[{0}] for Url:[{1}]", spHostTypeName, siteUrl));
                spHost = Activator.CreateInstance(spHostType, siteUrl);

                MetaPackTrace.Verbose("Created SPSite instance for Url:[{0}]", siteUrl);

                if (modelHostType.Name == "WebModelHost")
                {
                    MetaPackTrace.Verbose("Opening SPWeb for instance Url:[{0}]", siteUrl);
                    spHost = ReflectionUtils.InvokeMethod(spHost, "OpenWeb");
                }

                if (spHost == null)
                {
                    throw new MetaPackException(
                      string.Format("Cannot create spHost of type type:[{0}]",
                      spHostType));
                }
            }

            MetaPackTrace.Verbose(string.Format("Creating model host instance of type:[{0}]", modelHostType));
            object clientContextInstance = spHost;
            var modelHostInstance = modelHostFromXXXMethod.Invoke(null, new object[] { clientContextInstance });

            MetaPackTrace.Verbose(string.Format("Created model host instance of type:[{0}]", modelHostInstance));
            var provisionMethod = ReflectionUtils.GetMethod(provisionService, "DeployModel");

            if (provisionMethod == null)
            {
                throw new MetaPackException(
                    string.Format("Cannot find 'DeployModel' on the provision service of type:[{0}]",
                        provisionServiceClassName));
            }
            else
            {
                MetaPackTrace.Verbose(string.Format("Found .DeployModel method."));
            }

            // so much lazy to clean up that mess
            var provisionServiceType = provisionService.GetType();

            var onModelNodeProcessingEvent = provisionServiceType.GetField("OnModelNodeProcessing", BindingFlags.Public | BindingFlags.Instance);
            var onModelNodeProcessingHandler = GetType().GetMethod("OnModelNodeProcessing");
            var onModelNodeProcessingDelegate = Delegate.CreateDelegate(onModelNodeProcessingEvent.FieldType, this, onModelNodeProcessingHandler);

            onModelNodeProcessingEvent.SetValue(provisionService, onModelNodeProcessingDelegate);

            var onModelNodeProcessedEvent = provisionServiceType.GetField("OnModelNodeProcessed", BindingFlags.Public | BindingFlags.Instance);
            var onModelNodeProcessedHandler = GetType().GetMethod("OnModelNodeProcessed");
            var onModelNodeProcessedDelegate = Delegate.CreateDelegate(onModelNodeProcessingEvent.FieldType, this, onModelNodeProcessedHandler);

            onModelNodeProcessedEvent.SetValue(provisionService, onModelNodeProcessedDelegate);

            MetaPackTrace.Verbose(string.Format("Starting provision..."));
            provisionMethod.Invoke(provisionService, new[] { modelHostInstance, model });

            MetaPackTrace.Verbose(string.Format("Provision completed."));

            if (spHost is IDisposable)
            {
                MetaPackTrace.Verbose(string.Format("Disposing spHost object of type:[{0}]", spHost.GetType()));

                ((IDisposable)spHost).Dispose();
                spHost = null;
            }
            else
            {
                MetaPackTrace.Verbose(string.Format("No need to dispose spHost object of type:[{0}]", spHost.GetType()));
            }
        }

        private void ProcessCSOMDeployment(SolutionPackageProvisionOptions options, string provisionServiceClassName, IEnumerable<Assembly> allAssemblies, IEnumerable<Type> allClasses, object provisionService, string spVersion, object model, Type modelHostType)
        {
            MetaPackTrace.Verbose(string.Format("Detected CSOM provision."));

            var m2CSOMAssembly = ReflectionUtils.FindAssemblyByFullName(allAssemblies, "SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a");

            if (m2CSOMAssembly == null)
                throw new MetaPackException(string.Format("Cannot find assembly:[SPMeta2.CSOM]"));

            var userName = options.GetOptionValue(DefaultOptions.User.Name.Id);
            var userPassword = options.GetOptionValue(DefaultOptions.User.Password.Id);

            var siteUrl = options.GetOptionValue(DefaultOptions.Site.Url.Id);
            var clientContexClass = ReflectionUtils.FindTypeByName(allClasses, "ClientContext");

            if (clientContexClass == null)
                throw new MetaPackException(string.Format("Cannot find class by name:[{0}]", "ClientContext"));

            MetaPackTrace.Verbose(string.Format("Creating ClientContext for web site:[{0}]", siteUrl));
            var clientContextInstance = Activator.CreateInstance(clientContexClass, new object[] { siteUrl });

            if (clientContextInstance == null)
                throw new MetaPackException(string.Format("Cannot create client context"));

            if (Compare(spVersion, DefaultOptions.SharePoint.Version.O365.Value, true))
            {
                // O365 creds
                MetaPackTrace.Verbose(string.Format("O365 API detected"));

                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userPassword))
                {
                    MetaPackTrace.Verbose(string.Format("[{0}] and [{1}] aren't null.",
                            DefaultOptions.User.Name.Id,
                            DefaultOptions.User.Password.Id
                        ));

                    MetaPackTrace.Verbose(string.Format("Creating SharePointOnlineCredentials for web site:[{0}]", siteUrl));
                    var spCredentialsClass = ReflectionUtils.FindTypeByName(allClasses, "SharePointOnlineCredentials");

                    if (spCredentialsClass == null)
                        throw new MetaPackException(string.Format("Cannot find class by name:[{0}]",
                            "SharePointOnlineCredentials"));

                    var securePassword = new SecureString();
                    foreach (char c in userPassword)
                        securePassword.AppendChar(c);

                    var spCredentialsInstance = Activator.CreateInstance(spCredentialsClass, new object[]
                            {
                                userName,
                                securePassword
                            });

                    MetaPackTrace.Verbose(string.Format("Setting up credentials..."));
                    ReflectionUtils.SetPropertyValue(clientContextInstance, "Credentials", spCredentialsInstance);
                }
                else
                {
                    throw new MetaPackException(string.Format("O365 provision requires [{0}] and [{1}] to be set.",
                        DefaultOptions.User.Name.Id,
                        DefaultOptions.User.Password.Id
                    ));
                }
            }
            else
            {
                MetaPackTrace.Verbose(string.Format("On-premises CSOM API is detected"));

                // local network creds
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userPassword))
                {
                    MetaPackTrace.Verbose(string.Format("[{0}] and [{1}] aren't null.",
                            DefaultOptions.User.Name.Id,
                            DefaultOptions.User.Password.Id
                        ));

                    MetaPackTrace.Verbose(string.Format("Creating NetworkCredential for web site:[{0}]", siteUrl));

                    var spCredentialsInstance = new NetworkCredential(userName, userPassword);

                    MetaPackTrace.Verbose(string.Format("Setting up credentials..."));
                    ReflectionUtils.SetPropertyValue(clientContextInstance, "Credentials", spCredentialsInstance);
                }
                else
                {
                    MetaPackTrace.Verbose(string.Format("No username/userpassword were provided for site:[{0}]", siteUrl));
                }
            }

            var modelHostFromClientContextMethod = modelHostType.GetMethod("FromClientContext");

            if (modelHostFromClientContextMethod == null)
                throw new MetaPackException("Cannot find FromClientContext method on model host");

            MetaPackTrace.Verbose(string.Format("Creating model host instance of type:[{0}]", modelHostType));
            var modelHostInstance = modelHostFromClientContextMethod.Invoke(null, new object[] { clientContextInstance });

            MetaPackTrace.Verbose(string.Format("Created model host instance of type:[{0}]", modelHostInstance));
            var provisionMethod = ReflectionUtils.GetMethod(provisionService, "DeployModel");

            if (provisionMethod == null)
            {
                throw new MetaPackException(
                    string.Format("Cannot find 'DeployModel' on the provision service of type:[{0}]",
                        provisionServiceClassName));
            }
            else
            {
                MetaPackTrace.Verbose(string.Format("Found .DeployModel method."));
            }

            // so much lazy to clean up that mess
            var provisionServiceType = provisionService.GetType();

            var onModelNodeProcessingEvent = provisionServiceType.GetField("OnModelNodeProcessing", BindingFlags.Public | BindingFlags.Instance);
            var onModelNodeProcessingHandler = GetType().GetMethod("OnModelNodeProcessing");
            var onModelNodeProcessingDelegate = Delegate.CreateDelegate(onModelNodeProcessingEvent.FieldType, this, onModelNodeProcessingHandler);

            onModelNodeProcessingEvent.SetValue(provisionService, onModelNodeProcessingDelegate);

            var onModelNodeProcessedEvent = provisionServiceType.GetField("OnModelNodeProcessed", BindingFlags.Public | BindingFlags.Instance);
            var onModelNodeProcessedHandler = GetType().GetMethod("OnModelNodeProcessed");
            var onModelNodeProcessedDelegate = Delegate.CreateDelegate(onModelNodeProcessingEvent.FieldType, this, onModelNodeProcessedHandler);

            onModelNodeProcessedEvent.SetValue(provisionService, onModelNodeProcessedDelegate);

            MetaPackTrace.Verbose(string.Format("Starting provision..."));
            provisionMethod.Invoke(provisionService, new[] { modelHostInstance, model });

            MetaPackTrace.Verbose(string.Format("Provision completed."));
        }

        protected virtual string GetOnModelNodeEventTraceString(object s)
        {
            var TotalModelNodeCount = (int)ReflectionUtils.GetPropertyValue(s, "TotalModelNodeCount");
            var ProcessedModelNodeCount = (int)ReflectionUtils.GetPropertyValue(s, "ProcessedModelNodeCount");

            var currentNode = ReflectionUtils.GetPropertyValue(s, "CurrentNode");
            var currrentNodeValue = ReflectionUtils.GetPropertyValue(currentNode, "Value");

            var traceString = string.Format("[{0}/{1}] - [{2}%] - [{3}] [{4}]",
                new object[]
                {
                    ProcessedModelNodeCount,
                    TotalModelNodeCount,
                    Math.Round( 100d*(double) ProcessedModelNodeCount/(double) TotalModelNodeCount),
                    currrentNodeValue.GetType().Name,
                    currrentNodeValue
                });

            return traceString;
        }

        public void OnModelNodeProcessing(object o, object s)
        {
            MetaPackTrace.Info(string.Format("[SPMeta2] - Processing {0}", GetOnModelNodeEventTraceString(s)));
        }

        public void OnModelNodeProcessed(object o, object s)
        {
            MetaPackTrace.Info(string.Format("[SPMeta2] - Processed: {0}", GetOnModelNodeEventTraceString(s)));
        }
    }
}
