using System;
using System.Data;
using System.Net;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Net.Security;
using System.Web.Security;
using System.Collections;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;

using Newtonsoft.Json;
using eBest.Mobile.SyncEntities;
using eBest.Mobile.SyncHelper;
using eBest.Mobile.SyncCommon;


namespace eBest.SyncServer
{
    public partial class TestDownload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int index = Context.Request.Url.ToString().IndexOf("TestPage", StringComparison.OrdinalIgnoreCase);
            string path = Context.Request.Url.ToString().Substring(0, index);
            this.txtAddress.Text = path;

            if (!IsPostBack)
            {
                xml.DataSource = SyncConfig.GetXmlConfigName();
                this.DataBind();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string IsGzip = string.Empty;
            string result = string.Empty;

            if (isCheck.Checked)
                IsGzip = "1";
            else IsGzip = "0";
            bool enableZip = IsGzip == null ? true : IsGzip == "1";

            SyncTable table = new SyncTable();
            table.Name = txtTableList.Text;
            table.ParamValues = new string[] { "1753-01-01 00:00:00.000"};
           
            SyncTables tables = new SyncTables();
            tables.Tables.Add(table);

            SyncRequest request = new SyncRequest();
            request.LoginName = txtLoginName.Text;
            request.PassWord = txtPassword.Text;
            request.DomainID = txtDomainID.Text;
            request.Version = txtVersion.Text;
            request.IsGzip = IsGzip;
            request.ReqContent = tables;

            string myUrl = txtAddress.Text + "download.aspx";
            string myParams = JsonConvert.SerializeObject(request);
            string myResult = MyRequest.SyncHttpResult(myUrl, myParams, enableZip);

            SyncResult syncResult = new SyncResult();
            syncResult = JsonConvert.DeserializeObject<SyncResult>(myResult);

            if (syncResult.Status == SyncStatus.Success)
            {
                tables = JsonConvert.DeserializeObject<SyncTables>(syncResult.Result.ToString());
                foreach (SyncTable tab in tables.Tables)
                    foreach (string row in tab.Rows)
                        result += row + "<br/>";
            }
            else
                result = myResult;

            lblContent.Text = result;
        }


        protected void content_Click(object sender, EventArgs e)
        {
            if (this.xml.SelectedIndex > 0)
            {
                string currentXml = eBest.SyncServer.SyncConfig.ConfigTablesFolder + this.xml.SelectedItem;
                StreamReader reader = new StreamReader(currentXml);
                string xmlContent = reader.ReadToEnd();

                Session.Clear();
                Session["content"] = xmlContent;
                Response.Write("<script>window.open('ViewXml.aspx')</script>");
                this.xml.SelectedIndex = 0;
            }
        }
    }
}
