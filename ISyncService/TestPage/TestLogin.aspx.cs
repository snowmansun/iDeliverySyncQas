using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

using Newtonsoft.Json;
using eBest.Mobile.SyncEntities;
using eBest.Mobile.SyncHelper;
using eBest.Mobile.SyncConfig;
using System.Configuration;


public partial class TestPage_TestLogin : System.Web.UI.Page
{
    private static readonly SyncConfigManager mySync = (SyncConfigManager)ConfigurationManager.GetSection("sync");
    protected void Page_Load(object sender, EventArgs e)
    {
        int index = Context.Request.Url.ToString().IndexOf("TestPage", StringComparison.OrdinalIgnoreCase);
        string path = Context.Request.Url.ToString().Substring(0, index);
        this.txtAddress.Text = path;
        this.txtVersion.Text = mySync.Packages[txtPlatForm.Text.Trim()].VersionNum;
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        string url = string.Empty;
        string result = string.Empty;

        LoginRequest login = new LoginRequest()
        {
            UserName = txtLoginName.Text,
            Password = txtPassword.Text,
            NewPassword = txtNewPassword.Text,
            PlatForm = txtPlatForm.Text,
            VersionNum = txtVersion.Text
        };

        if (isChange.Checked)
            login.IsChangePwd = true;

        url = this.txtAddress.Text + "login.aspx";

        string tmp = JsonConvert.SerializeObject(login);
        result = MyRequest.LoginHttpResult(url, JsonConvert.SerializeObject(login));
        
        lblContent.Text = result;
    }
}