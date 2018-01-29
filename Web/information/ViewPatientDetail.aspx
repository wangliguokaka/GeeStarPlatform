<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewPatientDetail.aspx.cs" Inherits="GeeStar_ViewPatientDetail" %>

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
                url: "ViewPatientDetail.aspx",
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
            //$("#dcommentbox").html("");
            $.each(data.DataPatient, function (i, item) {
                $("#softCompany").html("<%=LoginUser.JGCName%>");
                $("#ModelNo").html(item.ModelNo);
                $("#OrderID").html(item.Order_ID);
                $("#Area").html(item.Area);
                $("#Patient").html(item.patient);
                $("#Hospital").html(item.hospital);
                $("#ProductType").html(item.productName);
                $("#Doctor").html(item.doctor);

                $("#Element").html(item.Element);

                $("#Color").html(item.bColor);
                $("#OutDate").html(item.OutDate);
                $("#Valid").html(item.Valid);
                $("#Telephone").html(item.tel);
                $("#TeethPosition").html("<table class=\"teethTable\" ><tr style=\"height:15px !important;line-height:15px;\"><td style=\"height:15px !important;line-height:15px;\" class=\"td1\">" + item.a_teeth + "</td><td style=\"height:15px !important;line-height:15px;\" class=\"td2\">" + item.b_teeth + "</td></tr><tr style=\"height:15px !important;line-height:15px;\"><td style=\"height:15px !important;line-height:15px;\" class=\"td3\">" + item.c_teeth + "</td><td style=\"height:15px !important;line-height:15px;\" class=\"td4\">" + item.d_teeth + "</td></tr></table>");
                $("#Checker").html(item.QlyName);
                $("#CheckDate").html(item.QlyTime);
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
            art.dialog.list['PIC012'].close();
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

        .tlist tr {
         height:40px;
        }
        #TeethPosition tr {
            height:5px;
        }
        .teethTable {
          
             /*border-collapse: separate;*/
              width:50%;     
              height:10px;  

             
        }
         .teethTable tr {
              height:10px  !important;         
        }
         .teethTable .td1 {
             line-height:10px !important;
             height:10px  !important;
             border-bottom-style:solid;
             border-right-style:solid;
             border-bottom-color:black;
             border-right-color:black;
             border-width:1px;
              width:50%;
              text-align:right;
             
        }

          .teethTable .td2 {
              height:10px  !important;
             border-bottom-style:solid;
             border-bottom-color:black;
             border-width:1px;
              width:50%;
               text-align:left;
        }

           .teethTable .td3 {
               height:10px  !important;
             border-right-style:solid;
          
             border-right-color:black;
              width:50%;
              text-align:right;
        }

            .teethTable .td4 {
                  text-align:left;
           height:10px  !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <input id="hsysuserid" name="hsysuserid" type="hidden" />
    <div style="height:430px;">
        <table id="dcommentbox" style="width:700px;"  class="tlist">
            <tr>
                <td style="width:100px;"><%= GetGlobalResourceObject("SystemResource","MadeFrom").ToString() %>：</td>
                <td style="width:200px;" id="softCompany"></td>
                <td style="width:100px;"><%= GetGlobalResourceObject("SystemResource","AssistCode").ToString() %>：</td>
                <td id="ModelNo"></td>
            </tr>
            <tr>
                <td><%= GetGlobalResourceObject("SystemResource","BarCode").ToString() %>：</td>
                <td id="OrderID"></td>
                <td><%= GetGlobalResourceObject("SystemResource","District").ToString() %>：</td>
                <td id="Area"></td>
            </tr>
             <tr>
                <td><%= GetGlobalResourceObject("SystemResource","Patient").ToString() %>：</td>
                <td id="Patient"></td>
                  <td><%= GetGlobalResourceObject("SystemResource","PhoneNumber").ToString() %>：</td>
                <td id="Telephone"></td>
               
            </tr>
             <tr>
                <td><%= GetGlobalResourceObject("SystemResource","Hospital").ToString() %>：</td>
                <td id="Hospital"></td>
                <td><%= GetGlobalResourceObject("SystemResource","Doctor").ToString() %>：</td>
                <td id="Doctor"></td>
            </tr>
             <tr>
                <td><%= GetGlobalResourceObject("SystemResource","DominantSector").ToString() %>：</td>
                <td id="Element"></td>
                <td><%= GetGlobalResourceObject("SystemResource","Color").ToString() %>：</td>
                <td id="Color"></td>
            </tr>
             <tr>
                <td><%= GetGlobalResourceObject("SystemResource","LeaveFactoryDate").ToString() %>：</td>
                <td id="OutDate"></td>
                <td><%= GetGlobalResourceObject("SystemResource","ExpirationTime").ToString() %>：</td>
                <td id="Valid"></td>
            </tr>
            <tr>
                <td><%= GetGlobalResourceObject("SystemResource","Type").ToString() %>：</td>
                <td colspan="3" style="text-align:left;" id="ProductType"></td>
             </tr>
            <tr style="height:20px;">
                <td><%= GetGlobalResourceObject("SystemResource","ToothPosition").ToString() %>：</td>
                <td colspan="3" id="TeethPosition"></td>
            </tr>
            <tr>
                <td><%= GetGlobalResourceObject("SystemResource","Inspector").ToString() %>：</td>
                <td id="Checker"></td>
                <td><%= GetGlobalResourceObject("SystemResource","CheckDate").ToString() %>：</td>
                <td id="CheckDate"></td>
            </tr>
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
