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
            InitializeComponent();
            tt();
            //TTT();
        }

        private void tt()
        {
            FoundationDAL.ConnectedString = "Server=10.68.99.108;Database=diqiu_crm;User ID=root;Password=Dasong@;Port=3306;TreatTinyAsBoolean=false;SslMode=none;Allow User Variables=True;charset=utf8mb4";
            //var query = new WhereQueryable<UserDTO, Department, Role>((x, y, z) => true);

            //query = query.WhereAnd((x, y, z) => x.ID == "1");
            //var user = await new UserBLL().GetAsync(query, "Department,Roles");
            //new OrderBLL().AddOrder();
            //new CallRecordBLL().AddCallRecordAsync(new CallRecordDTO { Duration = 12321, CustomerID = "-1", Customer = null, ID = null, Phone = "12312312", RecordUrl = "", Remark = "阿斯达四大撒大所多阿萨德", Type = 1, UpdatedTime = null, User = null, UserDisplay = null, UserID = "85", CreatedTime = DateTime.MinValue });
            //new CustomerBLL().GetAsync();
            new CallRecordBLL().GetAsync();
            //var a = "YHZxySCyG5iI0bnRNnlURwxVsqar3rdN07B8kNcEh7/Snfu5j3V44fWMJa/YcNmZ".AnalyseToken();
        }

        private void TTT()
        {
            var t = new Test();
            string inputmsg = "abefc501010008000000057102ac03e8ab0014b80295c40284c4057102b203e8b60014c60295c90284c40403e8b50014b70295c00284c1057102b603e8ae0014b80295bd0284c2047102aa03e8ad0295c20284c0047102a603e8b10295c40284be040295ba0014c00284c17102b70403e8af0295bf0014ba0284b20503e8ac0295bc0014be0284c07102b00503e8b50295c00014bd0284b57102a70503e8a80295c20014b80284ba7102af0403e8b20295c40014c20284bb0403e8b40295bf0014c17102bf0503e8b40295c30014ca0284c67102b20403e8b50295cc0014ca0284c80303e8b80295ca7102b90403e8b20295b90284c37102a90403e8b60295ca0014cb7102b2040295ca0014c90284bf7102b9050295c20014c70284c77102b003e8b1050295ca0014cb0284c57102ad03e8b5050295cd0014cb0284c67102af03e8b4040014c50284c17102bb03e8b8050014ca0284c37102a603e8b20295ca050284c57102b003e8b30295ca0014ca050284c17102b903e8b70295ca0014ca050284c57102ab03e8b20295be0014c7030284c503e8b60295ca037102b003e8b20014ca057102ba03e8b60295be0014c50284c60244030000ff0000000000000000000000000000db";
            byte[] message = t.HexStringToBytes(inputmsg.ToUpper());
            t.SaveMessage(message);
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