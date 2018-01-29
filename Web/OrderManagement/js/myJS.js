function send() {

    var OrderNumber = $("#OrderNumber").val();
    var userName = $("#UserName").val();
    var BelongFactory = $("#BelongFactory").val();
    //向comet_broadcast.asyn发送请求，消息体为文本框content中的内容，请求接收类为AsnyHandler
    
    if ($.trim($(".wangEditor-textarea").html()).replaceAll("&nbsp;", "").replaceAll(" ", "") != "")
    {
        $.post("comet_broadcast.asyn", { OrderNumber: OrderNumber, BelongFactory: BelongFactory, username: userName, content: $(".wangEditor-textarea").html() });
    }
    //清空内容
    $("#content").val("");
    $(".wangEditor-textarea").html("");
}


$(document).ready(function () {
    var userName = $("#UserName").val();
    var BelongFactory = $("#BelongFactory").val();

    function wait() {
        var OrderNumber = $("#OrderNumber").val();
        $.post("comet_broadcast.asyn", { OrderNumber: OrderNumber, BelongFactory:BelongFactory,username: userName, content: "-1" },
         function (data, status) {
             var result = $("#divResult");
             var lastModifyDate = data.split('|')[0]
             data = data.split('|')[1]
             setCookie(BelongFactory + OrderNumber, lastModifyDate);
             if (data.indexOf(userName + ":") == -1) {
                 result.html(result.html() + "<div>"+data+"</div>");
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
             console.log("状态异常");
             console.log(XMLHttpRequest.status);
             wait();
             console.log("状态异常重新发起");
             //alert("error");
             //alert(XMLHttpRequest.status);
             //alert(XMLHttpRequest.readyState);
             //alert(textStatus);
         });
    }

    //初始化连接
    wait();


    

    $("#btnSend").click(function () { send(); });
    $(".wangEditor-textarea").keypress(function (event) {
        if (event.keyCode == 13) {
            send();
        }
    });
   

});