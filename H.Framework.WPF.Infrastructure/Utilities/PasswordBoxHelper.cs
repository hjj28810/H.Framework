using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace H.Framework.WPF.Infrastructure.Utilities
{
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached("BoundPassword",
                typeof(string),
                typeof(PasswordBoxHelper),
                new FrameworkPropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static string GetBoundPassword(DependencyObject d)
        {
            var box = d as PasswordBox;
            if (box != null)
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
            var box = d as PasswordBox;

            if (box == null)
                return;

            box.Password = GetBoundPassword(d);
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var password = sender as PasswordBox;

            SetBoundPassword(password, password.Password);

            password.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(password, new object[] { password.Password.Length, 0 });
        }
    }
}