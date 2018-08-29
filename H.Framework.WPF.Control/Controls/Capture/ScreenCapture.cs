using H.Framework.WPF.Control.Utilities.Capture;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace H.Framework.WPF.Control.Controls.Capture
{
    public class ScreenCapture
    {
        public void StartCapture(int timeOutSeconds)
        {
            Thread.Sleep(150);
            StartCapture(timeOutSeconds, null);
        }

        private MaskWindow _mask;

        public void StartCapture(int timeOutSeconds, Size? defaultSize)
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

        public event EventHandler<ScreenCapturedEventArgs> ScreenCaptured;

        public event EventHandler<EventArgs> ScreenCaptureCancelled;

        internal void OnScreenCaptured(object sender, BitmapSource caputredBmp)
        {
            var filePath = Environment.GetEnvironmentVariable("TEMP") + @"\" + Guid.NewGuid().ToString("N") + ".png";
            caputredBmp.SaveImageToFile(filePath);
            //Clipboard.SetFileDropList(new System.Collections.Specialized.StringCollection { filePath });
            var dataObject = new DataObject();
            dataObject.SetData(DataFormats.FileDrop, new string[] { filePath });
            Clipboard.SetDataObject(dataObject, true);
            //Clipboard.SetImage(e.Bmp);直接保存图片到剪贴板
            ScreenCaptured?.Invoke(sender, new ScreenCapturedEventArgs(caputredBmp));
        }

        internal void OnScreenCaptureCancelled(object sender)
        {
            ScreenCaptureCancelled?.Invoke(sender, EventArgs.Empty);
        }
    }

    public class ScreenCapturedBitmapEventArgs : EventArgs
    {
        public System.Drawing.Bitmap BitMapArg
        {
            get;
            private set;
        }

        public ScreenCapturedBitmapEventArgs(System.Drawing.Bitmap bmp)
        {
            BitMapArg = bmp;
        }
    }

    public class ScreenCapturedEventArgs : EventArgs
    {
        public BitmapSource Bmp
        {
            get;
            private set;
        }

        public ScreenCapturedEventArgs(BitmapSource bmp)
        {
            Bmp = bmp;
        }
    }
}