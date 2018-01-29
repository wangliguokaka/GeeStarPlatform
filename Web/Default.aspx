<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>请不要关闭此页面，微信通道专用!!!</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="color: #FF0000">
      
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
      
    <asp:Timer ID="Timer1" runat="server" ontick="Timer1_Tick" Interval="6000">
    </asp:Timer>
     请不要关闭此页面，微信通道专用!!! 
    </div>  
    
    </form>

</body>
</html>

