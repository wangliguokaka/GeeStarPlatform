<%@ Page Title="" Language="C#" MasterPageFile="~/Weixinclient/App_Master/all_master.Master" AutoEventWireup="true" CodeFile="FinaceSummary.aspx.cs" Inherits="Weixin.FinaceSummary" %>
<%@ Register Src="~/Weixinclient/ascx/pagecutphone.ascx" TagName="pageweixincut" TagPrefix="uc1" %>
<%@ Register Src="~/Weixinclient/ascx/querycontrol.ascx" TagName="querycontrol" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<section >
   <uc2:querycontrol ID="querycontrol" runat="server" />

    <div class="search-list"  >  
      <table style="width:100%;margin:auto; text-align:center;">
          <tr style="background:#edeaea; height:40px; "><td><%= GetGlobalResourceObject("SystemResource","Product").ToString() %></td>
              <td><%= GetGlobalResourceObject("SystemResource","Count").ToString() %></td>
              <td><%= GetGlobalResourceObject("SystemResource","Money").ToString() %></td>
              <td><%= GetGlobalResourceObject("SystemResource","PreciousMetalWeight").ToString() %></td>
              <td style="display:none;"><%= GetGlobalResourceObject("SystemResource","PreciousMetalMoney").ToString() %></td></tr>
      
        <%for (int i = 0; i < dtFinaceSummary.Rows.Count; i++){%>
          <tr style="border-bottom:1px dotted ;height:40px;">
              <td style="max-width:150px;"><%=dtFinaceSummary.Rows[i]["itemname"].ToString() %></td>
              <td><%=dtFinaceSummary.Rows[i]["Qty"].ToString() %></td>
              <td><%=LoginUser.Disp_Money == "Y"?dtFinaceSummary.Rows[i]["amount"].ToString().ToDigitalString():"" %></td>
              <td><%=dtFinaceSummary.Rows[i]["NobleWeight"].ToString().ToDigitalString() %></td>
              <td style="display:none;"><%=dtFinaceSummary.Rows[i]["NobleAmount"].ToString().ToDigitalString() %></td>
          </tr>
        <%} %>
           <tr style="border-bottom:1px dotted ;height:40px;">
              <td><%= GetGlobalResourceObject("SystemResource","Total").ToString() %></td>
              <td><%=sumQty %></td>
              <td><%=LoginUser.Disp_Money == "Y"?sumAmount.ToDigitalString():"" %></td>
              <td><%=sumNobleWeight.ToDigitalString()%></td>
              <td style="display:none;"><%=sumNobleAmount.ToDigitalString()%></td>
          </tr>
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
