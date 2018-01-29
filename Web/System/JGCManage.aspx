<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JGCManage.aspx.cs" 
    MasterPageFile="~/System/System.master" Inherits="System_JGCManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <script type="text/javascript" language="javascript">
        <%--$(document).ready(function () {
            if ('<%=Request["txtNewPassword"]%>' != "")
            {

                art.dialog({
                    content: '密码修改成功',
                    ok: function () {
                        ColseWindow();                        
                    }
                });
               
            }
        });--%>

       

        $.ajaxSetup({ cache: false });
        function SavePassword() {
            if ($.trim($("#txtJGCBM").val()) == "") {
                art.dialog({ title: false, time: 2, icon: 'error', content: '<%= GetGlobalResourceObject("SystemResource","FactoryCodeNull").ToString() %>' });
                return false;
            }
            if ($.trim($("#txtJGCName").val()) == "") {
                art.dialog({ title: false, time: 2, icon: 'error', content: '<%= GetGlobalResourceObject("SystemResource","FactoryNameNull").ToString() %>' });
                return false;
            }

            if ($.trim($("#txtDBUser").val()) == "") {
                art.dialog({ title: false, time: 2, icon: 'error', content: '<%= GetGlobalResourceObject("SystemResource","DBAccountNull").ToString() %>' });
                return false;
            }

            if ($.trim($("#txtDBPassword").val()) == "") {
                art.dialog({ title: false, time: 2, icon: 'error', content: '<%= GetGlobalResourceObject("SystemResource","DBPwdNull").ToString() %>' });
                return false;
            }

            if ($.trim($("#txtDBServerIP").val()) == "") {
                art.dialog({ title: false, time: 2, icon: 'error', content: '<%= GetGlobalResourceObject("SystemResource","DBIPNull").ToString() %>' });
                return false;
            }

            if ($("#txtRemark").val().length > 200)
            {
                art.dialog({ title: false, time: 2, icon: 'error', content: '<%= GetGlobalResourceObject("SystemResource","CommentsTooLong").ToString() %>' });
                return false;
            }
            
            if ($.trim($("#txtNoCardMerId").val()) != "" && $.trim($("#txtB2CMerId").val())!="" && $("#txtNoCardMerId").val() == $("#txtB2CMerId").val()) {
                art.dialog({ title: false, time: 2, icon: 'error', content: '<%= GetGlobalResourceObject("SystemResource","QuickPayRepeat").ToString() %>' });
                return false;
            }

            var Ischeck = "0";
            $.ajax({
                url: "JGCManage.aspx?action=CheckJGCBM",
                data: { "JGCBM": $.trim($("#txtJGCBM").val()),"ID":'<%=Request["ID"]%>' },
                async: false,
                success:function(data)
                {
                    Ischeck = data;
                }
            })

            if (Ischeck == "1")
            {
                art.dialog({ title: false, time: 2, icon: 'error', content: '<%= GetGlobalResourceObject("SystemResource","FactoryCodeExists").ToString() %>' });
                return false;
            }

            $.ajax({
                url: "JGCManage.aspx?action=CheckConnection",
                data: { "JGCBM": $.trim($("#txtJGCBM").val()), "DBUser": $.trim($("#txtDBUser").val()), "DBPassword": $.trim($("#txtDBPassword").val()), "DBServerIP": $.trim($("#txtDBServerIP").val()), "SameJGCBM": $("#SameJGCBM").prop("checked") },
                async: false,
                success: function (data) {
                    Ischeck = data;
                }
            })


            if (Ischeck == "1") {
                art.dialog({ title: false, time: 2, icon: 'error', content: '<%= GetGlobalResourceObject("SystemResource","DBConnFail").ToString() %>' });
                return false;
            }

            $.ajax({
                url: "JGCManage.aspx?action=CheckMerID",
                data: { "NoCardMerId": $.trim($("#txtNoCardMerId").val()), "B2CMerId": $.trim($("#txtB2CMerId").val()), "ID": '<%=Request["ID"]%>' },
                async: false,
                success: function (data) {
                    Ischeck = data;
                }
             })

            if (Ischeck == "2") {
                art.dialog({ title: false, time: 2, icon: 'error', content: '<%= GetGlobalResourceObject("SystemResource","QuickPayExists").ToString() %>' });
                return false;
            }

            if (Ischeck == "3") {
                art.dialog({ title: false, time: 2, icon: 'error', content: '<%= GetGlobalResourceObject("SystemResource","B2CExists").ToString() %>' });
                return false;
            }

            $("#haddinfo").val("1");
            document.forms[0].submit();
        }
        function ColseWindow() {
            $("#divProcess", parent.window.document).hide();
        }
    </script>
   
    <input id="haddinfo" name="haddinfo" type="hidden" />
    <div class="nowposition">
       <%= GetGlobalResourceObject("SystemResource","OrderManagement").ToString() %> >
        <%= GetGlobalResourceObject("SystemResource","SystemManagement").ToString() %> >
        <b><%= GetGlobalResourceObject("SystemResource","FactoryManagement").ToString() %></b></div>
    <div class="expertnumber">
        <a href="javascript:void(0)" class="btn01" onclick="return SavePassword();"><%= GetGlobalResourceObject("SystemResource","Save").ToString() %></a> 
        <a href="JGCList.aspx?type=System" class="btn01" ><%= GetGlobalResourceObject("SystemResource","Cancel").ToString() %></a>
    </div>
    <table class="edittb1" style="margin-top:10px;">
        <tr>
            <td  class="elefttd1"  style="width:20%;text-align:center;" >
                <%= GetGlobalResourceObject("SystemResource","FactoryCode").ToString() %>
            </td>
            <td style="width:35%;">
                 <input id="txtJGCBM" class="inputblurClass2" name="txtJGCBM"  value="<%=jxuserModel.JGCBM %>" type="text" maxlength="50" />
            </td>       
            <td class="elefttd1" style="width:20%;text-align:center;"><%= GetGlobalResourceObject("SystemResource","FactoryName").ToString() %></td>          
            <td style="width:30%">
                <input id="txtJGCName" class="inputblurClass2" name="txtJGCName"  value="<%=jxuserModel.JGCName %>" type="text" maxlength="50" />
            </td>           
        </tr>
        
          <tr>
            <td class="elefttd1" style="text-align:center;"><%= GetGlobalResourceObject("SystemResource","Contacts").ToString() %></td>          
            <td>
                <input id="txtContact" class="inputblurClass2" name="txtContact"  value="<%=jxuserModel.Contact %>" type="text" maxlength="50" />
            </td> 
            <td  class="elefttd1"  style="text-align:center;">
                <%= GetGlobalResourceObject("SystemResource","Email").ToString() %>
            </td>
            <td >
                 <input id="txtEmail" class="inputblurClass2" name="txtEmail"  value="<%=jxuserModel.Email %>" type="text" maxlength="50" />
            </td>       
                     
        </tr>
          <tr>
              <td class="elefttd1" style="text-align:center;">QQ:</td>          
            <td>
                <input id="txtContactQQ" class="inputblurClass2" name="txtContactQQ"  value="<%=jxuserModel.ContactQQ %>" type="text" maxlength="50" />
            </td>  
            <td  class="elefttd1"  style="text-align:center;">
                <%= GetGlobalResourceObject("SystemResource","WeChat").ToString() %>
            </td>
            <td >
                 <input id="txtWeiXin" class="inputblurClass2" name="txtWeiXin"  value="<%=jxuserModel.WeiXin %>" type="text" maxlength="50" />
            </td>       
                  
        </tr>
         <tr>
              <td class="elefttd1" style="text-align:center;"><%= GetGlobalResourceObject("SystemResource","PhoneNumber").ToString() %></td>          
            <td>
                <input id="txtTelephone" class="inputblurClass2" name="txtTelephone"  value="<%=jxuserModel.Telephone %>" type="text" maxlength="50" />
            </td>    
            <td  class="elefttd1"  style="text-align:center;">
                <%= GetGlobalResourceObject("SystemResource","DBAccount").ToString() %>
            </td>
            <td >
                 <input id="txtDBUser" class="inputblurClass2" name="txtDBUser"  value="<%=jxuserModel.DBUser %>" type="text" maxlength="50" />
            </td>       
                     
        </tr>
        <tr>
             <td class="elefttd1" style="text-align:center;"><%= GetGlobalResourceObject("SystemResource","DBPwd").ToString() %></td>          
            <td>
                <input id="txtDBPassword" class="inputblurClass2" name="txtDBPassword"  value="<%=jxuserModel.DBPassword %>" type="text" maxlength="50" />
            </td> 
            <td  class="elefttd1"  style="text-align:center;">
               <%= GetGlobalResourceObject("SystemResource","DBIP").ToString() %> 
            </td>
            <td >
                 <input id="txtDBServerIP" class="inputblurClass2" name="txtDBServerIP"  value="<%=jxuserModel.DBServerIP %>" type="text" maxlength="50" />
            </td>       
                      
        </tr>
        <tr>
            <td  class="elefttd1" style="text-align:center;"  >
                <%= GetGlobalResourceObject("SystemResource","Address").ToString() %> 
            </td>
            <td colspan="3">
                 <input id="txtAddress" class="inputblurClass2" name="txtAddress" style="width:600px;"  value="<%=jxuserModel.Address %>" type="text" maxlength="50" />
            </td>       
                      
        </tr>
          <tr>
            
            <td  class="elefttd1"  style="text-align:center;">
               <%= GetGlobalResourceObject("SystemResource","LinkPass").ToString() %> 
            </td>
            <td >
                 <input id="txtUserPasswd" class="inputblurClass2" name="txtUserPasswd"  value="<%=jxuserModel.UserPasswd %>" type="text" maxlength="50" />
            </td>       
             <td class="elefttd1" style="text-align:center;"><%= GetGlobalResourceObject("SystemResource","SameJGCBM").ToString() %></td>          
            <td style="text-align:left;">
                <input type="checkbox" class="inputblurClass2" id="SameJGCBM" name="SameJGCBM"  <%=jxuserModel.DBsameJGCBM=="Y"?"checked=\"checked\"":""%> />
            </td>        
        </tr>
        <tr class="expertnumber"><td colspan="4" style="font-weight:bold; "><%= GetGlobalResourceObject("SystemResource","QuickPaySet").ToString() %></td></tr>
        <tr>
            <td  class="elefttd1" style="text-align:center;"  >
                <%= GetGlobalResourceObject("SystemResource","QuickPayAccount").ToString() %>
            </td>
            <td>
                 <input id="txtNoCardMerId" class="inputblurClass2" name="txtNoCardMerId" style="width:200px;"  value="<%=jxuserModel.PayNoCardMerId %>" type="text" maxlength="50" />
            </td> 
            <td  class="elefttd1" style="text-align:center;"  >
               <%= GetGlobalResourceObject("SystemResource","PublicUpload").ToString() %> 
            </td>
            <td>
                 <asp:FileUpload runat="server" ID="NoCardCP" />
            </td>
        </tr>
        <tr>
            <td  class="elefttd1" style="text-align:center;"  >
                <%= GetGlobalResourceObject("SystemResource","PrivateUpload").ToString() %>
            </td>
            <td>
                  <asp:FileUpload runat="server" ID="NoCardSign" />
            </td> 
            <td  class="elefttd1" style="text-align:center;"  >
                <%= GetGlobalResourceObject("SystemResource","PrivatePwd").ToString() %>
            </td>
            <td>
                 <input id="txtNoCardMerIdPass" class="inputblurClass2" name="txtNoCardMerIdPass" style="width:200px;"  value="<%=jxuserModel.PayNoCardPass %>" type="text" maxlength="50" />
            </td>          
        </tr>

        <tr class="expertnumber"><td colspan="4"  style="font-weight:bold; "><%= GetGlobalResourceObject("SystemResource","SetB2C").ToString() %></td></tr>
        <tr>
            <td  class="elefttd1" style="text-align:center;"  >
                <%= GetGlobalResourceObject("SystemResource","B2CAccount").ToString() %>
            </td>
            <td>
                 <input id="txtB2CMerId" class="inputblurClass2" name="txtB2CMerId" style="width:200px;"  value="<%=jxuserModel.PayB2CMerId %>" type="text" maxlength="50" />
            </td> 
            <td  class="elefttd1" style="text-align:center;"  >
                <%= GetGlobalResourceObject("SystemResource","B2CPublicUpload").ToString() %>
            </td>
            <td>
                 <asp:FileUpload runat="server" ID="B2CCP" />
            </td>
        </tr>
        <tr>
            <td  class="elefttd1" style="text-align:center;"  >
               <%= GetGlobalResourceObject("SystemResource","B2CPrivateUpload").ToString() %> 
            </td>
            <td>
                  <asp:FileUpload runat="server" ID="B2CSign" />
            </td> 
            <td  class="elefttd1" style="text-align:center;"  >
                <%= GetGlobalResourceObject("SystemResource","B2CPrivatePwd").ToString() %>
            </td>
            <td>
                 <input id="txtB2CMerIdPass" class="inputblurClass2" name="txtB2CMerIdPass" style="width:200px;"  value="<%=jxuserModel.PayB2CPass %>" type="text" maxlength="50" />
            </td>          
        </tr>
        <tr>

         <td class="elefttd1" style="text-align:center;" ><%= GetGlobalResourceObject("SystemResource","Comments").ToString() %></td>
            <td colspan="3">
                <textarea id="txtRemark" name="txtRemark" rows="3" style="width:600px" ><%=jxuserModel.Remark %></textarea>
            </td>
        </tr>
        </table>    
    
    <div style="clear:both; display:none;"></div>
       <div style="clear:both; display:none;">
            <table style="width:100%;">     
                <tr>
           
                    <td style="text-align:center;" >
                        <a href="javascript:void(0)" class="btn01" onclick="return SavePassword();"><%= GetGlobalResourceObject("SystemResource","Save").ToString() %></a> <a href="JGCList.aspx?type=System"
                            class="btn01" ><%= GetGlobalResourceObject("SystemResource","Cancel").ToString() %></a>
                    </td>
                </tr>
            </table>
       </div>
</asp:Content>
