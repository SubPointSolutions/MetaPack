﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Core.Utils;
using Microsoft.SharePoint.Client;

namespace MetaPack.Client.Common.Services
{
    public class SharePointService
    {
        public virtual void WithSharePointContext(
            string url,
            string userName,
            string userPassword,
            bool isSharePointOnline,
            Action<ClientContext> action)
        {
            MetaPackTrace.Info("Connecting to SharePoint web site:[{0}]", url);

            using (var context = new ClientContext(url))
            {
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userPassword))
                {
                    MetaPackTrace.Info("Using username:[{0}]", userName);

                    var securePassword = new SecureString();

                    foreach (var c in userPassword)
                        securePassword.AppendChar(c);

                    if (isSharePointOnline)
                    {
                        MetaPackTrace.Info("Using O365 runtime and SharePointOnlineCredentials");
                        context.Credentials = new SharePointOnlineCredentials(userName, securePassword);
                    }
                    else
                    {
                        MetaPackTrace.Info("Using SP2013 runtime and NetworkCredential");
                        context.Credentials = new NetworkCredential(userName, securePassword);
                    }
                }

                action(context);
            }
        }
    }
}
