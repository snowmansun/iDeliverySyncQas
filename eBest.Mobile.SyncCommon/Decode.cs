using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Drawing.Imaging;
using ICSharpCode.SharpZipLib.GZip;
using System.Drawing;




namespace eBest.Mobile.SyncCommon
{
    public class Decode
    {
        /// <summary>
        /// 压缩数据
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static byte[] Compress(String str)
        {
            try
            {
                byte[] sourse = System.Text.Encoding.UTF8.GetBytes(str);
                MemoryStream ms = new MemoryStream();
                ICSharpCode.SharpZipLib.GZip.GZipOutputStream stream = new GZipOutputStream(ms);
                stream.Write(sourse, 0, sourse.Length);
                stream.Close();
                byte[] data = ms.ToArray();

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static String Decompress(Stream inputStream)
        {
            string str = string.Empty;
            GZipInputStream stream = new GZipInputStream(inputStream);
            MemoryStream output = new MemoryStream();
            try
            {

                byte[] buffer = new byte[4096];
                int read = -1;
                read = stream.Read(buffer, 0, buffer.Length);
                while (read > 0)
                {
                    output.Write(buffer, 0, read);
                    read = stream.Read(buffer, 0, buffer.Length);
                }
                str = System.Text.Encoding.UTF8.GetString(output.ToArray(), 0, (int)output.Length);

            }
            catch
            {
                throw;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                    //stream = null;
                }
                if (output != null)
                {
                    output.Close();
                    output.Dispose();
                    //output = null;
                }
            }
            return str;
        }

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static String ToString(Stream inputStream)
        {
            string str = string.Empty;
            MemoryStream output = new MemoryStream();
            try
            {

                byte[] buffer = new byte[4096];
                int read = -1;
                read = inputStream.Read(buffer, 0, buffer.Length);
                while (read > 0)
                {
                    output.Write(buffer, 0, read);
                    read = inputStream.Read(buffer, 0, buffer.Length);
                }
                str = Encoding.UTF8.GetString(output.ToArray(), 0, (int)output.Length);

            }
            catch
            {
                throw;
            }
            finally
            {
                if (output != null)
                {
                    output.Close();
                    output.Dispose();
                    output = null;
                }
                if (inputStream != null)
                {
                    inputStream.Close();
                    inputStream.Dispose();
                    inputStream = null;
                }
            }
            return str;
        }

        /// <summary>
        /// 密码与MD5之间转换
        /// </summary>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static string MD5Convert(string Password)
        {
            MD5 md5 = MD5.Create();
            byte[] dataToHash = (new UTF8Encoding()).GetBytes(Password);
            byte[] hashvalue = (new MD5CryptoServiceProvider()).ComputeHash(dataToHash);

            string pwd = string.Empty;
            foreach (byte b in hashvalue)
            {
                pwd += b.ToString("X2");
            }
            return pwd;
        }
    }
}
