using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safir.ViewModels
{
    using Core.Settings;

    public class LibraryMenuViewModel
    {
        private ISettingStore _settings;

        public LibraryMenuViewModel(
            ISettingStore settings) {
            _settings = settings;
        }


    }
}
