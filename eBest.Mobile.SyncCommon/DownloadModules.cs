using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

using eBest.Mobile.SyncEntities;
using Newtonsoft.Json;


namespace eBest.Mobile.SyncCommon
{
    public sealed class DownloadModules
    {
        public DownloadModules() { }

        /// <summary>
        /// 生成表头
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="TimeStamp"></param>
        /// <returns></returns>
        public static SyncTable GeneractorHeader(String TableName, String TimeStamp, List<Column> FiledList)
        {
            SyncTable syncTable = new SyncTable();

            string str = string.Empty;
            foreach (Column column in FiledList)
                str += column.Name + ",";

            syncTable.Name = TableName;
            syncTable.ParamValues = new string[] { TimeStamp };
            syncTable.Fields = str.TrimEnd(',');

            return syncTable;
        }

        /// <summary>
        /// 生成下载内容
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="FiledList"></param>
        /// <returns></returns>
        public static SyncTable GeneractorContent(String TableName, IDataReader reader, List<Column> FiledList, String LastTimeStamp)
        {
            SyncTable syncTable = new SyncTable();
            StringBuilder sb = new StringBuilder();
            string NewStamp = LastTimeStamp;

            if (reader != null)
            {
                try
                {
                    while (reader.Read())
                    {
                        sb.Clear();
                        string KeyValues = String.Empty;
                        //循环每个字段
                        foreach (Column col in FiledList)
                        {
                            Object value = reader[col.Name];

                            if (value != DBNull.Value)
                            {
                                //传输二进制数据
                                if (col.Type.Equals("System.Byte"))
                                {
                                    byte[] bytes = (byte[])value;
                                    foreach (byte b in bytes)
                                    {
                                        sb.Append(b.ToString("x2"));
                                    }
                                }
                                else if (col.Type.Equals("System.String"))
                                {
                                    sb.Append(value.ToString().Replace(">", "&gt;").Replace("<", "&lt;"));
                                }
                                else if (col.Type.Equals("System.DateTime"))
                                {
                                    value = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
                                    sb.Append(value.ToString());
                                }
                                else if(col.Type.Equals("System.Date"))
                                {
                                    value = ((DateTime)value).ToString("yyyy-MM-dd");
                                    sb.Append(value.ToString());
                                }
                                else
                                {
                                    sb.Append(value.ToString());
                                }
                            }//if (value != DBNull.Value)
                            if (FiledList[FiledList.Count - 1] != col) sb.Append("▏");

                        }//foreach

                        syncTable.Rows.Add(sb.ToString());

                    }//while                  
                }
                catch
                {
                    throw;
                    GC.Collect();
                }
                finally
                {
                    reader.Close();
                  
                }
            }
            //如果在不存在下载的数据，则不提交给客户端进行解析
            //if (syncTable.Rows.Count > 0)
            //{
                SyncTable header = GeneractorHeader(TableName, NewStamp.ToString(), FiledList);

                syncTable.Name = header.Name;
                syncTable.Fields = header.Fields;
                syncTable.ParamValues = header.ParamValues;
            //}
            return syncTable;
        }
    }
}
