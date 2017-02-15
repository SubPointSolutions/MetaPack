﻿using MetaPack.Core.Packaging;
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
                throw new Exception(String.Format("Unsuported SharePoint Api:[{0}]", spApi));
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
                throw new Exception(String.Format("Unsuported SharePoint Version:[{0}]", spVersion));
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

            MetaPackTrace.WriteLine(string.Format("spVersion:[{0}]", spVersion));
            MetaPackTrace.WriteLine(string.Format("spEdition:[{0}]", spEdition));
            MetaPackTrace.WriteLine(string.Format("spApi:[{0}]", spApi));

            if (spApi != DefaultOptions.SharePoint.Api.CSOM.Value)
                throw new NotSupportedException(string.Format("SharePoint API [{0}] is not supported yet", spApi));


            MetaPackTrace.WriteLine("Resolving provision class...");

            var solutionPackage = options.SolutionPackage as SharePointPnPSolutionPackage;

            MetaPackTrace.WriteLine(string.Format("Found [{0}] provision templates",
                                    solutionPackage.ProvisioningTemplateFolders.Count));

            var pnpAssemblyName = "OfficeDevPnP.Core";

            var siteToTemplateConversionType = "OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers.SiteToTemplateConversion";
            var fileSystemConnectorType = "OfficeDevPnP.Core.Framework.Provisioning.Connectors.FileSystemConnector";
            var openXMLTemplateProviderType = "OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml.XMLOpenXMLTemplateProvider";
            var provisioningTemplateApplyingInformationType = "OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers.ProvisioningTemplateApplyingInformation";

            var openXmlConnectorType = "OfficeDevPnP.Core.Framework.Provisioning.Connectors.OpenXMLConnector";

            var pnpAssembly = AppDomain.CurrentDomain.GetAssemblies()
                                               .FirstOrDefault(a => a.FullName.Contains("OfficeDevPnP.Core,"));


            var allClasses = AppDomain.CurrentDomain.GetAssemblies()
                                    .FirstOrDefault(a => a.FullName.Contains("Microsoft.SharePoint.Client,"))
                                    .GetTypes().ToList();

            allClasses.AddRange(AppDomain.CurrentDomain.GetAssemblies()
                                    .FirstOrDefault(a => a.FullName.Contains("Microsoft.SharePoint.Client.Runtime,"))
                                    .GetTypes());

            foreach (var templateFolder in solutionPackage.ProvisioningTemplateOpenXmlPackageFolders)
            {
                MetaPackTrace.WriteLine(string.Format("Deploying OpenXML package from path: [{0}]", templateFolder));

                var allPackages = Directory.GetFiles(templateFolder, "*.pnp");

                MetaPackTrace.WriteLine(string.Format("Detected CSOM provision."));

                var userName = options.GetOptionValue(DefaultOptions.User.Name.Id);
                var userPassword = options.GetOptionValue(DefaultOptions.User.Password.Id);

                var siteUrl = options.GetOptionValue(DefaultOptions.Site.Url.Id);

                MetaPackTrace.WriteLine(string.Format("Creating ClientContext for web site:[{0}]", siteUrl));
                var clientContexClass = allClasses.FirstOrDefault(c => c.Name == "ClientContext");

                if (clientContexClass == null)
                    throw new Exception(string.Format("Cannot find class by name:[{0}]", "ClientContext"));

                MetaPackTrace.WriteLine(string.Format("Creating ClientContext for web site:[{0}]", siteUrl));
                var clientContextInstance = Activator.CreateInstance(clientContexClass, new object[] { siteUrl });

                if (clientContextInstance == null)
                    throw new Exception(string.Format("Cannot create client context"));

                var web = clientContextInstance.GetType().GetProperty("Web")
                                        .GetValue(clientContextInstance);

                if (spVersion == DefaultOptions.SharePoint.Version.O365.Value)
                {
                    MetaPackTrace.WriteLine(string.Format("O365 API detected"));

                    if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userPassword))
                    {
                        MetaPackTrace.WriteLine(string.Format("[{0}] and [{1}] aren't null.",
                                DefaultOptions.User.Name.Id,
                                DefaultOptions.User.Password.Id
                            ));

                        MetaPackTrace.WriteLine(string.Format("Creating Credentials for web site:[{0}]", siteUrl));
                        var spCredentialsClass =
                            allClasses.FirstOrDefault(c => c.Name == "SharePointOnlineCredentials");

                        if (spCredentialsClass == null)
                            throw new Exception(string.Format("Cannot find class by name:[{0}]",
                                "SharePointOnlineCredentials"));

                        var securePassword = new SecureString();
                        foreach (char c in userPassword)
                            securePassword.AppendChar(c);

                        var spCredentialsInstance = Activator.CreateInstance(spCredentialsClass, new object[]
                            {
                                userName,
                                securePassword
                            });

                        MetaPackTrace.WriteLine(string.Format("Setting up credentials..."));
                        ReflectionUtils.SetPropertyValue(clientContextInstance, "Credentials", spCredentialsInstance);
                    }
                    else
                    {
                        throw new Exception(string.Format("O365 provision requires [{0}] and [{1}] to be set.",
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

                    MetaPackTrace.WriteLine(string.Format("Deploying package:[{0}]", fileName));

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
                    // var provider = new XMLOpenXMLTemplateProvider(new OpenXMLConnector(fileName, fileSystemConnector));
                    var provider = providerInstance;

                    //var templates = provider.GetTemplates();
                    var getTemplatesMethod = provider.GetType()
                        .GetMethods().FirstOrDefault(m => m.Name == "GetTemplates" && m.GetParameters().Length == 0);

                    var templates = getTemplatesMethod.Invoke(provider, null) as IEnumerable;

                    var providerConnector = provider.GetType().GetProperty("Connector")
                        .GetValue(provider);



                    foreach (var template in templates)
                    {
                        //template.Connector = provider.Connector;
                        template.GetType().GetProperty("Connector")
                                          .SetValue(template, providerConnector);

                        var templateId = template.GetType().GetProperty("Id")
                            .GetValue(template);

                        MetaPackTrace.WriteLine(string.Format("Deploying template:[{0}]", templateId));

                        var provisionOptions = pnpAssembly.CreateInstance(provisioningTemplateApplyingInformationType);
                        var delType = provisionOptions.GetType();

                        var templateDisplayName = template.GetType().GetProperty("DisplayName")
                            .GetValue(template);

                        MetaPackTrace.WriteLine(string.Format("Deploying template:[{0}]", templateDisplayName));

                        var siteToTemplateConversionInstance = pnpAssembly.CreateInstance(siteToTemplateConversionType);

                        var onMessageDelegate = delType.GetProperty("MessagesDelegate", BindingFlags.Public | BindingFlags.Instance);
                        var onMessageHandler = GetType().GetMethod("onMessagesDelegate");

                        Delegate del = Delegate.CreateDelegate(onMessageDelegate.PropertyType, this, onMessageHandler);
                        onMessageDelegate.SetValue(provisionOptions, del);

                        var onProgressDelegate = delType.GetProperty("ProgressDelegate", BindingFlags.Public | BindingFlags.Instance);
                        var onProgressHandler = GetType().GetMethod("onProgressDelegate");

                        Delegate delProgress = Delegate.CreateDelegate(onProgressDelegate.PropertyType, this, onProgressHandler);
                        onProgressDelegate.SetValue(provisionOptions, delProgress);

                        var applyRemoteTemplateMethod = siteToTemplateConversionInstance.GetType().GetMethod("ApplyRemoteTemplate",
                            BindingFlags.Instance | BindingFlags.NonPublic);

                        applyRemoteTemplateMethod.Invoke(siteToTemplateConversionInstance, new object[]
                        {
                            web,
                            template,
                            provisionOptions
                        });
                    }
                }
            }
        }

        public enum tt
        {

        }

        public void onMessagesDelegate(string s, tt t)
        {
            MetaPackTrace.WriteLine(s);
        }

        public void onProgressDelegate(string m, int s, int t)
        {
            MetaPackTrace.WriteLine(string.Format("{0}/{1} - {2}",
                new object[] { s, t, m }));
        }

        #endregion
    }
}