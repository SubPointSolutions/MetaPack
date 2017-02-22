using MetaPack.Core.Packaging;
using MetaPack.NuGet.Common;
using MetaPack.NuGet.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Core.Common;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Security;
using MetaPack.NuGet.Utils;
using System.Collections;
using MetaPack.Core.Exceptions;
using NuGet;
using MetaPack.Core.Utils;

namespace MetaPack.SharePointPnP.Services
{
    public class SharePointPnPSolutionDeploymentService : SolutionPackageDeploymentService
    {
        #region methods

        public override IEnumerable<SolutionToolPackage> GetAdditionalToolPackages(SolutionPackageProvisionOptions options)
        {
            var result = new List<SolutionToolPackage>();

            var mainToolPackageId = string.Empty;

            var spVersion = options.GetOptionValue(DefaultOptions.SharePoint.Version.Id);
            var spEdition = options.GetOptionValue(DefaultOptions.SharePoint.Edition.Id);
            var spApi = options.GetOptionValue(DefaultOptions.SharePoint.Api.Id);

            // ensure m2 assemblies
            if (spApi != DefaultOptions.SharePoint.Api.CSOM.Value)
            {
                throw new MetaPackException(String.Format("Unsuported SharePoint Api:[{0}]", spApi));
            }

            if (spVersion == DefaultOptions.SharePoint.Version.O365.Value)
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
            else if (spVersion == DefaultOptions.SharePoint.Version.SP2013.Value)
            {
                throw new NotImplementedException();
            }
            else if (spVersion == DefaultOptions.SharePoint.Version.SP2016.Value)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new MetaPackException(String.Format("Unsuported SharePoint Version:[{0}]", spVersion));
            }

            result.Add(new SolutionToolPackage
            {
                Id = mainToolPackageId
            });

            return result;
        }


        public override void Deploy(SolutionPackageProvisionOptions options)
        {
            var spVersion = options.GetOptionValue(DefaultOptions.SharePoint.Version.Id);
            var spEdition = options.GetOptionValue(DefaultOptions.SharePoint.Edition.Id);
            var spApi = options.GetOptionValue(DefaultOptions.SharePoint.Api.Id);

            MetaPackTrace.Verbose(string.Format("spVersion:[{0}]", spVersion));
            MetaPackTrace.Verbose(string.Format("spEdition:[{0}]", spEdition));
            MetaPackTrace.Verbose(string.Format("spApi:[{0}]", spApi));

            if (spApi != DefaultOptions.SharePoint.Api.CSOM.Value)
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

#if PACKAGING_V2

            var solutionPackage = options.SolutionPackage as _SharePointPnPSolutionPackage;

            MetaPackTrace.Verbose(string.Format("Found [{0}] provision templates", solutionPackage.ProvisioningTemplateFolders.Count));

            var siteToTemplateConversionType = "OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers.SiteToTemplateConversion";
            var fileSystemConnectorType = "OfficeDevPnP.Core.Framework.Provisioning.Connectors.FileSystemConnector";
            var openXMLTemplateProviderType = "OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml.XMLOpenXMLTemplateProvider";
            var provisioningTemplateApplyingInformationType = "OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers.ProvisioningTemplateApplyingInformation";
            var openXmlConnectorType = "OfficeDevPnP.Core.Framework.Provisioning.Connectors.OpenXMLConnector";

            foreach (var templateFolder in solutionPackage.ProvisioningTemplateOpenXmlPackageFolders)
            {
                MetaPackTrace.Verbose(string.Format("Deploying OpenXML package from path: [{0}]", templateFolder));

                var allPackages = Directory.GetFiles(templateFolder, "*.pnp");

                MetaPackTrace.Verbose(string.Format("Detected CSOM provision."));

                var userName = options.GetOptionValue(DefaultOptions.User.Name.Id);
                var userPassword = options.GetOptionValue(DefaultOptions.User.Password.Id);

                var siteUrl = options.GetOptionValue(DefaultOptions.Site.Url.Id);

                MetaPackTrace.Verbose(string.Format("Creating ClientContext for web site:[{0}]", siteUrl));
                var clientContexClass = ReflectionUtils.FindTypeByName(allSharePointClasses, "ClientContext");

                if (clientContexClass == null)
                    throw new MetaPackException(string.Format("Cannot find class by name:[{0}]", "ClientContext"));

                MetaPackTrace.Verbose(string.Format("Creating ClientContext for web site:[{0}]", siteUrl));
                var clientContextInstance = Activator.CreateInstance(clientContexClass, new object[] { siteUrl });

                if (clientContextInstance == null)
                    throw new MetaPackException(string.Format("Cannot create client context"));

                var web = ReflectionUtils.GetPropertyValue(clientContextInstance, "Web");

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
                            DefaultOptions.User.Name.Id,
                            DefaultOptions.User.Password.Id
                        ));
                    }
                }

                foreach (var pnpPackage in allPackages)
                {
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

                    var provider = providerInstance;
                    var getTemplatesMethod = provider.GetType().GetMethods()
                                                     .FirstOrDefault(m => m.Name == "GetTemplates" && m.GetParameters().Length == 0);

                    var templates = getTemplatesMethod.Invoke(provider, null) as IEnumerable;

                    var providerConnector = ReflectionUtils.GetPropertyValue(provider, "Connector");

                    foreach (var template in templates)
                    {
                        ReflectionUtils.SetPropertyValue(template, "Connector", providerConnector);

                        MetaPackTrace.Verbose(string.Format("Deploying template:"));
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
            }

#endif
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
