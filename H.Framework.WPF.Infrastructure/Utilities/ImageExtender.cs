using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace H.Framework.WPF.Infrastructure.Utilities
{
    public class ImageExtender : DependencyObject
    {
        //public static readonly DependencyProperty ImageSourceURIProperty = DependencyProperty.RegisterAttached("ImageSourceURI",
        //    typeof(string), typeof(ImageExtender), new UIPropertyMetadata(null
        //        , ImageSourceURIChanged));

        //public static string GetImageSourceURI(DependencyObject obj)
        //{
        //    return (string)obj.GetValue(ImageSourceURIProperty);
        //}

        //public static void SetImageSourceURI(DependencyObject obj, DependencyObject value)
        //{
        //    obj.SetValue(ImageSourceURIProperty, value);
        //}

        //public static void ImageSourceURIChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //    var image = (Image)sender;

        //    var myBitmapImage = new BitmapImage();

        //    myBitmapImage.BeginInit();
        //    myBitmapImage.UriSource = new Uri(e.NewValue.ToString(), UriKind.RelativeOrAbsolute);
        //    myBitmapImage.CacheOption = BitmapCacheOption.OnLoad;//图像缓存到内存中，不会占用文件，没有被引用时会被自动回收。

        //    myBitmapImage.EndInit();
        //    myBitmapImage.Freeze();
        //    image.Source = myBitmapImage;
        //    image.Height = (myBitmapImage.Height / 3) < 10 ? 100 : myBitmapImage.Height / 3;
        //    image.Width = (myBitmapImage.Width / 3) < 10 ? 100 : myBitmapImage.Width / 3;
        //}

        public static readonly DependencyProperty WeakSourceUriProperty = DependencyProperty.RegisterAttached("WeakSourceUri",
           typeof(string), typeof(ImageExtender), new UIPropertyMetadata(null
               , WeakSourceUriChanged));

        public static string GetWeakSourceUri(DependencyObject obj)
        {
            return (string)obj.GetValue(WeakSourceUriProperty);
        }

        public static void SetWeakSourceUri(DependencyObject obj, DependencyObject value)
        {
            obj.SetValue(WeakSourceUriProperty, value);
        }

        public static void WeakSourceUriChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                var image = (Image)sender;
                var myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                var uri = e.NewValue.ToString();
                MemoryStream ms = null;
                var isFile = File.Exists(uri);
                if (isFile)
                {
                    ms = new MemoryStream(File.ReadAllBytes(uri));
                    myBitmapImage.StreamSource = ms;
                }
                else
                    myBitmapImage.UriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
                //myBitmapImage.DecodePixelWidth = 550;
                //myBitmapImage.DecodePixelHeight = 550;
                myBitmapImage.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                myBitmapImage.CacheOption = BitmapCacheOption.OnLoad;//图像缓存到内存中，不会占用文件，没有被引用时会被自动回收。
                myBitmapImage.EndInit();
                if (isFile)
                {
                    ms?.Dispose();
                }
                myBitmapImage.Freeze();
                image.Source = myBitmapImage;
                image.Height = (myBitmapImage.Height / 3) < 10 ? 100 : myBitmapImage.Height / 3;
                image.Width = (myBitmapImage.Width / 3) < 10 ? 100 : myBitmapImage.Width / 3;
            }
            catch
            { }
        }
    }
}