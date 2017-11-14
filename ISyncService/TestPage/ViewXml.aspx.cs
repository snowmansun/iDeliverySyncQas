using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class ViewXml : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Response.Charset = "UTF-8";
            Response.ContentType = "text/xml";
            Response.ContentEncoding = Encoding.UTF8;
            string xml = Session["content"].ToString();            
            Response.Write(xml);
            Session.Remove("content");
        }
    }
}