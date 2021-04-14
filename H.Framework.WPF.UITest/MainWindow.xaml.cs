using H.Framework.Core.Attributes;
using H.Framework.Core.Mapping;
using H.Framework.Core.Utilities;
using H.Framework.Data.ORM;
using H.Framework.Data.ORM.Attributes;
using H.Framework.Data.ORM.Foundations;
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

            tt();
        }

        private void tt()
        {
            FoundationDAL.ConnectedString = "Server=192.168.99.108;Database=diqiu_crm;User ID=root;Password=Dasong@;Port=3306;TreatTinyAsBoolean=false;SslMode=none;Allow User Variables=True;charset=utf8";
            //var query = new WhereQueryable<UserDTO, Department, Role>((x, y, z) => true);

            //query = query.WhereAnd((x, y, z) => x.ID == "1");
            //var user = await new UserBLL().GetAsync(query, "Department,Roles");
            //new OrderBLL().AddOrder();
            //new CallRecordBLL().AddCallRecordAsync(new CallRecordDTO { Duration = 12321, CustomerID = "-1", Customer = null, ID = null, Phone = "12312312", RecordUrl = "", Remark = "阿斯达四大撒大所多阿萨德", Type = 1, UpdatedTime = null, User = null, UserDisplay = null, UserID = "85",CreatedTime = DateTime.MinValue });
            new OrderBLL().GetOrdersAsync();
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

    public class TestSql
    {
        public static void Test()
        {
            FoundationDAL.ConnectedString = "Server=192.168.99.108;Database=Zeus;User ID=root;Password=Dasong@;Port=3306;TreatTinyAsBoolean=false;SslMode=none;Allow User Variables=True;charset=utf8";
            //var aa = new UserBLL();
            //var a = new ContractGroupBLL();
            //var aa = a.GetAAAsync();
            //aa.ContinueWith(x =>
            //{
            //    if (x.IsCompleted)
            //    {
            //        var bb = x.Result;
            //    }
            //});
            //aa.Update();
            //aa.Add();
            //var aa = a.Add(new List<Menu> { new Menu { Code = "aa", Name = "aaa", UserID = "999" } });
            //a.Update(new List<Menu> { new Menu { ID = "3", Name = "还好" } });
            //var bb = aa.GetList((a, a0) => a.UserID.Contains("','999") && a0.UserID == "999", 20, 0, "ListNotificationMark", null);
            //var aa = new MenuDAL();
            //aa.Delete(new List<string> { "1", "2" });
            //var aa = new FuturesCompanyBLL();
            //var a = aa.GetList(x => 1 == 1, "FuturesCompanyCounters");
            //var aa = new UserBLL();
            //aa.GetUsersWithLog(new UserStatusReq
            //{
            //    CreatedAtFrom = new DateTime(2020, 7, 28, 10, 10, 10),
            //    CreatedAtTo = new DateTime(2020, 7, 30, 10, 10, 10),
            //}); ;
        }

        public class FuturesCompanyBLL : BaseBLL<FuturesCompanyDTO, FuturesCompany, FuturesCompanyCounter, FuturesCompanyDAL>
        {
        }

        public class FuturesCompanyCounterDTO : IFoundationViewModel
        {
            public string ID { get; set; }
            public bool IsAuthorityValid { get; set; }
            public bool IsGateValid { get; set; }
            public string AuthCode { set; get; }
            public string GateAuthCode { get; set; }
            public bool IsMain { get; set; }
            public string Counter { get; set; }
        }

        public class FuturesCompanyDTO : IFoundationViewModel, ICustomMap<FuturesCompany>
        {
            public string Logo { get; set; }
            public string CreatedAt { get; set; }
            public string Name { get; set; }
            public string Description { set; get; }
            public string Phone { get; set; }
            public bool IsActive { get; set; }
            public string BrokerID { get; set; }

            public IEnumerable<FuturesCompanyCounterDTO> FuturesCompanyCounters { get; set; }
            public string ID { get; set; }

            public void MapFrom(FuturesCompany source)
            {
                FuturesCompanyCounters = source?.FuturesCompanyCounters?.MapAllTo(x => new FuturesCompanyCounterDTO());
            }
        }

        public class MenuDAL : BaseDAL<Menu>
        {
        }

        public class FuturesCompanyDAL : BaseDAL<FuturesCompany, FuturesCompanyCounter>
        {
        }

        public class FuturesCompanyCounter : IFoundationModel
        {
            [ForeignKeyID("futurescompany")]
            public string FuturesCompanyID { get; set; }

            public bool IsAuthorityValid { get; set; }
            public bool IsGateValid { get; set; }
            public string AuthCode { set; get; }
            public string GateAuthCode { get; set; }
            public bool IsMain { get; set; }
            public string Counter { get; set; }
            public string ID { get; set; }
        }

        public class FuturesCompany : IFoundationModel
        {
            public string Name { get; set; }

            public string CreatedAt { get; set; }

            public string Logo { get; set; }

            public string Phone { get; set; }

            public string Description { set; get; }

            public bool IsActive { get; set; }

            public string BrokerID { get; set; }

            [DetailList()]
            public List<FuturesCompanyCounter> FuturesCompanyCounters { get; set; }

            public string ID { get; set; }
        }

        public class NotificationDAL : BaseDAL<Notification, NotificationMark>
        {
            public Dictionary<string, IEnumerable<Notification>> GetUnreads()
            {
                return ExecuteQueryMutiSQL<Notification>(@"select count(*) as UnreadCount from notification a left join notificationmark b on a.id = b.NotificationID where a.NotificationTypeID = 1 and b.IsRead = 0 and b.isdeleted = 0 and b.userid = '12605911';select count(*) as UnreadCount from notification a left join notificationmark b on a.id = b.NotificationID where a.NotificationTypeID = 2 and b.IsRead = 0 and b.isdeleted = 0 and b.userid = '12605911';select count(*) as UnreadCount from notification a left join notificationmark b on a.id = b.NotificationID where a.NotificationTypeID = 3 and b.IsRead = 0 and b.isdeleted = 0 and b.userid = '12605911';select count(*) as UnreadCount from notification a left join notificationmark b on a.id = b.NotificationID where a.NotificationTypeID = 4 and b.IsRead = 0 and b.isdeleted = 0 and b.userid = '12605911';", new string[] { "1", "2", "3", "4" });
            }
        }

        public class NotificationBLL : BaseBLL<NotificationDTO, Notification, NotificationMark, NotificationDAL>
        {
            //public void Get()
            //{
            //    var query = new WhereQueryable<NotificationDTO, NotificationMark>((x, y) => y.UserID == "13321952950");
            //    var a = GetList(query, 10, 0, "NotificationMarks", new List<OrderByEntity> { new OrderByEntity { KeyWord = "CreatedAt", IsAsc = false } });
            //}

            public Dictionary<string, IEnumerable<Notification>> GetUnreads()
            {
                return DAL.GetUnreads();
            }

            public async void Add()
            {
                var a = await AddAsync(new NotificationDTO { Title = "ccccccc", Content = "asdasdasdadsa" });
            }
        }

        public class Menu : IFoundationModel
        {
            public string ID { get; set; }

            public string Code { get; set; }
            public string Name { get; set; }

            [LastIDCondition]
            public string UserID { get; set; }
        }

        public class NotificationMark : IFoundationModel
        {
            public string ID { get; set; }

            public bool IsRead { get; set; }

            [ForeignKeyID("notification")]
            public string NotificationID { get; set; }

            public string UserID { get; set; }
        }

        public class Notification : IFoundationModel
        {
            [DataFieldIgnore]
            public int UnreadCount { get; set; }

            public string UserID { get; set; }
            public string ID { get; set; }
            public string Title { get; set; }

            public string Content { get; set; }

            public string CreatedAt { get; set; }

            [DetailList]
            public List<NotificationMark> NotificationMarks { get; set; }
        }

        public class NotificationDTO : IFoundationViewModel
        {
            public string ID { get; set; }
            public string Title { get; set; }

            public string Content { get; set; }

            public string CreatedAt { get; set; }

            [MappingIgnore]
            public IEnumerable<NotificationMarkDTO> NotificationMarks { get; set; }
        }

        public class NotificationMarkDTO : IFoundationViewModel
        {
            public string ID { get; set; }

            public bool IsRead { get; set; }

            public int NotificationID { get; set; }

            public string UserID { get; set; }
        }

        //public class User : IFoundationModel
        //{
        //    public string ID { get; set; }
        //    public string Username { get; set; }

        //    public string Nickname { get; set; }
        //    public string Password { get; set; }
        //    public string Photo { get; set; }

        //    public string Description { get; set; }
        //    public string UpdatedAt { get; set; }
        //    public DateTime CreatedAt { get; set; }
        //    public int Level { get; set; }

        //    public string NicknameExt { get; set; }
        //    public string PhotoExt { get; set; }
        //    public string Privacy { get; set; }
        //    public string Cipher { get; set; }

        //    [DetailList()]
        //    public List<UserStatus> UserStatuss { get; set; }
        //}

        public class UserStatus : IFoundationModel
        {
            public string ID { get; set; }

            [ForeignKeyID("user")]
            public string UserID { get; set; }

            public string LastLoginIP { get; set; }

            public int LastLoginPlatform { get; set; }

            public DateTime LastLoginAt { get; set; }
        }

        public class UserStatusDTO : IFoundationViewModel
        {
            public string ID { get; set; }

            public string UserID { get; set; }

            public string LastLoginIP { get; set; }

            public int LastLoginPlatform { get; set; }

            public DateTime LastLoginAt
            {
                get; set;
            }

            //private string _lastLoginAt;

            //public string LastLoginAt
            //{
            //    get
            //    {
            //        if (DateTime.TryParse(_lastLoginAt, out DateTime lastLoginDate))
            //            return lastLoginDate.ToString("yyyy-MM-dd HH:mm:ss");
            //        return _lastLoginAt;
            //    }
            //    set { _lastLoginAt = value; }
            //}
        }

        //public class UserDTO : IFoundationViewModel, ICustomMap<User>
        //{
        //    public string ID { get; set; }
        //    public string Username { get; set; }
        //    public string Password { get; set; }
        //    public string Photo { get; set; }
        //    public string Token { get; set; }
        //    public string Nickname { get; set; }
        //    public bool IsSetPassword { get; set; }
        //    public string UpdatedAt { get; set; }

        //    public string Privacy { get; set; }
        //    public string Cipher { get; set; }

        //    //private string _createdAt;

        //    //public string CreatedAt
        //    //{
        //    //    get
        //    //    {
        //    //        if (DateTime.TryParse(_createdAt, out DateTime createdDate))
        //    //            return createdDate.ToString("yyyy-MM-dd HH:mm:ss");
        //    //        return _createdAt;
        //    //    }
        //    //    set { _createdAt = value; }
        //    //}

        //    public DateTime CreatedAt
        //    {
        //        get; set;
        //    }

        //    public string Description { get; set; }
        //    public int Level { get; set; }

        //    public string NicknameExt { get; set; }
        //    public string PhotoExt { get; set; }

        //    public bool IsRegistered { get; set; }
        //    public UserStatusDTO UserStatus { get; set; }

        //    public void MapFrom(User source)
        //    {
        //        UserStatus = source.UserStatuss?.First()?.MapTo(x => new UserStatusDTO());
        //    }
        //}

        public class UserDAL : BaseDAL<User, UserStatus>
        {
        }

        public class UserStatusReq
        {
            public DateTime? CreatedAtFrom { get; set; }

            public DateTime? CreatedAtTo { get; set; }

            public DateTime? LastLoginAtFrom { get; set; }

            public DateTime? LastLoginAtTo { get; set; }

            public string Username { get; set; }

            public int? Platform { get; set; }
        }

        public class UserBLL : BaseBLL<UserDTO, User, UserStatus, UserDAL>
        {
            //public void GetUsersWithLog(UserStatusReq req)
            //{
            //    var query = new WhereQueryable<UserDTO, UserStatus>((x, y) => 1 == 1);
            //    if (!string.IsNullOrWhiteSpace(req.Username))
            //        query = query.WhereAnd((x, y) => x.Username == req.Username);
            //    if (req.Platform.HasValue)
            //        query = query.WhereAnd((x, y) => y.LastLoginPlatform == req.Platform.Value);
            //    if (req.CreatedAtFrom.HasValue && req.CreatedAtTo.HasValue)
            //        query = query.WhereAnd((x, y) => x.CreatedAt > req.CreatedAtFrom.Value && x.CreatedAt < req.CreatedAtTo.Value);
            //    if (req.LastLoginAtFrom.HasValue && req.LastLoginAtTo.HasValue)
            //        query = query.WhereAnd((x, y) => y.LastLoginAt > req.LastLoginAtFrom.Value && y.LastLoginAt < req.LastLoginAtTo.Value);
            //    var list = GetList(query, 20, 0, "UserStatuss");
            //}

            //public void Update()
            //{
            //    var b = Encoding.Default.GetString(Convert.FromBase64String("eVRkd3JodWpNWjJmemhNWmVkaVJKWjlBNk45RFZReWw="));
            //    var list = GetList(x => 1 == 1, 20, 0);
            //    foreach (var item in list)
            //    {
            //        item.Cipher = HashEncryptHepler.EncryptAESToBase64(item.Username, b, b.Substring(0, 16));
            //    }

            //    Update(list);
            //}
        }

        public class Contract : IFoundationModel
        {
            public string ID { get; set; }

            [LastIDCondition]
            public string UserID { get; set; }

            public string Name { get; set; }

            public int Sort { get; set; }

            public int Type { get; set; }

            [ForeignKeyID("contractgroup")]
            public string ContractGroupID { get; set; }
        }

        public class ContractGroup : IFoundationModel
        {
            public string ID { get; set; }

            [LastIDCondition]
            public string UserID { get; set; }

            public string Name { get; set; }

            [DetailList()]
            public List<Contract> Contracts { get; set; }
        }

        public class ContractDTO : IFoundationViewModel
        {
            public string ID { get; set; }
            public string UserID { get; set; }

            public string Name { get; set; }
            public int Sort { get; set; }

            public int Type { get; set; }
        }

        public class ContractGroupDTO : IFoundationViewModel, ICustomMap<ContractGroup>
        {
            public string ID { get; set; }

            [LastIDCondition]
            public string UserID { get; set; }

            public string Name { get; set; }

            [MappingIgnore]
            public IEnumerable<ContractDTO> Contracts { get; set; }

            public void MapFrom(ContractGroup source)
            {
                Contracts = source?.Contracts?.MapAllTo(x => new ContractDTO());
            }
        }

        public class ContractGroupDAL : BaseDAL<ContractGroup, Contract>
        {
        }

        public class ContractGroupBLL : BaseBLL<ContractGroupDTO, ContractGroup, Contract, ContractGroupDAL>
        {
            //public async Task<IEnumerable<ContractGroupDTO>> GetAAAsync()
            //{
            //    return await GetListAsync((x, y) => x.UserID == "14", "Contracts");
            //}
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