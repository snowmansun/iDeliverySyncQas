using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;


namespace eBest.SyncServer
{
    /// <summary>
    /// Summary description for SyncInfor
    /// </summary>
    public static class SyncInfor
    {
        private static readonly string ConnecString = null;
        private static SqlConnection connec;
        private static SqlCommand cmdText;
        private static SqlDataAdapter da;
        private static DataSet ds;
        private static DataTable dt;

        static SyncInfor()
        {
            ConnecString = ConfigurationManager.ConnectionStrings["CloudingSFA"].ConnectionString;
        }

        public static DataTable DataBind(string code, string type)
        {
            dt = new DataTable();
            using (connec = new SqlConnection(ConnecString))
            {
                using (cmdText = new SqlCommand("SelectSyncInfor", connec))
                {
                    cmdText.CommandType = CommandType.StoredProcedure;
                    cmdText.Parameters.Clear();

                    SqlParameter UserCode = cmdText.Parameters.Add("@UserCode", SqlDbType.NVarChar, 50);
                    SqlParameter ActionType = cmdText.Parameters.Add("@ActionType", SqlDbType.Int, 8);

                    UserCode.Value = code;
                    ActionType.Value = int.Parse(type);

                    connec.Open();
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmdText;
                    ds = new DataSet();
                    da.Fill(ds);

                    dt.Clear();
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }

        public static DataTable DataBind(string code, string type, string level, DateTime startTime, DateTime endTime)
        {
            dt = new DataTable();
            using (connec = new SqlConnection(ConnecString))
            {
                using (cmdText = new SqlCommand("SelectInforDetails", connec))
                {
                    cmdText.CommandType = CommandType.StoredProcedure;
                    cmdText.Parameters.Clear();

                    SqlParameter UserCode = cmdText.Parameters.Add("@UserCode", SqlDbType.NVarChar, 50);
                    SqlParameter ActionType = cmdText.Parameters.Add("@ActionType", SqlDbType.Int, 8);
                    SqlParameter loglevel = cmdText.Parameters.Add("@Level", SqlDbType.NVarChar, 50);
                    SqlParameter start = cmdText.Parameters.Add("@StartTime", SqlDbType.DateTime, 8);
                    SqlParameter end = cmdText.Parameters.Add("@EndTime", SqlDbType.DateTime, 8);

                    UserCode.Value = code;
                    ActionType.Value = int.Parse(type);
                    loglevel.Value = level;
                    start.Value = startTime;
                    end.Value = endTime;

                    connec.Open();
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmdText;
                    ds = new DataSet();
                    da.Fill(ds);

                    dt.Clear();
                    dt = ds.Tables[0];
                }
            }

            return dt;
        }

        public static DataTable DataBind(string code, DateTime startTime, DateTime endTime, bool isAllLogContent)
        {
            dt = new DataTable();
            using (connec = new SqlConnection(ConnecString))
            {
                using (cmdText = new SqlCommand("ErrorLogInforDetails", connec))
                {
                    cmdText.CommandType = CommandType.StoredProcedure;
                    cmdText.Parameters.Clear();

                    SqlParameter UserCode = cmdText.Parameters.Add("@UserCode", SqlDbType.NVarChar, 50);
                    SqlParameter start = cmdText.Parameters.Add("@StartTime", SqlDbType.DateTime, 8);
                    SqlParameter end = cmdText.Parameters.Add("@EndTime", SqlDbType.DateTime, 8);
                    SqlParameter isAll = cmdText.Parameters.Add("@IsAllLOGCONTENT", SqlDbType.Bit, 1);

                    UserCode.Value = code;
                    start.Value = startTime;
                    end.Value = endTime;
                    isAll.Value = isAllLogContent;

                    connec.Open();
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmdText;
                    ds = new DataSet();
                    da.Fill(ds);

                    dt.Clear();
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
    }
}
