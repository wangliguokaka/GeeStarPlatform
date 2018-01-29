<%@ Page Title="" Language="C#" MasterPageFile="~/Weixinclient/App_Master/all_master.Master" AutoEventWireup="true" CodeFile="SmallClassSummary.aspx.cs" Inherits="Weixin.SmallClassSummary" %>
<%@ Register Src="~/Weixinclient/ascx/pagecutphone.ascx" TagName="pageweixincut" TagPrefix="uc1" %>
<%@ Register Src="~/Weixinclient/ascx/querycontrolSC.ascx" TagName="querycontrolSC" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<section >
   <uc2:querycontrolSC ID="querycontrolSC" runat="server" />

    <div class="search-list"  >  
      <table style="width:100%;margin:auto; text-align:center;">
          <tr style="background:#edeaea; height:40px;font-size:14px; "><td><%= GetGlobalResourceObject("SystemResource","SmallClass").ToString() %></td>
              <td><%=LoginUser.Disp_Money == "Y"? GetGlobalResourceObject("SystemResource","AOrderClass").ToString():IsCN?"正常(数量)":"Normal(Qty)" %></td>
              <td><%= GetGlobalResourceObject("SystemResource","BOrderClass").ToString() %></td>
              <td><%= GetGlobalResourceObject("SystemResource","COrderClass").ToString() %></td>
              <td><%= GetGlobalResourceObject("SystemResource","DOrderClass").ToString() %></td></tr>
      
        <%for (int i = 0; i < dtFinaceSummary.Rows.Count && dtFinaceSummary.Rows.Count>1; i++)
          {%>
          <tr style="border-bottom:1px dotted ;height:40px;">
              <td><%=ConvertSmallClass(dtFinaceSummary.Rows[i]["SmallClass"].ToString()) %></td>
               <td style="font-size:12px;"><%=LoginUser.Disp_Money == "Y"?dtFinaceSummary.Rows[i]["AOrderClass"].ToString():dtFinaceSummary.Rows[i]["AOrderClass"].ToString().IndexOf("/")==-1?"":dtFinaceSummary.Rows[i]["AOrderClass"].ToString().Substring(0,dtFinaceSummary.Rows[i]["AOrderClass"].ToString().IndexOf("/")) %></td>
               <td style="font-size:12px;"><%=LoginUser.Disp_Money == "Y"?dtFinaceSummary.Rows[i]["BOrderClass"].ToString():dtFinaceSummary.Rows[i]["BOrderClass"].ToString().IndexOf("/")==-1?"":dtFinaceSummary.Rows[i]["BOrderClass"].ToString().Substring(0,dtFinaceSummary.Rows[i]["BOrderClass"].ToString().IndexOf("/")) %></td>
               <td style="font-size:12px;"><%=LoginUser.Disp_Money == "Y"?dtFinaceSummary.Rows[i]["COrderClass"].ToString():dtFinaceSummary.Rows[i]["COrderClass"].ToString().IndexOf("/")==-1?"":dtFinaceSummary.Rows[i]["COrderClass"].ToString().Substring(0,dtFinaceSummary.Rows[i]["COrderClass"].ToString().IndexOf("/")) %></td>
               <td style="font-size:12px;"><%=LoginUser.Disp_Money == "Y"?dtFinaceSummary.Rows[i]["DOrderClass"].ToString():dtFinaceSummary.Rows[i]["DOrderClass"].ToString().IndexOf("/")==-1?"":dtFinaceSummary.Rows[i]["DOrderClass"].ToString().Substring(0,dtFinaceSummary.Rows[i]["DOrderClass"].ToString().IndexOf("/")) %></td>
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
