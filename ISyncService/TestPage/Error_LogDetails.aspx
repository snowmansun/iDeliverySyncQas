<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error_LogDetails.aspx.cs"
    Inherits="TestPage_SynclogDetails" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../Image/ComminStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Image/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        function check() {
            //var code=document.getElementById("txtCode").value;
            var start = new Date(document.getElementById("txtStart").value);
            var end = new Date(document.getElementById("txtEnd").value);
            //    if(code=="") alert("亲，当前用户账号不能为空！");
            //      else if(isNaN(code)) alert("亲，当前用户账号不是合法账户！");
            //    document.getElementById("txtCode").focus();        
            if (Date.parse(start) - Date.parse(end) > 0)
                alert("亲，当前起始时间大于截止时间！");

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="search" align="left">
        <table style="width: 691px">
            <tr>
                <td align="right" style="white-space: nowrap">
                    <b>用户账号： </b>
                </td>
                <td>
                    <asp:TextBox ID="txtCode" runat="server" Width="140px"></asp:TextBox>
                </td>
                <td align="right" style="white-space: nowrap">
                    <b>开始时间： </b>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtStart" CssClass="Wdate" onfocus="WdatePicker()"
                        Width="140px"></asp:TextBox>
                </td>
                <td align="right" style="white-space: nowrap">
                    <b>截止时间： </b>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtEnd" CssClass="Wdate" onfocus="WdatePicker()"
                        Width="140px"></asp:TextBox>
                </td>
                <td align="right" style="white-space: nowrap">
                    <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" OnClientClick="check()" />
                </td>
                <td align="right" style="white-space: nowrap">
                    <asp:Button ID="btnToExcel" runat="server" Text="导出CSV" OnClick="btnToExcel_Click"
                        OnClientClick="check()" />
                </td>
                <td>
                    <asp:Button ID="btnClear" runat="server" Text="清空" OnClick="btnClear_Click" />
                </td>
            </tr>
        </table>
    </div>
    <br />
    <div id="detail">
        <asp:Repeater ID="RpSyncInfor" runat="server">
            <HeaderTemplate>
                <table id="tab">
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td nowrap="nowrap">
                        <%# DataBinder.Eval(Container.DataItem, "ID")%>
                    </td>
                    <td nowrap="nowrap">
                        <%# DataBinder.Eval(Container.DataItem, "UserCode")%>
                    </td>
                    <td nowrap="nowrap">
                        <%# DataBinder.Eval(Container.DataItem, "Date")%>
                    </td>
                    <td nowrap="nowrap" style="width: 800px">
                        <%# DataBinder.Eval(Container.DataItem, "Message")%>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table></FooterTemplate>
        </asp:Repeater>
    </div>
    </form>
</body>
</html>
