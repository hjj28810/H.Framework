using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace H.Framework.WPF.Control.Controls
{
    [TemplatePart(Name = "PART_ErrorTip", Type = typeof(TipBubble))]
    public class TextBoxEx : TextBox
    {
        #region Properties

        public static readonly DependencyProperty KeepWatermarkOnGotFocusProperty = DependencyProperty.Register("KeepWatermarkOnGotFocus", typeof(bool), typeof(TextBoxEx), new UIPropertyMetadata(false));

        /// <summary>
        /// 是否聚焦时保持水印
        /// </summary>
        [Description("获取或设置是否聚焦时保持水印")]
        [Category("Defined Properties")]
        public bool KeepWatermarkOnGotFocus
        {
            get => (bool)GetValue(KeepWatermarkOnGotFocusProperty);
            set => SetValue(KeepWatermarkOnGotFocusProperty, value);
        }

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(object), typeof(TextBoxEx), new UIPropertyMetadata(null));

        /// <summary>
        /// 水印
        /// </summary>
        [Description("获取或设置水印")]
        [Category("Defined Properties")]
        public object Watermark
        {
            get => GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }

        public static readonly DependencyProperty WatermarkTemplateProperty = DependencyProperty.Register("WatermarkTemplate", typeof(DataTemplate), typeof(TextBoxEx), new UIPropertyMetadata(null));

        /// <summary>
        /// 水印模板
        /// </summary>
        [Description("获取或设置水印模板")]
        [Category("Defined Properties")]
        public DataTemplate WatermarkTemplate
        {
            get => (DataTemplate)GetValue(WatermarkTemplateProperty);
            set => SetValue(WatermarkTemplateProperty, value);
        }

        public static readonly DependencyProperty BorderCornerRadiusProperty = DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(TextBoxEx), new UIPropertyMetadata(new CornerRadius(0, 0, 0, 0)));

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

        #region ValidationError

        public static readonly DependencyProperty ErrorCornerProperty = DependencyProperty.Register("ErrorCorner", typeof(CornerRadius), typeof(TextBoxEx), new UIPropertyMetadata(new CornerRadius(3, 3, 3, 3), null));

        /// <summary>
        /// 异常-圆角
        /// </summary>
        [Description("获取或设置圆角")]
        [Category("Defined Properties")]
        public CornerRadius ErrorCorner
        {
            get => (CornerRadius)GetValue(ErrorCornerProperty);
            set => SetValue(ErrorCornerProperty, value);
        }

        public static readonly DependencyProperty ErrorTextProperty = DependencyProperty.Register("ErrorText", typeof(string), typeof(TextBoxEx), new UIPropertyMetadata("", null));

        /// <summary>
        /// 异常-文本
        /// </summary>
        [Description("获取或设置文本")]
        [Category("Defined Properties")]
        public string ErrorText
        {
            get => (string)GetValue(ErrorTextProperty);
            set => SetValue(ErrorTextProperty, value);
        }

        public static readonly DependencyProperty ErrorBackgroundProperty = DependencyProperty.Register("ErrorBackground", typeof(Brush), typeof(TextBoxEx), new UIPropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F1")), null));

        /// <summary>
        /// 异常-背景色
        /// </summary>
        [Description("获取或设置背景色")]
        [Category("Defined Properties")]
        public Brush ErrorBackground
        {
            get => (Brush)GetValue(ErrorBackgroundProperty);
            set => SetValue(ErrorBackgroundProperty, value);
        }

        public static readonly DependencyProperty ErrorBorderBrushProperty = DependencyProperty.Register("ErrorBorderBrush", typeof(Brush), typeof(TextBoxEx), new UIPropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 73, 74)), null));

        /// <summary>
        /// 异常-边框色
        /// </summary>
        [Description("获取或设置边框色")]
        [Category("Defined Properties")]
        public Brush ErrorBorderBrush
        {
            get => (Brush)GetValue(ErrorBorderBrushProperty);
            set => SetValue(ErrorBorderBrushProperty, value);
        }

        #endregion ValidationError

        #endregion Properties

        #region Constructors

        static TextBoxEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxEx), new FrameworkPropertyMetadata(typeof(TextBoxEx)));
        }

        #endregion Constructors

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_ErrorTip") is TipBubble tip)
            {
                tip.CustomPopupPlacementChanged -= CustomPopupPlacementChanged;
                tip.CustomPopupPlacementChanged += CustomPopupPlacementChanged;
            }
            TextChanged -= TextBoxEx_TextChanged;
            TextChanged += TextBoxEx_TextChanged;
        }

        private void TextBoxEx_TextChanged(object sender, TextChangedEventArgs e)
        {
            (sender as TextBoxEx)?.ScrollToEnd();
        }

        private CustomPopupPlacement[] CustomPopupPlacementChanged(Size popupSize, Size targetSize, Point offset)
        {
            return new CustomPopupPlacement[] { new CustomPopupPlacement(new Point(5, offset.Y + 25), PopupPrimaryAxis.Vertical) };
        }
    }
}