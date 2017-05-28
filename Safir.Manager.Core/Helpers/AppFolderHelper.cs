using System;
using System.IO;

namespace Safir.Core.Helpers
{
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
