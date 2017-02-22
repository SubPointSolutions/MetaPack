using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Core.Common
{
    public static class DefaultOptions
    {
        #region consts


        public static class SolutionToolPackage
        {
            public static class PackageId
            {
                public const string Id = "_metapack.solutiontoolpackage.id";
            }

            public static class PackageVersion
            {
                public const string Id = "_metapack.solutiontoolpackage.version";
            }
        }

        public static class Site
        {
            public static class Url
            {
                public const string Id = "_metapack.site.url";
            }
        }

        public static class User
        {
            public static class Name
            {
                public const string Id = "_metapack.user.name";
            }

            public static class Password
            {
                public const string Id = "_metapack.user.password";
            }
        }

        public static class Model
        {
            public static class Type
            {
                public const string Id = "_metapack.model.type";
            }
        }

        public static class SharePoint
        {
            public static class Api
            {
                public const string Id = "_metapack.sharepoint.api";

                public static OptionValue CSOM = new OptionValue
                {
                    Name = Id,
                    Value = "CSOM"
                };

                public static OptionValue SSOM = new OptionValue
                {
                    Name = Id,
                    Value = "SSOM"
                };
            }

            public static class Edition
            {
                public const string Id = "_metapack.sharepoint.edition";

                public static OptionValue Foundation = new OptionValue
                {
                    Name = Edition.Id,
                    Value = "Foundation"
                };

                public static OptionValue Standard = new OptionValue
                {
                    Name = Edition.Id,
                    Value = "Standard"
                };
            }

            public static class Version
            {
                public const string Id = "_metapack.sharepoint.version";

                public static OptionValue SP2013 = new OptionValue
                {
                    Name = Version.Id,
                    Value = "SP2013"
                };

                public static OptionValue SP2016 = new OptionValue
                {
                    Name = Version.Id,
                    Value = "SP2016"
                };


                public static OptionValue O365 = new OptionValue
                {
                    Name = Version.Id,
                    Value = "O365"
                };
            }
        }

        #endregion
    }
}
