using System;
using System.Threading.Tasks;
using System.Configuration;

using log4net;
using log4net.Appender;

namespace eBest.Mobile.SyncCommon
{
    /// <summary>
    /// Summary description for AdoNetAppenderExtension
    /// </summary>
    public class Log4netExtend : AdoNetAppender
    {
        public Log4netExtend()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private static ILog _Log;

        protected static ILog Log
        {
            get
            {

                if (_Log == null)

                    _Log = LogManager.GetLogger(typeof(Log4netExtend));

                return _Log;

            }

        }

        private string _ConnectionStringName;

        public override void ActivateOptions()
        {
            PopulateConnectionString();
            base.ActivateOptions();
        }

        private void PopulateConnectionString()
        {
            // if connection string already defined, do nothing  

            if (!String.IsNullOrEmpty(ConnectionString)) return;



            // if connection string name is not available, do nothing  

            if (String.IsNullOrEmpty(ConnectionStringName)) return;



            // grab connection string settings  

            ConnectionStringSettings settings = ConfigurationManager

                 .ConnectionStrings[ConnectionStringName];



            // if connection string name was not found in settings  

            if (settings == null)
            {

                // log error  

                if (Log.IsErrorEnabled)

                    Log.ErrorFormat("Connection String Name not found in Configuration: {0}",

                        ConnectionStringName);

                // do nothing more  

                return;

            }



            // retrieve connection string from the name  

            ConnectionString = settings.ConnectionString;

        }

        public string ConnectionStringName
        {
            get { return _ConnectionStringName; }
            set { _ConnectionStringName = value; }
        }
    }
}
