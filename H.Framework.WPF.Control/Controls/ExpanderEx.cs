using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace H.Framework.WPF.Control.Controls
{
    public class ExpanderEx : Expander
    {
        static ExpanderEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExpanderEx), new FrameworkPropertyMetadata(typeof(ExpanderEx)));
            ClipToBoundsProperty.OverrideMetadata(typeof(ExpanderEx), new FrameworkPropertyMetadata(false));
        }

        public static readonly DependencyProperty HeaderHeightProperty = DependencyProperty.Register("HeaderHeight", typeof(double), typeof(ExpanderEx), new PropertyMetadata(12.0, null));

        /// <summary>
        /// Header高度
        /// </summary>
        [Description("获取或设置Header高度")]
        [Category("Defined Properties")]
        public double HeaderHeight
        {
            get => (double)GetValue(HeaderHeightProperty);
            set => SetValue(HeaderHeightProperty, value);
        }
    }
}