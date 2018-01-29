<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addOthers.aspx.cs" Inherits="OrderManagement_addOthers" %>

<%@ Register Src="~/ascx/pagecut.ascx" TagName="pagecut" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <script type="text/javascript">
        var iPageCount = 0;
        
        function GoPage(iPageIndex) {
           
            $.ajax({
                type: "GET",
                url: "addOthers.aspx",
                cache: false,
                data: "type=GetJson&pageindex=" + iPageIndex + "&hPageNum=" + iPageCount + "&cid=<%=cid %>",
                dataType: "json",
                success: function(data) {
                    //用到这个方法的地方需要重写这个success方法
                    
                    ReturnSuccess(data, iPageIndex);
                }
            });
        }

        //jquery返回数据执行success方法
        function ReturnSuccess(data, iPageIndex) {
            //将取到的数据集放入到页面中
            var icounti = 0;
            $("#dcommentbox").html("<tr><th style=\"text-align:center;\"><%= GetGlobalResourceObject("Resource","OrderOther").ToString() %></th><th style=\"text-align:center;\"><%= GetGlobalResourceObject("Resource","Quantity").ToString() %></th></tr>");
           
            $.each(data.DataAccessory, function (i, item) {
                
                $("#dcommentbox").append("<tr><td>" + item.Accessory + "</td><td><input type=\"textbox\" maxlength=\"3\" onkeydown=\"onlyNum()\"  name=\"numberAccessory\" id=" + item.Code + " /></td></tr>");
                icounti++;
            });

          

            //如果有已经选择的成员，标记一下

            var OtherList = '<%=Request["OtherList"]%>';
            if (OtherList != '') {
                OtherListSplit = OtherList.split(":");
                for (var i = 0; i < OtherListSplit.length; i++) {
                    var id = OtherListSplit[i].split(",")[0];
                    var number = OtherListSplit[i].split(",")[1];
                    if (document.getElementById(id) != null) {
                        document.getElementById(id).value = number;
                    }

                }
            }
           
        }
        var checkedFee = 0;    

        //设置显示的分页
        function setMember() {
            var AccessoryValue = '';
            $("#dcommentbox td input ").each(function () {
               
                if ($(this).val() != "" && $(this).val() > 0)
                {
                    AccessoryValue = AccessoryValue + ":" + $(this).attr("id") + "," + $(this).val();
                }
            })
            AccessoryValue = AccessoryValue.replace(":", "");
            $("#OtherList").val(AccessoryValue);
            dialog.close();
        }

       
    </script>

    <style type="text/css">
        .tablestyle
        {
            border: none;
        }
        .tablestyle td
        {
            text-align: center;
            line-height: 18px;
            border: none;
            border-bottom: 1px solid #CECECE;
        }
        .tablestyle th
        {
            line-height: 25px;
            border: none;
            border-bottom: 1px solid #CECECE;
        }
        .pagediv
        {
        	text-align:center;
        	margin:10px;
        	
        }
        .pagediv a
        {
        	padding-left:5px;
        	padding-right:5px;
        	border:1px solid #cccccc;
        	margin:3px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <input id="hsysuserid" name="hsysuserid" type="hidden" value="" />
    <div style="width:500px;height:500px; overflow-x:hidden;">
        <table id="dcommentbox" class="tlist">
            <tr><td></td></tr>
        </table>
        <div id="divPageCut" style="display:none;" class="pagediv">
        </div>
    </div>
    <div>
    <a href="javascript:void(0)" onclick="setMember();" class="btn01"><%= GetGlobalResourceObject("Resource","OK").ToString() %></a>
    <a href="javascript:void(0)" onclick="closeme()" class="btn01"><%= GetGlobalResourceObject("Resource","Cancel").ToString() %></a>
    </div>

    <script type="text/javascript">
        GoPage(1);
    </script>

    </form>
</body>
</html>
