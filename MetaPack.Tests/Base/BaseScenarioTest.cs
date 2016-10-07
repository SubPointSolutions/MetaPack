using System;
using System.Security;
using MetaPack.Core.Packaging;
using MetaPack.SPMeta2;
using MetaPack.Tests.Common;
using MetaPack.Tests.Consts;
using MetaPack.Tests.Utils;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;

namespace MetaPack.Tests.Base
{
    [TestClass]
    public class BaseScenarioTest
    {
        #region general

        protected void WithRootSharePointContext(Action<ClientContext> action)
        {
            var rootWebUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.RootWebUrl);

            if (string.IsNullOrEmpty(rootWebUrl))
                throw new NullReferenceException("rootWebUrl");

            WithSharePointContext(rootWebUrl, action);
        }

        protected void WithSubWebSharePointContext(Action<ClientContext> action)
        {
            var subwebUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.SubWebUrl);

            if (string.IsNullOrEmpty(subwebUrl))
                throw new NullReferenceException("subwebUrl");

            WithSharePointContext(subwebUrl, action);
        }

        protected void WithSharePointContext(string url, Action<ClientContext> action)
        {
            var userName = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.O365UserName);
            var userPassword = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.O365UserPassword);

            using (var context = new ClientContext(url))
            {
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userPassword))
                {
                    var securePassword = new SecureString();

                    foreach (var c in userPassword)
                        securePassword.AppendChar(c);

                    context.Credentials = new SharePointOnlineCredentials(userName, securePassword);
                }

                action(context);
            }
        }

        protected void WithNuGetContext(Action<string, string, string> action)
        {
            var apiUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.NuGet.ApiUrl);
            var apiKey = EnvironmentUtils.GetEnvironmentVariable(RegConsts.NuGet.ApiKey);
            var repoUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.NuGet.RepoUrl);

            if (string.IsNullOrEmpty(apiUrl))
                throw new NullReferenceException("apiUrl");

            if (string.IsNullOrEmpty(apiKey))
                throw new NullReferenceException("apiKey");

            if (string.IsNullOrEmpty(repoUrl))
                throw new NullReferenceException("repoUrl");

            action(apiUrl, apiKey, repoUrl);
        }


        protected static void UpdatePackageVersion(SolutionPackageBase package)
        {
            var date = DateTime.UtcNow;
            package.Version = string.Format("1.{0}.{1}.{2}",
                date.ToString("yyyy"),
                date.ToString("MMdd"),
                date.ToString("HHHmm"));
        }

        #endregion

        #region utils

        protected SolutionPackageBase CreateNewSolutionPackage(SolutionPackageType type)
        {
            return CreateNewSolutionPackage(type, null);
        }

        protected SolutionPackageBase CreateNewSolutionPackage(SolutionPackageType type, Action<SolutionPackageBase> action)
        {
            if (type == SolutionPackageType.SPMeta2)
            {
                var solutionPackage = new SPMeta2SolutionPackage();

                solutionPackage.Name = "SPMeta2 CI Ppackage Name";
                solutionPackage.Title = "SPMeta2 CI Package";

                solutionPackage.Description = "SPMeta2 CI Package description";
                solutionPackage.Id = "SPMeta2.CI";
                solutionPackage.Authors = "SubPoint Solutions Authors";
                solutionPackage.Company = "SubPoint Solutions Company";
                solutionPackage.Version = "1.0.0.0";
                solutionPackage.Owners = "SubPoint Solutions Owners";

                solutionPackage.ReleaseNotes = "ReleaseNotes";
                solutionPackage.Summary = "Summary";
                solutionPackage.ProjectUrl = "https://github.com/SubPointSolutions/metapack";
                solutionPackage.IconUrl = "https://github.com/SubPointSolutions/metapack/metapack.png";
                solutionPackage.LicenseUrl = "https://opensource.org/licenses/MIT";

                solutionPackage.Copyright = "Some stuff here";
                solutionPackage.Tags = "CI SPMeta2 MetaPack";

                var models = new ModelNode[]
                {
                    SPMeta2Model.NewSiteModel(site => { }),
                    SPMeta2Model.NewWebModel(web => { }),
                };

                foreach (var model in models)
                {
                    solutionPackage.Models.Add(model);
                }

                if (action != null)
                    action(solutionPackage);

                return solutionPackage;
            }


            throw new NotImplementedException("type");
        }

        #endregion
    }
}
