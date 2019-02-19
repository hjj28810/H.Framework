using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace H.Framework.WPF.Control.Controls
{
    [TemplatePart(Name = "PART_Main", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_BottomTriangle", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_TopTriangle", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_LeftTriangle", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_RightTriangle", Type = typeof(Grid))]
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
                    if (ctrl._sbArr.Length > 0)
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
            //if (ctrl._topTriangle != null && ctrl._bottomTriangle != null)
            //    switch ((PlacementMode)e.NewValue)
            //    {
            //        case PlacementMode.Bottom:
            //            ctrl._topTriangle.Visibility = Visibility.Visible;
            //            ctrl._bottomTriangle.Visibility = Visibility.Hidden;
            //            break;

            //        case PlacementMode.Top:
            //            ctrl._topTriangle.Visibility = Visibility.Hidden;
            //            ctrl._bottomTriangle.Visibility = Visibility.Visible;
            //            break;

            //        default:
            //            ctrl._topTriangle.Visibility = Visibility.Visible;
            //            ctrl._bottomTriangle.Visibility = Visibility.Hidden;
            //            break;
            //    }
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

        public static readonly DependencyProperty IsAutoHidenProperty = DependencyProperty.Register("IsAutoHiden", typeof(bool), typeof(TipBubble), new UIPropertyMetadata(false, null));

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

        public static readonly DependencyProperty ShadowColorProperty = DependencyProperty.Register("ShadowColor", typeof(Brush), typeof(TipBubble), new UIPropertyMetadata(new SolidColorBrush(Colors.Blue), null));

        /// <summary>
        /// 阴影颜色
        /// </summary>
        [Description("获取或设置阴影颜色")]
        [Category("Defined Properties")]
        public Brush ShadowColor
        {
            get => (Brush)GetValue(ShadowColorProperty);
            set => SetValue(ShadowColorProperty, value);
        }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(TipBubble), new UIPropertyMetadata(null, null));

        /// <summary>
        /// 内容
        /// </summary>
        [Description("获取或设置内容")]
        [Category("Defined Properties")]
        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(TipBubble), new UIPropertyMetadata(null));

        /// <summary>
        /// 内容模板
        /// </summary>
        [Description("获取或设置内容模板")]
        [Category("Defined Properties")]
        public DataTemplate ContentTemplate
        {
            get => (DataTemplate)GetValue(ContentTemplateProperty);
            set => SetValue(ContentTemplateProperty, value);
        }

        //private StackPanel _main;
        private Grid _main, _bottomTriangle, _topTriangle, _leftTriangle, _rightTriangle;

        private Storyboard[] _sbArr;
        private Popup _popup;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _main = GetTemplateChild("PART_Main") as Grid;
            _bottomTriangle = GetTemplateChild("PART_BottomTriangle") as Grid;
            _topTriangle = GetTemplateChild("PART_TopTriangle") as Grid;
            _leftTriangle = GetTemplateChild("PART_LeftTriangle") as Grid;
            _rightTriangle = GetTemplateChild("PART_RightTriangle") as Grid;
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