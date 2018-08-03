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
    public class SearchBoxFocusAdorner : Adorner
    {
        public SearchBoxFocusAdorner(UIElement arg)
            : base(arg)
        {
            this.IsHitTestVisible = false;
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (pen == null)
            {
                pen = new Pen(new SolidColorBrush(Color.FromRgb(218, 216, 215)), .6);
                pen.Freeze();
            }

            dc.DrawRoundedRectangle(null, pen, new Rect(this.RenderSize), 12, 12);
        }

        private Pen pen;
    }
}