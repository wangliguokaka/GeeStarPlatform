<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="cic_login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=PlatformTitile%></title>

<meta http-equiv="X-UA-Compatible" content="IE=8" >

    <script type="text/javascript" src="/jQuery/jquery-1.10.2.js"></script>
    <script src="/artDialog/jquery.artDialog.source.js?skin=green" type="text/javascript"></script>
    <script type="text/javascript" src="/OrderManagement/js/all.js?rnd=1"></script>
    <script type="text/javascript">

        function expdologin() {

            if ($("#txtUser").val() == "") {
                alertdialog("<%= GetGlobalResourceObject("Resource","InputUserName").ToString() %>");
                document.getElementById("txtUser").focus();
                return;
            }
            if ($("#txtPassword").val() == "") {
                alertdialog("<%= GetGlobalResourceObject("Resource","InputPassword").ToString() %>");
                document.getElementById("txtPassword").focus();
                return;
            }
            if ($("#txtimgcode").val() == "") {
                alertdialog("<%= GetGlobalResourceObject("Resource","InputValidCode").ToString() %>");
                document.getElementById("txtimgcode").focus();
                return;
            }
            var ischecked = false;
            $.ajax({
                
                url: "login.aspx?action=checkcode&checkcode=" + encodeURIComponent($("#txtimgcode").val())+"&k=" + Math.random(),
                async: false,
                success: function (data) {
                    if (data == "1") {
                        ischecked = true;
                    }
                    else {
                        alertdialog("<%= GetGlobalResourceObject("Resource","ValidCodeFail").ToString() %>");
                        ischecked = false;
                    }
                }
            
            })
            if (ischecked) {
                document.getElementById("hlogin").value = "1";
                //document.forms[0].submit();
                $("#form1").submit();
            }
            
        }
        if (document.addEventListener) {   
            document.addEventListener("keypress", fireFoxHandler, true);

        } else {
            document.attachEvent("onkeypress", ieHandler);
        }

        function fireFoxHandler(evt) {
            if (evt.keyCode == 13) {
                expdologin();
            }
        }

        function ieHandler(evt) {
            if (evt.keyCode == 13) {

                expdologin();

            }
        }
        function startL() {
            document.getElementById("txtUser").focus();
        }
        function refreshcode() {
            document.getElementById("vimg").src = "Handler/ValidateCode.ashx?k=" + Math.random();
        }

        $(function () {
            refreshcode();
        })
    </script>
    <style type="text/css">
        
        body
        {
            padding: 0;
            margin: 0;
            background-color:#0f5590;
        }
        .topbox
        {
            height: 60px;
            width: 100%;
            background-color: #0f5590;
            border-bottom: 18px solid #167cc4;
        }
        .middlebox
        {
            height: 500px;
            width: 100%;
            position:absolute;
            top:50%;
            margin-top:-250px;
            background-color: #eee;
        }
        .cicblock
        {
            width: 100%;
            height: 79px;
        }
        .ciclogbox
        {
            height: 340px;
            width: 800px;
            background-color: #fff;
            border: 10px solid #e1e9ec;
            margin: 0 auto;
        }
        .cicloglogo
        {
            height: 240px;
            width: 360px; 
            background-image: url(images/tooth.jpg);
            background-repeat:no-repeat;
            background-position:0px 0px;
            float:left;
            border-right:1px solid #c9e3ed;
            margin: 30px 0 30px 0;
        }
        .namebox {
            float: left;
            font-family: 微软雅黑;
            font-size: 16px;
            line-height: 25px;
            padding-right: 12px;
            text-align: right;
            width: 90px;
            padding-top: 5px;
        }
        .txtiput {
            background-color: #FFFFFF;
            border: 1px solid #ccc;
            float: left;
            height: 28px;
            line-height: 26px;
            margin-right: 5px;
            width: 238px;
            z-index: 99999;
            padding:0 10px 0 10px;
        }
        .dla {
            background-color: #00B0F0;
            color: #FFFFFF;
            display: block;
            font-family: 微软雅黑;
            font-size: 16px;
            font-weight: bolder;
            height: 30px;
            line-height: 28px;
            margin-left: 1px;
            margin-top: 5px;
            text-align: center;
            width: 90px;
            border:0px;
        }
        .lrbtitle {
            border-bottom: 1px solid #4A7EBB;
            color: #333333;
            font-family: 微软雅黑;
            font-size: 20px;
            height: 42px;
            padding-left: 10px;
            padding-top: 50px;
            margin-right: 50px;
            margin-bottom: 15px;
            width: 329px;
            float:right;
        }
        .copytightline
        {
            height: 20px;
            width: 100%;
            vertical-align:bottom;
            background-color: #0f5590;
            text-align: center;
            font-family: 微软雅黑;
            font-size: 12px;
            color: #fff;
            position:fixed;
            bottom:0px;
            margin-top:-20px;
            top:100%;
        }
        .bottombox
        {
            height: 135px;
            width: 100%;
            background-color: #0f5590;
        }
        </style>
</head>
<body>
    <form id="form1" method="post" runat="server" autocomplete="off" >
    <input id="hlogin" name="hlogin" type="hidden" />
    <div runat="server" id="wx"  style="color: #FF0000">
      
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
      
    <asp:Timer ID="Timer1" runat="server" ontick="Timer1_Tick" Enabled="false" Interval="6000">
    </asp:Timer>
     请不要关闭此页面，微信通道专用!!! 
    </div>  
    <div runat="server" id="login">
        <%--<div class="topbox"></div>--%>
        <div class="middlebox">
            <div class="cicblock"></div>
            <div class="ciclogbox">
                <div class="cicloglogo"></div>
                <div class="lrbtitle">
                    <%--<%= GetGlobalResourceObject("Resource","GeeStarLogin").ToString() %>--%>
                    <%=loginPlatForm %>
                </div>
                <table style="width: 200px; margin-left: 383px; margin-top: 20px;">
                        <tr style=" height:40px;">
                            <td align="right" class="namebox" style="color: #333">
                                <%= GetGlobalResourceObject("Resource","UserName").ToString() %>
                            </td>
                            <td>
                                <input id="txtUser" name="txtUser" class="txtiput" type="text" value="" /><%----%>
                            </td>
                        </tr>
                        <tr style=" height:40px;">
                            <td align="right" class="namebox" style="color: #333">
                                <%= GetGlobalResourceObject("Resource","Pass").ToString() %>


                            </td>
                            <td>
                                <input id="txtPassword" name="txtPassword" class="txtiput" type="password" value="" /><%----%>
                            </td>
                        </tr>
                        <tr style=" height:40px;">
                            <td align="right" class="namebox" style="color: #333">
                                <%= GetGlobalResourceObject("Resource","Language").ToString() %>                                
                            </td>
                            <td>
                               <asp:DropDownList ID="ddlLanguage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Language_SelectedIndexChanged">
                                   <asp:ListItem Text="中文" Value="0"></asp:ListItem>
                                   <asp:ListItem Text="English" Value="1"></asp:ListItem>
                               </asp:DropDownList>
                            </td>
                        </tr>
                        <tr style=" height:40px;">
                            <td align="right" class="namebox" style="color: #333">
                                <%= GetGlobalResourceObject("Resource","ValidCode").ToString() %>
                            </td>
                            <td>
                                
                                <input class="txtinbox" type="text" id="txtimgcode" autocomplete="none" name="txtimgcode" style="width: 80px;" />
                            <img id="vimg" align="absmiddle" alt="不区分大小写,点击切换"
                                onclick="refreshcode();" />
                            <a href="javascript:void(0);" onclick="refreshcode();" style="font-size: 12px; background-image: none;">
                                <%= GetGlobalResourceObject("Resource","ClickChange").ToString() %></a>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td><input id="btnLogin" class="dla" onclick="expdologin();" type="button" value="<%= GetGlobalResourceObject("Resource","Login").ToString() %>"  style="cursor:pointer;" /></td>
                                        <td><a class="dla"  href="CaptureImage.zip" style="cursor:pointer; width:160px; text-decoration:none;"><%= GetGlobalResourceObject("Resource","DownloadActivex").ToString() %></a></td>
                                    </tr>
                                </table>  
                            </td>
                        </tr>                  
                </table>        
            </div>
            
        </div>
        <div class="copytightline"><%=CopyRight %></div>
        <%--<div class="bottombox"></div>--%>
    </div>
    </form>
</body>
</html>
