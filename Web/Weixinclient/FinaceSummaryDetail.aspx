<%@ Page Title="" Language="C#" MasterPageFile="~/Weixinclient/App_Master/all_master.Master" AutoEventWireup="true" CodeFile="FinaceSummaryDetail.aspx.cs" Inherits="Weixin.FinaceSummaryDetail" %>
<%@ Register Src="~/Weixinclient/ascx/pagecutphone.ascx" TagName="pageweixincut" TagPrefix="uc1" %>
<%@ Register Src="~/Weixinclient/ascx/querycontrol.ascx" TagName="querycontrol" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<section >
    <uc2:querycontrol ID="querycontrol" runat="server" />
    <div class="search-list"  >  
      <table style="width:100%;margin:0 auto; font-size:12px; text-align:center;">
          <tr style="background:#edeaea; height:40px; "><td>
              <%= GetGlobalResourceObject("SystemResource","BarCode").ToString() %>/<%= GetGlobalResourceObject("SystemResource","Doctor").ToString() %><br />/<%= GetGlobalResourceObject("SystemResource","Patient").ToString() %></td>
              <td><%= GetGlobalResourceObject("SystemResource","ToFactory").ToString() %>/<%= GetGlobalResourceObject("SystemResource","OutFacotry").ToString() %><br />/<%= GetGlobalResourceObject("SystemResource","Category").ToString() %></td>
              <td><%= GetGlobalResourceObject("SystemResource","Product").ToString() %>/<%= GetGlobalResourceObject("SystemResource","Count").ToString() %><br />/<%= GetGlobalResourceObject("SystemResource","PreciousMetalWeight").ToString() %></td>
              <td><%= GetGlobalResourceObject("SystemResource","ToothPosition").ToString() %></td>
              <td><%= GetGlobalResourceObject("SystemResource","Price").ToString() %><br /><%= GetGlobalResourceObject("SystemResource","Money").ToString() %>/</td></tr>
      
        <%for (int i = 0; i < dtFinaceSummary.Rows.Count; i++){%>
          <tr><td ><b>医疗机构：</td><td colspan="4" style="text-align:left; padding-left:20px;"><b><%=dtFinaceSummary.Rows[i]["hospital"]%></b></td></tr>
          <tr style="border-bottom:1px dotted ;height:40px;">
              <td style="width:20%;"><%=dtFinaceSummary.Rows[i]["Order_ID"].ToString() %><br/><%=dtFinaceSummary.Rows[i]["doctor"].ToString() %><br/><%=dtFinaceSummary.Rows[i]["patient"].ToString() %></td>
              <td style="width:20%;"><%=dtFinaceSummary.Rows[i]["indate"].ToString() %><br/><%=dtFinaceSummary.Rows[i]["Outdate"].ToString() %><br/><%=dtFinaceSummary.Rows[i]["orderclassname"].ToString() %></td>
              <td style="width:30%;"><%=dtFinaceSummary.Rows[i]["products_itemname"].ToString() %><br/><%=dtFinaceSummary.Rows[i]["qty"].ToString() %><br/><%=dtFinaceSummary.Rows[i]["NobleWeight"].ToString().ToDigitalString() %></td>
              <td style="width:20%;">
                  <table style="width:80%;">
                    <tr><td style="border-bottom:solid 1px;border-right:solid 1px; width:50%;"><%=dtFinaceSummary.Rows[i]["a_teeth"].ToString() %>&nbsp;</td><td><%=dtFinaceSummary.Rows[i]["b_teeth"].ToString() %>&nbsp;</td></tr>
                    <tr><td><%=dtFinaceSummary.Rows[i]["c_teeth"].ToString() %>&nbsp;</td><td style="border-left:solid 1px;border-top:solid 1px;"><%=dtFinaceSummary.Rows[i]["d_teeth"].ToString() %>&nbsp;</td></tr>
                  </table>
              </td>
              <td><%=LoginUser.Disp_Money == "Y"?dtFinaceSummary.Rows[i]["price"].ToString().ToDigitalString():""%><br/><%=LoginUser.Disp_Money == "Y"?dtFinaceSummary.Rows[i]["amount"].ToString().ToDigitalString():"" %></td>
          </tr>
        <%} %>
          <%if (LoginUser.Disp_Money == "Y") 
              {%>
           <tr style="border-bottom:1px dotted ;height:40px;">
              <td>合计</td>
              <td>&nbsp</td>
              <td>&nbsp</td>
              <td>&nbsp</td>
              <td><%=sumAmount.ToDigitalString()%></td>
          </tr>
          <%} %>
        </table>
      <!--search-div end-->
      <div class="clear" ></div>
    </div>
    <!--earch-list end-->
    <ul class="page" >
       <uc1:pageweixincut ID="pagecutID" runat="server" />
    </ul>
  </section>
  <!--section   end-->
</asp:Content>
