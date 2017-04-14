using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet;

namespace MetaPack.Client.Desktop.Impl.ViewModels
{
    public class NuGetPackageDeploymentViewModel : ViewModelBase<NuGetPackageDeploymentViewModel>
    {
        public NuGetPackageViewModel NuGetPackage { get; set; }
        public SharePointConnectionViewModel SharePointConnection { get; set; }

        public bool IsForceDeploy { get; set; }
    }
}
