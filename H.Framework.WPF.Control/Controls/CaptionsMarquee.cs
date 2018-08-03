using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace H.Framework.WPF.Control.Controls
{
    public class CaptionsMarquee : ContentControl
    {
        #region Dependency Properties

        //#region Loop

        ///// <summary>
        ///// Gets/Sets The loop of the animation. Zero represents forever
        ///// </summary>
        //public int Loop
        //{
        //    get { return (int)GetValue(LoopProperty); }
        //    set { SetValue(LoopProperty, value); }
        //}

        //public static readonly DependencyProperty LoopProperty = DependencyProperty.Register(
        //    "Loop",
        //    typeof(int),
        //    typeof(CaptionsMarquee),
        //    new FrameworkPropertyMetadata(1, OnCaptionsMarqueeParameterChanged, OnCoerceLoop));

        //private static object OnCoerceLoop(DependencyObject sender, object value)
        //{
        //    int loop = (int)value;
        //    if (loop < 0)
        //    {
        //        throw new ArgumentException("Loop必须大于等于0");
        //    }
        //    return value;
        //}

        //#endregion Loop

        #region Direction

        /// <summary>
        /// Gets/Sets the direction of the animation.
        /// </summary>
        public Direction Direction
        {
            get { return (Direction)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(
            "Direction",
            typeof(Direction),
            typeof(CaptionsMarquee),
            new FrameworkPropertyMetadata(Direction.Right, OnCaptionsMarqueeParameterChanged));

        #endregion Direction

        #region Behavior

        /// <summary>
        /// Gets/Sets The animation behavior.
        /// </summary>
        public CaptionsMarqueeBehavior Behavior
        {
            get { return (CaptionsMarqueeBehavior)GetValue(BehaviorProperty); }
            set { SetValue(BehaviorProperty, value); }
        }

        public static readonly DependencyProperty BehaviorProperty = DependencyProperty.Register(
            "Behavior",
            typeof(CaptionsMarqueeBehavior),
            typeof(CaptionsMarquee),
            new FrameworkPropertyMetadata(CaptionsMarqueeBehavior.Scroll, OnCaptionsMarqueeParameterChanged));

        #endregion Behavior

        #region Action

        /// <summary>
        /// Gets/Sets The animation behavior.
        /// </summary>
        public CaptionsMarqueeAction Action
        {
            get { return (CaptionsMarqueeAction)GetValue(ActionProperty); }
            set { SetValue(ActionProperty, value); }
        }

        public static readonly DependencyProperty ActionProperty = DependencyProperty.Register(
            "Action",
            typeof(CaptionsMarqueeAction),
            typeof(CaptionsMarquee),
            new FrameworkPropertyMetadata(CaptionsMarqueeAction.Loop, OnCaptionsMarqueeParameterChanged));

        #endregion Action

        #region ScrollAmount

        /// <summary>
        /// Gets/Sets The offset each time the content moves
        /// </summary>
        public double ScrollAmount
        {
            get { return (double)GetValue(ScrollAmountProperty); }
            set { SetValue(ScrollAmountProperty, value); }
        }

        public static readonly DependencyProperty ScrollAmountProperty = DependencyProperty.Register(
            "ScrollAmount",
            typeof(double),
            typeof(CaptionsMarquee),
            new FrameworkPropertyMetadata(10.0, OnCaptionsMarqueeParameterChanged, OnCoerceScrollAmount));

        private static object OnCoerceScrollAmount(DependencyObject sender, object value)
        {
            double amount = (double)value;
            if (amount - double.Epsilon <= 0.0)
            {
                throw new ArgumentException("ScrollAmount必须大于0");
            }
            return value;
        }

        #endregion ScrollAmount

        #region ScrollDelay

        /// <summary>
        /// Gets/Sets The time delay between each movement.
        /// </summary>
        public double ScrollDelay
        {
            get { return (double)GetValue(ScrollDelayProperty); }
            set { SetValue(ScrollDelayProperty, value); }
        }

        public static readonly DependencyProperty ScrollDelayProperty = DependencyProperty.Register(
            "ScrollDelay",
            typeof(double),
            typeof(CaptionsMarquee),
            new FrameworkPropertyMetadata(100.0, OnCaptionsMarqueeParameterChanged, OnCoerceScrollDelay));

        private static object OnCoerceScrollDelay(DependencyObject sender, object value)
        {
            double delay = (double)value;
            if (delay - double.Epsilon <= 0.0)
            {
                throw new ArgumentException("ScrollDelay必须大于0");
            }
            return value;
        }

        #endregion ScrollDelay

        private static void OnCaptionsMarqueeParameterChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CaptionsMarquee CaptionsMarquee = sender as CaptionsMarquee;
            if (CaptionsMarquee != null)
            {
                CaptionsMarquee.ResetAnimation();
            }
        }

        #endregion Dependency Properties

        #region Fields

        private ContentPresenter _contentHost = null;
        private DispatcherTimer _timer = null;
        private TranslateTransform _contentTranlate = null;

        // 用于Loop的增量计算
        //private int _currentLoop = -1;
        //private int _loopCounter = 1;

        // 当前的Direction
        private Direction _currentDirection;

        #endregion Fields

        #region Initialization

        static CaptionsMarquee()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CaptionsMarquee), new FrameworkPropertyMetadata(typeof(CaptionsMarquee)));
            ClipToBoundsProperty.OverrideMetadata(typeof(CaptionsMarquee), new FrameworkPropertyMetadata(false));
        }

        public CaptionsMarquee()
        {
            Background = new SolidColorBrush(Colors.Transparent);

            this.Loaded += new RoutedEventHandler(CaptionsMarquee_Loaded);
            this.Unloaded += new RoutedEventHandler(CaptionsMarquee_Unloaded);
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(Timer_Tick);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.OnUpdateContentPosition();
        }

        private void CaptionsMarquee_Loaded(object sender, RoutedEventArgs e)
        {
            Width = Content is string ? Content.ToString().Length * 12 : ((System.Windows.Controls.Control)Content).Width;
            if (Action == CaptionsMarqueeAction.Loop)
                _timer.Start();
        }

        private void CaptionsMarquee_Unloaded(object sender, RoutedEventArgs e)
        {
            if (Action == CaptionsMarqueeAction.Loop)
                _timer.Stop();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _contentHost = this.GetTemplateChild("PART_ContentHost") as ContentPresenter;
            ResetAnimation();
        }

        #endregion Initialization

        #region Logic for Animation

        #region Override Events

        // When size changed, reset animation
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            this.ResetAnimation();
        }

        protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (Action == CaptionsMarqueeAction.MouseOverLoop)
                _timer.Start();
            else if (Action == CaptionsMarqueeAction.MouseOver)
            { ResetAnimation(); _timer.Start(); }
        }

        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (Action == CaptionsMarqueeAction.MouseOverLoop)
                _timer.Stop();
            else if (Action == CaptionsMarqueeAction.MouseOver)
            {
                _timer.Stop();
                _contentHost.RenderTransform = new TranslateTransform() { X = 0 };
            }
        }

        #endregion Override Events

        #region Methods

        // Clear current animation, and restore content's position
        private void ResetAnimation()
        {
            if (_contentHost != null)
            {
                // 无限循环
                //if (this.Loop == 0)
                //{
                //_currentLoop = -1;
                //_loopCounter = 0;
                //}
                //else
                //{
                //    _currentLoop = 0;
                //    _loopCounter = 1;
                //}

                _timer.Interval = TimeSpan.FromMilliseconds(this.ScrollDelay);
                _currentDirection = this.Direction;

                if (this.Direction == Direction.Left)
                {
                    // 放到最右边
                    _contentTranlate = new TranslateTransform(this.ActualWidth, 0.0);
                }
                else if (this.Direction == Direction.Right)
                {
                    // 放到最左边
                    _contentTranlate = new TranslateTransform();
                }
                _contentHost.RenderTransform = _contentTranlate;
            }
        }

        // Update position of the content
        protected virtual void OnUpdateContentPosition()
        {
            if (_contentHost != null)
            {
                double contentWidth = _contentHost.DesiredSize.Width;
                double leftBound = 0.0, rightBound = 0.0;

                // Alternate不飞出边界
                if (this.Behavior == CaptionsMarqueeBehavior.Alternate || this.Behavior == CaptionsMarqueeBehavior.Slide)
                {
                    leftBound = 0;
                    rightBound = this.ActualWidth - contentWidth;
                }
                // Scroll要飞出边界
                else if (this.Behavior == CaptionsMarqueeBehavior.Scroll)
                {
                    leftBound = -contentWidth;
                    rightBound = this.ActualWidth;
                }

                // 循环次数的计数
                //if (_currentLoop < this.Loop)
                //{
                // 计算位移
                if (_currentDirection == Direction.Left)
                {
                    _contentTranlate.X -= this.ScrollAmount;
                    // 从右往左到头了
                    if (_contentTranlate.X <= leftBound)
                    {
                        if (this.Behavior == CaptionsMarqueeBehavior.Scroll || this.Behavior == CaptionsMarqueeBehavior.Slide)
                            _contentTranlate.X = rightBound;
                        else if (this.Behavior == CaptionsMarqueeBehavior.Alternate)
                            _currentDirection = Direction.Right;
                    }
                }
                else if (_currentDirection == Direction.Right)
                {
                    _contentTranlate.X += this.ScrollAmount;
                    // 从左往右到头了
                    if (_contentTranlate.X >= rightBound)
                    {
                        if (this.Behavior == CaptionsMarqueeBehavior.Scroll || this.Behavior == CaptionsMarqueeBehavior.Slide)
                            _contentTranlate.X = leftBound;
                        else if (this.Behavior == CaptionsMarqueeBehavior.Alternate)
                            _currentDirection = Direction.Left;
                    }
                }
                //_currentLoop += _loopCounter;
                //}// End of Loop
                else
                {
                    // 保证动画结束后可见
                    if (_contentTranlate.X < 0)
                        _contentTranlate.X = 0;
                    if (_contentTranlate.X > this.ActualWidth - contentWidth)
                        _contentTranlate.X = this.ActualWidth - contentWidth;
                }
            }
        }

        #endregion Methods

        #endregion Logic for Animation
    }

    public enum Direction
    {
        Left,
        Right
    }

    public enum CaptionsMarqueeBehavior
    {
        Scroll,
        Slide,
        Alternate,
    }

    public enum CaptionsMarqueeAction
    {
        Loop,
        MouseOverLoop,
        MouseOver
    }
}