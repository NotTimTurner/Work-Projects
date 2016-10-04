<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="checkInOrders.aspx.cs" Inherits="checkInOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>CheckIn Orders</h1>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <fieldset class="jumbotron">
                <center><legend class="alert-info" style="color: #239FE6; font-weight: 700; font-size:x-large" >Pick and Drop</legend></center>
                <center><div class="col-md-6 margin nopadding">
                    <div class="input-group">
                        <asp:TextBox ID="txtScanPickLoc" placeholder="Pick Product" CssClass="form-control input-lg largText text-capitalize" runat="server" AutoPostBack="True"></asp:TextBox>
                        <asp:LinkButton ID="btnPick" runat="server" CssClass="btn btn-primary btn-lg">Pick</asp:LinkButton>
                    </div>
                </div>
                <div class="col-md-6 margin nopadding">
                    <div class="input-group">
                        <asp:TextBox ID="txtScanDropLoc" placeholder="Drop Product" Width="80%" CssClass="form-control input-lg largText text-capitalize" runat="server" AutoPostBack="True"></asp:TextBox>
                        <asp:LinkButton ID="btnDrop" runat="server" CssClass="btn btn-primary btn-lg">Drop</asp:LinkButton>
                    </div>
                </div></center>
            </fieldset>
            </div>
            </fieldset>
            <fieldset>
                <div class="col-md-12 margin nopadding">
                    <div class=" ">
                        <legend class="bg-primary">List of Picked boxes
                   
                        </legend>
                    </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

