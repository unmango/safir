// <copyright file="SafirWindow.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.ControlLibrary.Controls
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Interop;
    using System.Windows.Media;
    using MahApps.Metro.Controls;
    using Native;

    [TemplatePart(Name = PART_Icon, Type = typeof(UIElement))]
    [TemplatePart(Name = PART_TitleBar, Type = typeof(UIElement))]
    [TemplatePart(Name = PART_WindowTitleBackground, Type = typeof(UIElement))]
    [TemplatePart(Name = PART_WindowTitleThumb, Type = typeof(Thumb))]
    [TemplatePart(Name = PART_WindowControlButtons, Type = typeof(WindowControlButtons))]
    public class SafirWindow : Window
    {
        #region Dependency Properties

        public static readonly DependencyProperty ShowIconOnTitleBarProperty = DependencyProperty.Register(
            "ShowIconOnTitleBar",
            typeof(bool),
            typeof(SafirWindow),
            new PropertyMetadata(true, OnShowIconOnTitleBarPropertyChangedCallback));

        public static readonly DependencyProperty IconEdgeModeProperty = DependencyProperty.Register(
            "IconEdgeMode",
            typeof(EdgeMode),
            typeof(SafirWindow),
            new PropertyMetadata(EdgeMode.Aliased));

        public static readonly DependencyProperty IconBitmapScalingModeProperty = DependencyProperty.Register(
            "IconBitmapScalingMode",
            typeof(BitmapScalingMode),
            typeof(SafirWindow),
            new PropertyMetadata(BitmapScalingMode.HighQuality));

        public static readonly DependencyProperty IconScalingModeProperty = DependencyProperty.Register(
            "IconScalingMode",
            typeof(MultiFrameImageMode),
            typeof(SafirWindow),
            new FrameworkPropertyMetadata(
                MultiFrameImageMode.ScaleDownLargerFrame,
                FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.Register(
            "IconTemplate",
            typeof(DataTemplate),
            typeof(SafirWindow),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register(
            "ShowCloseButton",
            typeof(bool),
            typeof(SafirWindow),
            new PropertyMetadata(true));

        public static readonly DependencyProperty ShowMaxRestoreButtonsProperty = DependencyProperty.Register(
            "ShowMaxRestoreButtons",
            typeof(bool),
            typeof(SafirWindow),
            new PropertyMetadata(true));

        public static readonly DependencyProperty ShowMinButtonProperty = DependencyProperty.Register(
            "ShowMinButton",
            typeof(bool),
            typeof(SafirWindow),
            new PropertyMetadata(true));

        public static readonly DependencyProperty UseNoneWindowStyleProperty = DependencyProperty.Register(
            "UseNoneWindowStyle",
            typeof(bool),
            typeof(SafirWindow),
            new PropertyMetadata(false, OnUseNoneWindowStylePropertyChangedCallback));

        public static readonly DependencyProperty WindowControlButtonsProperty = DependencyProperty.Register(
            "WindowControlButtons",
            typeof(WindowControlButtons),
            typeof(SafirWindow),
            new PropertyMetadata(null, UpdateLogicalChildren));

        public static readonly DependencyProperty ShowTitleBarProperty = DependencyProperty.Register(
            "ShowTitleBar",
            typeof(bool),
            typeof(SafirWindow),
            new PropertyMetadata(true, OnShowTitleBarPropertyChangedCallback, OnShowTitleBarCoerceValueCallback));

        public static readonly DependencyProperty TitleBarHeightProperty = DependencyProperty.Register(
            "TitleBarHeight",
            typeof(int),
            typeof(SafirWindow),
            new PropertyMetadata(30, TitleBarHeightPropertyChangedCallback));

        public static readonly DependencyProperty TitleForegroundProperty =
            DependencyProperty.Register("TitleForeground", typeof(Brush), typeof(SafirWindow));

        public static readonly DependencyProperty TitleAlignmentProperty = DependencyProperty.Register(
            "TitleAlignment",
            typeof(HorizontalAlignment),
            typeof(SafirWindow),
            new PropertyMetadata(HorizontalAlignment.Stretch, TitleAlignmentPropertyChangedCallback));

        public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register(
            "TitleTemplate",
            typeof(DataTemplate),
            typeof(SafirWindow),
            new PropertyMetadata(null));

        public static readonly DependencyProperty WindowTitleBrushProperty = DependencyProperty.Register(
            "WindowTitleBrush",
            typeof(Brush),
            typeof(SafirWindow),
            new PropertyMetadata(Brushes.Transparent));

        public static readonly DependencyProperty NonActiveWindowTitleBrushProperty = DependencyProperty.Register(
            "NonActiveWindowTitleBrush",
            typeof(Brush),
            typeof(SafirWindow),
            new PropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty ShowSystemMenuOnRightClickProperty = DependencyProperty.Register(
            "ShowSystemMenuOnRightClick",
            typeof(bool),
            typeof(SafirWindow),
            new PropertyMetadata(true));

        public static readonly DependencyProperty IsWindowDraggableProperty = DependencyProperty.Register(
            "IsWindowDraggable",
            typeof(bool),
            typeof(SafirWindow),
            new PropertyMetadata(true));

        #endregion

        private const string PART_Icon = "PART_Icon";
        private const string PART_TitleBar = "PART_TitleBar";
        private const string PART_WindowTitleBackground = "PART_WindowTitleBackground";
        private const string PART_WindowTitleThumb = "PART_WindowTitleThumb";
        private const string PART_WindowControlButtons = "PART_WindowControlButtons";
        private const string PART_Content = "PART_Content";

        private FrameworkElement _icon;
        private UIElement _titleBar;
        private UIElement _titleBarBackground;
        private Thumb _windowTitleThumb;
        private ContentPresenter _windowControlButtonsPresenter;

        static SafirWindow() {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(SafirWindow),
                new FrameworkPropertyMetadata(typeof(SafirWindow)));
        }

        #region Properties

        public bool ShowIconOnTitleBar {
            get => (bool)GetValue(ShowIconOnTitleBarProperty);
            set => SetValue(ShowIconOnTitleBarProperty, value);
        }

        public EdgeMode IconEdgeMode {
            get => (EdgeMode)GetValue(IconEdgeModeProperty);
            set => SetValue(IconEdgeModeProperty, value);
        }

        public BitmapScalingMode IconBitmapScalingMode {
            get => (BitmapScalingMode)GetValue(IconBitmapScalingModeProperty);
            set => SetValue(IconBitmapScalingModeProperty, value);
        }

        public MultiFrameImageMode IconScalingMode {
            get => (MultiFrameImageMode)GetValue(IconScalingModeProperty);
            set => SetValue(IconScalingModeProperty, value);
        }

        public DataTemplate IconTemplate {
            get => (DataTemplate)GetValue(IconTemplateProperty);
            set => SetValue(IconTemplateProperty, value);
        }

        public bool ShowTitleBar {
            get => (bool)GetValue(ShowTitleBarProperty);
            set => SetValue(ShowTitleBarProperty, value);
        }

        public int TitleBarHeight {
            get => (int)GetValue(TitleBarHeightProperty);
            set => SetValue(TitleBarHeightProperty, value);
        }

        public Brush WindowTitleBrush {
            get => (Brush)GetValue(WindowTitleBrushProperty);
            set => SetValue(WindowTitleBrushProperty, value);
        }

        public Brush NonActiveWindowTitleBrush {
            get => (Brush)GetValue(NonActiveWindowTitleBrushProperty);
            set => SetValue(NonActiveWindowTitleBrushProperty, value);
        }

        public Brush TitleForeground {
            get => (Brush)GetValue(TitleForegroundProperty);
            set => SetValue(TitleForegroundProperty, value);
        }

        public HorizontalAlignment TitleAlignment {
            get => (HorizontalAlignment)GetValue(TitleAlignmentProperty);
            set => SetValue(TitleAlignmentProperty, value);
        }

        public DataTemplate TitleTemplate {
            get => (DataTemplate)GetValue(TitleTemplateProperty);
            set => SetValue(TitleTemplateProperty, value);
        }

        public WindowControlButtons WindowControlButtons {
            get => (WindowControlButtons)GetValue(WindowControlButtonsProperty);
            set => SetValue(WindowControlButtonsProperty, value);
        }

        public bool UseNoneWindowStyle {
            get => (bool)GetValue(UseNoneWindowStyleProperty);
            set => SetValue(UseNoneWindowStyleProperty, value);
        }

        public bool ShowCloseButton {
            get => (bool)GetValue(ShowCloseButtonProperty);
            set => SetValue(ShowCloseButtonProperty, value);
        }

        public bool ShowMaxRestoreButtons {
            get => (bool)GetValue(ShowMaxRestoreButtonsProperty);
            set => SetValue(ShowMaxRestoreButtonsProperty, value);
        }

        public bool ShowMinButton {
            get => (bool)GetValue(ShowMinButtonProperty);
            set => SetValue(ShowMinButtonProperty, value);
        }

        public bool ShowSystemMenuOnRightClick {
            get => (bool)GetValue(ShowSystemMenuOnRightClickProperty);
            set => SetValue(ShowSystemMenuOnRightClickProperty, value);
        }

        public bool IsWindowDraggable {
            get => (bool)GetValue(IsWindowDraggableProperty);
            set => SetValue(IsWindowDraggableProperty, value);
        }

        protected override IEnumerator LogicalChildren {
            get {
                var children = new ArrayList {
                    Content
                };
                if (WindowControlButtons != null) {
                    children.Add(WindowControlButtons);
                }

                return children.GetEnumerator();
            }
        }

        protected IntPtr CriticalHandle {
            get {
                var value = typeof(Window)
                    .GetProperty("CriticalHandle", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(this, new object[0]);
                return (IntPtr)value;
            }
        }

        #endregion

        public override void OnApplyTemplate() {
            base.OnApplyTemplate();

            _windowControlButtonsPresenter = GetTemplateChild(PART_WindowControlButtons) as ContentPresenter;

            if (WindowControlButtons == null) {
                WindowControlButtons = new WindowControlButtons();
            }

            WindowControlButtons.ParentWindow = this;

            _icon = GetTemplateChild(PART_Icon) as FrameworkElement;
            _titleBar = GetTemplateChild(PART_TitleBar) as UIElement;
            _titleBarBackground = GetTemplateChild(PART_WindowTitleBackground) as UIElement;
            _windowTitleThumb = GetTemplateChild(PART_WindowTitleThumb) as Thumb;

            SetVisibilityForAllTitleElements(TitleBarHeight > 0);

            if (GetTemplateChild(PART_Content) is ContentControl contentControl) {
                // contentControl.TransitionCompleted += (sender, args) => RaiseEvent(new RoutedEventArgs(WindowTransitionCompletedEvent));
            }
        }

        internal static void DoWindowTitleThumbOnPreviewMouseLeftButtonUp(
            SafirWindow window, MouseButtonEventArgs mouseButtonEventArgs) {
            if (mouseButtonEventArgs.Source == mouseButtonEventArgs.OriginalSource) {
                Mouse.Capture(null);
            }
        }

        internal static void DoWindowTitleThumbMoveOnDragDelta(IMetroThumb thumb, SafirWindow window,
                                                               DragDeltaEventArgs dragDeltaEventArgs) {
            if (thumb == null) {
                throw new ArgumentNullException(nameof(thumb));
            }

            if (window == null) {
                throw new ArgumentNullException(nameof(window));
            }

            // drag only if IsWindowDraggable is set to true
            if (!window.IsWindowDraggable ||
                (!(Math.Abs(dragDeltaEventArgs.HorizontalChange) > 2) &&
                 !(Math.Abs(dragDeltaEventArgs.VerticalChange) > 2))) {
                return;
            }

            // tage from DragMove internal code
            window.VerifyAccess();

            // var cursorPos = WinApiHelper.GetPhysicalCursorPos();

            // if the window is maximized dragging is only allowed on title bar (also if not visible)
            var windowIsMaximized = window.WindowState == WindowState.Maximized;
            var isMouseOnTitleBar = Mouse.GetPosition(thumb).Y <= window.TitleBarHeight && window.TitleBarHeight > 0;
            if (!isMouseOnTitleBar && windowIsMaximized) {
                return;
            }

            // for the touch usage
            UnsafeNativeMethods.ReleaseCapture();

            if (windowIsMaximized) {
                // var cursorXPos = cursorPos.x;
                void WindowOnStateChanged(object sender, EventArgs args) {
                    // window.Top = 2;
                    // window.Left = Math.Max(cursorXPos - window.RestoreBounds.Width / 2, 0);
                    window.StateChanged -= WindowOnStateChanged;
                    if (window.WindowState == WindowState.Normal) {
                        Mouse.Capture(thumb, CaptureMode.Element);
                    }
                }

                window.StateChanged += WindowOnStateChanged;
            }

            var criticalHandle = window.CriticalHandle;

            // DragMove works too
            // window.DragMove();
            // instead this 2 lines
            Standard.NativeMethods.SendMessage(
                criticalHandle,
                Standard.WM.SYSCOMMAND,
                (IntPtr)Standard.SC.MOUSEMOVE,
                IntPtr.Zero);
            Standard.NativeMethods.SendMessage(criticalHandle, Standard.WM.LBUTTONUP, IntPtr.Zero, IntPtr.Zero);
        }

        internal static void DoWindowTitleThumbChangeWindowStateOnMouseDoubleClick(
            SafirWindow window, MouseButtonEventArgs mouseButtonEventArgs) {
            // restore/maximize only with left button
            if (mouseButtonEventArgs.ChangedButton != MouseButton.Left) return;
            // we can maximize or restore the window if the title bar height is set (also if title bar is hidden)
            var canResize = window.ResizeMode == ResizeMode.CanResizeWithGrip ||
                            window.ResizeMode == ResizeMode.CanResize;
            var mousePos = Mouse.GetPosition(window);
            var isMouseOnTitleBar = mousePos.Y <= window.TitleBarHeight && window.TitleBarHeight > 0;
            if (!canResize || !isMouseOnTitleBar) return;
            if (window.WindowState == WindowState.Normal) {
                Microsoft.Windows.Shell.SystemCommands.MaximizeWindow(window);
            } else {
                Microsoft.Windows.Shell.SystemCommands.RestoreWindow(window);
            }

            mouseButtonEventArgs.Handled = true;
        }

        internal static void DoWindowTitleThumbSystemMenuOnMouseRightButtonUp(
            SafirWindow window, MouseButtonEventArgs e) {
            if (!window.ShowSystemMenuOnRightClick) return;
            // show menu only if mouse pos is on title bar or if we have a window with none style and no title bar
            var mousePos = e.GetPosition(window);
            if ((mousePos.Y <= window.TitleBarHeight && window.TitleBarHeight > 0) ||
                (window.UseNoneWindowStyle && window.TitleBarHeight <= 0)) {
                ShowSystemMenuPhysicalCoordinates(window, window.PointToScreen(mousePos));
            }
        }

        private static void OnShowIconOnTitleBarPropertyChangedCallback(
            DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (e.NewValue == e.OldValue) return;
            var window = (SafirWindow)d;
            window.SetVisibilityForIcon();
        }

        private static object OnShowTitleBarCoerceValueCallback(DependencyObject d, object baseValue) {
            var window = (SafirWindow)d;
            return !window.UseNoneWindowStyle;
        }

        private static void OnShowTitleBarPropertyChangedCallback(DependencyObject d,
                                                                  DependencyPropertyChangedEventArgs e) {
            if (e.NewValue == e.OldValue) return;
            var window = (SafirWindow)d;
            window.SetVisibilityForAllTitleElements((bool)e.NewValue);
        }

        private static void TitleBarHeightPropertyChangedCallback(DependencyObject d,
                                                                  DependencyPropertyChangedEventArgs e) {
            if (e.NewValue == e.OldValue) return;
            var window = (SafirWindow)d;
            window.SetVisibilityForAllTitleElements((int)e.NewValue > 0);
        }

        private static void TitleAlignmentPropertyChangedCallback(DependencyObject d,
                                                                  DependencyPropertyChangedEventArgs e) {
            if (!(d is SafirWindow window)) return;
            window.SizeChanged -= window.SafirWindow_SizeChanged;
            if (e.NewValue is HorizontalAlignment alignment && alignment == HorizontalAlignment.Center) {
                window.SizeChanged += window.SafirWindow_SizeChanged;
            }
        }

        private static void OnUseNoneWindowStylePropertyChangedCallback(
            DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (e.NewValue == e.OldValue) return;
            // if UseNoneWindowStyle = true no title bar should be shown
            var useNoneWindowStyle = (bool)e.NewValue;
            var window = (SafirWindow)d;

            window.ToggleNoneWindowStyle(useNoneWindowStyle, window.ShowTitleBar);
        }

        private static void ShowSystemMenuPhysicalCoordinates(Window window, Point physicalScreenLocation) {
            if (window == null) {
                return;
            }

            var hwnd = new WindowInteropHelper(window).Handle;
            if (hwnd == IntPtr.Zero || !UnsafeNativeMethods.IsWindow(hwnd)) return;

            var hmenu = UnsafeNativeMethods.GetSystemMenu(hwnd, false);

            var cmd = UnsafeNativeMethods.TrackPopupMenuEx(
                hmenu,
                Constants.TPM_LEFTBUTTON | Constants.TPM_RETURNCMD,
                (int)physicalScreenLocation.X,
                (int)physicalScreenLocation.Y,
                hwnd,
                IntPtr.Zero);
            if (cmd != 0) {
                UnsafeNativeMethods.PostMessage(hwnd, Constants.SYSCOMMAND, new IntPtr(cmd), IntPtr.Zero);
            }
        }

        private static void UpdateLogicalChildren(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var window = d as SafirWindow;
            if (window == null) {
                return;
            }

            if (e.OldValue is FrameworkElement oldValue) {
                window.RemoveLogicalChild(oldValue);
            }

            if (!(e.NewValue is FrameworkElement newValue)) return;
            window.AddLogicalChild(newValue);
            newValue.DataContext = window.DataContext;
        }

        private void SetVisibilityForIcon() {
            if (_icon != null) {
                _icon.Visibility = ShowIconOnTitleBar && ShowTitleBar ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void SetVisibilityForAllTitleElements(bool visible) {
            SetVisibilityForIcon();

            var newVisibility = visible && ShowTitleBar ? Visibility.Visible : Visibility.Collapsed;
            if (_titleBar != null) {
                _titleBar.Visibility = newVisibility;
            }

            if (_titleBarBackground != null) {
                _titleBarBackground.Visibility = newVisibility;
            }

            SetWindowEvents();
        }

        private void ClearWindowEvents() {
            // clear all event handlers first:
            if (_windowTitleThumb != null) {
                _windowTitleThumb.PreviewMouseLeftButtonUp -= WindowTitleThumbOnPreviewMouseLeftButtonUp;
                _windowTitleThumb.DragDelta -= WindowTitleThumbMoveOnDragDelta;
                _windowTitleThumb.MouseDoubleClick -= WindowTitleThumbChangeWindowStateOnMouseDoubleClick;
                _windowTitleThumb.MouseRightButtonUp -= WindowTitleThumbSystemMenuOnMouseRightButtonUp;
            }

            if (_titleBar is IMetroThumb thumbContentControl) {
                thumbContentControl.PreviewMouseLeftButtonUp -= WindowTitleThumbOnPreviewMouseLeftButtonUp;
                thumbContentControl.DragDelta -= WindowTitleThumbMoveOnDragDelta;
                thumbContentControl.MouseDoubleClick -= WindowTitleThumbChangeWindowStateOnMouseDoubleClick;
                thumbContentControl.MouseRightButtonUp -= WindowTitleThumbSystemMenuOnMouseRightButtonUp;
            }

            // if (Icon != null) {
            //    Icon.MouseDown -= IconMouseDown;
            // }
            SizeChanged -= SafirWindow_SizeChanged;
        }

        private void SetWindowEvents() {
            // clear all event handlers first
            ClearWindowEvents();

            // set mouse down/up for icon
            // if (Icon != null && Icon.Visibility == Visibility.Visible) {
            //    Icon.MouseDown += IconMouseDown;
            // }
            if (_windowTitleThumb != null) {
                _windowTitleThumb.PreviewMouseLeftButtonUp += WindowTitleThumbOnPreviewMouseLeftButtonUp;
                _windowTitleThumb.DragDelta += WindowTitleThumbMoveOnDragDelta;
                _windowTitleThumb.MouseDoubleClick += WindowTitleThumbChangeWindowStateOnMouseDoubleClick;
                _windowTitleThumb.MouseRightButtonUp += WindowTitleThumbSystemMenuOnMouseRightButtonUp;
            }

            if (_titleBar is IMetroThumb thumbContentControl) {
                thumbContentControl.PreviewMouseLeftButtonUp += WindowTitleThumbOnPreviewMouseLeftButtonUp;
                thumbContentControl.DragDelta += WindowTitleThumbMoveOnDragDelta;
                thumbContentControl.MouseDoubleClick += WindowTitleThumbChangeWindowStateOnMouseDoubleClick;
                thumbContentControl.MouseRightButtonUp += WindowTitleThumbSystemMenuOnMouseRightButtonUp;
            }

            // handle size if we have a Grid for the title (e.g. clean window have a centered title)
            // if (titleBar != null && titleBar.GetType() == typeof(Grid))
            if (_titleBar != null && TitleAlignment == HorizontalAlignment.Center) {
                SizeChanged += SafirWindow_SizeChanged;
            }
        }

        private void SafirWindow_SizeChanged(object sender, RoutedEventArgs e) {
            // this all works only for centered title
            if (TitleAlignment != HorizontalAlignment.Center) {
                return;
            }

            // Half of this SafirWindow
            var halfDistance = ActualWidth / 2;

            // Distance between center and left/right
            var distanceToCenter = _titleBar.DesiredSize.Width / 2;

            // Distance between right edge from LeftWindowCommands to left window side
            var distanceFromLeft = _icon.ActualWidth; // + this.LeftWindowCommands.ActualWidth;

            // Distance between left edge from RightWindowCommands to right window side
            var distanceFromRight = WindowControlButtons.ActualWidth; // + this.RightWindowCommands.ActualWidth;
            // Margin
            const double horizontalMargin = 5.0;

            var dLeft = distanceFromLeft + distanceToCenter + horizontalMargin;
            var dRight = distanceFromRight + distanceToCenter + horizontalMargin;
            if ((dLeft < halfDistance) && (dRight < halfDistance)) {
                Grid.SetColumn(_titleBar, 0);
                Grid.SetColumnSpan(_titleBar, 5);
            } else {
                Grid.SetColumn(_titleBar, 2);
                Grid.SetColumnSpan(_titleBar, 1);
            }
        }

        private void WindowTitleThumbOnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            DoWindowTitleThumbOnPreviewMouseLeftButtonUp(this, e);
        }

        private void WindowTitleThumbMoveOnDragDelta(object sender, DragDeltaEventArgs dragDeltaEventArgs) {
            DoWindowTitleThumbMoveOnDragDelta(sender as IMetroThumb, this, dragDeltaEventArgs);
        }

        private void WindowTitleThumbChangeWindowStateOnMouseDoubleClick(
            object sender, MouseButtonEventArgs mouseButtonEventArgs) {
            DoWindowTitleThumbChangeWindowStateOnMouseDoubleClick(this, mouseButtonEventArgs);
        }

        private void WindowTitleThumbSystemMenuOnMouseRightButtonUp(object sender, MouseButtonEventArgs e) {
            DoWindowTitleThumbSystemMenuOnMouseRightButtonUp(this, e);
        }

        private void ToggleNoneWindowStyle(bool useNoneWindowStyle, bool isTitleBarVisible) {
            // UseNoneWindowStyle means no title bar, window commands or min, max, close buttons
            SetCurrentValue(ShowTitleBarProperty, !useNoneWindowStyle && isTitleBarVisible);
        }
    }
}
