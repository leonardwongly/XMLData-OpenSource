<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="XMLData._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-sm" style="padding-top: 20px">
                <br />
                <asp:Button ID="btnProcess" runat="server" OnClick="btnProcess_Click" Text="Process" CssClass="btn btn-primary" />
                <br />
                <asp:Label ID="lblStream" runat="server" Visible="true"></asp:Label>
            </div>
        </div>
    </div>

</asp:Content>
