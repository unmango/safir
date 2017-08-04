// <copyright file="AppFolderHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Core.Helpers
{
    using System;
    using System.IO;

    public static class AppFolderHelper
    {
        public static string GetAppFolderPath(string appName)
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
#if DEBUG
            folder = Path.GetTempPath();
#endif
            string sFolder = Path.Combine(folder, appName);
            if (!Directory.Exists(sFolder))
                Directory.CreateDirectory(sFolder);
            return sFolder;
        }
    }
}
