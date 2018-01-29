<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProcedureQuery.aspx.cs"  MasterPageFile="~/Information/Information.master" Inherits="Information_ProcedureQuery" %>
<%@ Register Src="~/ascx/pagecut.ascx" TagName="pagecut" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        <script type="text/javascript">      
            function SearchInfoList() {
                document.forms[0].action = "ProcedureQuery.aspx?type=Information";
                document.forms[0].submit();
            }
            function showpagenumber(id) {
                $("#hpnumbers").val(id);
                SearchInfoList();
            }

            function OpenQueryWindow(orderID, serial)
            {
                var queryaction = '<%=queryaction%>'

                if (queryaction == "procedure")
                {
                    ViewProcedure(orderID, serial);
                }
                else if (queryaction == "patient") {
                    ViewPatient(orderID);
                }
                else {
                    ViewPatient(orderID);
                }
            }

            function ViewProcedure(orderID, serial) {

                dialog = art.dialog({
                    id: 'PIC011',
                    title: " <%= GetGlobalResourceObject("SystemResource","ProcessSelect").ToString() %>",
                    width: 920,
                    height: 450,
                    background: '#000',
                    opacity: 0.3,
                    top:10,
                    close: function () {

                    }

                });
              
                $.ajax({
                    url: "ViewProcedureByLine.aspx?orderID=" + orderID + "&serial=" + serial,
                    success: function (data) {
                        dialog.content(data);
                    },
                    cache: false
                });
            }

            function ViewPatient(orderID) {

                dialog = art.dialog({
                    id: 'PIC012',
                    title: " <%= GetGlobalResourceObject("SystemResource","OrderMessage").ToString() %>",
                    width: 700,
                    height: 450,
                    background: '#000',
                    opacity: 0.3,
                    top: 12,
                    close: function () {

                    }

                });

                $.ajax({
                    url: "ViewPatientDetail.aspx?orderID=" + orderID,
                    success: function (data) {
                        dialog.content(data);
                    },
                    cache: false
                });
            }

    </script>
    <input id="hDelete" name="hDelete" type="hidden" />
    <input id="userName" name="userName"  type="hidden" value="" />
    <input id="hpnumbers" name="hpnumbers" type="hidden" value="<%=hddpnumbers %>" />
         <input id="selectedID" type="hidden" name="selectedID" value="" />
    <input id="selectedLevel" type="hidden" name="selectedLevel" value="" />
    <div class="searchbox" id="searchbox" runat="server"> <%= GetGlobalResourceObject("SystemResource","TimeToFactory").ToString() %> :
      <input id="starttime" onclick="WdatePicker({ el: 'starttime' })" name="starttime" readonly="readonly"
            style="width: 70px" type="text" value="" class="txtiput" />
        <img onclick="WdatePicker({el:'starttime'})" src="../../My97DatePicker/skin/datePicker.gif"
            style="width: 16px; height: 22px;" align="absmiddle">
        -
        <input id="endtime" onclick="WdatePicker({ el: 'endtime' })" readonly="readonly" name="endtime"
            style="width: 70px" type="text" value="" class="txtiput"/>
        <img onclick="WdatePicker({el:'endtime'})" src="../../My97DatePicker/skin/datePicker.gif"
            style="width: 16px; height: 22px;" align="absmiddle">
            <script type="text/javascript">
                $("#starttime").val('<%=Request.Params["starttime"]%>');
                $("#endtime").val('<%=Request.Params["endtime"]%>');
        </script>
        <a onclick="SearchInfoList()" href="javascript:void(0)" class="btn02"> <%= GetGlobalResourceObject("SystemResource","Search").ToString() %></a>
    </div>
    <div class="searchbox" id="searchboxpatient" runat="server" > <%= GetGlobalResourceObject("SystemResource","Patient").ToString() %> :
      <input id="txtPatient" class="txtiput" type="text" name="txtPatient" style="width: 260px" value="<%=Request["txtPatient"] %>" />
      &nbsp;&nbsp; <%= GetGlobalResourceObject("SystemResource","BarCode").ToString() %>: <input id="txtOrderNo" class="txtiput" type="text" name="txtOrderNo" style="width: 260px" value="<%=Request["txtOrderNo"] %>" />
        <a onclick="SearchInfoList()" href="javascript:void(0)" class="btn02"> <%= GetGlobalResourceObject("SystemResource","Search").ToString() %></a>
    </div>
    <div class="">
        <div class="">
            <div class="pagebox">
                <div class="dPage" style="float: left;padding-top:0px;">
        
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
                <div style="float: right"> <%= GetGlobalResourceObject("SystemResource","PageSize").ToString() %> 
                    <select id="seltiaoshu" name="seltiaoshu" onchange="showpagenumber(this.options[this.selectedIndex].value)">
                        <option value="20">20 </option>
                        <option value="50">50 </option>
                        <option value="100">100 </option>
                    </select><%= GetGlobalResourceObject("SystemResource","Messages").ToString() %> 
                </div>
            </div>
            <div style="overflow:auto;width:auto;">
        
                <table class="tablestyle">
                    <tr>
                        <th style="width:200px;"> <%= GetGlobalResourceObject("SystemResource","BarCode").ToString() %> </th>                        
                        <th style="width:100px;"> <%= GetGlobalResourceObject("SystemResource","Agent").ToString() %></th>
                     <%--   <th style="width:100px;">医院/诊所</th>--%>
                        <th style="width:100px;"> <%= GetGlobalResourceObject("SystemResource","AssistCode").ToString() %> </th>
                        <th style="width:80px;"> <%= GetGlobalResourceObject("SystemResource","Doctor").ToString() %></th>
                        <th style="width:80px;"> <%= GetGlobalResourceObject("SystemResource","Patient").ToString() %></th>                       
                        <th style="width:100px;"> <%= GetGlobalResourceObject("SystemResource","OrderType").ToString() %></th>
                        <th style="width:180px;"> <%= GetGlobalResourceObject("SystemResource","DateToFactory").ToString() %></th>
                    </tr>
                    <asp:Repeater ID="repOrderList" runat="server">
                        <ItemTemplate>
                            <tr class="<%# Container.ItemIndex % 2 == 0 ? "" : "dli0"%>" >
                                <td align="center"><a href="#" onclick="OpenQueryWindow('<%#DataBinder.Eval(Container.DataItem, "Order_ID")%>','<%#DataBinder.Eval(Container.DataItem, "serial")%>')"><%#DataBinder.Eval(Container.DataItem, "Order_ID")%></a></td>
                                <td align="center"><%#GetSeller(DataBinder.Eval(Container.DataItem, "SellerID"))%></td>  
                         <%--       <td align="center"><%#GetHospital(DataBinder.Eval(Container.DataItem, "hospitalID"))%></td>--%>
                                <td align="center"><%#DataBinder.Eval(Container.DataItem, "ModelNo")%></td>
                                <td align="center"><%#DataBinder.Eval(Container.DataItem, "doctor")%></td>  
                                <td align="center"><%#DataBinder.Eval(Container.DataItem, "patient")%></td>   
                                <td align="center"><%#GetTypeClass(DataBinder.Eval(Container.DataItem, "orderclass").ToString())%></td>
                                <td align="center"><%#DataBinder.Eval(Container.DataItem, "indate")%></td>                          
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
  
           <div class="pagebox">
             <div style="display:none;"> <uc1:pagecut ID="pagecut1" runat="server" /></div>  
             <div class="dPage" style="float: left;padding-top:8px;">
        
                    <script type="text/javascript">
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
                <div style="float: right"> <%= GetGlobalResourceObject("SystemResource","PageSize").ToString() %> 
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
