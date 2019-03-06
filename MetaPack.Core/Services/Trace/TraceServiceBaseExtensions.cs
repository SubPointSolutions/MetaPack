using System;

namespace MetaPack.Core.Services.Trace
{
    public static class TraceServiceBaseExtensions
    {
        #region information

        public static void InformationFormat(this TraceServiceBase traceService, int id, object message, object parameter)
        {
            InformationFormat(traceService, id, message, new object[] { parameter });
        }

        public static void InformationFormat(this TraceServiceBase traceService, int id, object message, object[] parameters)
        {
            InformationFormat(traceService, id, message, parameters, null);
        }

        public static void InformationFormat(this TraceServiceBase traceService, int id, object message, object[] parameters, Exception exception)
        {
            if (message is string)
            {
                traceService.Information(id, string.Format(message as string, parameters), exception);
            }
            else
            {
                traceService.Information(id, message, exception);
            }
        }

        #endregion

        #region warning

        public static void WarningFormat(this TraceServiceBase traceService, int id, object message, object parameter)
        {
            WarningFormat(traceService, id, message, new object[] { parameter });
        }

        public static void WarningFormat(this TraceServiceBase traceService, int id, object message, object[] parameters)
        {
            WarningFormat(traceService, id, message, parameters, null);
        }

        public static void WarningFormat(this TraceServiceBase traceService, int id, object message, object[] parameters, Exception exception)
        {
            if (message is string)
            {
                traceService.Warning(id, string.Format(message as string, parameters), exception);
            }
            else
            {
                traceService.Warning(id, message, exception);
            }
        }

        #endregion

        #region verbose

        public static void VerboseFormat(this TraceServiceBase traceService, int id, object message, object parameter)
        {
            if (parameter == null)
                VerboseFormat(traceService, id, message, new object[] { });
            else
                VerboseFormat(traceService, id, message, new object[] { parameter });
        }

        public static void VerboseFormat(this TraceServiceBase traceService, int id, object message, object[] parameters)
        {
            if (parameters == null)
                VerboseFormat(traceService, id, message, new object[] { }, null);
            else
                VerboseFormat(traceService, id, message, parameters, null);
        }

        public static void VerboseFormat(this TraceServiceBase traceService, int id, object message, object[] parameters, Exception exception)
        {
            if (message is string && parameters != null && parameters.Length > 0)
            {
                traceService.Verbose(id, string.Format(message as string, parameters), exception);
            }
            else
            {
                traceService.Verbose(id, message, exception);
            }
        }

        #endregion

        #region error

        public static void ErrorFormat(this TraceServiceBase traceService, int id, object message, object parameter)
        {
            if (parameter == null)
                ErrorFormat(traceService, id, message, new object[] { });
            else
                ErrorFormat(traceService, id, message, new object[] { parameter });
        }

        public static void ErrorFormat(this TraceServiceBase traceService, int id, object message, object[] parameters)
        {
            if (parameters == null)
                ErrorFormat(traceService, id, message, new object[] { }, null);
            else
                ErrorFormat(traceService, id, message, parameters, null);
        }

        public static void ErrorFormat(this TraceServiceBase traceService, int id, object message, object[] parameters, Exception exception)
        {
            if (message is string && parameters != null && parameters.Length > 0)
            {
                traceService.Error(id, string.Format(message as string, parameters), exception);
            }
            else
            {
                traceService.Error(id, message, exception);
            }
        }

        #endregion
    }
}
