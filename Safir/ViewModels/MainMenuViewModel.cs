// <copyright file="MainMenuViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Controls;
    using Caliburn.Micro;
    using Events;
    using Microsoft.WindowsAPICodePack.Dialogs;
    using Safir.Manager;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class MainMenuViewModel : Conductor<object>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;
        private readonly MusicManager _musicManager;

        private readonly PreferencesViewModel _preferences;

        //private List<MenuItem> _menuItems;

        public MainMenuViewModel(
            IEventAggregator eventAggregator,
            IWindowManager windowManager,
            MusicManager musicManager,
            PreferencesViewModel preferences) {
            _eventAggregator = eventAggregator;
            _windowManager = windowManager;
            _musicManager = musicManager;
            _preferences = preferences;
        }

        public List<MenuItem> MenuItems { get; set; }

        //public List<MenuItem> MenuItems {
        //    get {
        //        if (_menuItems != null) return _menuItems;
        //        if (!(GetView() is UserControl control)) return new List<MenuItem>();
        //        var menu = (Menu)control.Content;
        //        _menuItems = (List<MenuItem>)menu.Items.SourceCollection;
        //        return _menuItems;
        //    }
        //}

        public void AddFile() {
            using (var dialog = new CommonOpenFileDialog()) {
                dialog.Title = "Select File";
                dialog.Multiselect = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok) {
                    foreach (var file in dialog.FileNames) {
                        _musicManager.AddFile(file);
                    }
                }
            }
        }

        public void AddFolder() {
            using (var dialog = new CommonOpenFileDialog()) {
                dialog.Title = "Select Folder";
                dialog.IsFolderPicker = true;
                dialog.Multiselect = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok) {
                    foreach (var dir in dialog.FileNames) {
                        _musicManager.AddFolder(dir);
                    }
                }
            }
        }

        public void OpenPreferences() {
            ActivateItem(_preferences);
            _windowManager.ShowDialog(_preferences);
        }

        public void Close() => _eventAggregator.PublishOnUIThread(new CloseMainWindowEvent());

        private void AddFileMenu() {
            MenuItems.Add(new MenuItem {
                Header = "_File",
                ItemsSource = new List<object> {
                    new MenuItem {
                        Header = "New",
                        ItemsSource = new List<object> {
                            new MenuItem { Header = "Playlist" },
                            new MenuItem { Header = "Playlist from Selection" },
                            new MenuItem { Header = "Smart Playlist" },
                            new MenuItem { Header = "Playlist Folder" }
                        }
                    },
                    new MenuItem { Header = "Edit Playlist" },
                    new MenuItem { Header = "Close Window" },
                    new Separator(),
                    new MenuItem { Header = "Add File to Library" },
                    new MenuItem { Header = "Add Folder to Library" },
                    new MenuItem { Header = "Burn Playlist to Disc" },
                    new MenuItem {
                        Header = "Library",
                        ItemsSource = new List<object> {
                            new MenuItem { Header = "Organize Library" },
                            new MenuItem { Header = "Consolidate Files..." },
                            new MenuItem { Header = "Export Library" },
                            new Separator(),
                            new MenuItem { Header = "Import Playlist" },
                            new MenuItem { Header = "Export Playlist" },
                            new MenuItem { Header = "Show Duplicate Items" },
                            new Separator(),
                            new MenuItem { Header = "Get Album Artwork" },
                            new MenuItem { Header = "Get Track Names" }
                        }
                    },
                    new MenuItem {
                        Header = "Devices",
                        ItemsSource = new List<object> {
                            new MenuItem { Header = "Sync" },
                            new Separator(),
                            new MenuItem { Header = "Back Up" },
                            new MenuItem { Header = "Restore From Backup" }
                        }
                    },
                    new Separator(),
                    new MenuItem { Header = "Page Setup" },
                    new MenuItem { Header = "Print..." },
                    new Separator(),
                    new MenuItem {
                        Header = "Exit"
                    }
                }
            });
        }
    }
}
