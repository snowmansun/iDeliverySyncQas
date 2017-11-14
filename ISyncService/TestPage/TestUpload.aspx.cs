using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Net;
using System.Text;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Newtonsoft.Json;
using eBest.Mobile.SyncEntities;
using eBest.Mobile.SyncHelper;
using eBest.Mobile.SyncCommon;
using System.Threading;
using System.Diagnostics;

namespace eBest.SyncServer
{
    public partial class TestDownload : System.Web.UI.Page
    {
        int i = 0;
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
            
            if (isCheck.Checked)
                IsGzip = "1";
            else
                IsGzip = "0";
            bool enableZip = IsGzip == null ? true : IsGzip == "1";

            string myUrl = txtAddress.Text;
            if (IsPhoto.Checked)
                myUrl += "photo.aspx";
            else
                myUrl += "upload.aspx";

            //FileStream fs = new FileStream(@"D:\20141124112735.jpg", FileMode.Open, FileAccess.Read);
            //byte[] bytes = new byte[fs.Length];
            //fs.Read(bytes,0,(Int32)fs.Length);
            //string base64 =Convert.ToBase64String( bytes);
            //photoContent.Rows.Add(string.Format("201401030▏1▏{0}▏1▏2014-11-24 10:38:56", base64));

            SyncTable table = new SyncTable();
            table.Name = this.xml.SelectedValue.TrimEnd(new char[] { '.', 'x', 'm', 'l' });
            table.Rows.Add(this.txtTableList.Text);

            SyncTables tables = new SyncTables();
            tables.Tables.Add(table);

            SyncRequest request = new SyncRequest();
            request.DomainID = this.txtDomainID.Text;
            request.Version = this.txtVersion.Text;
            request.LoginName = this.txtLoginName.Text;
            request.PassWord = this.txtPassword.Text;
            request.ReqContent = tables;

            string myParams = JsonConvert.SerializeObject(request);
            string myResult = MyRequest.SyncHttpResult(myUrl, myParams, enableZip);
            lblContent.Text = myResult;

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
