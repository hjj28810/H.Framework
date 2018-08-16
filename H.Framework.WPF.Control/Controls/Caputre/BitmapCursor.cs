using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Interop;

namespace H.Framework.WPF.Control.Controls.Caputre
{
    internal class BitmapCursor : SafeHandle
    {
        public override bool IsInvalid => handle == (IntPtr)(-1);

        public static Cursor CreateBmpCursor(Bitmap cursorBitmap)
        {
            var c = new BitmapCursor(cursorBitmap);

            return CursorInteropHelper.Create(c);
        }

        protected BitmapCursor(Bitmap cursorBitmap)
            : base((IntPtr)(-1), true)
        {
            handle = cursorBitmap.GetHicon();
        }

        protected override bool ReleaseHandle()
        {
            bool result = DestroyIcon(handle);

            handle = (IntPtr)(-1);

            return result;
        }

        [DllImport("user32.dll")]
        public static extern bool DestroyIcon(IntPtr hIcon);

        public static Cursor CreateCrossCursor()
        {
            const int w = 25;
            const int h = 25;

            var bmp = new Bitmap(w, h);

            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.Default;
            g.InterpolationMode = InterpolationMode.High;

            var pen = new Pen(Brushes.Black, 2);
            g.DrawLine(pen, new Point(12, 0), new Point(12, 8)); // vertical line
            g.DrawLine(pen, new Point(12, 17), new Point(12, 25)); // vertical line
            g.DrawLine(pen, new Point(0, 12), new Point(8, 12)); // horizontal line
            g.DrawLine(pen, new Point(16, 12), new Point(24, 12)); // horizontal line
            g.DrawLine(pen, new Point(12, 12), new Point(12, 13)); // Middle dot

            g.Flush();
            g.Dispose();
            pen.Dispose();

            var c = CreateBmpCursor(bmp);

            bmp.Dispose();

            return c;
        }
    }
}