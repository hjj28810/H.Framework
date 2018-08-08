﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;

namespace H.Framework.WPF.Control.Controls
{
    [TemplatePart(Name = "PART_MoveableBorder", Type = typeof(Border))]
    [TemplatePart(Name = "PART_MinimizeButton", Type = typeof(ButtonEx))]
    [TemplatePart(Name = "PART_RestoreButton", Type = typeof(ButtonEx))]
    [TemplatePart(Name = "PART_CloseButton", Type = typeof(ButtonEx))]
    [TemplatePart(Name = "PART_ResizeGrid", Type = typeof(Grid))]
    public class WindowEx : Window
    {
        static WindowEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowEx), new FrameworkPropertyMetadata(typeof(WindowEx)));
        }

        public WindowEx()
        {
            PreviewMouseMove += OnPreviewMouseMove;
        }

        private HwndSource _hwndSource;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitializeControl();
        }

        private Border _titleBar;
        private ButtonEx _minimizeButton;
        private ButtonEx _restoreButton;
        private ButtonEx _closeButton;

        private void InitializeControl()
        {
            _titleBar = GetTemplateChild("PART_MoveableBorder") as Border;
            _titleBar.PreviewMouseDown += MoveableRect_PreviewMouseDown;

            _minimizeButton = GetTemplateChild("PART_MinimizeButton") as ButtonEx;
            _restoreButton = GetTemplateChild("PART_RestoreButton") as ButtonEx;
            _closeButton = GetTemplateChild("PART_CloseButton") as ButtonEx;
            _minimizeButton.Click += MinimizeButton_Click;
            _restoreButton.Click += RestoreButton_Click;
            _closeButton.Click += CloseButton_Click;
            SetTitleBarCornerRadius(WinCornerRadius);

            var resizeGrid = GetTemplateChild("PART_ResizeGrid") as Grid;
            if (resizeGrid != null)
            {
                foreach (var element in resizeGrid.Children)
                {
                    var resizeRectangle = element as Rectangle;
                    if (resizeRectangle != null)
                    {
                        resizeRectangle.PreviewMouseDown += ResizeRectangle_PreviewMouseDown;
                        resizeRectangle.MouseMove += ResizeRectangle_MouseMove;
                    }
                }
            }
        }

        private void MoveableRect_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void ResizeRectangle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rectangle = sender as Rectangle;

            if (rectangle != null)
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

                    default:
                        break;
                }
            }
        }

        private void ResizeRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            Rectangle rectangle = sender as Rectangle;

            if (rectangle != null)
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

                    default:
                        break;
                }
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            SourceInitialized += MainWindow_SourceInitialized;

            base.OnInitialized(e);
        }

        private void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            _hwndSource = (HwndSource)PresentationSource.FromVisual(this);
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
                Cursor = Cursors.Arrow;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);

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
            get { return (Brush)GetValue(TitleBarBGProperty); }
            set { SetValue(TitleBarBGProperty, value); }
        }

        public static readonly DependencyProperty TitleBarContentProperty = DependencyProperty.Register("TitleBarContent", typeof(object), typeof(WindowEx), new PropertyMetadata(null, null));

        /// <summary>
        /// 标题栏内容
        /// </summary>
        [Description("获取或设置标题栏内容")]
        [Category("Defined Properties")]
        public object TitleBarContent
        {
            get { return GetValue(TitleBarContentProperty); }
            set { SetValue(TitleBarContentProperty, value); }
        }

        public static readonly DependencyProperty WinCornerRadiusProperty = DependencyProperty.Register("WinCornerRadius", typeof(CornerRadius), typeof(WindowEx), new UIPropertyMetadata(new CornerRadius(0, 0, 0, 0), OnWinCornerRadiusPropertyChanged));

        /// <summary>
        /// 窗体的圆角
        /// </summary>
        [Description("获取或设置窗体的圆角")]
        [Category("Defined Properties")]
        public CornerRadius WinCornerRadius
        {
            get { return (CornerRadius)GetValue(WinCornerRadiusProperty); }
            set { SetValue(WinCornerRadiusProperty, value); }
        }

        public static void OnWinCornerRadiusPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var c = (WindowEx)sender;
            var value = (CornerRadius)e.NewValue;
            c.SetTitleBarCornerRadius(value);
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
            get { return (Visibility)GetValue(CloseVisibilityProperty); }
            set { SetValue(CloseVisibilityProperty, value); }
        }

        public static readonly DependencyProperty MaxSizeVisibilityProperty = DependencyProperty.Register("MaxSizeVisibility", typeof(Visibility), typeof(WindowEx), new UIPropertyMetadata(Visibility.Visible, null));

        /// <summary>
        /// 最大化按钮的显示
        /// </summary>
        [Description("获取或设置最大化按钮的显示")]
        [Category("Defined Properties")]
        public Visibility MaxSizeVisibility
        {
            get { return (Visibility)GetValue(MaxSizeVisibilityProperty); }
            set { SetValue(MaxSizeVisibilityProperty, value); }
        }

        public static readonly DependencyProperty MinSizeVisibilityProperty = DependencyProperty.Register("MinSizeVisibility", typeof(Visibility), typeof(WindowEx), new UIPropertyMetadata(Visibility.Visible, null));

        /// <summary>
        /// 最小化按钮的显示
        /// </summary>
        [Description("获取或设置最小化按钮的显示")]
        [Category("Defined Properties")]
        public Visibility MinSizeVisibility
        {
            get { return (Visibility)GetValue(MinSizeVisibilityProperty); }
            set { SetValue(MinSizeVisibilityProperty, value); }
        }

        public static readonly DependencyProperty WinBorderBrushProperty = DependencyProperty.Register("WinBorderBrush", typeof(Brush), typeof(WindowEx), new UIPropertyMetadata(new SolidColorBrush(Colors.Gray), null));

        /// <summary>
        /// 窗口边框颜色
        /// </summary>
        [Description("获取或设置窗口边框颜色")]
        [Category("Defined Properties")]
        public Brush WinBorderBrush
        {
            get { return (Brush)GetValue(WinBorderBrushProperty); }
            set { SetValue(WinBorderBrushProperty, value); }
        }

        public static readonly DependencyProperty WinBorderThicknessProperty = DependencyProperty.Register("WinBorderThickness", typeof(Thickness), typeof(WindowEx), new UIPropertyMetadata(new Thickness(0, 0, 0, 0), null));

        /// <summary>
        /// 窗口边框
        /// </summary>
        [Description("获取或设置窗口边框")]
        [Category("Defined Properties")]
        public Thickness WinBorderThickness
        {
            get { return (Thickness)GetValue(WinBorderThicknessProperty); }
            set { SetValue(WinBorderThicknessProperty, value); }
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