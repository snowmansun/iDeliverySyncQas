using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Text.RegularExpressions;
using System.Configuration;

/// <summary>
/// 数据库操作类
/// </summary>
public class DBHelper : IDisposable
{
    private const string APPSETTING_KEY = "CloudingSFA";

    private static string PROVIDER_NAME = "";
    private static string CONNECTION_STRING = "";

    private string mprovidername = "";
    private string mconnectionstring = "";

    private DbConnection dbcon;
    private DbTransaction dbtrans;
    private string mstrTaihiSQL;

    private DbProviderFactory mfactory;

    #region 静态构造
    static DBHelper()
    {
        Initialize();
        //Initialize(WebConfigurationManager.ConnectionStrings["showa"].ConnectionString);
    }
    #endregion

    #region 构造
    /// <summary>
    /// 构造一个数据库操作类的实例
    /// </summary>
    public DBHelper()
    {
        mprovidername = PROVIDER_NAME;
        mconnectionstring = CONNECTION_STRING;

        mfactory = DbProviderFactories.GetFactory(mprovidername);

        dbcon = this.CreateNewConnection();
    }

    /// <summary>
    /// 构造一个数据库操作类的实例
    /// </summary>
    /// <param name="transction">
    /// 是否使用事务
    /// 使用事务时，需要调用TransCommit方法提交事务，或者调用TransRollback回滚事务
    /// </param>
    public DBHelper(bool transction)
    {
        mprovidername = PROVIDER_NAME;
        mconnectionstring = CONNECTION_STRING;

        mfactory = DbProviderFactories.GetFactory(mprovidername);

        dbcon = this.CreateNewConnection();

        if (transction)
        {
            this.TrnStart();
        }
    }

    /// <summary>
    /// 指定connectionstring构造一个数据库操作类的实例
    /// </summary>
    /// <param name="connectionstringkey">web.config中connectionstring的name</param>
    public DBHelper(string connectionstringkey)
    {
        mprovidername = ConfigurationManager.AppSettings["ProviderName"];
        mconnectionstring = ConfigurationManager.AppSettings[connectionstringkey];

        mfactory = DbProviderFactories.GetFactory(mprovidername);

        dbcon = this.CreateNewConnection();
    }

    /// <summary>
    /// 指定connectionstring构造一个数据库操作类的实例
    /// </summary>
    /// <param name="connectionstringkey">web.config中connectionstring的name</param>
    /// <param name="transction">
    /// 是否使用事务
    /// 使用事务时，需要调用TransCommit方法提交事务，或者调用TransRollback回滚事务
    /// </param>
    public DBHelper(string connectionstringkey, bool transction)
    {
        mprovidername = ConfigurationManager.AppSettings["ProviderName"];
        mconnectionstring = ConfigurationManager.AppSettings["ConnectionString"];

        mfactory = DbProviderFactories.GetFactory(mprovidername);

        dbcon = this.CreateNewConnection();

        if (transction)
        {
            this.TrnStart();
        }
    }
    #endregion

    #region Initialize (static)
    /// <summary>
    /// 初始化DbHelperUtility的数据源提供者名称和连接字符串
    /// </summary>
    public static void Initialize()
    {
        //string value = System.Configuration.ConfigurationManager.ConnectionStrings[APPSETTING_KEY].ToString();

        DBHelper.Initialize(APPSETTING_KEY);
    }

    /// <summary>
    /// 初始化DbHelperUtility的数据源提供者名称和连接字符串
    /// </summary>
    /// <param name="connectionstringkey">web.config中connectionstring的name</param>
    public static void Initialize(string connectionstringkey)
    {

        PROVIDER_NAME = ConfigurationManager.ConnectionStrings["CloudingSFA"].ProviderName;
        CONNECTION_STRING = ConfigurationManager.ConnectionStrings["CloudingSFA"].ConnectionString;
    }

    /// <summary>
    /// 初始化DbHelperUtility的数据源提供者名称和连接字符串
    /// </summary>
    /// <param name="providername">数据源提供者名称</param>
    /// <param name="connectionstring">数据库连接字符串</param>
    public static void Initialize(string providername, string connectionstring)
    {
        PROVIDER_NAME = providername;
        CONNECTION_STRING = connectionstring;
    }
    #endregion

    #region Dispose

    public void Dispose()
    {
        //关闭连接
        this.Close();

        dbcon.Dispose();

        dbcon = null;

        System.GC.SuppressFinalize(this);
    }

    #endregion

    #region property

    /// <summary>
    /// 数据提供者名称
    /// </summary>
    public string DbProvider
    {
        get { return mprovidername; }
        set { mprovidername = value; }
    }

    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public string ConnectionString
    {
        get { return mconnectionstring; }
        set { mconnectionstring = value; }
    }

    /// <summary>
    /// 获取当前数据库连接
    /// </summary>
    public DbConnection DBConnection
    {
        get
        {
            return dbcon;
        }
    }

    /// <summary>
    /// 取得最后一次执行的sql语句
    /// </summary>
    public string TaihiSQL
    {
        get
        {
            return mstrTaihiSQL;
        }
    }
    #endregion

    #region CreateNewConnection
    public DbConnection CreateNewConnection()
    {
        DbConnection con = mfactory.CreateConnection();
        con.ConnectionString = mconnectionstring;
        con.Open();

        //using (DbCommand cmd = con.CreateCommand())
        //{
        //    cmd.CommandText = "SET NAMES 'UTF8';";
        //    cmd.ExecuteNonQuery();
        //}

        return con;
    }
    #endregion

    #region Open/Close
    /// <summary>
    /// 打开数据库连接
    /// </summary>
    /// <returns></returns>
    public void Open()
    {
        dbcon.ConnectionString = mconnectionstring;

        //打开连接
        dbcon.Open();

        //using (DbCommand cmd = dbcon.CreateCommand())
        //{
        //    cmd.CommandText = "SET NAMES 'UTF8';";
        //    cmd.ExecuteNonQuery();
        //}
    }

    /// <summary>
    /// 关闭数据库连接
    /// </summary>
    /// <returns></returns>
    public void Close()
    {
        if (dbcon.State == System.Data.ConnectionState.Open)
        {
            dbcon.Close();
        }
    }
    #endregion

    #region CreateCommand (private)
    private DbCommand CreateCommand()
    {
        if (dbcon.State == System.Data.ConnectionState.Closed) this.Open();

        DbCommand cmd = dbcon.CreateCommand();

        if (dbtrans != null) cmd.Transaction = dbtrans;

        return cmd;
    }
    #endregion

    #region CreateParameter
    /// <summary>
    /// 创建sql参数
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public DbParameter CreateParameter(string name, object value)
    {
        DbParameter param = mfactory.CreateParameter();

        param.ParameterName = name;
        param.Value = (value == null ? DBNull.Value : value);

        return param;
    }

    /// <summary>
    /// 创建sql参数
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="dbtype"></param>
    /// <returns></returns>
    public DbParameter CreateParameter(string name, object value, DbType dbtype)
    {
        DbParameter param = mfactory.CreateParameter();

        param.ParameterName = name;
        param.Value = (value == null ? DBNull.Value : value);
        param.DbType = dbtype;

        return param;
    }

    /// <summary>
    /// 创建sql参数
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="dbtype"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public DbParameter CreateParameter(string name, object value, DbType dbtype, int size)
    {
        DbParameter param = mfactory.CreateParameter();

        param.ParameterName = name;
        param.Value = (value == null ? DBNull.Value : value);
        param.DbType = dbtype;
        param.Size = size;

        return param;
    }
    #endregion

    #region ExecuteReader
    /// <summary>
    /// 执行sql语句（select），返回OleDbDataReader
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public DbDataReader ExecuteReader(string sql)
    {
        mstrTaihiSQL = sql;
        DbConnection con = this.CreateNewConnection(); // Modify by Li Zeyou 20150116 fixed the dbhelper bug

        try
        {
            using (DbCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = sql;
                return cmd.ExecuteReader(CommandBehavior.CloseConnection); // Modify by Li Zeyou 20150116 fixed the dbhelper bug
            }
        }
        catch
        {
            con.Close();
            throw;
        }
    }

    /// <summary>
    /// 执行sql语句（select），返回OleDbDataReader
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public DbDataReader ExecuteReader(string sql, DbParameter[] arrparm)
    {
        mstrTaihiSQL = sql;

        using (DbConnection con = this.CreateNewConnection())
        {
            con.Open();

            using (DbCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = sql;

                //执行sql的参数
                cmd.Parameters.Clear();
                for (int i = 0; i < arrparm.Length; i++)
                {
                    cmd.Parameters.Add(arrparm[i]);
                }

                return cmd.ExecuteReader();
            }
        }
    }
    #endregion

    #region ExecuteNonQuery
    /// <summary>
    /// 执行sql语句（insert，update，delete）
    /// </summary>
    /// <param name="sql"></param>
    /// <returns>sql执行影响的记录数</returns>
    public int ExecuteNonQuery(string sql)
    {
        mstrTaihiSQL = sql;

        using (DbCommand cmd = this.CreateCommand())
        {
            cmd.CommandText = sql;
            return cmd.ExecuteNonQuery();
        }
    }

    /// <summary>
    /// 执行带参数的sql语句（insert，update，delete）
    /// </summary>
    /// <param name="sql"></param>
    /// <returns>sql执行影响的记录数</returns>
    public int ExecuteNonQuery(string sql, DbParameter[] arrparm)
    {
        mstrTaihiSQL = sql;

        using (DbCommand cmd = this.CreateCommand())
        {
            cmd.CommandText = sql;

            //执行sql的参数
            cmd.Parameters.Clear();
            for (int i = 0; i < arrparm.Length; i++)
            {
                cmd.Parameters.Add(arrparm[i]);
            }

            return cmd.ExecuteNonQuery();
        }
    }
    #endregion

    /// <summary>
    /// 执行SP
    /// </summary>
    /// <param name="procName"></param>
    /// <returns>sql执行影响的记录数</returns>
    public int ExecuteNonProcedure(string procname, DbParameter[] arrparm)
    {
        mstrTaihiSQL = procname;

        using (DbConnection con = this.CreateNewConnection())
        {
            //con.Open();

            using (DbCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procname;

                //执行sql的参数
                cmd.Parameters.Clear();
                for (int i = 0; i < arrparm.Length; i++)
                {
                    cmd.Parameters.Add(arrparm[i]);
                }

                return cmd.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// 执行SP
    /// </summary>
    /// <param name="procName"></param>
    /// <returns>sql执行影响的记录数</returns>
    public int ExecuteNonProcedure(string procname)
    {
        mstrTaihiSQL = procname;

        using (DbConnection con = this.CreateNewConnection())
        {
            //con.Open();

            using (DbCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procname;


                return cmd.ExecuteNonQuery();
            }
        }
    }


    #region ExecuteProcedure
    /// <summary>
    /// 执行存储过程，返回DbDataReader
    /// </summary>
    /// <param name="procname">存储过程名称</param>
    /// <param name="arrparm">参数</param>
    /// <returns>存储过程执行结果DbDataReader</returns>
    public DbDataReader ExeDataReaderProcedure(string procname, DbParameter[] arrparm)
    {
        mstrTaihiSQL = procname;

        DbConnection con = this.CreateNewConnection(); // Modify by Li Zeyou
        //con.Open();

        try
        {
            using (DbCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procname;

                //执行sql的参数
                cmd.Parameters.Clear();
                for (int i = 0; i < arrparm.Length; i++)
                {
                    cmd.Parameters.Add(arrparm[i]);
                }

                return cmd.ExecuteReader(CommandBehavior.CloseConnection); // Modify by Li Zeyou
            }
        }
        catch
        {
            con.Close();
            throw;
        }
    }

    /// <summary>
    /// 执行存储过程，返回DataSet
    /// </summary>
    /// <param name="procname">存储过程名称</param>
    /// <param name="arrparm">参数</param>
    /// <returns>存储过程执行结果DataSet</returns>
    public DataSet ExeDatasetProcedure(string procname, DbParameter[] arrparm)
    {
        mstrTaihiSQL = procname;

        using (DbCommand cmd = this.CreateCommand())
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procname;

            //执行sql的参数
            cmd.Parameters.Clear();
            for (int i = 0; i < arrparm.Length; i++)
            {
                cmd.Parameters.Add(arrparm[i]);
            }

            DataSet ds = new DataSet();
            DbDataAdapter da = mfactory.CreateDataAdapter();
            cmd.CommandTimeout = 0;
            da.SelectCommand = cmd;

            da.Fill(ds);

            Close();
            return ds;
        }
    }
    #endregion

    #region GetDataSet
    /// <summary>
    /// 执行sql语句返回DataSet
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public DataSet GetDataSet(string sql)
    {
        mstrTaihiSQL = sql;

        using (DbCommand cmd = this.CreateCommand())
        {
            DataSet ds = new DataSet();
            DbDataAdapter da = mfactory.CreateDataAdapter();
            //cmd.CommandTimeout = Convert.ToInt16(ConfigurationManager.AppSettings["TimeOut"]);
            cmd.CommandTimeout = 0;
            cmd.CommandText = sql;
            da.SelectCommand = cmd;

            da.Fill(ds);

            Close();
            return ds;
        }
    }



    /// <summary>
    /// 执行sql语句返回DataSet
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public DataSet GetDataSet(string sql, DbParameter[] arrparm)
    {
        mstrTaihiSQL = sql;

        using (DbCommand cmd = this.CreateCommand())
        {
            cmd.CommandText = sql;

            //执行sql的参数
            cmd.Parameters.Clear();
            for (int i = 0; i < arrparm.Length; i++)
            {
                cmd.Parameters.Add(arrparm[i]);
            }

            DataSet ds = new DataSet();
            DbDataAdapter da = mfactory.CreateDataAdapter();
            cmd.CommandTimeout = 0;
            cmd.CommandText = sql;
            da.SelectCommand = cmd;

            da.Fill(ds);

            Close();
            return ds;
        }
    }

    /// <summary>
    /// 执行sql语句Count
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public Int32 GetDataCount(string sql)
    {
        mstrTaihiSQL = sql;

        using (DbCommand cmd = this.CreateCommand())
        {
            DataSet ds = new DataSet();
            DbDataAdapter da = mfactory.CreateDataAdapter();
            //cmd.CommandTimeout = Convert.ToInt16(ConfigurationManager.AppSettings["TimeOut"]);
            cmd.CommandTimeout = 0;
            cmd.CommandText = sql;
            da.SelectCommand = cmd;

            da.Fill(ds);

            Close();
            return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        }
    }

    #endregion

    #region Transaction处理
    /// <summary>
    /// 开始事务处理
    /// </summary>
    /// <returns></returns>
    public void TrnStart()
    {
        if (dbcon.State == System.Data.ConnectionState.Closed) this.Open();

        dbtrans = dbcon.BeginTransaction();
    }

    /// <summary>
    /// 提交事务
    /// </summary>
    /// <returns></returns>
    public void TrnCommit()
    {
        if (dbcon.State == System.Data.ConnectionState.Closed) this.Open();

        if (dbtrans != null)
        {
            dbtrans.Commit();
        }
    }

    /// <summary>
    /// 撤销事务
    /// </summary>
    /// <returns></returns>
    public void TrnRollBack()
    {
        try
        {
            if (dbcon.State == System.Data.ConnectionState.Closed) this.Open();

            if (dbtrans != null)
            {
                dbtrans.Rollback();
            }
        }
        catch { }
    }
    #endregion
}
