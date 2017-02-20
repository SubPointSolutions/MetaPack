﻿using MetaPack.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Tests.Services
{
    public class RegressionTraceService : TraceServiceBase
    {
        #region consturctors

        public RegressionTraceService()
        {
            IsVerboseEnabled = true;

            IsInformationEnabled = true;
            IsWarningEnabled = true;
            IsErrorEnabled = true;
            IsCriticalEnabled = true;

            var fileName = string.Format("metapack.regression.{0}.log", GetTimestamp());
            LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        #endregion

        #region classes

        protected enum Level
        {
            Critical,
            Error,
            Warning,
            Information,
            Verbose
        }

        #endregion

        #region props

        public bool IsVerboseEnabled { get; set; }
        public bool IsInformationEnabled { get; set; }
        public bool IsWarningEnabled { get; set; }
        public bool IsErrorEnabled { get; set; }
        public bool IsCriticalEnabled { get; set; }

        public string LogFilePath { get; set; }

        #endregion

        #region methods

        protected virtual string GetTimestamp()
        {
            return (DateTime.Now).ToString("yyyyMMdd_HHmmssfff");
        }

        protected virtual void InternalWrite(Level level, string message)
        {
            var internalMessgae = string.Format("[{0}]: {1}", level, message);

            Trace.WriteLine(internalMessgae);

            using (var sw = File.AppendText(LogFilePath))
                sw.WriteLine(internalMessgae);
        }

        public override void Critical(int id, object message, Exception exception)
        {
            if (IsCriticalEnabled)
                InternalWrite(Level.Critical, message + ((exception != null) ? exception.ToString() : string.Empty));
        }

        public override void Error(int id, object message, Exception exception)
        {
            if (IsErrorEnabled)
                InternalWrite(Level.Error, message + ((exception != null) ? exception.ToString() : string.Empty));
        }

        public override void Warning(int id, object message, Exception exception)
        {
            if (IsWarningEnabled)
                InternalWrite(Level.Warning, message + ((exception != null) ? exception.ToString() : string.Empty));
        }

        public override void Information(int id, object message, Exception exception)
        {
            if (IsInformationEnabled)
                InternalWrite(Level.Information, message + ((exception != null) ? exception.ToString() : string.Empty));
        }

        public override void Verbose(int id, object message, Exception exception)
        {
            if (IsVerboseEnabled)
                InternalWrite(Level.Verbose, message + ((exception != null) ? exception.ToString() : string.Empty));
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
        #endregion
    }
}
