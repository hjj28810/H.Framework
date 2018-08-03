using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace H.Framework.WPF.Control.Adorners
{
    public class WatermarkAdorner : Adorner
    {
        public WatermarkAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            this.IsHitTestVisible = false;
            if (adornedElement is TextBox)
            {
                adornedTextBox = adornedElement as TextBox;
                adornedTextBox.TextChanged += (s1, e1) => InvalidateVisual();
                adornedTextBox.GotFocus += (s1, e1) => InvalidateVisual();
                adornedTextBox.LostFocus += (s1, e1) => InvalidateVisual();
                this.InvalidateVisual();
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (adornedTextBox != null && !adornedTextBox.IsFocused && adornedTextBox.Visibility == Visibility.Visible)
            {
                var fmt = new FormattedText(GetText(adornedTextBox),
                CultureInfo.CurrentCulture,
                adornedTextBox.FlowDirection,
                adornedTextBox.FontFamily.GetTypefaces().FirstOrDefault(),
                GetFontSize(adornedTextBox),
                //adornedTextBox.FontSize,
                GetForeground(adornedTextBox));
                fmt.SetFontStyle(GetFontStyle(adornedTextBox));

                dc.DrawRectangle(GetBackground(adornedTextBox), null, new Rect(
                    new Point(adornedTextBox.Padding.Left + 4, adornedTextBox.Padding.Top + 3),
                    new Size(fmt.Width, fmt.Height)));

                dc.DrawText(fmt, new Point(adornedTextBox.Padding.Left + 4, adornedTextBox.Padding.Top + 3));
            }
        }

        public static void OnTextPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = d as TextBox;
            source.Loaded += (s1, e1) =>
            {
                var adorner = new WatermarkAdorner(source);
                if (adorner != null)
                {
                    var layer = AdornerLayer.GetAdornerLayer(source);
                    if (layer != null)
                        layer.Add(adorner);
                }
            };
        }

        public static string GetText(DependencyObject obj)
        {
            return (string)obj.GetValue(TextProperty);
        }

        public static void SetText(DependencyObject obj, string value)
        {
            obj.SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached("Text", typeof(string), typeof(WatermarkAdorner), new UIPropertyMetadata(null, OnTextPropertyChangedCallback));

        public static Brush GetForeground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(ForegroundProperty);
        }

        public static void SetForeground(DependencyObject obj, Brush value)
        {
            obj.SetValue(ForegroundProperty, value);
        }

        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.RegisterAttached("Foreground", typeof(Brush), typeof(WatermarkAdorner), new UIPropertyMetadata(new SolidColorBrush(Color.FromRgb(191, 190, 188))));

        public static Brush GetBackground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(BackgroundProperty);
        }

        public static void SetBackground(DependencyObject obj, Brush value)
        {
            obj.SetValue(BackgroundProperty, value);
        }

        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.RegisterAttached("Background", typeof(Brush), typeof(WatermarkAdorner), new UIPropertyMetadata(Brushes.Transparent));

        public static FontStyle GetFontStyle(DependencyObject obj)
        {
            return (FontStyle)obj.GetValue(FontStyleProperty);
        }

        public static void SetFontStyle(DependencyObject obj, FontStyle value)
        {
            obj.SetValue(FontStyleProperty, value);
        }

        public static readonly DependencyProperty FontStyleProperty =
            DependencyProperty.RegisterAttached("FontStyle", typeof(FontStyle), typeof(WatermarkAdorner), new UIPropertyMetadata(FontStyles.Italic));

        public static int GetFontSize(DependencyObject obj)
        {
            return (int)obj.GetValue(FontSizeProperty);
        }

        public static void SetFontSize(DependencyObject obj, int value)
        {
            obj.SetValue(FontSizeProperty, value);
        }

        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.RegisterAttached("FontSize", typeof(int), typeof(WatermarkAdorner), new UIPropertyMetadata(10));

        private TextBox adornedTextBox;
    }
}