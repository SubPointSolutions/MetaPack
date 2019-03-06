using MetaPack.Core.Packaging;
using MetaPack.NuGet.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Security;
using MetaPack.NuGet.Utils;
using System.Collections;
using MetaPack.Core.Exceptions;
using NuGet;
using MetaPack.Core.Utils;
using System.Net;
using MetaPack.Core.Data;
using MetaPack.Core.Extensions;
using MetaPack.NuGet.Data;

namespace MetaPack.SharePointPnP.Services
{
    public class SharePointPnPSolutionDeploymentService : SolutionPackageDeploymentService
    {
        #region methods

        private bool Compare(string v1, string v2)
        {
            return Compare(v1, v2, true);
        }

        private bool Compare(string v1, string v2, bool irnoreCase)
        {
            return string.Compare(v1, v2, true) == 0;
        }


        public override IEnumerable<SolutionToolPackage> GetAdditionalToolPackages(SolutionPackageBase solutionPackage, IDictionary<string, string> options)
        {
            var result = new List<SolutionToolPackage>();

            var mainToolPackageId = string.Empty;

            var spVersion = options.GetOptionValue(DefaultOptions.SharePointVersion);
            var spEdition = options.GetOptionValue(DefaultOptions.SharePointEdition);
            var spApi = options.GetOptionValue(DefaultOptions.SharePointApi);

            if (!Compare(spApi, "CSOM", true))
                throw new MetaPackException(String.Format("Unsupported SharePoint Api:[{0}]", spApi));

            if (Compare(spVersion, "O365"))
            {
                mainToolPackageId = "SharePointPnPCoreOnline";

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
            else if (Compare(spVersion, "SP2013"))
            {
                mainToolPackageId = "SharePointPnPCore2013";

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
            else if (Compare(spVersion, "SP2016"))
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new MetaPackException(String.Format("Unsupported SharePoint Version:[{0}]", spVersion));
            }

            result.Add(new SolutionToolPackage
            {
                Id = mainToolPackageId
            });

            return result;
        }


        public override void Deploy(SolutionPackageBase solutionPackage, IDictionary<string, string> options)
        {
            var spVersion = options.GetOptionValue(DefaultOptions.SharePointVersion);
            var spEdition = options.GetOptionValue(DefaultOptions.SharePointEdition);
            var spApi = options.GetOptionValue(DefaultOptions.SharePointApi);

            MetaPackTrace.Verbose(string.Format("spVersion:[{0}]", spVersion));
            MetaPackTrace.Verbose(string.Format("spEdition:[{0}]", spEdition));
            MetaPackTrace.Verbose(string.Format("spApi:[{0}]", spApi));

            if (!Compare(spApi, "CSOM", true))
                throw new NotSupportedException(string.Format("SharePoint API [{0}] is not supported yet", spApi));

            var allAssemblies = ReflectionUtils.GetAllAssembliesFromCurrentAppDomain();
            var allClasses = ReflectionUtils.GetAllTypesFromCurrentAppDomain();

            var pnpAssembly = allAssemblies.FirstOrDefault(a => a.FullName.Contains("OfficeDevPnP.Core,"));
            var spClientAssembly = allAssemblies.FirstOrDefault(a => a.FullName.Contains("Microsoft.SharePoint.Client,"));
            var spClientRuntimeAssembly = allAssemblies.FirstOrDefault(a => a.FullName.Contains("Microsoft.SharePoint.Client.Runtime,"));

            var allSharePointClasses = new List<Type>();
            allSharePointClasses.AddRange(spClientAssembly.GetTypes());
            allSharePointClasses.AddRange(spClientRuntimeAssembly.GetTypes());

            MetaPackTrace.Verbose("Resolving provision class...");

            var solutionModels = solutionPackage.GetModels();

            MetaPackTrace.Verbose(string.Format("Found [{0}] models", solutionModels.Count()));

            var siteToTemplateConversionType = "OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers.SiteToTemplateConversion";
            var fileSystemConnectorType = "OfficeDevPnP.Core.Framework.Provisioning.Connectors.FileSystemConnector";
            var openXMLTemplateProviderType = "OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml.XMLOpenXMLTemplateProvider";
            var provisioningTemplateApplyingInformationType = "OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers.ProvisioningTemplateApplyingInformation";
            var openXmlConnectorType = "OfficeDevPnP.Core.Framework.Provisioning.Connectors.OpenXMLConnector";

            var currentModelIndex = 0;
            var allModelsCount = solutionModels.Count();

            foreach (var modelContainer in solutionModels)
            {
                currentModelIndex++;

                MetaPackTrace.Verbose(string.Format(" - deploying model [{0}/{1}]", currentModelIndex, allModelsCount));

                // "SharePointPnP.FolderZip"
                // "SharePointPnP.OpenXml"

                var modelType = "SharePointPnP.OpenXml";
                var modelTypeOption = modelContainer.AdditionalOptions
                                                    .FirstOrDefault(o => o.Name.ToUpper() == DefaultOptions.ModelType);

                if (modelTypeOption != null)
                    modelType = modelTypeOption.Value;

                MetaPackTrace.Verbose(string.Format(" - model type: [{0}]", modelType));

                MetaPackTrace.Verbose(string.Format("Detected CSOM provision."));

                var userName = options.GetOptionValue(DefaultOptions.UserName);
                var userPassword = options.GetOptionValue(DefaultOptions.UserPassword);

                var siteUrl = options.GetOptionValue(DefaultOptions.SharePointSiteUrl);

                MetaPackTrace.Verbose(string.Format("Creating ClientContext for web site:[{0}]", siteUrl));
                var clientContexClass = ReflectionUtils.FindTypeByName(allSharePointClasses, "ClientContext");

                if (clientContexClass == null)
                    throw new MetaPackException(string.Format("Cannot find class by name:[{0}]", "ClientContext"));

                MetaPackTrace.Verbose(string.Format("Creating ClientContext for web site:[{0}]", siteUrl));
                var clientContextInstance = Activator.CreateInstance(clientContexClass, new object[] { siteUrl });

                if (clientContextInstance == null)
                    throw new MetaPackException(string.Format("Cannot create client context"));

                var web = ReflectionUtils.GetPropertyValue(clientContextInstance, "Web");

                if (Compare(spVersion, "O365", true))
                {
                    MetaPackTrace.Verbose(string.Format("O365 API detected"));

                    if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userPassword))
                    {
                        MetaPackTrace.Verbose(string.Format("[{0}] and [{1}] aren't null.",
                                DefaultOptions.UserName,
                                DefaultOptions.UserPassword
                            ));

                        MetaPackTrace.Verbose(string.Format("Creating Credentials for web site:[{0}]", siteUrl));
                        var spCredentialsClass = ReflectionUtils.FindTypeByName(allSharePointClasses, "SharePointOnlineCredentials");

                        if (spCredentialsClass == null)
                            throw new MetaPackException(string.Format("Cannot find class by name:[{0}]", "SharePointOnlineCredentials"));

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
                            DefaultOptions.UserName,
                            DefaultOptions.UserPassword
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
                                DefaultOptions.UserName,
                                DefaultOptions.UserPassword
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

                if (modelType.ToUpper() == "SharePointPnP.OpenXml".ToUpper())
                {
                    var tmpFileFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
                    Directory.CreateDirectory(tmpFileFolder);
                    var tmpFilePath = Path.Combine(tmpFileFolder, Guid.NewGuid().ToString("N") + ".pnp");

                    System.IO.File.WriteAllBytes(tmpFilePath, modelContainer.Model);
                    var pnpPackage = tmpFilePath;

                    var filConnectorPath = Path.GetDirectoryName(pnpPackage);
                    var fileSystemConnectorInstance = pnpAssembly.CreateInstance(fileSystemConnectorType,
                        true, BindingFlags.CreateInstance, null,
                        new object[] { filConnectorPath, "" },
                        CultureInfo.CurrentCulture,
                        null);

                    var fileSystemConnector = fileSystemConnectorInstance;
                    var fileName = Path.GetFileName(pnpPackage);

                    MetaPackTrace.Verbose(string.Format("Deploying package:[{0}]", fileName));

                    var openXMLConnectorInstance = pnpAssembly.CreateInstance(openXmlConnectorType,
                     true, BindingFlags.CreateInstance, null,
                     new object[] { fileName, fileSystemConnector, null, null },
                     CultureInfo.CurrentCulture,
                     null);

                    var providerInstance = pnpAssembly.CreateInstance(openXMLTemplateProviderType,
                     true, BindingFlags.CreateInstance, null,
                     new object[] { openXMLConnectorInstance },
                     CultureInfo.CurrentCulture,
                     null);

                    MetaPackTrace.Verbose(string.Format("Fetching templates..."));
                    var provider = providerInstance;
                    var getTemplatesMethod = provider.GetType().GetMethods()
                                                     .FirstOrDefault(m => m.Name == "GetTemplates" && m.GetParameters().Length == 0);

                    var templates = getTemplatesMethod.Invoke(provider, null) as IEnumerable;

                    var templatesCount = 0;

                    foreach (var template in templates)
                        templatesCount++;

                    MetaPackTrace.Verbose(string.Format("Found [{0}] templates", templatesCount));

                    var providerConnector = ReflectionUtils.GetPropertyValue(provider, "Connector");

                    var currentTemplateIndex = 0;

                    foreach (var template in templates)
                    {
                        currentTemplateIndex++;

                        ReflectionUtils.SetPropertyValue(template, "Connector", providerConnector);

                        MetaPackTrace.Verbose(string.Format("Deploying template [{0}/{1}]", currentTemplateIndex, templatesCount));
                        var templateId = ReflectionUtils.GetPropertyValue(template, "Id");
                        MetaPackTrace.Verbose(string.Format(" -ID:[{0}]", templateId));

                        var provisionOptions = pnpAssembly.CreateInstance(provisioningTemplateApplyingInformationType);
                        var delType = provisionOptions.GetType();

                        var templateDisplayName = ReflectionUtils.GetPropertyValue(template, "DisplayName");
                        MetaPackTrace.Verbose(string.Format(" -DisplayName:[{0}]", templateDisplayName));

                        var siteToTemplateConversionInstance = pnpAssembly.CreateInstance(siteToTemplateConversionType);

                        var onMessageDelegate = delType.GetProperty("MessagesDelegate", BindingFlags.Public | BindingFlags.Instance);
                        var onMessageHandler = GetType().GetMethod("onMessagesDelegate");
                        var onMessageDelegateImpl = Delegate.CreateDelegate(onMessageDelegate.PropertyType, this, onMessageHandler);
                        onMessageDelegate.SetValue(provisionOptions, onMessageDelegateImpl);

                        var onProgressDelegate = delType.GetProperty("ProgressDelegate", BindingFlags.Public | BindingFlags.Instance);
                        var onProgressHandler = GetType().GetMethod("onProgressDelegate");
                        var onProgressDelegateImpl = Delegate.CreateDelegate(onProgressDelegate.PropertyType, this, onProgressHandler);

                        onProgressDelegate.SetValue(provisionOptions, onProgressDelegateImpl);

                        var applyRemoteTemplateMethod = siteToTemplateConversionInstance.GetType()
                                                            .GetMethod("ApplyRemoteTemplate", BindingFlags.Instance | BindingFlags.NonPublic);

                        applyRemoteTemplateMethod.Invoke(siteToTemplateConversionInstance, new object[]
                        {
                            web,
                            template,
                            provisionOptions
                        });
                    }
                }
                else
                {
                    var errMesssage = string.Format("Deloyment models of type [{0}] is not supported yet", modelType);

                    MetaPackTrace.Info(errMesssage);
                    throw new MetaPackException(errMesssage);
                }
            }
        }

        public enum tt
        {

        }

        public void onMessagesDelegate(string s, tt t)
        {
            MetaPackTrace.Info(string.Format("[SharePointPnP] - {0}", s));
        }

        public void onProgressDelegate(string m, int s, int t)
        {
            MetaPackTrace.Info(string.Format("[SharePointPnP] - {0}/{1} - {2}",
                new object[] { s, t, m }));
        }

        #endregion
    }
}
