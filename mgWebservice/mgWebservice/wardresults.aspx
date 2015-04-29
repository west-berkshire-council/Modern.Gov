<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="index.master" CodeBehind="wardresults.aspx.cs" Inherits="mgWebservice.wardresults" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageContentHolder" runat="server">
    <div class="row">
        <div class="col-md-9">
            <h1><%Response.Write(strTitle);%>: Results by Ward</h1>                  
    <%Response.Write(strContent);%>
             </div>
    </div>
</asp:Content>