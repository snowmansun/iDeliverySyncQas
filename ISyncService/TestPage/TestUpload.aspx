<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" Inherits="eBest.SyncServer.TestDownload"
    CodeFile="TestUpload.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>下载测试</title>
    <link href="../Image/ComminStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function SelectValus() {
            var drValue = document.getElementById("xml").options[document.getElementById("xml").selectedIndex].text;
            var temp = drValue.split('.');
            //document.getElementById("txtTableList").value = temp[0] + ",0";
        }

        function InitiaUrl() {
            var path = window.location.href;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="style1">
                <tr>
                    <td>Address：
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server" Width="325px" Enabled="False"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>LoginName：
                    </td>
                    <td>
                        <asp:TextBox ID="txtLoginName" runat="server" Width="325px">Mr.User02</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Password：
                    </td>
                    <td>
                        <asp:TextBox ID="txtPassword" runat="server" Width="325px" TextMode="Password">11</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>DomainID：
                    </td>
                    <td>
                        <asp:TextBox ID="txtDomainID" runat="server" Width="325px">1</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Version：
                    </td>
                    <td>
                        <asp:TextBox ID="txtVersion" Text="V2" runat="server" Width="325px" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>IsZip：<asp:CheckBox ID="isCheck" runat="server" Checked="false" />
                        IsPhoto：<asp:CheckBox ID="IsPhoto" runat="server" Checked="false" />
                    </td>
                </tr>
                <tr>
                    <td class="style3">ConfigFile：
                    </td>
                    <td class="style2">
                        <asp:DropDownList ID="xml" runat="server" Width="325px" AppendDataBoundItems="true"
                            Height="26px" onChange="SelectValus()">
                            <asp:ListItem Text="Please Choice FileName"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:LinkButton ID="content" runat="server" Text="View" OnClick="content_Click"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>UploadData：
                    </td>
                    <td>
                        <asp:TextBox ID="txtTableList" runat="server" Columns="20" Rows="5" TextMode="MultiLine"
                            Width="325px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnDownload" runat="server" OnClick="Button1_Click" Text="Submit" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lblContent" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>
