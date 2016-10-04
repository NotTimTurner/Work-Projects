<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <div class="container-fluid">

            <div class="jumbotron" style="border: 1px groove #1027b5; box-shadow: 0px 2px 5px #ccc;">
                <h1>Welcome to WMS Philadelphia</h1>
                
                    <div class="col-sm-4">
                        <h2></h2>
                    </div>

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="col-sm-4">
                                <h2>
                                    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label></h2>
                            </div>

                            <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick">
                            </asp:Timer>
                            <div class="col-sm-4">
                                <h2></h2>
                            </div>
                        </ContentTemplate>

                    </asp:UpdatePanel>
                </div>
               
            </div>
        </div>

        
   
</asp:Content>
