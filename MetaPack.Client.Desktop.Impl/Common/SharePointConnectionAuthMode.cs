using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Client.Desktop.Impl.Common
{
    [DataContract]
    public class SharePointConnectionAuthMode
    {
        static SharePointConnectionAuthMode()
        {
            NotConnected = new SharePointConnectionAuthMode
            {
                Id = new Guid("3575E2F4-0011-4EF3-BADE-A47F26F78A36"),
                Name = "Not connected"
            };

            SharePointOnline = new SharePointConnectionAuthMode
            {
                Id = new Guid("DA39A6D5-9033-4615-B027-B84551B6983B"),
                Name = "SharePoint Online (Office 365)"
            };

            WindowsAuthentication = new SharePointConnectionAuthMode
            {
                Id = new Guid("F184E963-0ABC-4AB0-8760-66199F596F29"),
                Name = "Windows Authentication"
            };
        }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public static SharePointConnectionAuthMode SharePointOnline { get; set; }
        public static SharePointConnectionAuthMode WindowsAuthentication { get; set; }

        public static SharePointConnectionAuthMode NotConnected { get; set; }
    }
}
