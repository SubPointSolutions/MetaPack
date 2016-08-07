﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Core.Services.Base;
using MetaPack.Core.Utils;

namespace MetaPack.Core.Services
{
    public class TraceSourceService : TraceServiceBase
    {
        #region properties

        public TraceSourceService()
            : this("MetaPack")
        {
        }

        public TraceSourceService(string traceSourceName)
        {
            TraceSource = new TraceSource(traceSourceName);
        }

        #endregion

        #region properties

        public TraceSource TraceSource;

        #endregion

        #region methods

        public override void Critical(int id, object message, Exception exception)
        {
            TraceEvent(id, TraceEventType.Critical, message, exception);
        }

        public override void Error(int id, object message, Exception exception)
        {
            TraceEvent(id, TraceEventType.Error, message, exception);
        }

        public override void Warning(int id, object message, Exception exception)
        {
            TraceEvent(id, TraceEventType.Warning, message, exception);
        }

        public override void Information(int id, object message, Exception exception)
        {
            TraceEvent(id, TraceEventType.Information, message, exception);
        }

        public override void Verbose(int id, object message, Exception exception)
        {
            TraceEvent(id, TraceEventType.Verbose, message, exception);
        }

        #endregion

        #region utils

        protected virtual void TraceEvent(int id, TraceEventType messageType, object message, Exception exception)
        {
            var traceString = string.Empty;
            var messageString = message == null ? string.Empty : message.ToString();

            if (exception != null)
            {
                var subMessage = messageString;

                if (!subMessage.EndsWith("."))
                    subMessage += ".";

                traceString = string.Format("{0} Exception: [{1}]", subMessage, exception);
            }
            else
            {
                var subMessage = messageString;

                if (!subMessage.EndsWith("."))
                    subMessage += ".";

                traceString = string.Format("{0}", subMessage);
            }

            TraceSource.TraceEvent(messageType, id, traceString);
            TraceSource.Flush();
        }

        #endregion

        public override void TraceActivityStart(int id, object message)
        {
            TraceSource.TraceEvent(TraceEventType.Start, id, ConvertUtils.ToString(message));
        }

        public override void TraceActivityStop(int id, object message)
        {
            TraceSource.TraceEvent(TraceEventType.Stop, id, ConvertUtils.ToString(message));
        }

        public override void TraceActivityTransfer(int id, object message, Guid relatedActivityId)
        {
            TraceSource.TraceTransfer(id, ConvertUtils.ToString(message), relatedActivityId);
        }

        public override Guid CurrentActivityId
        {
            get
            {
                return Trace.CorrelationManager.ActivityId;
            }
            set { Trace.CorrelationManager.ActivityId = value; }
        }
    }
}
