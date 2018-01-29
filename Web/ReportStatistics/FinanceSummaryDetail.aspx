<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FinanceSummaryDetail.aspx.cs" 
    MasterPageFile="~/ReportStatistics/ReportStatistics.master" Inherits="ReportStatistics_FinanceSummaryDetail" %>

<%@ Register Src="~/ascx/pagecut.ascx" TagName="pagecut" TagPrefix="uc1" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <script type="text/javascript">
         function addHospital() {
             var seller = $("select[id*=seller]").val();
             if (seller == "") {
                 alertdialog('<%= GetGlobalResourceObject("SystemResource","SelectSalesMan").ToString() %>！');
                 return false;
             }
             dialog = art.dialog({
                 id: 'PIC022',
                 title: "<%= GetGlobalResourceObject("SystemResource","SelectHospitalOrClinic").ToString() %>",
                 background: '#000',
                 opacity: 0.3,
                 close: function () {
                 }
             });

             $.ajax({
                 url: "SelectHospital.aspx?HospitalList=" + $("#HospitalList").val() + "&radio=0&seller=" + seller,
                 success: function (data) {
                     dialog.content(data);
                 },
                 cache: false
             });
         }

         function SearchInfoList() {
             $("#HidHospitalName").val($('#HospitalName').html());
             $("#refresh").val("1");
             document.forms[0].submit();
         }

         $(function () {
             $("#<%=seller.ClientID%>").on
             (
                 {
                     change: function () {
                         $("#HospitalList").val("");
                         $("#HidHospitalName").val("");
                         $("#HospitalName").html("");
                     }
                 }
             )

         })
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
       <div class="nowposition">
            <%= GetGlobalResourceObject("SystemResource","OrderManagement").ToString() %> >
        <%= GetGlobalResourceObject("SystemResource","ReportStatistics").ToString() %> >
       <b><%= GetGlobalResourceObject("SystemResource","SalesSettleDetails").ToString() %>&nbsp;</b></div> 
    <div class="searchbox1" >
        <input type="hidden" id="HospitalList" name="HospitalList"  value="<%=Request["HospitalList"] %>" />
        <input type="hidden" id="refresh" name="refresh"  value="" /> 
        <input type="hidden" id="HidHospitalName" name="HidHospitalName"  value="" /> 
        <%= GetGlobalResourceObject("SystemResource","SalesMan").ToString() %>:<asp:DropDownList runat="server" ID="seller"></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;
        <a class="abtn01" href="javascript:void(0)" onclick="addHospital()"><%= GetGlobalResourceObject("SystemResource","SelectHospital").ToString() %></a><label id="HospitalName" style="margin-right:20px;"><%=Request["HidHospitalName"] %></label>
       <%= GetGlobalResourceObject("SystemResource","Barcode").ToString() %>：<asp:TextBox runat="server" ID="OrderNo"></asp:TextBox> &nbsp;&nbsp;&nbsp;&nbsp;<a class="abtn01" href="javascript:void(0)" style="display:none;"><%= GetGlobalResourceObject("SystemResource","Payway").ToString() %>：</a><asp:CheckBoxList runat="server" Visible="false" RepeatDirection="Horizontal" RepeatLayout="Flow"  ID="charges" DataTextField="DictName" DataValueField="Code" ></asp:CheckBoxList>&nbsp;&nbsp;&nbsp;&nbsp;
            <a class="abtn01" href="javascript:void(0)"><%= GetGlobalResourceObject("SystemResource","OrderType").ToString() %>:</a><asp:CheckBoxList runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"  ID="orderType" DataTextField="DictName" DataValueField="Code" ></asp:CheckBoxList>
        </div>
    <div class="searchbox1"><%= GetGlobalResourceObject("SystemResource","DateToFactory").ToString() %>:
        <input type="radio" name="dateselect" id="indate" value="0" <%if(String.IsNullOrEmpty(Request["dateselect"]) || Request["dateselect"] == "0") {%>checked="checked"<%} %> /> &nbsp;&nbsp;<%= GetGlobalResourceObject("SystemResource","OutFacotry").ToString() %><input type="radio" name="dateselect" id="outdate" value="1" <%if(Request["dateselect"] == "1") {%>checked="checked"<%} %> />&nbsp;&nbsp;<input id="txtdatefrom" readonly="readonly" value="<%=Request["txtdatefrom"] %>" onfocus="WdatePicker({el:'txtdatefrom',dateFmt:'yyyy-MM-dd HH:mm:ss'})" name="txtdatefrom" style="width: 140px" type="text" />
                    <img onclick="WdatePicker({el:'txtdatefrom',dateFmt:'yyyy-MM-dd HH:mm:ss'})" src="../My97DatePicker/skin/datePicker.gif"style="width: 16px; height: 22px;" align="absmiddle">
                     &nbsp;&nbsp;&nbsp;&nbsp;~&nbsp;&nbsp;&nbsp;&nbsp;
                    <input id="txtdateto" readonly="readonly" value="<%=Request["txtdateto"] %>" onfocus="WdatePicker({el:'txtdateto',dateFmt:'yyyy-MM-dd HH:mm:ss'})" name="txtdateto" style="width: 140px" type="text" />
                    <img onclick="WdatePicker({el:'txtdateto',dateFmt:'yyyy-MM-dd HH:mm:ss'})" src="../My97DatePicker/skin/datePicker.gif"style="width: 16px; height: 22px;" align="absmiddle">
                    &nbsp;&nbsp;&nbsp;&nbsp;<%= GetGlobalResourceObject("SystemResource","ReadyOut").ToString() %>:<input type="radio" name="dateselect" id="preoutdate" value="2" <%if(Request["dateselect"] == "2") {%>checked="checked"<%} %> />&nbsp;&nbsp;<input id="txtpredate" readonly="readonly"  value="<%=Request["txtpredate"] %>" onfocus="WdatePicker({el:'txtpredate',dateFmt:'yyyy-MM-dd'})" name="txtpredate" style="width: 140px" type="text" />
                    <img onclick="WdatePicker({el:'txtpredate',dateFmt:'yyyy-MM-dd'})" src="../My97DatePicker/skin/datePicker.gif"style="width: 16px; height: 22px;" align="absmiddle"><a onclick="SearchInfoList()" href="javascript:void(0)" class="btn02"><%= GetGlobalResourceObject("SystemResource","Search").ToString() %></a>
        </div>
       <div style="height:65%;">
               <rsweb:ReportViewer ID="FinanceReportViewer" CssClass="reportView" runat="server" PageCountMode="Actual"  Font-Names="Verdana" Font-Size="8pt"  Height="90%"  WaitMessageFont-Names="Verdana"  WaitMessageFont-Size="14pt">
                <LocalReport  ReportPath="ReportStatistics\Rdlc\MergeFinanceSummaryReport.rdlc" >
                </LocalReport>
                   
            </rsweb:ReportViewer>
        </div> 
        
</asp:Content>

