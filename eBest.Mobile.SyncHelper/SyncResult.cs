using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;
using eBest.Mobile.SyncEntities;

namespace eBest.Mobile.SyncHelper
{
    public enum SyncStatus
    {
        /// <summary>
        /// 同步成功
        /// </summary>
        Success = 1,

        /// <summary>
        /// 同步失败
        /// </summary>
        Failed = 0
    }

    public enum SyncType
    {
        /// <summary>
        /// 登录
        /// </summary>
        Login = 0,

        /// <summary>
        /// 数据下载
        /// </summary>
        Download = 1,

        /// <summary>
        /// 数据上传
        /// </summary>
        Upload = 2,

        /// <summary>
        /// 图片上传
        /// </summary>
        Photo = 3,
        /// <summary>
        /// 修改密码
        /// </summary>
        ChangePassword = 4
    }

    public enum ExceptionType
    {
        /// <summary>
        /// 帐号密码不正确
        /// </summary>
        AuthenticationFailed = 1,

        /// <summary>
        /// 密码过期
        /// </summary>
        MustChangePassword = 2,

        /// <summary>
        /// 帐号锁定
        /// </summary>
        IsLocked = 3,

        /// <summary>
        /// 帐号无效
        /// </summary>
        InActive = 4,

        /// <summary>
        /// 版本更新
        /// </summary>
        VersionUpdate = 5,

        /// <summary>
        /// 其他
        /// </summary>
        Other = 6,

        /// <summary>
        /// 当前司机没有分配Shipment
        /// </summary>
        NoShipment = 7,

        /// <summary>
        /// 未配置正确的版本信息
        /// </summary>
        IncorrectVersion = 8
    }

    public class SyncResult
    {
        /// <summary>
        /// 同步用户；
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 同步状态；
        /// </summary>
        public SyncStatus Status { get; set; }

        /// <summary>
        /// 失败原因
        /// </summary>
        public ExceptionType? ExceptionCode { get; set; }

        /// <summary>
        /// 失败原因
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// 同步类型；
        /// </summary>
        public SyncType SyncType { get; set; }


        /// <summary>
        /// 同步结果；
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public object Result { get; set; }
    }
}
