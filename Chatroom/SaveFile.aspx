<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SaveFile.aspx.cs" Inherits="SaveFile" %>

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
                  <td width="51" align="left" valign="middle" bgcolor="#F5F5F5"><label onmouseover="this.style.cursor='hand'" onmouseout="this.style.cursor='default'" onclick="document.all('chatcontent').innerHTML = ''" style="FONT-SIZE:14px; COLOR: #ffcc00; TEXT-ALIGN: center"><span class="STYLE8"><a href="Login.aspx">聊天室</a></span></label></td>
                  <td width="53" align="center" valign="middle" bgcolor="#F5F5F5"><img src="image/tui.gif" width="34" height="35" /></td>
                  <td width="65" align="left" valign="middle" bgcolor="#F5F5F5"><label onMouseOver="this.style.cursor='hand'" onMouseOut="this.style.cursor='default'" onClick="if (confirm('您确定要退出该聊天室吗？')) Close();" style="FONT-SIZE:14px; COLOR: #ffcc00; TEXT-ALIGN: center"><span class="STYLE8">退出聊天</span></label>&nbsp;                   </td>
                  <td width="24" align="left" valign="middle" bgcolor="#F5F5F5"></td>
                </tr>
                <tr>
                  <td width="13" height="27">&nbsp;</td>
                  <td width="31">&nbsp;</td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
                </tr>
                <tr>
                  <td height="196">&nbsp;</td>
                  <td colspan="8" align="center" valign="middle">
                      <asp:DataGrid ID="Save_DataGrid" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                          CellPadding="4" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                          Font-Underline="False" ForeColor="#333333" GridLines="None" Height="184px" Width="384px" OnDeleteCommand="Save_DataGrid_DeleteCommand" OnPageIndexChanged="Save_DataGrid_PageIndexChanged" PageSize="6">
                          <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                          <EditItemStyle BackColor="#2461BF" />
                          <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                          <PagerStyle BackColor="WhiteSmoke" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                              Font-Strikeout="False" Font-Underline="False" ForeColor="Gray" HorizontalAlign="Right" NextPageText="下一页" PrevPageText="上一页" />
                          <AlternatingItemStyle BackColor="White" />
                          <ItemStyle BackColor="InactiveCaption" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                              Font-Strikeout="False" Font-Underline="False" />
                          <HeaderStyle BackColor="WhiteSmoke" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                              Font-Strikeout="False" Font-Underline="False" ForeColor="Gray" />
                          <Columns>
                              <asp:BoundColumn DataField="id" HeaderText="序号" Visible="False"></asp:BoundColumn>
                              <asp:BoundColumn DataField="Send_FileName" HeaderText="标题">
                                  <HeaderStyle Width="20%" />
                              </asp:BoundColumn>
                              <asp:BoundColumn DataField="Send_UserName" HeaderText="发送者">
                                  <HeaderStyle Width="20%" />
                              </asp:BoundColumn>
                              <asp:BoundColumn DataField="Send_Time" HeaderText="日期">
                                  <HeaderStyle Width="30%" />
                              </asp:BoundColumn>
                             <asp:TemplateColumn>
                     <HeaderTemplate>
                       下载
                     </HeaderTemplate>
                     <ItemTemplate>
                       <center><a href="<%# DataBinder.Eval(Container.DataItem, "Send_File") %>"><asp:Image ID="Image1" ImageUrl="image/down.gif" Alternatetext="下载" Runat="Server" /></a></center>
                     </ItemTemplate>
                     <HeaderStyle width="15%"></HeaderStyle>
                   </asp:TemplateColumn>
                              <asp:ButtonColumn CommandName="Delete" HeaderText="删除" Text="删除">
                                  <HeaderStyle Width="15%" />
                              </asp:ButtonColumn>
                          </Columns>
                      </asp:DataGrid>&nbsp;</td>
                  <td>&nbsp;</td>
                </tr>
                <tr>
                  <td height="44">&nbsp;</td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
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
