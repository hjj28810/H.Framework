using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace H.Framework.WPF.Control.Controls
{
    [TemplatePart(Name = "PART_MoveableBorder", Type = typeof(Border))]
    [TemplatePart(Name = "PART_MinimizeButton", Type = typeof(ButtonEx))]
    [TemplatePart(Name = "PART_RestoreButton", Type = typeof(ButtonEx))]
    [TemplatePart(Name = "PART_CloseButton", Type = typeof(ButtonEx))]
    [TemplatePart(Name = "PART_ResizeGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_MainGrid", Type = typeof(Grid))]
    public class WindowEx : Window
    {
        private HwndSource _hwndSource;

        private Border _titleBar;
        private ButtonEx _minimizeButton;
        private ButtonEx _restoreButton;
        private ButtonEx _closeButton;
        private Rect _rcNormal;
        private Grid _resizeGrid;
        private Grid _mainGrid;

        static WindowEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowEx), new FrameworkPropertyMetadata(typeof(WindowEx)));
        }

        public WindowEx()
        {
            PreviewMouseMove += OnPreviewMouseMove;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitializeControl();
        }

        private void InitializeControl()
        {
            _titleBar = GetTemplateChild("PART_MoveableBorder") as Border;
            if (_titleBar != null)
                _titleBar.MouseDown += MoveableRect_MouseDown;

            _minimizeButton = GetTemplateChild("PART_MinimizeButton") as ButtonEx;
            _restoreButton = GetTemplateChild("PART_RestoreButton") as ButtonEx;
            _closeButton = GetTemplateChild("PART_CloseButton") as ButtonEx;
            if (_minimizeButton != null)
                _minimizeButton.Click += MinimizeButton_Click;
            if (_restoreButton != null)
                _restoreButton.Click += RestoreButton_Click;
            if (_closeButton != null)
                _closeButton.Click += CloseButton_Click;
            SetTitleBarCornerRadius(WinCornerRadius);

            _resizeGrid = GetTemplateChild("PART_ResizeGrid") as Grid;
            _mainGrid = GetTemplateChild("PART_MainGrid") as Grid;
            if (_resizeGrid != null && ResizeMode != ResizeMode.NoResize)
            {
                foreach (var element in _resizeGrid.Children)
                {
                    if (element is Rectangle resizeRectangle)
                    {
                        resizeRectangle.PreviewMouseDown += ResizeRectangle_PreviewMouseDown;
                        resizeRectangle.PreviewMouseUp += ResizeRectangle_PreviewMouseUp;
                        resizeRectangle.MouseMove += ResizeRectangle_MouseMove;
                    }
                }
            }
        }

        //private void MoveableRect_MouseMove(object sender, MouseEventArgs e)
        //{
        //    //if (Mouse.LeftButton == MouseButtonState.Pressed)
        //    //{
        //    GetCursorPos(out POINT p);
        //    Trace.WriteLine(p.X);
        //    Trace.WriteLine(p.Y);
        //    Trace.WriteLine(WinState);
        //    //}
        //}

        private void ResizeRectangle_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            _rcNormal = new Rect(Left, Top, Width, Height);//保存下当前位置与大小
        }

        private void MoveableRect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount % 2 == 0)
            {
                if (WinState == 0)
                    WinState = 2;
                else
                    WinState = 0;
            }
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        //public struct POINT
        //{
        //    public int X;
        //    public int Y;

        //    public POINT(int x, int y)
        //    {
        //        X = x;
        //        Y = y;
        //    }
        //}

        private void ResizeRectangle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle rectangle)
            {
                switch (rectangle.Name)
                {
                    case "Top":
                        Cursor = Cursors.SizeNS;
                        ResizeWindow(ResizeDirection.Top);
                        break;

                    case "Bottom":
                        Cursor = Cursors.SizeNS;
                        ResizeWindow(ResizeDirection.Bottom);
                        break;

                    case "Left":
                        Cursor = Cursors.SizeWE;
                        ResizeWindow(ResizeDirection.Left);
                        break;

                    case "Right":
                        Cursor = Cursors.SizeWE;
                        ResizeWindow(ResizeDirection.Right);
                        break;

                    case "TopLeft":
                        Cursor = Cursors.SizeNWSE;
                        ResizeWindow(ResizeDirection.TopLeft);
                        break;

                    case "TopRight":
                        Cursor = Cursors.SizeNESW;
                        ResizeWindow(ResizeDirection.TopRight);
                        break;

                    case "BottomLeft":
                        Cursor = Cursors.SizeNESW;
                        ResizeWindow(ResizeDirection.BottomLeft);
                        break;

                    case "BottomRight":
                        Cursor = Cursors.SizeNWSE;
                        ResizeWindow(ResizeDirection.BottomRight);
                        break;
                }
            }
        }

        private void ResizeRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Rectangle rectangle)
            {
                switch (rectangle.Name)
                {
                    case "Top":
                        Cursor = Cursors.SizeNS;
                        break;

                    case "Bottom":
                        Cursor = Cursors.SizeNS;
                        break;

                    case "Left":
                        Cursor = Cursors.SizeWE;
                        break;

                    case "Right":
                        Cursor = Cursors.SizeWE;
                        break;

                    case "TopLeft":
                        Cursor = Cursors.SizeNWSE;
                        break;

                    case "TopRight":
                        Cursor = Cursors.SizeNESW;
                        break;

                    case "BottomLeft":
                        Cursor = Cursors.SizeNESW;
                        break;

                    case "BottomRight":
                        Cursor = Cursors.SizeNWSE;
                        break;
                }
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            SourceInitialized += MainWindow_SourceInitialized;
            SizeChanged += WindowEx_SizeChanged;
            base.OnInitialized(e);
        }

        private void WindowEx_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ActualHeight > SystemParameters.WorkArea.Height || ActualWidth > SystemParameters.WorkArea.Width)
            {
                WindowState = WindowState.Normal;
                WinState = 2;
            }
            //else
            //{
            //    ToNormal();
            //}
        }

        private void ToMax()
        {
            if (_restoreButton != null)
                _restoreButton.Content = "2";
            if (_resizeGrid != null)
                _resizeGrid.Visibility = Visibility.Collapsed;
            if (_mainGrid != null)
                _mainGrid.Margin = new Thickness(0);

            _rcNormal = new Rect(Left, Top, Width, Height);//保存下当前位置与大小
            Left = 0;//设置位置
            Top = 0;
            var rc = SystemParameters.WorkArea;//获取工作区大小
            Width = rc.Width;
            Height = rc.Height;
        }

        private void ToNormal()
        {
            if (_restoreButton != null)
                _restoreButton.Content = "1";
            if (_resizeGrid != null)
                _resizeGrid.Visibility = Visibility.Visible;
            if (_mainGrid != null)
                _mainGrid.Margin = new Thickness(10);

            Left = _rcNormal.Left;
            Top = _rcNormal.Top;
            //if (_rcNormal.Width != 0 && _rcNormal.Height != 0)
            //{
            Width = _rcNormal.Width;
            Height = _rcNormal.Height;
            //}
        }

        private void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            _hwndSource = (HwndSource)PresentationSource.FromVisual(this);
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
            WinState = 1;
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as ButtonEx;
            if (btn.Content.ToString() == "1")
            {
                //WindowState = WindowState.Maximized;
                WinState = 2;
            }
            else
            {
                // = WindowState.Normal;
                WinState = 0;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                Cursor = Cursors.Arrow;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        //[DllImport("user32.dll")]
        //public static extern bool GetCursorPos(out POINT lpPoint);

        private void ResizeWindow(ResizeDirection direction)
        {
            SendMessage(_hwndSource.Handle, 0x112, (IntPtr)(61440 + direction), IntPtr.Zero);
        }

        public static readonly DependencyProperty TitleBarBGProperty = DependencyProperty.Register("TitleBarBG", typeof(Brush), typeof(WindowEx), new PropertyMetadata(new SolidColorBrush(Colors.White), null));

        /// <summary>
        /// 标题栏背景色
        /// </summary>
        [Description("获取或设置标题栏背景色")]
        [Category("Defined Properties")]
        public Brush TitleBarBG
        {
            get => (Brush)GetValue(TitleBarBGProperty);
            set => SetValue(TitleBarBGProperty, value);
        }

        public static readonly DependencyProperty TitleBarContentProperty = DependencyProperty.Register("TitleBarContent", typeof(object), typeof(WindowEx), new PropertyMetadata(null, null));

        /// <summary>
        /// 标题栏内容
        /// </summary>
        [Description("获取或设置标题栏内容")]
        [Category("Defined Properties")]
        public object TitleBarContent
        {
            get => GetValue(TitleBarContentProperty);
            set => SetValue(TitleBarContentProperty, value);
        }

        public static readonly DependencyProperty WinStateProperty = DependencyProperty.Register("WinState", typeof(int), typeof(WindowEx), new UIPropertyMetadata(0, OnWinStatePropertyChanged));

        /// <summary>
        /// 窗体状态
        /// </summary>
        [Description("获取或设置窗体状态")]
        [Category("Defined Properties")]
        public int WinState
        {
            get => (int)GetValue(WinStateProperty);
            set => SetValue(WinStateProperty, value);
        }

        public static void OnWinStatePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var c = (WindowEx)sender;
            var value = (int)e.NewValue;
            if (value == 2)
                c.ToMax();
            if (value == 0)
                c.ToNormal();
        }

        public static readonly DependencyProperty WinCornerRadiusProperty = DependencyProperty.Register("WinCornerRadius", typeof(CornerRadius), typeof(WindowEx), new UIPropertyMetadata(new CornerRadius(0, 0, 0, 0), OnWinCornerRadiusPropertyChanged));

        /// <summary>
        /// 窗体的圆角
        /// </summary>
        [Description("获取或设置窗体的圆角")]
        [Category("Defined Properties")]
        public CornerRadius WinCornerRadius
        {
            get => (CornerRadius)GetValue(WinCornerRadiusProperty);
            set => SetValue(WinCornerRadiusProperty, value);
        }

        public static void OnWinCornerRadiusPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var c = (WindowEx)sender;
            var value = (CornerRadius)e.NewValue;
            c.SetTitleBarCornerRadius(value);
            //c.RectangleRadius = value.TopLeft;
            c.InnerRadius = new CornerRadius(0, 0, value.BottomRight, value.BottomLeft);
        }

        private void SetTitleBarCornerRadius(CornerRadius cr)
        {
            if (_titleBar != null)
                _titleBar.CornerRadius = new CornerRadius(cr.TopLeft, cr.TopRight, 0, 0);
            if (_closeButton != null && CloseVisibility == Visibility)
            {
                _closeButton.Corner = new CornerRadius(0, cr.TopRight, 0, 0);
                return;
            }
            if (_restoreButton != null && MaxSizeVisibility == Visibility)
            {
                _restoreButton.Corner = new CornerRadius(0, cr.TopRight, 0, 0);
                return;
            }
            if (_minimizeButton != null && MinSizeVisibility == Visibility)
            {
                _minimizeButton.Corner = new CornerRadius(0, cr.TopRight, 0, 0);
                return;
            }
        }

        public static readonly DependencyProperty CloseVisibilityProperty = DependencyProperty.Register("CloseVisibility", typeof(Visibility), typeof(WindowEx), new UIPropertyMetadata(Visibility.Visible, null));

        /// <summary>
        /// 关闭按钮的显示
        /// </summary>
        [Description("获取或设置关闭按钮的显示")]
        [Category("Defined Properties")]
        public Visibility CloseVisibility
        {
            get => (Visibility)GetValue(CloseVisibilityProperty);
            set => SetValue(CloseVisibilityProperty, value);
        }

        public static readonly DependencyProperty MaxSizeVisibilityProperty = DependencyProperty.Register("MaxSizeVisibility", typeof(Visibility), typeof(WindowEx), new UIPropertyMetadata(Visibility.Visible, null));

        /// <summary>
        /// 最大化按钮的显示
        /// </summary>
        [Description("获取或设置最大化按钮的显示")]
        [Category("Defined Properties")]
        public Visibility MaxSizeVisibility
        {
            get => (Visibility)GetValue(MaxSizeVisibilityProperty);
            set => SetValue(MaxSizeVisibilityProperty, value);
        }

        public static readonly DependencyProperty MinSizeVisibilityProperty = DependencyProperty.Register("MinSizeVisibility", typeof(Visibility), typeof(WindowEx), new UIPropertyMetadata(Visibility.Visible, null));

        /// <summary>
        /// 最小化按钮的显示
        /// </summary>
        [Description("获取或设置最小化按钮的显示")]
        [Category("Defined Properties")]
        public Visibility MinSizeVisibility
        {
            get => (Visibility)GetValue(MinSizeVisibilityProperty);
            set => SetValue(MinSizeVisibilityProperty, value);
        }

        public static readonly DependencyProperty WinBorderBrushProperty = DependencyProperty.Register("WinBorderBrush", typeof(Brush), typeof(WindowEx), new UIPropertyMetadata(new SolidColorBrush(Colors.Gray), null));

        /// <summary>
        /// 窗口边框颜色
        /// </summary>
        [Description("获取或设置窗口边框颜色")]
        [Category("Defined Properties")]
        public Brush WinBorderBrush
        {
            get => (Brush)GetValue(WinBorderBrushProperty);
            set => SetValue(WinBorderBrushProperty, value);
        }

        public static readonly DependencyProperty WinBorderThicknessProperty = DependencyProperty.Register("WinBorderThickness", typeof(Thickness), typeof(WindowEx), new UIPropertyMetadata(new Thickness(0, 0, 0, 0), null));

        /// <summary>
        /// 窗口边框
        /// </summary>
        [Description("获取或设置窗口边框")]
        [Category("Defined Properties")]
        public Thickness WinBorderThickness
        {
            get => (Thickness)GetValue(WinBorderThicknessProperty);
            set => SetValue(WinBorderThicknessProperty, value);
        }

        public static readonly DependencyProperty ShowLoadingProperty = DependencyProperty.Register("ShowLoading", typeof(bool), typeof(WindowEx), new UIPropertyMetadata(false, null));

        /// <summary>
        /// 是否显示Loading
        /// </summary>
        [Description("获取或设置是否显示Loading")]
        [Category("Defined Properties")]
        public bool ShowLoading
        {
            get => (bool)GetValue(ShowLoadingProperty);
            set => SetValue(ShowLoadingProperty, value);
        }

        public static readonly DependencyProperty TitleBorderBrushProperty = DependencyProperty.Register("TitleBorderBrush", typeof(Brush), typeof(WindowEx), new UIPropertyMetadata(new SolidColorBrush(Colors.Gray), null));

        /// <summary>
        /// 窗口Title边框颜色
        /// </summary>
        [Description("获取或设置窗口边框颜色")]
        [Category("Defined Properties")]
        public Brush TitleBorderBrush
        {
            get => (Brush)GetValue(TitleBorderBrushProperty);
            set => SetValue(TitleBorderBrushProperty, value);
        }

        public static readonly DependencyProperty TitleBorderThicknessProperty = DependencyProperty.Register("TitleBorderThickness", typeof(Thickness), typeof(WindowEx), new UIPropertyMetadata(new Thickness(0, 0, 0, 0), null));

        /// <summary>
        /// 窗口Title边框
        /// </summary>
        [Description("获取或设置窗口边框")]
        [Category("Defined Properties")]
        public Thickness TitleBorderThickness
        {
            get => (Thickness)GetValue(TitleBorderThicknessProperty);
            set => SetValue(TitleBorderThicknessProperty, value);
        }

        public static readonly DependencyProperty WinButtonSizeProperty = DependencyProperty.Register("WinButtonSize", typeof(double), typeof(WindowEx), new UIPropertyMetadata(12.0, null));

        /// <summary>
        /// 按钮大小
        /// </summary>
        [Description("获取或设置按钮大小")]
        [Category("Defined Properties")]
        public double WinButtonSize
        {
            get => (double)GetValue(WinButtonSizeProperty);
            set => SetValue(WinButtonSizeProperty, value);
        }

        public static readonly DependencyProperty WinButtonFGProperty = DependencyProperty.Register("WinButtonFG", typeof(Brush), typeof(WindowEx), new UIPropertyMetadata(new SolidColorBrush(Colors.Black), null));

        /// <summary>
        /// 按钮前景色
        /// </summary>
        [Description("获取或设置按钮前景色")]
        [Category("Defined Properties")]
        public Brush WinButtonFG
        {
            get => (Brush)GetValue(WinButtonFGProperty);
            set => SetValue(WinButtonFGProperty, value);
        }

        //public static readonly DependencyProperty RectangleRadiusProperty = DependencyProperty.Register("RectangleRadius", typeof(double), typeof(WindowEx), new UIPropertyMetadata(0.0, null));

        ///// <summary>
        ///// RectangleRadius
        ///// </summary>
        //[Description("获取或设置RectangleRadius")]
        //[Category("Defined Properties")]
        //public double RectangleRadius
        //{
        //    get => (double)GetValue(RectangleRadiusProperty);
        //    set => SetValue(RectangleRadiusProperty, value);
        //}

        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register("InnerRadius", typeof(CornerRadius), typeof(WindowEx), new UIPropertyMetadata(new CornerRadius(0, 0, 0, 0), null));

        /// <summary>
        /// InnerRadius
        /// </summary>
        [Description("获取或设置InnerRadius")]
        [Category("Defined Properties")]
        public CornerRadius InnerRadius
        {
            get => (CornerRadius)GetValue(InnerRadiusProperty);
            set => SetValue(InnerRadiusProperty, value);
        }

        public static readonly DependencyProperty HideTitleBarProperty = DependencyProperty.Register("HideTitleBar", typeof(bool), typeof(WindowEx), new UIPropertyMetadata(false, null));

        /// <summary>
        /// 是否隐藏TitleBar
        /// </summary>
        [Description("获取或设置是否隐藏TitleBar")]
        [Category("Defined Properties")]
        public bool HideTitleBar
        {
            get => (bool)GetValue(HideTitleBarProperty);
            set => SetValue(HideTitleBarProperty, value);
        }
    }

    public enum ResizeDirection
    {
        Left = 1,
        Right = 2,
        Top = 3,
        TopLeft = 4,
        TopRight = 5,
        Bottom = 6,
        BottomLeft = 7,
        BottomRight = 8,
    }
}