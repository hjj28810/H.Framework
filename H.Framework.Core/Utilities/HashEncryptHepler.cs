using Jose;
using System;
using System.Collections.Generic;
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

        public static string SHA256Hash(string content)
        {
            try
            {
                using (var sha256 = new SHA256CryptoServiceProvider())
                {
                    var bytes_in = Encoding.UTF8.GetBytes(content);
                    var bytes_out = sha256.ComputeHash(bytes_in);
                    sha256.Dispose();
                    var result = BitConverter.ToString(bytes_out);
                    result = result.Replace("-", "");
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("SHA256加密出错：" + ex.Message);
            }
        }

        /// <summary>
        /// AES加密 pw长度，支持16,24,32 分别对应128 192 256
        /// </summary>
        /// <param name="text"></param>
        /// <param name="password"></param>
        /// <param name="iv"></param>
        /// <param name="mode"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        public static byte[] EncryptAES(string text, string password, byte[] iv = null, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        {
            using (var rijndaelCipher = new RijndaelManaged
            {
                Mode = mode,
                Padding = padding
            })
            {
                rijndaelCipher.Key = Encoding.UTF8.GetBytes(password);
                if (iv == null && mode != CipherMode.ECB)
                {
                    throw new Exception("除ECB模式，iv不能为空");
                }
                rijndaelCipher.IV = iv;
                var transform = rijndaelCipher.CreateEncryptor();
                var plainText = Encoding.UTF8.GetBytes(text);
                return transform.TransformFinalBlock(plainText, 0, plainText.Length);
            }

            //using (var aes = new AesCryptoServiceProvider
            //{
            //    Mode = mode,
            //    Padding = padding
            //})
            //{
            //    var pwdBytes = Encoding.UTF8.GetBytes(password);
            //    var keyBytes = new byte[32];
            //    var len = pwdBytes.Length;
            //    if (len > keyBytes.Length) len = keyBytes.Length;
            //    Array.Copy(pwdBytes, keyBytes, len);
            //    aes.Key = keyBytes;
            //    aes.IV = Encoding.UTF8.GetBytes(iv);
            //    var enc = aes.CreateEncryptor(aes.Key, aes.IV);
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        using (CryptoStream cs = new CryptoStream(ms, enc, CryptoStreamMode.Write))
            //        {
            //            var bts = Encoding.UTF8.GetBytes(text);
            //            cs.Write(bts, 0, bts.Length);
            //        }
            //        return ms.ToArray();
            //    }
            //}
        }

        /// <summary>
        /// AES加密成Base64 pw长度，支持16,24,32 分别对应128 192 256
        /// </summary>
        /// <param name="text"></param>
        /// <param name="password"></param>
        /// <param name="iv"></param>
        /// <param name="mode"></param>
        /// <param name="padding"></param
        /// <returns></returns>
        public static string EncryptAESToBase64(string text, string password, byte[] iv = null, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        {
            return Convert.ToBase64String(EncryptAES(text, password, iv, mode, padding));
        }

        /// <summary>
        /// AES解密 pw长度，支持16,24,32 分别对应128 192 256
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="password"></param>
        /// <param name="iv"></param>
        /// <param name="mode"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        public static byte[] DecryptAES(byte[] buffer, string password, byte[] iv = null, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        {
            try
            {
                using (var rijndaelCipher = new RijndaelManaged
                {
                    Mode = mode,
                    Padding = padding
                })
                {
                    rijndaelCipher.Key = Encoding.UTF8.GetBytes(password);
                    if (iv == null && mode != CipherMode.ECB)
                    {
                        throw new Exception("除ECB模式，iv不能为空");
                    }
                    rijndaelCipher.IV = iv;
                    var transform = rijndaelCipher.CreateDecryptor();
                    return transform.TransformFinalBlock(buffer, 0, buffer.Length);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// AES解密Base64字符串 pw长度，支持16,24,32 分别对应128 192 256
        /// </summary>
        /// <param name="base64text"></param>
        /// <param name="password"></param>
        /// <param name="iv"></param>
        /// <param name="mode"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        public static byte[] DecryptAES(string base64text, string password, byte[] iv = null, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        {
            return DecryptAES(Convert.FromBase64String(base64text), password, iv, mode, padding);
        }

        /// <summary>
        /// AES解密成string pw长度，支持16,24,32 分别对应128 192 256
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="password"></param>
        /// <param name="iv"></param>
        /// <param name="mode"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        public static string DecryptAESToString(byte[] buffer, string password, byte[] iv = null, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        {
            return Encoding.UTF8.GetString(DecryptAES(buffer, password, iv, mode, padding));
        }

        /// <summary>
        /// AES解密Base64字符串成string pw长度，支持16,24,32 分别对应128 192 256
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="password"></param>
        /// <param name="iv"></param>
        /// <param name="mode"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        public static string DecryptAESToString(string base64text, string password, byte[] iv = null, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        {
            return Encoding.UTF8.GetString(DecryptAES(Convert.FromBase64String(base64text), password, iv, mode, padding));
        }

        ///// <summary>
        ///// AES解密(.net core)
        ///// </summary>
        ///// <param name="buffer"></param>
        ///// <param name="password"></param>
        ///// <param name="iv"></param>
        ///// <param name="keySize"></param>
        ///// <param name="mode"></param>
        ///// <param name="padding"></param>
        ///// <param name="pwLength">pw长度，默认32位，不足补0，支持16,24,32</param>
        ///// <returns></returns>
        //public static byte[] DecryptAESCore(byte[] buffer, string password, string iv = "", int keySize = 128, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7, int pwLength = 32)
        //{
        //    try
        //    {
        //        using (var aes = Aes.Create())
        //        {
        //            aes.Mode = mode;
        //            aes.Padding = padding;
        //            aes.KeySize = keySize;
        //            var pwdBytes = Encoding.UTF8.GetBytes(password);
        //            var keyBytes = new byte[pwLength];
        //            Array.Copy(pwdBytes, keyBytes, pwdBytes.Length);
        //            aes.Key = keyBytes;
        //            aes.IV = Encoding.UTF8.GetBytes(iv);
        //            using (ICryptoTransform transform = aes.CreateDecryptor())
        //                return transform.TransformFinalBlock(buffer, 0, buffer.Length);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        ///// <summary>
        ///// AES解密Base64字符串(.net core)
        ///// </summary>
        ///// <param name="buffer"></param>
        ///// <param name="password"></param>
        ///// <param name="iv"></param>
        ///// <param name="keySize"></param>
        ///// <param name="mode"></param>
        ///// <param name="padding"></param>
        ///// <returns></returns>
        //public static byte[] DecryptAESCore(string base64text, string password, string iv = "", int keySize = 128, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        //{
        //    return DecryptAESCore(Convert.FromBase64String(base64text), password, iv, keySize, mode, padding);
        //}

        ///// <summary>
        ///// AES解密成string(.net core)
        ///// </summary>
        ///// <param name="buffer"></param>
        ///// <param name="password"></param>
        ///// <param name="iv"></param>
        ///// <param name="keySize"></param>
        ///// <param name="mode"></param>
        ///// <param name="padding"></param>
        ///// <param name="pwLength">pw长度，默认32位，不足补0，支持16,24,32</param>
        ///// <returns></returns>
        //public static string DecryptAESToStringCore(byte[] buffer, string password, string iv = "", int keySize = 128, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7, int pwLength = 32)
        //{
        //    return Encoding.UTF8.GetString(DecryptAESCore(buffer, password, iv, keySize, mode, padding, pwLength));
        //}

        ///// <summary>
        ///// AES解密Base64字符串成string(.net core)
        ///// </summary>
        ///// <param name="buffer"></param>
        ///// <param name="password"></param>
        ///// <param name="iv"></param>
        ///// <param name="keySize"></param>
        ///// <param name="mode"></param>
        ///// <param name="padding"></param>
        ///// <param name="pwLength">pw长度，默认32位，不足补0，支持16,24,32</param>
        ///// <returns></returns>
        //public static string DecryptAESToStringCore(string base64text, string password, string iv = "", int keySize = 128, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7, int pwLength = 32)
        //{
        //    return Encoding.UTF8.GetString(DecryptAESCore(Convert.FromBase64String(base64text), password, iv, keySize, mode, padding, pwLength));
        //}

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="buffer">加密数组</param>
        /// <param name="password">加密的密码</param>
        /// <param name="iv">密钥</param>
        /// <param name="mode">运算模式</param>
        /// <returns>Base64字符串</returns>
        public static byte[] EncryptDES(DESTYPE type, byte[] buffer, string password, CipherMode cMode = CipherMode.ECB, PaddingMode pMode = PaddingMode.Zeros, byte[] iv = null)
        {
            try
            {
                SymmetricAlgorithm des = null;
                if (type == DESTYPE.DES)
                    des = new DESCryptoServiceProvider();
                if (type == DESTYPE.TripleDES)
                    des = new TripleDESCryptoServiceProvider();
                des.Key = Encoding.UTF8.GetBytes(password);
                des.Mode = cMode;
                des.Padding = pMode;

                if (cMode == CipherMode.CBC)
                {
                    des.IV = iv;
                }
                var desEncrypt = des.CreateEncryptor();
                return desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// DES加密成Base64
        /// </summary>
        /// <param name="buffer">加密数组</param>
        /// <param name="password">加密的密码</param>
        /// <param name="iv">密钥</param>
        /// <param name="mode">运算模式</param>
        /// <returns>Base64字符串</returns>
        public static string EncryptDESToBase64(DESTYPE type, byte[] buffer, string password, CipherMode cMode = CipherMode.ECB, PaddingMode pMode = PaddingMode.Zeros, byte[] iv = null)
        {
            return Convert.ToBase64String(EncryptDES(type, buffer, password, cMode, pMode, iv));
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="buffer">加密的数组</param>
        /// <param name="password">密钥</param>
        /// <param name="iv">解密矢量：只有在CBC解密模式下才适用</param>
        /// <param name="cMode">运算模式</param>
        /// <returns></returns>
        public static byte[] DecryptDES(DESTYPE type, byte[] buffer, string password, CipherMode cMode = CipherMode.ECB, PaddingMode pMode = PaddingMode.Zeros, byte[] iv = null)
        {
            try
            {
                SymmetricAlgorithm des = null;
                if (type == DESTYPE.DES)
                    des = new DESCryptoServiceProvider();
                if (type == DESTYPE.TripleDES)
                    des = new TripleDESCryptoServiceProvider();
                des.Key = Encoding.UTF8.GetBytes(password);
                des.Mode = cMode;
                des.Padding = pMode;

                if (cMode == CipherMode.CBC)
                {
                    des.IV = iv;
                }
                return des.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// DES解密Base64字符串
        /// </summary>
        /// <param name="text">加密的Base64字符串</param>
        /// <param name="password">密钥</param>
        /// <param name="iv">解密矢量：只有在CBC解密模式下才适用</param>
        /// <param name="cMode">运算模式</param>
        /// <returns>解密的字符串</returns>
        public static byte[] DecryptDES(DESTYPE type, string base64text, string password, CipherMode cMode = CipherMode.ECB, PaddingMode pMode = PaddingMode.Zeros, byte[] iv = null)
        {
            return DecryptDES(type, Convert.FromBase64String(base64text), password, cMode, pMode, iv);
        }

        /// <summary>
        /// DES解密Base64字符串成string
        /// </summary>
        /// <param name="buffer">加密的数组</param>
        /// <param name="password">密钥</param>
        /// <param name="iv">解密矢量：只有在CBC解密模式下才适用</param>
        /// <param name="cMode">运算模式</param>
        /// <returns></returns>
        public static string DecryptDESToString(DESTYPE type, byte[] buffer, string password, CipherMode cMode = CipherMode.ECB, PaddingMode pMode = PaddingMode.Zeros, byte[] iv = null)
        {
            return Encoding.UTF8.GetString(DecryptDES(type, buffer, password, cMode, pMode, iv));
        }

        /// <summary>
        /// DES解密成string
        /// </summary>
        /// <param name="buffer">加密的数组</param>
        /// <param name="password">密钥</param>
        /// <param name="iv">解密矢量：只有在CBC解密模式下才适用</param>
        /// <param name="cMode">运算模式</param>
        /// <returns></returns>
        public static string DecryptDESToString(DESTYPE type, string base64text, string password, CipherMode cMode = CipherMode.ECB, PaddingMode pMode = PaddingMode.Zeros, byte[] iv = null)
        {
            return Encoding.UTF8.GetString(DecryptDES(type, Convert.FromBase64String(base64text), password, cMode, pMode, iv));
        }

        /// <summary>
        /// HMACSHA1加密(base64)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string HMACSHA1Base64(string text, string key)
        {
            using (var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(key)))
            {
                return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(text)));
            }
        }

        /// <summary>
        /// JWT By JWK(密钥可以从https://mkjwk.org/上获取)
        /// </summary>
        /// <param name="privateKey">JWK PrivateKey</param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static string JWKRS256ToJWT(string privateKey, string payload, string kid)
        {
            {
                var jwk = privateKey.ToJsonObj<IDictionary<string, string>>();

                var p = Base64Url.Decode(jwk["p"]);
                var q = Base64Url.Decode(jwk["q"]);
                var d = Base64Url.Decode(jwk["d"]);
                var e = Base64Url.Decode(jwk["e"]);
                var qi = Base64Url.Decode(jwk["qi"]);
                var dq = Base64Url.Decode(jwk["dq"]);
                var dp = Base64Url.Decode(jwk["dp"]);
                var n = Base64Url.Decode(jwk["n"]);

                using (var rsa = RSA.Create())
                {
                    var keyParams = new RSAParameters
                    {
                        P = p,
                        Q = q,
                        D = d,
                        Exponent = e,
                        InverseQ = qi,
                        DP = dp,
                        DQ = dq,
                        Modulus = n
                    };
                    rsa.KeySize = 2048;
                    rsa.ImportParameters(keyParams);
                    var headers = new Dictionary<string, object>()
                    {
                         { "alg", "RS256"},
                         { "typ", "JWT"},
                         { "kid", kid}
                    };
                    return JWT.Encode(payload, rsa, JwsAlgorithm.RS256, headers);
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

        public enum DESTYPE
        {
            DES,
            TripleDES
        }
    }
}