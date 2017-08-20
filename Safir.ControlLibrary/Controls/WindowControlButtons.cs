// <copyright file="WindowControlButtons.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.ControlLibrary.Controls
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using Annotations;

    [TemplatePart(Name = "PART_Min", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Max", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Close", Type = typeof(Button))]
    public class WindowControlButtons :
        ContentControl,
        INotifyPropertyChanged
    {
        private Button _minButton;
        private Button _maxButton;
        private Button _closeButton;

        static WindowControlButtons() {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(WindowControlButtons),
                new FrameworkPropertyMetadata(typeof(WindowControlButtons)));
        }

        public delegate void ClosingWindowEventHandler(object sender, CancelEventArgs args);

        public event ClosingWindowEventHandler ClosingWindow;

        public event PropertyChangedEventHandler PropertyChanged;

        public SafirWindow ParentWindow { get; set; }

        public override void OnApplyTemplate() {
            base.OnApplyTemplate();

            _closeButton = Template.FindName("PART_Close", this) as Button;
            if (_closeButton != null) {
                _closeButton.Click += CloseClick;
            }

            _maxButton = Template.FindName("PART_Max", this) as Button;
            if (_maxButton != null) {
                _maxButton.Click += MaximizeClick;
            }

            _minButton = Template.FindName("PART_Min", this) as Button;
            if (_minButton != null) {
                _minButton.Click += MinimizeClick;
            }
        }

        private void OnCloseWindow(CancelEventArgs args) {
            ClosingWindow?.Invoke(this, args);
        }

        private void CloseClick(object sender, RoutedEventArgs e) {
            var args = new CancelEventArgs();
            OnCloseWindow(args);

            if (args.Cancel) {
                return;
            }

            ParentWindow?.Close();
        }

        private void MaximizeClick(object sender, RoutedEventArgs e) {
            if (ParentWindow == null) {
                return;
            }

            if (ParentWindow.WindowState == WindowState.Maximized) {
                SystemCommands.RestoreWindow(ParentWindow);
            } else {
                SystemCommands.MaximizeWindow(ParentWindow);
            }
        }

        private void MinimizeClick(object sender, RoutedEventArgs e) {
            if (ParentWindow == null) {
                return;
            }

            SystemCommands.MinimizeWindow(ParentWindow);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
