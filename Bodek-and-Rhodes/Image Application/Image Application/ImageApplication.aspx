<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImageApplication.aspx.cs" Inherits="Image_Application.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:FileUpload id="FileUpload1" runat="server" />
    <asp:Button runat="server" id="UploadButton" text="Upload" onclick="UploadButton_Click" />
    <br /><br />
    <asp:Label runat="server" id="lblMessage" text="Upload status: " />
    
    </div>
    </form>
</body>
</html>
