using System;
using System.Xml;
using System.IO;
using System.Configuration;
using System.Collections;


using eBest.Mobile.SyncCommon;
using eBest.Mobile.SyncConfig;
using eBest.Mobile.SyncEntities;

using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using System.Drawing;
using System.Drawing.Imaging;



namespace eBest.SyncServer
{
    /// <summary>
    /// Read sync configuration xml file
    /// MDF by Younest 2011-09-28
    /// </summary>
    public class SyncConfig
    {

        private static ICacheManager xmlCache = CacheFactory.GetCacheManager("XML Cache Manager");
        private static SyncConfigManager syncConfigManager = (SyncConfigManager)ConfigurationManager.GetSection("sync");

        /// <summary>
        /// 同步配置表文件夹路径
        /// </summary>
        public static readonly string ConfigTablesFolder = AppDomain.CurrentDomain.BaseDirectory + syncConfigManager.Common["ConfigDirectory"].Value + "\\";

        /// <summary>
        /// 同步上传照片文件夹路径
        /// </summary>
        public static readonly string UploadPhotoPath =syncConfigManager.Common["UploadPhotoPath"].Value + "\\";


        /// <summary>
        /// 签名照保存为本地图片文件并返回保存路径信息存入DB
        /// </summary>
        /// <param name="photoContent">上传照片内容</param>
        /// <returns></returns>
        public static string GetPhotoPath(string photoContent)
        {
            string photoPath = string.Empty;
            MemoryStream ms = null;
            Image image = null;
            try
            {
                byte[] imageBytes = Convert.FromBase64String(photoContent);
                ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);
                image = Image.FromStream(ms, true);

                if (!Directory.Exists(UploadPhotoPath))
                    Directory.CreateDirectory(UploadPhotoPath);

                string fileName = UploadPhotoPath + DateTime.Now.Ticks+".jpg";
                image.Save(fileName);

                photoPath =fileName;
            }
            catch (Exception)
            {
                throw new Exception("Upload Photo Save Fail");
            }
            finally
            {
                if (ms != null) ms.Close();
                image.Dispose();
            }

            return photoPath;
        }

        /// <summary>
        /// Get Server Config FilesName
        /// </summary>
        /// <returns></returns>
        public static ArrayList GetXmlConfigName()
        {
            ArrayList list = new ArrayList();
            DirectoryInfo infor = new DirectoryInfo(ConfigTablesFolder);
            foreach (FileInfo item in infor.GetFiles("*.xml"))
            {
                list.Add(item.Name);
            }

            return list;
        }

        /// <summary>
        /// Get script entity from server ram
        /// </summary>
        /// <param name="tableName">table name of the entity</param>
        /// <returns>script entity object</returns>
        public static ScriptEntity GetEntity(string tableName)
        {
            return GetXmlContent(tableName);
        }

        /// <summary>
        /// Table schema exists or not
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool IsTableSchemaExists(string tableName)
        {
            //return xmlCache.Contains(tableName);
            return File.Exists(ConfigTablesFolder + tableName + ".xml");
        }

        private static ScriptEntity GetXmlContent(string tableName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            string filePath = ConfigTablesFolder + tableName + ".xml";
            ScriptEntity entity = (ScriptEntity)xmlCache[tableName];
            if (entity == null)
            {
                lock (xmlCache)
                {
                    if (!File.Exists(filePath)) return null;
                    xmlDoc.Load(filePath);
                    entity = XmlConvertor.XmlToObject(typeof(ScriptEntity), xmlDoc.OuterXml) as ScriptEntity;
                    if (entity != null)
                    {
                        AddToCache(tableName, entity, xmlCache);
                        FileSystemWatcher xmlWatcher = new FileSystemWatcher(ConfigTablesFolder);
                        xmlWatcher.Changed += new FileSystemEventHandler(xmlWatcher_Changed);
                    }
                }
            }
            return entity;
        }

        private static void AddToCache(string tableName, object obj, ICacheManager cacheManager)
        {
            string filePath = ConfigTablesFolder + tableName + ".xml";
            FileDependency expireNotice = new FileDependency(filePath);
            cacheManager.Add(tableName, obj, CacheItemPriority.Normal, null, expireNotice);
        }

        private static void xmlWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            string tableName = e.FullPath.Substring(e.FullPath.LastIndexOf("\\") + 1);
            xmlCache.Remove(tableName);

            lock (xmlCache)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(e.FullPath);
                ScriptEntity entity = XmlConvertor.XmlToObject(typeof(ScriptEntity), xmlDoc.OuterXml) as ScriptEntity;
                AddToCache(tableName, entity, xmlCache);
            }
        }
    }
}
