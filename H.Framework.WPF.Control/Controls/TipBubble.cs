using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace H.Framework.WPF.Control.Controls
{
    [TemplatePart(Name = "PART_Main", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    public class TipBubble : System.Windows.Controls.Control
    {
        static TipBubble()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TipBubble), new FrameworkPropertyMetadata(typeof(TipBubble)));
        }

        public static readonly DependencyProperty CornerProperty = DependencyProperty.Register("Corner", typeof(CornerRadius), typeof(TipBubble), new UIPropertyMetadata(new CornerRadius(3, 3, 3, 3), null));

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

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TipBubble), new UIPropertyMetadata("消息", null));

        /// <summary>
        /// 文本
        /// </summary>
        [Description("获取或设置文本")]
        [Category("Defined Properties")]
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty IconVisibilityProperty = DependencyProperty.Register("IconVisibility", typeof(Visibility), typeof(TipBubble), new UIPropertyMetadata(Visibility.Visible, null));

        /// <summary>
        /// 文本
        /// </summary>
        [Description("获取或设置文本")]
        [Category("Defined Properties")]
        public Visibility IconVisibility
        {
            get => (Visibility)GetValue(IconVisibilityProperty);
            set => SetValue(IconVisibilityProperty, value);
        }

        public static readonly DependencyProperty IsShowProperty = DependencyProperty.Register("IsShow", typeof(bool), typeof(TipBubble), new UIPropertyMetadata(false, IsShowPropertyChangedCallback));

        /// <summary>
        /// 是否显示
        /// </summary>
        [Description("获取或设置是否显示")]
        [Category("Defined Properties")]
        public bool IsShow
        {
            get => (bool)GetValue(IsShowProperty);
            set => SetValue(IsShowProperty, value);
        }

        public static void IsShowPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (TipBubble)d;
            if ((bool)e.NewValue)
            {
                if (ctrl.IsAutoHiden)
                    ctrl._sbArr[0]?.Begin();
            }
            else
            {
            }
        }

        public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register("Placement", typeof(PlacementMode), typeof(TipBubble), new UIPropertyMetadata(PlacementMode.Bottom, PlacementPropertyChangedCallback));

        /// <summary>
        /// 显示位置
        /// </summary>
        [Description("获取或设置显示位置")]
        [Category("Defined Properties")]
        public PlacementMode Placement
        {
            get => (PlacementMode)GetValue(PlacementProperty);
            set => SetValue(PlacementProperty, value);
        }

        public static void PlacementPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (TipBubble)d;
            if ((PlacementMode)e.NewValue == PlacementMode.Custom)
            {
            }
            else
            {
            }
        }

        public static readonly DependencyProperty PlacementTargetProperty = DependencyProperty.Register("PlacementTarget", typeof(UIElement), typeof(TipBubble), new UIPropertyMetadata(null, null));

        /// <summary>
        /// 跟随的目标
        /// </summary>
        [Description("获取或设置跟随的目标")]
        [Category("Defined Properties")]
        public UIElement PlacementTarget
        {
            get => (UIElement)GetValue(PlacementTargetProperty);
            set => SetValue(PlacementTargetProperty, value);
        }

        public static readonly DependencyProperty IsAutoHidenProperty = DependencyProperty.Register("IsAutoHiden", typeof(bool), typeof(TipBubble), new UIPropertyMetadata(false, null
            ));

        /// <summary>
        /// 自动隐藏
        /// </summary>
        [Description("获取或设置自动隐藏")]
        [Category("Defined Properties")]
        public bool IsAutoHiden
        {
            get => (bool)GetValue(IsAutoHidenProperty);
            set => SetValue(IsAutoHidenProperty, value);
        }

        private Grid _main;
        private Storyboard[] _sbArr;
        private Popup _popup;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _main = GetTemplateChild("PART_Main") as Grid;
            _popup = GetTemplateChild("PART_Popup") as Popup;
            _popup.CustomPopupPlacementCallback += CustomPopupPlacementChanged;
            if (IsAutoHiden)
                _sbArr = CreateAnimation(_main);
        }

        private Storyboard[] CreateAnimation(FrameworkElement element)
        {
            //var showStory = new Storyboard();
            var hideStory = new Storyboard();
            hideStory.Completed += (s, e) =>
            {
                IsShow = false;
            };
            //var opacityShowAnmiation = new DoubleAnimation();
            //opacityShowAnmiation.BeginTime = new TimeSpan(0, 0, 0, 0, 0);
            //opacityShowAnmiation.Duration = new Duration(TimeSpan.FromSeconds(0.8));
            //opacityShowAnmiation.From = 0;
            //opacityShowAnmiation.To = 0.6;
            //Storyboard.SetTargetProperty(opacityShowAnmiation, new PropertyPath("Opacity"));
            //Storyboard.SetTarget(opacityShowAnmiation, element);
            //showStory.Children.Add(opacityShowAnmiation);

            var opacityHideAnmiation = new DoubleAnimation();
            opacityHideAnmiation.BeginTime = new TimeSpan(0, 0, 0, 2, 500);
            opacityHideAnmiation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            opacityHideAnmiation.From = 1;
            opacityHideAnmiation.To = 0;
            opacityHideAnmiation.FillBehavior = FillBehavior.Stop;
            Storyboard.SetTargetProperty(opacityHideAnmiation, new PropertyPath("Opacity"));
            Storyboard.SetTarget(opacityHideAnmiation, element);
            hideStory.Children.Add(opacityHideAnmiation);

            //showStory?.Begin();

            return new Storyboard[] { hideStory };
        }

        #region Event

        [Description("CustomPlacement触发")]
        public CustomPopupPlacementCallback CustomPopupPlacementChanged
        {
            get; set;
        }

        #endregion Event
    }
}