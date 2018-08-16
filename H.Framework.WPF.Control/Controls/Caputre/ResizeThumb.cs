using H.Framework.WPF.Control.Utilities.Caputre;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace H.Framework.WPF.Control.Controls.Caputre
{
    internal class ResizeThumb : ThumbBase
    {
        private RotateTransform rotateTransform;
        private Point transformOrigin;
        private double angle;

        public ResizeThumbPlacement Placement
        {
            get => (ResizeThumbPlacement)GetValue(PlacementProperty);
            set => SetValue(PlacementProperty, value);
        }

        public static readonly DependencyProperty PlacementProperty =
            DependencyProperty.Register("Placement", typeof(ResizeThumbPlacement),
            typeof(ResizeThumb), new UIPropertyMetadata(ResizeThumbPlacement.None));

        protected override void OnDragStarted(object sender, DragStartedEventArgs e)
        {
            if (Target != null)
            {
                var canvas = VisualTreeHelper.GetParent(Target) as MaskCanvas;
                if (canvas != null)
                {
                    transformOrigin = Target.RenderTransformOrigin;
                    rotateTransform = Target.GetRenderTransform<RotateTransform>();

                    if (rotateTransform != null)
                    {
                        angle = rotateTransform.Angle * Math.PI / 180;
                    }
                    else
                    {
                        angle = 0.0;
                    }
                }
            }
        }

        protected override void OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Target != null)
            {
                var delta = new Point(e.HorizontalChange, e.VerticalChange);

                var canvas = Target.Parent as MaskCanvas;
                if (canvas != null)
                {
                    double x = Canvas.GetLeft(Target);
                    double y = Canvas.GetTop(Target);
                    double w = Target.ActualWidth;
                    double h = Target.ActualHeight;

                    //adjust delta.X when normal resize
                    switch (Placement)
                    {
                        case ResizeThumbPlacement.LeftTop:
                        case ResizeThumbPlacement.LeftBottom:
                        case ResizeThumbPlacement.LeftCenter:
                            {
                                if (w - delta.X <= Target.MinWidth && delta.X > 0)
                                {
                                    delta.X = w - Target.MinWidth;
                                }

                                if (w - delta.X >= Target.MaxWidth && delta.X < 0)
                                {
                                    delta.X = w - Target.MaxWidth;
                                }
                            }
                            break;

                        default:
                            break;
                    }

                    //adjust delta.Y when normal resize
                    switch (Placement)
                    {
                        case ResizeThumbPlacement.LeftTop:
                        case ResizeThumbPlacement.RightTop:
                        case ResizeThumbPlacement.TopCenter:
                            {
                                if (h - delta.Y <= Target.MinHeight && delta.Y > 0)
                                {
                                    delta.Y = h - Target.MinHeight;
                                }

                                if (h - delta.Y >= Target.MaxHeight && delta.Y < 0)
                                {
                                    delta.Y = h - Target.MaxHeight;
                                }
                            }
                            break;

                        default:
                            break;
                    }

                    //adjust delta.X when rotated an then resize
                    switch (Placement)
                    {
                        case ResizeThumbPlacement.RightTop:
                        case ResizeThumbPlacement.RightBottom:
                        case ResizeThumbPlacement.RightCenter:
                            {
                                if (w + delta.X <= Target.MinWidth && delta.X < 0)
                                {
                                    delta.X = Target.MinWidth - w;
                                }

                                if (w + delta.X >= Target.MaxWidth && delta.X > 0)
                                {
                                    delta.X = Target.MaxWidth - w;
                                }
                            }
                            break;

                        default:
                            break;
                    }

                    //adjust delta.Y when rotated an then resize
                    switch (Placement)
                    {
                        case ResizeThumbPlacement.LeftBottom:
                        case ResizeThumbPlacement.RightBottom:
                        case ResizeThumbPlacement.BottomCenter:
                            {
                                if (h + delta.Y <= Target.MinHeight && delta.Y < 0)
                                {
                                    delta.Y = Target.MinHeight - h;
                                }

                                if (h + delta.Y >= Target.MaxHeight && delta.Y > 0)
                                {
                                    delta.Y = Target.MaxHeight - h;
                                }
                            }
                            break;

                        default:
                            break;
                    }

                    switch (Placement)
                    {
                        case ResizeThumbPlacement.LeftTop:
                            {
                                x += delta.Y * Math.Sin(-angle) - transformOrigin.Y * delta.Y * Math.Sin(-angle);
                                y += delta.Y * Math.Cos(-angle) + transformOrigin.Y * delta.Y * (1 - Math.Cos(-angle));

                                x += delta.X * Math.Cos(angle) + transformOrigin.X * delta.X * (1 - Math.Cos(angle));
                                y += delta.X * Math.Sin(angle) - transformOrigin.X * delta.X * Math.Sin(angle);

                                w -= delta.X;
                                h -= delta.Y;
                            }
                            break;

                        case ResizeThumbPlacement.TopCenter:
                            {
                                x += delta.Y * Math.Sin(-angle) - transformOrigin.Y * delta.Y * Math.Sin(-angle);
                                y += delta.Y * Math.Cos(-angle) + transformOrigin.Y * delta.Y * (1 - Math.Cos(-angle));

                                h -= delta.Y;
                            }
                            break;

                        case ResizeThumbPlacement.RightTop:
                            {
                                x += delta.Y * Math.Sin(-angle) - transformOrigin.Y * delta.Y * Math.Sin(-angle);
                                y += delta.Y * Math.Cos(-angle) + transformOrigin.Y * delta.Y * (1 - Math.Cos(-angle));

                                x -= delta.X * transformOrigin.X * (1 - Math.Cos(angle));
                                y += delta.X * transformOrigin.X * Math.Sin(angle);

                                w += delta.X;
                                h -= delta.Y;
                            }
                            break;

                        case ResizeThumbPlacement.RightCenter:
                            {
                                x -= delta.X * transformOrigin.X * (1 - Math.Cos(angle));
                                y += delta.X * transformOrigin.X * Math.Sin(angle);

                                w += delta.X;
                            }
                            break;

                        case ResizeThumbPlacement.RightBottom:
                            {
                                x += delta.Y * transformOrigin.Y * Math.Sin(-angle);
                                y -= delta.Y * transformOrigin.Y * (1 - Math.Cos(-angle));

                                x -= delta.X * transformOrigin.X * (1 - Math.Cos(angle));
                                y += delta.X * transformOrigin.X * Math.Sin(angle);

                                w += delta.X;
                                h += delta.Y;
                            }
                            break;

                        case ResizeThumbPlacement.BottomCenter:
                            {
                                x += delta.Y * transformOrigin.Y * Math.Sin(-angle);
                                y -= delta.Y * transformOrigin.Y * (1 - Math.Cos(-angle));

                                h += delta.Y;
                            }
                            break;

                        case ResizeThumbPlacement.LeftBottom:
                            {
                                x += delta.Y * transformOrigin.Y * Math.Sin(-angle);
                                y -= delta.Y * transformOrigin.Y * (1 - Math.Cos(-angle));

                                x += delta.X * Math.Cos(angle) + transformOrigin.X * delta.X * (1 - Math.Cos(angle));
                                y += delta.X * Math.Sin(angle) - transformOrigin.X * delta.X * Math.Sin(angle);

                                w -= delta.X;
                                h += delta.Y;
                            }
                            break;

                        case ResizeThumbPlacement.LeftCenter:
                            {
                                x += delta.X * Math.Cos(angle) + transformOrigin.X * delta.X * (1 - Math.Cos(angle));
                                y += delta.X * Math.Sin(angle) - transformOrigin.X * delta.X * Math.Sin(angle);

                                w -= delta.X;
                            }
                            break;

                        default:
                            break;
                    }

                    if (x.IsNormalNumber() && y.IsNormalNumber() &&
                        w.IsNormalNumber() && h.IsNormalNumber())
                    {
                        w = Math.Min(Target.MaxWidth, Math.Max(Target.MinWidth, w));
                        h = Math.Min(Target.MaxHeight, Math.Max(Target.MinHeight, h));

                        var rect = new Rect(x, y, w, h);

                        Target.Resize(rect);
                    }
                }
            }
        }

        protected override void OnDragCompleted(object sender, DragCompletedEventArgs e)
        {
        }
    }
}