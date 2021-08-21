using H.Framework.Core.Utilities;
using H.Framework.Data.ORM.Foundations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace H.Framework.WPF.UI.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); tt();
        }

        private void tt()
        {
            FoundationDAL.ConnectedString = "Server=192.168.99.108;Database=diqiu_crm;User ID=root;Password=Dasong@;Port=3306;TreatTinyAsBoolean=false;SslMode=none;Allow User Variables=True;charset=utf8mb4";
            //var query = new WhereQueryable<UserDTO, Department, Role>((x, y, z) => true);

            //query = query.WhereAnd((x, y, z) => x.ID == "1");
            //var user = await new UserBLL().GetAsync(query, "Department,Roles");
            //new OrderBLL().AddOrder();
            //new CallRecordBLL().AddCallRecordAsync(new CallRecordDTO { Duration = 12321, CustomerID = "-1", Customer = null, ID = null, Phone = "12312312", RecordUrl = "", Remark = "阿斯达四大撒大所多阿萨德", Type = 1, UpdatedTime = null, User = null, UserDisplay = null, UserID = "85",CreatedTime = DateTime.MinValue });
            new CustomerBLL().GetAsync();
            //var a = "YHZxySCyG5iI0bnRNnlURwxVsqar3rdN07B8kNcEh7/Snfu5j3V44fWMJa/YcNmZ".AnalyseToken();
        }
    }

    public static class Exten
    {
        private const string _tokenPW = "qiK5jiZ7$rgBWVz1V*jJ!@ly7d2vxT8j";
        private const string _tokenIV = "AqIm%czX6M20mi9w";

        public static string AnalyseToken(this string original)
        {
            try
            {
                return HashEncryptHepler.DecryptAESToString(original, _tokenPW, Encoding.UTF8.GetBytes(_tokenIV));
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}