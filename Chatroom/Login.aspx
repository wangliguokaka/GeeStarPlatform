<%@ Register TagPrefix="cc1" Namespace="book09" %>
<%@ Page language="c#" Inherits="book09.Migrated_Login" codePage="936" CodeFile="Login.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
 <HEAD>
  <title>µÇÂ¼Ò³Ãæ</title>
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
  <meta content="C#" name="CODE_LANGUAGE">
  <meta content="JavaScript" name="vs_defaultClientScript">
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
  <LINK href="global.css" type="text/css" rel="stylesheet">
 </HEAD>
 <body>
  <form id="Form1" method="post" runat="server">
   <table height="90%" cellSpacing="1" cellPadding="1" width="100%" border="0">
    <tr>
     <td height="100"><FONT face="ËÎÌå"></FONT></td>
    </tr>
    <tr>
     <td align="center">
      <cc1:LoginCustomControl id="LoginCustomControl1" runat="server" ShowRegister="False" onlogin="LoginCustomControl1_Login"></cc1:LoginCustomControl>
     </td>
    </tr>
    <tr>
     <td align="center">
      <asp:Label id="lblMessage" runat="server" ForeColor="Red"></asp:Label>
     </td>
    </tr>
   </table>
  </form>
  <a href="http://www.51aspx.com" target="_blank"></a>
 </body>
</HTML>
