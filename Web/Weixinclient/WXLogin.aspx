<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WXLogin.aspx.cs" Inherits="Weixinclient_WXLogin" %>
<!doctype html>
<html>
<head runat="server">
<meta charset="utf-8">
<meta name="keywords" content="" />
<meta name="description" content="" />
<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
<meta name="format-detection" content="telephone=no" />
<meta name="format-detection" content="email=no" />
<meta name="apple-mobile-web-app-capable" content="yes" />
<meta name="apple-mobile-web-app-status-bar-style" content="black">
<link type="text/css" rel="stylesheet" href="css/style.css" />
<script type="text/javascript" src="js/zepto.min.js"></script>
<script type="text/javascript" src="js/frozen.js"></script>
<script type="text/javascript" src="js/jquery-1.10.2.js"></script>
<script type="text/javascript" src="js/payment.js"></script>
<script src="/artDialog/jquery.artDialog.source.js?skin=green" type="text/javascript"></script>  
<link rel="stylesheet" type="text/css" href="css/normalize.css">

<link rel="stylesheet" type="text/css" href="css/jquery.slideunlock.css">


    <script type="text/javascript" src="js/jquery.min.js"></script>

<%--<script type="text/javascript" src="js/jquery.mobile.min.js"></script>--%>
<style>
*{ margin:0; padding:0; list-style:none;}
img{ border:0;}
.lanren{ position:fixed; right:0; top:15%;}
.lanren .slide_min{ width:28px; height:112px; background:url(images/slide_min.jpg) no-repeat; cursor:pointer;}
.lanren .slide_box{ width:154px; height:auto; overflow:hidden; background:url(http://demo.lanrenzhijia.com/2014/service1205/images/slide_box_bg.jpg) repeat-y; font-size:12px; text-align:center; line-height:130%; color:#666; border-bottom:2px solid #76A20D;}
.lanren .slide_box .weixin{ margin-bottom:5px;}
.lanren .slide_box img{ cursor:pointer;}
.lanren .slide_box p{ text-align:center; padding:5px; margin:5px;border-bottom:1px solid #ddd;}
.lanren .slide_box span{ padding:5px 10px; display:block;}
.lanren .slide_box span a{ color:#76A20C;}
</style>
<script type="text/javascript" src="js/jquery.slideunlock.min.js"></script>

<script type="text/javascript">

    $(function () {
        $(".logo-btn").attr("disabled", "disabled");
        $(".logo-btn").css("color", "gray");
        var slider = new SliderUnlock("#slider", {}, function () {

            $('#lableTip').html("<%= GetGlobalResourceObject("SystemResource","Unlocked").ToString() %>");
            $(".logo-btn").removeAttr("disabled");
            // $(":input").removeAttr("disabled");
            $(".logo-btn").css("color", "white");
            $(".logo-btn").attr("class", "logo-btn");
            
            $("#label").unbind();
            $("#slider").unbind();
        },
         function () {

            // $(".warn").text("index:" + slider.index + "， max:" + slider.max + ",lableIndex:" + slider.lableIndex + ",value:" + $("#lockable").val() + " date:" + new Date().getUTCDate());
         });
        
        slider.init();
       
        var action = '<%=Request["action"]%>'
        
        
        if (action == 'resubmit' && '<%=haveResubmit%>' == '')
        {
            document.getElementById("hlogin").value = "1";
            $("#form1").submit();
        }
        //$("#reset-btn").on('click', function () {
        //    slider.reset();
        //});
    })

</script>
    <script>
        $(function(){
            var thisBox = $('.lanren');
            var defaultTop = thisBox.offset().top;
            var slide_min = $('.lanren .slide_min');
            var slide_box = $('.lanren .slide_box');
            var closed = $('.lanren .slide_box h2 img');
            slide_min.on('click',function(){$(this).hide();    slide_box.show(500);});
            closed.on('click', function () { slide_box.hide(400); slide_min.show(800); });
            if ('<%=userName%>' == "")
            {
                $(".slide_min").css("background-image", "url(images/slide_minsy.jpg)");
                $("#idslide_box").attr("src", "images/slide_boxsy.jpg")
            }
            $('.telephone').click(function () {
                if ($(this).data('href')) {
                    location.href = $(this).data('href');
                }
            });
        });
</script>
<script type="text/javascript">
    function expdologin() {

        if ($("#txtUser").val() == "") {
            art.dialog("<%= GetGlobalResourceObject("Resource","InputUserName").ToString() %>");
            document.getElementById("txtUser").focus();
            return;
        }
        if ($("#txtPassword").val() == "") {
            art.dialog("<%= GetGlobalResourceObject("Resource","InputPassword").ToString() %>");
            document.getElementById("txtPassword").focus();
            return;
        }
        if ($("#txtimgcode").val() == "") {
            art.dialog("<%= GetGlobalResourceObject("Resource","InputValidCode").ToString() %>");
            document.getElementById("txtimgcode").focus();
            return;
        }

        if ($(".language_val").html() == "<%= GetGlobalResourceObject("SystemResource","Chinese").ToString() %>") {
            $("#languageType").val(0);
        }
        else if ($(".language_val").html() == "<%= GetGlobalResourceObject("SystemResource","English").ToString() %>") {
            $("#languageType").val(1);
        }
        else {
            art.dialog("<%= GetGlobalResourceObject("SystemResource","PleaseSelectLanguage").ToString() %>");
            return false;
        }

        var ischecked = false;
        <%--$.ajax({

            url: "login.aspx?action=checkcode&checkcode=" + encodeURIComponent($("#txtimgcode").val()) + "&k=" + Math.random(),
            async: false,
            success: function (data) {
                if (data == "1") {
                    ischecked = true;
                }
                else {
                    alert("<%= GetGlobalResourceObject("Resource","ValidCodeFail").ToString() %>");
                    ischecked = false;
                }
            }

        })--%>
        ischecked = true;
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
        document.getElementById("vimg").src = "../Handler/ValidateCode.ashx?k=" + Math.random();
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

    $(function () {
        try
        {
            
            var arr = getCookie("rememberpasscookie");
  
            if (arr != null)
            {
             
                if (arr == "1")
                {
                    $("#txtPassword").val("<%=password%>")
                }
            }
        }
        catch(err)
        {
        
        }

        refreshcode();
    })
</script>
<%--<title><%= GetGlobalResourceObject("Resource","GeeStarLogin").ToString() %></title>--%>
    <title><%=corp %></title>
</head>
<body style="background:#f2f2f2;">
    <div class="lanren">
        <div class="slide_min"></div>
        <div class="slide_box" style="display:none;">
            <h2><img id="idslide_box" src="images/slide_box.jpg" /></h2>
            <%if (userName == "") { %><p><a title="点击这里给我发消息" href="http://wpa.qq.com/msgrd?v=3&uin=2403887739&menu=yes" target="_blank"><img src="http://wpa.qq.com/pa?p=2:123456789:41"></a></p><%} %>
            <p class="telephone" data-href="tel:0411-39030189">            
                <b>客户服务热线1</b><br />
                <%=tel1 %>
            </p>           
            <p class="telephone" data-href="tel:<%=tel2 %>">
                <b>客户服务热线2</b><br />
               <%=tel2 %>
            </p>
        </div>
    </div>
    <form id="form1" runat="server">
        <input type="hidden" id="hlogin" name="hlogin" />
        <div class="viewport" >
       <%--   <header class="header header-positive border-b">
              <i class="icon-return" onclick="history.back()"></i>
              <h1>吉星义齿平台系统登录</h1>
          </header>--%>
          <!--header   end-->
          <section>
            <div class="logo-img" ><img src="<%=picture %>" alt="" ></div>
            <div class="logo" >
              <div class="logo-div" ><input type="text" id="txtUser" value="<%=userName %>" name="txtUser" class="logo-input logo-name" placeholder="<%= GetGlobalResourceObject("SystemResource","PleaseInputYourName").ToString() %>" ></div>
              <div class="logo-div" ><input type="password" value=""  id="txtPassword" name="txtPassword" class="logo-input logo-password" placeholder="<%= GetGlobalResourceObject("SystemResource","PleaseInputPwd").ToString() %>" ></div>
                <div style="margin-bottom:20px; ">
	                <input type="hidden" value="" id="lockable">
                     <div id="slider">		
                            <span id="label" ></span>		
                            <span id="lableTip"><%= GetGlobalResourceObject("SystemResource","SlideToRightToUnlock").ToString() %></span>	
                    </div>
                </div>                

              <div class="logo-div" style="display:none;" ><input type="text" value="0000" id="txtimgcode" style="width:160px;" class="logo-input logo-code" placeholder="<%= GetGlobalResourceObject("SystemResource","Securitycode").ToString() %>" ><img id="vimg" align="absmiddle" alt="<%= GetGlobalResourceObject("SystemResource","Cut").ToString() %>" style="height:30px; padding-top:10px;" onclick="refreshcode();" /><input style="display:none;" type="button" class="logo-click" value="<%= GetGlobalResourceObject("SystemResource","Cut").ToString() %>" onclick="refreshcode();" ></div>
              <div class="logo-div" >                
                <ul class="logo-language" style="width:100%;" >
                  <li class="language_val" ><%= GetGlobalResourceObject("SystemResource","Chinese").ToString() %></li>
                  <li class="language_div" ><%= GetGlobalResourceObject("SystemResource","English").ToString() %></li>
                </ul>
                 <input type="hidden" id="languageType" name="languageType" value="0" />
              </div>              
              <div class="logo-div1 logo-div1-margin" ><input type="button" class="logo-btn" onclick="expdologin();" value="<%= GetGlobalResourceObject("SystemResource","Login").ToString() %>" ></div>
              <div style="display:none;" class="logo-div1" ><input type="button" class="logo-btn logo-download" value="<%= GetGlobalResourceObject("SystemResource","DownloadTheSigningCertificate").ToString() %>" ></div>
            </div>
          </section>
          <!--section   end-->
          <div class="clear" ></div>
        </div>
        <!--viewport   end-->
         <script type="text/javascript">

             var i = 0;
             $(function () {

                 $(".language_val").click(function ()
                 {

                     var ul = $(".language_div");
                     if (ul.css("display") == "none") {
                         ul.css("display", "block");
                     }
                     else if (ul.css("display") == "block") {
                         ul.css("display","none");
                     }
                     if ($(this).html() == "<%= GetGlobalResourceObject("SystemResource","Language").ToString() %>") {
                         $(this).html("<%= GetGlobalResourceObject("SystemResource","Chinese").ToString() %>");
                     }
                     
                    }
                 );

                 $(".language_div").click(function () {
                     var txt = $(this).text();
                     var txt2 = $(".language_val").html();
                     $(".language_val").html(txt);
                     $(".language_div").html(txt2);
                     $(".language_div").hide();

                 });

             });
         </script>
    </form>
</body>
</html>
