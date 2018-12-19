using H.Framework.Core.Utilities;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace H.Framework.WPF.Control.Controls
{
    [TemplatePart(Name = "PART_Grid", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_Bar", Type = typeof(ProgressBar))]
    public class DownloadComponent : System.Windows.Controls.Control
    {
        private Grid PART_Panel;
        private ProgressBar PART_Bar;

        private FileHelper _fileHelper;

        static DownloadComponent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DownloadComponent), new FrameworkPropertyMetadata(typeof(DownloadComponent)));
            ClipToBoundsProperty.OverrideMetadata(typeof(DownloadComponent), new FrameworkPropertyMetadata(false));
        }

        public DownloadComponent()
        {
            _fileHelper = new FileHelper();
            _fileHelper.DownLoadStatusCallBackHandle = DownLoadStatusCallBack;
        }

        public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register("FileName", typeof(string), typeof(DownloadComponent), new PropertyMetadata("", null));

        /// <summary>
        /// 文件名
        /// </summary>
        [Description("获取或设置文件名")]
        [Category("Defined Properties")]
        public string FileName
        {
            get => (string)GetValue(FileNameProperty);
            set => SetValue(FileNameProperty, value);
        }

        public static readonly DependencyProperty FileSizeProperty = DependencyProperty.Register("FileSize", typeof(long), typeof(DownloadComponent), new PropertyMetadata((long)0, OnFileSizePropertyChangedCallback));

        /// <summary>
        /// 文件大小
        /// </summary>
        [Description("获取或设置文件大小")]
        [Category("Defined Properties")]
        public long FileSize
        {
            get => (long)GetValue(FileSizeProperty);
            set => SetValue(FileSizeProperty, value);
        }

        public static void OnFileSizePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = (double)(long)e.NewValue;
            if (value != 0)
                (d as DownloadComponent).FileSizeReadable = value.ReadableFilesize();
        }

        public static readonly DependencyProperty FileSizeReadableProperty = DependencyProperty.Register("FileSizeReadable", typeof(string), typeof(DownloadComponent), new PropertyMetadata("", null));

        /// <summary>
        /// 可读文件大小
        /// </summary>
        [Description("获取或设置可读文件大小")]
        [Category("Defined Properties")]
        public string FileSizeReadable
        {
            get => (string)GetValue(FileSizeReadableProperty);
            set => SetValue(FileSizeReadableProperty, value);
        }

        public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register("FilePath", typeof(string), typeof(DownloadComponent), new PropertyMetadata("", OnFilePathPropertyChangedCallback));

        /// <summary>
        /// 文件路径
        /// </summary>
        [Description("获取或设置文件路径")]
        [Category("Defined Properties")]
        public string FilePath
        {
            get => (string)GetValue(FilePathProperty);
            set => SetValue(FilePathProperty, value);
        }

        public static void OnFilePathPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as DownloadComponent;
            if (e.NewValue != null && string.IsNullOrWhiteSpace(c.FileName))
            {
                c.FileName = Path.GetFileName(e.NewValue.ToString());
                c.FileExt = Path.GetExtension(e.NewValue.ToString()).TrimStart('.');
            }
        }

        public static readonly DependencyProperty IconSourceProperty = DependencyProperty.Register("IconSource", typeof(ImageSource), typeof(DownloadComponent), new PropertyMetadata(new BitmapImage(new Uri("pack://application:,,,/H.Framework.WPF.Control;component/Resources/Icons/downFile.png", UriKind.RelativeOrAbsolute)), null));

        /// <summary>
        /// icon图片
        /// </summary>
        [Description("获取或设置icon图片")]
        [Category("Defined Properties")]
        public ImageSource IconSource
        {
            get => (ImageSource)GetValue(IconSourceProperty);
            set => SetValue(IconSourceProperty, value);
        }

        public static readonly DependencyProperty IsShowMenuProperty = DependencyProperty.Register("IsShowMenu", typeof(bool), typeof(DownloadComponent), new PropertyMetadata(true, OnIsShowMenuPropertyChangedCallback));

        /// <summary>
        /// 是否显示右键菜单
        /// </summary>
        [Description("获取或设置是否显示右键菜单")]
        [Category("Defined Properties")]
        public bool IsShowMenu
        {
            get => (bool)GetValue(IsShowMenuProperty);
            set => SetValue(IsShowMenuProperty, value);
        }

        public static readonly DependencyProperty BrushColorProperty = DependencyProperty.Register("BrushColor", typeof(Brush), typeof(DownloadComponent), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(63, 151, 254)), null));

        /// <summary>
        /// 整体颜色
        /// </summary>
        [Description("获取或设置整体颜色")]
        [Category("Defined Properties")]
        public Brush BrushColor
        {
            get => (Brush)GetValue(BrushColorProperty);
            set => SetValue(BrushColorProperty, value);
        }

        public static void OnIsShowMenuPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as DownloadComponent;
            c.ShowMenu();
        }

        public static readonly DependencyProperty IsDownLoadedProperty = DependencyProperty.Register("IsDownLoaded", typeof(bool), typeof(DownloadComponent), new PropertyMetadata(false, null));

        /// <summary>
        /// 是否已下载
        /// </summary>
        [Description("获取或设置是否已下载")]
        [Category("Defined Properties")]
        public bool IsDownLoaded
        {
            get => (bool)GetValue(IsDownLoadedProperty);
            set => SetValue(IsDownLoadedProperty, value);
        }

        public static readonly DependencyProperty SavePathProperty = DependencyProperty.Register("SavePath", typeof(string), typeof(DownloadComponent), new PropertyMetadata("", null));

        /// <summary>
        /// 保存的路径
        /// </summary>
        [Description("获取或设置保存的路径")]
        [Category("Defined Properties")]
        public string SavePath
        {
            get => (string)GetValue(SavePathProperty);
            set => SetValue(SavePathProperty, value);
        }

        public static readonly DependencyProperty FileExtProperty = DependencyProperty.Register("FileExt", typeof(string), typeof(DownloadComponent), new PropertyMetadata("", null));

        /// <summary>
        /// 文件扩展名
        /// </summary>
        [Description("获取或设置文件扩展名")]
        [Category("Defined Properties")]
        public string FileExt
        {
            get => (string)GetValue(FileExtProperty);
            set => SetValue(FileExtProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_Panel = (Grid)GetTemplateChild("PART_Panel");
            PART_Bar = (ProgressBar)GetTemplateChild("PART_Bar");
            PART_Panel.MouseLeftButtonDown += this_MouseLeftButtonDown;
            ShowMenu();
        }

        private void ShowMenu()
        {
            if (IsShowMenu)
            {
                if (!IsDownLoaded)
                    ContextMenu = GetSaveMenu();
                else
                    ContextMenu = GetOpenMenu();
            }
            else
                ContextMenu = null;
        }

        private ContextMenu GetSaveMenu()
        {
            var menuItem = new MenuItem();
            menuItem.Header = "另存为";
            menuItem.Click += MenuSaveItem_Click;
            var context = new ContextMenu();
            context.Items.Add(menuItem);
            return context;
        }

        private ContextMenu GetOpenMenu()
        {
            var menuItem = new MenuItem();
            menuItem.Header = "打开";
            menuItem.Click += MenuOpenItem_Click;
            var context = new ContextMenu();
            context.Items.Add(menuItem);
            return context;
        }

        private void this_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && IsShowMenu)
            {
                if (!IsDownLoaded)
                {
                    if (string.IsNullOrWhiteSpace(SavePath))
                        SavePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads\" + FileName;
                    else
                        SavePath += FileName;
                    BeginDownload();
                }
                else
                    OpenFile();
            }
        }

        private void BeginDownload()
        {
            var url = FilePath;
            var path = SavePath;
            PART_Bar.Visibility = Visibility.Visible;
            var t = Task.Run(() => { return _fileHelper.HttpDownload(url, path, true); });
            t.ContinueWith((end) =>
            {
                if (end.IsCompleted)
                {
                    Dispatcher.Invoke(() =>
                    {
                        if (end.Result)
                            IsDownLoaded = end.Result;
                        ShowMenu();
                        PART_Bar.Visibility = Visibility.Collapsed;
                        OpenFile();
                    });
                }
            });
        }

        private void DownLoadStatusCallBack(double status)
        {
            Dispatcher.Invoke(() => PART_Bar.Value = status);
        }

        private void MenuSaveItem_Click(object sender, RoutedEventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.InitialDirectory = @"D:\";
            sfd.FileName = FileName;
            var ext = Path.GetExtension(FileName);
            sfd.Filter = ext.TrimStart('.') + "文件|" + ext;
            if (sfd.ShowDialog() == true)
            {
                SavePath = sfd.FileName;
                BeginDownload();
            }
        }

        private void MenuOpenItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        private void OpenFile()
        {
            if (File.Exists(SavePath))
                Process.Start(SavePath);
            else
            {
                IsDownLoaded = false;
                ShowMenu();
                MessageBox.Show("文件未找到，请重新下载", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}