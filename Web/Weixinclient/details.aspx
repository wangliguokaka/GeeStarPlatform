<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Weixinclient/App_Master/all_master.Master" CodeFile="details.aspx.cs" Inherits="Weixinclient_details" %>
<%@ Register Src="~/Weixinclient/ascx/pagecutphone.ascx" TagName="pageweixincut" TagPrefix="uc1" %>
<%@ Register Src="~/Weixinclient/ascx/weixintop.ascx" TagName="weixintop" TagPrefix="uccontrol" %>
<%@ Register Src="~/Weixinclient/ascx/weixinbottom.ascx" TagName="weixinbottom" TagPrefix="uccontrol" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {




            //点击关闭按钮的时候，遮罩层关闭
            $(".talk-mask .order-close").on('click', function () {
                $("#order-bg,.talk-mask").css("display", "none");
            });

        });
        var paraCourierCom;
        var paraCourierNo;
        var time;
        function OpenCourier(CourierCom, CourierNo) {
            time = time + 1;
            $("#order-bg").css({
                display: "block", height: $(document).height()
            });
            var $box = $('.talk-mask');
            $box.css({
                display: "block",
            });

            if (CourierCom.indexOf("顺丰") > -1) {
                paraCourierCom = "shunfeng";
            }
            else if (CourierCom.indexOf("圆通") > -1) {
                paraCourierCom = "yuantong";
            }
            else if (CourierCom.indexOf("申通") > -1) {
                paraCourierCom = "shentong";
            }
            else if (CourierCom.indexOf("韵达") > -1) {
                paraCourierCom = "yunda";
            }
            else if (CourierCom.indexOf("中通") > -1) {
                paraCourierCom = "中通";
            }
            else if (CourierCom.indexOf("德邦") > -1) {
                paraCourierCom = "debangwuliu";
            }
            else if (CourierCom.indexOf("百世") > -1 || CourierCom.indexOf("汇通") > -1) {
                paraCourierCom = "huitongkuaid";
            }
            else if (CourierCom.indexOf("邮政包裹") > -1 || CourierCom == "邮政") {
                paraCourierCom = "youzhengguonei";
            }
            else if (CourierCom.indexOf("邮政国际") > -1) {
                paraCourierCom = "null";
            }
            else if (CourierCom.indexOf("EMS") > -1) {
                paraCourierCom = "ems";
            }

            //  window.open("http://m.kuaidi100.com/result.jsp?com=" + CourierCom + "&nu=" + CourierNo, '物流跟踪', 'width=500,height=700')
            $("#frameCourier").attr("src", "http://m.kuaidi100.com/result.jsp?com=" + paraCourierCom + "&nu=" + CourierNo);

        }

        function FillSrc() {

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<section >
    <div class="details" >
      <div class="details_img" ><img src="images/img.jpg" alt="" ></div>
      <h3 class="details-title" ><%= GetGlobalResourceObject("SystemResource","BarCode").ToString() %> : <%=orderModel.Order_ID %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%= GetGlobalResourceObject

("SystemResource","AssistCode").ToString() %> : <%=orderModel.ModelNo %></h3>
      <table width="100%" border="0" cellspacing="0" cellpadding="0" class="details-text"  >
        <tr>
          <td colspan="4"><%= GetGlobalResourceObject("Resource","STHospital").ToString() %> : <%=orderModel.hospital %></td>
        </tr>
        <tr>
          <td style="width:25%;"><%= GetGlobalResourceObject("SystemResource","Doctor").ToString() %></td>
          <td style="width:20%;"><span class="details-text-color details-text-solid" ><%=orderModel.doctor %>&nbsp;</span></td>
          <td style="width:25%;"><%= GetGlobalResourceObject("SystemResource","SalesMan").ToString() %></td>
          <td style="width:30%;"><span class="details-text-color" > <%=orderModel.seller %>&nbsp;</span></td>
        </tr>
        <tr>
          <td><%= GetGlobalResourceObject("SystemResource","Patient").ToString() %></td>
          <td><span class="details-text-color details-text-solid" ><%=orderModel.Patient %>&nbsp;</span></td>
          <td><%= GetGlobalResourceObject("SystemResource","Sex").ToString() %></td>
          <td><span class="details-text-color" ><%=orderModel.Sex %>&nbsp;</span></td>
        </tr>
        <tr>
          <td><%= GetGlobalResourceObject("SystemResource","Age").ToString() %></td>
          <td><span class="details-text-color details-text-solid" ><%=orderModel.Age %>&nbsp;</span></td>
          <td><%= GetGlobalResourceObject("SystemResource","DateToFactory").ToString() %></td>
          <td><span class="details-text-color" ><%=orderModel.indate.ToShortDateString() %>&nbsp;</span></td>
        </tr>
        <tr>
          <td><%= GetGlobalResourceObject("SystemResource","LeaveFactoryDate").ToString() %></td>
          <td><span class="details-text-color details-text-solid" >&nbsp;<%=orderModel.OutDate.ToShortDateString() == "0001/1/1"?"":orderModel.OutDate.ToShortDateString()%></span></td>
          <td><%= GetGlobalResourceObject("SystemResource","IsFenge").ToString() %></td>
          <td><span class="details-text-color" ><%=orderModel.Fenge == "Y"?"是":"否"%>&nbsp;</span></td>
        </tr>
        <tr>
          <td><%= GetGlobalResourceObject("SystemResource","Courier").ToString() %></td>
          <td><span class="details-text-color details-text-solid" ><%=orderModel.Courier%>&nbsp;</span></td>
          <td><%= GetGlobalResourceObject("SystemResource","CourierNo").ToString() %>&nbsp;</td>
         <td ><span class="details-text-color" style="word-wrap: break-word; break-word: break-all;"  onclick="OpenCourier('<%=orderModel.Courier%>','<%=orderModel.CourierNo %>')"><u style="word-wrap: break-word; word-

break: break-all;"><%=orderModel.CourierNo %></u></span></td>       </tr>
      </table>
      <div class="requirements" >
         <span class="requirements-title" ><%= GetGlobalResourceObject("SystemResource","OutSay").ToString() %>：</span><%=orderModel.OutSay %>
         <div class="clear" ></div>
      </div>
        <%if (itemnamelist.Count() > 0)
                {%>
      <h3 class="details-title details-title1" ><%= GetGlobalResourceObject("SystemResource", "ProductList").ToString() %><i ><img src="images/details_img.png" alt="" ></i></h3>
      <ul class="details-list details-list1">
          
          <%for (int i = 0; i < itemnamelist.Count(); i++)
                {%>
          <a style="color:black;" href="product.aspx?orderID=<%=orderID %>&serial=<%=serialID %>&productid=<%=ProductIdlist[i]%>"><li data-href="details.aspx" ><i ><img src="images/details-i.png" alt="" ></i><%=itemnamelist[i] %></li></a>
          <%} %>
      </ul>
        <%} %>
    </div>
        <div id="order-bg"></div>
        <div class="talk-mask" >
            <div class="order-close" >&nbsp;</div>
            <div ><span style="color:#20d4c9;">物流信息(如果无跟踪结果，请点击查询按钮)</span></div>
           <div style="width:100%;height:90%;-webkit-overflow-scrolling:touch;overflow:auto;">
              <iframe id="frameCourier" style="width:90%;height:90%; " frameborder="0" scrolling="yes"></iframe>
           </div>
        </div>  
  </section>
  <!--section   end-->
</asp:Content>
