using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Client.Desktop.Impl.Common;
using MetaPack.Client.Desktop.Impl.ViewModels;

namespace MetaPack.Client.Desktop.Impl.ViewModels
{
    public class SharePointConnectionViewModel :
        TypedViewModelBase<SharePointConnectionViewModel, SharePointConnection>
    {
        static SharePointConnectionViewModel()
        {
            AuthModes = new List<SharePointConnectionAuthMode>();

            AuthModes.Add(SharePointConnectionAuthMode.SharePointOnline);
            AuthModes.Add(SharePointConnectionAuthMode.WindowsAuthentication);

            NotConnected = new SharePointConnectionViewModel
            {
                Dto = new SharePointConnection
                {
                    AuthMode = SharePointConnectionAuthMode.NotConnected,
                    Url = "Not connected"
                }
            };
        }

        public SharePointConnectionViewModel()
            : this(new SharePointConnection())
        {

        }

        public SharePointConnectionViewModel(SharePointConnection dto) :
            base(dto)
        {
            AuthMode = SharePointConnectionAuthMode.SharePointOnline;
        }

        public string Url
        {
            get { return Dto.Url; }
            set
            {
                Dto.Url = value;
                OnPropertyChanged(c => c.Url);
            }
        }

        public SharePointConnectionAuthMode AuthMode
        {
            get { return Dto.AuthMode; }
            set
            {
                Dto.AuthMode = value;
                OnPropertyChanged(c => c.AuthMode);
            }
        }

        public string UserName
        {
            get { return Dto.UserName; }
            set
            {
                Dto.UserName = value;
                OnPropertyChanged(c => c.UserName);
            }
        }

        public string UserPassword
        {
            get { return Dto.UserPassword; }
            set
            {
                Dto.UserPassword = value;
                OnPropertyChanged(c => c.UserPassword);
            }
        }

        public static List<SharePointConnectionAuthMode> AuthModes { get; private set; }
        public static SharePointConnectionViewModel NotConnected { get; set; }
    }
}
