using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using eBest.SyncServer;

public partial class TestPage_SyncLog : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["usercode"] != null)
            {
                string code = Request.QueryString["usercode"];
                string type = Request.QueryString["type"];                
                this.RpSyncInfor.DataSource = SyncInfor.DataBind(code, type);
                this.RpSyncInfor.DataBind();
            }
        }

    }
}

