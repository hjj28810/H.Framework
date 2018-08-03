using H.Framework.WPF.Control.Adorners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace H.Framework.WPF.Control.Controls
{
    [TemplatePart(Name = "PART_Cancel", Type = typeof(Button))]
    public class BusyChrome : System.Windows.Controls.Control
    {
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var cancelButton = GetTemplateChild("PART_Cancel") as Button;
            if (cancelButton != null)
            {
                cancelButton.Click += new RoutedEventHandler(cancelButton_Click);
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(this) as BusyAdorner;
            parent.FireCancel();
        }

        static BusyChrome()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BusyChrome), new FrameworkPropertyMetadata(typeof(BusyChrome)));
        }

        public static void OnAllowCancelPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as BusyChrome;
            bool allow = (bool)e.NewValue;
            var cancelButton = self.GetTemplateChild("PART_Cancel") as Button;

            if (cancelButton != null)
            {
                if (allow)
                {
                    cancelButton.Visibility = Visibility.Visible;
                }
                else
                {
                    cancelButton.Visibility = Visibility.Collapsed;
                }
            }
        }

        public bool AllowCancel
        {
            get { return (bool)GetValue(AllowCancelProperty); }
            set { SetValue(AllowCancelProperty, value); }
        }

        public static readonly DependencyProperty AllowCancelProperty =
            DependencyProperty.Register("AllowCancel", typeof(bool), typeof(BusyChrome), new UIPropertyMetadata(true, OnAllowCancelPropertyChangedCallback));
    }
}