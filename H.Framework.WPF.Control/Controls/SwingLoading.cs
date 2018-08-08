using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Windows.Media.Animation;

namespace H.Framework.WPF.Control.Controls
{
    public class SwingLoading : System.Windows.Controls.Control
    {
        static SwingLoading()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SwingLoading), new FrameworkPropertyMetadata(typeof(SwingLoading)));
            ClipToBoundsProperty.OverrideMetadata(typeof(SwingLoading), new FrameworkPropertyMetadata(false));
        }

        public static readonly DependencyProperty ShowUpProperty = DependencyProperty.Register("ShowUp", typeof(Visibility), typeof(SwingLoading), new PropertyMetadata(Visibility.Hidden, null));

        /// <summary>
        /// 是否ShowUp
        /// </summary>
        [Description("获取或设置是否ShowUp")]
        [Category("Defined Properties")]
        public Visibility ShowUp
        {
            get { return (Visibility)GetValue(ShowUpProperty); }
            set { SetValue(ShowUpProperty, value); }
        }
    }
}