<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PatientQueryList.aspx.cs" Inherits="Information_PatientQueryList" %>
<%@ Register Src="~/ascx/pagecut.ascx" TagName="pagecut" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript" src="/jQuery/jquery-1.10.2.js"></script>
    <script src="/artDialog/jquery.artDialog.source.js?skin=green" type="text/javascript"></script>  
    <script type="text/javascript" src="../OrderManagementjs/all.js"></script>
    <link href="../OrderManagement/images/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        //window.onbeforeunload = function () {
        //    return "关闭窗口后所有数据将丢失";
        //}


        function TalkDialogByOrder(talkid) {
            window.open("ChatForm.aspx?OrderNumber=" + talkid + "&userName=" + $("#userName").val(), "", "height=500, width=600, toolbar= no, menubar=no, scrollbars=no, resizable=no, location=no, status=no");
        }


        function SearchInfoList() {
            $("#selectedID").val($('#selectedID', parent.document).val());
            $("#selectedLevel").val($('#selectedLevel', parent.document).val());
            document.forms[0].action = "PatientQueryList.aspx?type=Information";
            document.forms[0].submit();
        } function showpagenumber(id) {
            $("#hpnumbers").val(id);
            SearchInfoList();
        }

      

        $(function () {
            var sessionUsrName ='<%= Session["UserName"]%>'
            $("#userName").val(sessionUsrName);
            
            $("#seltiaoshu").parent().hide();
            $("#seltiaoshu_bot").parent().hide();
          
        })

        function ViewPatient(orderID) {

            dialog = art.dialog({
                id: 'PIC012',
                title: " <%= GetGlobalResourceObject("SystemResource","OrderMessage").ToString() %> ",
                width: 700,
                height: 450,
                background: '#000',
                opacity: 0.3,
                top: 2,
                left:2,
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
</head>
<body>
    <form id="form1" runat="server">
    <input id="hDelete" name="hDelete" type="hidden" />
    <input id="userName" name="userName"  type="hidden" value="" />
    <input id="hpnumbers" name="hpnumbers" type="hidden" value="<%=hddpnumbers %>" />
         <input id="selectedID" type="hidden" name="selectedID" value="" />
    <input id="selectedLevel" type="hidden" name="selectedLevel" value="" />
    <div class="searchbox1" ><%= GetGlobalResourceObject("SystemResource","Patient").ToString() %>
      <input id="txtPatient" class="txtiput" type="text" name="txtPatient" style="width: 260px" value="<%=Request["txtPatient"] %>" />
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
                <div style="float: right;"><%= GetGlobalResourceObject("SystemResource","PageSize").ToString() %>
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
                        <th style="width:150px;"><%= GetGlobalResourceObject("SystemResource","OrderCode").ToString() %></th>
                        <th style="width:120px;"><%= GetGlobalResourceObject("SystemResource","Agent").ToString() %></th>
                        <%--<th style="width:120px;">医院/诊所</th>--%>
                        <th style="width:100px;"><%= GetGlobalResourceObject("SystemResource","Doctor").ToString() %></th>
                        <th style="width:100px;"><%= GetGlobalResourceObject("SystemResource","Patient").ToString() %></th>
                        <th style="width:100px;"><%= GetGlobalResourceObject("SystemResource","Product").ToString() %></th>
                        <th style="width:60px;"><%= GetGlobalResourceObject("SystemResource","ExpirationTime").ToString() %></th>
                    </tr>
                    <asp:Repeater ID="repOrderList" runat="server">
                        <ItemTemplate>
                            <tr class="<%# Container.ItemIndex % 2 == 0 ? "" : "dli0"%>">
                                <td align="center"><a href="#" onclick="ViewPatient('<%#DataBinder.Eval(Container.DataItem, "Order_ID")%>')" ><%#DataBinder.Eval(Container.DataItem, "Order_ID")%></a> </td>
                                <td align="center"><%#DataBinder.Eval(Container.DataItem, "seller")%></td>  
                               <%-- <td align="center"><%#DataBinder.Eval(Container.DataItem, "hospital")%></td>--%>
                                <td align="center"><%#DataBinder.Eval(Container.DataItem, "doctor")%></td>
                                <td align="center"><%#DataBinder.Eval(Container.DataItem, "patient")%></td>
                                <td align="center"><%#DataBinder.Eval(Container.DataItem, "productName")%></td>
                                <td align="center"><%#DataBinder.Eval(Container.DataItem, "Valid")%></td>                                                
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
    </form>
</body>
</html>
