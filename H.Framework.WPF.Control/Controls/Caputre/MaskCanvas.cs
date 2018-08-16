using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace H.Framework.WPF.Control.Controls.Caputre
{
    internal class MaskCanvas : Canvas
    {
        public delegate void FrameDrawEventHander(Rect rect);

        public event FrameDrawEventHander OnMove;

        public MaskCanvas()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //make the render effect same as SnapsToDevicePixels
            //"SnapsToDevicePixels = true;" doesn't work on "OnRender"
            //however, this maybe make some offset form the render target's origin location
            //SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

            //ini this
            //Cursor = BitmapCursor.CreateCrossCursor();
            Background = Brushes.Transparent;

            //ini mask rect
            maskRectLeft.Fill = maskRectRight.Fill = maskRectTop.Fill = maskRectBottom.Fill = Config.MaskWindowBackground;

            //these propeties(x, y...) will not changed
            SetLeft(maskRectLeft, 0);
            SetTop(maskRectLeft, 0);
            SetRight(maskRectRight, 0);
            SetTop(maskRectRight, 0);
            SetTop(maskRectTop, 0);
            SetBottom(maskRectBottom, 0);
            maskRectLeft.Height = ActualHeight;

            Children.Add(maskRectLeft);
            Children.Add(maskRectRight);
            Children.Add(maskRectTop);
            Children.Add(maskRectBottom);

            //ini selection border
            selectionBorder.Stroke = Config.SelectionBorderBrush;
            selectionBorder.StrokeThickness = Config.SelectionBorderThickness.Left;
            Children.Add(selectionBorder);

            //ini indicator
            indicator = new IndicatorObject(this);
            Children.Add(indicator);

            CompositionTarget.Rendering += OnCompositionTargetRendering;
        }

        private void UpdateSelectionBorderLayout()
        {
            if (!selectionRegion.IsEmpty)
            {
                SetLeft(selectionBorder, selectionRegion.Left);
                SetTop(selectionBorder, selectionRegion.Top);
                selectionBorder.Width = selectionRegion.Width;
                selectionBorder.Height = selectionRegion.Height;
            }
        }

        private void UpdateMaskRectanglesLayout()
        {
            var actualHeight = ActualHeight;
            var actualWidth = ActualWidth;

            if (selectionRegion.IsEmpty)
            {
                SetLeft(maskRectLeft, 0);
                SetTop(maskRectLeft, 0);
                maskRectLeft.Width = actualWidth;
                maskRectLeft.Height = actualHeight;

                maskRectRight.Width = maskRectRight.Height = maskRectTop.Width = maskRectTop.Height = maskRectBottom.Width = maskRectBottom.Height = 0;
            }
            else
            {
                var temp = selectionRegion.Left;
                if (maskRectLeft.Width != temp)
                {
                    maskRectLeft.Width = temp < 0 ? 0 : temp; //Math.Max(0, selectionRegion.Left);
                }

                temp = ActualWidth - selectionRegion.Right;
                if (maskRectRight.Width != temp)
                {
                    maskRectRight.Width = temp < 0 ? 0 : temp; //Math.Max(0, ActualWidth - selectionRegion.Right);
                }

                if (maskRectRight.Height != actualHeight)
                {
                    maskRectRight.Height = actualHeight;
                }

                SetLeft(maskRectTop, maskRectLeft.Width);
                SetLeft(maskRectBottom, maskRectLeft.Width);

                temp = actualWidth - maskRectLeft.Width - maskRectRight.Width;
                if (maskRectTop.Width != temp)
                {
                    maskRectTop.Width = temp < 0 ? 0 : temp; //Math.Max(0, ActualWidth - maskRectLeft.Width - maskRectRight.Width);
                }

                temp = selectionRegion.Top;
                if (maskRectTop.Height != temp)
                {
                    maskRectTop.Height = temp < 0 ? 0 : temp; //Math.Max(0, selectionRegion.Top);
                }

                maskRectBottom.Width = maskRectTop.Width;

                temp = actualHeight - selectionRegion.Bottom;
                if (maskRectBottom.Height != temp)
                {
                    maskRectBottom.Height = temp < 0 ? 0 : temp; //Math.Max(0, ActualHeight - selectionRegion.Bottom);
                }
            }
        }

        #region Fileds & Props

        private IndicatorObject indicator;
        private Point? selectionStartPoint;
        private Point? selectionEndPoint;
        private Rect selectionRegion = Rect.Empty;
        private bool isMaskDraging;

        private readonly System.Windows.Shapes.Rectangle selectionBorder = new System.Windows.Shapes.Rectangle();

        private readonly System.Windows.Shapes.Rectangle maskRectLeft = new System.Windows.Shapes.Rectangle();
        private readonly System.Windows.Shapes.Rectangle maskRectRight = new System.Windows.Shapes.Rectangle();
        private readonly System.Windows.Shapes.Rectangle maskRectTop = new System.Windows.Shapes.Rectangle();
        private readonly System.Windows.Shapes.Rectangle maskRectBottom = new System.Windows.Shapes.Rectangle();

        public Size? DefaultSize
        {
            get;
            set;
        }

        public MaskWindow MaskWindowOwner
        {
            get;
            set;
        }

        #endregion Fileds & Props

        #region Mouse Managment

        private bool IsMouseOnThis(RoutedEventArgs e)
        {
            return e.Source.Equals(this) || e.Source.Equals(maskRectLeft) || e.Source.Equals(maskRectRight) || e.Source.Equals(maskRectTop) || e.Source.Equals(maskRectBottom);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (selectionRegion != Rect.Empty)
            {
                return;
            }
            //mouse down on this self
            if (IsMouseOnThis(e))
            {
                PrepareShowMask(Mouse.GetPosition(this));
            }
            //mouse down on indicator
            else if (e.Source.Equals(indicator))
            {
                HandleIndicatorMouseDown(e);
            }
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (IsMouseOnThis(e))
            {
                UpdateSelectionRegion(e, UpdateMaskType.ForMouseMoving);

                e.Handled = true;
                //委托调用
                //Rect rec = OnMove();
                if (OnMove != null)
                {
                    if (selectionRegion != null)
                    {
                        OnMove(selectionRegion);
                    }
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (IsMouseOnThis(e))
            {
                UpdateSelectionRegion(e, UpdateMaskType.ForMouseLeftButtonUp);
                FinishShowMask();
            }
            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            ClearSelectedDataAndView();
            base.OnMouseRightButtonUp(e);
        }

        internal void HandleIndicatorMouseDown(MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                FinishAction();
            }
        }

        public void FinishAction()
        {
            if (MaskWindowOwner != null)
            {
                MaskWindowOwner.ClipSnapshot(GetIndicatorRegion());
                ClearSelectionData();
            }
        }

        public System.Drawing.Bitmap GetSnapBitmap()
        {
            System.Drawing.Bitmap saveBitmap = null;
            if (MaskWindowOwner != null)
            {
                Rect clipRegion = GetIndicatorRegion();
                saveBitmap = MaskWindowOwner.CopyBitmapFromScreenSnapshot(clipRegion);

                //close mask window
                //Close();
                //ClearSelectionData();
            }
            return saveBitmap;
        }

        private void PrepareShowMask(Point mouseLoc)
        {
            indicator.Visibility = Visibility.Collapsed;
            selectionBorder.Visibility = Visibility.Visible;
            selectionStartPoint = new Point?(mouseLoc);

            if (!IsMouseCaptured)
            {
                CaptureMouse();
            }
        }

        private void UpdateSelectionRegion(MouseEventArgs e, UpdateMaskType updateType)
        {
            if (updateType == UpdateMaskType.ForMouseMoving && e.LeftButton != MouseButtonState.Pressed)
            {
                selectionStartPoint = null;
            }

            if (selectionStartPoint.HasValue)
            {
                selectionEndPoint = e.GetPosition(this);

                var startPoint = (Point)selectionEndPoint;
                var endPoint = (Point)selectionStartPoint;
                var sX = startPoint.X;
                var sY = startPoint.Y;
                var eX = endPoint.X;
                var eY = endPoint.Y;

                var deltaX = eX - sX;
                var deltaY = eY - sY;

                if (Math.Abs(deltaX) >= SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(deltaX) >= SystemParameters.MinimumVerticalDragDistance)
                {
                    isMaskDraging = true;

                    double x = sX < eX ? sX : eX;//Math.Min(sX, eX);
                    double y = sY < eY ? sY : eY;//Math.Min(sY, eY);
                    double w = deltaX < 0 ? -deltaX : deltaX;//Math.Abs(deltaX);
                    double h = deltaY < 0 ? -deltaY : deltaY;//Math.Abs(deltaY);

                    selectionRegion = new Rect(x, y, w, h);
                }
                else
                {
                    if (DefaultSize.HasValue && updateType == UpdateMaskType.ForMouseLeftButtonUp)
                    {
                        isMaskDraging = true;

                        selectionRegion = new Rect(startPoint.X, startPoint.Y, DefaultSize.Value.Width, DefaultSize.Value.Height);
                    }
                    else
                    {
                        isMaskDraging = false;
                    }
                }
            }
        }

        internal void UpdateSelectionRegion(Rect region)
        {
            selectionRegion = region;
        }

        public Rect GetSelectionRegion()
        {
            return selectionRegion;
        }

        private void FinishShowMask()
        {
            if (IsMouseCaptured)
            {
                ReleaseMouseCapture();
            }

            if (isMaskDraging)
            {
                if (MaskWindowOwner != null)
                {
                    MaskWindowOwner.OnShowMaskFinished(selectionRegion);
                }

                UpdateIndicator(selectionRegion);

                ClearSelectionData();
            }
        }

        public void ClearSelectedDataAndView()
        {
            indicator.Visibility = Visibility.Collapsed;
            selectionRegion = Rect.Empty;
            selectionBorder.Width = selectionBorder.Height = 0;
            ClearSelectionData();
            UpdateMaskRectanglesLayout();
        }

        private void ClearSelectionData()
        {
            isMaskDraging = false;
            selectionBorder.Visibility = Visibility.Collapsed;
            selectionStartPoint = null;
            selectionEndPoint = null;
        }

        private void UpdateIndicator(Rect region)
        {
            if (region.Width < indicator.MinWidth || region.Height < indicator.MinHeight)
            {
                return;
            }

            indicator.Width = region.Width;
            indicator.Height = region.Height;
            SetLeft(indicator, region.Left);
            SetTop(indicator, region.Top);

            indicator.Visibility = Visibility.Visible;
        }

        private Rect GetIndicatorRegion()
        {
            return new Rect(GetLeft(indicator), GetTop(indicator), indicator.ActualWidth, indicator.ActualHeight);
        }

        #endregion Mouse Managment

        #region Render

        private void OnCompositionTargetRendering(object sender, EventArgs e)
        {
            UpdateSelectionBorderLayout();
            UpdateMaskRectanglesLayout();
        }

        #endregion Render

        #region inner types

        private enum UpdateMaskType
        {
            ForMouseMoving,
            ForMouseLeftButtonUp
        }

        #endregion inner types
    }
}