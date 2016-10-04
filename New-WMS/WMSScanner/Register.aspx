<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Account_Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2>Create A New Account</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

   
        
        <hr />
    <asp:UpdatePanel runat="server" ID="RegisterUpdatePannel">
        <ContentTemplate>

       
        <asp:ValidationSummary runat="server" CssClass="text-danger" />
        <div class="form-group">
            <div class="row">
           <h5> <asp:Label runat="server" AssociatedControlID="UserName" CssClass="col-sm-2 control-label">First Name</asp:Label></h5>
            <div class="col-sm-10">
                <asp:TextBox runat="server" ID="FirstName" CssClass="form-control" OnTextChanged="FirstName_TextChanged" AutoPostBack="true" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="FirstName"
                    CssClass="text-danger" ErrorMessage="You Must Enter Your First Name." />
            </div>
                </div>
        </div>

        <div class="form-group">
            <div class="row">
            <h5><asp:Label runat="server" AssociatedControlID="UserName" CssClass="col-sm-2 control-label">Last Name</asp:Label></h5>
            <div class="col-sm-10">
                <asp:TextBox runat="server" ID="LastName" CssClass="form-control" AutoPostBack="true" OnTextChanged="LastName_TextChanged" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="LastName"
                    CssClass="text-danger" ErrorMessage="You Must Enter Your Last Name." />
            </div>
                </div>
        </div>

        
        <div class="form-group">
            <div class="row">
           <h5> <asp:Label runat="server" AssociatedControlID="UserName" CssClass="col-sm-2 control-label">User name</asp:Label></h5>
            <div class="col-sm-10">
                <asp:TextBox runat="server" ID="UserName" CssClass="form-control" ReadOnly="True" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                    CssClass="text-danger" ErrorMessage="The user name field is required." />
            </div>
                </div>
        </div>
        <div class="form-group">
            <div class="row">
            <h5><asp:Label runat="server" AssociatedControlID="Password"  CssClass="col-sm-2 control-label">Password</asp:Label></h5>
            <div class="col-sm-10">
                <asp:TextBox runat="server" ID="Password"  TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                    CssClass="text-danger" ErrorMessage="The password field is required." />
            </div>
                </div>
        </div>
        <div class="form-group">
            <div class="row">
           <h5> <asp:Label runat="server" AssociatedControlID="ConfirmPassword" CssClass="col-sm-2 control-label">Confirm password</asp:Label></h5>
            <div class="col-sm-10">
                <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The confirm password field is required." />
                <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The password and confirmation password do not match." />
            </div>
                </div>
        </div>
        <div class="form-group">
            <div class="row">
            <h4><asp:Label runat="server" ID="Gender" CssClass="col-sm-2 control-label" Visible="false"  Enabled="false">Gender</asp:Label></h4>
            <div class="col-sm-10">
                <asp:DropDownList runat="server" ID="GenderDropDown" CssClass="form-control" Visible="false"  Enabled="false">
                    <asp:ListItem Text="" Value="" Selected="true"/>
                    <asp:ListItem Text="Male" Value="Male" />
                    <asp:ListItem Text="Female" Value="Female" />
                </asp:DropDownList>
            </div>
                </div>
        </div>
    

       
    <div class="row">
        
           
           <h4> <asp:Label runat="server" ID="Label1" CssClass="col-sm-2" Visible="false"  Enabled="false">Birthday</asp:Label></h4>
            
               
            
                <asp:DropDownList runat="server" ID="MonthDropDown" Visible="false"  Enabled="false" CssClass="form-control col-sm-2" Width="120px" >
                   <asp:ListItem Text="Month" Selected="True"/>
                </asp:DropDownList>
            
                <asp:DropDownList runat="server" ID="DayDropDown"  Visible="false" Enabled="false" CssClass="form-control col-sm-2" Width="120px"  >
                   <asp:ListItem Text="Day" Selected="True"/>
                </asp:DropDownList>

         <asp:DropDownList runat="server" ID="YearDropDown"  Enabled="false" Visible="false" CssClass="form-control col-sm-2" Width="120px"  >
                   <asp:ListItem Text="Year" Selected="True"/>
                </asp:DropDownList>
          
        </div>
            
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                <asp:Button ID="Button1" runat="server" OnClick="CreateUser_Click" Text="Register" CssClass="btn btn-default" />
            </div>
        </div>
      </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

