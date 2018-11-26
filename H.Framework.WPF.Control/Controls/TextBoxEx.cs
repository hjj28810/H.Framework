using System.Windows;
using System.Windows.Controls;

namespace H.Framework.WPF.Control.Controls
{
    public class TextBoxEx : TextBox
    {
        #region Properties

        #region KeepWatermarkOnGotFocus

        public static readonly DependencyProperty KeepWatermarkOnGotFocusProperty = DependencyProperty.Register("KeepWatermarkOnGotFocus", typeof(bool), typeof(TextBoxEx), new UIPropertyMetadata(false));

        public bool KeepWatermarkOnGotFocus
        {
            get => (bool)GetValue(KeepWatermarkOnGotFocusProperty);
            set => SetValue(KeepWatermarkOnGotFocusProperty, value);
        }

        #endregion KeepWatermarkOnGotFocus

        #region Watermark

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(object), typeof(TextBoxEx), new UIPropertyMetadata(null));

        public object Watermark
        {
            get => GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }

        #endregion Watermark

        #region WatermarkVisibility

        public static readonly DependencyProperty WatermarkVisibilityProperty = DependencyProperty.Register("WatermarkVisibility", typeof(Visibility), typeof(TextBoxEx), new UIPropertyMetadata(Visibility.Collapsed));

        public Visibility WatermarkVisibility
        {
            get => (Visibility)GetValue(WatermarkVisibilityProperty);
            set => SetValue(WatermarkVisibilityProperty, value);
        }

        #endregion WatermarkVisibility

        #region WatermarkTemplate

        public static readonly DependencyProperty WatermarkTemplateProperty = DependencyProperty.Register("WatermarkTemplate", typeof(DataTemplate), typeof(TextBoxEx), new UIPropertyMetadata(null));

        public DataTemplate WatermarkTemplate
        {
            get => (DataTemplate)GetValue(WatermarkTemplateProperty);
            set => SetValue(WatermarkTemplateProperty, value);
        }

        #endregion WatermarkTemplate

        #region CornerRadius

        public static readonly DependencyProperty BorderCornerRadiusProperty = DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(TextBoxEx), new UIPropertyMetadata(new CornerRadius(0, 0, 0, 0)));

        public CornerRadius BorderCornerRadius
        {
            get => (CornerRadius)GetValue(BorderCornerRadiusProperty);
            set => SetValue(BorderCornerRadiusProperty, value);
        }

        #endregion CornerRadius

        #endregion Properties

        #region Constructors

        static TextBoxEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxEx), new FrameworkPropertyMetadata(typeof(TextBoxEx)));
        }

        #endregion Constructors
    }
}