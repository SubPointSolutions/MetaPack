using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SubPointSolutions.Shelly.Desktop.Services;

namespace MetaPack.Client.Desktop.Impl.Controls.Base
{
    public class ViewUserControl<TViewModel> : UserControl
        where TViewModel : class, new()
    {
        private TViewModel _viewModel;

        public TViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;

                BindService = ShBinder.NewBinder<TViewModel>(ViewModel);
                OnBindViewModel();
            }
        }

        public ShBindService<TViewModel> BindService { get; set; }

        protected virtual void OnBindViewModel()
        {

        }
    }
}
