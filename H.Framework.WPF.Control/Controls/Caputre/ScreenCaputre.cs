using H.Framework.WPF.Control.Utilities.Caputre;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace H.Framework.WPF.Control.Controls.Caputre
{
    public class ScreenCaputre
    {
        public void StartCaputre(int timeOutSeconds)
        {
            Thread.Sleep(150);
            StartCaputre(timeOutSeconds, null);
        }

        private MaskWindow _mask;

        public void StartCaputre(int timeOutSeconds, Size? defaultSize)
        {
            if (_mask == null)
            {
                _mask = new MaskWindow(this, timeOutSeconds);
                _mask.Show(defaultSize);
            }
            else
            {
                _mask.GetScreenShoot();
                _mask.Visibility = Visibility.Visible;
            }
        }

        public event EventHandler<ScreenCaputredEventArgs> ScreenCaputred;

        public event EventHandler<EventArgs> ScreenCaputreCancelled;

        internal void OnScreenCaputred(object sender, BitmapSource caputredBmp)
        {
            var filePath = Environment.GetEnvironmentVariable("TEMP") + @"\" + Guid.NewGuid().ToString("N") + ".png";
            caputredBmp.SaveImageToFile(filePath);
            Clipboard.SetFileDropList(new System.Collections.Specialized.StringCollection { filePath });
            //Clipboard.SetImage(e.Bmp);直接保存图片到剪贴板
            ScreenCaputred?.Invoke(sender, new ScreenCaputredEventArgs(caputredBmp));
        }

        internal void OnScreenCaputreCancelled(object sender)
        {
            ScreenCaputreCancelled?.Invoke(sender, EventArgs.Empty);
        }
    }

    public class ScreenCaputredBitmapEventArgs : EventArgs
    {
        public System.Drawing.Bitmap BitMapArg
        {
            get;
            private set;
        }

        public ScreenCaputredBitmapEventArgs(System.Drawing.Bitmap bmp)
        {
            BitMapArg = bmp;
        }
    }

    public class ScreenCaputredEventArgs : EventArgs
    {
        public BitmapSource Bmp
        {
            get;
            private set;
        }

        public ScreenCaputredEventArgs(BitmapSource bmp)
        {
            Bmp = bmp;
        }
    }
}