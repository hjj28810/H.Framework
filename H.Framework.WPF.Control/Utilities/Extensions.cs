using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace H.Framework.WPF.Control.Utilities
{
    public static class Extensions
    {
        /// <summary>
        /// 弹簧式放大
        /// </summary>
        /// <param name="element"></param>
        public static void ScaleEasingInAnimation(this FrameworkElement element, bool isActivated = true)
        {
            ScaleTransform scale = new ScaleTransform();

            if (element.RenderTransform is TransformGroup)
            {
                TransformGroup group = element.RenderTransform as TransformGroup;
                var item = group.Children.SingleOrDefault(entity => entity is ScaleTransform);
                if (item != null)
                    group.Children.Remove(item);
                RotateTransform itemRotate = group.Children.SingleOrDefault(entity => entity is RotateTransform) as RotateTransform;
                scale.CenterX = itemRotate.CenterX;
                scale.CenterY = itemRotate.CenterY;
                group.Children.Add(scale);
            }
            else
            {
                element.RenderTransform = scale;
                element.RenderTransformOrigin = new Point(0.5, 0.5);
            }
            if (!isActivated)
            {
                scale.ScaleX = 1.2;
                scale.ScaleY = 1.2;
                return;
            }
            EasingFunctionBase easing = new ElasticEase()
            {
                EasingMode = EasingMode.EaseOut,
                Oscillations = 10,
                Springiness = 15
            };
            DoubleAnimation scaleAnimation = new DoubleAnimation()
            {
                //From = 1,
                To = 1.2,
                EasingFunction = easing,
                Duration = new TimeSpan(0, 0, 0, 1, 500),
                FillBehavior = FillBehavior.HoldEnd
            };
            AnimationClock clock = scaleAnimation.CreateClock();
            scale.ApplyAnimationClock(ScaleTransform.ScaleXProperty, clock, HandoffBehavior.Compose);
            scale.ApplyAnimationClock(ScaleTransform.ScaleYProperty, clock, HandoffBehavior.Compose);
        }

        /// <summary>
        /// 弹簧式缩小
        /// </summary>
        /// <param name="element"></param>
        public static void ScaleEasingOutAnimation(this FrameworkElement element)
        {
            ScaleTransform scale = new ScaleTransform();
            if (element.RenderTransform is TransformGroup)
            {
                TransformGroup group = element.RenderTransform as TransformGroup;
                var item = group.Children.SingleOrDefault(entity => entity is ScaleTransform);
                group.Children.Remove(item);
                RotateTransform itemRotate = group.Children.SingleOrDefault(entity => entity is RotateTransform) as RotateTransform;
                scale.CenterX = itemRotate.CenterX;
                scale.CenterY = itemRotate.CenterY;
                group.Children.Add(scale);
            }
            else
            {
                element.RenderTransform = scale;
                element.RenderTransformOrigin = new Point(0.5, 0.5);
            }
            DoubleAnimation scaleAnimation = new DoubleAnimation()
            {
                To = 1,
                Duration = new TimeSpan(0, 0, 0, 1, 0)
            };
            AnimationClock clock = scaleAnimation.CreateClock();
            scale.ApplyAnimationClock(ScaleTransform.ScaleXProperty, clock, HandoffBehavior.Compose);
            scale.ApplyAnimationClock(ScaleTransform.ScaleYProperty, clock, HandoffBehavior.Compose);
        }
    }
}