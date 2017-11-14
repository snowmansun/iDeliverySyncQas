<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="TestPage_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../Image/ComminStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Image/jquery-easyui-1.2.3/themes/default/easyui.css" rel="stylesheet"
        type="text/css" />
    <link href="../Image/jquery-easyui-1.2.3/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../Image/jquery-easyui-1.2.3/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../Image/jquery-easyui-1.2.3/jquery.easyui.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <img src="../Image/eBest.png" /><br />
        <h3>
            FrameWorkFunction</h3>
        <p style="color: #FF0000; font-weight: bold">
            LogIn：</p>
        <p>
            主要模拟测试手机端登陆认证访问是否正常；
        </p>
        <p style="color: #FF0000; font-weight: bold">
            Download：</p>
        <p>
            主要模拟测试手机端请求下载服务器端数据；</p>
        <p style="color: #FF0000; font-weight: bold">
            Upload：</p>
        <p>
            主要模拟测试手机端请求上传数据至服务器端；</p>
        <p style="color: #FF0000; font-weight: bold">
            LogQuery：
        </p>
        <p>
            主要提供同步实时日志查询服务，以便于排查问题原因；</p>
        <p style="color: #FF0000; font-weight: bold">
            ConfigFiles：
        </p>
        <p>
            主要提供配置文件详细信息给手机端开发人员，以便其与服务端进行字段核对；</p>
    </div>
    </form>
</body>
</html>
