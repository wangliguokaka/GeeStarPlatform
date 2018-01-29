<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderListQuery.aspx.cs" MasterPageFile="~/ReportStatistics/ReportStatistics.master" Inherits="ReportStatistics_OrderListQuery" %>
<%@ Register Src="~/ascx/pagecut.ascx" TagName="pagecut" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   
    <input id="userName" name="userName"  type="hidden" value="" />
    <input id="hpnumbers" name="hpnumbers" type="hidden" value="<%=hddpnumbers %>" />
    <div class="searchbox1" ><%= GetGlobalResourceObject("SystemResource","OrderCode").ToString() %>
      <input id="txtModelNumber" class="txtiput" type="text" name="txtOrder_ID" style="width: 260px" value="<%=Request["txtModelNumber"] %>" />
        <a onclick="SearchInfoList()" href="javascript:void(0)" class="btn02"><%= GetGlobalResourceObject("SystemResource","Search").ToString() %></a>
    </div>
    <div class="rightbox">
        <div class="rightboxin">
            <div class="pagebox">
                <div class="dPage" style="float: left;padding-top:8px;">
        
                    <script type="text/javascript">
                        function btnGo_click_CurrentTop() {
                            document.getElementById("txtGo").value = document.getElementById("txtGoTop").value;
                            btnGo_click();
                        }
                    </script>
                    <div style="height: 15px; line-height: 15px;" >
                        <div style="float: left;" id="fyboxTop">

                        </div>
                        <div style="float: left;">
                            <input id="txtGoTop" name="txtGoTop" style="width: 42px; height: 13px;line-height:13px;font-size:12px;
                                border: 1px solid #444444;" type="text" value="<%=Request["sPageID"]%>" /></div>
                        <div style="float: left; padding-left: 10px;">
                            <input class="button" type="button" style="width: 30px; height: 18px; border: 1px solid #444444;
                                font-size: 10px" name="Submit" onclick="btnGo_click_CurrentTop(); return false;"
                                value="GO" /></div>
                    </div>
                </div>
                <div style="float: right"><%= GetGlobalResourceObject("SystemResource","PageSize").ToString() %>
                    <select id="seltiaoshu" name="seltiaoshu" onchange="showpagenumber(this.options[this.selectedIndex].value)">
                        <option value="20">20 </option>
                        <option value="50">50 </option>
                        <option value="100">100 </option>
                    </select><%= GetGlobalResourceObject("SystemResource","Messages").ToString() %>
                </div>
            </div>
            <div style="overflow:auto;width:auto;">
        
                <table class="tablestyle" style="width:100%;">
                    <tr>
                        <th style="width:200px;"><%= GetGlobalResourceObject("SystemResource","OrderCode").ToString() %></th>                        
                        <th style="width:100px;"><%= GetGlobalResourceObject("SystemResource","SalesOrAggent").ToString() %></th>
                        <th style="width:100px;"><%= GetGlobalResourceObject("SystemResource","HospitalOrClinic").ToString() %></th>                                      
                        <th ><%= GetGlobalResourceObject("SystemResource","Operation").ToString() %></th>
                    </tr>
                    <asp:Repeater ID="repOrderList" runat="server">
                        <ItemTemplate>
                            <tr class="<%# Container.ItemIndex % 2 == 0 ? "" : "dli0"%>">
                                <td align="center"><%#DataBinder.Eval(Container.DataItem, "Order_ID")%></td>                    
                                <td align="center"><%#DataBinder.Eval(Container.DataItem, "seller")%></td>  
                                <td align="center"><%#DataBinder.Eval(Container.DataItem, "hospital")%></td>
                                <td align="center">
                                    <a href="OrderDetail.aspx?type=ReportStatistics&Order_ID=<%#DataBinder.Eval(Container.DataItem, "Order_ID") %>&serial=<%#DataBinder.Eval(Container.DataItem, "serial") %>" target="_parent">订单明细表</a> 
                                     &nbsp;&nbsp;&nbsp;&nbsp;<a href="ProductCheckReport.aspx?type=ReportStatistics&Order_ID=<%#DataBinder.Eval(Container.DataItem, "Order_ID") %>&serial=<%#DataBinder.Eval(Container.DataItem, "serial") %>" >出厂检验报告 </a>
                                    &nbsp;&nbsp;&nbsp;&nbsp;<a href="OrdersElement.aspx?type=ReportStatistics&Order_ID=<%#DataBinder.Eval(Container.DataItem, "Order_ID") %>&serial=<%#DataBinder.Eval(Container.DataItem, "serial") %>">材料成份及批号</a>
                                     &nbsp;&nbsp;&nbsp;&nbsp;<a href="QSProcRec.aspx?type=ReportStatistics&Order_ID=<%#DataBinder.Eval(Container.DataItem, "Order_ID") %>&serial=<%#DataBinder.Eval(Container.DataItem, "serial") %>">生产过程检验报告</a>
                                     &nbsp;&nbsp;&nbsp;&nbsp;<a href="EquipRec.aspx?type=ReportStatistics&Order_ID=<%#DataBinder.Eval(Container.DataItem, "Order_ID") %>&serial=<%#DataBinder.Eval(Container.DataItem, "serial") %>">使用设备登记</a>
                                     &nbsp;&nbsp;&nbsp;&nbsp;<a href="DisinRec.aspx?type=ReportStatistics&Order_ID=<%#DataBinder.Eval(Container.DataItem, "Order_ID") %>&serial=<%#DataBinder.Eval(Container.DataItem, "serial") %>">消毒记录</a>
                                </td>                   
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
  
           <div class="pagebox">
             <div style="display:none;"> <uc1:pagecut ID="pagecut1" runat="server" /></div>  
             <div class="dPage" style="float: left;padding-top:8px;">
        
                    <script>
                        function btnGo_click_CurrentBop() {
                            document.getElementById("txtGo").value = document.getElementById("txtGoBop").value;
                            btnGo_click();
                        }
                    </script>
                    <div style="height: 15px; line-height: 15px;" >
                        <div style="float: left;" id="fyboxBot">

                        </div>
                        <div style="float: left;">
                            <input id="txtGoBop" name="txtGoBop" style="width: 42px; height: 13px;line-height:13px;font-size:12px;
                                border: 1px solid #444444;" type="text" value="<%=Request["sPageID"]%>" /></div>
                        <div style="float: left; padding-left: 10px;">
                            <input class="button" type="button" style="width: 30px; height: 18px; border: 1px solid #444444;
                                font-size: 10px" name="Submit" onclick="btnGo_click_CurrentBop(); return false;"
                                value="GO" /></div>
                    </div>
                </div>
                <div style="float: right"><%= GetGlobalResourceObject("SystemResource","PageSize").ToString() %>
                   <select id="seltiaoshu_bot" name="seltiaoshu_bot"  onchange="showpagenumber(this.options[this.selectedIndex].value)">
                        <option value="20">20 </option>
                        <option value="50">50 </option>
                        <option value="100">100 </option>
                    </select><%= GetGlobalResourceObject("SystemResource","Messages").ToString() %>
                </div>
                <script type="text/javascript">
                    var tepv = $("#hpnumbers").val();
                    if (tepv == "") {
                        $("#seltiaoshu").val("20");
                        $("#seltiaoshu_bot").val("20");
                    } else {
                        $("#seltiaoshu").val(tepv);
                        $("#seltiaoshu_bot").val(tepv);
                    }

                    document.getElementById("fyboxTop").innerHTML = document.getElementById("fybox").innerHTML;
                    document.getElementById("fyboxBot").innerHTML = document.getElementById("fybox").innerHTML;

                </script>
            </div>
        </div>
    </div>
</asp:Content>
