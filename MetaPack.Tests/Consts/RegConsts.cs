using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Tests.Consts
{
    public static class RegConsts
    {
        public static class NuGet
        {
            public static string ApiUrl = "MetaPack.NuGet.ApiUrl";
            public static string ApiKey = "MetaPack.NuGet.ApiKey";
            public static string RepoUrl = "MetaPack.NuGet.RepoUrl";
        }

        public static class O365
        {
            public static string RootWebUrl = "MetaPack.SharePoint.O365.RootWebUrl";
            public static string SubWebUrl = "MetaPack.SharePoint.O365.SubWebUrl";

            public static string UserName = "MetaPack.SharePoint.O365.UserName";
            public static string UserPassword = "MetaPack.SharePoint.O365.UserPassword";
        }

        public static class OnPremis
        {
            public static string RootWebUrl = "MetaPack.SharePoint.OnPremis.RootWebUrl";
            public static string SubWebUrl = "MetaPack.SharePoint.OnPremis.SubWebUrl";

            public static string UserName = "MetaPack.SharePoint.OnPremis.UserName";
            public static string UserPassword = "MetaPack.SharePoint.OnPremis.UserPassword";
        }
    }
}
