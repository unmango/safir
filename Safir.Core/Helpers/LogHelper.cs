// <copyright file="LogHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Core.Helpers
{
    using System.Runtime.CompilerServices;
    using log4net;

    public class LogHelper
    {
        public static ILog GetLogger([CallerFilePath]string filename = "") {
            return LogManager.GetLogger(filename);
        }
    }
}
