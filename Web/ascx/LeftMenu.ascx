<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LeftMenu.ascx.cs" Inherits="ascx_LeftMenu" %>
<link href="../OrderManagement/images/main.css" type="text/css" rel="Stylesheet" />

    <div class="leftnav">
        <div <%=lntitleCss %>  id="Order" onclick="ShowModule('Order')" <%=accessOrder %>><%= GetGlobalResourceObject("Resource","OrderManage").ToString() %></div>
        <ul id="Orderul" <%=accessOrder %>>
            <li><a id="la1" href="/OrderManagement/OrderInput.aspx?type=Order"><%= GetGlobalResourceObject("Resource","OrderManage").ToString() %></a></li>
            <li><a id="la2" href="/OrderManagement/OrderList.aspx?type=Order"><%= GetGlobalResourceObject("Resource","OrderList").ToString() %></a></li>          
          
        </ul>
        <div <%=lntitleCss %> id="Information" onclick="ShowModule('Information')" <%=accessProcedure %>><%= GetGlobalResourceObject("Resource","ProcedureQuery").ToString() %></div>
        <ul id="Informationul" <%=accessProcedure %>>
            <li><a id="la3" href="/Information/ProcedureQuery.aspx?type=Information&queryaction=procedure"><%= GetGlobalResourceObject("Resource","FactoryQuery").ToString() %></a></li>
            <li><a id="la4" href="/Information/ProcedureQuery.aspx?type=Information&queryaction=patient"><%= GetGlobalResourceObject("Resource","PatientQuery").ToString() %></a></li>
            <li style="display:none;"><a id="lahide4" href="/Information/PatientQuery.aspx?type=Information">患者查询</a></li>    
        </ul>
        <div <%=lntitleCss %> id="ReportStatistics" onclick="ShowModule('ReportStatistics')" <%=accessReport %>><%= GetGlobalResourceObject("Resource","ReportStatistics").ToString() %></div>
        <ul id="ReportStatisticsul" <%=accessReport %>>
            <li><a id="la5" href="/ReportStatistics/FinanceSummaryDetail.aspx?type=ReportStatistics"><%= GetGlobalResourceObject("Resource","FinanceDetail").ToString() %></a></li>
            <li><a id="la6" href="/ReportStatistics/FinaceSummaryByHosipital.aspx?type=ReportStatistics"><%= GetGlobalResourceObject("Resource","Final2").ToString() %></a></li>
            <li><a id="la7" href="/ReportStatistics/ModelProcessingSummary.aspx?type=ReportStatistics"><%= GetGlobalResourceObject("Resource","ModelStatistics").ToString() %></a></li>
            <li><a id="la8" href="/ReportStatistics/DailyReport.aspx?type=ReportStatistics"><%= GetGlobalResourceObject("Resource","DailyReport").ToString() %></a></li>
            <li><a id="la9" href="/ReportStatistics/ReceivedAmount.aspx?type=ReportStatistics"><%= GetGlobalResourceObject("Resource","Receivable").ToString() %></a></li>   
            <li><a id="la10" href="/ReportStatistics/DebtAmount.aspx?type=ReportStatistics"><%= GetGlobalResourceObject("Resource","DebtQuery").ToString() %></a></li>   
            <li><a id="la11" href="/ReportStatistics/OrderListQuery.aspx?type=ReportStatistics"><%= GetGlobalResourceObject("Resource","OrderDetail").ToString() %></a></li>     
            <li><a id="la12" href="/ReportStatistics/ProductPrice.aspx?type=ReportStatistics"><%= GetGlobalResourceObject("Resource","PriceReport").ToString() %></a></li>
            <%if (Session["Language"] != "zh-cn") {%><li><a id="la21" href="/ReportStatistics/ConfirmDeList.aspx?type=ReportStatistics">Confirmde List</a></li> <%} %>                           
        </ul>
        <div <%=lntitleCss %> id="System" <%=accessSystem %> onclick="ShowModule('System')"><%= GetGlobalResourceObject("Resource","SystemManage").ToString() %></div>
        <ul id="Systemul" <%=accessSystem %>>
            <li><a id="la13"  href="/System/JGCList.aspx?type=System"><%= GetGlobalResourceObject("Resource","FactoryManage").ToString() %></a></li>
            <li><a id="la14" href="/System/RoleList.aspx?type=System"><%= GetGlobalResourceObject("Resource","AuthorizationList").ToString() %></a></li>
            <li><a id="la15" style="display:none" href="/System/UserManage.aspx?type=System"><%= GetGlobalResourceObject("Resource","UserManage").ToString() %></a></li>
            <li><a id="la16" href="/System/UserList.aspx?type=System"><%= GetGlobalResourceObject("Resource","UserList").ToString() %></a></li>
        </ul>
        <div <%=lntitleCss %> id="BankPay" <%=accessPay %> onclick="ShowModule('BankPay')"><%= GetGlobalResourceObject("Resource","PayManage").ToString() %></div>
        <ul id="BankPayul" <%=accessPay %>>
            <li><a id="la17"  href="/BankPay/BankPay.aspx?type=BankPay"><%= GetGlobalResourceObject("Resource","BankPay").ToString() %></a></li>  
             <li><a id="la18"  href="/BankPay/FrontRcvResponse.aspx?type=BankPay"><%= GetGlobalResourceObject("Resource","PayList").ToString() %></a></li> 
            <li><a id="la19"  target="_blank" href="https://www.alipay.com/"><%= GetGlobalResourceObject("Resource","Alipay").ToString() %></a></li>        
        </ul>
        <div <%=lntitleCss %> id="Weixin" <%=accessWeixin %> onclick="ShowModule('Weixin')"><%= GetGlobalResourceObject("Resource","WXManage").ToString() %></div>
        <ul id="Weixinul" <%=accessWeixin %>>
            <li><a id="la120"  href="#"><%= GetGlobalResourceObject("Resource","WeiXin").ToString() %></a></li>                   
        </ul>
         <script type="text/javascript">
             var s = window.location.href;
             var arguments = window.location.search
             var type = arguments.split('=')[1]
             if (type.indexOf("&") > -1)
             {
                 type = type.split("&")[0];
             }

             function ShowModule(type) {
                 //$(".leftnav ul[id=" + type + "ul]").show('slow', 'linear');
                 if ($(".leftnav div[id=" + type + "]").css("background-image").indexOf("down.png") > -1) {
                     $(".leftnav div[id=" + type + "]").css("background-image", "url('../OrderManagement/images/up.png')")
                 } else {
                     $(".leftnav div[id=" + type + "]").css("background-image", "url('../OrderManagement/images/down.png')")
                 }
                 $(".leftnav ul[id=" + type + "ul]").slideToggle("slow", function () {
                     //if ($(".leftnav ul[id=" + type + "ul]")[0].style.display == "block") {
                     //    $(".leftnav div[id=" + type + "]").css("background-image", "url('../OrderManagement/images/up.png')")
                     //} else {
                     //    $(".leftnav div[id=" + type + "]").css("background-image", "url('../OrderManagement/images/down.png')")
                     //}

                   
                 });
                
                
                 //$(".leftnav ul[id!=" + type + "ul]").animate({ height: "0px" });
                 $(".leftnav ul[id!=" + type + "ul]").hide();
                 $(".leftnav div[id!=" + type + "]").css("background-image", "url('../OrderManagement/images/down.png')")

               
             }
             
             //$(".leftnav ul[id!=" + type + "ul]").css("height", "0px");
             $(".leftnav ul[id!=" + type + "ul]").hide();
             $(".leftnav div[id=" + type + "]").css("background-image", "url('../OrderManagement/images/up.png')");
             if (s.indexOf("/OrderManagement/OrderInput.aspx") > 0) {
                 n = '1';
             } else if (s.indexOf("/OrderManagement/OrderList.aspx") > 0) {
                 n = '2';
             }
             else if (s.indexOf("/Information/ProcedureQuery.aspx") > 0 && s.indexOf("procedure") > 0) {
                 n = '3';
             } else if (s.indexOf("/Information/ProcedureQuery.aspx") > 0 && s.indexOf("procedure") == -1) {
                 n = '4';
             }
             else if (s.indexOf("/ReportStatistics/FinanceSummaryDetail.aspx") > 0) {
                 n = '5';
             } else if (s.indexOf("/ReportStatistics/FinaceSummaryByHosipital.aspx") > 0) {
                 n = '6';
             } else if (s.indexOf("/ReportStatistics/ModelProcessingSummary.aspx") > 0) {
                 n = '7';
             } else if (s.indexOf("/ReportStatistics/DailyReport.aspx") > 0) {
                 n = '8';
             }
             else if (s.indexOf("/ReportStatistics/ReceivedAmount.aspx") > 0) {
                 n = '9';
             }
             else if (s.indexOf("/ReportStatistics/DebtAmount.aspx") > 0) {
                 n = '10';
             }
             else if (s.indexOf("/ReportStatistics/OrderListQuery.aspx") > 0) {
                 n = '11';
             }
             else if (s.indexOf("OrderDetail.aspx") > 0 || s.indexOf("ProductCheckReport.aspx") > 0 || s.indexOf("OrdersElement.aspx") > 0 || s.indexOf("QSProcRec.aspx") > 0
                 || s.indexOf("EquipRec.aspx") > 0 || s.indexOf("DisinRec.aspx") > 0) {
                 n = '11';
             }
             else if (s.indexOf("/ReportStatistics/ProductPrice.aspx") > 0) {
                 n = '12';
             }
             else if (s.indexOf("/ReportStatistics/ConfirmDeList.aspx") > 0) {
                 n = '21';
             }
             else if (s.indexOf("/System/JGCList.aspx") > 0) {
                 n = '13';
             }
             else if (s.indexOf("/System/JGCManage.aspx") > 0) {
                 n = '13';
             }
             else if (s.indexOf("/System/RoleEdit.aspx") > 0) {
                 n = '14';
             } else if (s.indexOf("/System/RoleList.aspx") > 0) {
                 n = '14';
             } else if (s.indexOf("/System/UserManage.aspx") > 0) {
                 n = '16';
             } else if (s.indexOf("/System/UserList.aspx") > 0) {
                 n = '16';
             }
             else if (s.indexOf("/BankPay/BankPay.aspx") > 0) {
                 n = '17';
             }
             else if (s.indexOf("/BankPay/FrontRcvResponse.aspx") > 0) {
                 n = '18';
             }
             else if (s.indexOf("www.alipay.com") > 0) {
                 n = '19';
             }

             document.getElementById("la" + n).style.backgroundColor = "#aecfdc";
             document.getElementById("la" + n).style.color = "White";
            </script>
    </div>
    
