using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace H.Framework.WPF.Control.Controls.ExtendedWindows
{
    public static class WindowParameters
    {
        private static Thickness? _paddedBorderThickness;

        private static double? _ribbonContextualTabGroupHeight;

        /// <summary>
        /// returns the border thickness padding around captioned windows,in pixels. Windows XP/2000:  This value is not supported.
        /// </summary>
        public static Thickness PaddedBorderThickness
        {
            [SecurityCritical]
            get
            {
                if (_paddedBorderThickness == null)
                {
                    var paddedBorder = NativeMethods.GetSystemMetrics(SM.CXPADDEDBORDER);
                    var dpi = GetDpi();
                    Size frameSize = new Size(paddedBorder, paddedBorder);
                    Size frameSizeInDips = DpiHelper.DeviceSizeToLogical(frameSize, dpi / 96.0, dpi / 96.0);
                    _paddedBorderThickness = new Thickness(frameSizeInDips.Width, frameSizeInDips.Height, frameSizeInDips.Width, frameSizeInDips.Height);
                }

                return _paddedBorderThickness.Value;
            }
        }

        public static double RibbonContextualTabGroupHeight
        {
            get
            {
                if (_ribbonContextualTabGroupHeight == null)
                {
                    _ribbonContextualTabGroupHeight = SystemParameters.WindowNonClientFrameThickness.Top + (1d * GetDpi() / 96.0);
                }

                return _ribbonContextualTabGroupHeight.Value;
            }
        }

        /// <summary>
        /// Get Dpi
        /// </summary>
        /// <returns>Return 96,144/returns>
        public static double GetDpi()
        {
            var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);

            var dpiX = (int)dpiXProperty.GetValue(null, null);
            return dpiX;
        }
    }

    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms724385(v=vs.85).aspx
    /// Retrieves the specified system metric or system configuration setting.
    /// Note that all dimensions retrieved by GetSystemMetrics are in pixels.
    /// </summary>
    public enum SM
    {
        /// <summary>
        /// The amount of border padding for captioned windows, in pixels.
        /// Returns the amount of extra border padding around captioned windows
        /// Windows XP/2000:  This value is not supported.
        /// </summary>
        CXPADDEDBORDER = 92,
    }

    internal static class NativeMethods
    {
        [DllImport("user32.dll")]
        internal static extern int GetSystemMetrics(SM nIndex);
    }

    internal static class DpiHelper
    {
        [ThreadStatic]
        private static Matrix _transformToDevice;

        [ThreadStatic]
        private static Matrix _transformToDip;

        /// <summary>
        /// Convert a point in device independent pixels (1/96") to a point in the system coordinates.
        /// </summary>
        /// <param name="logicalPoint">A point in the logical coordinate system.</param>
        /// <returns>Returns the parameter converted to the system's coordinates.</returns>
        public static Point LogicalPixelsToDevice(Point logicalPoint, double dpiScaleX, double dpiScaleY)
        {
            _transformToDevice = Matrix.Identity;
            _transformToDevice.Scale(dpiScaleX, dpiScaleY);
            return _transformToDevice.Transform(logicalPoint);
        }

        /// <summary>
        /// Convert a point in system coordinates to a point in device independent pixels (1/96").
        /// </summary>
        /// <param name="devicePoint">A point in the physical coordinate system.</param>
        /// <param name="dpiScaleX">dpiScaleX</param>
        /// <param name="dpiScaleY">dpiScaleY</param>
        /// <returns>Returns the parameter converted to the device independent coordinate system.</returns>
        public static Point DevicePixelsToLogical(Point devicePoint, double dpiScaleX, double dpiScaleY)
        {
            _transformToDip = Matrix.Identity;
            _transformToDip.Scale(1d / dpiScaleX, 1d / dpiScaleY);
            return _transformToDip.Transform(devicePoint);
        }

        public static Rect LogicalRectToDevice(Rect logicalRectangle, double dpiScaleX, double dpiScaleY)
        {
            Point topLeft = LogicalPixelsToDevice(new Point(logicalRectangle.Left, logicalRectangle.Top), dpiScaleX, dpiScaleY);
            Point bottomRight = LogicalPixelsToDevice(new Point(logicalRectangle.Right, logicalRectangle.Bottom), dpiScaleX, dpiScaleY);

            return new Rect(topLeft, bottomRight);
        }

        public static Rect DeviceRectToLogical(Rect deviceRectangle, double dpiScaleX, double dpiScaleY)
        {
            Point topLeft = DevicePixelsToLogical(new Point(deviceRectangle.Left, deviceRectangle.Top), dpiScaleX, dpiScaleY);
            Point bottomRight = DevicePixelsToLogical(new Point(deviceRectangle.Right, deviceRectangle.Bottom), dpiScaleX, dpiScaleY);

            return new Rect(topLeft, bottomRight);
        }

        public static Size LogicalSizeToDevice(Size logicalSize, double dpiScaleX, double dpiScaleY)
        {
            Point pt = LogicalPixelsToDevice(new Point(logicalSize.Width, logicalSize.Height), dpiScaleX, dpiScaleY);

            return new Size { Width = pt.X, Height = pt.Y };
        }

        public static Size DeviceSizeToLogical(Size deviceSize, double dpiScaleX, double dpiScaleY)
        {
            Point pt = DevicePixelsToLogical(new Point(deviceSize.Width, deviceSize.Height), dpiScaleX, dpiScaleY);

            return new Size(pt.X, pt.Y);
        }

        public static Thickness LogicalThicknessToDevice(Thickness logicalThickness, double dpiScaleX, double dpiScaleY)
        {
            Point topLeft = LogicalPixelsToDevice(new Point(logicalThickness.Left, logicalThickness.Top), dpiScaleX, dpiScaleY);
            Point bottomRight = LogicalPixelsToDevice(new Point(logicalThickness.Right, logicalThickness.Bottom), dpiScaleX, dpiScaleY);

            return new Thickness(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
        }
    }
}