using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

using eBest.Mobile.SyncEntities;
using Newtonsoft.Json;

namespace eBest.Mobile.SyncHelper
{
    public class SyncRequest
    {
        /// <summary>
        /// 用户登录名；
        /// </summary>
        /// 
        [JsonProperty(Order = 1)]
        public string LoginName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        /// 
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, Order = 2)]
        public string PassWord { get; set; }

        /// <summary>
        /// 用户所在域
        /// </summary>
        /// 
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, Order = 3)]
        public string DomainID { get; set; }

        /// <summary>
        /// 默认版本号；
        /// </summary>
        /// 
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, Order = 4)]
        public string Version { get; set; }

        /// <summary>
        /// 是否压缩；
        /// </summary>
        /// 
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, Order = 5)]
        public string IsGzip { get; set; }

        /// <summary>
        /// 请求内容(基础数据实体集合)；
        /// </summary>
        /// 
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, Order = 6)]
        public SyncTables ReqContent { get; set; }

    }
}
