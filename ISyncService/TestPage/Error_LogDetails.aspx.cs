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
        DateTime startTime = DateTime.Parse(this.txtStart.Text);
        DateTime endTime = DateTime.Parse(this.txtEnd.Text);
        endTime = endTime.AddDays(1);

        this.RpSyncInfor.DataSource = SyncInfor.DataBind(code, startTime, endTime, false);
        this.RpSyncInfor.DataBind();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        this.RpSyncInfor.Controls.Clear();
    }

    /// <summary>
    /// csv导出
    /// </summary>
    /// <param name="ds"></param>
    public void DataTableToCsv(string FileName, DataTable dt)
    {
        //ds = bll._GetAnswer("");
        string data = "";
        int it = 1;
        int ir = 1;

        //data += tb.TableName + "\n";
        //写出列名
        foreach (DataColumn column in dt.Columns)
        {
            //用Tab分隔，可换为其他符号
            data += column.ColumnName + ",";
        }
        data = data.Substring(0, data.Length - 1);
        data += Environment.NewLine;

        ir = 1;
        //写出数据
        foreach (DataRow row in dt.Rows)
        {

            foreach (DataColumn column in dt.Columns)
            {
                //用Tab分隔，可换为其他符号
                data += row[column].ToString() + ",";
            }
            data = data.Substring(0, data.Length - 1);
            if (ir++ < dt.Rows.Count)
            {
                data += Environment.NewLine;
            }
        }

        if (data != "")
        {
            string temp = string.Format("attachment;filename={0}", FileName + ".csv");
            Response.ClearHeaders();
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("Unicode");
            Response.AppendHeader("Content-disposition", temp);
            Response.Write(data);
            Response.End();
        }
    }

    protected void btnToExcel_Click(object sender, EventArgs e)
    {
        string code = this.txtCode.Text;
        DateTime startTime = DateTime.Parse(this.txtStart.Text);
        DateTime endTime = DateTime.Parse(this.txtEnd.Text);
        endTime = endTime.AddDays(1);

        DataSet ds = new DataSet();
        DataTable dt = SyncInfor.DataBind(code, startTime, endTime, true);
        DataTableToCsv("ERROR_LOG", dt);

    }
}
