<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HistoryTalk.aspx.cs" Inherits="OrderManagement_HistoryTalk" %>

<%@ Register Src="~/ascx/pagecut.ascx" TagName="pagecut" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        var iPageCount = 0;

        function GoPage(iPageIndex) {
            $.ajax({
                type: "GET",
                url: "HistoryTalk.aspx",
                cache: false,
                data: "type=GetJson&pageindex=" + iPageIndex + "&hPageNum=10&OrderNumber=<%=Request["OrderNumber"] %>",
                dataType: "json",
                success: function(data) {
                    //用到这个方法的地方需要重写这个success方法
                    ReturnSuccess(data, iPageIndex);
                }
            });
        }

        //jquery返回数据执行success方法
        function ReturnSuccess(data, iPageIndex) {
            //将取到的数据集放入到页面中
            var icounti = 0;
            iPageCount = data.PageCount;
            if (iPageCount>0){
                $("#dcommentbox").html("<tr><th style=\"width:150px;\"><%= GetGlobalResourceObject("Resource","TalkTime").ToString() %></th><th style=\"width:100px;\"><%= GetGlobalResourceObject("Resource","User").ToString() %></th><th><%= GetGlobalResourceObject("Resource","TalkRecord").ToString() %></th></tr>");
                $.each(data.replyJson, function (i, item) {
                    $("#dcommentbox").append("<tr><td>" + item.TalkTime + "</td><td>" + item.TalkUser + "</td><td>" + item.TalkMessage + "</td></tr>");
                    icounti++;
                });

                if (icounti == 0) {
                    $("#divPageCut").html("");
                } else {
                    SetPageCut(iPageIndex);
                }
            }
        }

        //设置显示的分页

        function SetPageCut(iPageIndex) {

            var iIndex = iPageIndex;    //当前页数
            var iCount = iPageCount == 0 ? 1 : iPageCount;    //总页数


            var strHtml = "";

            if (iCount <= 8) {

                for (var i = 1; i <= iCount; i++) {
                    if (i == iIndex) {
                        strHtml += "<span style=\"padding-left:10px\">" + i + "</span>";
                    }
                    else {
                        strHtml += "<span><a href=\"javascript:void(0)\" onclick=\"GoPage(" + i + ");\">" + i + "</a></span>";
                    }
                }
            } else {

                if (iIndex <= 5) {
                    for (var j = 1; j <= 9; j++) {
                        if (j == iIndex) {
                            strHtml += "<span style=\"padding-left:10px\">" + j + "</span>";
                        }
                        else if (j == 8) {
                            strHtml += "<span style=\"padding-left:10px\">…</span>";
                        }
                        else if (j == 9) {
                            strHtml += "<span><a href=\"javascript:void(0)\" onclick=\"GoPage(" + iCount + ");\">" + iCount + "</a></span>";
                        }
                        else {
                            strHtml += "<span><a href=\"javascript:void(0)\" onclick=\"GoPage(" + j + ");\">" + j + "</a></span>";
                        }
                    }
                }
                else if (iCount - iIndex <= 3) {
                    for (var k = 1; k <= 9; k++) {
                        if (k == 1) {
                            strHtml += "<span><a href=\"javascript:void(0)\" onclick=\"GoPage(" + k + ");\">" + k + "</a></span>";
                        }
                        else if (k == 2) {
                            strHtml += "<span style=\"padding-left:10px\">…</span>";
                        }
                        else if ((parseInt(iCount) + parseInt(k) - 9) == iIndex) {
                            strHtml += "<span style=\"padding-left:10px\">" + iIndex + "</span>";
                        }
                        else {
                            strHtml += "<span><a href=\"javascript:void(0)\" onclick=\"GoPage(" + (parseInt(iCount) + parseInt(k) - 9) + ");\">" + (parseInt(iCount) + parseInt(k) - 9) + "</a></span>";
                        }
                    }
                }
                else {
                    for (var n = 1; n <= 10; n++) {
                        if (n == 1) {
                            strHtml += "<span><a href=\"javascript:void(0)\" onclick=\"GoPage(" + n + ");\">" + n + "</a></span>";
                        }
                        else if (n == 10) {
                            strHtml += "<span><a href=\"javascript:void(0)\" onclick=\"GoPage(" + iCount + ");\">" + iCount + "</a></span>";
                        }
                        else if (n == 2 || n == 9) {
                            strHtml += "<span style=\"padding-left:10px\">…</span>";
                        }
                        else if (n == 6) {
                            strHtml += "<span  style=\"padding-left:10px\">" + iIndex + "</span>";
                        }
                        else {
                            strHtml += "<span><a href=\"javascript:void(0)\" onclick=\"GoPage(" + (parseInt(iIndex) - 6 + parseInt(n)) + ");\">" + (parseInt(iIndex) - 6 + parseInt(n)) + "</a></span>";
                        }
                    }
                }
            }
            $("#divPageCut").html(strHtml);
        }
        function CancelPage() {
            art.dialog.list['PIC031'].close();
        }
    </script>

    <style type="text/css">
       
        .tlist 
        {
            overflow:auto;
        }
         .tlist td
        {
             text-align:center;
             height:20px;
              white-space:nowrap;
        }
         .tlist th
        {
            
             white-space:nowrap;
        }

        .pagediv
        {
        	text-align:center;
        	margin:10px;
        	
        }
        .pagediv a
        {
        	padding-left:5px;
        	padding-right:5px;
        	border:1px solid #cccccc;
        	margin:3px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <input id="hsysuserid" name="hsysuserid" type="hidden" />
    <div style="height:360px; width:750px; overflow:auto;">
        <table id="dcommentbox" style="width:650px" class="tlist">
            <tr><td></td></tr>
        </table>
        
    </div>
        <div id="divPageCut" class="pagediv">
        </div>
    <div>
    <a href="javascript:void(0)" onclick="CancelPage()" class="btn01"><%= GetGlobalResourceObject("Resource","Return").ToString() %></a>
    </div>

    <script type="text/javascript">
        GoPage(1);
    </script>

    </form>
</body>
</html>
