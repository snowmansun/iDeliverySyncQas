using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Configuration;

using Newtonsoft.Json;
using eBest.Mobile.SyncCommon;
using eBest.Mobile.SyncConfig;
using eBest.Mobile.SyncEntities;
using eBest.Mobile.SyncHelper;
using System.Data.SqlClient;

namespace eBest.SyncServer
{
    public class LoginSync : IHttpHandler
    {
        #region Parameters

        private static readonly log4net.ILog mylog = log4net.LogManager.GetLogger("Login");
        private static readonly SyncConfigManager mySync = (SyncConfigManager)ConfigurationManager.GetSection("sync");
        private StringBuilder sb = new StringBuilder();
        private SyncMasterServer server = new SyncMasterServer();
        private IDbCommand command;
        private LoginRequest entity;

        private SyncResult syncResult;
        private UserEntity User;
        DBHelper dbHelper = new DBHelper();

        #endregion

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpResponse response = context.Response;
            response.Clear();
            response.ContentType = "text/plain;text/json;charset=UTF-8";

            syncResult = new SyncResult();

            string readerContent = new StreamReader(context.Request.InputStream).ReadToEnd();
            try
            {
                entity = JsonConvert.DeserializeObject<LoginRequest>(readerContent);

                syncResult.LoginName = entity.UserName;
                syncResult.ExceptionCode = null;

                log4net.ThreadContext.Properties["UserName"] = entity.UserName;
                log4net.ThreadContext.Properties["ActionType"] = 0;

                mylog.Info(JsonConvert.SerializeObject(entity));
                //IsExist UserInfor;
                UserInfo uInfo = LoginAuthentication();
                User = new UserEntity();
                if (uInfo.UserName != null)
                {
                    //Operation Change Password
                    if (entity.IsChangePwd && !string.IsNullOrEmpty(entity.NewPassword))
                    {
                        syncResult.SyncType = SyncType.ChangePassword;
                        string pwd = string.Empty;
                        pwd = Decode.MD5Convert(entity.NewPassword);
                        ChangePassword(entity.UserName, pwd);

                        syncResult.Status = SyncStatus.Success;
                    }
                    else
                    {
                        SyncStatus status = SyncStatus.Success;
                        syncResult.SyncType = SyncType.Login;
                        //密码过期
                        if (uInfo.NeedChangePassword)
                        {
                            syncResult.ExceptionCode = ExceptionType.MustChangePassword;
                            status = SyncStatus.Failed;
                        }
                        //帐号锁定
                        else if (uInfo.IsLock)
                        {
                            syncResult.ExceptionCode = ExceptionType.IsLocked;
                            status = SyncStatus.Failed;
                        }
                        //帐号无效
                        else if (!uInfo.IsValid)
                        {
                            syncResult.ExceptionCode = ExceptionType.InActive;
                            status = SyncStatus.Failed;
                        }
                        // 未配置版本信息
                        else if (uInfo.Version.Equals(""))
                        {
                            syncResult.ExceptionCode = ExceptionType.IncorrectVersion;
                            status = SyncStatus.Failed;
                        }

                        else if (CheckShipment() == false)
                        {
                            syncResult.ExceptionCode = ExceptionType.NoShipment;
                            status = SyncStatus.Failed;
                        }
                        
                        if (status == SyncStatus.Success)
                        {
                            User.UserName = uInfo.UserName;
                            User.OrgId = uInfo.OrgId;
                            User.Device = uInfo.Device;
                            User.Alert = uInfo.Alert;
                            //Get VersionNum and PackagePath
                            string path = context.Request.Url.ToString().Substring(0, context.Request.Url.ToString().ToLower().IndexOf("login.aspx")) + "Package/";

                            VersionInfo version = new VersionInfo();
                            version.VersionNum = uInfo.Version;//mySync.Packages[entity.PlatForm].VersionNum;
                            version.IsLastVersion = true;
                            
                            //当前手机端手否最新版本
                            int clientVersion =  0;
                            int.TryParse(entity.VersionNum.Replace(".",""),out clientVersion);
                            if (clientVersion < int.Parse(version.VersionNum.Replace(".", "")))
                            {
                                version.IsLastVersion = false;
                            }
                            //如果不是最新版本，并且后天设置需要版本更新，则登录失败
                            if (User.Alert && !version.IsLastVersion)
                            {
                                syncResult.ExceptionCode = ExceptionType.VersionUpdate;
                                status = SyncStatus.Failed;
                            }
                            if (!entity.PlatForm.Equals("IPhone", StringComparison.OrdinalIgnoreCase))
                            {
                                version.DownloadUrl = path + uInfo.FileName;//mySync.Packages[entity.PlatForm].File;
                                version.UpdateDesc = path + uInfo.FileName + ".txt";//mySync.Packages[entity.PlatForm].UpDesc;
                            }
                            else
                                version.DownloadUrl = uInfo.FileName;// mySync.Packages[entity.PlatForm].File;

                            version.FileSize = uInfo.Version;

                            User.Version = version;
                            User.ServerTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        syncResult.Result = User;
                        syncResult.Status = status;

                        
                    }
                }
                else
                {
                    //throw new Exception("the user authentication failed");
                    syncResult.ExceptionCode = ExceptionType.AuthenticationFailed;
                    syncResult.Status = SyncStatus.Failed;
                }
            }
            catch (JsonReaderException ex)
            {
                syncResult.ExceptionCode = ExceptionType.Other;
                syncResult.Status = SyncStatus.Failed;
            }
            catch (Exception err)
            {
                syncResult.ExceptionCode = ExceptionType.Other;
                syncResult.Status = SyncStatus.Failed;
                syncResult.Exception = err.Message;
            }
            finally
            {
                response.Write(JsonConvert.SerializeObject(syncResult));
                //response.Write(JsonHelper.GetJsonByObject(syncResult));

                if (syncResult.Status == SyncStatus.Success)
                {
                    mylog.Info(JsonConvert.SerializeObject(syncResult));
                }
                else
                {
                    mylog.Error(JsonConvert.SerializeObject(syncResult));
                }

                if (server.Connection != null && server.Connection.State == ConnectionState.Open)
                {
                    server.Connection.Close();
                    server.Connection.Dispose();
                    server = null;
                }
            }
        }

        private UserInfo LoginAuthentication()
        {
            entity.Password = Decode.MD5Convert(entity.Password);

            UserInfo User = new UserInfo();
            SqlParameter[] prams = { 
                                       new SqlParameter("@UserCode", entity.UserName), 
                                       new SqlParameter("@Password", entity.Password)
                                   };

            using (DataTable dt = dbHelper.ExeDatasetProcedure("sp_GetUserInfo", prams).Tables[0])
            {
                bool isSuccess = false;
                if (dt != null && dt.Rows.Count > 0)
                {
                    isSuccess = true;
                    User.OrgId = int.Parse(dt.Rows[0]["OrgId"].ToString());
                    User.UserName = dt.Rows[0]["UserName"].ToString();
                    User.IsLock = Boolean.Parse(dt.Rows[0]["IsLock"].ToString());
                    User.IsValid = Boolean.Parse(dt.Rows[0]["IsValid"].ToString());
                    User.Alert = dt.Rows[0]["Alert"].ToString() == "1";
                    User.FileName = dt.Rows[0]["FileName"].ToString();
                    User.FileSize = dt.Rows[0]["FileSize"].ToString();
                    User.Version = dt.Rows[0]["Version"].ToString();
                    User.NeedChangePassword = dt.Rows[0]["NeedChangePassword"].ToString() == "1" ? true : false;
                }
                SqlParameter[] pramsUsp = { 
                                            new SqlParameter("@UserCode", entity.UserName), 
                                            new SqlParameter("@Status", isSuccess ? 1 : 0)
                                          };
                dbHelper.ExecuteNonProcedure("usp_UserLoginStatus", pramsUsp);
            }
            return User;
        }

        private void ChangePassword(string userName, string pwd)
        {
            try
            {
                SqlParameter[] prams = { 
                                        new SqlParameter("@UserCode", userName), 
                                        new SqlParameter("@NewPassword", pwd)
                                       };
                dbHelper.ExecuteNonProcedure("usp_ChangePassword", prams);
            }
            catch
            {
                throw new Exception("current user change passsword failed");
            }
        }

        private bool CheckShipment()
        {
            try
            {
                SqlParameter[] prams = {
                                           new SqlParameter("@userCode",entity.UserName)
                                       };
                using (DataTable dt = dbHelper.ExeDatasetProcedure("usp_CheckShipment", prams).Tables[0])
                {
                    if (dt != null & dt.Rows.Count > 0 )
                    {
                        if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                            return true;
                        else
                            return false;
                    }
                }
            }
            catch
            {
            }

            return false;
        }
    }
}