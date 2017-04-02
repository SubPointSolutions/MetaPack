using System;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;
using MetaPack.Core;
using MetaPack.Core.Services;
using MetroFramework.Forms;
using SubPointSolutions.Shelly.Desktop.MetroFramework.Services;
using SubPointSolutions.Shelly.Desktop.Services;
using MetroFramework;
using MetroFramework.Components;
using System.Reflection;
using SubPointSolutions.Shelly.Core.Exceptions;

namespace MetaPack.Client.Desktop.Impl.Services
{
    //public class MetaPackAppService : MetroFrameworkDesktopAppService
    public class MetaPackAppService : ShDesktopAppService
    {
        public void InitAppServices()
        {
            this.InitInternal();
        }

        public override void Run()
        {
            ProcessArgs();

            base.Run();
        }

        private void ProcessArgs()
        {
            if (Args != null
               && Args.Any(a => !string.IsNullOrEmpty(a) && a.ToLower() == "version"))
            {
                var fileVersionAttribute = GetAssemblyFileVersion(GetType().Assembly);

                Console.WriteLine(fileVersionAttribute.ToString());
                Environment.Exit(0);
            }
        }

        protected virtual Version GetAssemblyFileVersion(Assembly assembly)
        {
            var fileVersionAttribute = Attribute.GetCustomAttribute(
                                            assembly,
                                            typeof(AssemblyFileVersionAttribute), false) as AssemblyFileVersionAttribute;

            // paranoic about regression caused by MS changes in the future
            if (fileVersionAttribute == null)
            {
                throw new ShAppException(string.Format(
                        "Cannot find AssemblyFileVersionAttribute in assembly:[{0}]",
                        assembly.FullName));
            }

            if (string.IsNullOrEmpty(fileVersionAttribute.Version))
            {
                throw new ShAppException(string.Format(
                        "Found AssemblyFileVersionAttribute but .Version is null or empty. Assembly:[{0}]",
                        assembly.FullName));
            }


            return new Version(fileVersionAttribute.Version);
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