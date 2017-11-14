using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using eBest.SyncServer;

public partial class TestPage_SynclogDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtStart.Text = this.txtEnd.Text = DateTime.Now.ToShortDateString();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string code = this.txtCode.Text;
        string type = this.type.SelectedValue;
        string level = this.Level.SelectedValue;
        DateTime startTime = DateTime.Parse(this.txtStart.Text);
        DateTime endTime = DateTime.Parse(this.txtEnd.Text);
        endTime = endTime.AddDays(1);

        if (!string.IsNullOrEmpty(this.txtCode.Text))
        {
            this.RpSyncInfor.DataSource = SyncInfor.DataBind(code, type, level, startTime, endTime);
            this.RpSyncInfor.DataBind();
        }
    }


    protected void btnClear_Click(object sender, EventArgs e)
    {
        this.RpSyncInfor.Controls.Clear();
    }
}
