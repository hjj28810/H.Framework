using H.Framework.WPF.Control.Utilities.Capture;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace H.Framework.WPF.Control.Controls.Capture
{
    internal class MoveThumb : ThumbBase
    {
        /// <summary>
        /// usually, the move thumb is beyond  the left(right, top, bottom) side of target,
        /// however, some of targets can be moved by drag the center of it.
        /// </summary>
        public bool IsMoveByTargetCenter
        {
            get => (bool)GetValue(IsMoveByTargetCenterProperty);
            set => SetValue(IsMoveByTargetCenterProperty, value);
        }

        public static readonly DependencyProperty IsMoveByTargetCenterProperty =
            DependencyProperty.Register("IsMoveByTargetCenter", typeof(bool), typeof(MoveThumb), new UIPropertyMetadata(false));

        protected override void OnDragStarted(object sender, DragStartedEventArgs e)
        {
        }

        protected override void OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Target != null)
            {
                var delta = new Point(e.HorizontalChange, e.VerticalChange);
                Target.Move(delta);
            }
        }

        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (IsMoveByTargetCenter)
            {
                var canvas = this.GetAncestor<MaskCanvas>();
                if (canvas != null)
                {
                    canvas.HandleIndicatorMouseDown(e);
                }
            }
        }
    }
}