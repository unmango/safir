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

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class MainMenuViewModel : Conductor<object>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;

        private readonly PreferencesViewModel _preferences;

        public MainMenuViewModel(
            IEventAggregator eventAggregator,
            IWindowManager manager,
            PreferencesViewModel preferences) {
            _eventAggregator = eventAggregator;
            _windowManager = manager;
            _preferences = preferences;
        }

        public List<MenuItem> MenuItems { get; set; }

        public void AddItem() {
            MenuItems.Add(new MenuItem { Header = "NewItem" });
        }

        public void OpenPreferences() {
            ActivateItem(_preferences);
            _windowManager.ShowDialog(_preferences);
        }

        public void Close() {
            _eventAggregator.PublishOnUIThread(new CloseMainWindowEvent());
        }
    }
}
