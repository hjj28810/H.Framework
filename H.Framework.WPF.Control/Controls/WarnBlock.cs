using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Threading;

namespace H.Framework.WPF.Control.Controls
{
    public class WarnBlock : System.Windows.Controls.Control
    {
        static WarnBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WarnBlock), new FrameworkPropertyMetadata(typeof(WarnBlock)));
            ClipToBoundsProperty.OverrideMetadata(typeof(WarnBlock), new FrameworkPropertyMetadata(false));
        }

        public WarnBlock()
        {
        }

        public static readonly DependencyProperty InfoColorProperty = DependencyProperty.Register("InfoColor", typeof(Brush), typeof(WarnBlock), new PropertyMetadata(new SolidColorBrush(Colors.LawnGreen), null));

        /// <summary>
        /// InfoColor
        /// </summary>
        [Description("获取或设置InfoColor")]
        [Category("Defined Properties")]
        public Brush InfoColor
        {
            get => (Brush)GetValue(InfoColorProperty);
            set => SetValue(InfoColorProperty, value);
        }

        public static readonly DependencyProperty ErrorColorProperty = DependencyProperty.Register("ErrorColor", typeof(Brush), typeof(WarnBlock), new PropertyMetadata(new SolidColorBrush(Colors.Red), null));

        /// <summary>
        /// ErrorColor
        /// </summary>
        [Description("ErrorColor")]
        [Category("Defined Properties")]
        public Brush ErrorColor
        {
            get => (Brush)GetValue(ErrorColorProperty);
            set => SetValue(ErrorColorProperty, value);
        }

        public static readonly DependencyProperty WarnColorProperty = DependencyProperty.Register("WarnColor", typeof(Brush), typeof(WarnBlock), new PropertyMetadata(new SolidColorBrush(Colors.DodgerBlue), null));

        /// <summary>
        /// WarnColor
        /// </summary>
        [Description("WarnColor")]
        [Category("Defined Properties")]
        public Brush WarnColor
        {
            get => (Brush)GetValue(WarnColorProperty);
            set => SetValue(WarnColorProperty, value);
        }

        public static readonly DependencyProperty TextColorProperty = DependencyProperty.Register("TextColor", typeof(Brush), typeof(WarnBlock), new PropertyMetadata(new SolidColorBrush(Colors.White), null));

        /// <summary>
        /// TextColor
        /// </summary>
        [Description("获取或设置TextColor")]
        [Category("Defined Properties")]
        public Brush TextColor
        {
            get => (Brush)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static readonly DependencyProperty CloseColorProperty = DependencyProperty.Register("CloseColor", typeof(Brush), typeof(WarnBlock), new PropertyMetadata(new SolidColorBrush(Colors.White), null));

        /// <summary>
        /// CloseColor
        /// </summary>
        [Description("获取或设置CloseColor")]
        [Category("Defined Properties")]
        public Brush CloseColor
        {
            get => (Brush)GetValue(CloseColorProperty);
            set => SetValue(CloseColorProperty, value);
        }

        //public static readonly DependencyProperty AlertStyleProperty = DependencyProperty.Register("AlertStyle", typeof(AlertStyle), typeof(WarnBlock), new PropertyMetadata(AlertStyle.Error, null));

        ///// <summary>
        ///// AlertStyle
        ///// </summary>
        //[Description("获取或设置AlertStyle")]
        //[Category("Defined Properties")]
        //public AlertStyle AlertStyle
        //{
        //    get { return (AlertStyle)GetValue(AlertStyleProperty); }
        //    set { SetValue(AlertStyleProperty, value); }
        //}

        private Grid _grid;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _grid = GetTemplateChild("PART_Grid") as Grid;
        }

        private Border CreateBorder(string text, AlertStyle style = AlertStyle.Error)
        {
            var border = new Border
            {
                BorderThickness = new Thickness(0),
                CornerRadius = new CornerRadius(4),
                Opacity = 0
            };
            var shadow = new DropShadowEffect
            {
                BlurRadius = 5,
                ShadowDepth = 0,
                Color = Color.FromRgb(136, 136, 136),
                Opacity = 0.3
            };
            if (style == AlertStyle.Error)
            {
                border.Background = ErrorColor;
            }
            if (style == AlertStyle.Info)
            {
                border.Background = InfoColor;
            }
            if (style == AlertStyle.Warinng)
            {
                border.Background = WarnColor;
            }

            border.Effect = shadow;
            var borderGrid = new Grid();
            borderGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(18) });
            borderGrid.RowDefinitions.Add(new RowDefinition());
            var txt1 = new TextBlock
            {
                Margin = new Thickness(6, 0, 0, 0),
                FontFamily = new FontFamily("Webdings"),
                FontSize = 11,
                Foreground = TextColor,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "i"
            };
            var stackPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(20, 0, 20, 0),
                Orientation = Orientation.Horizontal
            };
            stackPanel.Children.Add(txt1);
            var txt2 = new TextBlock
            {
                Margin = new Thickness(6, 0, 0, 0),
                FontFamily = new FontFamily("Webdings"),
                HorizontalAlignment = HorizontalAlignment.Left,
                FontSize = 12,
                VerticalAlignment = VerticalAlignment.Center,
                Cursor = Cursors.Hand,
                Foreground = CloseColor,
                Text = "r"
            };
            Grid.SetRow(txt2, 0);

            var txtWarn = new TextBlock
            {
                Margin = new Thickness(6, 0, 0, 0),
                FontSize = 11,
                Foreground = TextColor,
                TextTrimming = TextTrimming.CharacterEllipsis,
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Center,
                Text = text,
                ToolTip = text
            };
            stackPanel.Children.Add(txtWarn);
            Grid.SetRow(stackPanel, 1);

            var sbArr = CreateAnimation(border);
            txt2.MouseDown += (o, e) => { sbArr[1]?.Begin(); txt2.IsEnabled = false; };
            sbArr[1].Completed += (o, e) =>
            {
                sbArr[0]?.Stop();
                sbArr[1]?.Stop();
                sbArr[0] = null;
                sbArr[1] = null;
                _grid.Children.Remove(border);
            };

            borderGrid.Children.Add(txt2);
            borderGrid.Children.Add(stackPanel);
            border.Child = borderGrid;
            return border;
        }

        private Storyboard[] CreateAnimation(Border border)
        {
            var timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 3, 500) };
            var showStory = new Storyboard();
            var hideStory = new Storyboard();
            timer.Tick += (time, arg) =>
            {
                hideStory?.Begin();
                timer?.Stop();
                timer = null;
            };

            var opacityShowAnmiation = new DoubleAnimation();
            opacityShowAnmiation.BeginTime = new TimeSpan(0, 0, 0, 0, 0);
            opacityShowAnmiation.Duration = new Duration(TimeSpan.FromSeconds(0.8));
            opacityShowAnmiation.From = 0;
            opacityShowAnmiation.To = 1;
            Storyboard.SetTargetProperty(opacityShowAnmiation, new PropertyPath("Opacity"));
            Storyboard.SetTarget(opacityShowAnmiation, border);
            showStory.Children.Add(opacityShowAnmiation);

            var opacityHideAnmiation = new DoubleAnimation();
            opacityHideAnmiation.BeginTime = new TimeSpan(0, 0, 0, 0, 0);
            opacityHideAnmiation.Duration = new Duration(TimeSpan.FromSeconds(0.8));
            opacityHideAnmiation.From = 1;
            opacityHideAnmiation.To = 0;
            Storyboard.SetTargetProperty(opacityHideAnmiation, new PropertyPath("Opacity"));
            Storyboard.SetTarget(opacityHideAnmiation, border);
            hideStory.Children.Add(opacityHideAnmiation);

            showStory?.Begin();
            timer?.Start();
            return new Storyboard[] { showStory, hideStory };
        }

        public void Show(string text, AlertStyle style = AlertStyle.Error)
        {
            if (_grid != null)
                _grid.Children.Add(CreateBorder(text, style));
        }
    }

    public enum AlertStyle
    {
        Info,
        Error,
        Warinng
    }
}