<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SyncLog.aspx.cs" Inherits="TestPage_SyncLog" %>

<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Image/ComminStyle.css" rel="stylesheet" type="text/css" />
</head>
<body bgcolor="#cccccc">
    <form id="form1" runat="server">
    <div style="width: 100%;">
        <asp:Repeater ID="RpSyncInfor" runat="server">
            <HeaderTemplate>
                <table id="tab" border="0">
<%--                    <tr>
                        <td>
                            <strong>ID</strong>
                            <td>
                                <strong>UserCode</strong>
                            </td>
                            <td>
                                <strong>Date</strong>
                            </td>
                            <td>
                                <strong>Message</strong>
                            </td>
                    </tr>--%>
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
                    <td nowrap="nowrap" style="width:800px">
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
