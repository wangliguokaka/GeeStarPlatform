<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReceivedAmount.aspx.cs" 
    MasterPageFile="~/ReportStatistics/ReportStatistics.master" Inherits="ReportStatistics_ReceivedAmount" %>

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
             //if ($("#HospitalList").val() == "") {
             //    alertdialog('请选择医院！');
             //    return false;
             //}
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
    <div class="nowposition">
        <%= GetGlobalResourceObject("SystemResource","OrderManagement").ToString() %> >
        <%= GetGlobalResourceObject("SystemResource","ReportStatistics").ToString() %> >
       <b><%= GetGlobalResourceObject("SystemResource","AccountsReceivableQueries").ToString() %>&nbsp;</b></div> 
      <div class="searchbox1" >
             <input type="hidden" id="HospitalList" name="HospitalList"  value="<%=Request["HospitalList"] %>" />
        <input type="hidden" id="refresh" name="refresh"  value="" /> 
        <input type="hidden" id="HidHospitalName" name="HidHospitalName"  value="" /> <%= GetGlobalResourceObject("SystemResource","SalesMan").ToString() %>:
            <asp:DropDownList runat="server" ID="seller"></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;
        <a class="abtn01" href="javascript:void(0)" onclick="addHospital()"><%= GetGlobalResourceObject("SystemResource","SelectHospital").ToString() %>:</a><label id="HospitalName" style="margin-right:20px;"><%=Request["HidHospitalName"] %></label>  
         <a onclick="SearchInfoList()" href="javascript:void(0)" class="btn02" style="margin-right:20px;"><%= GetGlobalResourceObject("SystemResource","Search").ToString() %></a>
              </div>
     <div style="height:70%;">
           <rsweb:ReportViewer ID="FinanceReportViewer" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="100%" CssClass="reportView" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
            <LocalReport ReportPath="ReportStatistics\Rdlc\ReceivedAmount.rdlc">
            </LocalReport>
        </rsweb:ReportViewer>
     </div>
&nbsp;<asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
  
        
</asp:Content>

