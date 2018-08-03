using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace H.Framework.Core.Utilities
{
    public class FileHelper
    {
        public Action<double> DownLoadStatusCallBackHandle;

        public bool HttpDownload(string url, string path, bool isRemoveTemp = false)
        {
            string tempPath = Path.GetDirectoryName(path) + @"\temp";
            Directory.CreateDirectory(tempPath);  //创建临时文件目录
            string tempFile = tempPath + @"\" + Path.GetFileName(path) + ".temp"; //临时文件
            if (File.Exists(tempFile))
                File.Delete(tempFile);
            try
            {
                var fs = new FileStream(tempFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                var request = WebRequest.Create(url) as HttpWebRequest;
                var response = request.GetResponse() as HttpWebResponse;
                var responseStream = response.GetResponseStream();
                long totalSize = response.ContentLength;
                long hasDownSize = 0;
                var bArr = new byte[1024];
                var size = responseStream.Read(bArr, 0, bArr.Length);
                while (size > 0)
                {
                    fs.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, bArr.Length);
                    hasDownSize += size;
                    DownLoadStatusCallBackHandle?.Invoke((double)hasDownSize / totalSize * 100);
                }
                fs.Close();
                responseStream.Close();
                fs.Dispose();
                responseStream.Dispose();
                if (File.Exists(path))
                    File.Delete(path);
                File.Move(tempFile, path);
                if (isRemoveTemp)
                {
                    File.Delete(tempFile);
                    Directory.Delete(tempPath);
                }
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return false;
            }
        }

        public bool HttpDownloadWithResume(string url, string path, bool isRemoveTemp = false)
        {
            string tempPath = Path.GetDirectoryName(path) + @"\temp";
            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);  //创建临时文件目录
            string tempFile = tempPath + @"\" + Path.GetFileName(path) + ".temp"; //临时文件
            long lStartPos = 0;
            FileStream fs = null;
            if (File.Exists(tempFile))
            {
                fs = File.OpenWrite(tempFile);
                lStartPos = fs.Length;
                fs.Seek(lStartPos, SeekOrigin.Current); //移动文件流中的当前指针
            }
            else
            {
                fs = new FileStream(tempFile, FileMode.Create);
                lStartPos = 0;
            }
            try
            {
                var request = WebRequest.Create(url) as HttpWebRequest;
                if (lStartPos > 0)
                    request.AddRange((int)lStartPos); //设置Range值
                var response = request.GetResponse() as HttpWebResponse;
                var responseStream = response.GetResponseStream();
                long totalSize = response.ContentLength;
                long hasDownSize = 0;
                var bArr = new byte[1024];
                var size = responseStream.Read(bArr, 0, bArr.Length);
                while (size > 0)
                {
                    fs.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, bArr.Length);
                    hasDownSize += size;
                    DownLoadStatusCallBackHandle?.Invoke((double)hasDownSize / totalSize * 100);
                }
                fs.Close();
                responseStream.Close();
                fs.Dispose();
                responseStream.Dispose();
                if (File.Exists(path))
                    File.Delete(path);
                File.Move(tempFile, path);
                if (isRemoveTemp)
                {
                    File.Delete(tempFile);
                    Directory.Delete(tempPath);
                }
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return false;
            }
        }
    }
}