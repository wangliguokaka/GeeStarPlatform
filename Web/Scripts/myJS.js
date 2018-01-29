/// <reference path="jquery-1.3.2.min.js" >
$(document).ready(function () {
    function send() {

        var userSend = $("#userSender").val();
        alert($("#content").val());
         var array = new Array();
         array[0] = userSend;
          array[1] = $("#content").val();

          //向comet_broadcast.asyn发送请求，消息体为文本框content中的内容，请求接收类为AsnyHandler

        $.post("comet_broadcast.asyn", {userID:userSend, content: $("#content").val() });

        //清空内容
        $("#content").val("");
    }
    
    function wait() {
        $.post("comet_broadcast.asyn", { content: "-1" },
         function (data, status) {
             var result = $("#divResult");
             result.html(result.html() + "<br/>" +data);

             //服务器返回消息,再次立连接
             wait();
         }, "html"
         );
    }

    //初始化连接
    wait();


    

    $("#btnSend").click(function () { send(); });
    $("#content").keypress(function (event) {
        if (event.keyCode == 13) {
            send();
        }
    });


});