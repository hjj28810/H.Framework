using H.Framework.WPF.Control.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace H.Framework.WPF.Control.Adorners
{
    public class BusyAdorner : Adorner
    {
        public event EventHandler Cancel;

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        public BusyAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            this.IsHitTestVisible = true;

            this.chrome = new BusyChrome();
            chrome.DataContext = adornedElement;
            this.AddVisualChild(chrome);
        }

        public void FireCancel()
        {
            if (Cancel != null) { Cancel(this, EventArgs.Empty); }
        }

        protected override Visual GetVisualChild(int index)
        {
            return this.chrome;
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            this.chrome.Arrange(new Rect(arrangeBounds));
            return arrangeBounds;
        }

        private BusyChrome chrome;
    }
}