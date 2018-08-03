using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace H.Framework.WPF.Infrastructure.Utilities
{
    public class ImageExtender : DependencyObject
    {
        public static readonly DependencyProperty ImageSourceURIProperty = DependencyProperty.RegisterAttached("ImageSourceURI",
            typeof(string), typeof(ImageExtender), new UIPropertyMetadata(null
                , ImageSourceURIChanged));

        public static string GetImageSourceURI(DependencyObject obj)
        {
            return (string)obj.GetValue(ImageSourceURIProperty);
        }

        public static void SetImageSourceURI(DependencyObject obj, DependencyObject value)
        {
            obj.SetValue(ImageSourceURIProperty, value);
        }

        public static void ImageSourceURIChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var image = (Image)sender;
            var sourceURL = GetImageSourceURI(image).ToString();

            var myBitmapImage = new BitmapImage();

            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(sourceURL, UriKind.RelativeOrAbsolute);

            //myBitmapImage.DecodePixelWidth = 550;
            //myBitmapImage.DecodePixelHeight = 550;
            myBitmapImage.EndInit();

            image.Source = myBitmapImage;
            image.Height = (myBitmapImage.Height / 3) < 10 ? 100 : myBitmapImage.Height / 3;
            image.Width = (myBitmapImage.Width / 3) < 10 ? 100 : myBitmapImage.Width / 3;
        }
    }
}