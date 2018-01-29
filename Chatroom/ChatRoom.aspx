<%@ Page language="C#" Inherits="book09.ChatRoom" codePage="936" CodeFile="ChatRoom.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
 <title>������</title>
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
  <meta content="C#" name="CODE_LANGUAGE">
  <meta content="JavaScript" name="vs_defaultClientScript">
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
  <LINK href="global.css" type="text/css" rel="stylesheet">
  <script language="javascript">
  //������Ϣ
  function send()
  {
   var txtContent = document.all("content").value; //�ı�����������
   if (txtContent == "") return;
   
   var user_to = document.all("userlist").value;  //�������
   var textcolor = document.all("textcolor").value;  //��ɫ
   var expression = document.all("expression").value;  //����
   var isPublic = !(document.all("isSecret").checked);  //�Ƿ���̸    
   
   //���÷������˷���������Ϣ
   ChatRoom.SendMsg(txtContent, user_to, textcolor, expression, isPublic);
   
   //��������������ʾ
   var div = document.all("chatcontent");
   div.innerHTML = ChatRoom.GetNewMsgString().value + div.innerHTML;
   
   //��������
   document.all("content").value = "";
  }
  
  //��ʱ������������
  function refresh_chatcontent()
  {
   //���÷�����������ȡ������Ϣ��HTML�ַ���
   var div = document.all("chatcontent");
   var strNewMsg = ChatRoom.GetNewMsgString().value;
   
   //�ж��Ƿ�Ϊ�գ����ⲻ��Ҫ�ĸ���
   if (strNewMsg != "")
    div.innerHTML = strNewMsg + div.innerHTML;
    
   //��ʱ����
   window.setTimeout(refresh_chatcontent, 1000);
  }
  
  //�����û��б����������б�
  function refresh_onlineusers()
  {
   //���Ͷ����б�
   var userlist = document.all("userlist");
   
   //���÷������˷�����ȡ�û��б��ַ������ö��ŷָ���
   var strUserlist = ChatRoom.GetOnlineUserString().value;
   
   //��ȡ�ͻ�����ʾ���û��б��ַ���
   var strUserlistClient = "";
   for (var i = 1;i < userlist.options.length;i++)
   {
    if (i != userlist.options.length - 1)
    {
     strUserlistClient += userlist.options[i].value + ",";
    }
    else
    {
     strUserlistClient += userlist.options[i].value;
    }
   }
   
   if (strUserlistClient != strUserlist)  //�����û��б����仯
   {
    var userArr = strUserlist.split(',');
    
    //�����û���
    var usercount = document.all("usercount");
    usercount.innerHTML = "������������" + userArr.length + "�ˣ�";
    
    //����û��б�
    var tableHTML = "<table>";
    for (var i = 0;i < userArr.length;i++)
    {
     tableHTML += "<tr><td><label onmouseover=\"this.style.cursor='hand'\" onmouseout=\"this.style.cursor='default'\" onclick=\"setObj('" + userArr[i] + "')\">" + userArr[i] + "</label></td></tr>";
    }
    tableHTML += "</table>";
    var div = document.all("onlineusers");
    div.innerHTML = tableHTML;
    
    
    //��ʼ��
    while (userlist.options.length > 0)
    {
     userlist.removeChild(userlist.options[0]);  //�������ѡ��
    }
    
    //���ӡ����е��ˡ�ѡ��
    var oOption = document.createElement("OPTION");
    oOption.text = "���е���";
    oOption.value = "���";
    userlist.add(oOption);
    
    //�����б������������û���ѡ��
    for (var i = 0;i < userArr.length;i++)
    {
     var oOption = document.createElement("OPTION");
     oOption.text = userArr[i];
     oOption.value = userArr[i];
     userlist.add(oOption);
    }     
   }   
   
   //ÿ��1�����
   window.setTimeout(refresh_onlineusers, 1000);
  }
  
  //�˳�������
  function logout()
  {
   ChatRoom.Logout();
  } 
  
  //�����������
  function setObj(str)
  {
   var userlist = document.all("userlist");
   for (var i = 0;i < userlist.options.length;i++)
   {
    if (str == userlist.options[i].value)
    {
     userlist.selectedIndex = i;
     break;      
    }
   }
  }
  
  //�ر����������
  function Close()
  { 
   var ua = navigator.userAgent;
   var ie = navigator.appName == "Microsoft Internet Explorer" ? true:false;
   if (ie) 
   { 
    var IEversion=parseFloat(ua.substring(ua.indexOf("MSIE ")+5,ua.indexOf(";",ua.indexOf("MSIE "))));
    if (IEversion< 5.5) 
    { 
      var str  = '<object id=noTipClose classid="clsid:ADB880A6-D8FF-11CF-9377-00AA003B7A11">'; 
      str += '<param name="Command" value="Close"></object>'; 
      document.body.insertAdjacentHTML("beforeEnd", str); 
      document.all.noTipClose.Click();
    } 
    else 
    { 
     window.opener = null; 
     window.close(); 
    }
   } 
   else 
   { 
    window.close();
   } 
  }

  </script>
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
<body bottomMargin="0" onbeforeunload="logout()" leftMargin="0" topMargin="0" rightMargin="0">
  <form id="Form1" method="post" runat="server">
<table width="772" height="572" border="0" align="center" cellspacing="0" bgcolor="#FFFFFF">
  <tr>
    <td height="19" valign="top"></td>
    <td width="606" rowspan="2" valign="top"><table height="490" border="0" cellspacing="0" class="f2">
      <tr>
        <td height="488"><table width="100%" border="0" cellpadding="0" cellspacing="0">
          <!--DWLayoutTable-->
          <tbody>
            <tr>
              <td height="65" colspan="3" valign="top"><img src="image/top.bmp" width="576" height="63" /></td>
            </tr>
            <tr>
              <td width="441" height="429" valign="top"><table width="100%" border="0" cellpadding="0" cellspacing="0" class="f2">
                <!--DWLayoutTable-->
                <tr>
                  <td width="49" height="46" align="center" valign="middle" bgcolor="#F5F5F5"><a href="Chuanshu.aspx"><img src="image/N_agency.gif" width="34" height="30" border="0" /></a></td>
                  <td width="45" align="left" valign="middle" bgcolor="#F5F5F5"><a href="Chuanshu.aspx">�ϴ�</a></td>
                  <td width="58" align="center" valign="middle" bgcolor="#F5F5F5"><a href="SaveFile.aspx"><img src="image/N_download.GIF" width="39" height="30" border="0" /></a></td>
                  <td width="43" align="left" valign="middle" bgcolor="#F5F5F5"><a href="SaveFile.aspx">����</a></td>
                  <td width="56" align="center" valign="middle" bgcolor="#F5F5F5"><img src="image/feedback.GIF" width="30" height="22" /></td>
                  <td width="51" align="left" valign="middle" bgcolor="#F5F5F5"><label onMouseOver="this.style.cursor='hand'" onMouseOut="this.style.cursor='default'" onClick="document.all('chatcontent').innerHTML = ''" style="FONT-SIZE:14px; COLOR: #ffcc00; TEXT-ALIGN: center"><span class="STYLE8">�����Ļ</span></label> </td>
                  <td width="53" align="center" valign="middle" bgcolor="#F5F5F5"><img src="image/tui.gif" width="34" height="35" /></td>
                  <td width="60" align="left" valign="middle" bgcolor="#F5F5F5"><label onMouseOver="this.style.cursor='hand'" onMouseOut="this.style.cursor='default'" onClick="if (confirm('��ȷ��Ҫ�˳�����������')) Close();" style="FONT-SIZE:14px; COLOR: #ffcc00; TEXT-ALIGN: center"><span class="STYLE8">�˳�����</span></label>&nbsp;                   </td>
                  <td width="24" align="left" valign="middle" bgcolor="#F5F5F5"></td>
                </tr>
                <tr>
                  <td height="177" colspan="9" valign="top"><div id="chatcontent" style="OVERFLOW-Y: scroll;WIDTH: 100%; POSITION: relative; HEIGHT: 100%"><span class="STYLE9"></span></div></td>
                </tr>
                <tr>
                  <td height="28" colspan="9" valign="top" bgcolor="#F5F5F5">��ɫ<span style="FONT-SIZE: 13px">
           <select style="FONT-SIZE: 12px" name="textcolor">
             <option style="COLOR: #000000" value="000000" selected> ���Ժ�ɫ
               <option style="COLOR: #000080" value="000080"> ��������
                <option style="COLOR: #0000ff" value="0000ff"> �̿�����
                <option style="COLOR: #008080" value="008080"> ��������
                <option style="COLOR: #0080ff" value="0080ff"> ε������
                <option style="COLOR: #8080ff" value="8080ff"> ����֮��
                <option style="COLOR: #8000ff" value="8000ff"> ��������
                <option style="COLOR: #aa00cc" value="aa00cc"> �ϵľн�
                <option style="COLOR: #808000" value="808000"> �����Ʒ�
                <option style="COLOR: #808080" value="808080"> �׶ػ���
                <option style="COLOR: #ccaa00" value="ccaa00"> ������ŵ
                <option style="COLOR: #800000" value="800000"> ��ɬ�ĺ�
                <option style="COLOR: #ff0000" value="ff0000"> ����ϲ��
                <option style="COLOR: #ff0080" value="ff0080"> ���İ�ʾ
                <option style="COLOR: #ff00ff" value="ff00ff"> ��ķ���
                <option style="COLOR: #ff8080" value="ff8080"> ����ƮƮ
                <option style="COLOR: #ff8000" value="ff8000"> �ƽ�����
                <option style="COLOR: #ff80ff" value="ff80ff"> �Ͻ�����
                <option style="COLOR: #008000" value="008000"> �������
                <option style="COLOR: #345678" value="345678">�Ҳ�֪��</option>
           </select>
         ����
         <select style="FONT-SIZE: 12px" name="expression">
           <option value="" selected> ��ѡ��
             <option value="Ц��"> Ц��
               <option value="���˵�"> ���˵�
                <option value="��������"> ��������
                <option value="΢Ц"> ΢Ц
                <option value="�Ҹ�"> �Ҹ�
                <option value="�е�����"> �е�����
                <option value="ʹ����ο"> ʹ����ο
                <option value="��������"> ��������
                <option value="���Ҫ��"> ���Ҫ��
                <option value="�������"> �������
                <option value="һ�ѱ���"> һ�ѱ���
                <option value="���޹�"> ���޹�
                <option value="����ˮ"> ����ˮ
                <option value="��������"> ��������
                <option value="�����ֻ�"> �����ֻ�
                <option value="�ܲ�����"> �ܲ�����
                <option value="��������"> ��������
                <option value="ȭ�����"> ȭ�����
                <option value="��֪����"> ��֪����
                <option value="���䵹��"> ���䵹��
                <option value="���ź�"> ���ź�
                <option value="������"> ������
                <option value="���⾯��"> ���⾯��
                <option value="������Ȼ"> ������Ȼ
                <option value="��Ƿ����"> ��Ƿ����
                <option value="С����"> С����
                <option value="��������"> ��������
                <option value="���һ��"> ���һ��
                <option value="�ź���˵"> �ź���˵
                <option value="�޾����"> �޾����
                <option value="����"> ����
                <option value="���"> ���
                <option value="������˼"> ������˼
                <option value="���˵س�"> ���˵س�
                <option value="����س�"> ����س�
                <option value="�ܲ���"> �ܲ���
                <option value="��������">��������</option>
         </select>
         �������
         <SELECT style="FONT-SIZE: 12px" name="userlist">
           <OPTION value="���" selected>���е���</OPTION>
         </SELECT>
         <INPUT id="isSecret" type="checkbox" name="isSecret">
         ��̸</span></td>
                </tr>
                <tr>
                  <td height="133" colspan="9"><span style="FONT-SIZE: 13px">
                    <textarea id="content" onKeyDown="if (event.keyCode == 13) {send();return false;}" style="width: 432px; height: 120px; right: 0px;" name="content"></textarea>
                  </span></td>
                  </tr>
                <tr>
                  <td height="34" colspan="9" align="center" valign="middle"><span style="FONT-SIZE: 13px; height: 39px;">
                    <input id="btnSend" onClick="send();return false;" type="button" value="����" name="btnSend" />
                  </span>&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; <span style="FONT-SIZE: 13px; height: 39px;">
                  <input id="btnSend2" onClick="document.all('chatcontent').innerHTML = ''" type="button" value="����" name="btnSend2" />
                  </span>&nbsp;</td>
                </tr>
                
                
              </table>              </td>
              <td width="13">&nbsp;</td>
              <td width="122" valign="top"><br>
      <br>
      <div id="usercount"></div>
      <div id="onlineusers"></div></td>
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
 <div align="center">
     <script language="javascript">
   refresh_chatcontent();
   refresh_onlineusers();
   </script>
    </div>
  </form>
</body>
</html>
