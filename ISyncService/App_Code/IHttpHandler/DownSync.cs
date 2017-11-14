using System;
using System.IO;
using System.Web;
using System.Data;
using System.Reflection;
using System.Collections.Generic;

using Newtonsoft.Json;
using eBest.Mobile.SyncCommon;
using eBest.Mobile.SyncConfig;
using eBest.Mobile.SyncEntities;
using eBest.Mobile.SyncHelper;
using eBest.SyncConfiguration;

namespace eBest.SyncServer
{
    public class DownSync : IHttpHandler
    {
        #region Parameters

        private static readonly log4net.ILog mylog = log4net.LogManager.GetLogger("Download");
        private SyncConfigurationSection config = null;

        private IDbCommand command;
        private MyWriter writer;
        private object instance;
        private bool enableZip;

        private SyncRequest sync;
        private SyncResult syncResult;
        private SyncTables tab;

        #endregion

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpResponse response = context.Response;
            response.Clear();

            response.ContentType = "text/plain;text/json;charset=UTF-8";

            syncResult = new SyncResult();
            syncResult.SyncType = SyncType.Download;

            MemoryStream stream = new MemoryStream();
            SyncMasterServer server = new SyncMasterServer();

            tab = new SyncTables();
            string readerContent = new StreamReader(context.Request.InputStream).ReadToEnd();

            try
            {
                sync = JsonConvert.DeserializeObject<SyncRequest>(readerContent);

                enableZip = sync.IsGzip == null ? true : sync.IsGzip == "1";
                if (enableZip)
                    response.AddHeader("Content-Encoding", "gzip");

                writer = new MyWriter(stream, enableZip);

                syncResult.LoginName = sync.LoginName;

                log4net.ThreadContext.Properties["UserName"] = sync.LoginName;
                log4net.ThreadContext.Properties["ActionType"] = 1;

                #region Validate config version

                Dictionary<string, SyncConfigurationSection> syncConfig = context.Cache["SyncConfiguration"] as Dictionary<string, SyncConfigurationSection>;
                
                config = syncConfig["V2"]; // 这里写死默认的版本号
                //config = syncConfig[sync.Version];
                if (config == null)
                    throw new Exception("the version {0} does not exist;");

                #endregion

                #region Validate user authorization

                bool loginAuthFlag = false;
                Assembly assembly = Assembly.LoadFile(config.FullPath);
                instance = Activator.CreateInstance(assembly.GetType(config.Type.FullName));
                loginAuthFlag = (bool)instance.GetType().InvokeMember("Authorization",
                                                                      BindingFlags.InvokeMethod,
                                                                      Type.DefaultBinder,
                                                                      instance,
                                                                      new object[] { sync.LoginName, sync.PassWord });
                if (!loginAuthFlag)
                    throw new Exception("the user authentication failed");

                #endregion

                #region Main process for Download data

                if (server.Connection != null && server.Connection.State != ConnectionState.Open)
                    server.Connection.Open();
                command = server.Connection.CreateCommand();

                mylog.Info(JsonConvert.SerializeObject(sync));

                DownStartAsyncTask(sync.ReqContent.Tables);

                syncResult.Result = tab;
                syncResult.Status = SyncStatus.Success;

                #endregion
            }

            #region Exception

            catch (JsonReaderException err)
            {
                syncResult.Exception = err.Message;
                syncResult.Status = SyncStatus.Failed;
            }
            catch (OutOfMemoryException err)
            {
                GC.Collect();

                syncResult.Exception = err.Message;
                syncResult.Status = SyncStatus.Failed;
            }
            catch (Exception err)
            {
                syncResult.Exception = err.Message;
                syncResult.Status = SyncStatus.Failed;
            }
            finally
            {
                response.AppendHeader("SERVER_TIME", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                writer.Write(JsonConvert.SerializeObject(syncResult));
                writer.Flush();
                writer.Finish();

                if (syncResult.Status == SyncStatus.Success)
                {
                    mylog.Info(JsonConvert.SerializeObject(syncResult));
                }
                else
                {
                    mylog.Error(JsonConvert.SerializeObject(syncResult));
                }

                //Clear Tables Data;
                if (tab.Tables.Count > 0)
                    tab.Tables.Clear();

                if (stream.Length > 0)
                {
                    stream.Position = 0;
                    stream.WriteTo(response.OutputStream);
                }

                if (server.Connection != null && server.Connection.State == ConnectionState.Open)
                {
                    server.Connection.Close();
                    server.Connection.Dispose();
                    server = null;
                }
                writer.Close();
                writer = null;

            }//try
            #endregion
        }

        public void DownStartAsyncTask(List<SyncTable> tables)
        {
            foreach (SyncTable table in tables)
            {
                #region XmlDeserializeObject

                string tableName = table.Name;

                if (!SyncConfig.IsTableSchemaExists(tableName))
                    throw new Exception(string.Format("Can't find the server table {0} structure information", tableName));

                ScriptEntity scriptEntity = SyncConfig.GetEntity(tableName);
                if (scriptEntity == null)
                    throw new Exception(string.Format("Reading table {0} structure information on the server failed", tableName));

                Script script = scriptEntity.FindScript(sync.Version);
                if (script == null)
                    throw new Exception(string.Format("Table {0} does not exist Download logic in Version {1};", tableName, sync.Version));

                if (script.DownloadColumns.Count == 0) continue;

                #endregion

                #region Process download script

                command.CommandText = string.Empty;
                command.CommandTimeout = 180;

                if (script.Down.Type == ScriptType.SQL) //Script type is sql
                {
                    command.CommandText = script.Down.Code;
                }
                else //Script type is Assembly
                {
                    string methodName = script.Down.Code;

                    if (table.ParamValues != null)
                        command.CommandText = config.Type.InvokeMember(methodName, BindingFlags.InvokeMethod, Type.DefaultBinder, instance, table.ParamValues).ToString();
                    else
                        command.CommandText = config.Type.InvokeMember(methodName, BindingFlags.InvokeMethod, Type.DefaultBinder, instance, null).ToString();
                }

                if (string.IsNullOrEmpty(command.CommandText))
                    continue;

                #endregion

                IDataReader reader = command.ExecuteReader();
                SyncTable data = DownloadModules.GeneractorContent(tableName, reader, script.DownloadColumns, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));

                tab.Tables.Add(data);

                reader.Close();
            }
        }
    }
}