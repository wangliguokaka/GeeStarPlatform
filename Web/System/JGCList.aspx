<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JGCList.aspx.cs" 
    MasterPageFile="~/System/System.master" Inherits="System_JGCList" %>

<%@ Register Src="~/ascx/pagecut.ascx" TagName="pagecut" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <input id="hpnumbers" name="hpnumbers" type="hidden" value="<%=hddpnumbers %>" />
    <script type="text/javascript">
        //window.onbeforeunload = function () {
        //    return "关闭窗口后所有数据将丢失";
        //}

        function SearchInfoList() {
            document.forms[0].submit();
        } function showpagenumber(id) {
            $("#hpnumbers").val(id);
            SearchInfoList();
        }

        function DeleteJGC(ID) {
            art.dialog({
                title: false,
                icon: 'question',
                
                content: '<%= GetGlobalResourceObject("SystemResource","ConfirmDeleteFactory").ToString() %>',
                ok: function () {
                    $("#hDelete").val(ID);
                    document.forms[0].submit();
                    return false;
                },
                cancelVal: '<%= GetGlobalResourceObject("SystemResource","Cancel").ToString() %>',
                cancel: true
            });
        }

        $(function () {
            var sessionUsrName ='<%= Session["UserName"]%>'
            $("#userName").val(sessionUsrName);
        })
    </script>
    <input id="hDelete" name="hDelete" type="hidden" />
    <input id="userName" name="userName"  type="hidden" value="" />
    <div class="nowposition">
        <%= GetGlobalResourceObject("SystemResource","OrderManagement").ToString() %> >
        <%= GetGlobalResourceObject("SystemResource","SystemManagement").ToString() %> >
        <b><%= GetGlobalResourceObject("SystemResource","FactoryList").ToString() %></b></div>
    <div class="searchbox">
        <%= GetGlobalResourceObject("SystemResource","FactoryCode").ToString() %>
      <input id="txtUserName" class="txtiput" type="text" name="txtUserName" style="width: 260px"
            value='<%=Request["txtUserName"] %>' />
        <a onclick="SearchInfoList()" href="javascript:void(0)" class="btn02">
            <%= GetGlobalResourceObject("SystemResource","Search").ToString() %>
            </a><a  href="JGCManage.aspx?type=System" class="btn02"><%= GetGlobalResourceObject("SystemResource","AddFactory").ToString() %></a>
    </div>
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
    <div style="overflow:auto;width:100%;">
        
        <table class="tablestyle" style="width:100%;">
            <tr>
                <th style="width:20%;"><%= GetGlobalResourceObject("SystemResource","FactoryCode").ToString() %></th>
                <th style="width:20%;"><%= GetGlobalResourceObject("SystemResource","FactoryName").ToString() %></th>
                <th style="width:20%;"><%= GetGlobalResourceObject("SystemResource","Contacts").ToString() %></th>
                <th style="width:25%;"><%= GetGlobalResourceObject("SystemResource","PhoneNumber").ToString() %></th>
                <th style="width:15%;"><%= GetGlobalResourceObject("SystemResource","Operation").ToString() %></th>
            </tr>
            <asp:Repeater ID="repUserList" runat="server">
            
                <ItemTemplate>
                    <tr class="<%# Container.ItemIndex % 2 == 0 ? "" : "dli0"%>">
                        <td align="center"><%#Eval("JGCBM")%></td>
                        <td align="center"><%#Eval("JGCName")%></td> 
                        <td align="center"><%#Eval("Contact")%></td>            
                        <td align="center"><%#Eval("Telephone")%></td>    
                         <td align="center" >  
                             <a href="JGCManage.aspx?type=System&ID=<%#Eval("ID")%>"><%= GetGlobalResourceObject("SystemResource","Edit").ToString() %> </a>&nbsp;&nbsp;&nbsp;&nbsp;
                             <a href="javascript:void(0);" onclick="DeleteJGC('<%#Eval("ID")%>')"><%= GetGlobalResourceObject("SystemResource","Delete").ToString() %></a>
                        </td>                   
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
  
   <div class="pagebox">
     <div style="display:none;"> <uc1:pagecut ID="pagecut1" runat="server" /></div>  
        <script>
            var tepv = $("#hpnumbers").val();
            if (tepv == "") {
                $("#seltiaoshu").val("20");
            } else {
                $("#seltiaoshu").val(tepv);
            }

            document.getElementById("fyboxTop").innerHTML = document.getElementById("fybox").innerHTML;

        </script>
    </div>
</asp:Content>
