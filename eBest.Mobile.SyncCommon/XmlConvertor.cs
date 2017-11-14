using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace eBest.Mobile.SyncCommon
{
    public class XmlConvertor
    {
        private const string xmlLnsString = " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"";
        /// <summary>
        /// serialize an object to string.
        /// </summary>
        /// <param name="obj">
        /// the object.
        /// </param>
        /// <returns>
        /// the serialized string.
        /// </returns>
        public static string ObjectToXml(object obj)
        {
            return ObjectToXml(obj, false, false);
        }

        public static string ObjectToXml(object obj, bool removeHead)
        {
            return ObjectToXml(obj, false, removeHead);
        }

        /// <summary>
        /// serialize an object to string.
        /// </summary>
        /// <param name="obj">
        /// the object.
        /// </param>
        /// <param name="toBeIndented">
        /// whether to be indented.
        /// </param>
        public static string ObjectToXml(object obj, bool toBeIndented, bool removeHead)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            UTF8Encoding encoding = new UTF8Encoding(false);
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, encoding);
            xmlTextWriter.Formatting = toBeIndented ? Formatting.Indented : Formatting.None;
            xmlSerializer.Serialize(xmlTextWriter, obj);

            string content = encoding.GetString(memoryStream.ToArray());
            if (removeHead) content = content.Substring(content.IndexOf("?>") + 2);
            return content.Replace(xmlLnsString, "");
        }

        /// <summary>
        /// deserialize string.to an object.
        /// </summary>
        /// <param name="type">
        /// the type of the object.
        /// </param>
        /// <param name="xml">
        /// the string need to be deserialized.
        /// </param>
        /// <returns>
        /// the deserialized object.
        /// </returns>
        public static object XmlToObject(Type type, string xml)
        {
            if (xml == null)
            {
                //throw new ArgumentNullException("xml");
                return null;
            }
            if (type == null)
            {
                //throw new ArgumentNullException("type");
                return null;
            }

            object obj = null;
            XmlSerializer xmlSerializer = new XmlSerializer(type);
            StringReader stringReader = new StringReader(xml);
            XmlReader xmlReader = new XmlTextReader(stringReader);
            try
            {
                obj = xmlSerializer.Deserialize(xmlReader);
            }
            catch
            {
                //throw new Exception(exception.Message);
                return null;
            }
            finally
            {
                xmlReader.Close();
            }
            return obj;
        }

        public static byte[] BinarySerialize(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                ms.Close();
                return ms.ToArray();
            }

        }

        public static object BinaryDeserialize(byte[] bytes)
        {
            BinaryFormatter bf = new BinaryFormatter();
            object obj;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                obj = bf.Deserialize(ms);
                ms.Close();
            }
            return obj;
        }


    }
}
