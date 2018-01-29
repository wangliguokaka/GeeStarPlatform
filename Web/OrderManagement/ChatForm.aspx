<%@ Page Language="C#" EnableViewStateMac="false" Inherits="PageBase" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        #divResult
        {
             border:1px solid #000;
             width:250px;
             border-top:none; 
        }

        #divOpenTalkHistory
        {
             border:1px solid #000;
             width:250px;
             border-bottom:none; width:100%; height:20px; cursor:pointer; font-size:12px; text-align:center; color:blue;
        }

        .wangEditor-container .wangEditor-btn-container 
        {
            content:"";
            clear:both;
	        display:block !important;
            height:30px;
        }
    </style>

     <script src="/OrderManagement/js/jquery-1.10.2.min.js" type="text/javascript"></script> 
    <script src="/OrderManagement/js/all.js?rnd=1" type="text/javascript"></script>
    <script src="/OrderManagement/js/myJS.js?rnd=1" type="text/javascript"></script>
    <script src="/artDialog/jquery.artDialog.source.js?skin=green" type="text/javascript"></script>  
    <script type="text/javascript" src="../jQuery/jquery.form.js"></script>
    <script type="text/javascript" src="../jQuery/browser.js"></script>   
    <script type="text/javascript" src="/OrderManagement/js/wangEditor-1.3.0.min.js?rnd=12"></script>
    <link href="/OrderManagement/images/wangEditor-1.3.0.min.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        $(document).ready(function () {
            //document.getElementById("content").focus();


            $('#content').wangEditor({
                'menuConfig': [['fontFamily', 'fontSize'], ['bold', 'underline', 'italic'], ['justifyLeft', 'justifyCenter', 'justifyRight'], ['insertExpression'], ['capturePic', 'uploadImg']],
                'uploadUrl': '/OrderManagement/uploadFile.ashx'
            });

           
            //$('#content').focus();
           
        });

        function closeDialog()
        {
            $("#talk" + OrderNumber, opener.window.document).attr("disabled", false);
            opener.window.document.getElementById("talk" + OrderNumber).style.color = "#0098e3";            
        }

    </script>
</head>
<body onunload="closeDialog()">
<form id="form1" runat="server" method="post" enctype="multipart/form-data" >
<input type="hidden" id="OrderNumber" />
<input type="hidden" id="BelongFactory" />
<input type="hidden" id="UserName" />
<input id="fileCaptureImg" type="hidden" name="fileCaptureImg" />
<div id="divHidden"></div>
    <object id="myActiveX" classid="clsid:1D93E31E-5C9A-494B-A0B9-9C5FE6EB38A0" style="display:none;" codebase="../CaptureImageActivex.cab" >	</object>


<script type="text/javascript" language="javascript">
    String.prototype.replaceAll  = function(s1,s2){    
        return this.replace(new RegExp(s1,"gm"),s2);    
    } 
    
    function sendContent(keyCode) {
        if (keyCode == 13) {
        var userName = $("#UserName").val();
       
        var OrderNumber = $("#OrderNumber").val();
        var BelongFactory = $("#BelongFactory").val();
            //向comet_broadcast.asyn发送请求，消息体为文本框content中的内容，请求接收类为AsnyHandler
     
        if ($(".wangEditor-textarea").html().replaceAll("&nbsp;", "").replaceAll(" ", "")!= "<p></p><p></p>")
        {
            $.post("comet_broadcast.asyn", { OrderNumber: OrderNumber, BelongFactory:BelongFactory,username: userName, content: $(".wangEditor-textarea").html().replaceAll("<p>", "").replaceAll("</p>", "") });
        }
        //清空内容
        $("#content").val("");
        $(".wangEditor-textarea").html("");
        return false;
        }
    }

    function OpenTalkHistory() {

        var talkid = $("#OrderNumber").val();
        var BelongFactory = $("#BelongFactory").val();

        dialog = art.dialog({
            id: 'PIC031',
            title: '<%= GetGlobalResourceObject("Resource","TalkHistory").ToString() %>',
            background: '#000',
            width: 800,
            height: 400,
            opacity: 0.3,

            close: function () {
            }
        });

        $("#record" + talkid).html("<%= GetGlobalResourceObject("Resource","TalkRecord").ToString() %>");
        $("#record" + talkid).css("color", "#0098e3");

        $.ajax({
            url: "OrderListQuery.aspx?OrderNumber=" + talkid + "&action=ReadTalkRecord",
            success: function (data) {
                setCookie(BelongFactory+talkid, data)
                },
                cache: false
            })

        $.ajax({
            url: "HistoryTalk.aspx?OrderNumber=" + talkid,
            success: function (data) {
                dialog.content(data);
            },
            cache: false
        });
    }

    function Capture(editor) {
        var obj = document.getElementById("myActiveX");
        var timestamp = Date.parse(new Date());
        var captureimg = obj.ShowMessage();
        var random = timestamp + Math.floor(Math.random() * (1000 + 1));
        //        $("#fileCaptureImg").val(encodeURIComponent(captureimg));
        fnPreHandle(encodeURIComponent(captureimg))
        $('#form1').ajaxSubmit({
            url: "../Handler/SaveCaptureFile.ashx?random=" + random,
            type: 'post',
            success: function (data) {
                if (data != "") {
                    editor.command(null, 'customeInsertImage', { 'url': data, 'title': "" }, null);
                }
            },
            error: function (data) {
               // alert(data);

            }
        });
    };

    //数据拆分，并放到相应的hidden域中，在Form的onSubmit事件中激发     
    function fnPreHandle(postValue) {
        var iCount; //拆分为多少个域     
        var strData; //原始数据     
        var iMaxChars = 50000; //考虑到汉字为双字节，域的最大字符数限制为50K     
        var iBottleNeck = 2000000; //如果文章超过2M字，需要提示用户     
        var strHTML;
                //原始数据     
        strData = postValue;

        //如果文章实在太长，需要提醒用户     
//        if (strData.length > iBottleNeck) {
//            if (confirm("您要发布的文章太长，建议您拆分为几部分分别发布。\n如果您坚持提交，注意需要较长时间才能提交成功。\n\n是否坚持提交？") == false)
//                return false;
//        }

        iCount = parseInt(strData.length / iMaxChars) + 1;

        //hdnCount记录原数据域拆分为多少个子域     
        strHTML = "<input type=hidden name=hdnCount value=" + iCount + ">";

        //生成各子域的HTML代码     
        for (var i = 1; i <= iCount; i++) {
            strHTML = strHTML + "\n" + "<input type=hidden name=fileCaptureImg" + i + ">";
        }

        //在Form中DIV(divHidden)内动态插入各hidden域的HTML代码
        document.all.divHidden.innerHTML = "";
        document.all.divHidden.innerHTML = strHTML;

        //给各子域赋值     
        for (var i = 1; i <= iCount; i++) {
            document.getElementsByName("fileCaptureImg" + i)[0].value = strData.substring((i - 1) * iMaxChars, i * iMaxChars);
        }

        //原数据域清空     
        strData = "";
    } 
</script>
    <%--<div id="talkWindow" style="float:left;">--%>
            <div id="divOpenTalkHistory" onclick="return OpenTalkHistory()" >==<%= GetGlobalResourceObject("Resource","OpenRecordList").ToString() %>==</div>
            <div id="divResult" style="width:100%; height:260px; word-break:break-all; overflow-y:scroll;"></div>
            <div style="width:80%; margin-top:10px;float:left; "><textarea  style=" height:190px;" id="content"  name="content" ></textarea></div><div style="float:left;margin-top:10px; width:18%; margin-left:10px;"><input type="button" style="width:100%; height:190px;" id="btnSend" value='<%= GetGlobalResourceObject("Resource","Send").ToString() %>' /></div>
    <%--</div>--%>
    <%--<div id="talkHistoryWindow" style="float:left;">
        <iframe id="iframeTalkHistory" src="HistoryTalk.aspx" frameborder="0" width="750px" height="360px"></iframe>
    </div>--%>
        
      <script type="text/javascript">
         var OrderNumber = '<%=Request["OrderNumber"] %>';
         document.getElementById("OrderNumber").value = OrderNumber;
         document.getElementById("UserName").value = '<%=this.UserName %>';
          document.getElementById("BelongFactory").value = '<%=LoginUser.BelongFactory%>'
     </script>
     </form>
</body>
</html>