using H.Framework.Core.Log;
using H.Framework.Core.Utilities;
using H.Framework.WPF.Control.Controls;
using H.Framework.WPF.Control.Controls.Capture;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace H.Framework.WPF.UITest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : WindowEx, INotifyPropertyChanged
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
            LogHelper.Register(new Log4NetLogger("System"), "System");
            LogHelper.WriteLogFile(new LogMessage<string> { Title = "消息处理XAML", Data = "adsasd" }, LogType.System);

            var ste = "asdasd > 2019/2/28 18:22:33 && asdasd < 2017/12/01 12:23:43";
            var r = ste.IsMatchDateTime('/');
            var r1 = ste.MatchsDateTime('/');
            var aaa = Get().BuildLine(aa => aa.ID, bb => bb.PID, m => m.Parent, (c, p) => new Node { ID = c.ID, PID = c.PID, Parent = p }, "222");
            var l = (new DateTime(2018, 9, 1)).ToLong();

            var aaaaa = HashEncryptHepler.MD5Hash("appId=wx329328d6d1af8bd8&customer_weixin=wxid_411rjwe7lvgz22&secret=d16dc6fe103c3fc600fe13903a0ed5d2&timestamp=1537953627&user_id=100100&weixin=HUANGBO19891006", MD5Format.X);
            var Hash = HashEncryptHepler.Encrypt3DES("123456789", "SKFMNGHJVBNDKI=56ELBGKFW");
            var aHash = HashEncryptHepler.Decrypt3DES("Mk3H0XFcl6gS1rKMAG2woL0fG2ni9AcrcbOE2DVbKRgSs4eWMadeHtPVlBBAQqDwtYXKFo8mBFlyuOkuB/fMEvnwJdNlbsegaAwKf73H6nCdhyUWZOlVtwpY9mzQf/oU", "SKFMNGHJVBNDKI=56ELBGKFW");
        }

        public IEnumerable<Node> Get()
        {
            return new List<Node> { new Node { ID = "1", PID = "0" }, new Node { ID = "11", PID = "1" }, new Node { ID = "22", PID = "1" }, new Node { ID = "33", PID = "1" }, new Node { ID = "111", PID = "11" }, new Node { ID = "112", PID = "11" }, new Node { ID = "1111", PID = "111" }, new Node { ID = "222", PID = "22" } };
        }
    }

    public class Node
    {
        public string ID { get; set; }
        public string PID { get; set; }
        public IEnumerable<Node> Children { get; set; }

        public Node Parent { get; set; }
    }
}