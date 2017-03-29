using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Client.Desktop.Impl.Common;
using MetaPack.Client.Desktop.Impl.ViewModels;

namespace MetaPack.Client.Desktop.Impl.ViewModels
{
    public class NuGetGalleryConnectionViewModel : TypedViewModelBase<NuGetGalleryConnectionViewModel, NuGetGalleryConnection>
    {
        public NuGetGalleryConnectionViewModel()
        {
            
        }

        public NuGetGalleryConnectionViewModel(NuGetGalleryConnection dto) :
            base(dto)
        {

        }

        public string Name
        {
            get { return Dto.Name; }
            set
            {
                Dto.Name = value;
                OnPropertyChanged(c => c.Name);
            }
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

       
    }
}
