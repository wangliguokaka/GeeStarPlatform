<%@ Page Language="C#" AutoEventWireup="true" CodeFile="menu.aspx.cs" Inherits="Weixinclient_menu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">

        function UpdateInfo() {

            if (document.getElementById("txtmenu").value == "") {
                alert("请输入menu");
                document.getElementById("txtmenu").focus();
                return;
            }

            document.getElementById("upinfo").value = "1";
            document.forms[0].submit();

        }
        function delmenu() {

            document.getElementById("hdel").value = "1";
            document.forms[0].submit();
        }
        function getToken() {

           

            document.getElementById("hgettoken").value = "1";
            document.forms[0].submit();
            
        }
    </script>
    <input id="hgettoken" name="hgettoken" type="hidden" />
    <input id="upinfo" name="upinfo" type="hidden" />
    <input id="hdel" name="hdel" type="hidden" />
    <table style="width:800px">
        <tr>
            <td>
                菜单内容
            </td>
            <td>
                <textarea   id="txtmenu" name="txtmenu" cols="20" style="width:600px;height:200px" rows="2">
{
     "button":[
         {
          "type":"view",
          "name":"客户自助平台",
          "url":"http://www.chaya8.com/Weixinclient/WXLogin.aspx"
          },
         {
    "name":"上传照片",
           "sub_button":[
            {
               "type":"pic_sysphoto",
               "name":"拍照上传",
               "key": "rselfmenu_1_0",
                   "sub_button": [ ]
            },
            {
               "type": "pic_weixin",
                    "name": "打开相册",
                    "key": "rselfmenu_1_2",
                    "sub_button": [ ]
            },
         {
          "type":"click",
          "name":"上传照片设置",
          "key":"SETUPLOADPATH"
          
          }]},
         {
    "name":"义齿相关",
           "sub_button":[
         {
          "type":"click",
          "name":"义齿新闻",
          "key":"BINDUSER"
          },
            {
          "type":"click",
          "name":"义齿介绍",
          "key":"BINDUSERBABY"
            }]}
      ]
}
                

                </textarea>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <input id="Button1" type="button" value="设置菜单" onclick="UpdateInfo();" />
                <input id="Button2" type="button" value="删除菜单" onclick="delmenu();" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            <%=setresult %>
            </td>
        </tr>
    </table>
 </form>
</body>
</html>

