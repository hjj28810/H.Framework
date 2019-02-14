using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace H.Framework.Core.Utilities
{
    public class HashEncryptHepler
    {
        public static string MD5Hash(byte[] bytes, MD5Format md5Format = MD5Format.x2)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var data = md5.ComputeHash(bytes);
                return string.Join(null, data.Select(x => x.ToString(Enum.GetName(typeof(MD5Format), md5Format))));
            }
        }

        public static string MD5Hash(Stream stream, MD5Format md5Format = MD5Format.x2)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var data = md5.ComputeHash(stream);
                return string.Join(null, data.Select(x => x.ToString(Enum.GetName(typeof(MD5Format), md5Format))));
            }
        }

        public static string MD5Hash(string str, MD5Format md5Format = MD5Format.x2, bool isFile = false)
        {
            //new FileIOPermission(FileIOPermissionAccess.Read, filename).Demand();

            using (var md5 = MD5.Create())
                if (isFile)
                    using (var stream = new FileStream(str, FileMode.Open, FileAccess.Read))
                    {
                        var data = md5.ComputeHash(stream);
                        return string.Join(null, data.Select(x => x.ToString(Enum.GetName(typeof(MD5Format), md5Format))));
                    }
                else
                {
                    var data = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                    return string.Join(null, data.Select(x => x.ToString(Enum.GetName(typeof(MD5Format), md5Format))));
                }
        }

        public static string SHA1Hash(string content)
        {
            try
            {
                using (var sha1 = new SHA1CryptoServiceProvider())
                {
                    var bytes_in = Encoding.UTF8.GetBytes(content);
                    var bytes_out = sha1.ComputeHash(bytes_in);
                    sha1.Dispose();
                    var result = BitConverter.ToString(bytes_out);
                    result = result.Replace("-", "");
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("SHA1加密出错：" + ex.Message);
            }
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="text">加密字符</param>
        /// <param name="password">加密的密码</param>
        /// <param name="iv">密钥</param>
        /// <returns></returns>
        public static string EncryptAES(string text, string password, string iv)
        {
            using (var rijndaelCipher = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128
            })
            {
                var pwdBytes = Encoding.UTF8.GetBytes(password);
                var keyBytes = new byte[32];
                var len = pwdBytes.Length;
                if (len > keyBytes.Length) len = keyBytes.Length;
                Array.Copy(pwdBytes, keyBytes, len);
                rijndaelCipher.Key = keyBytes;
                rijndaelCipher.IV = Encoding.UTF8.GetBytes(iv);
                var transform = rijndaelCipher.CreateEncryptor();
                var plainText = Encoding.UTF8.GetBytes(text);
                var cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);
                return Convert.ToBase64String(cipherBytes);
            }
        }

        /// <summary>
        /// 3DES加密
        /// </summary>
        /// <param name="text">加密字符</param>
        /// <param name="password">加密的密码</param>
        /// <param name="iv">密钥</param>
        /// <param name="mode">运算模式</param>
        /// <returns></returns>
        public static string Encrypt3DES(string text, string password, CipherMode cMode = CipherMode.ECB, PaddingMode pMode = PaddingMode.Zeros, string iv = "")
        {
            return Encrypt3DES(Encoding.UTF8.GetBytes(text), password, cMode, pMode, iv);
        }

        /// <summary>
        /// 3DES加密
        /// </summary>
        /// <param name="buffer">加密数组</param>
        /// <param name="password">加密的密码</param>
        /// <param name="iv">密钥</param>
        /// <param name="mode">运算模式</param>
        /// <returns></returns>
        public static string Encrypt3DES(byte[] buffer, string password, CipherMode cMode = CipherMode.ECB, PaddingMode pMode = PaddingMode.Zeros, string iv = "")
        {
            try
            {
                using (var des = new TripleDESCryptoServiceProvider
                {
                    Key = Encoding.UTF8.GetBytes(password),
                    Mode = cMode,
                    Padding = pMode
                })
                {
                    if (cMode == CipherMode.CBC)
                    {
                        des.IV = Encoding.UTF8.GetBytes(iv);
                    }
                    var desEncrypt = des.CreateEncryptor();
                    return Convert.ToBase64String(desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="text">加密的字符串</param>
        /// <param name="password">密钥</param>
        /// <param name="iv">解密矢量：只有在CBC解密模式下才适用</param>
        /// <param name="cMode">运算模式</param>
        /// <returns>解密的字符串</returns>
        public static byte[] Decrypt3DES(string text, string password, CipherMode cMode = CipherMode.ECB, PaddingMode pMode = PaddingMode.Zeros, string iv = "")
        {
            try
            {
                using (var des = new TripleDESCryptoServiceProvider
                {
                    Key = Encoding.UTF8.GetBytes(password),
                    Mode = cMode,
                    Padding = pMode
                })
                {
                    if (cMode == CipherMode.CBC)
                    {
                        des.IV = Encoding.UTF8.GetBytes(iv);
                    }
                    var buffer = Convert.FromBase64String(text);
                    return des.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    public enum MD5Format
    {
        X,
        x,
        X2,
        x2
    }
}