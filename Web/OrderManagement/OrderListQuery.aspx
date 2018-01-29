<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderListQuery.aspx.cs" Buffer="false" Inherits="OrderManagement_OrderListQuery" %>
<%@ Register Src="~/ascx/pagecut.ascx" TagName="pagecut" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript" src="/jQuery/jquery-1.10.2.js"></script>
    <script src="/artDialog/jquery.artDialog.source.js?skin=green" type="text/javascript"></script>  
    <script type="text/javascript" src="js/all.js"></script>
    <link href="images/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        //window.onbeforeunload = function () {
        //    return "关闭窗口后所有数据将丢失";
        //}


        function TalkDialogByOrder(talkid) {

            if ($("#talk" + talkid).attr("disabled") == "disabled")
            {
                return false
            }
            else {           
                $("#talk" + talkid).attr("disabled", true);
                $("#talk" + talkid).css("color", "gray");
                //window.location.href = "ChatForm.aspx?OrderNumber=" + talkid + "&userName=" + $("#userName").val()
                window.open("ChatForm.aspx?OrderNumber=" + talkid + "&userName=" + $("#userName").val(), "", "");
                return true;
            }
        }


        function SearchInfoList() {
            $("#selectedID").val($('#selectedID', parent.document).val());
            $("#selectedLevel").val($('#selectedLevel', parent.document).val());
            document.forms[0].action = "OrderListQuery.aspx?type=Order";
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

        function VerifierOrder(ModelNo)
        {
            if ($("#Verify" + ModelNo).attr("disabled") == "disabled") {
                return false
            }

            var IDRule = '<%=IDRule%>'
            if (IDRule == "B")
            {
                $("#VerifyOrderFrame").attr("src", "VerifyOrder.aspx?VerifierNo=Verifier" + ModelNo)
                $("#divVerifyOrder").show();
            }
            else {
                $.ajax({
                    url: "OrderListQuery.aspx?OrderNumber=" + ModelNo + "&action=VerifierOrder",
                    success: function (data) {
                        alertdialog('<%= GetGlobalResourceObject("Resource","TalkRecord").ToString() %>');
                },
                cache: false
                })
            }
            
        }

        function deleteOrder(ModelNo) {
            art.dialog({
                title: false,
                icon: 'question',
                content: '<%= GetGlobalResourceObject("Resource","OrderChecked").ToString() %>',
                ok: function () {
                    $("#hDelete").val(ModelNo);
                    document.forms[0].submit();
                    return false;
                },
                cancelVal: '<%= GetGlobalResourceObject("Resource","Cancel").ToString() %>',
                cancel: true
            });
        }

        function OpenTalkHistory(talkid) {
             
            dialog = art.dialog({
                id: 'PIC031',
                title: "<%= GetGlobalResourceObject("Resource","TalkHistory").ToString() %>",
                background: '#000',
                width: 800,
                height: 400,
                opacity: 0.3,

                close: function () {
                }
            });

            $("#record" + talkid).html("<%= GetGlobalResourceObject("Resource","TalkRecord").ToString() %>");
            $("#record" + talkid).css("color", "#0098e3");

            $.ajax({
                url: "OrderListQuery.aspx?OrderNumber=" + talkid+"&action=ReadTalkRecord",
                success: function (data) {
                    setCookie("<%=LoginUser.BelongFactory%>" + talkid, data)
                },
                cache: false
            })

            $.ajax({
                url: "HistoryTalk.aspx?OrderNumber=" + talkid,
                success: function (data) {
                    dialog.content(data);
                },
                cache: false
            });
        }
       

        function FileUpload(ModelNo) {

            window.open("FileManageByOrderNo.aspx?ModelNo=" + ModelNo, "", "height=500, width=700, toolbar=no, menubar=no, scrollbars=no, resizable=no, location=no, status=no");

          
        }

        $(function ()
        {
            CompareTalkRecord();
            setInterval("CompareTalkRecord()",100000)
            $("a[id*=Verifier]").each(function ()
            {
                if ($(this).attr("status") == "1")
                {
                    $(this).attr("disabled", true);
                    $(this).css("color", "gray");
                    $(this).html("<%= GetGlobalResourceObject("Resource","Checked").ToString() %>");
                }
            })
            
        }
        )

        function CompareTalkRecord()
        {
            var myData = new Date();
            var times = myData.getTime();

            $.ajax({
                url: "OrderListQuery.aspx?action=CompareTalkRecord&version=" + times,
                success: function (data) {
                    $("#messgeDiv").html("");
                    //$("a[id*=record]").html("聊天记录");
                    //$("a[id*=record]").css("color", "#0098e3");
                    
                    if (data != "") { 
                        
                        for (var i = 0; i < data.split(',').length; i++) {
                            
                           // if ($("#talk" + data.split(',')[i]).attr("disabled") != "disabled") {
                                //$("#record" + data.split(',')[i]).html("新消息");
                                //$("#record" + data.split(',')[i]).css("color", "red");
                            // } 
                           
                            $("#messgeDiv").append("<div><input type='radio' name='copyorder' style='margin-left:20px;' onclick='ClickboardData(this)' title='复制单号' value='" + data.split(',')[i] + "'/><input style='border-style:none;background-color:#f0f6e4;width:120px;' readonly='readonly' value='" + data.split(',')[i] + "'></input></div>")
                            
                        }                        
                    }
                    
                    //alert(localTime);
                    //alert(data);
                },
                cache: false
            })
           // alert(getCookie("lastModifyDateByGeeStarOrder"))
        }

        function ClickboardData(objectRadio)
        {
            $("#txtModelNumber").val(objectRadio.value)
            $(objectRadio).next().select();
            window.clipboardData.setData('Text', objectRadio.value);
        }
      </script>
</head>
<body style="overflow:auto;">

    <form id="form1" runat="server">
    <input id="hDelete" name="hDelete" type="hidden" />
    <input id="userName" name="userName"  type="hidden" value="" />
    <input id="hpnumbers" name="hpnumbers" type="hidden" value="<%=hddpnumbers %>" />
         <input id="selectedID" type="hidden" name="selectedID" value="" />
    <input id="selectedLevel" type="hidden" name="selectedLevel" value="" />
    <div class="nowposition" ><%= GetGlobalResourceObject("Resource","DoctorOrder").ToString() %> > <%= GetGlobalResourceObject("Resource","OrderManagement").ToString() %> > <b><%= GetGlobalResourceObject("Resource","OrderList").ToString() %></b></div> 
    <div class="searchbox1" >
      <%= GetGlobalResourceObject("Resource","OrderBM").ToString() %><input id="txtModelNumber" class="txtiput" type="text" name="txtModelNumber" style="width: 260px" value="<%=Request["txtModelNumber"] %>" />
        <a onclick="SearchInfoList()" href="javascript:void(0)" class="btn02"><%= GetGlobalResourceObject("Resource","Search").ToString() %></a>
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
                <div style="float: right">
                    <%= GetGlobalResourceObject("Resource","PageShow").ToString() %><select id="seltiaoshu" name="seltiaoshu" onchange="showpagenumber(this.options[this.selectedIndex].value)">
                        <option value="20">20 </option>
                        <option value="50">50 </option>
                        <option value="100">100 </option>
                    </select><%= GetGlobalResourceObject("Resource","Records").ToString() %>
                </div>
            </div>
            <div style="overflow:auto;width:auto;">
        
                <table class="tablestyle">
                    <tr>
                        <th style="width:150px;"><%= GetGlobalResourceObject("Resource","OrderBM").ToString() %></th>
                        <th style="width:100px;display:none;"><%= GetGlobalResourceObject("Resource","OrderCategory").ToString() %></th>
                        <th style="width:80px;"><%= GetGlobalResourceObject("Resource","SellerAgency").ToString() %></th>
                        <th style="width:150px;"><%= GetGlobalResourceObject("Resource","HospitalClinic").ToString() %></th>
                        <th style="width:80px;"><%= GetGlobalResourceObject("Resource","Doctor").ToString() %></th>
                        <th style="width:100px;display:none;"><%= GetGlobalResourceObject("Resource","Patient").ToString() %></th>
                        <th style="width:240px;"><%= GetGlobalResourceObject("Resource","Operation").ToString() %></th>
                    </tr>
                    <asp:Repeater ID="repOrderList" runat="server">
                        <ItemTemplate>
                            <tr class="<%# Container.ItemIndex % 2 == 0 ? "" : "dli0"%>">
                                <td align="center"><%#DataBinder.Eval(Container.DataItem, "ModelNo")%></td>                    
                                <td align="center" style="display:none;"><%#GetTypeClass(DataBinder.Eval(Container.DataItem, "OrderClass"))%></td>
                                <td align="center"><%#GetSeller(DataBinder.Eval(Container.DataItem, "SellerID"))%></td>  
                                <td align="center"><%#GetHospital(DataBinder.Eval(Container.DataItem, "hospitalID"))%></td>
                                <td align="center"><%#GetDoctor(DataBinder.Eval(Container.DataItem, "DoctorId"))%></td>
                                <td align="center" style="display:none;"><%#DataBinder.Eval(Container.DataItem, "patient")%></td>
                                <td align="center">                                    
                                    <a href="javascript:void(0)" style="display:none;" id="Verifier<%#DataBinder.Eval(Container.DataItem, "ModelNo")%>" status="<%#DataBinder.Eval(Container.DataItem, "Auth")%>"  onclick="VerifierOrder('<%#DataBinder.Eval(Container.DataItem, "ModelNo") %>');"><%= GetGlobalResourceObject("Resource","Check").ToString() %></a>
                                     &nbsp;<a href="javascript:void(0)"  onclick="FileUpload('<%#DataBinder.Eval(Container.DataItem, "ModelNo") %>');"><%= GetGlobalResourceObject("Resource","Attachment").ToString() %></a>
                                   <a href="OrderInput.aspx?type=Order&ModelNo=<%#DataBinder.Eval(Container.DataItem, "ModelNo") %>" <%#(DataBinder.Eval(Container.DataItem, "Auth") == null ||DataBinder.Eval(Container.DataItem, "Auth").ToString() == "0")?"":"style=\"display:none;\"" %>  target="_parent"> &nbsp;<%= GetGlobalResourceObject("Resource","Edit").ToString() %></a> 
                                     &nbsp;<a href="javascript:void(0)"id="talk<%#DataBinder.Eval(Container.DataItem, "ModelNo")%>" onclick="return TalkDialogByOrder('<%#DataBinder.Eval(Container.DataItem, "ModelNo")%>')">
                                     <%= GetGlobalResourceObject("Resource","Talk").ToString() %> </a><a href="javascript:void(0);" <%#(DataBinder.Eval(Container.DataItem, "Auth") == null ||DataBinder.Eval(Container.DataItem, "Auth").ToString() == "0")?"":"style=\"display:none;\"" %> onclick="deleteOrder('<%#DataBinder.Eval(Container.DataItem, "ModelNo") %>')">&nbsp;<%= GetGlobalResourceObject("Resource","Delete").ToString() %></a>
                                    &nbsp;<a href="javascript:void(0);" id="record<%#DataBinder.Eval(Container.DataItem, "ModelNo") %>"  style="display:none;"  onclick="OpenTalkHistory('<%#DataBinder.Eval(Container.DataItem, "ModelNo") %>')"><%= GetGlobalResourceObject("Resource","TalkRecord").ToString() %></a>
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
                <div style="float: right">
                    每页显示<select id="seltiaoshu_bot" name="seltiaoshu_bot"  onchange="showpagenumber(this.options[this.selectedIndex].value)">
                        <option value="20">20 </option>
                        <option value="50">50 </option>
                        <option value="100">100 </option>
                    </select>条信息
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
    <div style="position:absolute;right:0px;bottom:0px;">
        <div style="height:20px; width:200px; text-align:right;border-bottom-style:solid; cursor:pointer; border-bottom-width:1px; border-color:gray;background-color:#f0f6e4;" id="openDiv" onclick="PopMessage()">打开消息</div>
        <div style="height:260px;width:200px; display:none;border-color:aliceblue;  overflow:auto; background-color:#f0f6e4;" id="messgeDiv"></div>
    </div>
    <script type="text/javascript">
        function PopMessage()
        {
            $("#messgeDiv").animate(
                {
                    height:'toggle'
                },
                "slow", "linear", function () { if (($(this).prev().text() == "打开消息")) { $(this).prev().text("关闭窗口") } else { $(this).prev().text("打开消息") } }
                );
        
        }
    </script>    
    <div id="divVerifyOrder" style="display:none; text-align:center; width:100%; height:100%; position:fixed; top:0px;   background-color:white; opacity:1; padding-top:100px;">
        <iframe id="VerifyOrderFrame" src="VerifyOrder.aspx" width="700px" height="150px" frameborder="0"></iframe>
    </div>
    </form>
</body>
</html>
