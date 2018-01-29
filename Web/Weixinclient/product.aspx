<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Weixinclient/App_Master/all_master.Master"   CodeFile="product.aspx.cs" Inherits="Weixinclient_product" %>
<%@ Register Src="~/Weixinclient/ascx/pagecutphone.ascx" TagName="pageweixincut" TagPrefix="uc1" %>
<%@ Register Src="~/Weixinclient/ascx/weixintop.ascx" TagName="weixintop" TagPrefix="uccontrol" %>
<%@ Register Src="~/Weixinclient/ascx/weixinbottom.ascx" TagName="weixinbottom" TagPrefix="uccontrol" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <!-- 引入ystep样式 -->
 <section >
    <div class="details" >
      <table width="100%" border="0" cellspacing="0" cellpadding="0" class="details-text">
        <tr>
          <td ><%= GetGlobalResourceObject("SystemResource","Product").ToString() %></td>
          <td ><span class="details-text-color" ><%=productsModel.ItemName %></span></td>
        </tr>
        <tr>
          <td ><%= GetGlobalResourceObject("SystemResource","Count").ToString() %></td>
          <td ><span class="details-text-color" ><%=productsModel.Number %></span></td>
        </tr>
        <tr class="tooth-position">
          <td ><%= GetGlobalResourceObject("SystemResource","ToothPosition").ToString() %></td>
          <td >&nbsp;</td>
        </tr>
        <tr>
          <td colspan="2" >
             
            <div class="tooth-position-img" >
                <div style="width:48%; text-align:right; float:left; ">&nbsp;
                    <%if (!String.IsNullOrEmpty(productsModel.righttop))
                      {
                          for (int i = 0; i < productsModel.righttop.Length; i++)
                          {%>
                    <i ><span ><%=productsModel.righttop.Substring(i, 1)%></span></i>
                <%}
                      } %>&nbsp;
                </div>
                <div style="width:48%;text-align:left;float:left;border-left:1px dashed #20d4c9;">&nbsp;
                    <%if (!String.IsNullOrEmpty(productsModel.lefttop))
                      {
                          for (int i = 0; i < productsModel.lefttop.Length; i++)
                          {%>
                        <i ><span ><%=productsModel.lefttop.Substring(i, 1)%></span></i>
                    <%}
                      } %>&nbsp;
                </div>           
            </div>
            <div class="tooth-position-img tooth-position-img1;" style="clear:both;border-top:1px dashed #20d4c9;" >
                <div style="width:48%; text-align:right;float:left;">&nbsp;
                    <%if (!String.IsNullOrEmpty(productsModel.rightbottom))
                      {
                          for (int i = 0; i < productsModel.rightbottom.Length; i++)
                          {%>
                        <i ><span ><%=productsModel.rightbottom.Substring(i, 1)%></span></i>
                    <%}
                      } %>&nbsp;
                </div>
                <div style="width:48%; text-align:left;float:left;border-left:1px dashed #20d4c9;">&nbsp;
                    <%if (!String.IsNullOrEmpty(productsModel.leftbottom))
                      {
                          for (int i = 0; i < productsModel.leftbottom.Length; i++)
                          {%>
                        <i ><span ><%=productsModel.leftbottom.Substring(i, 1)%></span></i>
                    <%}
                      } %> &nbsp;
                </div> 
            </div>
          </td>
        </tr>
      </table>
      <h3 class="details-title" ><%= GetGlobalResourceObject("SystemResource","FactoryProcess").ToString() %><i ><img src="images/xx.png" alt="" ></i></h3>
      <ul class="details-list">
          <%foreach(System.Data.DataRow dr in drZhujian) {%>
            <%if(dr["overflag"].ToString() =="F") {%>
            <li class="details-list-bg1" ><i ><img src="images/details-img1.png" alt="" ></i><%=dr["procedureName"] %><i style="float:right;margin-right:20px;"><%=dr["finishTime"]==null?"":DateTime.Parse(dr["finishTime"].ToString()).ToString("MM/dd HH:mm") %></i></li>
          <%}
              else if (dr["overflag"].ToString() == "M")
              { %>
            <li class="details-list-bg2" ><i ><img src="images/details-img1.png" alt="" ></i><%=dr["procedureName"] %></li>
          <%} else { %>
            <li class="details-list-bg3" ><i ><img src="images/details-img1.png" alt="" ></i><%=dr["procedureName"] %></li>
          <%} }%>
         <%-- <li class="details-list-bg1" ><i ><img src="images/details-img1.png" alt="" ></i>石膏</li>
          <li class="details-list-bg1" ><i ><img src="images/details-img2.png" alt="" ></i>蜡型</li>
          <li class="details-list-bg1" ><i ><img src="images/details-img3.png" alt="" ></i>铸造</li>
          <li class="details-list-bg2" ><i ><img src="images/details-img4.png" alt="" ></i>车金</li>
          <li class="details-list-bg3" ><i ><img src="images/details-img5.png" alt="" ></i>上OP</li>
          <li class="details-list-bg3" ><i ><img src="images/details-img6.png" alt="" ></i>上瓷</li>
          <li class="details-list-bg3" ><i ><img src="images/details-img7.png" alt="" ></i>上釉</li>--%>
          <li class="" ><i ><img src="images/details-img8.png" alt="" ></i>预出厂日期:<%=preOutDate %></li>
      </ul>
    </div>
  </section>
  <!--section   end-->
</asp:Content>

