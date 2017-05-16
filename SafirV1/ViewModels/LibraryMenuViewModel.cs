using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safir.ViewModels
{
    internal class LibraryMenuViewModel : ViewModelBase
    {
        public LibraryMenuViewModel()
        {
            Items = new List<LibraryItem>
            {
                new LibraryItem { ImagePath = "../Resources/Icons/brand.ico", Name = "TestItem" },
                new LibraryItem { ImagePath = "../Resources/Icons/brand.ico", Name = "TestItem2" },
                new LibraryItem { ImagePath = "../Resources/Icons/brand.ico", Name = "TestItem3" },
                new LibraryItem { ImagePath = "../Resources/Icons/brand.ico", Name = "TestItem4" },
            };
        }
        
        public List<LibraryItem> Items { get; set; }

        public class LibraryItem
        {
            public string ImagePath { get; set; }
            public string Name { get; set; }
        }
    }
}
