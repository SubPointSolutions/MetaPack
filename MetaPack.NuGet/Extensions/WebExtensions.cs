using System;
using Microsoft.SharePoint.Client;

namespace MetaPack.NuGet.Extensions
{
    internal static class WebExtensions
    {
        #region methods
        public static List LoadListByUrl(this Web web, string listUrl)
        {
            try
            {
                var ctx = web.Context;

                var listFolder = web.GetFolderByServerRelativeUrl(listUrl);

                ctx.Load(listFolder.Properties);
                ctx.ExecuteQuery();


                var listId = new Guid(listFolder.Properties["vti_listname"].ToString());
                var list = web.Lists.GetById(listId);

                ctx.Load(list);
                ctx.ExecuteQuery();

                return list;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        #endregion
    }
}
