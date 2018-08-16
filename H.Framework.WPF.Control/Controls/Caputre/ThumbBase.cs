using H.Framework.WPF.Control.Utilities.Caputre;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace H.Framework.WPF.Control.Controls.Caputre
{
    internal class ThumbBase : Thumb
    {
        /// <summary>
        /// Get the <see cref="IndicatorObject"/> target which this thumb is created for.
        /// </summary>
        public IndicatorObject Target
        {
            get;
            private set;
        }

        public ThumbBase()
        {
            FocusVisualStyle = null;
            DragStarted += OnDragStarted;
            DragDelta += OnDragDelta;
            DragCompleted += OnDragCompleted;
        }

        //usually, you should override this method to do some clear up work.
        //Add push current operation into undo-redo manager
        protected virtual void OnDragCompleted(object sender, DragCompletedEventArgs e)
        {
        }

        //usually, you should override this method to drag
        protected virtual void OnDragDelta(object sender, DragDeltaEventArgs e)
        {
        }

        //usually, you should override this method to do some prepare work for dragging
        protected virtual void OnDragStarted(object sender, DragStartedEventArgs e)
        {
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);

            Target = this.GetAncestor<IndicatorObject>();
        }
    }
}