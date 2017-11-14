using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace eBest.Mobile.SyncEntities
{
    public class SyncTables
    {
        /// <summary>
        /// 请求表实体集合；
        /// </summary>
        /// 
        public List<SyncTable> Tables { get; set; }

        public SyncTables()
        {
            Tables = new List<SyncTable>();
        }
    }

    public class SyncTable
    {
        /// <summary>
        /// 请求表名；
        /// </summary>
        /// 
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// 请求表参数；
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] ParamValues { get; set; }

        /// <summary>
        /// 请求表字段；
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Fields { get; set; }

        /// <summary>
        /// 请求表行数据；
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Rows { get; set; }

        public SyncTable()
        {
            Rows = new List<string>();
        }

    }
}
