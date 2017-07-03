using System;
using System.Collections.Generic;
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

namespace Safir.Controls
{
    public class SafirWindow : Window
    {
        static SafirWindow() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SafirWindow), new FrameworkPropertyMetadata(typeof(SafirWindow)));
        }

        #region Dependency Properties

        public static readonly DependencyProperty TitleBarProperty =
            DependencyProperty.Register(
                "TitleBar",
                typeof(ContentControl),
                typeof(Window));

        #endregion

        #region Properties

        public ContentControl TitleBar {
            get {
                return (ContentControl)GetValue(TitleBarProperty);
            }
            set {
                SetValue(TitleBarProperty, value);
            }
        }

        #endregion

        #region Commands

        private void _OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void _OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = ResizeMode != ResizeMode.NoResize;
        }

        private void _OnCloseWindow(object target, ExecutedRoutedEventArgs e) {
            SystemCommands.CloseWindow(this);
        }

        private void _OnMaximizeWindow(object target, ExecutedRoutedEventArgs e) {
            SystemCommands.MaximizeWindow(this);
        }

        private void _OnMinimizeWindow(object target, ExecutedRoutedEventArgs e) {
            // Hacky solution to get the minimize animation
            // Also seems to help with the window overflowing outside the screen when maximized...
            WindowStyle = WindowStyle.SingleBorderWindow;
            SystemCommands.MinimizeWindow(this);
        }

        private void _OnRestoreWindow(object target, ExecutedRoutedEventArgs e) {
            // Hacky solution to get the minimize animation
            // Also seems to help with the window overflowing outside the screen when maximized...
            WindowStyle = WindowStyle.None;
            SystemCommands.RestoreWindow(this);
        }

        #endregion
    }
}
