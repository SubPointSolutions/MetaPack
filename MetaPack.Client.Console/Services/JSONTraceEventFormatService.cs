using MetaPack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace MetaPack.Client.Console.Services
{
    public class JSONTraceEventFormatService : TraceEventFormatServiceBase
    {
        public JSONTraceEventFormatService()
        {
            Serializer = new JavaScriptSerializer();
        }

        #region properties

        protected JavaScriptSerializer Serializer { get; private set; }

        #endregion

        public override string FormatEvent(TraceEventFormatOptions options)
        {
            if (options == null)
                return string.Empty;

            return Serializer.Serialize(options);
        }
    }
}
