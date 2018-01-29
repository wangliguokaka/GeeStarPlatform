<%@ Page Title="" Language="C#" MasterPageFile="~/Weixinclient/App_Master/all_master.Master" AutoEventWireup="true" CodeFile="MemberCenter.aspx.cs" Inherits="Weixin.MemberCenter" %>
<%@ Register Src="~/Weixinclient/ascx/pagecutphone.ascx" TagName="pageweixincut" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="/artDialog/jquery.artDialog.source.js?skin=green" type="text/javascript"></script>  
    <script type="text/javascript">
        $(document).ready(function () {
            if ('<%=Request["txtNewPassword"]%>' != "") {

                art.dialog({
                    content: '<%= GetGlobalResourceObject("SystemResource","PwdSuccess").ToString() %>',
                    ok: function () {
                       
                    }
                });

            }

            var allMenuBM = "<%=Session["allMenuBM"]%>"
            allMenuBM.split(',').forEach(function (i,n)
            {
                if (i != "")
                {
                    $("." + i).prop("disabled", "");
                }
            })

            var arr = getCookie("rememberpasscookie");
            var selectMenu = getCookie("menubmcookie");
            if (arr == "1") {
                $("#remember").prop("checked", true);
            }
            if (selectMenu != null)
            {
                $("#" + selectMenu).prop("checked","checked")
            }

            $("#openmenu").bind("click",function (e) {
                if ($("#diaMenu").css("display") == "none") {
                    $("#diaMenu").css("width", "150px")
                    $("#diaMenu").show(500);
                   
                    $("#diaMenu").css({
                        "top": (e.pageY+5) + "px",
                        "left": (e.pageX - 160) + "px"
                    });
                }
                else {
                    $("#diaMenu").hide(300);
                }
            });
        });        

        $.ajaxSetup({ cache: false });
        function SavePassword() {
           
            if ($.trim($("#txtOldPassword").val()) == "" || $.trim($("#txtNewPassword").val()) == "" || $.trim($("#txtConfirmPassword").val()) == "") {
                art.dialog({ title: false, time: 2, icon: 'error', content: '<%= GetGlobalResourceObject("SystemResource","PwdNull").ToString() %>' });
                return false;
            }
            if ($.trim($("#txtNewPassword").val()) != $.trim($("#txtConfirmPassword").val())) {
                art.dialog({ title: false, time: 2, icon: 'error', content: '<%= GetGlobalResourceObject("SystemResource","PwdDifferent").ToString() %>' });
                return false;
            }
            var Ischeck = "0";
            $.ajax({
                url: "MemberCenter.aspx?action=CheckPassword",
                data: { "oldpassword": $("#txtOldPassword").val() },
                async: false,
                success: function (data) {
                    Ischeck = data;
                }
            })

            if (Ischeck == "0") {
                art.dialog({ title: false, time: 2, icon: 'error', content: '<%= GetGlobalResourceObject("SystemResource","WrongOldPwd").ToString() %>' });
                return false;
            }

            $("#haddinfo").val("1");
            document.forms[0].submit();
        }

        function DoLogout()
        {
            $("#haddinfo").val("2");
            document.forms[0].submit();
        }

        function setCookie(name, value) {
            var Days = 30;
            var exp = new Date();
            exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
            document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString() + ";path=/";
        }

        function getCookieVal(offset) {

            var endstr = document.cookie.indexOf(";", offset);

            if (endstr == -1) {

                endstr = document.cookie.length;

            }

            return unescape(document.cookie.substring(offset, endstr));

        }



        // primary function to retrieve cookie by name

        function getCookie(name) {

            var arg = name + "=";

            var alen = arg.length;

            var clen = document.cookie.length;

            var i = 0;

            while (i < clen) {

                var j = i + alen;

                if (document.cookie.substring(i, j) == arg) {

                    return getCookieVal(j);

                }

                i = document.cookie.indexOf(" ", i) + 1;

                if (i == 0) break;

            }

            return null;

        }

        function RememberPass()
        {
            if ($("#remember").prop("checked") == true) {               
                setCookie("rememberpasscookie", "1")
            
            }
            else {               
                setCookie("rememberpasscookie", "0")                
            }
        }

        function SetDefaultPage(MenuBM) {
           
            setCookie("menubmcookie", MenuBM);
            $("#diaMenu").hide(200);
        }
    </script>
<section >
    <div style="height:150px; background-color:#fa8a8a;">
       <div class="logo-img" style=" float:left;height:100%; margin:auto; width:100px; margin-top:25px; text-align:center;">
           <img src="<%=Session["HeadUrl"] %>" style="width:80px; height:80px;" />
       </div>
        <div style="float:left; margin:auto; margin-top:30px;">
             <p style="line-height:30px;"><%if (IsCN) {%>用户名：<%}else{ %>UserName:<%} %><%=LoginUser.UserName %></p>
            <%--<p style="line-height:30px;"><%if (IsCN) {%>微信号：<%}else{ %>Weixin:<%} %><%=Session["NickName"].ToString() %></p>--%>
            <p  style="line-height:30px;"><%if (IsCN) {%>加工厂：<%}else{ %>Lab.:<%} %><%=LoginUser.JGCName %></p>   
            <p style="line-height:30px; "><%if (IsCN) {%>记住密码：<%}else{ %>Remember Password:<%} %><input type="checkbox" style="height:15px;width:15px;"  id="remember" onclick="RememberPass()" />&nbsp;&nbsp;&nbsp;&nbsp;<%if (IsCN) {%>首页设置：<%}else{ %>HomePage:<%} %><a href="#" id="openmenu" > <img src="images/set_on.png" style="height:18px;width:18px; margin-bottom:5px;"  /></a></p>  
            <p style="line-height:30px; "> </p>                  
        </div>
        <div style="float:right; margin-top:35px;margin-right:60px;display:none;">
           <a href="#" id="openmenu1" > <img src="images/set_on.png"  /></a>
        </div>
    </div>
    <input id="haddinfo" name="haddinfo" type="hidden" />
    <div style=" width: 100%; margin-top:30px; text-align:center; " >
        <div style="width:80%; margin:0px auto;">
             <ul>
			    <li style="margin-bottom:15px;"><%= GetGlobalResourceObject("SystemResource","OldPwd").ToString() %>：&nbsp;&nbsp;&nbsp;&nbsp;<input type="text" class="User-text" name="txtOldPassword" id="txtOldPassword" ></li>
                <li style="margin-bottom:15px;"><%= GetGlobalResourceObject("SystemResource","NewPwd").ToString() %>：&nbsp;&nbsp;&nbsp;&nbsp;<input type="text" class="User-text"  name="txtNewPassword"  id="txtNewPassword"  ></li>
                <li style="margin-bottom:15px;"><%= GetGlobalResourceObject("SystemResource","ConfrimPwd").ToString() %>：<input type="text" class="User-text"  name="txtConfirmPassword" id="txtConfirmPassword"></li>
                <li style="margin-bottom:15px;"><button onclick="SavePassword();" type="button" class="c-btnbig"><%= GetGlobalResourceObject("Resource","ModifyPass").ToString() %></button></li>
                <li><button  class="c-btnbig" type="button" onclick="DoLogout();"><%= GetGlobalResourceObject("Resource","Logout").ToString() %></button></li>
            </ul>
        </div>
       
    </div>
    <div id="diaMenu" style="position:absolute; font-family:'Microsoft YaHei UI'; color:white; border:solid #aaa 1px;background-color:#fa8a8a;display:none; height:220px ">
        <div style=" background-color:white; text-align:center;color:black; "><strong>设置功能首页</strong></div>
        <div style="margin:5px;">
             
            <input type="radio" name="menucheck" class="OI" id="OrderInput" disabled="disabled" onclick="SetDefaultPage('OrderInput')"  />&nbsp;&nbsp;订单输入<hr />
            <input type="radio" name="menucheck" class="OQ"  id="WXOrderList" disabled="disabled" onclick="SetDefaultPage('WXOrderList')"  />&nbsp;&nbsp;订单查询<hr />
            <input type="radio" name="menucheck" class="PI"  id="MemberCenter" disabled="disabled" onclick="SetDefaultPage('MemberCenter')"  />&nbsp;&nbsp;个人中心<hr />
            <input type="radio" name="menucheck" class="FS"  id="FinaceSummary" disabled="disabled"  onclick="SetDefaultPage('FinaceSummary')" />&nbsp;&nbsp;结算单汇总表<hr />
            <input type="radio" name="menucheck" class="FD"  id="FinaceSummaryDetail" disabled="disabled" onclick="SetDefaultPage('FinaceSummaryDetail')"  />&nbsp;&nbsp;结算单明细表<hr />
		<input type="radio" name="menucheck" class="PL"  id="SmallClassSummary" disabled="disabled" onclick="SetDefaultPage('SmallClassSummary')"  />&nbsp;&nbsp;来模一览表<hr />
		<input type="radio" name="menucheck" class="AP"  id="AnalysisPie" disabled="disabled" onclick="SetDefaultPage('AnalysisPie')"  />&nbsp;&nbsp;销售分析表
        </div>
       
    </div>
    <!--earch-list end-->
  
  </section>
  <!--section   end-->
</asp:Content>
