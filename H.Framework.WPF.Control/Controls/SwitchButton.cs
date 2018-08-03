using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace H.Framework.WPF.Control.Controls
{
    public class SwitchButton : Canvas
    {
        private double _outSize = 60;

        /// <summary>
        /// 设置外圈圆半径大小
        /// </summary>
        public double OutSize
        {
            set
            {
                if (value <= 0)
                {
                    return;
                }
                _outSize = value;
                Width = _outSize;
                Height = _outSize;
                InvalidateVisual();
            }
            get { return _outSize; }
        }

        private double _innerCircleSize = 4;

        /// <summary>
        /// 内圈圆的半径大小
        /// </summary>
        public double InnerCircleSize
        {
            set
            {
                if (value <= 0)
                {
                    return;
                }
                if (value >= _outSize)
                {
                    return;
                }
                _innerCircleSize = value;
                InvalidateVisual();
            }
            get { return _innerCircleSize; }
        }

        private List<String> _buttonValues = new List<string> { "CLC", "RDC", "TXI" };

        /// <summary>
        /// 按钮的值
        /// </summary>
        public List<String> ButtonValues
        {
            set { _buttonValues = value; InvalidateVisual(); }
            get { return _buttonValues; }
        }

        private double _fontSize = 18;

        public double FontSize
        {
            set
            {
                if (value < 0)
                {
                    return;
                }
                _fontSize = value;
                InvalidateVisual();
            }
            get { return _fontSize; }
        }

        private int _defaultButtonIndex = -1;

        /// <summary>
        /// 默认的按钮，如果为-1则为无默认值
        /// </summary>
        public int DefaultButtonIndex
        {
            set
            {
                if (value < -1 || value > _buttonValues.Count - 1)
                {
                    return;
                }
                _defaultButtonIndex = value;
                InvalidateVisual();
            }
            get { return _defaultButtonIndex; }
        }

        private double _buttonBorderWidth = 1;

        /// <summary>
        /// 按钮边框粗细
        /// </summary>
        public double ButtonBorderWidth
        {
            set
            {
                if (value < 0)
                {
                    return;
                }
                _buttonBorderWidth = value;
                InvalidateVisual();
            }
            get { return _buttonBorderWidth; }
        }

        private string _planeNo = "";

        /// <summary>
        /// 航班号
        /// </summary>
        public string PlaneNo
        {
            set { _planeNo = value; }
            get { return _planeNo; }
        }

        #region Default Style

        private Color _defaultButtonColor = Color.FromScRgb(1.0f, 51.0f / 255.0f, 241.0f / 255.0f, 261.0f / 255.0f);

        /// <summary>
        /// 默认按钮的颜色
        /// </summary>
        public Color DefaultButtonColor
        {
            set { _defaultButtonColor = value; InvalidateVisual(); }
            get { return _defaultButtonColor; }
        }

        private Color _defaultButtonTextColor = Colors.White;

        /// <summary>
        /// 默认按钮文字颜色
        /// </summary>
        public Color DefaultButtonTextColor
        {
            set
            {
                _defaultButtonTextColor = value;
                InvalidateVisual();
            }
            get { return _defaultButtonTextColor; }
        }

        private Color _defaultButtonBorderColor = Color.FromScRgb(1.0f, 176.0f / 255.0f, 176 / 255.0f, 176.0f / 255.0f);

        /// <summary>
        /// 默认按钮的边框颜色
        /// </summary>
        public Color DefaultButtonBorderColor
        {
            set { _defaultButtonBorderColor = value; InvalidateVisual(); }
            get { return _defaultButtonBorderColor; }
        }

        private Color _defaultButtonSelectColor = Colors.DeepSkyBlue;

        /// <summary>
        /// 默认按钮选择后颜色
        /// </summary>
        public Color DefaultButtonSelectColor
        {
            set
            {
                _defaultButtonSelectColor = value;
                InvalidateVisual();
            }
            get { return _defaultButtonSelectColor; }
        }

        private Color _defaultButtonSelectTextColor = Colors.WhiteSmoke;

        /// <summary>
        /// 默认按钮选择后文字颜色
        /// </summary>
        public Color DefaultButtonSelectTextColor
        {
            set { _defaultButtonSelectTextColor = value; InvalidateVisual(); }
            get { return _defaultButtonSelectTextColor; }
        }

        #endregion Default Style

        private Color _buttonColor = Color.FromScRgb(1.0f, 245.0f / 255.0f, 245.0f / 255.0f, 245.0f / 255.0f);

        /// <summary>
        /// 非默认按钮的颜色
        /// </summary>
        public Color ButtonColor
        {
            set { _buttonColor = value; InvalidateVisual(); }
            get { return _buttonColor; }
        }

        private Color _buttonBorderColor = Color.FromScRgb(1.0f, 176.0f / 255.0f, 176 / 255.0f, 176.0f / 255.0f);

        /// <summary>
        /// 非默认按钮的边框颜色
        /// </summary>
        public Color ButtonBorderColor
        {
            set { _buttonBorderColor = value; InvalidateVisual(); }
            get { return _buttonBorderColor; }
        }

        private Color _buttonTextColor = Color.FromScRgb(1.0f, 25.0f / 255.0f, 25 / 255.0f, 25.0f / 255.0f);

        /// <summary>
        /// 非默认按钮的文字颜色
        /// </summary>
        public Color ButtonTextColor
        {
            set { _buttonTextColor = value; InvalidateVisual(); }
            get { return _buttonTextColor; }
        }

        private Color _buttonSelectTextColor = Colors.WhiteSmoke;

        /// <summary>
        /// 非默认按钮的选择后的文字颜色
        /// </summary>
        public Color ButtonSelectTextColor
        {
            set { _buttonSelectTextColor = value; InvalidateVisual(); }
            get { return _buttonSelectTextColor; }
        }

        private Color _buttonSelectColor = Colors.DeepSkyBlue;

        /// <summary>
        /// 非默认按钮的选择颜色
        /// </summary>
        public Color ButtonSelectColor
        {
            set { _buttonSelectColor = value; InvalidateVisual(); }
            get { return _buttonSelectColor; }
        }

        private int _splitButtons = 5;

        /// <summary>
        /// 分割的个数
        /// </summary>
        public int SpliteButtons
        {
            set
            {
                if (value < 2)
                {
                    return;
                }
                if (value > 6)
                {
                    return;
                }
                _splitButtons = value;
                InvalidateVisual();
            }
            get { return _splitButtons; }
        }

        private int _currentButtonCount;

        public int CurrentButtonCount
        {
            set { _currentButtonCount = value; }
            get { return _currentButtonCount; }
        }

        private double _selectZoomScale = 1.25;

        public double SelectZoomScale
        {
            set { _selectZoomScale = value; }
            get { return _selectZoomScale; }
        }

        private bool _useButtonValuesForSplit;

        /// <summary>
        /// 是否使用ButtonValues列表的值的个数作为分割的依据，如果为true，SpliteButtons则不起作用
        /// </summary>
        public bool UseButtonValuesForSplit
        {
            set
            {
                if (_buttonValues.Count < 2)
                {
                    _useButtonValuesForSplit = false;
                }
                _useButtonValuesForSplit = value;
                InvalidateVisual();
            }
            get { return _useButtonValuesForSplit; }
        }

        private bool _showTrackLine = true;

        public bool ShowTrackLine
        {
            set { _showTrackLine = value; }
            get { return _showTrackLine; }
        }

        private Color _trackLineColor = Colors.Orange;

        public Color TrackLineColor
        {
            set { _trackLineColor = value; }
            get { return _trackLineColor; }
        }

        private double _trackLineWidth = 2;

        public double TrackLineWidth
        {
            set { _trackLineWidth = value; }
            get { return _trackLineWidth; }
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            _currentButtonCount = UseButtonValuesForSplit ? _buttonValues.Count : _splitButtons;
            DrawButtons(dc, _currentButtonCount);
        }

        private int _selectIndex = -1;
        private readonly List<PointCollection> _allButtons = new List<PointCollection>();
        private bool _hasAdded;

        private void DrawButtons(DrawingContext dc, int count)
        {
            double transformAngle = -90 - 360 / count / 2;
            RotateTransform tf = new RotateTransform(transformAngle) { CenterX = Width / 2, CenterY = Height / 2 };

            dc.PushTransform(tf);
            for (int j = 0; j < count; j++)
            {
                StreamGeometry streamGeometry = new StreamGeometry();
                PointCollection points = new PointCollection();
                int preP = 360 / count * j;
                int toP = 360 / count * (j + 1);
                using (StreamGeometryContext geometryContext = streamGeometry.Open())
                {
                    for (int i = preP; i <= toP; i++)
                    {
                        double theta = Math.PI * (i / 180.0);
                        double oy, ox;
                        if (j == _selectIndex)
                        {
                            oy = Height / 2 + (_outSize * _selectZoomScale * Math.Sin(theta));
                            ox = Width / 2 + (_outSize * _selectZoomScale * Math.Cos(theta));
                        }
                        else
                        {
                            oy = Height / 2 + (_outSize * Math.Sin(theta));
                            ox = Width / 2 + (_outSize * Math.Cos(theta));
                        }

                        Point p = new Point(ox, oy);
                        if (i == preP)
                        {
                            geometryContext.BeginFigure(p, true, true);
                        }
                        else
                        {
                            points.Add(p);
                        }
                    }

                    for (int k = toP - 1; k >= preP; k--)
                    {
                        double theta = Math.PI * (k / 180.0);
                        double iy = Height / 2 + (_innerCircleSize * Math.Sin(theta));
                        double ix = Width / 2 + (_innerCircleSize * Math.Cos(theta));
                        Point p = new Point(ix, iy);
                        points.Add(p);
                    }
                    geometryContext.PolyLineTo(points, true, true);
                }
                if (!_hasAdded)
                {
                    _allButtons.Add(points);
                }
                if (_selectIndex == j)
                {
                    if (j == _defaultButtonIndex)
                    {
                        dc.DrawGeometry(new SolidColorBrush(DefaultButtonSelectColor), new Pen(new SolidColorBrush(DefaultButtonBorderColor), ButtonBorderWidth), streamGeometry);
                    }
                    else
                    {
                        dc.DrawGeometry(new SolidColorBrush(ButtonSelectColor), new Pen(new SolidColorBrush(ButtonBorderColor), ButtonBorderWidth), streamGeometry);
                    }
                }
                else
                {
                    if (j == _defaultButtonIndex)
                    {
                        dc.DrawGeometry(new SolidColorBrush(DefaultButtonColor), new Pen(new SolidColorBrush(DefaultButtonBorderColor), ButtonBorderWidth), streamGeometry);
                    }
                    else
                    {
                        dc.DrawGeometry(new SolidColorBrush(ButtonColor), new Pen(new SolidColorBrush(ButtonBorderColor), ButtonBorderWidth), streamGeometry);
                    }
                }
            }
            dc.Pop();
            for (int i = 0; i < count; i++)
            {
                if (i >= _buttonValues.Count)
                {
                    break;
                }
                int preP = 360 / count * i;
                int toP = 360 / count * (i + 1);
                double ca = ((toP + preP) / 2.0 + transformAngle) % 360;
                double theta = Math.PI * (ca / 180.0);
                double oy, ox;
                if (_selectIndex == i)
                {
                    oy = Height / 2 + (_outSize * _selectZoomScale * Math.Sin(theta));
                    ox = Width / 2 + (_outSize * _selectZoomScale * Math.Cos(theta));
                }
                else
                {
                    oy = Height / 2 + (_outSize * Math.Sin(theta));
                    ox = Width / 2 + (_outSize * Math.Cos(theta));
                }
                double iy = Height / 2 + (_innerCircleSize * Math.Sin(theta));
                double ix = Width / 2 + (_innerCircleSize * Math.Cos(theta));
                double length = Math.Sqrt(Math.Pow(ox - ix, 2) + Math.Pow(oy - iy, 2));
                Size s = ComputeRect(_buttonValues[i], _fontSize);
                double fh = s.Height / 2;
                double pers = fh / length + 0.5;
                Point cp = new Point(ix + (ox - ix) * pers, iy + (oy - iy) * pers);
                RotateTransform tf1 = new RotateTransform(ca + 90) { CenterX = cp.X, CenterY = cp.Y };
                dc.PushTransform(tf1);
                if (_selectIndex == i)
                {
                    if (i == _defaultButtonIndex)
                    {
                        if (i < _buttonValues.Count)
                        {
                            dc.DrawText(CreateTextFormat(_buttonValues[i], DefaultButtonSelectTextColor, _fontSize + 3, FlowDirection.LeftToRight), cp);
                        }
                    }
                    else
                    {
                        if (i < _buttonValues.Count)
                        {
                            dc.DrawText(CreateTextFormat(_buttonValues[i], ButtonSelectTextColor, _fontSize + 3, FlowDirection.LeftToRight), cp);
                        }
                    }
                }
                else
                {
                    if (i == _defaultButtonIndex)
                    {
                        if (i < _buttonValues.Count)
                        {
                            dc.DrawText(CreateTextFormat(_buttonValues[i], DefaultButtonTextColor, _fontSize, FlowDirection.LeftToRight), cp);
                        }
                    }
                    else
                    {
                        if (i < _buttonValues.Count)
                        {
                            dc.DrawText(CreateTextFormat(_buttonValues[i], ButtonTextColor, _fontSize, FlowDirection.LeftToRight), cp);
                        }
                    }
                }
                dc.Pop();
            }
            _hasAdded = true;

            DrawTrackLine(dc);
        }

        private void DrawTrackLine(DrawingContext dc)
        {
            if (ShowTrackLine)
            {
                dc.DrawEllipse(new SolidColorBrush(_trackLineColor), new Pen(new SolidColorBrush(_trackLineColor), _trackLineWidth), new Point(Width / 2, Height / 2), null, 3.5, null, 3.5, null);
                dc.DrawLine(new Pen(new SolidColorBrush(_trackLineColor), _trackLineWidth), new Point(Width / 2, Height / 2), null, _currentPoint, null);
                dc.DrawEllipse(new SolidColorBrush(_trackLineColor), new Pen(new SolidColorBrush(_trackLineColor), _trackLineWidth), _currentPoint, null, 3.5, null, 3.5, null);
            }
        }

        private FormattedText CreateTextFormat(string text, Color color, double fontSize, FlowDirection flowDirection)
        {
            if (text == null)
            {
                return null;
            }
            FormattedText formattedText = new FormattedText(text, CultureInfo.CurrentCulture, flowDirection, new Typeface("思源黑体 CN Bold"), fontSize, new SolidColorBrush(color));
            formattedText.SetFontWeight(FontWeights.Bold);
            formattedText.TextAlignment = TextAlignment.Center;
            return formattedText;
        }

        private Size ComputeRect(string message, double fontSize)
        {
            if (message == null)
            {
                return new Size(0, 0);
            }
            double addWidth = 10;
            if (System.Text.RegularExpressions.Regex.IsMatch(message, @"^.*\w+.*$"))
            {
                addWidth = 20;
            }
            FormattedText ft = new FormattedText(message, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("思源黑体 CN Bold"), fontSize, new SolidColorBrush(Colors.Black))
            {
                TextAlignment = TextAlignment.Center
            };
            double width = ft.Width + addWidth;
            double height = ft.Height;
            Size s = new Size(width, height);
            return s;
        }

        protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (_showTrackLine)
            {
                CaptureMouse();
            }
        }

        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            _selectIndex = -1;
            InvalidateVisual();
        }

        private Point _prePoint = new Point(-1, -1);
        private Point _currentPoint = new Point(-1, -1);

        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.GetPosition(this) != _prePoint)
            {
                _currentPoint = e.GetPosition(this);
                _selectIndex = GetMouseInButtonIndex(e.GetPosition(this));
                InvalidateVisual();
            }
            _prePoint = e.GetPosition(this);
        }

        public class MouseUpEventArgs
        {
            public MouseUpEventArgs()
            {
            }

            public MouseUpEventArgs(int selectIndex, string selectTitle, string planeNum)
            {
                SelectButtonIndex = selectIndex;
                SelectButtonTitle = selectTitle;
                PlaneNum = planeNum;
            }

            private int _selectButtonIndex = -1;

            public int SelectButtonIndex
            {
                set { _selectButtonIndex = value; }
                get { return _selectButtonIndex; }
            }

            private string _selectButtonTitle = "";

            public string SelectButtonTitle
            {
                set { _selectButtonTitle = value; }
                get { return _selectButtonTitle; }
            }

            private string _planeNum = "";

            public string PlaneNum
            {
                set { _planeNum = value; }
                get { return _planeNum; }
            }
        }

        public delegate void SwitchButtonMouseUp(object sender, MouseUpEventArgs e);

        public event SwitchButtonMouseUp OnSwitchButtonUp;

        protected override void OnMouseUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (_showTrackLine)
            {
                ReleaseMouseCapture();
            }
            _selectIndex = GetMouseInButtonIndex(e.GetPosition(this));
            if (_selectIndex == -1)
            {
                return;
            }
            string selectTitle = "";
            if (_selectIndex < _buttonValues.Count)
            {
                selectTitle = _buttonValues[_selectIndex];
            }
            if (OnSwitchButtonUp != null)
                OnSwitchButtonUp(this, new MouseUpEventArgs(_selectIndex, selectTitle, PlaneNo));
        }

        private int GetMouseInButtonIndex(Point mouseLocation)
        {
            double transformAngle = -90 - 360 / _currentButtonCount / 2;
            Point mp = Tp(mouseLocation, -transformAngle);
            for (int i = 0; i < _allButtons.Count; i++)
            {
                if (Ps(_allButtons[i], mp))
                {
                    return i;
                }
            }
            return -1;
        }

        private bool Ps(PointCollection points, Point point)
        {
            if (_showTrackLine)
            {
                int i, j = points.Count - 1;
                bool ps = false;

                double cx = Width / 2;
                double cy = Height / 2;
                double l = GL(cx, cy, _currentPoint.X, _currentPoint.Y);
                if (l < _innerCircleSize)
                {
                    return false;
                }

                double x = cx + (point.X - cx) * (_innerCircleSize + 2) / l;
                double y = cy + (point.Y - cy) * (_innerCircleSize + 2) / l;

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

        public void MoveToPoint(Point point)
        {
            SetLeft(this, point.X - Width / 2);
            SetTop(this, point.Y - Height / 2);
        }

        public void ShowInPoint(Point point, Canvas parent)
        {
            RenderTransform = new ScaleTransform();
            RenderTransformOrigin = new Point(0.5, 0.5);

            DoubleAnimation xAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                EasingFunction = new CircleEase { EasingMode = EasingMode.EaseIn }
            };
            object[] propertyScale = new object[]
            {
                RenderTransformProperty,
                ScaleTransform.ScaleXProperty
            };

            DoubleAnimation yAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                EasingFunction = new CircleEase { EasingMode = EasingMode.EaseIn }
            };
            object[] propertyYScale = new object[]
            {
                RenderTransformProperty,
                ScaleTransform.ScaleYProperty
            };

            Storyboard story = new Storyboard();
            Storyboard.SetTarget(xAnimation, this);
            Storyboard.SetTargetProperty(xAnimation, new PropertyPath("(0).(1)", propertyScale));

            Storyboard.SetTarget(yAnimation, this);
            Storyboard.SetTargetProperty(yAnimation, new PropertyPath("(0).(1)", propertyYScale));

            story.Children.Add(xAnimation);
            story.Children.Add(yAnimation);
            story.Begin();

            MoveToPoint(point);
            if (parent != null)
            {
                SetZIndex(this, 16000);
                parent.Children.Add(this);
            }
        }

        public void Hide(Canvas parent)
        {
            RenderTransform = new ScaleTransform();
            RenderTransformOrigin = new Point(0.5, 0.5);

            DoubleAnimation xAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                EasingFunction = new CircleEase { EasingMode = EasingMode.EaseInOut }
            };
            object[] propertyScale = new object[]
            {
                RenderTransformProperty,
                ScaleTransform.ScaleXProperty
            };
            DoubleAnimation yAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                EasingFunction = new CircleEase { EasingMode = EasingMode.EaseInOut }
            };
            object[] propertyYScale = new object[]
            {
                RenderTransformProperty,
                ScaleTransform.ScaleYProperty
            };

            Storyboard story = new Storyboard();
            Storyboard.SetTarget(xAnimation, this);
            Storyboard.SetTargetProperty(xAnimation, new PropertyPath("(0).(1)", propertyScale));

            Storyboard.SetTarget(yAnimation, this);
            Storyboard.SetTargetProperty(yAnimation, new PropertyPath("(0).(1)", propertyYScale));

            story.Children.Add(xAnimation);
            story.Children.Add(yAnimation);
            story.Begin();

            Thread anotherThread = new Thread(() =>
            {
                Thread.Sleep(1000);
                for (int i = 0; i < 1; i++)
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        if (parent != null)
                        {
                            parent.Children.Remove(this);
                        }
                    }));
                }
            });

            anotherThread.SetApartmentState(ApartmentState.STA);
            anotherThread.Start();
        }
    }
}