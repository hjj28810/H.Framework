using H.Framework.WPF.Control.Adorners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace H.Framework.WPF.Control.Controls
{
    public class BusyDecorator : System.Windows.Controls.Control
    {
        public event EventHandler Cancel;

        public BusyDecorator()
        {
            this.Loaded += ControlBusyDecorator_Loaded;
        }

        private void ControlBusyDecorator_Loaded(object sender, RoutedEventArgs e)
        {
            var parent = this.Parent as Panel;
        }

        private void ShowAdorner()
        {
            if (this.adorner != null)
            {
                this.adorner.Visibility = Visibility.Visible;
            }
            else
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(this);

                if (adornerLayer != null)
                {
                    var parent = this.Parent as Panel;
                    this.adorner = new BusyAdorner(parent);
                    this.adorner.Cancel += (s1, e1) => { if (Cancel != null) { Cancel(s1, e1); } };
                    adornerLayer.Add(this.adorner);
                }
            }
        }

        private void HideAdorner()
        {
            if (this.adorner != null)
            {
                this.adorner.Visibility = Visibility.Hidden;
            }
        }

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public static void OnIsBusyPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as BusyDecorator;
            bool showDecorator = (bool)e.NewValue;

            if (showDecorator)
            {
                self.ShowAdorner();
            }
            else
            {
                self.HideAdorner();
            }
        }

        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(BusyDecorator), new UIPropertyMetadata(false, OnIsBusyPropertyChangedCallback));

        private BusyAdorner adorner;
    }
}