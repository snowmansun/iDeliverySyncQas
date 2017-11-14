using System;
using System.IO;
using System.Web;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using System.Collections.Generic;

using Newtonsoft.Json;
using eBest.Mobile.SyncConfig;
using eBest.Mobile.SyncEntities;
using eBest.Mobile.SyncHelper;
using eBest.SyncConfiguration;

namespace eBest.SyncServer
{
    public class UpSync : IHttpHandler
    {
        #region Parameters

        private static readonly log4net.ILog mylog = log4net.LogManager.GetLogger("Upload");
        private SyncConfigurationSection config = null;

        private MyWriter writer;
        private bool enableZip;
        private object instance;

        private IDbTransaction tran;
        private IDbCommand insertCom;
        private IDbCommand updateCom;

        private SyncRequest sync;
        private SyncResult syncResult;

        #endregion

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpResponse response = context.Response;
            response.Clear();
            response.ContentType = ("text/plain;text/json;charset=UTF-8");

            syncResult = new SyncResult();
            syncResult.SyncType = SyncType.Upload;

            MemoryStream stream = new MemoryStream();
            SyncMasterServer server = new SyncMasterServer();

            //Start decode the sync data
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
                log4net.ThreadContext.Properties["ActionType"] = 2;

                #region Validate config version

                Dictionary<string, SyncConfigurationSection> syncConfig = context.Cache["SyncConfiguration"] as Dictionary<string, SyncConfigurationSection>;
                config = syncConfig["V2"]; // 这里写死默认的版本号
                //config = syncConfig[sync.Version];
                if (config == null)
                    throw new Exception("the version does not exist;");

                #endregion

                #region Validate user authorization

                bool loginAuthFlag = false;
                Assembly assembly = Assembly.LoadFile(config.FullPath);
                instance = Activator.CreateInstance(assembly.GetType(config.Type.FullName));
                loginAuthFlag = (bool)instance.GetType().InvokeMember("Authorization",
                                                                      BindingFlags.InvokeMethod,
                                                                      Type.DefaultBinder,
                                                                      instance, new object[] { sync.LoginName, sync.PassWord });

                if (!loginAuthFlag)
                    throw new Exception("the user authentication failed");

                #endregion

                #region Main process for Upload data

                if (server.Connection != null && server.Connection.State != ConnectionState.Open)
                    server.Connection.Open();

                tran = server.Connection.BeginTransaction(); //Begin transaction 

                insertCom = server.Connection.CreateCommand();
                updateCom = server.Connection.CreateCommand();

                insertCom.Transaction = tran;
                updateCom.Transaction = tran;

                mylog.Info(JsonConvert.SerializeObject(sync));

                UpStartAsyncTask(sync.ReqContent.Tables);

                tran.Commit();

                syncResult.Exception = "the user synchronous upload successfully";
                syncResult.Status = SyncStatus.Success;

                #endregion
            }

            #region Exception

            catch (JsonReaderException ex)
            {
                syncResult.Exception = ex.Message;
                syncResult.Status = SyncStatus.Failed;
            }
            catch (SqlException ex)
            {
                if (tran != null)
                    tran.Rollback();

                syncResult.Exception = ex.Message;
                syncResult.Status = SyncStatus.Failed;
            }
            catch (Exception ex)
            {
                if (tran != null)
                    tran.Rollback();

                syncResult.Exception = ex.Message;
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

            }
            #endregion
        }

        private void UpStartAsyncTask(List<SyncTable> tables)
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
                    throw new Exception(string.Format("Table {0} does not exist Upload logic in Version {1};", tableName, sync.Version));

                if (script.Up.Type == ScriptType.Unknown)
                    throw new Exception(string.Format("Table {0} does not exist Upload logic in Version {1};", tableName, sync.Version));

                if (script.UploadColumns.Count == 0) continue;

                #endregion

                #region Process upload script

                updateCom.CommandText = string.Empty;
                insertCom.CommandText = string.Empty;

                string updateMethod = script.Up.UpdateCode;
                string insertMethod = script.Up.InsertCode;
                //Get column list
                string columnList = script.GetUploadColumnList();

                if (script.Up.Type == ScriptType.SQL)  //script type is sql
                {
                    updateCom.CommandText = script.Up.UpdateCode;
                    insertCom.CommandText = script.Up.InsertCode;
                }
                else  //script type is assembly
                {
                    if (!string.IsNullOrEmpty(updateMethod))
                    {

                        if (table.ParamValues != null)
                            updateCom.CommandText = config.Type.InvokeMember(updateMethod, BindingFlags.InvokeMethod, Type.DefaultBinder,
                                                           instance, table.ParamValues).ToString();
                        else
                            updateCom.CommandText = config.Type.InvokeMember(updateMethod, BindingFlags.InvokeMethod, Type.DefaultBinder,
                                                                                instance, null).ToString();
                    }

                    if (!string.IsNullOrEmpty(insertMethod))
                    {
                        if (table.ParamValues != null)
                            insertCom.CommandText = config.Type.InvokeMember(insertMethod, BindingFlags.InvokeMethod, Type.DefaultBinder,
                                                           instance, table.ParamValues).ToString();
                        else
                            insertCom.CommandText = config.Type.InvokeMember(insertMethod, BindingFlags.InvokeMethod, Type.DefaultBinder,
                                                                                instance, null).ToString();
                    }
                }//if

                int paramCount = 0;
                insertCom.Parameters.Clear();
                updateCom.Parameters.Clear();
                if (insertCom.CommandText != "")
                {
                    paramCount = insertCom.CommandText.Split('@').Length - 1;
                    for (int j = 1; j <= paramCount; j++)
                    {
                        System.Data.IDbDataParameter param = insertCom.CreateParameter();
                        param.Value = DBNull.Value;
                        param.ParameterName = "@" + j.ToString();
                        insertCom.Parameters.Add(param);
                    }
                }//if
                if (updateCom.CommandText != "")
                {
                    paramCount = updateCom.CommandText.Split('@').Length - 1; //The command parameters will be defined with prefix '@'
                    for (int j = 1; j <= paramCount; j++)
                    {
                        System.Data.IDbDataParameter param = updateCom.CreateParameter();
                        param.Value = DBNull.Value;
                        param.ParameterName = "@" + j.ToString();
                        updateCom.Parameters.Add(param);
                    }
                }//if

                #endregion

                #region upload data

                int count = 0; //affected record count of insert operation
                int count2 = 0; //affected record count of update operation
                List<string> rows = table.Rows;

                UploadTableData(tableName, rows, insertCom, updateCom, script.UploadColumns, ref count, ref count2); //Import data to database server
                mylog.InfoFormat("User：{0} In 《{1}》 Tables Uploading 《{2}》 rows data,Update 《{3}》 rows data;",
                                  sync.LoginName, tableName, count, count2);

                #endregion

            }//foreach
        }

        private void UploadTableData(string tableName, List<string> rows, IDbCommand insertCom, IDbCommand updateCom, ColumnCollection updateColumns, ref int count, ref int count2)
        {
            if (rows.Count == 0) return;

            insertCom.CommandTimeout = 180;
            updateCom.CommandTimeout = 180;

            foreach (string row in rows)
            {
                int result = UploadRowData(tableName, row, updateCom, updateColumns);//Update operation  
                if (result == 0)
                {
                    result = UploadRowData(tableName, row, insertCom, updateColumns); //Insert operation
                    count += result;
                }
                else
                    count2++;
            }//foreach

        }

        private int UploadRowData(string tableName, string row, IDbCommand cmd, ColumnCollection columns)
        {
            int result = 0; //affected row count
            string[] values = row.Split('▏');
            if (columns.Count != values.Length)
                throw new Exception(string.Format("Table {0} contains {1} fields, but uploaded data has {2} fields", tableName, columns.Count, values.Length));

            //Execute Update
            if (!string.IsNullOrEmpty(cmd.CommandText))
            {
                //The upload Visit_Photo Save Image Operation
                if (tableName.Equals("Visit_Photo", StringComparison.OrdinalIgnoreCase))
                    if (!string.IsNullOrEmpty(values[2]))
                        values[2] = SyncConfig.GetPhotoPath(values[2]);

                //The field values in the data package should be corresponding to the command parameters(both field order and value)
                //The columns defination in the xml schema file can be ignored 
                for (int k = 0; k < cmd.Parameters.Count; k++)
                {
                    IDbDataParameter paramU = (IDbDataParameter)cmd.Parameters[k];
                    paramU.DbType = DbType.String;
                    object paramValue = System.DBNull.Value;
                    if (values[k].Length == 0)
                    {
                        paramU.Value = paramValue;
                        if (columns[k].Type.Equals("System.Byte")) paramU.DbType = DbType.Binary;
                    }
                    else
                    {
                        paramU.Value = values[k];
                        if (columns[k].Type.Equals("System.DateTime"))
                        {
                            paramValue = Convert.ToDateTime(values[k]);
                            paramU.Value = paramValue;
                        }
                        else if (columns[k].Type.Equals("System.Byte"))
                        {
                            if (values[k].Length % 2 != 0) values[k] += "20";//空格
                            // 需要将 hex 转换成 byte 数组。 
                            byte[] bb = new byte[values[k].Length / 2];
                            for (int index = 0; index < bb.Length; index++)
                            {
                                // 每两个字符是一个 byte。 
                                bb[index] = byte.Parse(values[k].Substring(index * 2, 2), System.Globalization.NumberStyles.HexNumber);
                            }
                            paramU.Value = bb;
                            paramU.DbType = DbType.Binary;
                        }//if
                    }//if (values[k].Length == 0)

                }//for
                //result = cmd.ExecuteNonQuery();

                cmd.ExecuteNonQuery();

                result++;
            }//if (!string.IsNullOrEmpty(updateCom.CommandText))

            return result;
        }

    }
}
