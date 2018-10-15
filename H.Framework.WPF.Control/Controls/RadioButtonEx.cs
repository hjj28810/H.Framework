using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace H.Framework.WPF.Control.Controls
{
    public class RadioButtonEx : RadioButton
    {
        static RadioButtonEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioButtonEx), new FrameworkPropertyMetadata(typeof(RadioButtonEx)));
        }

        public static readonly DependencyProperty CornerProperty = DependencyProperty.Register("Corner", typeof(CornerRadius), typeof(RadioButtonEx), new UIPropertyMetadata(new CornerRadius(0, 0, 0, 0), null));

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

        public static readonly DependencyProperty CheckedColorProperty = DependencyProperty.Register("CheckedColor", typeof(Brush), typeof(RadioButtonEx), new UIPropertyMetadata(new SolidColorBrush(Colors.AliceBlue), null));

        /// <summary>
        /// 选中色
        /// </summary>
        [Description("获取或设置选中色")]
        [Category("Defined Properties")]
        public Brush CheckedColor
        {
            get => (Brush)GetValue(CheckedColorProperty);
            set => SetValue(CheckedColorProperty, value);
        }

        public static readonly DependencyProperty IsChangedBackgroundProperty = DependencyProperty.Register("IsChangedBackground", typeof(bool), typeof(RadioButtonEx), new UIPropertyMetadata(false, IsChangedBackgroundPropertyChangedCallback));

        /// <summary>
        /// 选中色
        /// </summary>
        [Description("获取或设置选中色")]
        [Category("Defined Properties")]
        public bool IsChangedBackground
        {
            get => (bool)GetValue(IsChangedBackgroundProperty);
            set => SetValue(IsChangedBackgroundProperty, value);
        }

        public static void IsChangedBackgroundPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var btn = (RadioButtonEx)d;
            //if ((bool)e.NewValue)
            //{
            //    btn.Background = btn.CheckedColor;
            //    btn.Foreground = new SolidColorBrush(Colors.White);
            //}
            //else
            //{
            //    btn.Background = btn.CheckedColor;
            //    btn.Foreground = new SolidColorBrush(Colors.White);
            //}
        }
    }
}