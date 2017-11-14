using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;


namespace eBest.Mobile.SyncConfig
{
    public class SyncDBConfigManager
    {
        public IDbConnection _dbConnection = null;

        public System.Data.IDbConnection Connection
        {
            get
            {
                return _dbConnection;
            }
        }
    }

    public class SyncMasterServer : SyncDBConfigManager
    {
        public SyncMasterServer()
        {
            _dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CloudingSFA"].ConnectionString);
        }
    }

    public class SyncSlaveServer : SyncDBConfigManager
    {
        public SyncSlaveServer()
        {
            _dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CloudSFA_Photo"].ConnectionString);
        }
    }
}
