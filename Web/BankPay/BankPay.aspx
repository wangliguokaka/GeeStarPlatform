<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BankPay.aspx.cs" EnableEventValidation="false" ViewStateEncryptionMode="Never"  MasterPageFile="~/BankPay/BankPayMaster.master" Inherits="BankPay_BankPay" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <script type="text/javascript" src="../zTree_v3/js/jquery.ztree.all-3.5.js"></script>
    <script type="text/javascript" src="js/Json.js"></script>
    <style type="text/css"> 
        /* Bank Select */
        .ui-list-icons li { float:left; margin:0px 10px 25px 0px; width:218px; padding-right:10px; display:inline; } 
        .ui-list-icons li input { vertical-align:middle; } 
        .ui-list-icons li .icon-box { cursor:pointer; width:190px; background:#FFF; line-height:36px; border:1px solid #DDD; vertical-align:middle; position:relative; display:inline-block; zoom:1; } 
        .ui-list-icons li .icon-box.hover, .ui-list-icons li .icon-box.current { border:1px solid #FA3; } 
        .ui-list-icons li .icon-box-sparkling { position:absolute; top:-18px; left:0px; height:14px; line-height:14px; } 
        .ui-list-icons li .icon { float:left; width:126px; padding:0px; height:36px; display:block; line-height:30px; color:#07f; font-weight:bold; white-space:nowrap; overflow:hidden; position:relative; z-index:1; } 
        .ui-list-icons li .bank-name { position:absolute; left:5px; z-index:0; height:36px; width:121px; overflow:hidden; } 
        /* Bank Background */
        .ui-list-icons li .ABC { background:#FFF url(/images/banklogo/农业银行.gif) no-repeat 5px center; }
         .ui-list-icons li .ICBC { background:#FFF url(/images/banklogo/工商银行.gif) no-repeat 5px center; }
         .ui-list-icons li .CCB { background:#FFF url(/images/banklogo/建设银行.gif) no-repeat 5px center; }
          .ui-list-icons li .PSBC { background:#FFF url(/images/banklogo/中国邮政储蓄银行.gif) no-repeat 5px center; }
           
        .ui-list-icons li .BOC { background:#FFF url(/images/banklogo/中国银行.gif) no-repeat 5px center; }
         .ui-list-icons li .CMB { background:#FFF url(/images/banklogo/招商银行.gif) no-repeat 5px center; }
          .ui-list-icons li .COMM  { background:#FFF url(/images/banklogo/交通银行.gif) no-repeat 5px center; }
           .ui-list-icons li .SPDB { background:#FFF url(/images/banklogo/浦发银行.gif) no-repeat 5px center; } 
        .ui-list-icons li .CEB { background:#FFF url(/images/banklogo/光大银行.gif) no-repeat 5px center; } 
        /* Bank Submit */
        .paybok { padding:0px 0px 0px 20px; } 
        .paybok .csbtx { border-radius:3px; color:#FFF; font-weight:bold; } 
     </style> 
    <script type="text/javascript">
        $(function () {
            //Bank Hover 
            $('.ui-list-icons > li').hover(function () {
                $(this).children('.icon-box').addClass('hover');
            }, function () {
                $(this).children('.icon-box').removeClass('hover');
            });

            //Bank Click 
            $('.ui-list-icons > li').click(function () {
                for (var i = 0; i < $('.ui-list-icons > li').length; i++) {
                    $('.ui-list-icons > li').eq(i).children('.icon-box').removeClass('current');
                }
                $(this).children('.icon-box').addClass('current');
            });

            
        })
        
        function SubmitCheck()
        {
            if ($("#money").val() == "")
            {
                alertdialog('<%= GetGlobalResourceObject("Resource","MoneyNotEmpty").ToString() %>');
                return false;
            }
        }

     

        function CalculateAmount()
        {
           
            var payMethod = $('input:radio[name="paymethod"]:checked').val();
            var money = $("#money").val();
            if (money != "") {
                if (payMethod == "1") {
                    var actualAmount = parseFloat(money) * (1.008);
                    actualAmount = (Math.ceil(actualAmount * 10) / 10).toFixed(1)
                    $("#actualAmount").val(actualAmount);

                }
                else {
                    var actualAmount = parseFloat(money) * (1.005);
                    actualAmount = (Math.ceil(actualAmount * 10) / 10).toFixed(1)
                    $("#actualAmount").val(actualAmount);
                }
            }
            else {
                $("#actualAmount").val("");
            }
        }

        function onlyNumber() {
            
            if (!(event.keyCode == 46) && !(event.keyCode == 8) && !(event.keyCode == 37) && !(event.keyCode == 39))
                if (!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)))
                    event.returnValue = false;
        }
</script> 
    <input id="hDelete" name="hDelete" type="hidden" />
    <input id="userName" name="userName"  type="hidden" value="" />
    <input id="selectedID" type="hidden" name="selectedID" value="" />
    <input id="selectedLevel" type="hidden" name="selectedLevel" value="" />
    <div class="nowposition"><%= GetGlobalResourceObject("Resource","BankPayTitle").ToString() %></div>     
    <div class="expertnumber"><asp:Button ID="btnConsume" runat="server" OnClientClick="return SubmitCheck()" OnClick="btnConsume_Click" CssClass="btn04" Text="" /></div>
      <table class="edittb">
        <tr>
            <td style="width:200px;">
                <%= GetGlobalResourceObject("Resource","PayMoney").ToString() %>:
            </td>
            <td style="width:300px;">                
                <input type="text" id="money" onkeydown="onlyNum()" onblur="CalculateAmount()" name="money" value="" /><%= GetGlobalResourceObject("Resource","Yuan").ToString() %>
            </td>
            <td>&nbsp</td>
        </tr>
        <tr>
            <td style="width:200px;">
                <%= GetGlobalResourceObject("Resource","PayActual").ToString() %>:
            </td>
            <td style="width:300px;">
                <input type="text" id="actualAmount" name="actualAmount" style="border-width:0px;" readonly="readonly" value="0" /> 元
            </td>
            <td>&nbsp</td>
        </tr>
           <tr>
            <td style="width:100px;">
                <%= GetGlobalResourceObject("Resource","PayMethod").ToString() %>:
            </td>
            <td style="width:300px;">       
                <input type="radio" value="1" onclick="CalculateAmount()"  id="nocard" <%=Request["paymethod"] == "2"? "":"checked=\"checked\"" %> name="paymethod" />&nbsp;&nbsp;快捷支付<input type="radio" value="2" <%=Request["paymethod"] == "2"? "checked=\"checked\"":"" %> onclick="CalculateAmount()"   id="netpay" name="paymethod" />&nbsp;&nbsp;B2C
            </td>
            <td>&nbsp</td>
        </tr>
    </table>
   <%-- <div class="rightbox">
        <div class="rightboxin">
            <div style="overflow:auto;width:auto;">
        
               <div class="paying"> 

                   
                    <div class="payamont" style="margin:10px 0px 10px 20px"> 
                        金额：<input type="text" id="money" onkeydown="onlyNum()" name="money" value="" /> 
                        <span>元</span> 
                    </div> 
                     <div>支付方式：<input type="radio" value="1" checked="checked" id="nocard" name="paymethod" />&nbsp;&nbsp;快捷支付<input type="radio" value="2"  id="netpay" name="paymethod" />&nbsp;&nbsp;B2C </div>
                   <%-- <ul class="ui-list-icons clrfix"> 
                        <li> 
                        <input type="radio" name="bank" id="ABC" value="" checked="checked"> 
                        <label class="icon-box current" for="ABC"> 
                        <span class="icon-box-sparkling" bbd="false"> </span> 
                        <span class="false icon ABC" title="中国农业银行"></span> 
                        <span class="bank-name">中国农业银行</span> 
                        </label> 
                        </li> 
                    <li> 
                        <input type="radio" name="bank" id="ICBC" value=""> 
                        <label class="icon-box" for="ICBC"> 
                        <span class="icon-box-sparkling" bbd="false"> </span> 
                        <span class="false icon ICBC" title="中国工商银行"></span> 
                        <span class="bank-name">中国工商银行</span> 
                        </label> 
                    </li> 
                    <li> 
                        <input type="radio" name="bank" id="CCB" value=""> 
                        <label class="icon-box" for="CCB"> 
                        <span class="icon-box-sparkling" bbd="false"> </span> 
                        <span class="false icon CCB" title="中国建设银行"></span> 
                        <span class="bank-name">中国建设银行</span> 
                        </label> 
                    </li>                    
                    <li> 
                        <input type="radio" name="bank" id="PSBC" value=""> 
                        <label class="icon-box" for="PSBC"> 
                        <span class="icon-box-sparkling" bbd="false"> </span> 
                        <span class="false icon PSBC" title="中国邮政储蓄银行"></span> 
                        <span class="bank-name">中国邮政储蓄银行</span> 
                        </label> 
                    </li> 
                    <li> 
                        <input type="radio" name="bank" id="BOC" value=""> 
                        <label class="icon-box" for="BOC"> 
                        <span class="icon-box-sparkling" bbd="false"> </span> 
                        <span class="false icon BOC" title="中国银行"></span> 
                        <span class="bank-name">中国银行</span> 
                        </label> 
                    </li> 
                    <li> 
                        <input type="radio" name="bank" id="CMB" value=""> 
                        <label class="icon-box" for="CMB"> 
                        <span class="icon-box-sparkling" bbd="false"> </span> 
                        <span class="false icon CMB" title="招商银行"></span> 
                        <span class="bank-name">招商银行</span> 
                        </label> 
                    </li> 
                    <li> 
                        <input type="radio" name="bank" id="COMM" value=""> 
                        <label class="icon-box" for="COMM"> 
                        <span class="icon-box-sparkling" bbd="false"> </span> 
                        <span class="false icon COMM" title="交通银行"></span> 
                        <span class="bank-name">交通银行</span> 
                        </label> 
                    </li> 
                   
                    <li> 
                        <input type="radio" name="bank" id="CEB" value=""> 
                        <label class="icon-box" for="CEB"> 
                        <span class="icon-box-sparkling" bbd="false"> </span> 
                        <span class="false icon CEB" title="中国光大银行"></span> 
                        <span class="bank-name">中国光大银行</span> 
                        </label> 
                    </li> 
                    </ul> --%>
                   
                   
                <%--</div>--%> 
                
            <%--</div>--%> 
        <%--</div>--%>
    <%--</div>--%>
</asp:Content>
