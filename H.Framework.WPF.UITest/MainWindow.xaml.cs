using H.Framework.Core.Attributes;
using H.Framework.Core.Mapping;
using H.Framework.Core.Utilities;
using H.Framework.WPF.Control.Controls;
using H.Framework.WPF.Control.Controls.Capture;
using H.Framework.WPF.Infrastructure.Lists;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace H.Framework.WPF.UITest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : WindowEx, INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly ScreenCapture screenCapture = new ScreenCapture();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            //var a = ReadCSV();
            //var b = Encoding.Default.GetString(Convert.FromBase64String("eVRkd3JodWpNWjJmemhNWmVkaVJKWjlBNk45RFZReWw="));
            //foreach (DataRow row in a.Rows)
            //{
            //    var c = HashEncryptHepler.DecryptAESToString(row["Cipher"].ToString(), b, b.Substring(0, 16));
            //    row["phone"] = c;
            //}
            //SaveCSV(a, @"C:\Users\huasheng\Desktop\ccc.csv");
            //var a = HashEncryptHepler.MD5Hash(HashEncryptHepler.MD5Hash("18020019311I4kl$0bs"));

            ////var a = HashEncryptHepler.EncryptAESToBase64("10000115", b, b.Substring(0, 16));

            //var c = HashEncryptHepler.DecryptAESToStringCore(b, ")O[xx]6,YF}+eecaj{+oESb7d8>Z'e9N", "UXS9rr9^1wUBzVu#");
            //var a = "cM067Ca06ivfYFjcJyUwHQjyhNydLioNn5tLbr7ac3uRTH0z/iP2wSdkICSxEgw3".AnalyseToken();
            //ListNode = new ThreadSafeObservableCollection<Node>();
            //ListNode.CollectionChanged += ListNode_CollectionChanged;
            //TestSql.Test();
            //ListType.WriteJson("appSettings.json");

            //var l = List.BuildLine(x => x.ID, y => y.PID, p => p.Parent, (m, n) => new Node { Parent = n, ID = m.ID, PID = m.PID }, "1");
            //var a = new UserDB().GetUsers();
            //var ll = List.GetChildren(x => x.ID, x => x.PID, "22");
            //var aa = CurrentNode.ProcessParentValues(y => y.Parent);
        }

        public void SaveCSV(DataTable dt, string fullPath)
        {
            FileInfo fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            FileStream fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            string data = "";
            //写出列名称
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                data += dt.Columns[i].ColumnName.ToString();
                if (i < dt.Columns.Count - 1)
                {
                    data += ",";
                }
            }
            sw.WriteLine(data);
            //写出各行数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string str = dt.Rows[i][j].ToString();
                    str = str.Replace("\"", "\"\"");//替换英文冒号 英文冒号需要换成两个冒号
                    if (str.Contains(',') || str.Contains('"')
                        || str.Contains('\r') || str.Contains('\n')) //含逗号 冒号 换行符的需要放到引号中
                    {
                        str = string.Format("\"{0}\"", str);
                    }

                    data += str;
                    if (j < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
            }
            sw.Close();
            fs.Close();
        }

        public DataTable ReadCSV()
        {
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(@"C:\Users\huasheng\Desktop\bbb.csv", System.IO.FileMode.Open, System.IO.FileAccess.Read);

            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            //string fileContent = sr.ReadToEnd();
            //encoding = sr.CurrentEncoding;
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                //strLine = Common.ConvertStringUTF8(strLine, encoding);
                //strLine = Common.ConvertStringUTF8(strLine);

                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (aryLine != null && aryLine.Length > 0)
            {
                dt.DefaultView.Sort = tableHead[0] + " " + "asc";
            }

            sr.Close();
            fs.Close();
            return dt;
        }

        public List<KeyValueModel> ListType { get; } = new List<KeyValueModel> {
                new KeyValueModel{ Key="today",Value="仅当前交易日有效",Type="当日"},
                new KeyValueModel{ Key="forever",Value="一直有效，直至触发或无持仓vvv",Type="永久"},
        };

        private Visibility _SBVisibility = Visibility.Collapsed;

        public Visibility SBVisibility { get => _SBVisibility; set { _SBVisibility = value; OnPropertyChanged("SBVisibility"); } }

        private bool _SBShow;

        public bool SBShow
        {
            get => _SBShow;
            set { _SBShow = value; OnPropertyChanged("SBShow"); }
        }

        private int? _testText;

        //[DataValidation(typeof(TestValidation), "Test", "this")]
        public int? TestText
        {
            get => _testText;
            set
            {
                _testText = value;
                OnPropertyChanged("TestText");
            }
        }

        private ThreadSafeObservableCollection<Node> _listNode;

        //[DataValidation(typeof(TestValidation), "Test", "this")]
        public ThreadSafeObservableCollection<Node> ListNode
        {
            get => _listNode;
            set
            {
                _listNode = value;
                OnPropertyChanged("ListNode");
            }
        }

        private string _testBtnText;

        //[Required(ErrorMessage = "sdsadasd")]
        public string TestBtnText
        {
            get => _testBtnText;
            set
            {
                _testBtnText = value;
                OnPropertyChanged("TestBtnText");
            }
        }

        private bool _ppOpen;

        //[Required(ErrorMessage = "sdsadasd")]
        public bool PpOpen
        {
            get => _ppOpen;
            set
            {
                _ppOpen = value;
                OnPropertyChanged("PpOpen");
                Trace.WriteLine(_ppOpen.ToString());
            }
        }

        private string _pw;

        //[Required(ErrorMessage = "sdsadasd")]
        public string PW
        {
            get => _pw;
            set
            {
                _pw = value;
                OnPropertyChanged("PW");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            ListNode = new ThreadSafeObservableCollection<Node> { new Node { ID = "11" },
                new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" },
 new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "33" },
new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "44" },
new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "22" }, new Node { ID = "55" }};
            var aaa = ListNode.ForAny(x => x.ID == "111");
            var a = TimeHelper.CurrentServerTime;
            var aaaa = DateTime.Parse("2019.06.08");
            PpOpen = !PpOpen;
            if (SBVisibility == Visibility.Visible)
                SBVisibility = Visibility.Collapsed;
            else
                SBVisibility = Visibility.Visible;
            SBShow = !SBShow;
            //if (busyC.Visibility == Visibility)
            //    busyC.Visibility = Visibility.Collapsed;
            //else
            //    busyC.Visibility = Visibility.Visible;
            //warnBlock.Show(SBVisibility.ToString(), Control.Controls.AlertStyle.Info);
            //warmBlock.Show("haaaaaa");
            //warmBlock.Show("ggggggggggggggggggg");
            //warmBlock.Show("ha");

            var list = new List<string>();
            var list2 = new string[] { "1", "2" };
            list.AddRangeNoRept(list2);
            //var g = new GWindow();
            //pwb.Password = "asdasd";
            //g.Show();
            //screenCapture.StartCapture(30);

            //swingLoading.ShowUp = swingLoading.ShowUp == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void ListNode_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //TimeHelper.ServerInitTime = DateTime.Parse("2019-03-29 16:02:00");
            //SBVisibility = Visibility.Collapsed;
            //var a = "asdasd.pdf".IsMatchFileExt(".pdf");
            //LogHelper.Register(new Log4NetLogger("Error"), "Error");
            //LogHelper.Register(new Log4NetLogger("Business"), "Business");
            //LogHelper.Register(new Log4NetLogger("System"), "System");
            //LogHelper.WriteLogFile(new LogMessage<string> { Title = "消息处理XAML", Data = "adsasd" }, LogType.System);

            //List<IDictionary<string, string>> ll = new List<IDictionary<string, string>>();
            //IDictionary<string, string> dict = new Dictionary<string, string>();
            //dict.Add("ID", "2");
            //dict.Add("IsRead", "true");
            //dict.Add("NotificationID", "3");
            //ll.Add(dict);
            //var lll = ll.ToEnumerable<NotificationMarkDTO>();
            //var a21 = lll.MapAllTo(x => new NotificationDTO());
            //var ste = "asdasd > 2019/2/28 18:22:33 && asdasd < 2017/12/01 12:23:43";
            //var r = ste.IsMatchDateTime('/');
            //var r1 = ste.MatchsDateTime('/');
            //var aaa = Get().BuildLine(aa => aa.ID, bb => bb.PID, m => m.Parent, (c, p) => new Node { ID = c.ID, PID = c.PID, Parent = p }, "222");
            //var l = (new DateTime(2018, 9, 1)).ToLong(true);

            //var aaaaa = HashEncryptHepler.MD5Hash("appId=wx329328d6d1af8bd8&customer_weixin=wxid_411rjwe7lvgz22&secret=d16dc6fe103c3fc600fe13903a0ed5d2&timestamp=1537953627&user_id=100100&weixin=HUANGBO19891006", MD5Format.X);
            //var Hash = HashEncryptHepler.EncryptAESToBase64("1|1566467834", "_accessToken", "1111111111111111", 128, System.Security.Cryptography.CipherMode.CFB);

            //var str = HashEncryptHepler.DecryptAESToString(Hash, "_accessToken", "1111111111111111");
            //var aHash = HashEncryptHepler.Decrypt3DES("JxVJzmZzKlcAybCaXnn5Odjfrtnw5kBu3LOYOsQK0Yg7tepNAeJyQNjfrtnw5kBu3LOYOsQK0YiBxFDnQlo9blYD44J1lr32UpKh1sMF33nufbggphIXmT1AlmRkHN4pPJuVnFfEr1A3TlSnKvy6b4ylyguVd/tLom8xHaOTBbQRi0y1K88cvpKQs/4z7Cb5J0O3PMFYGpENPz6Cth+7oNyAERMhVzFf3QLayjEkZX5bRfc9uSHeUTpEWJoHUk9NibNstXZZ6HXEWXw+l2ERlQeqFLYYzH7vkajkuba6xz74TQonW6217ccPCb29orG9JLm5OBN19P1erlA7W2gmenIM9RwLHMmbBTnFsTeqJxwnx5C28kkdprsv5psuYFEfur1h1Yx38EROrYr3sU4BbFagL2nwLKcCeaxFkOapZ2F7OY+2EEL7TZLcvqikX3qho2KH07j8eDkP6UcpU0WohK/O3LfG3ZbibOZXip9pd4BRauwLaPOw2gU1zrgPh3/Qx1GPAXR2pC6J", "SKFMNGHJVBNDKI=56ELBGKFW");
        }

        private Node CurrentNode => new Node { ID = "5", PID = "3", Parent = new Node { ID = "3", PID = "1", Parent = new Node { ID = "1", PID = "0" } } };

        public IEnumerable<Node> Get()
        {
            return new List<Node> { new Node { ID = "1", PID = "0" }/*, new Node { ID = "11", PID = "1" }, new Node { ID = "22", PID = "1" }, new Node { ID = "33", PID = "1" }, new Node { ID = "111", PID = "11" }, new Node { ID = "112", PID = "11" }, new Node { ID = "1111", PID = "111" }, new Node { ID = "222", PID = "22" }, new Node { ID = "2223", PID = "22" }, new Node { ID = "2225", PID = "22" }, new Node { ID = "2212", PID = "22" }, new Node { ID = "5222", PID = "22" } */};
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                try
                {
                    return this.ValidateProperty(columnName);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<Node> List => Get().ToList();

        public int ListCount
        {
            get;
            set;
        } = 120;

        //[Required(ErrorMessage = "sdsadasd")]
        public int PageSize
        {
            get;
            set;
        }

        private int _currentPage = 1;

        //[Required(ErrorMessage = "sdsadasd")]
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged("CurrentPage");
                Trace.WriteLine(_currentPage);
            }
        }

        private void Sss_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
            IsShowTip = true;
            CurrentPage = 1;
        }

        private bool _isShowTip;

        //[Required(ErrorMessage = "sdsadasd")]
        public bool IsShowTip
        {
            get => _isShowTip;
            set
            {
                _isShowTip = value;
                OnPropertyChanged("IsShowTip");
            }
        }

        private void ButtonEx_Click(object sender, RoutedEventArgs e)
        {
            IsShowTip = false;
        }

        private void WindowEx_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        }

        private void sss_Drop(object sender, DragEventArgs e)
        {
            var A = e.Data.GetFormats();
        }

        private void sss_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Link;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }
    }

    public class KeyValueModel
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public string Type { get; set; }
    }

    public class Node
    {
        public string ID { get; set; }
        public string PID { get; set; }
        public IEnumerable<Node> Children { get; set; }

        public Node Parent { get; set; }
    }

    public class TestValidation
    {
        public static void Test(string value, MainWindow a)
        {
        }
    }
}

public static class Exten
{
    //private const string _tokenPW = "qiK5jiZ7$rgBWVz1V*jJ!@ly7d2vxT9j";
    //private const string _tokenIV = "AqIm%czX6M20mi8w";

    //public static string AnalyseToken(this string original)
    //{
    //    try
    //    {
    //        return HashEncryptHepler.DecryptAESToString(original, _tokenPW, _tokenIV);
    //    }
    //    catch
    //    {
    //        return string.Empty;
    //    }
    //}
}