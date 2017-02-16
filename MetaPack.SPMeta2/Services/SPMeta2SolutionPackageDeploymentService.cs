using System;
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

namespace MetaPack.SPMeta2.Services
{
    /// <summary>
    /// Solution package deployment service for SPMeta2 provision.
    /// </summary>
    public class SPMeta2SolutionPackageDeploymentService : SolutionPackageDeploymentService
    {
        public override IEnumerable<SolutionToolPackage> GetAdditionalToolPackages(SolutionPackageProvisionOptions options)
        {
            var result = new List<SolutionToolPackage>();

            var mainToolPackageId = "SPMeta2";

            var spVersion = options.GetOptionValue(DefaultOptions.SharePoint.Version.Id);
            var spEdition = options.GetOptionValue(DefaultOptions.SharePoint.Edition.Id);
            var spApi = options.GetOptionValue(DefaultOptions.SharePoint.Api.Id);

            // ensure m2 assemblies
            if (spApi == DefaultOptions.SharePoint.Api.CSOM.Value)
                mainToolPackageId += ".CSOM";
            else if (spApi == DefaultOptions.SharePoint.Api.SSOM.Value)
                mainToolPackageId += ".SSOM";
            else
                throw new MetaPackException(String.Format("Unsuported SharePoint Api:[{0}]", spApi));

            if (spEdition == DefaultOptions.SharePoint.Edition.Foundation.Value)
                mainToolPackageId += ".Foundation";
            else if (spEdition == DefaultOptions.SharePoint.Edition.Standard.Value)
                mainToolPackageId += ".Standard";
            else
                throw new MetaPackException(String.Format("Unsuported SharePoint Edition:[{0}]", spEdition));

            if (spVersion == DefaultOptions.SharePoint.Version.O365.Value)
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
            else if (spVersion == DefaultOptions.SharePoint.Version.SP2013.Value)
            {
                // nothing
            }
            else if (spVersion == DefaultOptions.SharePoint.Version.SP2016.Value)
            {
                // nothing
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

            if (spEdition == DefaultOptions.SharePoint.Edition.Foundation.Value)
            {
                if (spApi == DefaultOptions.SharePoint.Api.CSOM.Value)
                    provisionServiceClassName = "SPMeta2.CSOM.Services.CSOMProvisionService";
                else if (spApi == DefaultOptions.SharePoint.Api.SSOM.Value)
                    provisionServiceClassName = "SPMeta2.SSOM.Services.SSOMProvisionService";
                else
                    throw new MetaPackException(String.Format("Unsuported SharePoint Api:[{0}]", spApi));
            }
            else if (spEdition == DefaultOptions.SharePoint.Edition.Foundation.Value)
            {
                if (spApi == DefaultOptions.SharePoint.Api.CSOM.Value)
                    provisionServiceClassName = "SPMeta2.CSOM.Standard.Services.StandardCSOMProvisionService";
                else if (spApi == DefaultOptions.SharePoint.Api.SSOM.Value)
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

            if (spApi == DefaultOptions.SharePoint.Api.SSOM.Value)
            {
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
            else if (spApi == DefaultOptions.SharePoint.Api.CSOM.Value)
            {
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

            var solutionPackage = options.SolutionPackage as SPMeta2SolutionPackage;

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

            if (spApi != DefaultOptions.SharePoint.Api.CSOM.Value)
                throw new NotSupportedException(string.Format("SharePoint API [{0}] is not supported yet", spApi));

            MetaPackTrace.Verbose(string.Format("Starting provision for [{0}] model folders", solutionPackage.ModelFolders.Count));

            foreach (var modelFolder in solutionPackage.ModelFolders)
            {
                var modelFiles = Directory.GetFiles(modelFolder, "*.xml");
                MetaPackTrace.Verbose(string.Format("Starting provision for [{0}] models", modelFiles.Count()));

                foreach (var modelFle in modelFiles)
                {
                    var modelContent = File.ReadAllText(modelFle);

                    var model = fromXMLMethod.Invoke(null, new object[] { modelContent });
                    var rootDefinitionValue = model.GetType().GetProperty("Value")
                                                   .GetValue(model);

                    var rootDefinitionClassName = rootDefinitionValue.GetType().Name;

                    MetaPackTrace.Verbose(string.Format("Provisioning model [{0}]", rootDefinitionValue.GetType().Name));
                    MetaPackTrace.Verbose(string.Format("Resoling model host type..."));

                    var modelHostType = ResolveModelHostType(allClasses, rootDefinitionClassName, spApi);

                    MetaPackTrace.Verbose(string.Format("Resolved as [{0}]", modelHostType));

                    if (spApi == DefaultOptions.SharePoint.Api.CSOM.Value)
                    {
                        MetaPackTrace.Verbose(string.Format("Detected CSOM provision."));

                        var m2CSOMAssembly = ReflectionUtils.FindAssemblyByFullName(allAssemblies, "SPMeta2.CSOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d71faae3bf28531a");

                        if (m2CSOMAssembly == null)
                            throw new MetaPackException(string.Format("Cannot find assembly:[SPMeta2.CSOM]"));

                        var userName = options.GetOptionValue(DefaultOptions.User.Name.Id);
                        var userPassword = options.GetOptionValue(DefaultOptions.User.Password.Id);

                        var siteUrl = options.GetOptionValue(DefaultOptions.Site.Url.Id);

                        MetaPackTrace.Verbose(string.Format("Creating ClientContext for web site:[{0}]", siteUrl));
                        var clientContexClass = ReflectionUtils.FindTypeByName(allClasses, "ClientContext");

                        if (clientContexClass == null)
                            throw new MetaPackException(string.Format("Cannot find class by name:[{0}]", "ClientContext"));

                        MetaPackTrace.Verbose(string.Format("Creating ClientContext for web site:[{0}]", siteUrl));
                        var clientContextInstance = Activator.CreateInstance(clientContexClass, new object[] { siteUrl });

                        if (clientContextInstance == null)
                            throw new MetaPackException(string.Format("Cannot create client context"));

                        if (spVersion == DefaultOptions.SharePoint.Version.O365.Value)
                        {
                            MetaPackTrace.Verbose(string.Format("O365 API detected"));

                            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userPassword))
                            {
                                MetaPackTrace.Verbose(string.Format("[{0}] and [{1}] aren't null.",
                                        DefaultOptions.User.Name.Id,
                                        DefaultOptions.User.Password.Id
                                    ));

                                MetaPackTrace.Verbose(string.Format("Creating Credentials for web site:[{0}]", siteUrl));
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
                    else
                    {
                        // TODO
                        throw new NotSupportedException(string.Format("SharePoint API [{0}] is not supported yet", spApi));
                    }
                }
            }
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
