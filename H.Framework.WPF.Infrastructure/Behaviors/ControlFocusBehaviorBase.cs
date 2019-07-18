using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace H.Framework.WPF.Infrastructure.Behaviors
{
    public class ControlFocusBehaviorBase : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.RegisterAttached(
            "IsFocused", typeof(bool), typeof(ControlFocusBehaviorBase),
            new PropertyMetadata(IsFocusedPropertyChanged));

        private static void IsFocusedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (!(dependencyObject is Control p)) return;
            if ((e.NewValue is bool ? (bool)e.NewValue : false))
            {
                p.Focus();
            }
        }

        public static bool GetIsFocused(DependencyObject p)
        {
            return p.GetValue(IsFocusedProperty) is bool ? (bool)p.GetValue(IsFocusedProperty) : false;
        }

        public static void SetIsFocused(DependencyObject p, bool value)
        {
            p.SetValue(IsFocusedProperty, value);
        }
    }
}