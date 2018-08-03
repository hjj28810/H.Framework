using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace H.Framework.WPF.Control.Controls
{
    [TemplatePart(Name = "mPanel", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_Ellipse", Type = typeof(Ellipse))]
    [TemplatePart(Name = "PART_TextBlock", Type = typeof(TextBlock))]
    public class UnreadTip : System.Windows.Controls.Control
    {
        static UnreadTip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UnreadTip), new FrameworkPropertyMetadata(typeof(UnreadTip)));
            ClipToBoundsProperty.OverrideMetadata(typeof(UnreadTip), new FrameworkPropertyMetadata(true));
            VisibilityProperty.OverrideMetadata(typeof(UnreadTip), new FrameworkPropertyMetadata(Visibility.Collapsed));
            HeightProperty.OverrideMetadata(typeof(UnreadTip), new FrameworkPropertyMetadata(10.00));
            WidthProperty.OverrideMetadata(typeof(UnreadTip), new FrameworkPropertyMetadata(15.00));
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
          DependencyProperty.Register("Text", typeof(string), typeof(UnreadTip), new PropertyMetadata("0", new PropertyChangedCallback(OnTextPropertyChanged)));

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (UnreadTip)d;
            if (e.NewValue.ToString() == "0")
                obj.Visibility = Visibility.Collapsed;
            else
                obj.Visibility = Visibility.Visible;
            obj.OnTextChanged(e.OldValue, e.NewValue);
        }

        public bool IsShowText
        {
            get { return (bool)GetValue(IsShowTextProperty); }
            set { SetValue(IsShowTextProperty, value); }
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

        protected virtual void OnTextChanged(Object oldValue, Object newValue)
        {
            RoutedPropertyChangedEventArgs<Object> arg =
                new RoutedPropertyChangedEventArgs<Object>(oldValue, newValue, TextChangedEvent);
            this.RaiseEvent(arg);
        }
    }
}