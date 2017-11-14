<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestLogin.aspx.cs" Inherits="TestPage_TestLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Image/ComminStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Image/jquery-easyui-1.2.3/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Image/jquery-easyui-1.2.3/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../Image/jquery-easyui-1.2.3/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../Image/jquery-easyui-1.2.3/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../Image/json/c.js"></script>
    <link href="../Image/json/s.css" rel="stylesheet" />
    <script type="text/javascript">
        $(document).ready(function () {

            $("[name = isChange]:checkbox").attr("checked", false);
            $("#NewPwd").hide();
            $("#ConfirmPwd").hide();

            $("#isChange").click(function () {

                var isChangePwd = $("#isChange").attr("checked");
                if (isChangePwd) {
                    $("#NewPwd").show()
                    $("#ConfirmPwd").show();
                }
                else {
                    $("#NewPwd").hide();
                    $("#ConfirmPwd").hide();
                }

                $("#txtNewPassword").val("");
                $("#txtConfirmPwd").val("");
            })
        })

        function IsCheckPwd() {

            var newPwd = $("#txtNewPassword").val();
            var newConfirmPwd = $("#txtConfirmPwd").val();

            if (newPwd == newConfirmPwd) {
                return true;
            }
            else {
                $("#lblContent").html("Two password is not consistent");
                return false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
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
                        <asp:TextBox ID="txtLoginName" runat="server" Width="325px">Mr.User01</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Password：
                    </td>
                    <td>
                        <asp:TextBox ID="txtPassword" runat="server" Width="325px" TextMode="Password">02</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>ChangePwd：<input type="checkbox" id="isChange" name="isChange" runat="server" />
                    </td>
                </tr>
                <tr id="NewPwd">
                    <td>NewPwd：
                    </td>
                    <td>
                        <asp:TextBox ID="txtNewPassword" runat="server" Width="325px" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr id="ConfirmPwd">
                    <td>ConfirmPwd：
                    </td>
                    <td>
                        <asp:TextBox ID="txtConfirmPwd" runat="server" Width="325px" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>DomainCode：
                    </td>
                    <td>
                        <asp:TextBox ID="txtDomainCode" runat="server" Width="325px" Enabled="false">eBest</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>PlatForm：
                    </td>
                    <td>
                        <asp:TextBox ID="txtPlatForm" Text="Android" runat="server" Width="325px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Version No.</td>
                    <td>
                        <asp:TextBox ID="txtVersion" runat="server" Width="325px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnDownload" runat="server" Text="Submit" OnClientClick="return IsCheckPwd();" OnClick="btnDownload_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lblContent" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>
