// <copyright file="Log4NetAdapter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Core.Helpers
{
    using log4net;
    using log4net.Core;

    public sealed class Log4NetAdapter<T> : LogImpl
    {
        public Log4NetAdapter()
            : base(LogManager.GetLogger(typeof(T)).Logger) {
        }
    }
}
