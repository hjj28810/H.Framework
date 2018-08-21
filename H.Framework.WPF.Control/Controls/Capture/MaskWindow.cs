using H.Framework.Core.Utilities;
using H.Framework.WPF.Control.Utilities.Capture;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace H.Framework.WPF.Control.Controls.Capture
{
    internal class MaskWindow : Window
    {
        private MaskCanvas innerCanvas;
        private Bitmap screenSnapshot;
        private Timer timeOutTimmer;
        private readonly ScreenCapture screenCaptureOwner;

        //截图显示尺寸label
        private System.Windows.Controls.Label label = null;

        //截图显示按键
        private ToolBarControl toolBarContrl = null;

        //截图保存图片
        private Bitmap m_bmpLayerCurrent;

        private int _timeOutSecond;

        public MaskWindow(ScreenCapture screenCaptureOwner, int timeOutSecond)
        {
            this.screenCaptureOwner = screenCaptureOwner;
            _timeOutSecond = timeOutSecond;
            if (timeOutSecond > 0)
            {
                if (timeOutTimmer == null)
                {
                    timeOutTimmer = new System.Windows.Forms.Timer();
                    timeOutTimmer.Tick += OnTimeOutTimmerTick;
                    timeOutTimmer.Interval = _timeOutSecond * 1000;
                }
            }
            GetScreenShoot();
            Init();
            innerCanvas.OnMove += DrawShowSize;
        }

        public void GetScreenShoot()
        {
            screenSnapshot = HelperMethods.GetScreenSnapshot();

            if (screenSnapshot != null)
            {
                var bmp = screenSnapshot.ToBitmapSource();
                //var bmp = screenSnapshot.ToBitmapImage();
                if (bmp == null)
                    return;
                Background = new ImageBrush(bmp);
            }

            timeOutTimmer?.Start();
        }

        private void Init()
        {
            //ini normal properties
            //Topmost = true;
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            ShowInTaskbar = false;

            //set bounds to cover all screens
            var rect = SystemInformation.VirtualScreen;
            Left = rect.X;
            Top = rect.Y;
            Width = rect.Width;
            Height = rect.Height;

            //set background
            //AllowsTransparency = true;
            //Background = new SolidColorBrush(System.Windows.Media.Colors.Black);
            //Opacity = .3;

            //screenSnapshot = HelperMethods.GetScreenSnapshot();

            //if (screenSnapshot != null)
            //{
            //    var bmp = screenSnapshot.ToBitmapSource();
            //    if (bmp == null)
            //        return;
            //    bmp.Freeze();
            //    Background = new ImageBrush(bmp);
            //}

            //ini canvas
            innerCanvas = new MaskCanvas
            {
                MaskWindowOwner = this
            };
            Content = innerCanvas;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            //鼠标右键双击取消
            if (e.RightButton == MouseButtonState.Pressed && e.ClickCount >= 2)
            {
                CancelCapture();
            }

            CreatLabel(e.GetPosition(innerCanvas));
            label.Visibility = Visibility.Visible;
            if (toolBarContrl != null)
            {
                toolBarContrl.Visibility = Visibility.Hidden;
            }
        }

        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (timeOutTimmer != null && timeOutTimmer.Enabled)
            {
                timeOutTimmer.Stop();
                timeOutTimmer.Start();
            }
            //设置左上角label和右下角toolbar鼠标跟随
            Rect temRect = innerCanvas.GetSelectionRegion();
            if (temRect == Rect.Empty)
            {
                return;
            }
            label.Content = "选中区域大小：" + temRect.Width + "×" + temRect.Height + "宽：" + temRect.Width + "高：" + temRect.Height;
            if (label != null)
            {
                Canvas.SetLeft(label, temRect.X);
                Canvas.SetTop(label, temRect.Y - 25);
            }
            if (toolBarContrl != null)
            {
                Canvas.SetLeft(toolBarContrl, temRect.X + temRect.Width - 75);
                Canvas.SetTop(toolBarContrl, temRect.Y + temRect.Height);
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            CreatToolBar(e.GetPosition(innerCanvas));
            toolBarContrl.Visibility = Visibility.Visible;
        }

        //创建提示选中区域大小控件
        private void CreatLabel(System.Windows.Point location)
        {
            if (label == null)
            {
                label = new System.Windows.Controls.Label();
                innerCanvas.Children.Add(label);
            }
            label.Content = GetLabelContent();
            label.Height = 25;
            Canvas.SetLeft(label, location.X);
            Canvas.SetTop(label, location.Y - 25);
        }

        private void CreatToolBar(System.Windows.Point location)
        {
            if (toolBarContrl == null)
            {
                toolBarContrl = new ToolBarControl();
                innerCanvas.Children.Add(toolBarContrl);
                Canvas.SetLeft(toolBarContrl, location.X - 75);
                Canvas.SetTop(toolBarContrl, location.Y);
                toolBarContrl.OnOK += OKAction;
                toolBarContrl.OnCancel += CancelAction;
                toolBarContrl.OnSaveCapture += SaveCaptureAction;
            }
        }

        private string GetLabelContent()
        {
            string strContent = "";
            return strContent;
        }

        //protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
        //{
        //    base.OnKeyDown(e);

        //    if (e.Key == Key.Escape)
        //    {
        //        CancelCapture();
        //    }
        //}

        protected override void OnSourceInitialized(EventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(hwnd).AddHook(new HwndSourceHook(WndProc));
            base.OnSourceInitialized(e);
        }

        private const int WM_KEYDOWN = 0x0100;
        private const int PKeyEsc = 0x0000001b;

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_KEYDOWN:
                    switch (wParam.ToInt32())
                    {
                        case PKeyEsc:
                            CancelCapture();
                            break;
                    }
                    break;

                default:

                    break;
            }
            return IntPtr.Zero;
        }

        private void CancelCapture()
        {
            //Close();
            timeOutTimmer?.Stop();
            Dispose();
            screenCaptureOwner.OnScreenCaptureCancelled(null);
        }

        internal void OnShowMaskFinished(Rect maskRegion)
        {
        }

        internal void ClipSnapshot(Rect clipRegion)
        {
            BitmapSource caputredBmp = CopyFromScreenSnapshot(clipRegion);
            if (caputredBmp != null)
            {
                screenCaptureOwner?.OnScreenCaptured(null, caputredBmp);
            }
            //close mask window
            Dispose();
            //Close();
        }

        internal BitmapSource CopyFromScreenSnapshot(Rect region)
        {
            if (region.Width.Equals(0.0) || region.Height.Equals(0.0))
            {
                return null;
            }
            var sourceRect = region.ToRectangle();
            var destRect = new Rectangle(0, 0, sourceRect.Width, sourceRect.Height);

            if (screenSnapshot != null)
            {
                using (var bitmap = new Bitmap(sourceRect.Width, sourceRect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                {
                    using (var g = Graphics.FromImage(bitmap))
                    {
                        g.DrawImage(screenSnapshot, destRect, sourceRect, GraphicsUnit.Pixel);
                    }
                    return bitmap.ToBitmapSource();
                }
            }

            return null;
        }

        private void SaveCaptureAction()
        {
            using (m_bmpLayerCurrent = innerCanvas.GetSnapBitmap())
            {
                if (m_bmpLayerCurrent == null)
                {
                    return;
                }
                var saveDlg = new Microsoft.Win32.SaveFileDialog();
                string mydocPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                saveDlg.InitialDirectory = mydocPath/* + "\\"*/;
                saveDlg.Filter = "JPEG|*.jpg|PNG|*.png|Bitmap|*.bmp";
                saveDlg.FilterIndex = 2;
                saveDlg.FileName = "capture_" + DateTime.Now.ToLong().ToString();
                if (saveDlg.ShowDialog().Value)
                {
                    switch (saveDlg.FilterIndex)
                    {
                        case 1:
                            m_bmpLayerCurrent.Save(saveDlg.FileName,
                                System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;

                        case 2:
                            m_bmpLayerCurrent.Save(saveDlg.FileName,
                                System.Drawing.Imaging.ImageFormat.Png);
                            break;

                        case 3:
                            m_bmpLayerCurrent.Clone(new Rectangle(0, 0, m_bmpLayerCurrent.Width, m_bmpLayerCurrent.Height),
                                System.Drawing.Imaging.PixelFormat.Format24bppRgb).Save(saveDlg.FileName,
                                System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                    }
                }
            }
        }

        internal Bitmap CopyBitmapFromScreenSnapshot(Rect region)
        {
            if (region.Width.Equals(0.0) || region.Height.Equals(0.0))
            {
                return null;
            }
            var sourceRect = region.ToRectangle();
            var destRect = new Rectangle(0, 0, sourceRect.Width, sourceRect.Height);

            if (screenSnapshot != null)
            {
                var bitmap = new Bitmap(sourceRect.Width, sourceRect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.DrawImage(screenSnapshot, destRect, sourceRect, GraphicsUnit.Pixel);
                }
                return bitmap;
            }
            return null;
        }

        public void Show(System.Windows.Size? defaultSize)
        {
            if (innerCanvas != null)
            {
                innerCanvas.DefaultSize = defaultSize;
            }

            Show();
            Focus();
        }

        private void OnTimeOutTimmerTick(object sender, EventArgs e)
        {
            CancelCapture();
        }

        public void DrawShowSize(Rect rec)
        {
            if (rec == Rect.Empty)
            {
                return;
            }
            var wX = rec.Width;
            var hY = rec.Height;
            label.Content = "选中区域大小：" + wX + "×" + hY + "宽：" + wX + "高：" + hY;
        }

        public void OKAction()
        {
            innerCanvas.FinishAction();
        }

        public void CancelAction()
        {
            CancelCapture();
        }

        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonUp(e);
            if (toolBarContrl != null && label != null)
            {
                label.Visibility = Visibility.Hidden;
                toolBarContrl.Visibility = Visibility.Hidden;
            }
        }

        public void Dispose()
        {
            innerCanvas.ClearSelectedDataAndView();
            if (toolBarContrl != null && label != null)
            {
                label.Visibility = Visibility.Hidden;
                toolBarContrl.Visibility = Visibility.Hidden;
            }
            Visibility = Visibility.Collapsed;
            screenSnapshot?.Dispose();
            //GC.SuppressFinalize(screenSnapshot);
            screenSnapshot = null;
            Background = null;
        }
    }
}