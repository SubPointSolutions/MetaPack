using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Core.Services;
using MetaPack.Core.Services.Base;

namespace MetaPack.Core
{
    public class MetaPackServiceContainer
    {
        #region constructors

        static MetaPackServiceContainer()
        {
            Instance = new MetaPackServiceContainer();
        }

        private MetaPackServiceContainer()
        {
            Services = new Dictionary<Type, List<object>>();

            InitServices();
        }

        private void InitServices()
        {
            RegisterService(typeof(TraceServiceBase), new TraceSourceService());
        }

        #endregion

        #region properties

        public Dictionary<Type, List<object>> Services { get; set; }

        #endregion

        #region methods

        public void RegisterService(Type type, object service)
        {
            if (!Services.ContainsKey(type))
                Services.Add(type, new List<object>());

            var list = Services[type];

            if (!list.Contains(service))
                list.Add(service);
        }

        public void RegisterServices(Type type, List<object> services)
        {
            foreach (var s in services)
                RegisterService(type, s);
        }

        public TService GetService<TService>()
            where TService : class
        {
            List<object> services;

            Services.TryGetValue(typeof(TService), out services);

            if (services != null)
                return services.FirstOrDefault() as TService;

            return null;
        }

        public IEnumerable<TService> GetServices<TService>()
           where TService : class
        {
            List<object> services;

            Services.TryGetValue(typeof(TService), out services);

            if (services != null)
            {
                return services
                    .Where(s => s as TService != null)
                    .Select(s => s as TService);
            }

            return Enumerable.Empty<TService>();
        }

        #endregion

        #region properties

        public static MetaPackServiceContainer Instance { get; set; }

        #endregion
    }
}
