<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewProcedure.aspx.cs" Inherits="GeeStar_ViewProcedure" %>

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
                url: "ViewProcedure.aspx",
                cache: false,
                data: "type=GetJson&pageindex=" + iPageIndex + "&hPageNum=" + iPageCount + "&orderID=<%=orderID %>",
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

            iPageCount = data.PageCount;
            var icounti = 0;
            $("#dcommentbox").html("");
            $.each(data.DataZhiJian, function (i, item) {
                $("#dcommentbox").append("<tr class=\"trClass\"><td><%= GetGlobalResourceObject("SystemResource","BarCode").ToString() %>：" + item.Order_ID + "</td><td><%= GetGlobalResourceObject("SystemResource","Product").ToString() %>：" + item.ProductName + "</td><td><%= GetGlobalResourceObject("SystemResource","ByThePiece").ToString() %>：" + item.itemQty + "</td><td><%= GetGlobalResourceObject("SystemResource","Department").ToString() %>：" + item.dept + "</td><td><%= GetGlobalResourceObject("SystemResource","UpRight").ToString() %>：" + item.a_teeth + "</td><td><%= GetGlobalResourceObject("SystemResource","UpLeft").ToString() %>：" + item.b_teeth + "</td><td rowspan=\"2\">" + item.Upweb + "</td></tr>");
                $("#dcommentbox").append("<tr class=\"trClass\"><td><%= GetGlobalResourceObject("SystemResource","Procedure").ToString() %>：" + item.procedureName + "</td><td><%= GetGlobalResourceObject("SystemResource","MadeBy").ToString() %>：" + item.producer + "</td><td><%= GetGlobalResourceObject("SystemResource","Receive").ToString() %>：" + item.getin + "</td><td><%= GetGlobalResourceObject("SystemResource","Complete").ToString() %>：" + item.finishTime + "</td><td><%= GetGlobalResourceObject("SystemResource","DownRight").ToString() %>：" + item.c_teeth + "</td><td><%= GetGlobalResourceObject("SystemResource","DownLeft").ToString() %>：" + item.d_teeth + "</td></tr>");
                $("#dcommentbox").append("<tr><td colspan=\"7\"></td></tr>");
                icounti++;
            });
            
            //加载完评论后，应当重新设置分页

            if (icounti == 0) {
                $("#divPageCut").html("");
            } else {
                SetPageCut(iPageIndex);
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
        function BackPage() {
           
         
        }
    </script>

    <style type="text/css">
        .tablestyle
        {
            border: none;
        }
        .tablestyle td
        {
            text-align: center;
            line-height: 18px;
            border: none;
            border-bottom: 1px solid #CECECE;
        }
        .tablestyle th
        {
            line-height: 25px;
            border: none;
            border-bottom: 1px solid #CECECE;
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
        .trClass {
            background-color:#bdd6ed;
            height:25px;
        }

        
         .uploadStatus td {
            width:2px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <input id="hsysuserid" name="hsysuserid" type="hidden" />
    <div style="height:630px;">
        <table id="dcommentbox" style="width:900px;" class="tlist">
        </table>
        <div id="divPageCut" class="pagediv">
        </div>
    </div>
    <div>
    <a href="javascript:void(0)" onclick="BackPage();" class="btn01"><%= GetGlobalResourceObject("SystemResource","Return").ToString() %></a>
    </div>

    <script type="text/javascript">
        GoPage(1);
    </script>

    </form>
</body>
</html>
