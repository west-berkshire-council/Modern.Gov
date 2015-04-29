<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="infobar.aspx.cs" Inherits="mgWebservice.infobar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="assets/css/style.css" rel="stylesheet" />
    <link href="results.css" rel="stylesheet" />
        <link href='http://fonts.googleapis.com/css?family=Noto+Sans' rel='stylesheet' type='text/css'>
    <link href='http://fonts.googleapis.com/css?family=Lato:400,700,900' rel='stylesheet' type='text/css'>
</head>
<body style="padding:0;">
    <form id="form1" runat="server">
    <div>
     <%Response.Write(strContent);%>
    </div>
    </form>
</body>
</html>
