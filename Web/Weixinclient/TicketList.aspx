<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TicketList.aspx.cs" Inherits="Weixinclient_TicketList" %>
<%@ Register Src="~/Weixinclient/ascx/pagecutphone.ascx" TagName="pageweixincut" TagPrefix="uc1" %>
<%@ Register Src="~/Weixinclient/ascx/weixintop.ascx" TagName="weixintop" TagPrefix="uccontrol" %>
<%@ Register Src="~/Weixinclient/ascx/weixinbottom.ascx" TagName="weixinbottom" TagPrefix="uccontrol" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>	     
    </title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=no" />
    <link href="css/dd.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/jquery-1.11.0.min.js"></script>  
    <script type="text/javascript" src="js/slick.js"></script> 
    <script type="text/javascript" src="js/slick.min.js"></script> 
    <script type="text/javascript" src="js/weixin.js"></script>
    <script type="text/javascript" src="js/WeixinSplitPage.js"></script>
    <link rel="stylesheet" type="text/css" href="css/all.css"/> 
    <script type="text/javascript" src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
</head>
<body>
    <form  name="form1" method="post" action="TicketList.aspx" id="form1">
       <script type="text/javascript">
           var x ;
           var y ;
           var localIds; 
           var serverId; 
           wx.config({
                       debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
                       appId:'<%=Session["APPID"]%>', // 必填，公众号的唯一标识
                       timestamp:<%=timeStamp%>, // 必填，生成签名的时间戳
                nonceStr:'Wm3WZYTPz0wzccnW', // 必填，生成签名的随机串
                signature: '<%=signalticket%>', // 必填，签名，见附录1
                     jsApiList:['checkJsApi','getLocation','onMenuShareTimeline','chooseImage','uploadImage','hideOptionMenu'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
            });

           var images = {
               localId: [],
               serverId: []
           };
           function ChooseImage () {
               wx.chooseImage({
                   success: function (res) {
                       $("#picurl").html("");
                       images.localId = res.localIds;                    
                   }
               });
           }

           wx.ready(function(){        
               ChooseImage();           
           });
       </script>

    </form>
</body>
</html>

