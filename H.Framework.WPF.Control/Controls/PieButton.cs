using H.Framework.WPF.Control.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace H.Framework.WPF.Control.Controls
{
    //[TemplatePart(Name = "gLegend", Type = typeof(Grid))]
    [TemplatePart(Name = "mPanel", Type = typeof(Canvas))]
    //[TemplatePart(Name = "spLegend", Type = typeof(StackPanel))]
    //默认Width200,Height200
    public class PieButton : System.Windows.Controls.Control
    {
        #region Fields

        private const double LabelSpace = 1;

        private List<Path> _ListPath;

        private static Storyboard _ShowUpStoryboard;

        private static Storyboard _HiddenStoryboard;

        //private bool isActivatedScaleAnimation = true;

        private Point _currentPoint = new Point(-1, -1);

        private Point _prePoint = new Point(-1, -1);

        private Canvas mPanel;

        private readonly List<PointCollection> _ListPoints = new List<PointCollection>();

        #endregion Fields

        #region Constructor

        static PieButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PieButton), new FrameworkPropertyMetadata(typeof(PieButton)));
            ClipToBoundsProperty.OverrideMetadata(typeof(PieButton), new FrameworkPropertyMetadata(false));
        }

        public PieButton()
        {
            //this.DefaultStyleKey = typeof(PieButton);
            _ListPath = new List<Path>();
            _ShowUpStoryboard = new Storyboard();
            _HiddenStoryboard = new Storyboard();
        }

        #endregion Constructor

        #region DependencyProperties

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(PieButton), new PropertyMetadata(null, new PropertyChangedCallback(OnItemsSourcePropertyChanged)));

        /// <summary>
        /// 绑定的数据源
        /// </summary>
        [Description("获取或设置绑定的数据源")]
        [Category("Defined Properties")]
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //PieButton obj = (PieButton)d;
            //obj.CreateControl();
        }

        public static readonly DependencyProperty ShowTextProperty = DependencyProperty.Register("ShowText", typeof(bool), typeof(PieButton), new PropertyMetadata(true, null));

        /// <summary>
        /// 是否ShowText
        /// </summary>
        [Description("获取或设置是否ShowText")]
        [Category("Defined Properties")]
        public bool ShowText
        {
            get { return (bool)GetValue(ShowTextProperty); }
            set { SetValue(ShowTextProperty, value); }
        }

        public static readonly DependencyProperty InnerCircleOpacityProperty = DependencyProperty.Register("InnerCircleOpacity", typeof(double), typeof(PieButton), new PropertyMetadata(0.0, null));

        /// <summary>
        /// InnerCircle透明度
        /// </summary>
        [Description("获取或设置是否InnerCircleOpacity")]
        [Category("Defined Properties")]
        public double InnerCircleOpacity
        {
            get { return (double)GetValue(InnerCircleOpacityProperty); }
            set { SetValue(InnerCircleOpacityProperty, value); }
        }

        public static readonly DependencyProperty InnerCircleShowUpProperty = DependencyProperty.Register("InnerCircleShowUp", typeof(bool), typeof(PieButton), new PropertyMetadata(true, null));

        /// <summary>
        /// 是否Show InnerCircle
        /// </summary>
        [Description("获取或设置是否InnerCircleShowUp")]
        [Category("Defined Properties")]
        public bool InnerCircleShowUp
        {
            get { return (bool)GetValue(InnerCircleShowUpProperty); }
            set { SetValue(InnerCircleShowUpProperty, value); }
        }

        public static readonly DependencyProperty ShowTextSizeProperty = DependencyProperty.Register("ShowTextSize", typeof(int), typeof(PieButton), new PropertyMetadata(15, null));

        /// <summary>
        /// ShowText大小
        /// </summary>
        [Description("获取或设置是否ShowTextSize")]
        [Category("Defined Properties")]
        public int ShowTextSize
        {
            get { return (int)GetValue(ShowTextSizeProperty); }
            set { SetValue(ShowTextSizeProperty, value); }
        }

        public static readonly DependencyProperty ShowUpProperty = DependencyProperty.Register("ShowUp", typeof(bool), typeof(PieButton), new PropertyMetadata(false, new PropertyChangedCallback(OnShowUpPropertyChanged)));

        /// <summary>
        /// 是否ShowUp
        /// </summary>
        [Description("获取或设置是否ShowUp")]
        [Category("Defined Properties")]
        public bool ShowUp
        {
            get { return (bool)GetValue(ShowUpProperty); }
            set { SetValue(ShowUpProperty, value); }
        }

        private static void OnShowUpPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PieButton obj = (PieButton)d;

            if ((bool)e.NewValue)
            {
                //if (obj._List.Count > 0)
                //{
                //    _ShowUpStoryboard.Begin();
                //    return;
                //}
                obj.CreateControl();
                obj.CreateInnerCircleCancel();
                _ShowUpStoryboard.Begin();
                //obj.InvalidateMeasure();
                //obj.InvalidateVisual();
                //obj.InvalidateArrange();
                //obj.UpdateLayout();
            }
            else
            {
                _HiddenStoryboard.Begin();

                Task task = new Task(() =>
                {
                    Thread.Sleep(200);
                    obj.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        obj.ClearControl();
                    }));
                });

                task.Start();
            }
        }

        public static readonly DependencyProperty ItemsCountProperty = DependencyProperty.Register("ItemsCount", typeof(int), typeof(PieButton), new PropertyMetadata(0, null));

        /// <summary>
        /// 数据源的Count
        /// </summary>
        [Description("获取或设置数据源的Count")]
        [Category("Defined Properties")]
        public int ItemsCount
        {
            get { return (int)GetValue(ItemsCountProperty); }
            set { SetValue(ItemsCountProperty, value); }
        }

        public static readonly DependencyProperty ShowLineProperty = DependencyProperty.Register("ShowLine", typeof(bool), typeof(PieButton), new PropertyMetadata(true, null));

        /// <summary>
        /// 数据源的Count
        /// </summary>
        [Description("获取或设置数据源的ShowLine")]
        [Category("Defined Properties")]
        public bool ShowLine
        {
            get { return (bool)GetValue(ShowLineProperty); }
            set { SetValue(ShowLineProperty, value); }
        }

        private static void OnShowLegendPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //PieButton obj = (PieButton)d;
            //obj.BindData();
        }

        public static readonly DependencyProperty DisplayPathProperty = DependencyProperty.Register("DisplayPath", typeof(string), typeof(PieButton), new PropertyMetadata(null, null));

        /// <summary>
        /// Text显示的路径
        /// </summary>
        [Description("获取或设置Text显示的路径")]
        [Category("Defined Properties")]
        public string DisplayPath
        {
            get { return (string)GetValue(DisplayPathProperty); }
            set { SetValue(DisplayPathProperty, value); }
        }

        public static readonly DependencyProperty InnerCircleSizeProperty = DependencyProperty.Register("InnerCircleSize", typeof(int), typeof(PieButton), new PropertyMetadata(20, null));

        /// <summary>
        /// 内圈半径
        /// </summary>
        [Description("获取或设置内圈半径")]
        [Category("Defined Properties")]
        public int InnerCircleSize
        {
            get { return (int)GetValue(InnerCircleSizeProperty); }
            set { SetValue(InnerCircleSizeProperty, value); }
        }

        public static readonly DependencyProperty OutterCircleSizeProperty = DependencyProperty.Register("OutterCircleSize", typeof(int), typeof(PieButton), new PropertyMetadata(100, null));

        /// <summary>
        /// 外圈半径
        /// </summary>
        [Description("获取或设置半径")]
        [Category("Defined Properties")]
        public int OutterCircleSize
        {
            get { return (int)GetValue(OutterCircleSizeProperty); }
            set { SetValue(OutterCircleSizeProperty, value); }
        }

        public static readonly DependencyProperty PaletteProperty = DependencyProperty.Register("Palette", typeof(PaletteCollection), typeof(PieButton), new PropertyMetadata(new PaletteCollection()));

        /// <summary>
        /// 调色板
        /// </summary>
        [Description("获取或设置调色板")]
        [Category("Defined Properties")]
        public PaletteCollection Palette
        {
            get { return (PaletteCollection)GetValue(PaletteProperty); }
            set { SetValue(PaletteProperty, value); }
        }

        public static readonly DependencyProperty SelectedBorderProperty = DependencyProperty.Register("SelectedBorder", typeof(Brush), typeof(PieButton), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        /// <summary>
        /// SelectedBorder颜色
        /// </summary>
        [Description("获取或设置SelectedBorder颜色")]
        [Category("Defined Properties")]
        public Brush SelectedBorder
        {
            get { return (Brush)GetValue(SelectedBorderProperty); }
            set { SetValue(SelectedBorderProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(PieButton), new PropertyMetadata(null, new PropertyChangedCallback(OnSelectedItemPropertyChanged)));

        /// <summary>
        /// 当前选中的记录
        /// </summary>
        [Description("获取或设置当前选中的记录")]
        [Category("Defined Properties")]
        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        private static void OnSelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PieButton obj = (PieButton)d;
            obj.ChangeSelectedItem();
            obj.OnSelectedChanged(e.OldValue, e.NewValue);
        }

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(PieButton), new PropertyMetadata(-1, new PropertyChangedCallback(OnSelectedItemPropertyChanged)));

        /// <summary>
        /// 当前选中的记录Index
        /// </summary>
        [Description("获取或设置当前选中的记录Index")]
        [Category("Defined Properties")]
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        #endregion DependencyProperties

        #region Methods

        /// <summary>
        /// 当前选中的记录触发
        /// </summary>
        private void ChangeSelectedItem()
        {
            if (SelectedIndex != -1)
            {
                foreach (Path path in _ListPath)
                {
                    //SelectedItem = _List[SelectedIndex].Tag;
                    if (path.Tag.Equals(this.SelectedItem))
                    {
                        path.Stroke = SelectedBorder;
                        //Canvas.SetZIndex(path, 20);
                    }
                    else
                    {
                        foreach (Setter setter in path.Style.Setters)
                        {
                            if (setter.Property == Path.StrokeProperty)
                                path.Stroke = (Brush)setter.Value;
                        }
                        Canvas.SetZIndex(path, -1);
                    }
                }
            }
            //if (OnSelected != null) OnSelected(this, EventArgs.Empty);
        }

        /// <summary>
        /// 绑定数据生成控件
        /// </summary>
        private void CreateControl()
        {
            if (ItemsSource == null || ItemsCount == 0)
                return;
            if (Width == double.NaN || Height == 0) return;
            if (mPanel == null) return;

            double transformAngle = -90 - 360 / ItemsCount / 2;
            RotateTransform rotateTransform = new RotateTransform(transformAngle) { CenterX = Width / 2, CenterY = Height / 2 };

            //清空
            mPanel.Children.Clear();
            _ListPath.Clear();
            this.SelectedItem = null;

            var centerPoint = new Point(Width / 2, Height / 2);
            //Win32.POINT screenPos = new Win32.POINT();
            //var endPoint = PointFromScreen(new Point(screenPos.X, screenPos.Y));

            if (ItemsSource != null && !string.IsNullOrEmpty(DisplayPath))
            {
                int paletteIndex = 0;
                //Brush fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xBA, 0xBA, 0xBA));
                //Brush stroke = new SolidColorBrush(Colors.Black);
                Style style = new Style();
                style.Setters.Add(new Setter { Property = Path.FillProperty, Value = new SolidColorBrush(Color.FromArgb(0xFF, 0xBA, 0xBA, 0xBA)) });
                style.Setters.Add(new Setter { Property = Path.StrokeProperty, Value = new SolidColorBrush(Colors.Transparent) });
                int itemCount = 0;
                foreach (object item in ItemsSource)
                {
                    //获取调色板
                    if (Palette != null && Palette.Count > 0)
                    {
                        style = Palette[paletteIndex];
                        //foreach (Setter setter in style.Setters)
                        //{
                        //    if (setter.Property == Path.FillProperty)
                        //        fill = (Brush)setter.Value;
                        //    if (setter.Property == Path.StrokeProperty)
                        //        stroke = (Brush)setter.Value;
                        //}
                        paletteIndex++;
                        if (paletteIndex >= Palette.Count)
                            paletteIndex = 0;
                    }

                    PropertyInfo propertyDisplayText = item.GetType().GetProperty(DisplayPath);

                    //插入块
                    Path path = new Path();
                    path.Name = "piePath";
                    path.Style = style;
                    path.StrokeThickness = 0;
                    //pathFigure.Segments.Add(new ArcSegment
                    //{
                    //    Point = new Point(ActualWidth, ActualHeight / 2),
                    //    IsLargeArc = false,
                    //    Size = new Size(ActualWidth / ItemsCount * 3, ActualHeight / 2),
                    //    SweepDirection = SweepDirection.Clockwise
                    //});

                    //pathFigure.Segments.Add(new LineSegment
                    //{
                    //    Point = new Point(ActualWidth / ItemsCount * 3, ActualHeight / 2)
                    //});

                    //pathFigure.Segments.Add(new ArcSegment
                    //{
                    //    Point = new Point(ActualWidth / 2, ActualHeight / ItemsCount),
                    //    Size = new Size((ActualWidth / 2), 0),
                    //    SweepDirection = SweepDirection.Counterclockwise
                    //});
                    StreamGeometry streamGeometry = new StreamGeometry();
                    int previewPath = 360 / ItemsCount * itemCount;
                    int nextPath = 360 / ItemsCount * (itemCount + 1);
                    PointCollection points = new PointCollection();
                    using (StreamGeometryContext geometryContext = streamGeometry.Open())
                    {
                        for (int i = previewPath; i <= nextPath; i++)
                        {
                            double pathOutterAngle = Math.PI * (i / 180.0);
                            double outterY = Height / 2 + (OutterCircleSize * Math.Sin(pathOutterAngle));
                            double outterX = Width / 2 + (OutterCircleSize * Math.Cos(pathOutterAngle));
                            Point OutterPoint = new Point(outterX, outterY);
                            if (i == previewPath)
                            {
                                geometryContext.BeginFigure(OutterPoint, true, true);
                            }
                            else
                            {
                                points.Add(OutterPoint);
                            }
                        }

                        for (int j = nextPath - 1; j >= previewPath; j--)
                        {
                            double pathInnerAngle = Math.PI * (j / 180.0);
                            double innerY = Height / 2 + (InnerCircleSize * Math.Sin(pathInnerAngle));
                            double innerX = Width / 2 + (InnerCircleSize * Math.Cos(pathInnerAngle));
                            Point innerPoint = new Point(innerX, innerY);
                            points.Add(innerPoint);
                        }
                        geometryContext.PolyLineTo(points, true, true);
                    }
                    _ListPoints.Add(points);
                    itemCount++;
                    path.Opacity = 0;
                    path.Data = streamGeometry;
                    path.Tag = item;
                    TransformGroup group = new TransformGroup();
                    group.Children.Add(rotateTransform);
                    path.RenderTransform = group;
                    DoubleAnimation opacityUpAnmiation = new DoubleAnimation();
                    opacityUpAnmiation.BeginTime = new TimeSpan(0, 0, 0, 0, itemCount * 90 - 20);
                    opacityUpAnmiation.Duration = new Duration(TimeSpan.FromSeconds(0.2));
                    opacityUpAnmiation.From = 0;
                    opacityUpAnmiation.To = 0.95;

                    Storyboard.SetTarget(opacityUpAnmiation, path);
                    Storyboard.SetTargetProperty(opacityUpAnmiation, new PropertyPath("Opacity"));
                    _ShowUpStoryboard.Children.Add(opacityUpAnmiation);

                    DoubleAnimation opacityDownAnmiation = new DoubleAnimation();
                    opacityDownAnmiation.BeginTime = new TimeSpan(0, 0, 0, 0, itemCount * 90 - 20);
                    opacityDownAnmiation.Duration = new Duration(TimeSpan.FromSeconds(0.2));
                    opacityDownAnmiation.From = 0.95;
                    opacityDownAnmiation.To = 0;

                    Storyboard.SetTarget(opacityDownAnmiation, path);
                    Storyboard.SetTargetProperty(opacityDownAnmiation, new PropertyPath("Opacity"));
                    _HiddenStoryboard.Children.Add(opacityDownAnmiation);

                    //path.Visibility = Visibility.Collapsed;

                    //ObjectAnimationUsingKeyFrames visibilityAnimation = new ObjectAnimationUsingKeyFrames();
                    //DiscreteObjectKeyFrame frame = new DiscreteObjectKeyFrame(Visibility.Visible, new TimeSpan(0, 0, 0, 5));
                    //visibilityAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                    //visibilityAnimation.KeyFrames.Add(frame);
                    //visibilityAnimation.BeginTime = new TimeSpan(0, 0, 0, 0, itemCount * 90 - 20);
                    //Storyboard.SetTarget(visibilityAnimation, path);
                    //Storyboard.SetTargetProperty(visibilityAnimation, new PropertyPath("Visibility"));
                    //_ShowUpStoryboard.Children.Add(visibilityAnimation);
                    //DoubleAnimationUsingPath pathAnmiation = new DoubleAnimationUsingPath();
                    //pathAnmiation.BeginTime = new TimeSpan(0, 0, 0, 0, paletteIndex * 150);
                    //pathAnmiation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                    //pathAnmiation.PathGeometry = path.Data.GetFlattenedPathGeometry();

                    //Storyboard.SetTarget(pathAnmiation, path);
                    //Storyboard.SetTargetProperty(pathAnmiation, new PropertyPath("Data.Figures[0].Segments[0].Point"));

                    //sboard.Children.Add(pathAnmiation);
                    //path.MouseEnter += (sender, e) =>
                    //{
                    //    AnimationHelper.ScaleEasingInAnimation(path, isActivatedScaleAnimation);
                    //};

                    //path.MouseLeave += (sender, e) =>
                    //{
                    //    AnimationHelper.ScaleEasingOutAnimation(path);
                    //    isActivatedScaleAnimation = true;
                    //};

                    //path.MouseUp += (sender, e) =>
                    //{
                    //    path_MouseUp(path);
                    //};
                    Panel.SetZIndex(path, 3);
                    mPanel.Children.Add(path);
                    _ListPath.Add(path);

                    //插入文字
                    if (ShowText)
                    {
                        double mAngle = 1 * 360 / ItemsCount + 0;
                        string title = propertyDisplayText.GetValue(item, null).ToString();
                        double cAngle = ((nextPath + previewPath) / 2.0 + transformAngle) % 360;
                        double titleAngle = Math.PI * (cAngle / 180.0);
                        double titleOutterY = Height / 2 + (OutterCircleSize * Math.Sin(titleAngle));
                        double titleOutterX = Width / 2 + (OutterCircleSize * Math.Cos(titleAngle));
                        double titleInnerY = Height / 2 + (InnerCircleSize * Math.Sin(titleAngle));
                        double titleInnerX = Width / 2 + (InnerCircleSize * Math.Cos(titleAngle));
                        double length = Math.Sqrt(Math.Pow(titleOutterX - titleInnerX, 2) + Math.Pow(titleOutterY - titleInnerY, 2));
                        Size titleSize = ComputeRect(title, ShowTextSize);
                        double halfHeight = titleSize.Height / 2;
                        double pers = halfHeight / length + 0.5;
                        Point titlePoint = new Point(titleInnerX + (titleOutterX - titleInnerX) * pers, titleInnerY + (titleOutterY - titleInnerY) * pers);

                        TextBlock textBlock = new TextBlock { Name = "pieBlock", TextWrapping = TextWrapping.NoWrap, TextAlignment = TextAlignment.Center, Text = title, FontSize = ShowTextSize, FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush(Colors.White), Style = null, Opacity = 0 };
                        textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                        textBlock.VerticalAlignment = VerticalAlignment.Top;
                        textBlock.Margin = new Thickness(title.Length < 4 ? titlePoint.X + LabelSpace - 12 : titlePoint.X + LabelSpace - 20, titlePoint.Y + LabelSpace - 8, 0, 0);
                        //if (mAngle <= 90)
                        //{
                        //    //textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                        //    //textBlock.VerticalAlignment = VerticalAlignment.Bottom;
                        //    textBlock.Margin = new Thickness(titlePoint.X + LabelSpace - 12, 0, 0, this.ActualHeight - titlePoint.Y + LabelSpace - 6);
                        //}
                        //else
                        //{
                        //    if (mAngle <= 180)
                        //    {
                        //        //textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                        //        //textBlock.VerticalAlignment = VerticalAlignment.Top;
                        //        textBlock.Margin = new Thickness(titlePoint.X + LabelSpace - 12, titlePoint.Y + LabelSpace - 6, 0, 0);
                        //    }
                        //    else
                        //    {
                        //        if (mAngle <= 270)
                        //        {
                        //            //textBlock.HorizontalAlignment = HorizontalAlignment.Right;
                        //            //textBlock.VerticalAlignment = VerticalAlignment.Top;
                        //            textBlock.Margin = new Thickness(0, titlePoint.Y + LabelSpace - 6, this.ActualWidth - titlePoint.X + LabelSpace + 12, 0);
                        //        }
                        //        else
                        //        {
                        //            //textBlock.HorizontalAlignment = HorizontalAlignment.Right;
                        //            //textBlock.VerticalAlignment = VerticalAlignment.Bottom;
                        //            textBlock.Margin = new Thickness(0, 0, this.ActualWidth + LabelSpace - 12, this.ActualHeight + LabelSpace - 12);
                        //        }
                        //    }
                        //}

                        //textBlock.MouseEnter += (sender, e) =>
                        //{
                        //    AnimationHelper.ScaleEasingInAnimation(path, false);
                        //};

                        //textBlock.MouseLeave += (sender, e) =>
                        //{
                        //    AnimationHelper.ScaleEasingInAnimation(path, false);
                        //    isActivatedScaleAnimation = false;
                        //};

                        //textBlock.MouseUp += (sender, e) =>
                        //{
                        //    path_MouseUp(path);
                        //};

                        DoubleAnimation opacityTextUpAnmiation = new DoubleAnimation();
                        opacityTextUpAnmiation.BeginTime = new TimeSpan(0, 0, 0, 0, itemCount * 90 - 20);
                        opacityTextUpAnmiation.Duration = new Duration(TimeSpan.FromSeconds(0.2));
                        opacityTextUpAnmiation.From = 0;
                        opacityTextUpAnmiation.To = 0.95;
                        Storyboard.SetTarget(opacityTextUpAnmiation, textBlock);
                        Storyboard.SetTargetProperty(opacityTextUpAnmiation, new PropertyPath("Opacity"));
                        _ShowUpStoryboard.Children.Add(opacityTextUpAnmiation);

                        DoubleAnimation opacityTextDownAnmiation = new DoubleAnimation();
                        opacityTextDownAnmiation.BeginTime = new TimeSpan(0, 0, 0, 0, itemCount * 90 - 20);
                        opacityTextDownAnmiation.Duration = new Duration(TimeSpan.FromSeconds(0.2));
                        opacityTextDownAnmiation.From = 0.95;
                        opacityTextDownAnmiation.To = 0;

                        Storyboard.SetTarget(opacityTextDownAnmiation, textBlock);
                        Storyboard.SetTargetProperty(opacityTextDownAnmiation, new PropertyPath("Opacity"));
                        _HiddenStoryboard.Children.Add(opacityTextDownAnmiation);
                        Panel.SetZIndex(textBlock, 6);
                        mPanel.Children.Add(textBlock);
                    }
                }
            }
        }

        private void CreateInnerCircleCancel()
        {
            if (!InnerCircleShowUp || ItemsSource == null || ItemsCount == 0 || mPanel == null)
                return;
            Path cancelEllipse = new Path();
            EllipseGeometry cancelGeometry = new EllipseGeometry();
            cancelGeometry.Center = new Point(Width / 2, Height / 2);
            cancelGeometry.RadiusX = InnerCircleSize;
            cancelGeometry.RadiusY = InnerCircleSize;
            cancelEllipse.Opacity = InnerCircleOpacity;
            cancelEllipse.Data = cancelGeometry;
            cancelEllipse.Fill = new SolidColorBrush(Color.FromRgb(116, 123, 124));
            DoubleAnimation opacityCancelEllipseAnmiation = new DoubleAnimation();
            opacityCancelEllipseAnmiation.BeginTime = new TimeSpan(0, 0, 0, 0, 100);
            opacityCancelEllipseAnmiation.Duration = new Duration(TimeSpan.FromSeconds(0.2));
            opacityCancelEllipseAnmiation.From = InnerCircleOpacity;
            opacityCancelEllipseAnmiation.To = 0;
            Storyboard.SetTarget(opacityCancelEllipseAnmiation, cancelEllipse);
            Storyboard.SetTargetProperty(opacityCancelEllipseAnmiation, new PropertyPath("Opacity"));
            _HiddenStoryboard.Children.Add(opacityCancelEllipseAnmiation);
            Panel.SetZIndex(cancelEllipse, 3);
            mPanel.Children.Add(cancelEllipse);

            Line cancelline1 = new Line();
            cancelline1.X1 = Width / 2 - InnerCircleSize * 0.4;
            cancelline1.Y1 = Height / 2 + InnerCircleSize * 0.4;
            cancelline1.X2 = Width / 2 + InnerCircleSize * 0.4;
            cancelline1.Y2 = Height / 2 - InnerCircleSize * 0.4;
            cancelline1.Stroke = new SolidColorBrush(Colors.White);
            cancelline1.StrokeThickness = 2;
            cancelline1.Opacity = InnerCircleOpacity;
            Storyboard.SetTarget(opacityCancelEllipseAnmiation, cancelline1);
            Storyboard.SetTargetProperty(opacityCancelEllipseAnmiation, new PropertyPath("Opacity"));
            _HiddenStoryboard.Children.Add(opacityCancelEllipseAnmiation);
            Panel.SetZIndex(cancelline1, 3);
            mPanel.Children.Add(cancelline1);

            Line cancelline2 = new Line();
            cancelline2.X1 = Width / 2 - InnerCircleSize * 0.4;
            cancelline2.Y1 = Height / 2 - InnerCircleSize * 0.4;
            cancelline2.X2 = Width / 2 + InnerCircleSize * 0.4;
            cancelline2.Y2 = Height / 2 + InnerCircleSize * 0.4;
            cancelline2.Stroke = new SolidColorBrush(Colors.White);
            cancelline2.StrokeThickness = 2;
            cancelline2.Opacity = InnerCircleOpacity;
            Storyboard.SetTarget(opacityCancelEllipseAnmiation, cancelline2);
            Storyboard.SetTargetProperty(opacityCancelEllipseAnmiation, new PropertyPath("Opacity"));
            _HiddenStoryboard.Children.Add(opacityCancelEllipseAnmiation);
            Panel.SetZIndex(cancelline2, 3);
            mPanel.Children.Add(cancelline2);
        }

        private void CreateLine()
        {
            List<UIElement> list = new List<UIElement>();
            foreach (UIElement element in mPanel.Children)
            {
                if (element is Line)
                {
                    var linePath = element as Line;
                    if (linePath.Name == "arrowLine")
                        list.Add(element);
                }
                else if (element is Path)
                {
                    var ellipsePath = element as Path;
                    if (ellipsePath.Name == "startEllipse" || ellipsePath.Name == "endEllipse")
                        list.Add(element);
                }
            }
            list.ForEach(entity => { mPanel.Children.Remove(entity); });

            Line line = new Line();
            line.Name = "arrowLine";
            line.X1 = Width / 2;
            line.Y1 = Height / 2;
            line.X2 = _currentPoint.X;
            line.Y2 = _currentPoint.Y;
            line.Stroke = new SolidColorBrush(Colors.Orange);
            line.StrokeThickness = 3;
            DoubleAnimation opacityLineEllipseAnmiation = new DoubleAnimation();
            opacityLineEllipseAnmiation.BeginTime = new TimeSpan(0, 0, 0, 0, 100);
            opacityLineEllipseAnmiation.Duration = new Duration(TimeSpan.FromSeconds(0.2));
            opacityLineEllipseAnmiation.From = 0.95;
            opacityLineEllipseAnmiation.To = 0;
            Storyboard.SetTarget(opacityLineEllipseAnmiation, line);
            Storyboard.SetTargetProperty(opacityLineEllipseAnmiation, new PropertyPath("Opacity"));
            _HiddenStoryboard.Children.Add(opacityLineEllipseAnmiation);
            Panel.SetZIndex(line, 9);
            mPanel.Children.Add(line);

            Path startEllipse = new Path();
            startEllipse.Name = "startEllipse";
            EllipseGeometry startGeometry = new EllipseGeometry();
            startGeometry.Center = new Point(ActualWidth / 2, Height / 2);
            startGeometry.RadiusX = 5;
            startGeometry.RadiusY = 5;
            startEllipse.Data = startGeometry;
            startEllipse.Fill = new SolidColorBrush(Colors.Orange);
            Storyboard.SetTarget(opacityLineEllipseAnmiation, startEllipse);
            Storyboard.SetTargetProperty(opacityLineEllipseAnmiation, new PropertyPath("Opacity"));
            _HiddenStoryboard.Children.Add(opacityLineEllipseAnmiation);
            Panel.SetZIndex(startEllipse, 9);
            if (!InnerCircleShowUp || InnerCircleOpacity == 0.0)
                mPanel.Children.Add(startEllipse);

            Path endEllipse = new Path();
            endEllipse.Name = "endEllipse";
            EllipseGeometry endGeometry = new EllipseGeometry();
            endGeometry.Center = _currentPoint;
            endGeometry.RadiusX = 5;
            endGeometry.RadiusY = 5;
            endEllipse.Data = endGeometry;
            endEllipse.Fill = new SolidColorBrush(Colors.Orange);
            Storyboard.SetTarget(opacityLineEllipseAnmiation, endEllipse);
            Storyboard.SetTargetProperty(opacityLineEllipseAnmiation, new PropertyPath("Opacity"));
            _HiddenStoryboard.Children.Add(opacityLineEllipseAnmiation);
            Panel.SetZIndex(endEllipse, 9);
            mPanel.Children.Add(endEllipse);
        }

        private void ClearControl()
        {
            //清空
            mPanel.Children.Clear();
            //_ListPath.Clear();
            //this.SelectedItem = null;
        }

        //计算title的size,size的width和height是椭圆的半径,详见http://msdn.microsoft.com/zh-cn/library/system.windows.media.arcsegment.size%28v=vs.95%29.aspx
        private Size ComputeRect(string message, double fontSize)
        {
            FormattedText formattedText = CreateTextFormat(message, Colors.Black, fontSize, FlowDirection.LeftToRight);
            double width = formattedText.Width;
            double height = formattedText.Height;
            Size size = new Size(width, height);
            return size;
        }

        private FormattedText CreateTextFormat(string text, Color color, double fontSize, FlowDirection flowDirection)
        {
            if (text == null)
            {
                return null;
            }
            FormattedText formattedText = new FormattedText(text, CultureInfo.CurrentCulture, flowDirection, new Typeface("思源黑体 CN Bold"), fontSize, new SolidColorBrush(color), VisualTreeHelper.GetDpi(this).PixelsPerDip);
            formattedText.SetFontWeight(FontWeights.Bold);
            formattedText.TextAlignment = TextAlignment.Center;
            return formattedText;
        }

        private int GetMouseInButtonIndex(Point mouseLocation)
        {
            double transformAngle = -90 - 360 / ItemsCount / 2;
            Point mp = Tp(mouseLocation, -transformAngle);
            for (int i = 0; i < _ListPoints.Count; i++)
            {
                if (Ps(_ListPoints[i], mp))
                {
                    return i;
                }
            }
            return -1;
        }

        private bool Ps(PointCollection points, Point point)
        {
            if (ShowLine)
            {
                int i, j = points.Count - 1;
                bool ps = false;

                double cx = Width / 2;
                double cy = Height / 2;
                double l = GL(cx, cy, _currentPoint.X, _currentPoint.Y);
                if (l < InnerCircleSize)
                {
                    return false;
                }

                double x = cx + (point.X - cx) * (InnerCircleSize + 2) / l;
                double y = cy + (point.Y - cy) * (InnerCircleSize + 2) / l;

                for (i = 0; i < points.Count; i++)
                {
                    Point pi = points[i];
                    Point pj = points[j];
                    if (((pi.Y < y && pj.Y >= y) || (pj.Y < y && pi.Y >= y)) && (pi.X <= x || pj.X <= x))
                    {
                        ps ^= (pi.X + (y - pi.Y) / (pj.Y - pi.Y) * (pj.X - pi.X) < x);
                    }
                    j = i;
                }
                return ps;
            }
            else
            {
                int i, j = points.Count - 1;
                bool ps = false;
                double x = point.X;
                double y = point.Y;
                for (i = 0; i < points.Count; i++)
                {
                    Point pi = points[i];
                    Point pj = points[j];
                    if (((pi.Y < y && pj.Y >= y) || (pj.Y < y && pi.Y >= y)) && (pi.X <= x || pj.X <= x))
                    {
                        ps ^= (pi.X + (y - pi.Y) / (pj.Y - pi.Y) * (pj.X - pi.X) < x);
                    }
                    j = i;
                }
                return ps;
            }
        }

        private Point Tp(Point p, double a)
        {
            Point cp = new Point(Width / 2, Height / 2);
            double r = a * Math.PI / 180.0;
            double x = (p.X - cp.X) * Math.Cos(r) - (p.Y - cp.Y) * Math.Sin(r) + cp.X;
            double y = (p.Y - cp.Y) * Math.Cos(r) + (p.X - cp.X) * Math.Sin(r) + cp.Y;
            return new Point(x, y);
        }

        private double GL(double x1, double y1, double x2, double y2)
        {
            double x = Math.Abs(x2 - x1);
            double y = Math.Abs(y2 - y1);
            double result = Math.Sqrt(x * x + y * y);
            return result;
        }

        private void ExecutePathAnimation(Path path)
        {
            if (path.RenderTransform is TransformGroup)
            {
                TransformGroup group = path.RenderTransform as TransformGroup;
                var item = group.Children.SingleOrDefault(entity => entity is ScaleTransform);
                if (item != null)
                {
                    var scaleItem = item as ScaleTransform;
                    if (scaleItem.ScaleX > 1 && scaleItem.ScaleY > 1)
                    {
                        return;
                    }
                }
                path.ScaleEasingInAnimation();
                _ListPath.ForEach(entity => { if (entity != path) entity.ScaleEasingOutAnimation(); });
            }
        }

        #endregion Methods

        #region OverrideMethods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            mPanel = (Canvas)GetTemplateChild("mPanel");
            if (ShowUp)
            {
                CreateControl();
                CreateInnerCircleCancel();
                _ShowUpStoryboard.Begin();
            }
            //CreateControl();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            //DrawText(drawingContext);
            //DrawTrackLine(drawingContext);
        }

        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (ItemsSource == null || ItemsCount == 0)
                return;
            if (e.GetPosition(this) != _prePoint)
            {
                _currentPoint = e.GetPosition(this);
                if (ShowUp)
                    CreateLine();
                SelectedIndex = GetMouseInButtonIndex(e.GetPosition(this));
                if (SelectedIndex == -1)
                    return;
                if (_ListPath.Count - SelectedIndex <= 0)
                    return;
                ExecutePathAnimation(_ListPath.ElementAt(SelectedIndex));
            }
            _prePoint = e.GetPosition(this);
        }

        protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (ShowLine)
                CaptureMouse();
        }

        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (ItemsSource == null || ItemsCount == 0)
                return;
            if (ShowLine)
                ReleaseMouseCapture();
            SelectedIndex = GetMouseInButtonIndex(e.GetPosition(this));
            ShowUp = false;
            if (SelectedIndex == -1)
                return;
            if (_ListPath.Count - SelectedIndex <= 0)
                SelectedIndex = 0;
            SelectedItem = _ListPath.ElementAt(SelectedIndex).Tag;
        }

        #endregion OverrideMethods

        #region Event

        public static readonly RoutedEvent SelectedChangedEvent = EventManager.RegisterRoutedEvent("SelectedChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Object>), typeof(PieButton));

        [Description("选择项后触发")]
        public event RoutedPropertyChangedEventHandler<Object> SelectedChanged
        {
            add
            {
                this.AddHandler(SelectedChangedEvent, value);
            }
            remove
            {
                this.RemoveHandler(SelectedChangedEvent, value);
            }
        }

        protected virtual void OnSelectedChanged(Object oldValue, Object newValue)
        {
            RoutedPropertyChangedEventArgs<Object> arg =
                new RoutedPropertyChangedEventArgs<Object>(oldValue, newValue, SelectedChangedEvent);
            this.RaiseEvent(arg);
        }

        #endregion Event
    }

    public class PaletteCollection : Collection<Style>
    {
    }
}