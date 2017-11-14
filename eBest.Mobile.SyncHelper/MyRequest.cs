using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;


using eBest.Mobile.SyncCommon;

namespace eBest.Mobile.SyncHelper
{
    /// <summary>
    /// Summary description for HttpRequestHelp
    /// </summary>
    public class MyRequest
    {
        public static string LoginHttpResult(string url, string parameters)
        {
            string result = string.Empty;

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                byte[] data = Encoding.UTF8.GetBytes(parameters);
                request.ContentType = "application/json";
                request.Method = WebRequestMethods.Http.Post;
                request.ContentLength = data.Length;

                ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return result;
        }

        public static string SyncHttpResult(string url, string parameters, bool isEnableZip)
        {
            string result = string.Empty;

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Timeout = 1000000000;
                byte[] data = Encoding.UTF8.GetBytes(parameters);
                request.ContentType = "application/json";
                request.Method = WebRequestMethods.Http.Post;
                request.ContentLength = data.Length;

                ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;

                using (Stream reqStrm = request.GetRequestStream())
                {
                    reqStrm.Write(data, 0, data.Length);
                    using (WebResponse response = request.GetResponse())
                    {
                        using (Stream resStrm = response.GetResponseStream())
                        {
                            if (isEnableZip)
                                result = Decode.Decompress(resStrm);
                            else
                                result = Decode.ToString(resStrm);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

    }
}
