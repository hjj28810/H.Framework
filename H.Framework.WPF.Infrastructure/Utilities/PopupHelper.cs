using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace H.Framework.WPF.Infrastructure.Utilities
{
    public class PopupHelper
    {
        public static DependencyObject GetPopupPlacementTarget(DependencyObject obj)
        {
            return (DependencyObject)obj.GetValue(PopupPlacementTargetProperty);
        }

        public static void SetPopupPlacementTarget(DependencyObject obj, DependencyObject value)
        {
            obj.SetValue(PopupPlacementTargetProperty, value);
        }

        // Using a DependencyProperty as the backing store for PopupPlacementTarget.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PopupPlacementTargetProperty =
            DependencyProperty.RegisterAttached("PopupPlacementTarget", typeof(DependencyObject), typeof(PopupHelper), new PropertyMetadata(null, OnPopupPlacementTargetChanged));

        private static void OnPopupPlacementTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var pop = d as Popup;
                var w = Window.GetWindow(e.NewValue as DependencyObject);
                if (null != w)
                {
                    w.LocationChanged += delegate
                    {
                        var offset = pop.HorizontalOffset;
                        pop.HorizontalOffset = offset + 1;
                        pop.HorizontalOffset = offset;
                    };
                }
            }
        }
    }
}