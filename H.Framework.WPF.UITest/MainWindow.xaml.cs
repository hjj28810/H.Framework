using H.Framework.Core.Log;
using H.Framework.Core.Utilities;
using H.Framework.WPF.Control.Controls.Capture;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace H.Framework.WPF.UITest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly ScreenCapture screenCapture = new ScreenCapture();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private Visibility _SBVisibility = Visibility.Visible;

        public Visibility SBVisibility { get => _SBVisibility; set { _SBVisibility = value; OnPropertyChanged("SBVisibility"); } }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (SBVisibility == Visibility) SBVisibility = Visibility.Collapsed;
            else SBVisibility = Visibility.Visible;
            warnBlock.Show("hahahahah", Control.Controls.AlertStyle.Info);
            //warmBlock.Show("haaaaaa");
            //warmBlock.Show("ggggggggggggggggggg");
            //warmBlock.Show("ha");

            var list = new List<string>();
            var list2 = new string[] { "1", "2" };
            list.AddRangeNoRept(list2);

            screenCapture.StartCapture(30);
            //swingLoading.ShowUp = swingLoading.ShowUp == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SBVisibility = Visibility.Collapsed;
            var a = "asdasd.pdf".IsMatchFileExt(".pdf");
            LogHelper.Register(new Log4NetLogger("Error"), "Error");
            LogHelper.Register(new Log4NetLogger("Business"), "Business");
            LogHelper.WriteLogFileAsync(new LogMessage<string> { Title = "消息处理XAML", Data = "adsasd" }, LogType.Business);

            var ste = "asdasd > 2017/02/01 18:22:33 && asdasd < 2017/03/01 12:23:43";
            var r = ste.IsMatchTime();
            var r1 = ste.MatchsTime();
        }
    }
}