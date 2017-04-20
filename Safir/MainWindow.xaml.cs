using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Safir
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SeedData();
        }

        public void SeedData()
        {
            PopulatePlaylists();
        }

        public void PopulatePlaylists()
        {
            var list = new ObservableCollection<Playlist>()
            {
                new Playlist{Image = "Image1", Name = "Playlist1"},
                new Playlist{Image = "Image2", Name = "Playlist2"},
                new Playlist{Image = "Image3", Name = "Playlist3"},
                new Playlist{Image = "Image4", Name = "Playlist4"},
                new Playlist{Image = "Image5", Name = "Playlist5"},
            };

            ICollectionView view = CollectionViewSource.GetDefaultView(list);
            view.GroupDescriptions.Add(new PropertyGroupDescription("Image"));
            lbPlaylists.ItemsSource = view;
        }

        class Playlist
        {
            public string Image { get; set; }
            public string Name { get; set; }
        }
    }
}
