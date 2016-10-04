<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Piece Picking.aspx.cs" Inherits="Piece_Picking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <script type="text/javascript">
        function LocationClear() {
            alert("HI");
           document.getElementById("LocationTextBox").value = "";
          

        }
    </script>
    <script>
       
       
        Sys.Application.add_load(Help);
        function Help() {
            $('.glyphicon-question-sign').tooltip({ title: "<table class='table table-striped table-hover '><thead><tr><th>WHAT EACH COLOR MEANS</tr></thead><tr class='info'><td><font color='black'>Line Compleated</font></td></tr></tr></thead><tr class='success'><td><font color='black'>Line needs to be picked<font></td></tr></tr></thead><tr class='warning'><td><font color='black'>Replen going here but ok to pick<font></td></tr></thead><tr class='danger'><td><font color='black'>Replean heading here, can not be picked<font></td></tr></tr></table>", html: true, trigger: "hover", placement: "bottom" });
        }

    </script>

    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnRandom">
        <div class="row">
            <div class="col-sm-3"></div>
            <div class="col-sm-6">
                <div id="LocationGroup" class="form-group" runat="server">
                    <label id="StartLocationLabel" class="control-label" for="focusedInput">Please Enter Starting Location</label>
                    <div class="input-group-lg">
                        <asp:TextBox ID="StartLocationBox" CssClass="form-control" runat="server" placeholder="Starting Location" Width="200px"></asp:TextBox>
                        <asp:LinkButton ID="btnRandom"
                            runat="server"
                            CssClass="btn btn-primary btn-lg"
                            OnClick="btnRandom_Click">Enter</asp:LinkButton>
                    </div>
                </div>
                <div class="col-sm-3"></div>
            </div>
        </div>

    </asp:Panel>

    <asp:Panel ID="Panel2" runat="server" Visible="False" DefaultButton="LinkButton2">
        <div class="panel panel-success">
            <div class="panel-body">
                <div class="row">
                    <div class="form-group">
                        <div class="col-sm-4">
                            <label class="control-label" for="disabledInput">Current Zone:</label>
                            <asp:TextBox ID="CurrentLocationTextBox" runat="server" CssClass="form-control" ReadOnly="true" placeholder="Starting Location" ViewStateMode="Inherit" Width="40px"></asp:TextBox>
                        </div>

                        <div class="col-sm-4">
                            <div id="BoxScanGroup" class="form-group" runat="server">
                                <label class="control-label" for="disabledInput">Current Box:</label>
                                <div class="input-group">
                                    <asp:TextBox ID="BoxScannTextBox" runat="server" CssClass="form-control" placeholder="Please Scan The Box" Wrap="False" Width="175px"></asp:TextBox>
                                    <span>
                                        <asp:LinkButton ID="LinkButton2"
                                            runat="server"
                                            CssClass="btn btn-primary" OnClick="LinkButton2_Click">Validate</asp:LinkButton>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">

                            <asp:CheckBox ID="CheckBox1" CssClass="checkbox " runat="server" Checked="true" OnCheckedChanged="CheckBox1_CheckedChanged" />
                            <label class="control-label">Current Picks</label>
                        </div>


                        <div class="container">
                            <span class="glyphicon glyphicon-question-sign"></span>
                        </div>


                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>




    <asp:UpdatePanel ID="CartonUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <asp:Panel ID="CartonPanel" runat="server" Visible="False">
                <div class="row">
                    <asp:Timer ID="CartonTimer" runat="server" Interval="1000" OnTick="CartonTimer_Tick"></asp:Timer>
                    <div class="table-responsive">
                        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataSourceID="PickTicketDataSource" CssClass="table table-condensed table-responsive table-bordered" OnRowDataBound="GridView1_RowDataBound" AlternatingRowStyle-Wrap="False">
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>
                                <asp:BoundField DataField="ORDER_LINE" HeaderText="ORDER LINE" SortExpression="ORDER_LINE" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                <asp:BoundField DataField="From_Location" HeaderText="LOCATION" SortExpression="From_Location" />
                                <asp:BoundField DataField="SHORT_DESCRIPTION" HeaderText="ITEM" SortExpression="SHORT_DESCRIPTION" />
                                <asp:BoundField DataField="DESCRIPTION" HeaderText="DESCRIPTION" SortExpression="DESCRIPTION" />
                                <asp:BoundField DataField="Quantity" HeaderText="PCS" SortExpression="Quantity" />
                            </Columns>
                        </asp:GridView>

                    </div>
                    <asp:SqlDataSource ID="PickTicketDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:WMSConnectionString2 %>" ></asp:SqlDataSource>
                </div>

            </asp:Panel>
            </div>
           
        </ContentTemplate>
    </asp:UpdatePanel>
    
     
  <asp:UpdatePanel ID="ToGoUpdatePannel" runat="server" UpdateMode="Conditional">
      <ContentTemplate>
          <asp:Timer ID="ToGoTimer" runat="server" Interval="1000" OnTick="ToGoTimer_Tick" />
          <div class="col-sm-2"></div>
          <div class="col-sm-8">
           <h1> <asp:Label CssClass="row"  runat="server" ID="ToDoLabel" Text="THIS LABEL DOES THINGS" Visible="false" ></asp:Label></h1>  
              </div>
          <div class="col-sm-2"></div>
      </ContentTemplate>
  </asp:UpdatePanel>            
      
                    
               
           

    




    <asp:UpdatePanel ID="PickingUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div style="padding-left: 15px"/>
                <asp:Panel ID="LocationPanel" runat="server" DefaultButton="LocationButton" Visible="false">
                    <div class="col-sm-4">
                       <div class="form-group" id="LocationInputGroup" runat="server">
                        <label id="LocationCheckLabel" class="control-label" for="focusedInput">Location</label>
                        <div class="input-group">
                            <asp:TextBox ID="LocationTextBox" CssClass="form-control" runat="server" placeholder="Scan Location" Width="150px">
                                <ClientEvents OnFocus="LocationClear" />
                            </asp:TextBox>
                            <asp:LinkButton ID="LocationButton"
                                runat="server"
                                CssClass="btn btn-primary" Width="60px" OnClick="LocationButton_Click">Enter</asp:LinkButton>
                            </div>
                        </div>
                    </div>

                </asp:Panel>
                 <asp:Panel ID="ProductPanel" runat="server" DefaultButton="ProductCheckButton" Visible="false">
                    <div class="col-sm-4">
                        <div class="form-group" id="ProductInputGroup" runat="server">
                        <label id="ProductCheckLabel" class="control-label" for="focusedInput">Product</label>
                        <div class="input-group" >
                            <asp:TextBox ID="ProductTextBox" CssClass="form-control" runat="server" placeholder="Scan Product Box" Width="150px"></asp:TextBox>
                            <asp:LinkButton ID="ProductCheckButton"
                                runat="server"
                                CssClass="btn btn-primary" OnClick="ProductCheckButton_Click" Width="60px">Enter</asp:LinkButton>
                        </div>
                    </div>

                </asp:Panel>
                <asp:Panel ID="QuantityPanel" runat="server" DefaultButton="QuantityButton" Visible="false">
                    <div class="col-sm-4">
                        <div class="form-group" id="QuantityInputGroup" runat="server">
                        <label id="QuantityCheckLabel" class="control-label" for="focusedInput">Quantity</label>
                        <div class="input-group">
                            <asp:TextBox ID="QuantityTextBox" CssClass="form-control" runat="server" placeholder="Enter Quantity" Width="150px"></asp:TextBox>
                            <asp:LinkButton ID="QuantityButton"
                                runat="server"
                                CssClass="btn btn-primary" Width="60px" OnClick="QuantityButton_Click">Enter</asp:LinkButton>
                        </div>
                            </div>
                </asp:Panel>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

