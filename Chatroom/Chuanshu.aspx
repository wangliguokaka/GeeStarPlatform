<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Chuanshu.aspx.cs" Inherits="Chuanshu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title>聊天室</title>
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
  <meta content="C#" name="CODE_LANGUAGE">
  <meta content="JavaScript" name="vs_defaultClientScript">
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
  <LINK href="global.css" type="text/css" rel="stylesheet">
<style type="text/css">
<!--
@import url("css/home_ge.css");
@import url("css/home_ly.css");
body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
	background-image: url(images/bg.jpg);
}
a:link {
	text-decoration: none;
}
a:visited {
	text-decoration: none;
	color: #333333;
}
a:hover {
	text-decoration: underline;
	color: #FF0000;
}
a:active {
	text-decoration: none;
}
.STYLE8 {
	font-size: 12px;
	color: #333333;
}
.STYLE9 {font-size: 12px}
-->
</style>
</head>
<body bottomMargin="0"  leftMargin="0" topMargin="0" rightMargin="0">
  <form id="Form1" method="post" runat="server">
<table width="772" height="572" border="0" align="center" cellspacing="0" bgcolor="#FFFFFF">
  <tr>
    <td height="19" valign="top"></td>
    <td width="606" rowspan="2" valign="top"><table height="410" border="0" cellspacing="0" class="f2">
      <tr>
        <td height="408"><table width="100%" border="0" cellpadding="0" cellspacing="0">
          <!--DWLayoutTable-->
          <tbody>
            <tr>
              <td height="65" colspan="3" valign="top"><img src="image/top.bmp" width="576" height="63" /></td>
            </tr>
            <tr>
              <td width="441" rowspan="2" valign="top"><table width="100%" border="0" cellpadding="0" cellspacing="0" class="f2">
                <!--DWLayoutTable-->
                <tr>
                  <td height="46" colspan="2" align="center" valign="middle" bgcolor="#F5F5F5"><a href="Chuanshu.aspx"><img src="image/N_agency.gif" width="34" height="30" border="0" /></a></td>
                  <td width="45" align="left" valign="middle" bgcolor="#F5F5F5"><a href="Chuanshu.aspx">上传</a></td>
                  <td width="58" align="center" valign="middle" bgcolor="#F5F5F5"><a href="SaveFile.aspx"><img src="image/N_download.GIF" width="39" height="30" border="0" /></a></td>
                  <td width="43" align="left" valign="middle" bgcolor="#F5F5F5"><a href="SaveFile.aspx">下载</a></td>
                  <td width="56" align="center" valign="middle" bgcolor="#F5F5F5"><img src="image/feedback.GIF" width="30" height="22" /></td>
                  <td width="51" align="left" valign="middle" bgcolor="#F5F5F5"><a href="Login.aspx">聊天室 </a></td>
                  <td width="53" align="center" valign="middle" bgcolor="#F5F5F5"><img src="image/tui.gif" width="34" height="35" /></td>
                  <td colspan="2" align="left" valign="middle" bgcolor="#F5F5F5"><label onMouseOver="this.style.cursor='hand'" onMouseOut="this.style.cursor='default'" onClick="if (confirm('您确定要退出该聊天室吗？')) Close();" style="FONT-SIZE:14px; COLOR: #ffcc00; TEXT-ALIGN: center"><span class="STYLE8">退出聊天</span></label>&nbsp;                   </td>
                  <td width="24" align="left" valign="middle" bgcolor="#F5F5F5"></td>
                </tr>
                <tr>
                  <td height="37" colspan="9">&nbsp;</td>
                  <td width="12">&nbsp;</td>
                  <td>&nbsp;</td>
                </tr>
                <tr>
                  <td width="38" style="height: 50px">&nbsp;</td>
                  <td colspan="9" align="left" valign="middle" style="height: 50px">文件名称：<asp:TextBox ID="FileTitle" runat="server" Width="144px"></asp:TextBox></td>
                  <td style="height: 50px">&nbsp;</td>
                </tr>
                <tr>
                  <td style="height: 20px"></td>
                  <td colspan="9" valign="top" style="height: 20px">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;
                      <asp:FileUpload ID="FileUpload1" runat="server" Width="216px" /></td>
                  <td style="height: 20px"></td>
                </tr>
                <tr>
                  <td></td>
                  <td colspan="9" align="left" valign="middle">
                      &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;
                  </td>
                  <td></td>
                </tr>
                <tr>
                  <td style="height: 19px"></td>
                  <td colspan="9" valign="top" style="height: 19px"><!--DWLayoutEmptyCell-->&nbsp;</td>
                  <td style="height: 19px"></td>
                </tr>
                <tr>
                  <td height="32"></td>
                  <td colspan="9" align="left" valign="middle">
                      &nbsp;&nbsp; &nbsp; 接受者：<asp:TextBox ID="Accept_User" runat="server" Width="136px"></asp:TextBox>
                      </td>
                  <td></td>
                </tr>
                <tr>
                  <td height="17"></td>
                  <td colspan="9" valign="top"><!--DWLayoutEmptyCell-->&nbsp;</td>
                  <td></td>
                </tr>
                <tr>
                  <td height="37"></td>
                  <td colspan="9" align="center" valign="middle" style="text-align: left">
                      &nbsp; &nbsp; &nbsp;发送者：<asp:TextBox ID="Send_User" runat="server" Width="136px"></asp:TextBox></td>
                  <td></td>
                </tr>
                <tr>
                  <td height="47"></td>
                  <td colspan="9" style="text-align: center">&nbsp;<asp:Button ID="SaveBtn" runat="server" OnClick="SaveBtn_Click" Text="确定" />
                      &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                      <asp:Button ID="C_Set" runat="server" OnClick="C_Set_Click" Text="重置" /></td>
                  <td></td>
                </tr>
               
                
                

                
              </table>              </td>
              <td width="13" height="312">&nbsp;</td>
              <td width="122" valign="top"></td>
            </tr>
        
          </tbody>
        </table></td>
      </tr>
    </table></td>
  </tr>
  <tr>
    <td width="162" height="539" valign="top">&nbsp;</td>
  </tr>
</table>
  </form>
</body>
</html>
