using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace eBest.Mobile.SyncHelper
{
    public class MyWriter
    {
        public MemoryStream answer = null;
       
        private Stream stream = null;
        public MyWriter(System.IO.MemoryStream answer)
        {
            try
            {
                stream = new ICSharpCode.SharpZipLib.GZip.GZipOutputStream(answer);
                this.answer = answer;
            }
            catch { }


        }

        public MyWriter(MemoryStream answer, bool EnabledGzip)
        {
            if (EnabledGzip)
                stream = new ICSharpCode.SharpZipLib.GZip.GZipOutputStream(answer);
            else
                stream = answer;
            this.answer = answer;
        }


        public void Write(String towrite)
        {
            try
            {
                byte[] data =Encoding.UTF8.GetBytes(towrite);
                stream.Write(data, 0, data.Length);

            }
            catch { }
        }



        public void Println(String toWrite)
        {
            byte[] data =Encoding.UTF8.GetBytes(toWrite + "\n");
            stream.Write(data, 0, data.Length);
        }

        public void Finish()
        {
            try
            {
                if (stream is ICSharpCode.SharpZipLib.GZip.GZipOutputStream)
                    ((ICSharpCode.SharpZipLib.GZip.GZipOutputStream)stream).Finish();
                else
                    stream.Flush();
            }
            catch
            {

            }
        }
        public void Close()
        {
            try
            {
                stream.Close();

            }
            catch { }
        }

        public void Flush()
        {
            try
            {
                stream.Flush();

            }
            catch { }
        }
    }
}
