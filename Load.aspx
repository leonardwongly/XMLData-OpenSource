<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Load.aspx.cs" Inherits="XMLData.Load" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row" style="margin-top: 30px">
        <div class="col-sm">
            <h3>Load by MMSI</h3>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-sm">
            <div class="form-group">
                <label for="tbMMSI" style="font-weight: 400">Enter Vessel's MMSI</label>
                <asp:TextBox ID="tbMMSI" runat="server" CssClass="form-control" ClientIDMode="AutoID" Style="width: 30%"></asp:TextBox>
                <br />
                <label for="tbCount" style="font-weight: 400">Number of items to load</label>
                <asp:RadioButtonList ID="rblNumeric" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblNumeric_SelectedIndexChanged">
                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                    <asp:ListItem Text="Custom" Value="Custom"></asp:ListItem>
                </asp:RadioButtonList>
                <small id="txtToolTip" class="form-text text-muted">Note: The larger the number of items, the longer it will take to load.</small>
                <br />
                <asp:Label ID="lblTotalItems" runat="server"></asp:Label>
                <br />
                <asp:TextBox ID="tbCount" runat="server" CssClass="form-control" ClientIDMode="AutoID" Visible="false" Style="width: 30%"></asp:TextBox>
                <br />
                <asp:Button ID="btnCount" runat="server" CssClass="btn btn-primary" Text="Retrieve" OnClick="btnCount_Click" />
                <br />
                <asp:Button ID="btnExportExcel" runat="server" Text="Export to Excel" OnClick="btnExportExcel_Click" Visible="false"/> 


            </div>
            <br />


            <div class="row" style="margin-top: 30px;">
                <div class="col-sm">
                    <asp:Table ID="tblAIS" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" CellSpacing="3" GridLines="Both" class="table table-striped table-bordered table-dark" Style="width: 100%; text-align: center;">
                    </asp:Table>

                </div>
            </div>
        </div>
    </div>


</asp:Content>
