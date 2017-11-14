using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eBest.Mobile.SyncEntities
{
    /// <summary>
    /// Summary description for UserEntity
    /// </summary>
    public class UserEntity
    {
        private string _UserCode;
        private int _OrgId;
        private string _UserName;
        private string _Device;
        private Boolean _Alert;

        private string _ServerTime;

        public UserEntity()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        /// <summary>
        /// 组织架构
        /// </summary>
        public int OrgId
        {
            get { return _OrgId; }
            set { _OrgId = value; }
        }
        /////<summary>
        /////用户编号
        /////</summary>
        //public string UserCode 
        //{
        //    get { return _UserCode; }
        //    set { _UserCode = value; }
        //}
        /// <summary>
        /// 用户名称；
        /// </summary>
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Device
        {
            get { return _Device; }
            set { _Device = value; }
        }
        public bool Alert
        {
            get { return _Alert; }
            set { _Alert = value; }
        }
        /// <summary>
        /// 服务器时间；
        /// </summary>
        public string ServerTime
        {
            get { return _ServerTime; }
            set { _ServerTime = value; }
        }

        /// <summary>
        /// 版本信息
        /// </summary>
        private VersionInfo _VersionInfo;
        public VersionInfo Version
        {
            get { return _VersionInfo; }
            set { _VersionInfo = value; }
        }
    }

    public class VersionInfo
    {
        public VersionInfo()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        /// <summary>
        /// 当前手机端是否最新版本
        /// </summary>
        private Boolean _IsLastVersion;
        public Boolean IsLastVersion
        {
            get { return _IsLastVersion; }
            set { _IsLastVersion = value; }
        }

        /// <summary>
        /// 下载地址
        /// </summary>
        private string _DownloadUrl;
        public string DownloadUrl
        {
            get { return _DownloadUrl; }
            set { _DownloadUrl = value; }
        }

        /// <summary>
        /// 版本号；
        /// </summary>
        private string _VersionNum;
        public string VersionNum
        {
            get { return _VersionNum; }
            set { _VersionNum = value; }
        }
        /// <summary>
        /// apk大小
        /// </summary>
        private string _FileSize;
        public string FileSize
        {
            get { return _FileSize; }
            set { _FileSize = value; }
        }

        /// <summary>
        /// 版本更新描述文件名访问路径；
        /// </summary>
        private string _UpdateDesc;
        public string UpdateDesc
        {
            get { return _UpdateDesc; }
            set { _UpdateDesc = value; }
        }
    }
    public class UserInfo
    {
        private int _OrgId;
        public int OrgId
        {
            get { return _OrgId; }
            set { _OrgId = value; }
        }

        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        private Boolean _IsLock;
        public Boolean IsLock
        {
            get { return _IsLock; }
            set { _IsLock = value; }
        }

        private Boolean _IsValid;
        public Boolean IsValid
        {
            get { return _IsValid; }
            set { _IsValid = value; }
        }

        private string _FileName;
        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        private string _FileSize;
        public string FileSize
        {
            get { return _FileSize; }
            set { _FileSize = value; }
        }

        private Boolean _Alert;
        public Boolean Alert
        {
            get { return _Alert; }
            set { _Alert = value; }
        }

        private string _Version;
        public string Version
        {
            get { return _Version; }
            set { _Version = value; }
        }

        private string _Device;
        public string Device
        {
            get { return _Device; }
            set { _Device = value; }
        }

        private Boolean _NeedChangePassword;
        public Boolean NeedChangePassword
        {
            get { return _NeedChangePassword; }
            set { _NeedChangePassword = value; }
        }
    }

}
