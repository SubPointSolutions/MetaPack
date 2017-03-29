using System;
using System.Diagnostics;
using System.Windows.Forms;
using MetaPack.Core;
using MetaPack.Core.Services;
using MetroFramework.Forms;
using SubPointSolutions.Shelly.Desktop.MetroFramework.Services;
using SubPointSolutions.Shelly.Desktop.Services;
using MetroFramework;
using MetroFramework.Components;

namespace MetaPack.Client.Desktop.Impl.Services
{
    //public class MetaPackAppService : MetroFrameworkDesktopAppService
    public class MetaPackAppService : ShDesktopAppService
    {
        public void InitAppServices()
        {
            this.InitInternal();
        }

        protected override Form CreateAppForm()
        {
            var form = base.CreateAppForm();

            var metroForm = form as MetroForm;

            if (metroForm != null)
            {
                //metroForm.StyleManager = new MetroStyleManager();
                //metroForm.StyleManager.Style = MetroColorStyle.Red;

                //metroForm.StyleManager.Update();
            }

            return form;
        }

        public MetaPackAppService()
        {
            AppName = "MetaPack GUI";
            AppVersion = FileVersionInfo.GetVersionInfo(typeof(MetaPackAppService).Assembly.Location).FileVersion;

            var additionaAssemblies = new[]
            {
                typeof(MetroFrameworkAppUIService).Assembly,
                //typeof(WfAppUIService).Assembly,
                typeof(ShBusinessEntityDataService).Assembly
            };

            AppDataServiceAssemblies.AddRange(additionaAssemblies);
            AppUIServiceAssemblies.AddRange(additionaAssemblies);
            AppPluginsAssemblies.AddRange(additionaAssemblies);

            ConfigureMetaPackServices();
        }

        private void ConfigureMetaPackServices()
        {
            var logging = new MetaPackUITraceService();
            MetaPackServiceContainer.Instance.ReplaceService(typeof(TraceServiceBase), logging);
        }

        protected override void InitAppUIServices(object uiHostControl)
        {
            base.InitAppUIServices(uiHostControl);

            var form = uiHostControl as Form;

            if (form != null)
            {
                form.ShowIcon = false;
            }
        }

        protected override void ConfigureAppDataServices()
        {
            base.ConfigureAppDataServices();

            // map out logging services
            //m2ServiceContainer.Instance.Services[typeof(SPMeta2.Services.TraceServiceBase)].Clear();
            //m2ServiceContainer.Instance.Services[typeof(SPMeta2.Services.TraceServiceBase)].Add(new m2EventableTraceService());

            //mpServiceContainer.Instance.Services[typeof(MetaPack.Core.Services.Base.TraceServiceBase)].Clear();
            //mpServiceContainer.Instance.Services[typeof(MetaPack.Core.Services.Base.TraceServiceBase)].Add(new mpEventableTraceService());

            //var defService = ShServiceContainer.Instance.QuarkAppMetadataService.GetAppDataService<Quark.Core.Services.TraceSourceService>();

            //ShServiceContainer.Instance.QuarkAppMetadataService.AppDataServices.Remove(defService);
            //ShServiceContainer.Instance.QuarkAppMetadataService.AppDataServices.Add(new qEventableTraceService());
        }

        protected override void ConfigureAppForm(Form form)
        {
            base.ConfigureAppForm(form);

            form.Width = 900;
            form.Height = 600;

            //AppUIServiceAssemblies.Add(typeof(WfAppUIService).Assembly);

            SetApplicationTitle(string.Format("{0} - not connected", AppName));
        }
    }
}