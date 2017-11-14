using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBest.Mobile.SyncHelper
{
    public class LoginRequest
    {
        /// <summary>
        /// 用户登陆名；
        /// </summary>
        /// 
        string _userName;
        [JsonProperty(Order = 1)]
        public string UserName {
            get
            {
                return _userName;
            }
            set 
            { 
                _userName = value.Trim(); 
            } 
        }

        /// <summary>
        /// 用户密码；
        /// </summary>
        /// 
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, Order = 2)]
        public string Password { get; set; }

        /// <summary>
        /// 是否修改密码；
        /// </summary>
        /// 
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, Order = 3)]
        public bool IsChangePwd { get; set; }

        /// <summary>
        /// 新密码；
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, Order = 4)]
        public string NewPassword { get; set; }

        /// <summary>
        /// 手机端平台；
        /// </summary>
        /// 
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, Order = 5)]
        public string PlatForm { get; set; }


        /// <summary>
        /// 手机端版本号；
        /// </summary>
        /// 
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, Order = 6)]
        public string VersionNum { get; set; }
    }
}
