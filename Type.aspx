<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Type.aspx.cs" Inherits="XMLData.Type" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row" style="margin-top: 30px">
        <div class="col-sm">
            <h3>Load by Type</h3>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-sm">
            <div class="form-group">
                <label for="ddlTypeList" style="font-weight: 400">Choose Type</label>
                <asp:DropDownList ID="ddlTypeList" runat="server" CssClass="form-control form-control-sm" Style="width:40%">
                    <asp:ListItem Selected="True" Text="Choose Vessel Type"></asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Button ID="btnCount" runat="server" CssClass="btn btn-primary" Text="Load" OnClick="btnCount_Click" />
                <br />
                <asp:Label ID="lblTotalItems" runat="server"></asp:Label>
                <br />
                <label for="tbCount" style="font-weight: 400; margin-top: 30px">Number of items to load</label>
                <asp:RadioButtonList ID="rblNumeric" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblNumeric_SelectedIndexChanged">
                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                    <asp:ListItem Text="Custom" Value="Custom"></asp:ListItem>
                </asp:RadioButtonList>
                <br />
                <asp:TextBox ID="tbCount" runat="server" CssClass="form-control" ClientIDMode="AutoID" Visible="false" Style="width: 30%"></asp:TextBox>
                <br />
                <small id="txtToolTip" class="form-text text-muted">Note: The larger the number of items, the longer it will take to load.</small>
                <br />
                <asp:Button ID="btnProceed" runat="server" CssClass="btn btn-primary" Text="Proceed" OnClick="btnProceed_Click" Visible="false" />
                <br />


            </div>
        </div>
        <br />

        <div class="container">
            <div class="row" style="margin-top: 30px;">
                <div class="col-sm">
                    <asp:Table ID="tblAISType" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" CellSpacing="3" GridLines="Both" class="table table-striped table-bordered table-dark" Style="width: 80%; text-align: center;">
                    </asp:Table>

                </div>
            </div>
        </div>
    </div>



</asp:Content>
