using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;

namespace pde.pub
{
    public static class HttpUtil
    {
        private static string ContentType_JSON = "application/json;charset=UTF-8";
        private static string ContentType_FORM = "application/x-www-form-urlencoded";
        private static string DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";

        public static string httpPost(string contentType, string url, string postStr, CookieContainer cc)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.CookieContainer = cc;
            request.Method = "POST";
            request.ContentType = contentType;
            request.Timeout = 20000;
            byte[] postData = Encoding.UTF8.GetBytes(postStr);
            request.ContentLength = postData.Length;
            request.ServicePoint.Expect100Continue = false;
            request.GetRequestStream().Write(postData, 0, postData.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader stream = new StreamReader(response.GetResponseStream());
            string responseContent = stream.ReadToEnd();
            response.Close();
            stream.Close();
            request.Abort();
            return responseContent;
        }

        //通过HTTP下载文件
        public static bool DownloadFile(string url, string localFileName)
        {
            bool flag = false;
            //打开上次下载的文件
            long SPosition = 0;
            //实例化流对象
            FileStream FStream;
            //判断要下载的文件夹是否存在
            if (File.Exists(localFileName))
            {
                //打开要下载的文件
                FStream = File.OpenWrite(localFileName);
                //获取已经下载的长度
                SPosition = FStream.Length;
                FStream.Seek(SPosition, SeekOrigin.Current);
            }
            else
            {
                //文件不保存创建一个文件
                FStream = new FileStream(localFileName, FileMode.Create);
                SPosition = 0;
            }
            try
            { 
                //打开网络连接
                HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(url);

                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    myRequest.ProtocolVersion = HttpVersion.Version11;
                    myRequest.UserAgent = DefaultUserAgent;
                }

                if (SPosition > 0)
                    myRequest.AddRange((int)SPosition);             //设置Range值
                //向服务器请求,获得服务器的回应数据流
                WebResponse response = (HttpWebResponse)myRequest.GetResponse();
                Stream myStream = response.GetResponseStream(); 
                //定义一个字节数据
                byte[] btContent = new byte[512];
                int intSize = 0;
                intSize = myStream.Read(btContent, 0, 512);
                while (intSize > 0)
                {
                    FStream.Write(btContent, 0, intSize);
                    intSize = myStream.Read(btContent, 0, 512);
                }
                //关闭流
                FStream.Close();
                myStream.Close();
                flag = true;        //返回true下载成功
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                FStream.Close();
                flag = false;       //返回false下载失败
            }
            return flag;
        }


        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受
        }
    }
}
