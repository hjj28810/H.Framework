using H.Framework.WPF.Control.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace H.Framework.WPF.Control.ControlHelpers
{
    public static class TextBoxHelper
    {
        public static readonly DependencyProperty IsOnFocusProperty =
       DependencyProperty.RegisterAttached("IsOnFocus",
           typeof(bool),
           typeof(TextBoxHelper),
           new FrameworkPropertyMetadata(OnIsOnFocusChanged));

        public static bool GetIsOnFocus(DependencyObject d)
        {
            return (bool)d.GetValue(IsOnFocusProperty);
        }

        public static void SetIsOnFocus(DependencyObject d, bool value)
        {
            d.SetValue(IsOnFocusProperty, value);
        }

        private static void OnIsOnFocusChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TextBoxEx box))
                return;
            var b = (bool)e.NewValue;

            if (b)
            {
                box.Focus();
            }
        }

        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached("BoundPassword",
                typeof(string),
                typeof(TextBoxHelper),
                new FrameworkPropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static string GetBoundPassword(DependencyObject d)
        {
            if (d is WatermarkPasswordBox box)
            {
                box.PasswordChanged -= PasswordChanged;
                box.PasswordChanged += PasswordChanged;
            }

            return (string)d.GetValue(BoundPasswordProperty);
        }

        public static void SetBoundPassword(DependencyObject d, string value)
        {
            if (string.Equals(value, GetBoundPassword(d)))
                return;

            d.SetValue(BoundPasswordProperty, value);
        }

        private static void OnBoundPasswordChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (!(d is WatermarkPasswordBox box))
                return;

            box.Password = GetBoundPassword(d);
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var password = sender as WatermarkPasswordBox;

            SetBoundPassword(password, password.Password);

            //password.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(password, new object[] { password.Password.Length, 0 });
        }
    }
}