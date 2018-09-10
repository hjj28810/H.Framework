using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace H.Framework.Core.Utilities
{
    public class HashEncryptHepler
    {
        public static string MD5Hash(byte[] bytes)
        {
            var md5 = new MD5CryptoServiceProvider();
            var data = md5.ComputeHash(bytes);

            return string.Join(null, data.Select(x => x.ToString("x")));
        }

        public static string MD5Hash(Stream stream)
        {
            var md5 = new MD5CryptoServiceProvider();
            var data = md5.ComputeHash(stream);

            return string.Join(null, data.Select(x => x.ToString("x")));
        }

        public static string MD5Hash(string filename)
        {
            //new FileIOPermission(FileIOPermissionAccess.Read, filename).Demand();

            var md5 = new MD5CryptoServiceProvider();
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var data = md5.ComputeHash(stream);
                return string.Join(null, data.Select(x => x.ToString("x")));
            }
        }

        public static string SHA1Hash(string content)
        {
            try
            {
                var sha1 = new SHA1CryptoServiceProvider();
                var bytes_in = Encoding.UTF8.GetBytes(content);
                var bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();
                var result = BitConverter.ToString(bytes_out);
                result = result.Replace("-", "");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("SHA1加密出错：" + ex.Message);
            }
        }
    }
}