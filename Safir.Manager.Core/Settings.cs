using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safir.Core
{
    public class Settings
    {
        private string _baseDir;

        public Settings()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string sFolder = Path.Combine(folder, "Safir");
            if (!Directory.Exists(sFolder))
                Directory.CreateDirectory(sFolder);
        }

        public string GetDB()
        {
            throw new NotImplementedException();
        }
    }
}
