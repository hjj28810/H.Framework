using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace H.Framework.WPF.Control.Controls
{
    public class ImageButton : ButtonBase
    {
        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
            ClipToBoundsProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(false));
        }

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null, OnImageSourceChangedCallback));

        /// <summary>
        /// 背景图片路径
        /// </summary>
        [Description("获取或设置背景图片路径")]
        [Category("Defined Properties")]
        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public static void OnImageSourceChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as ImageButton;
            if (e.NewValue != e.OldValue)
            {
                var value = (ImageSource)e.NewValue;
                if (c.CoverImageSource == null)
                    c.CoverImageSource = value;
                if (c.PressImageSource == null)
                    c.PressImageSource = value;
            }
        }

        public static readonly DependencyProperty CoverImageSourceProperty = DependencyProperty.Register("CoverImageSource", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null, OnCoverImageSourceChangedCallback));

        /// <summary>
        /// 鼠标over图片路径
        /// </summary>
        [Description("获取或设置鼠标over图片路径")]
        [Category("Defined Properties")]
        public ImageSource CoverImageSource
        {
            get => (ImageSource)GetValue(CoverImageSourceProperty);
            set => SetValue(CoverImageSourceProperty, value);
        }

        public static void OnCoverImageSourceChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var c = d as ImageButton;
            //if (e.NewValue != null)
            //{
            //    var trigger = new Trigger();
            //    trigger.Property = IsMouseOverProperty;
            //    trigger.Value = true;
            //    trigger.Setters.Add(new Setter(ImageSourceProperty, e.NewValue, "PART_Image"));
            //    c.Triggers.Add(trigger);
            //}
            //else
            //    c.Triggers.Clear();
        }

        public static readonly DependencyProperty PressImageSourceProperty = DependencyProperty.Register("PressImageSource", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null, null));

        /// <summary>
        /// 鼠标Press图片路径
        /// </summary>
        [Description("获取或设置鼠标Press图片路径")]
        [Category("Defined Properties")]
        public ImageSource PressImageSource
        {
            get => (ImageSource)GetValue(PressImageSourceProperty);
            set => SetValue(PressImageSourceProperty, value);
        }
    }
}