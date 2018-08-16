using System.Windows;
using System.Windows.Media;

namespace H.Framework.WPF.Control.Controls.Caputre
{
    internal static class Config
    {
        public static Brush SelectionBorderBrush = new SolidColorBrush(Color.FromArgb(255, 49, 106, 196));
        public static Thickness SelectionBorderThickness = new Thickness(2.0);
        public static Brush MaskWindowBackground = new SolidColorBrush(Color.FromArgb(45, 255, 255, 255));
    }

    internal enum ResizeThumbPlacement
    {
        None,
        LeftTop,
        TopCenter,
        RightTop,
        RightCenter,
        RightBottom,
        BottomCenter,
        LeftBottom,
        LeftCenter
    }
}