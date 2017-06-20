using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Safir.Views
{
    public class DefaultWindow : Window
    {
        public DefaultWindow() {
            Style = Application.Current.Resources["WindowStyle"] as Style;
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnCloseWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, OnMaximizeWindow, OnCanResizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, OnMinimizeWindow, OnCanMinimizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, OnRestoreWindow, OnCanResizeWindow));
            //MinimizeButtonVisible = true;
            //RestoreDownButtonVisible = true;
            //MaximizeButtonVisible = true;
            //CloseButtonVisible = true;
        }

        public DependencyObject TitleBar { get; set; }

        //public bool MinimizeButtonVisible { get; set; }
        //public bool MaximizeButtonVisible { get; set; }
        //public bool RestoreDownButtonVisible { get; set; }
        //public bool CloseButtonVisible { get; set; }

        private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = ResizeMode != ResizeMode.NoResize;
        }

        private void OnCloseWindow(object target, ExecutedRoutedEventArgs e) {
            SystemCommands.CloseWindow(this);
        }

        private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e) {
            SystemCommands.MaximizeWindow(this);
        }

        private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e) {
            // Hacky solution to get the minimize animation
            // Also seems to help with the window overflowing outside the screen when maximized...
            WindowStyle = WindowStyle.SingleBorderWindow;
            SystemCommands.MinimizeWindow(this);
        }

        private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e) {
            // Hacky solution to get the minimize animation
            // Also seems to help with the window overflowing outside the screen when maximized...
            WindowStyle = WindowStyle.None;
            SystemCommands.RestoreWindow(this);
        }
    }
}
