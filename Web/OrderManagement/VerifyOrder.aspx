﻿<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="VerifyOrder.aspx.cs" Inherits="OrderManagement_VerifyOrder"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <script type="text/javascript" src="/jQuery/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="/jQuery/browser.js"></script>
    <script type="text/javascript" src="/jQuery/jquery.form.js"></script> 
    <script type="text/javascript" src="/OrderManagement/js/all.js?rnd=1"></script>
    <script src="/artDialog/jquery.artDialog.source.js?skin=default" type="text/javascript"></script>
    <link href="/OrderManagement/images/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            if ('<%=checkPass%>' == "1") {
                var VerifierNo = '<%=Request["VerifierNo"]%>'
                $("#" + VerifierNo, parent.window.document).attr("disabled", "disabled");
                $("#" + VerifierNo, parent.window.document).css("color", "gray");
                $("#" + VerifierNo, parent.window.document).html("审核通过");
                $("#divVerifyOrder", parent.window.document)
                art.dialog({
                    content: '订单审核通过',
                    ok: function () {
                        ColseWindow();
                    }
                });
            }
            else if ('<%=checkPass%>' == "0") {
                art.dialog({ title: false, time: 2, icon: 'error', content: '订单审核失败' });
            }
        });


        function VerifyOrder() {
            if ($.trim($("#txtOrder").val()) == "") {
                art.dialog({ title: false, time: 2, icon: 'error', content: '订单号不能为空' });
                return false;
            }

            var Ischeck = "0";
            $.ajax({
                url: "VerifyOrder.aspx?action=CheckOrder",
                data: { "txtOrder": $("#txtOrder").val() },
                async: false,
                success: function (data) {
                    Ischeck = data;
                }
            })

            if (Ischeck == "0") {
                art.dialog({ title: false, time: 2, icon: 'error', content: '该单号已经存在' });
                return false;
            }

            $("#haddinfo").val("1");
            document.forms[0].submit();
        }

        $.ajaxSetup({ cache: false });
       
        function ColseWindow() {
            $("#divVerifyOrder", parent.window.document).hide();
        }
    </script>
    <style type="text/css">
        .expertnumber
        {
         margin: 10px 0px 5px 0px;
         height: 40px;
         font-size: 16px;
         font-family: 微软雅黑;
         color: #555;
         background-color: #f1f7e6;
         border: 1px solid #d3e1b8;
         line-height: 39px;
         padding-left: 15px;
         margin-top: 5px;
        }
    </style>
   </head>
   <body style="background-color:#f1f7e6;">
    <form id="form1" runat="server">
   <input id="VerifierNo" name="VerifierNo" type="hidden" value="<%=Request["VerifierNo"] %>" />
    <input id="haddinfo" name="haddinfo" type="hidden" />
    <div class="expertnumber">输入订单号</div>
    <table class="edittb1">
      
        <tr>
            <td  class="elefttd1" style="width:30%;" >
                订单号:
            </td>
            <td style="width:5%;">&nbsp;</td>
            <td style="width:65%;">
               <input id="txtOrder" class="inputblurClass2" name="txtOrder"  style="width: 300px;" value="" type="text" maxlength="50" />
            </td>
        </tr>           
        </table>    
    
    <div style="clear:both"></div>
       <div>
            <table style="width:100%;">     
                <tr>
           
                    <td style="text-align:center;" >
                        <a href="javascript:void(0)" class="btn01" onclick="return VerifyOrder();">审 核</a> <a href="javascript:void(0)"
                            class="btn01" onclick="ColseWindow();">取 消</a>
                    </td>
                </tr>
            </table>
       </div>
   </form>
   </body>
</html>