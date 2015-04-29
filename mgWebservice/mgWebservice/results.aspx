<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="index.master" CodeBehind="results.aspx.cs" Inherits="mgWebservice.results" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageContentHolder" runat="server">
    <div class="row">
        <div class="col-md-9">
           <h1><%Response.Write(strTitle);%>: Overall Results</h1>             
    <%Response.Write(strContent);%>
             </div>
    </div>
</asp:Content>
