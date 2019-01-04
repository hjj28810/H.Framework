using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace H.Framework.WPF.Control.Controls
{
    public class MarketLoading : System.Windows.Controls.Control
    {
        static MarketLoading()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MarketLoading), new FrameworkPropertyMetadata(typeof(MarketLoading)));
            ClipToBoundsProperty.OverrideMetadata(typeof(MarketLoading), new FrameworkPropertyMetadata(false));
            BackgroundProperty.OverrideMetadata(typeof(MarketLoading), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent)));
        }

        public static readonly DependencyProperty ShowUpProperty = DependencyProperty.Register("ShowUp", typeof(Visibility), typeof(MarketLoading), new PropertyMetadata(Visibility.Hidden, null));

        /// <summary>
        /// 是否ShowUp
        /// </summary>
        [Description("获取或设置是否ShowUp")]
        [Category("Defined Properties")]
        public Visibility ShowUp
        {
            get => (Visibility)GetValue(ShowUpProperty);
            set => SetValue(ShowUpProperty, value);
        }

        public static readonly DependencyProperty AnimationColorProperty = DependencyProperty.Register("AnimationColor", typeof(Brush), typeof(MarketLoading), new PropertyMetadata(new SolidColorBrush(Colors.Blue), null));

        /// <summary>
        /// AnimationColor
        /// </summary>
        [Description("获取或设置AnimationColor")]
        [Category("Defined Properties")]
        public Brush AnimationColor
        {
            get => (Brush)GetValue(AnimationColorProperty);
            set => SetValue(AnimationColorProperty, value);
        }
    }
}