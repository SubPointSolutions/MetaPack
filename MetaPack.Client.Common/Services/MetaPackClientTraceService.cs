using System;
using System.Diagnostics;
using System.IO;
using MetaPack.Core.Services;
using MetaPack.Core.Services.Trace;

namespace MetaPack.Client.Common.Services
{
    public class MetaPackClientTraceService : EventableTraceServiceBase
    {
        #region consturctors



        public MetaPackClientTraceService()
        {
            IsVerboseEnabled = false;

            IsInformationEnabled = true;
            IsWarningEnabled = true;
            IsErrorEnabled = true;
            IsCriticalEnabled = true;

            var fileName = string.Format("metapack.{0}.log", GetTimestamp());
            LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            IsConsoleWriterEnabled = true;
            IsFileWriterEnabled = true;

            Out = Console.Out;
        }

        #endregion

        #region props

        public bool IsConsoleWriterEnabled { get; set; }
        public bool IsTraceWriterEnabled { get; set; }
        public bool IsDebugWriterEnabled { get; set; }

        public bool IsFileWriterEnabled { get; set; }

        public bool IsVerboseEnabled { get; set; }
        public bool IsInformationEnabled { get; set; }
        public bool IsWarningEnabled { get; set; }
        public bool IsErrorEnabled { get; set; }
        public bool IsCriticalEnabled { get; set; }

        public string LogFilePath { get; set; }

        public TraceEventFormatServiceBase TraceEventFormatter { get; set; }

        #endregion

        #region methods

        protected virtual String GetTimestamp()
        {
            return (DateTime.Now).ToString("yyyyMMdd_HHmm_ssfff");
        }

        protected virtual void InternalWrite(TraceEventLevel level, string message)
        {
            var internalMessage = string.Format("[{0}]: {1}", level, message);
            var internalOutputMessage = internalMessage;

            if (TraceEventFormatter != null)
            {
                internalOutputMessage = TraceEventFormatter.FormatEvent(new TraceEventFormatOptions
                {
                    Level = level.ToString(),
                    Message = message,
                    TimeStamp = DateTime.Now
                });
            }

            if (IsConsoleWriterEnabled)
                Out.WriteLine(internalOutputMessage);

            if (IsTraceWriterEnabled)
                Trace.WriteLine(internalOutputMessage);

            if (IsDebugWriterEnabled)
                Trace.WriteLine(internalOutputMessage);

            if (IsFileWriterEnabled)
            {
                using (var sw = File.AppendText(LogFilePath))
                    sw.WriteLine(internalOutputMessage);
            }

            InvokeOnTraceMessage(new TraceEventEventArgs
            {
                Level = level,
                Message = message
            });
        }

        public override void Critical(int id, object message, Exception exception)
        {
            if (IsCriticalEnabled)
                InternalWrite(TraceEventLevel.Critical, message + ((exception != null) ? exception.ToString() : string.Empty));
        }

        public override void Error(int id, object message, Exception exception)
        {
            if (IsErrorEnabled)
                InternalWrite(TraceEventLevel.Error, message + ((exception != null) ? exception.ToString() : string.Empty));
        }

        public override void Warning(int id, object message, Exception exception)
        {
            if (IsWarningEnabled)
                InternalWrite(TraceEventLevel.Warning, message + ((exception != null) ? exception.ToString() : string.Empty));
        }

        public override void Information(int id, object message, Exception exception)
        {
            if (IsInformationEnabled)
                InternalWrite(TraceEventLevel.Information, message + ((exception != null) ? exception.ToString() : string.Empty));
        }

        public override void Verbose(int id, object message, Exception exception)
        {
            if (IsVerboseEnabled)
                InternalWrite(TraceEventLevel.Verbose, message + ((exception != null) ? exception.ToString() : string.Empty));
        }

        public override void TraceActivityStart(int id, object message)
        {

        }

        public override void TraceActivityStop(int id, object message)
        {

        }

        public override void TraceActivityTransfer(int id, object message, Guid relatedActivityId)
        {

        }


        private Guid? _currentActivityId;

        public override Guid CurrentActivityId
        {
            get
            {
                if (_currentActivityId == null)
                    _currentActivityId = Guid.NewGuid();

                return _currentActivityId.Value;
            }
            set
            {
                _currentActivityId = value;
            }
        }

        public TextWriter Out { get; private set; }

        #endregion
    }
}
