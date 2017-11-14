<%@ Page Language="C#" AutoEventWireup="true" Inherits="eBest.SyncServer.TestDownload"
    CodeFile="TestDownload.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>下载测试</title>
    <link href="../Image/ComminStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        function SelectValue() {
            var drValue = document.getElementById("<%=xml.ClientID%>");
            var index = drValue.selectedIndex;

            var temp = drValue.options[index].value.split('.');
            document.getElementById("txtTableList").value = temp[0];
        }

        function CheckValue() {
            var drValue = document.getElementById("<%=xml.ClientID%>");
            var index = drValue.selectedIndex;

            if (index > 0) {
                return true;
            }
            else {
                document.getElementById("lblContent").innerText = "Please Choice FileName!";
                return false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="style1">
                <tr>
                    <td class="style3">Address：
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="txtAddress" runat="server" Width="325px" Enabled="False"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style3">LoginName：
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="txtLoginName" runat="server" Width="325px">User001</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style3">Password：
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="txtPassword" runat="server" Width="325px">111111</asp:TextBox>
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
                    <td class="style3">Version：
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="txtVersion" Text="V2" runat="server" Width="325px"
                            ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>IsZip：
                    </td>
                    <td>
                        <asp:CheckBox ID="isCheck" runat="server" Checked="true" />
                    </td>
                </tr>
                <tr>
                    <td class="style3">ConfigFile：
                    </td>
                    <td class="style2">
                        <asp:DropDownList ID="xml" runat="server" Width="325px" AppendDataBoundItems="true"
                            Height="26px" onChange="SelectValue()">
                            <asp:ListItem Text="Please Choice FileName"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:LinkButton ID="content" runat="server" Text="View" OnClick="content_Click"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td class="style3">DownloadList：
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="txtTableList" runat="server" Columns="20" Rows="5" TextMode="MultiLine"
                            Width="325px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnDownload" runat="server" OnClientClick=" return CheckValue();" OnClick="Button1_Click" Text="Submit" />
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblContent" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>
