using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace H.Framework.WPF.Control.Utilities.Capture
{
    internal static class HelperMethods
    {
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr o);

        /// <summary>
        /// 保存图片到文件
        /// </summary>
        /// <param name="image">图片数据</param>
        /// <param name="filePath">保存路径</param>
        public static void SaveImageToFile(this BitmapSource image, string filePath)
        {
            var encoder = GetBitmapEncoder(filePath);
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(stream);
            }
        }

        /// <summary>
        /// 根据文件扩展名获取图片编码器
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>图片编码器</returns>
        private static BitmapEncoder GetBitmapEncoder(this string filePath)
        {
            var extName = System.IO.Path.GetExtension(filePath).ToLower();
            switch (extName)
            {
                case ".png":
                    return new PngBitmapEncoder();

                case ".bmp":
                    return new BmpBitmapEncoder();

                case ".jpg":
                    return new JpegBitmapEncoder();

                default:
                    return new JpegBitmapEncoder();
            }
        }

        public static Rect ToRect(this Rectangle rectangle)
        {
            return new Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static Rectangle ToRectangle(this Rect rect)
        {
            return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }

        public static Rect GetRectContainsAllScreens()
        {
            var rect = Rect.Empty;
            foreach (var screen in Screen.AllScreens)
            {
                rect.Union(screen.Bounds.ToRect());
            }

            return rect;
        }

        public static Bitmap GetScreenSnapshot()
        {
            try
            {
                var rc = SystemInformation.VirtualScreen;
                var bitmap = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);

                using (var memoryGrahics = Graphics.FromImage(bitmap))
                {
                    memoryGrahics.CopyFromScreen(rc.X, rc.Y, 0, 0, rc.Size, CopyPixelOperation.SourceCopy);
                }

                return bitmap;
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch (Exception)
            // ReSharper restore EmptyGeneralCatchClause
            {
            }

            return null;
        }

        public static BitmapSource ToBitmapSource(this Bitmap bmp)
        {
            BitmapSource returnSource;
            var bitItr = bmp.GetHbitmap();
            try
            {
                returnSource = Imaging.CreateBitmapSourceFromHBitmap(bitItr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                DeleteObject(bitItr);
            }
            catch (Exception e)
            {
                returnSource = null;
                Debug.WriteLine(e);
            }
            returnSource?.Freeze();
            return returnSource;
        }

        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            var bitmapImage = new BitmapImage();
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, bitmap.RawFormat);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            return bitmapImage;
        }

        public static T GetAncestor<T>(this DependencyObject element)
        {
            while (!(element == null || element is T))
            {
                element = VisualTreeHelper.GetParent(element);
            }

            if ((element != null) && (element is T))
            {
                return (T)(object)element;
            }

            return default(T);
        }

        public static T GetRenderTransform<T>(this UIElement element) where T : Transform
        {
            if (element.RenderTransform.Value.IsIdentity)
            {
                element.RenderTransform = CreateSimpleTransformGroup();
            }

            if (element.RenderTransform is T)
            {
                return (T)element.RenderTransform;
            }

            if (element.RenderTransform is TransformGroup)
            {
                var group = (TransformGroup)element.RenderTransform;

                foreach (var t in group.Children)
                {
                    if (t is T)
                    {
                        return (T)t;
                    }
                }
            }

            throw new NotSupportedException("Can not get instance of " + typeof(T).Name + " from " + element + "'s RenderTransform : " + element.RenderTransform);
        }

        public static TransformGroup CreateSimpleTransformGroup()
        {
            var group = new TransformGroup();

            //notes that : the RotateTransform must must be the first one in this group
            group.Children.Add(new RotateTransform());
            group.Children.Add(new TranslateTransform());
            group.Children.Add(new ScaleTransform());
            group.Children.Add(new SkewTransform());

            return group;
        }

        public static bool IsNormalNumber(this double d)
        {
            return !double.IsInfinity(d) &&
                   !double.IsNaN(d) &&
                   !double.IsNegativeInfinity(d) &&
                   !double.IsPositiveInfinity(d);
        }
    }
}