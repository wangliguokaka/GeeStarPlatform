<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RoleEdit.aspx.cs" 
    MasterPageFile="~/System/System.master" EnableViewState="true" ViewStateMode="Enabled" Inherits="System_RoleEdit" %>

<%@ Register Src="~/ascx/pagecut.ascx" TagName="pagecut" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <input id="hpnumbers" name="hpnumbers" type="hidden" value="<%=hddpnumbers %>" />
    <script type="text/javascript">
        //window.onbeforeunload = function () {
        //    return "关闭窗口后所有数据将丢失";
        //}

        $(function () {

            //权限全选
            $("input[name='checkAll']").click(function () {
                if ($(this).prop("checked") == true) {
                    $(this).parent().siblings("td").find("input[type='checkbox']").prop("checked", true);
                } else {
                    $(this).parent().siblings("td").find("input[type='checkbox']").prop("checked", false);
                }
            });
        });

        function FillFollowDate(thisObject)
        {
            if($(thisObject).val()!="")
            {
                classType = $(thisObject).attr("classType");
                if (classType == "All")
                {
                    $(".ValidDate").val($(thisObject).val());
                }

                if (classType == "Order" || classType == "Report" || classType == "Procedure" || classType == "System") {
                    $("input[classType*=" + classType + "]").val($(thisObject).val());
                }
            }
            
        }

        function FillFollowMonth(thisObject) {
            if ($(thisObject).val() != "") {
                validMonth = $(thisObject).attr("validMonth");
                if (validMonth == "All") {
                    $(".ValidMonth").val($(thisObject).val());
                }

                if (validMonth == "Order" || validMonth == "Report" || validMonth == "Procedure" || validMonth == "System") {
                    $("input[validMonth*=" + validMonth + "]").val($(thisObject).val());
                }
            }

        }

        function RoleAdd() {
            var checkValid = true;
            $("input[type='text']").each(function ()
            {
                if ($(this).val() == "")
                {
                    alertdialog("<%= GetGlobalResourceObject("SystemResource","EffectiveDateNull").ToString() %>");
                    checkValid = false;
                }
            }
            )
            if ($("input[type='checkbox']:checked").length == 0)
            {
                alertdialog("<%= GetGlobalResourceObject("SystemResource","SelectFunction").ToString() %>");
                checkValid = false;
            }

            if (!checkValid)
            {
                return false;
            }
            $("#haddinfo").val("1");
           
            document.forms[0].submit();
        }
</script>

    <input id="haddinfo" name="haddinfo" type="hidden" />
    <div class="nowposition">
       <%= GetGlobalResourceObject("SystemResource","OrderManagement").ToString() %> >
        <%= GetGlobalResourceObject("SystemResource","SystemManagement").ToString() %> >
        <b><%= GetGlobalResourceObject("SystemResource","AuthManagement").ToString() %></b></div>
    <div class="searchbox">
      <%= GetGlobalResourceObject("SystemResource","FactoryName").ToString() %><%=Request["FactoryName"] %>
        
    </div>
    <table>
      
        <tr>
            <td>
            </td>
            <td>
                <a href="javascript:void(0)" class="btn01" style="color:white;" onclick="return RoleAdd();"><%= GetGlobalResourceObject("SystemResource","Save").ToString() %></a>
            </td>
        </tr>
    </table>
    <div style="overflow:auto;width:100%;  margin-top:5px;height:400px;">
        <table border="0" cellspacing="0" cellpadding="0" class="border-table" width="98%">
        <thead>
          <tr>
            <th width="50%"><%= GetGlobalResourceObject("SystemResource","AddFactory").ToString() %></th>
            <th><%= GetGlobalResourceObject("SystemResource","Operation").ToString() %></th>
          </tr>
        </thead>
        <tbody>
          <asp:Repeater ID="rptList" runat="server" EnableViewState="true" ViewStateMode="Enabled" onitemdatabound="rptList_ItemDataBound">
          <ItemTemplate>
          <tr>
            <td style="white-space:nowrap;word-break:break-all;overflow:hidden;">
              <asp:HiddenField ID="hidID" Value='<%#Eval("id") %>' runat="server" />
              <asp:HiddenField ID="hidNavID" Value='<%#Eval("NavID") %>' runat="server" />
              <asp:HiddenField ID="hidMonth" Value='<%#Eval("Valid") %>' runat="server" />
              <asp:HiddenField ID="hidDatefrom" Value='<%#Eval("StartTime") %>' runat="server" />  
              <asp:HiddenField ID="HiddenField1" Value='<%#Eval("StartTime") %>' runat="server" />              
              <asp:HiddenField ID="hidLayer" Value='<%#Eval("class_layer") %>' runat="server" />
              <asp:Literal ID="LitFirst" runat="server"></asp:Literal>
              <%#Eval("title")%>               
            </td>
            <td>
                <input type="checkbox" runat="server" id="checkValid" />
                <div runat="server" id="divSet"><%= GetGlobalResourceObject("SystemResource","EffectiveDate").ToString() %>&nbsp;&nbsp;&nbsp;&nbsp;<input id="txtdatefrom" readonly="readonly" onblur="FillFollowDate(this)" class="ValidDate" runat="server" classType='<%#Eval("action_type") %>'  onfocus="WdatePicker({el:this.id,dateFmt:'yyyy-MM-dd'})" name="txtdatefrom" style="width: 100px" type="text" />
                    <img onclick="WdatePicker({el:'<%#Container.FindControl("txtdatefrom").ClientID %>',dateFmt:'yyyy-MM-dd'})" src="../My97DatePicker/skin/datePicker.gif"style="width: 16px; height: 22px;" align="absmiddle">
                &nbsp;&nbsp;&nbsp;&nbsp;<%= GetGlobalResourceObject("SystemResource","Validity").ToString() %>&nbsp;&nbsp;&nbsp;&nbsp;<input type="text" id="validmonth" onblur="FillFollowMonth(this)" maxlength="4" style="width:60px;" class="ValidMonth" validMonth='<%#Eval("action_type") %>' runat="server" onkeydown="onlyNum()" />&nbsp;&nbsp;<%= GetGlobalResourceObject("SystemResource","Month").ToString() %></div>
            </td>            
          </tr>
          </ItemTemplate>
          </asp:Repeater>
        </tbody>
      </table>
    </div>
 
   
</asp:Content>
