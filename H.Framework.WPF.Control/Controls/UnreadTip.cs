using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace H.Framework.WPF.Control.Controls
{
    [TemplatePart(Name = "mPanel", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_Ellipse", Type = typeof(Ellipse))]
    [TemplatePart(Name = "PART_TextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_TextBlockLong", Type = typeof(TextBlock))]
    public class UnreadTip : System.Windows.Controls.Control
    {
        private TextBlock _longBlock, _block;

        static UnreadTip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UnreadTip), new FrameworkPropertyMetadata(typeof(UnreadTip)));
            ClipToBoundsProperty.OverrideMetadata(typeof(UnreadTip), new FrameworkPropertyMetadata(true));
            VisibilityProperty.OverrideMetadata(typeof(UnreadTip), new FrameworkPropertyMetadata(Visibility.Collapsed));
            HeightProperty.OverrideMetadata(typeof(UnreadTip), new FrameworkPropertyMetadata(10.00));
            WidthProperty.OverrideMetadata(typeof(UnreadTip), new FrameworkPropertyMetadata(15.00));
            FontSizeProperty.OverrideMetadata(typeof(UnreadTip), new FrameworkPropertyMetadata(8.00));
            BackgroundProperty.OverrideMetadata(typeof(UnreadTip), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Red)));
        }

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty ValueProperty =
          DependencyProperty.Register("Value", typeof(string), typeof(UnreadTip), new PropertyMetadata("0", new PropertyChangedCallback(OnTextPropertyChanged)));

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (UnreadTip)d;
            if (e.NewValue?.ToString() == "0")
                obj.Visibility = Visibility.Collapsed;
            else
                obj.Visibility = Visibility.Visible;
            if (obj._longBlock != null && obj._block != null)
                if (e.NewValue.ToString().Length > 2)
                {
                    obj._longBlock.Visibility = Visibility.Visible;
                    obj._longBlock.FontSize = obj.FontSize - 2;
                    obj._block.Visibility = Visibility.Hidden;
                }
                else
                {
                    obj._longBlock.Visibility = Visibility.Hidden;
                    obj._block.Visibility = Visibility.Visible;
                }
            obj.OnTextChanged(e.OldValue, e.NewValue);
        }

        public bool IsShowText
        {
            get => (bool)GetValue(IsShowTextProperty);
            set => SetValue(IsShowTextProperty, value);
        }

        public static readonly DependencyProperty IsShowTextProperty =
          DependencyProperty.Register("IsShowText", typeof(bool), typeof(UnreadTip), new PropertyMetadata(true, null));

        public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent("TextChangedChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Object>), typeof(UnreadTip));

        [Description("Text更改后触发")]
        public event RoutedPropertyChangedEventHandler<Object> TextChanged
        {
            add
            {
                this.AddHandler(TextChangedEvent, value);
            }
            remove
            {
                this.RemoveHandler(TextChangedEvent, value);
            }
        }

        protected virtual void OnTextChanged(object oldValue, object newValue)
        {
            RoutedPropertyChangedEventArgs<object> arg =
                new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, TextChangedEvent);
            this.RaiseEvent(arg);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _longBlock = GetTemplateChild("PART_TextBlockLong") as TextBlock;
            _block = GetTemplateChild("PART_TextBlock") as TextBlock;
        }
    }
}