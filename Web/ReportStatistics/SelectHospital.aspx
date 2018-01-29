<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectHospital.aspx.cs" Inherits="ReportStatistics_SelectHospital" %>

<%@ Register Src="~/ascx/pagecut.ascx" TagName="pagecut" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <script type="text/javascript">
        var iPageCount = 0;
        var inputType = ""
        var radio = '<%=Request["radio"]%>';
        var seller = '<%=Request["seller"]%>'
        if (radio == "0") {
            inputType = "checkbox";
        }
        else {
            inputType = "radio";
        }
        function GoPage(iPageIndex) {
           
            $.ajax({
                type: "GET",
                url: "SelectHospital.aspx",
                cache: false,
                data: "type=GetJson&pageindex=" + iPageIndex + "&hPageNum=" + iPageCount + "&cid=<%=cid %>&seller=" + seller,
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
            $("#dcommentbox").html("<tr><th style=\"text-align:center;\"><%= GetGlobalResourceObject("SystemResource","Select").ToString() %></th><th style=\"text-align:center;\"><%= GetGlobalResourceObject("SystemResource","HospitalOrClinic").ToString() %></th></tr>");
           
            $.each(data.DataHospital, function (i, item) {
                
                $("#dcommentbox").append("<tr><td><input type=\"" + inputType + "\" onclick=\"selsysuser('" + item.HospitalID + "','" + item.Hospital + "')\" name=\"radiocheck\" id=" + item.HospitalID + " /></td><td>\
                " + item.Hospital + "</td></tr>");
                icounti++;
            });
            //加载完评论后，应当重新设置分页

            if (icounti == 0) {
                $("#divPageCut").html("");
            } else {
                SetPageCut(iPageIndex);
            }

            //如果有已经选择的成员，标记一下

            var selusers = $("#hsysuserid").val().split(":");

            if (selusers == "") {
                selusers = '<%=Request["HospitalList"]%>';
                $("#hsysuserid").val(selusers);
                selusers = selusers.split(":");
            }
            for (var i = 0; i < selusers.length; i++) {
                if (document.getElementById(selusers[i]) != null) {
                    document.getElementById(selusers[i]).checked = true;
                }

            }
        }
        var checkedFee = 0;    
        var Hospital = $("#HospitalName").html()
        if (Hospital != "") {
            Hospital = "+" + Hospital;
        }
        function selsysuser(sysuserid, itemHospital) {
            var selusers = $("#hsysuserid").val();
            if (document.getElementById(sysuserid).checked == true) {
                if (radio == "1") {
                    Hospital = "";
                    selusers = "";
                }
                selusers = selusers + ":" + sysuserid;
               
                Hospital = Hospital + "+" + itemHospital;
            } else {
                selusers = ":" + selusers + ":";
                selusers = selusers.replace(":" + sysuserid + ":", ":");
                Hospital = Hospital.replace("+" + itemHospital, "");
            }
            if (selusers.length > 0) {
                if (selusers.indexOf(":") == 0) {
                    selusers = selusers.substr(1, selusers.length - 1);
                   
                }
                if (selusers.lastIndexOf(":") == selusers.length - 1) {
                    selusers = selusers.substr(0, selusers.length - 1);
                    
                }
            }          
           
    
            $("#hsysuserid").val(selusers);
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
        function setMember() {
            $("#HospitalList").val($("#hsysuserid").val());
            if (Hospital.indexOf("+") == 0) {
                Hospital = Hospital.substr(1, Hospital.length - 1);
            }
            $("#HospitalName").html(Hospital);
            art.dialog.list['PIC022'].close();
        }

        function CancelPage() {
            art.dialog.list['PIC022'].close();
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <input id="hsysuserid" name="hsysuserid" type="hidden" value="" />
    <div style="width:400px;height:500px; overflow-x:hidden;">
        <table id="dcommentbox" class="tlist">
            <tr><td></td></tr>
        </table>
        <div id="divPageCut" style="display:none;" class="pagediv">
        </div>
    </div>
    <div>
    <a href="javascript:void(0)" onclick="setMember();" class="btn01"><%= GetGlobalResourceObject("SystemResource","Confirm").ToString() %></a>
    <a href="javascript:void(0)" onclick="CancelPage()" class="btn01"><%= GetGlobalResourceObject("SystemResource","Cancel").ToString() %></a>
    </div>

    <script type="text/javascript">
        GoPage(1);
    </script>

    </form>
</body>
</html>
