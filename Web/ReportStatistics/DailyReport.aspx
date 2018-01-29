<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DailyReport.aspx.cs" 
    MasterPageFile="~/ReportStatistics/ReportStatistics.master" Inherits="ReportStatistics_DailyReport" %>

<%@ Register Src="~/ascx/pagecut.ascx" TagName="pagecut" TagPrefix="uc1" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function SearchInfoList() {            
            $("#refresh").val("1");
            document.forms[0].submit();
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="nowposition">
        <%= GetGlobalResourceObject("SystemResource","OrderManagement").ToString() %> >
        <%= GetGlobalResourceObject("SystemResource","ReportStatistics").ToString() %> >
       <b><%= GetGlobalResourceObject("SystemResource","DailyReport").ToString() %>&nbsp;</b></div>  
    <div class="searchbox1">
        <input type="hidden" id="refresh" name="refresh"  value="" /> <%= GetGlobalResourceObject("SystemResource","Date").ToString() %> :
        <input id="txtdatefrom" readonly="readonly" value="<%=Request["txtdatefrom"] %>" onfocus="WdatePicker({el:'txtdatefrom',dateFmt:'yyyy-MM-dd'})" name="txtdatefrom" style="width: 140px" type="text" />
        <img onclick="WdatePicker({el:'txtdatefrom',dateFmt:'yyyy-MM-dd'})" src="../My97DatePicker/skin/datePicker.gif"style="width: 16px; height: 22px;" align="absmiddle">
            &nbsp;&nbsp;&nbsp;&nbsp;~&nbsp;&nbsp;&nbsp;&nbsp;
        <input id="txtdateto" readonly="readonly" value="<%=Request["txtdateto"] %>" onfocus="WdatePicker({el:'txtdateto',dateFmt:'yyyy-MM-dd'})" name="txtdateto" style="width: 140px" type="text" />
        <img onclick="WdatePicker({el:'txtdateto',dateFmt:'yyyy-MM-dd'})" src="../My97DatePicker/skin/datePicker.gif"style="width: 16px; height: 22px;" align="absmiddle">
        &nbsp;&nbsp;<a onclick="SearchInfoList()" href="javascript:void(0)" class="btn02"><%= GetGlobalResourceObject("SystemResource","Search").ToString() %></a>
    </div>
    <div style="height:70%;">
        <rsweb:ReportViewer ID="FinanceReportViewer" runat="server" Font-Names="Verdana" Font-Size="8pt"  Height="100%" CssClass="reportView" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
            <LocalReport ReportPath="ReportStatistics\Rdlc\DailyReport.rdlc">
            </LocalReport>
        </rsweb:ReportViewer>
     </div>
</asp:Content>

