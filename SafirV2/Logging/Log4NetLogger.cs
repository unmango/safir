using log4net;
using System;

namespace Safir.Logging
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog Log;

        public Log4NetLogger(Type type) {
            Log = LogManager.GetLogger(type);
        }

        public bool IsFatalEnabled => Log.IsFatalEnabled;

        public bool IsWarnEnabled => Log.IsWarnEnabled;

        public bool IsInfoEnabled => Log.IsInfoEnabled;

        public bool IsDebugEnabled => Log.IsDebugEnabled;

        public bool IsErrorEnabled => Log.IsErrorEnabled;

        public void Debug(object message) =>
            Log.Debug(message);

        public void Debug(object message, Exception exception) =>
            Log.Debug(message, exception);

        public void DebugFormat(IFormatProvider provider, string format, params object[] args) =>
            Log.DebugFormat(provider, format, args);

        public void DebugFormat(string format, params object[] args) =>
            Log.DebugFormat(format, args);

        public void DebugFormat(string format, object arg0) =>
            Log.DebugFormat(format, arg0);

        public void DebugFormat(string format, object arg0, object arg1, object arg2) =>
            Log.DebugFormat(format, arg0, arg1, arg2);

        public void DebugFormat(string format, object arg0, object arg1) =>
            Log.DebugFormat(format, arg0, arg1);

        public void Error(object message) =>
            Log.Error(message);

        public void Error(object message, Exception exception) =>
            Log.Error(message, exception);

        public void Error(string format, params object[] args) =>
            Log.ErrorFormat(format, args);

        public void Error(Exception exception, string format, params object[] args) =>
            Log.ErrorFormat(format, exception, args);

        public void Error(Exception exception) =>
            Log.Error(exception);

        public void ErrorFormat(string format, object arg0, object arg1, object arg2) =>
            Log.ErrorFormat(format, arg0, arg1, arg2);

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args) =>
            Log.ErrorFormat(provider, format, args);

        public void ErrorFormat(string format, object arg0, object arg1) =>
            Log.ErrorFormat(format, arg0, arg1);

        public void ErrorFormat(string format, object arg0) =>
            Log.ErrorFormat(format, arg0);

        public void ErrorFormat(string format, params object[] args) =>
            Log.ErrorFormat(format, args);

        public void Fatal(object message) =>
            Log.Fatal(message);

        public void Fatal(object message, Exception exception) =>
            Log.Fatal(message, exception);

        public void FatalFormat(string format, object arg0, object arg1, object arg2) =>
            Log.FatalFormat(format, arg0, arg1, arg2);

        public void FatalFormat(string format, object arg0) =>
            Log.FatalFormat(format, arg0);

        public void FatalFormat(string format, params object[] args) =>
            Log.FatalFormat(format, args);

        public void FatalFormat(IFormatProvider provider, string format, params object[] args) =>
            Log.FatalFormat(provider, format, args);

        public void FatalFormat(string format, object arg0, object arg1) =>
            Log.FatalFormat(format, arg0, arg1);

        public void Info(object message, Exception exception) =>
            Log.Info(message, exception);

        public void Info(object message) =>
            Log.Info(message);

        public void Info(string format, params object[] args) =>
            Log.InfoFormat(format, args);

        public void InfoFormat(string format, object arg0, object arg1, object arg2) =>
            Log.InfoFormat(format, arg0, arg1, arg2);

        public void InfoFormat(string format, object arg0, object arg1) =>
            Log.InfoFormat(format, arg0, arg1);

        public void InfoFormat(string format, object arg0) =>
            Log.InfoFormat(format, arg0);

        public void InfoFormat(string format, params object[] args) =>
            Log.InfoFormat(format, args);

        public void InfoFormat(IFormatProvider provider, string format, params object[] args) =>
            Log.InfoFormat(provider, format, args);

        public void Warn(object message) =>
            Log.Warn(message);

        public void Warn(object message, Exception exception) =>
            Log.Warn(message, exception);

        public void Warn(string format, params object[] args) =>
            Log.WarnFormat(format, args);

        public void WarnFormat(string format, object arg0, object arg1) =>
            Log.WarnFormat(format, arg0, arg1);

        public void WarnFormat(string format, object arg0) =>
            Log.WarnFormat(format, arg0);

        public void WarnFormat(string format, params object[] args) =>
            Log.WarnFormat(format, args);

        public void WarnFormat(IFormatProvider provider, string format, params object[] args) =>
            Log.WarnFormat(provider, format, args);

        public void WarnFormat(string format, object arg0, object arg1, object arg2) =>
            Log.WarnFormat(format, arg0, arg1, arg2);
    }
}
