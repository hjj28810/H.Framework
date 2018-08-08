using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        //public static readonly DependencyProperty WarnTextProperty = DependencyProperty.Register("WarnText", typeof(string), typeof(WarnBlock), new PropertyMetadata("", null));

        ///// <summary>
        ///// WarnText
        ///// </summary>
        //[Description("获取或设置WarnText")]
        //[Category("Defined Properties")]
        //public string WarnText
        //{
        //    get { return (string)GetValue(WarnTextProperty); }
        //    set { SetValue(WarnTextProperty, value); }
        //}

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
                CornerRadius = new CornerRadius(5),
                Opacity = 0
            };
            var shadow = new DropShadowEffect
            {
                BlurRadius = 5,
                ShadowDepth = 0
            };
            if (style == AlertStyle.Error)
            {
                border.Background = new SolidColorBrush(Colors.Red);
                shadow.Color = Colors.Red;
            }
            if (style == AlertStyle.Info)
            {
                shadow.Color = Colors.LawnGreen;
                border.Background = new SolidColorBrush(Colors.LawnGreen);
            }
            if (style == AlertStyle.Warinng)
            {
                shadow.Color = Colors.DodgerBlue;
                border.Background = new SolidColorBrush(Colors.DodgerBlue);
            }

            border.Effect = shadow;
            var borderGrid = new Grid();
            borderGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            borderGrid.RowDefinitions.Add(new RowDefinition());
            var txt1 = new TextBlock
            {
                Margin = new Thickness(5, 5, 0, 0),
                FontFamily = new FontFamily("Webdings"),
                FontSize = 10,
                Foreground = new SolidColorBrush(Colors.White),
                Text = "i"
            };
            Grid.SetRow(txt1, 0);

            var txt2 = new TextBlock
            {
                Margin = new Thickness(0, 5, 5, 0),
                FontFamily = new FontFamily("Webdings"),
                HorizontalAlignment = HorizontalAlignment.Right,
                FontSize = 10,
                Foreground = new SolidColorBrush(Colors.White),
                Text = "r"
            };
            Grid.SetRow(txt2, 0);

            var txtWarn = new TextBlock
            {
                Margin = new Thickness(5),
                FontSize = 13,
                Foreground = new SolidColorBrush(Colors.White),
                TextTrimming = TextTrimming.CharacterEllipsis,
                TextWrapping = TextWrapping.Wrap,
                Text = text,
                ToolTip = text
            };
            Grid.SetRow(txtWarn, 1);

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
            borderGrid.Children.Add(txt1);
            borderGrid.Children.Add(txt2);
            borderGrid.Children.Add(txtWarn);
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
            opacityShowAnmiation.To = 0.6;
            Storyboard.SetTargetProperty(opacityShowAnmiation, new PropertyPath("Opacity"));
            Storyboard.SetTarget(opacityShowAnmiation, border);
            showStory.Children.Add(opacityShowAnmiation);

            var opacityHideAnmiation = new DoubleAnimation();
            opacityHideAnmiation.BeginTime = new TimeSpan(0, 0, 0, 0, 0);
            opacityHideAnmiation.Duration = new Duration(TimeSpan.FromSeconds(0.8));
            opacityHideAnmiation.From = 0.6;
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