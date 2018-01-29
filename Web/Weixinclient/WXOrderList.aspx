<%@ Page Title="" Language="C#" MasterPageFile="~/Weixinclient/App_Master/all_master.Master" AutoEventWireup="true" CodeFile="WXOrderList.aspx.cs" Inherits="Weixinclient_WXOrderList" %>
<%@ Register Src="~/Weixinclient/ascx/pagecutphone.ascx" TagName="pageweixincut" TagPrefix="uc1" %>
<%@ Register Src="~/Weixinclient/ascx/querycontrol.ascx" TagName="querycontrol" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script src="/artDialog/jquery.artDialog.source.js?skin=green" type="text/javascript"></script>  
<script src="/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
<script type="text/javascript" src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
<style type="text/css">
    .trace 
    {
        display:none;
    }
</style>
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

    function TourDriven()
    {
        wx.checkJsApi({
            jsApiList: [
              'getLocation',
            ],
            success: function (res) {
              
            }
        });
    }
    TourDriven();
    $(function () {
        $(".advice").click(function () {
            $("#order-bg").css({
                display: "block", height: $(document).height()
            });
            var $box = $('.talk-mask');
            $box.css({
                display: "block",
            });
            OrderNumber = $(this).parent().find("span").html();
            $("#sendContent").val("");
            $("#divResult").html("");
            wait();
        });
        //点击关闭按钮的时候，遮罩层关闭
        $(".order-close").on('click', function () {
            $("#order-bg,.talk-mask").css("display", "none");
        });


        $(".trace").click(function () {
            $("#order-bg").css({
                display: "block", height: $(document).height()
            });
            var $box = $('.trace-mask');
            $box.css({
                display: "block",
            });

            $.ajax({
            type: "GET",
            url: "WXOrderList.aspx",
            cache: false,
            data: "type=GetTrace",
            dataType: "json",
            success: function(data) 
            {
                $.each(data.data, function (i, item) 
                {
                    $(".result-list").append("<div class=\"split\"><div class=\"col1\"><dl><dt>"+item.time.split(' ')[0]+"</dt><dd>"+item.time.split(' ')[1]+"</dd></dl></div><div class=\"col2\">")
                    .append(" <span class=\"step\"><span class=\"line2\"></span><span class=\"point\"></span></span> </div> <div class=\"col3\">"+item.context+"</div></div>");
  
                }
                );
               
                //用到这个方法的地方需要重写这个success方法
               
            }
        });
        });
        //点击关闭按钮的时候，遮罩层关闭
        $(".trace-mask .order-close").on('click', function () {
            $("#order-bg,.trace-mask").css("display", "none");
        });
        
    });

    var userName = '<%=this.UserName %>';
    var BelongFactory = '<%=LoginUser.BelongFactory%>';
    var OrderNumber;
    function CommunicationByOrder() {
        
        //向comet_broadcast.asyn发送请求，消息体为文本框content中的内容，请求接收类为AsnyHandler
        if ($.trim($("#sendContent").val()).replaceAll("&nbsp;", "").replaceAll(" ", "") != "") {
            $.post("comet_broadcast.asyn", { OrderNumber: OrderNumber, BelongFactory: BelongFactory, username: userName, content: $("#sendContent").val() });
        }
        //清空内容
        $("#sendContent").val("");
    }


    function wait() {
        $.post("comet_broadcast.asyn", { OrderNumber: OrderNumber, BelongFactory: BelongFactory, username: userName, content: "-1" },
         function (data, status) {
             var result = $("#divResult");
             var lastModifyDate = data.split('|')[0]
             data = data.split('|')[1]
             setCookie(BelongFactory + OrderNumber, lastModifyDate);
             if (data.indexOf(userName + ":") == -1) {
                 result.html(result.html() + "<div>" + data + "</div>");
             }
             else {
                 //var selfSend = data;
                 result.html(result.html() + "<div style=\"color:#0098e3\">" + data + "</div>");
                 //selfSend = selfSend.replace(userName + ":", "") + ":" + userName;
                 //result.html(result.html() + "<br/><div style=\"text-align:right\">" + selfSend + "</div>");
             }
             document.getElementById("divResult").scrollTop = document.getElementById("divResult").scrollHeight;
             //服务器返回消息,再次立连接
             //console.log(data);
             wait();
         }, "html"
         ).error(function (XMLHttpRequest, textStatus, errorThrown) {
            // console.log("状态异常");
           //  console.log(XMLHttpRequest.status);
             wait();
           //  console.log("状态异常重新发起");
             //alert("error");
             //alert(XMLHttpRequest.status);
             //alert(XMLHttpRequest.readyState);
             //alert(textStatus);
         });
    }

    String.prototype.replaceAll = function (s1, s2) {
        return this.replace(new RegExp(s1, "gm"), s2);
    }

    function setCookie(name, value) {
        var Days = 30;
        var exp = new Date();
        exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
        document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
    }

    //读取cookies 
    function getCookie(name) {
        var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");

        if (arr = document.cookie.match(reg))

            return unescape(arr[2]);
        else
            return null;
    }

    function OpenTalkHistory() {

        dialog = art.dialog({
            id: 'PIC031',
            title: "<%= GetGlobalResourceObject("Resource","TalkHistory").ToString() %>",
                background: '#000',
                width: 300,
                height: 400,
                opacity: 0.3,

                close: function () {
                }
            });

       <%--  $.ajax({
             url: "OrderListQuery.aspx?OrderNumber=" + talkid + "&action=ReadTalkRecord",
             success: function (data) {
                 setCookie("<%=LoginUser.BelongFactory%>" + talkid, data)
                },
                cache: false
            })--%>

            $.ajax({
                url: "HistoryTalk.aspx?OrderNumber=" + OrderNumber,
                success: function (data) {                   
                    dialog.content(data);
                },
                cache: false
            });
        }
</script>
 <section >
        <uc2:querycontrol ID="querycontrol" runat="server" />
         <div class="search-list">
             <%if (ilist != null)
                     {
                         for (int i = 0; i < ilist.Count; i++)
                         {%>
                <% if (i % 2 == 0)
                     {%>
                 <div class="search-div search-div-bg" >
                     <div class="search-details" ><a href="details.aspx?orderID=<%=((ORDERS)ilist[i]).Order_ID %>&serial=<%=((ORDERS)ilist[i]).serial %>"><%= GetGlobalResourceObject("SystemResource", "BarCode").ToString() %>:<span><%=((ORDERS)ilist[i]).Order_ID %></span><i ><%= GetGlobalResourceObject("SystemResource", "Patient").ToString() %>:<%=((ORDERS)ilist[i]).Patient %></i></a><i class="advice" ><img src="images/advice.png" alt="" ><%=IsCN?"沟通":"Talk" %></i>&nbsp;<i class="trace" ><img src="images/advice.png" alt="" ><%=IsCN?"物流信息":"Trace" %></i></div>
                     <a href="details.aspx?orderID=<%=((ORDERS)ilist[i]).Order_ID %>&serial=<%=((ORDERS)ilist[i]).serial %>"><div class="search-details" ><i ><%= GetGlobalResourceObject("Resource", "STHospital").ToString() %>:<%=((ORDERS)ilist[i]).hospital %></i><i ><%= GetGlobalResourceObject("SystemResource", "Doctor").ToString() %>:<%=((ORDERS)ilist[i]).doctor %></i></div>
                     <div class="search-details" ><i ><%= GetGlobalResourceObject("SystemResource", "DateToFactory").ToString() %>:<%=((ORDERS)ilist[i]).indate.ToShortDateString() %></i><i ><%= GetGlobalResourceObject("SystemResource", "FactoryProcess").ToString() %>:<%=((ORDERS)ilist[i]).process %></i></div></a>
                </div>
             <%}
                     else
                     { %>
                 <div class="search-div" >
                     <div class="search-details" ><a href="details.aspx?orderID=<%=((ORDERS)ilist[i]).Order_ID %>&serial=<%=((ORDERS)ilist[i]).serial %>"><a href="details.aspx?orderID=<%=((ORDERS)ilist[i]).Order_ID %>&serial=<%=((ORDERS)ilist[i]).serial %>"><%= GetGlobalResourceObject("SystemResource", "BarCode").ToString() %>:<span><%=((ORDERS)ilist[i]).Order_ID %></span><i ><%= GetGlobalResourceObject("SystemResource", "Patient").ToString() %>:<%=((ORDERS)ilist[i]).Patient %></i></a><i class="advice" ><img src="images/advice.png" alt="" ><%=IsCN?"沟通":"Talk" %></i>&nbsp;<i class="trace" ><img src="images/advice.png" alt="" ><%=IsCN?"物流信息":"Trace" %></i></div>
                     <a href="details.aspx?orderID=<%=((ORDERS)ilist[i]).Order_ID %>&serial=<%=((ORDERS)ilist[i]).serial %>"><div class="search-details" ><i ><%= GetGlobalResourceObject("Resource", "STHospital").ToString() %>:<%=((ORDERS)ilist[i]).hospital %></i><i ><%= GetGlobalResourceObject("SystemResource", "Doctor").ToString() %>:<%=((ORDERS)ilist[i]).doctor %></i></div>
                     <div class="search-details" ><i ><%= GetGlobalResourceObject("SystemResource", "DateToFactory").ToString() %>:<%=((ORDERS)ilist[i]).indate.ToShortDateString() %></i><i ><%= GetGlobalResourceObject("SystemResource", "FactoryProcess").ToString() %>:<%=((ORDERS)ilist[i]).process %></i></div></a>
                </div>
             <%} %>
             <%}
                     } %>
         </div>        
         <ul class="page" >            
           <uc1:pageweixincut ID="pagecutID" runat="server" />
        </ul> 
        <div id="order-bg"></div>
        <div class="talk-mask" >
            <div class="order-close" >&nbsp;</div>
            <div onclick="OpenTalkHistory()" ><span style="color:#20d4c9;"><%=IsCN?"==打开聊天记录==":"==Talk Record==" %></span></div>
             <div style="position: absolute; top: 40px;bottom: 60px; left:0px; right:0px; background-color :#edeaea; text-align:left; "id="divResult">&nbsp;</div>
             <div style=" height:50px; position: absolute; bottom: 10px; left:0px; right:0px; ">
               <div style="float:left; width:83%; margin-left:2%; "><textarea id="sendContent" style="width:100%; height:95%; border:none; margin-top:5px;border-bottom:1px solid black;"></textarea></div>
                 <div style="float:left; width:12%;  margin-left:2%; vertical-align:central; height:80%; text-align:center;"><button type="button" class="btn-sendimg" onclick="CommunicationByOrder()"><span><%=IsCN?"发送":"Send" %></span></button></div>
             </div>
        </div>  
        <div class="trace-mask" >
            物流信息<div class="order-close" >&nbsp;</div>
          <div style="position: absolute; top: 40px;bottom: 60px; left:0px; right:0px; background-color :white; text-align:left; overflow:auto; " id="divTrace">
            <div class="result-success" id="success" >
                <div class="result-top" style="width:100%;" id="resultTop">
                  <div class="col1" style="text-align:center;">时间</div>
                  <div class="col2">地点和跟踪进度</div>
                </div>
                <div id="result" class="result-list">
                  
                </div>            
            </div>
          </div>
        </div>        
   </section>
  <!--section   end-->

</asp:Content>
