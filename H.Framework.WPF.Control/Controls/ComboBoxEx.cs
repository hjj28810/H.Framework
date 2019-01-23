using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace H.Framework.WPF.Control.Controls
{
    public class ComboBoxEx : ComboBox
    {
        #region Properties

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(object), typeof(ComboBoxEx), new UIPropertyMetadata(null));

        public object Watermark
        {
            get => GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }

        public static readonly DependencyProperty WatermarkTemplateProperty = DependencyProperty.Register("WatermarkTemplate", typeof(DataTemplate), typeof(ComboBoxEx), new UIPropertyMetadata(null));

        public DataTemplate WatermarkTemplate
        {
            get => (DataTemplate)GetValue(WatermarkTemplateProperty);
            set => SetValue(WatermarkTemplateProperty, value);
        }

        public static readonly DependencyProperty BorderCornerRadiusProperty = DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(ComboBoxEx), new UIPropertyMetadata(new CornerRadius(0, 0, 0, 0)));

        /// <summary>
        /// 圆角
        /// </summary>
        [Description("获取或设置圆角")]
        [Category("Defined Properties")]
        public CornerRadius BorderCornerRadius
        {
            get => (CornerRadius)GetValue(BorderCornerRadiusProperty);
            set => SetValue(BorderCornerRadiusProperty, value);
        }

        public static readonly DependencyProperty DropBlurRadiusProperty = DependencyProperty.Register("DropBlurRadius", typeof(CornerRadius), typeof(ComboBoxEx), new UIPropertyMetadata(new CornerRadius(4, 4, 4, 4)));

        /// <summary>
        /// 下拉阴影
        /// </summary>
        [Description("获取或设置下拉阴影")]
        [Category("Defined Properties")]
        public CornerRadius DropBlurRadius
        {
            get => (CornerRadius)GetValue(DropBlurRadiusProperty);
            set => SetValue(DropBlurRadiusProperty, value);
        }

        public static readonly DependencyProperty DropMinWidthProperty = DependencyProperty.Register("DropMinWidth", typeof(double), typeof(ComboBoxEx), new UIPropertyMetadata(0.0));

        /// <summary>
        /// 最小宽度
        /// </summary>
        [Description("获取或设置下拉最小宽度")]
        [Category("Defined Properties")]
        public double DropMinWidth
        {
            get => (double)GetValue(DropMinWidthProperty);
            set => SetValue(DropMinWidthProperty, value);
        }

        #endregion Properties

        #region Constructors

        static ComboBoxEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBoxEx), new FrameworkPropertyMetadata(typeof(ComboBoxEx)));
        }

        public ComboBoxEx()
        {
        }

        //protected override void OnRender(DrawingContext drawingContext)
        //{
        //    base.OnRender(drawingContext);
        //    DropMinWidth = ActualWidth + 12;
        //}

        #endregion Constructors
    }
}