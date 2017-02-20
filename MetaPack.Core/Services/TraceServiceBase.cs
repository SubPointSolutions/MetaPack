using System;

namespace MetaPack.Core.Services
{
    /// <summary>
    /// High-level abstration for loggin service
    /// </summary>
    public abstract class TraceServiceBase
    {
        #region methods

        public virtual void Critical(int id, object message)
        {
            Critical(id, message, null);
        }

        public abstract void Critical(int id, object message, Exception exception);

        public virtual void Error(int id, object message)
        {
            Error(id, message, null);
        }

        public abstract void Error(int id, object message, Exception exception);

        public virtual void Warning(int id, object message)
        {
            Warning(id, message, null);
        }

        public abstract void Warning(int id, object message, Exception exception);

        public virtual void Information(int id, object message)
        {
            Information(id, message, null);
        }

        public abstract void Information(int id, object message, Exception exception);

        public virtual void Verbose(int id, object message)
        {
            Verbose(id, message, null);
        }

        public abstract void Verbose(int id, object message, Exception exception);

        public abstract void TraceActivityStart(int id, object message);
        public abstract void TraceActivityStop(int id, object message);
        public abstract void TraceActivityTransfer(int id, object message, Guid relatedActivityId);

        public abstract Guid CurrentActivityId
        {
            get;
            set;
        }

        #endregion
    }
}
