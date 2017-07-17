// <copyright file="SafirWindow.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    [TemplatePart(Name = "PART_WindowControlButtons", Type = typeof(WindowControlButtons))]
    public class SafirWindow : Window
    {
        #region Dependency Properties

        public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register("ShowCloseButton", typeof(bool), typeof(SafirWindow), new PropertyMetadata(true));
        public static readonly DependencyProperty ShowMaxRestoreButtonsProperty = DependencyProperty.Register("ShowMaxRestoreButtons", typeof(bool), typeof(SafirWindow), new PropertyMetadata(true));
        public static readonly DependencyProperty ShowMinButtonProperty = DependencyProperty.Register("ShowMinButton", typeof(bool), typeof(SafirWindow), new PropertyMetadata(true));

        public static readonly DependencyProperty WindowControlButtonsProperty = DependencyProperty.Register("WindowControlButtons", typeof(WindowControlButtons), typeof(SafirWindow));

        public static readonly DependencyProperty TitleBarProperty = DependencyProperty.Register("TitleBar", typeof(ContentControl), typeof(SafirWindow), new PropertyMetadata());

        internal ContentPresenter WindowControlButtonsPresenter;

        #endregion

        static SafirWindow() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SafirWindow), new FrameworkPropertyMetadata(typeof(SafirWindow)));
        }

        #region Properties

        public WindowControlButtons WindowControlButtons {
            get { return (WindowControlButtons)GetValue(WindowControlButtonsProperty); }
            set { SetValue(WindowControlButtonsProperty, value); }
        }

        public ContentControl TitleBar {
            get { return (ContentControl)GetValue(TitleBarProperty); }
            set { SetValue(TitleBarProperty, value); }
        }

        #endregion
        
        public bool ShowCloseButton {
            get { return (bool)GetValue(ShowCloseButtonProperty); }
            set { SetValue(ShowCloseButtonProperty, value); }
        }

        public bool ShowMaxRestoreButtons {
            get { return (bool)GetValue(ShowMaxRestoreButtonsProperty); }
            set { SetValue(ShowMaxRestoreButtonsProperty, value); }
        }

        public bool ShowMinButton {
            get { return (bool)GetValue(ShowMinButtonProperty); }
            set { SetValue(ShowMinButtonProperty, value); }
        }

        public override void OnApplyTemplate() {
            // TODO: Assign windowControlButtons
            WindowControlButtonsPresenter = GetTemplateChild("PART_WindowControlButtons") as ContentPresenter;

            if (WindowControlButtons == null) {
                WindowControlButtons = new WindowControlButtons();
            }

            WindowControlButtons.ParentWindow = this;
        }

        #region System Commands

        private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void OnCanMinWindow(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = ResizeMode != ResizeMode.NoResize;
        }

        private void OnCloseWindow(object target, ExecutedRoutedEventArgs e) {
            SystemCommands.CloseWindow(this);
        }

        private void OnMaxWindow(object target, ExecutedRoutedEventArgs e) {
            SystemCommands.MaximizeWindow(this);
        }

        private void OnMinWindow(object target, ExecutedRoutedEventArgs e) {
            // Hacky solution to get the Min animation
            // Also seems to help with the window overflowing outside the screen when Maxd...
            WindowStyle = WindowStyle.SingleBorderWindow;
            SystemCommands.MinimizeWindow(this);
        }

        private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e) {
            // Hacky solution to get the Min animation
            // Also seems to help with the window overflowing outside the screen when Maxd...
            WindowStyle = WindowStyle.None;
            SystemCommands.RestoreWindow(this);
        }

        #endregion
    }
}
