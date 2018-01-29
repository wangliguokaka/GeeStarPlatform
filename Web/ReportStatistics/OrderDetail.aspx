<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderDetail.aspx.cs" 
    MasterPageFile="~/ReportStatistics/ReportStatistics.master" Inherits="ReportStatistics_Detail" %>

<%@ Register Src="~/ascx/pagecut.ascx" TagName="pagecut" TagPrefix="uc1" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <div class="nowposition">
        <%= GetGlobalResourceObject("SystemResource","OrderManagement").ToString() %> >
        <%= GetGlobalResourceObject("SystemResource","ReportStatistics").ToString() %> >
       <b><%= GetGlobalResourceObject("SystemResource","OrderDetails").ToString() %>&nbsp;</b></div> 
    <div style="height:80%">
        <rsweb:ReportViewer ID="FinanceReportViewer" runat="server" Font-Names="Verdana" Font-Size="8pt" CssClass="reportView" Height="100%" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
            <LocalReport ReportPath="ReportStatistics\Rdlc\OrderDetail.rdlc">
            </LocalReport>
        </rsweb:ReportViewer>
    </div>
&nbsp;<asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
&nbsp;    
        
</asp:Content>

