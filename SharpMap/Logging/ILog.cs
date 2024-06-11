using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMap.Logging
{
    public interface ILog
    {
        #region Trace

        void Trace ( IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception );
        void Trace ( IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback );
        void Trace ( Action<FormatMessageHandler> formatMessageCallback, Exception exception );
        void Trace ( Action<FormatMessageHandler> formatMessageCallback );
        void Trace ( object message, Exception exception );
        void Trace ( object message );
        void TraceFormat ( IFormatProvider formatProvider, string format, Exception exception, params object[] args );
        void TraceFormat ( IFormatProvider formatProvider, string format, params object[] args );
        void TraceFormat ( string format, Exception exception, params object[] args );
        void TraceFormat ( string format, params object[] args );

        #endregion

        #region Debug

        void Debug ( IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception );
        void Debug ( IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback );
        void Debug ( Action<FormatMessageHandler> formatMessageCallback, Exception exception );
        void Debug ( Action<FormatMessageHandler> formatMessageCallback );
        void Debug ( object message, Exception exception );
        void Debug ( object message );
        void DebugFormat ( IFormatProvider formatProvider, string format, Exception exception, params object[] args );
        void DebugFormat ( IFormatProvider formatProvider, string format, params object[] args );
        void DebugFormat ( string format, Exception exception, params object[] args );
        void DebugFormat ( string format, params object[] args );

        #endregion

        #region Info

        void Info ( IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception );
        void Info ( IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback );
        void Info ( Action<FormatMessageHandler> formatMessageCallback, Exception exception );
        void Info ( Action<FormatMessageHandler> formatMessageCallback );
        void Info ( object message, Exception exception );
        void Info ( object message );
        void InfoFormat ( IFormatProvider formatProvider, string format, Exception exception, params object[] args );
        void InfoFormat ( IFormatProvider formatProvider, string format, params object[] args );
        void InfoFormat ( string format, Exception exception, params object[] args );
        void InfoFormat ( string format, params object[] args );

        #endregion

        #region Warn

        void Warn ( IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception );
        void Warn ( IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback );
        void Warn ( Action<FormatMessageHandler> formatMessageCallback, Exception exception );
        void Warn ( Action<FormatMessageHandler> formatMessageCallback );
        void Warn ( object message, Exception exception );
        void Warn ( object message );
        void WarnFormat ( IFormatProvider formatProvider, string format, Exception exception, params object[] args );
        void WarnFormat ( IFormatProvider formatProvider, string format, params object[] args );
        void WarnFormat ( string format, Exception exception, params object[] args );
        void WarnFormat ( string format, params object[] args );

        #endregion

        #region Error

        void Error ( IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception );
        void Error ( IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback );
        void Error ( Action<FormatMessageHandler> formatMessageCallback, Exception exception );
        void Error ( Action<FormatMessageHandler> formatMessageCallback );
        void Error ( object message, Exception exception );
        void Error ( object message );
        void ErrorFormat ( IFormatProvider formatProvider, string format, Exception exception, params object[] args );
        void ErrorFormat ( IFormatProvider formatProvider, string format, params object[] args );
        void ErrorFormat ( string format, Exception exception, params object[] args );
        void ErrorFormat ( string format, params object[] args );

        #endregion

        #region Fatal

        void Fatal ( IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception );
        void Fatal ( IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback );
        void Fatal ( Action<FormatMessageHandler> formatMessageCallback, Exception exception );
        void Fatal ( Action<FormatMessageHandler> formatMessageCallback );
        void Fatal ( object message, Exception exception );
        void Fatal ( object message );
        void FatalFormat ( IFormatProvider formatProvider, string format, Exception exception, params object[] args );
        void FatalFormat ( IFormatProvider formatProvider, string format, params object[] args );
        void FatalFormat ( string format, Exception exception, params object[] args );
        void FatalFormat ( string format, params object[] args );

        #endregion
    }
}
