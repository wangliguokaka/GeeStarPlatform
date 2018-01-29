<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FileManageByOrderNo.aspx.cs" Inherits="OrderManagement_FileManageByOrderNo" %>
<%@ Register TagPrefix="Upload" Namespace="Brettle.Web.NeatUpload" Assembly="Brettle.Web.NeatUpload" %>
<%@ Register Src="~/ascx/pagecut.ascx" TagName="pagecut" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link type="text/css" rel="stylesheet" href="images/main.css" />
    <script type="text/javascript" src="/jQuery/jquery-1.10.2.js"></script>
    <script src="/artDialog/jquery.artDialog.source.js?skin=green" type="text/javascript"></script>  
    <script type="text/javascript" src="js/all.js"></script>
    <script type="text/javascript">
        function ShowProcess()
        {
            if($("#<%=AttachFile.ClientID%>").val()=="")
            {
                alertdialog('<%= GetGlobalResourceObject("Resource","InputAttachmentAlert").ToString() %>');
                return false;
            }
            $("#divProcess").show();
            return true;
        }


        function deleteFile(ID) {
            art.dialog({
                title: false,
                icon: 'question',
                content: '<%= GetGlobalResourceObject("Resource","ConfirmDeleteAttachmentAlert").ToString() %>',
                ok: function () {
                    $("#hDelete").val(ID);
                    document.forms[0].submit();
                    return false;
                },
                okVal: '<%= GetGlobalResourceObject("Resource","OK").ToString() %>',
                cancelVal: '<%= GetGlobalResourceObject("Resource","Cancel").ToString() %>',
                cancel: true
            });
        }

    </script>
    <style type="text/css">
        html, body 
        {
            margin:0px 0px;
            height:100%;
            width:100%;
        }

        
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
               
        
        
    </div>
        <input id="hDelete" name="hDelete" type="hidden" />
    <input id="userName" name="userName"  type="hidden" value="" />
    <input id="hpnumbers" name="hpnumbers" type="hidden" value="<%=hddpnumbers %>" />
         <input id="selectedID" type="hidden" name="selectedID" value="" />
    <input id="selectedLevel" type="hidden" name="selectedLevel" value="" />
    <div class="nowposition" ><%= GetGlobalResourceObject("Resource","DoctorOrder").ToString() %> > <%= GetGlobalResourceObject("Resource","OrderManagement").ToString() %> > <b><%= GetGlobalResourceObject("Resource","AttachmentManage").ToString() %></b></div>   
    <div class="searchbox1" >
      <%= GetGlobalResourceObject("Resource","UploadAttachment").ToString() %>：<Upload:InputFile ID="AttachFile"  runat="server"/>&nbsp;&nbsp;<asp:Button ID="btnSave" runat="server" CssClass="uploadbtn"  OnClientClick="return ShowProcess()" OnClick="btnSave_Click" />
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
                
            </div>
            <div style="overflow:auto;width:auto;">
        
                <table class="tablestyle">
                    <tr>
                        <th style="width:300px;"><%= GetGlobalResourceObject("Resource","AttachmentName").ToString() %></th>
                        <th style="width:200px;"><%= GetGlobalResourceObject("Resource","UploadDate").ToString() %></th>
                        <th style="width:100px;"><%= GetGlobalResourceObject("Resource","Operation").ToString() %></th>                        
                    </tr>
                    <asp:Repeater ID="repOrderList" runat="server">
                        <ItemTemplate>
                            <tr class="<%# Container.ItemIndex % 2 == 0 ? "" : "dli0"%>">
                                <td align="center"><a href="<%#DataBinder.Eval(Container.DataItem, "FilePath") %>" target="_blank"><%#DataBinder.Eval(Container.DataItem, "Filename")%></a> </td>    
                                <td align="center"><%#DataBinder.Eval(Container.DataItem, "CreateDate")%></td>                 
                                <td align="center">
                                   <a href="javascript:void(0);" onclick="deleteFile('<%#DataBinder.Eval(Container.DataItem, "ID") %>')"><%= GetGlobalResourceObject("Resource","Delete").ToString() %></a>
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
                       
                    </div>
                </div>
               
                <script type="text/javascript">
                    var tepv = $("#hpnumbers").val();
                    if (tepv == "") {
                        $("#seltiaoshu").val("20");
                    } else {
                        $("#seltiaoshu").val(tepv);
                    }

                    document.getElementById("fyboxTop").innerHTML = document.getElementById("fybox").innerHTML;

                </script>
            </div>
        </div>
    </div>
    <div id="divProcess" style="display:none; text-align:center; width:100%; height:100%; position:absolute; top:0px;  background-color:gray; opacity:0.9; padding-top:100px;">
        <Upload:ProgressBar ID="ProgressBar2" Visible="true" Inline="true" Width="600px"  runat='server'></Upload:ProgressBar>
    </div>
    </form>
</body>
</html>
