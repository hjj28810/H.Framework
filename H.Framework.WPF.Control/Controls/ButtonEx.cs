using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace H.Framework.WPF.Control.Controls
{
    public class ButtonEx : ButtonBase
    {
        static ButtonEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonEx), new FrameworkPropertyMetadata(typeof(ButtonEx)));
        }

        public static readonly DependencyProperty CornerProperty = DependencyProperty.Register("Corner", typeof(CornerRadius), typeof(ButtonEx), new UIPropertyMetadata(new CornerRadius(0, 0, 0, 0), null));

        /// <summary>
        /// 圆角
        /// </summary>
        [Description("获取或设置圆角")]
        [Category("Defined Properties")]
        public CornerRadius Corner
        {
            get => (CornerRadius)GetValue(CornerProperty);
            set => SetValue(CornerProperty, value);
        }
    }
}