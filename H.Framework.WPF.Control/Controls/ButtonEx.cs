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

        public static readonly DependencyProperty ErrorMsgProperty = DependencyProperty.Register("ErrorMsg", typeof(string), typeof(ButtonEx), new UIPropertyMetadata(null, ErrorMsgPropertyChangedCallback));

        /// <summary>
        /// 错误信息
        /// </summary>
        [Description("获取或设置错误信息")]
        [Category("Defined Properties")]
        public string ErrorMsg
        {
            get => (string)GetValue(ErrorMsgProperty);
            set => SetValue(ErrorMsgProperty, value);
        }

        public static void ErrorMsgPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ButtonEx)d;
            ctrl._tip.IsShow = !string.IsNullOrWhiteSpace(e.NewValue.ToString());
        }

        private TipBubble _tip;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _tip = GetTemplateChild("PART_ErrorTip") as TipBubble;
            if (_tip != null)
            {
                _tip.CustomPopupPlacementChanged -= CustomPopupPlacementChanged;
                _tip.CustomPopupPlacementChanged += CustomPopupPlacementChanged;
            }
        }

        private CustomPopupPlacement[] CustomPopupPlacementChanged(Size popupSize, Size targetSize, Point offset)
        {
            return new CustomPopupPlacement[] { new CustomPopupPlacement(new Point(5, offset.Y + 15), PopupPrimaryAxis.Vertical) };
        }
    }
}