// <copyright file="Log4NetLogger.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Logging
{
    using System;
    using log4net;

    public class Log4NetLogger : ILogger
    {
        private readonly ILog _log;

        public Log4NetLogger(Type type) {
            _log = LogManager.GetLogger(type);
        }

        public bool IsFatalEnabled => _log.IsFatalEnabled;

        public bool IsWarnEnabled => _log.IsWarnEnabled;

        public bool IsInfoEnabled => _log.IsInfoEnabled;

        public bool IsDebugEnabled => _log.IsDebugEnabled;

        public bool IsErrorEnabled => _log.IsErrorEnabled;

        public void Debug(object message) =>
            _log.Debug(message);

        public void Debug(object message, Exception exception) =>
            _log.Debug(message, exception);

        public void DebugFormat(IFormatProvider provider, string format, params object[] args) =>
            _log.DebugFormat(provider, format, args);

        public void DebugFormat(string format, params object[] args) =>
            _log.DebugFormat(format, args);

        public void DebugFormat(string format, object arg0) =>
            _log.DebugFormat(format, arg0);

        public void DebugFormat(string format, object arg0, object arg1, object arg2) =>
            _log.DebugFormat(format, arg0, arg1, arg2);

        public void DebugFormat(string format, object arg0, object arg1) =>
            _log.DebugFormat(format, arg0, arg1);

        public void Error(object message) =>
            _log.Error(message);

        public void Error(object message, Exception exception) =>
            _log.Error(message, exception);

        public void Error(string format, params object[] args) =>
            _log.ErrorFormat(format, args);

        public void Error(Exception exception, string format, params object[] args) =>
            _log.ErrorFormat(format, exception, args);

        public void Error(Exception exception) =>
            _log.Error(exception);

        public void ErrorFormat(string format, object arg0, object arg1, object arg2) =>
            _log.ErrorFormat(format, arg0, arg1, arg2);

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args) =>
            _log.ErrorFormat(provider, format, args);

        public void ErrorFormat(string format, object arg0, object arg1) =>
            _log.ErrorFormat(format, arg0, arg1);

        public void ErrorFormat(string format, object arg0) =>
            _log.ErrorFormat(format, arg0);

        public void ErrorFormat(string format, params object[] args) =>
            _log.ErrorFormat(format, args);

        public void Fatal(object message) =>
            _log.Fatal(message);

        public void Fatal(object message, Exception exception) =>
            _log.Fatal(message, exception);

        public void FatalFormat(string format, object arg0, object arg1, object arg2) =>
            _log.FatalFormat(format, arg0, arg1, arg2);

        public void FatalFormat(string format, object arg0) =>
            _log.FatalFormat(format, arg0);

        public void FatalFormat(string format, params object[] args) =>
            _log.FatalFormat(format, args);

        public void FatalFormat(IFormatProvider provider, string format, params object[] args) =>
            _log.FatalFormat(provider, format, args);

        public void FatalFormat(string format, object arg0, object arg1) =>
            _log.FatalFormat(format, arg0, arg1);

        public void Info(object message, Exception exception) =>
            _log.Info(message, exception);

        public void Info(object message) =>
            _log.Info(message);

        public void Info(string format, params object[] args) =>
            _log.InfoFormat(format, args);

        public void InfoFormat(string format, object arg0, object arg1, object arg2) =>
            _log.InfoFormat(format, arg0, arg1, arg2);

        public void InfoFormat(string format, object arg0, object arg1) =>
            _log.InfoFormat(format, arg0, arg1);

        public void InfoFormat(string format, object arg0) =>
            _log.InfoFormat(format, arg0);

        public void InfoFormat(string format, params object[] args) =>
            _log.InfoFormat(format, args);

        public void InfoFormat(IFormatProvider provider, string format, params object[] args) =>
            _log.InfoFormat(provider, format, args);

        public void Warn(object message) =>
            _log.Warn(message);

        public void Warn(object message, Exception exception) =>
            _log.Warn(message, exception);

        public void Warn(string format, params object[] args) =>
            _log.WarnFormat(format, args);

        public void WarnFormat(string format, object arg0, object arg1) =>
            _log.WarnFormat(format, arg0, arg1);

        public void WarnFormat(string format, object arg0) =>
            _log.WarnFormat(format, arg0);

        public void WarnFormat(string format, params object[] args) =>
            _log.WarnFormat(format, args);

        public void WarnFormat(IFormatProvider provider, string format, params object[] args) =>
            _log.WarnFormat(provider, format, args);

        public void WarnFormat(string format, object arg0, object arg1, object arg2) =>
            _log.WarnFormat(format, arg0, arg1, arg2);
    }
}
