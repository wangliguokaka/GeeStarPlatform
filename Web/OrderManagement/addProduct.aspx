<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addProduct.aspx.cs" Inherits="OrderManagement_addProduct" %>

<%@ Register Src="~/ascx/pagecut.ascx" TagName="pagecut" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="images/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        $(document).ready(function () {
            BindItemName();
            $("[id*=ddlSmallClass]").change(function () { BindItemName(); });
        });

        function BindItemName() {
            var ddlProductId = '<%=worderDetail.ProductId %>';
             $("[id*=ddlItemName]").html("");
             var strId = encodeURI($("[id*=ddlSmallClass]")[0].value, "utf-8");
             if (strId != -1) {
                 $.getJSON("../Handler/GetDataHandler.ashx?IsCN=<%=IsCN %>&ddlType=SmallClass&ddlId=" + strId, function (data) {

                     for (var i = 0; i < data.length; i++) {
                         if (data[i].ID == ddlProductId) {
                             $("select[name$=ddlItemName]").append($("<option selected></option>").val(data[i].ID).html(data[i].Cname));
                         } else {
                             $("select[name$=ddlItemName]").append($("<option></option>").val(data[i].ID).html(data[i].Cname));
                         }
                     };

                 });
             }
        }

        function AddProducts() {
            var date = Date.parse(new Date());
            var itemID = $("[id*=ddlItemName]").val();
            var number = $("#productNumber").val();
            if (number == "")
            {
                alertdialog("<%= GetGlobalResourceObject("Resource","InputProductQuantity").ToString() %>");
                return false;
            }

            if (number == 0) {
                alertdialog("<%= GetGlobalResourceObject("Resource","InputProductQuantityMoreThan0").ToString() %>");
                return false;
            }
            //var productclass = encodeURI($("#ddlClass").val(), "utf-8");
            //var productItem = encodeURI($("#ddlClass").val(), "utf-8");
            var righttop = $("#righttop").val();
            var lefttop = $("#lefttop").val();
            var rightbottom = $("#rightbottom").val();
            var leftbottom = $("#leftbottom").val();
            var PositionList = $("#PositionList").val()
            if (itemID == null)
            {
                alertdialog("<%= GetGlobalResourceObject("Resource","PleaseSelectProduct").ToString() %>");
                return false;
            }
            $("#ProductList tr:not(:first)").remove();
            $.get("../Handler/GetDataHandler.ashx?t=date&subID=<%=subID%>&PositionList=" + PositionList + "&IsCN=<%=IsCN %>&ddlType=AddProducts&itemID=" + itemID, { righttop: righttop, lefttop: lefttop, rightbottom: rightbottom, leftbottom: leftbottom, number: number, ColorTypeName: $("#ColorTypeName").val() }, function (dataResult) {
                $("#ProductList").append(dataResult);
            });
            art.dialog.list['PIC0009'].close();
        }

        function addColorType() {
            dialog = art.dialog({
                id: 'PIC002',
                title: "<%= GetGlobalResourceObject("Resource","PleaseSelectColor").ToString() %>",
                background: '#000',
                opacity: 0.3,
                close: function () {
                }
            });

            $.ajax({
                url: "addColorForProduct.aspx?TypeList=" + $("#ColorTypeName").val(),
                success: function (data) {
                    dialog.content(data);
                },
                cache: false
            });
        }

        function addToothPosition() {

            dialog = art.dialog({
                id: 'PIC0021',
                title: "<%= GetGlobalResourceObject("Resource","SelectTeeth").ToString() %>",
                width: 1000,
                height: 240,
                background: '#000',
                opacity: 0.3,
                close: function () {
                }
            });

            $.ajax({
                url: "SelectTeethPosition.aspx?PositionList=" + $("#PositionList").val(),
                success: function (data) {
                    dialog.content(data);
                },
                cache: false
            });
        }

        function onlyNum() 
        {
            if(!(event.keyCode==46)&&!(event.keyCode==8)&&!(event.keyCode==37)&&!(event.keyCode==39)) 
                if (!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)))
                    event.returnValue=false; 
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
             width:480px;
	        line-height: 39px;
	        padding-left: 15px;
	        margin-top: 5px;
        }

        .leftMargin {
         text-align:left !important;
         padding-left:35px !important;
        }
        .auto-style1 {
            height: 30px;
        }
        .auto-style2 {
            height: 30px;
            width: 158px;
        }
        .auto-style3 {
            width: 158px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <input id="hsysuserid" name="hsysuserid" type="hidden" value="" />
    <input id="TypeList" name="TypeList" type="hidden" value="<%=TrimWithNull(worderDetail.bColor) %>" /> 
    <input id="PositionList" name="PositionList" type="hidden" value="<%=worderDetail.PositionList %>" />
    <div class="expertnumber"><%= GetGlobalResourceObject("Resource","InputProduct").ToString() %></div>
    <table class="tlist">
        <tr>
            <td style="width:100px;"><%= GetGlobalResourceObject("Resource","SelectTeeth").ToString() %>:</td>
            <td>
                <table cellpadding="0" border="0" cellspacing="0">
                    <tr>
                         <td rowspan="2"><%= GetGlobalResourceObject("Resource","Right").ToString() %></td>
                            <td style="border-bottom:1px solid black ;border-right:1px solid black ;" class="auto-style1"><input type="text" id="righttop" name="righttop" style="text-align:right;" value="<%=TrimWithNull(worderDetail.a_teeth) %>" readonly="readonly" onfocus="addToothPosition()" /></td>
                            <td style="border-bottom:1px solid black;" class="auto-style2" colspan="0"><input type="text" id="lefttop" value="<%=TrimWithNull(worderDetail.b_teeth) %>" name="lefttop"  onfocus="addToothPosition()"/></td>
                            <td rowspan="2"><%= GetGlobalResourceObject("Resource","Left").ToString() %></td>
                    </tr>
                    <tr>
                        <td style="border-right:1px solid black;" colspan="0"><input type="text" value="<%=TrimWithNull(worderDetail.c_teeth) %>" id="rightbottom" style="text-align:right;" name="rightbottom"  onfocus="addToothPosition()"/></td>
                        <td class="auto-style3" ><input type="text" id="leftbottom"  name="leftbottom" value="<%=TrimWithNull(worderDetail.d_teeth) %>" onfocus="addToothPosition()"/></td>
                    </tr>
                </table>
            </td>                     
        </tr>
       
        <tr>
            <td><%= GetGlobalResourceObject("Resource","SmallProduct").ToString() %>：</td>
            <td class="leftMargin"><asp:DropDownList ID="ddlSmallClass" Width="150px" runat="server" ></asp:DropDownList></td>
           <script type="text/javascript">
             

                </script>
        </tr>
        <tr>
              <td><%= GetGlobalResourceObject("Resource","Product").ToString() %>:</td>
              <td class="leftMargin"><asp:DropDownList ID="ddlItemName" Width="150px" runat="server" ></asp:DropDownList></td>
        </tr>
        <tr>
            <td><%= GetGlobalResourceObject("Resource","Color").ToString() %>:</td>
            <td class="leftMargin">
                <a class="abtn01" href="javascript:void(0)" onclick="addColorType()"><%= GetGlobalResourceObject("Resource","SelectColor").ToString() %></a><label style="display:none;" id="ColorTypeName1"><%=worderDetail.bColor %></label><input type="text" name="ColorTypeName" id="ColorTypeName" value="<%=worderDetail.bColor==null?"":worderDetail.bColor.Trim() %>" />    
            </td>
        </tr>
        <tr>
            <td><%= GetGlobalResourceObject("Resource","Quantity").ToString() %>:</td>
            <td class="leftMargin"><input type="text" id="productNumber"  onkeydown="onlyNum()" maxlength="5" style="ime-mode:disabled;Width:146px;" value="<%=worderDetail.Qty %>" name="lefttop" /></td>
        </tr>
    </table>
    <div>
        <a href="javascript:void(0)" class="btn01" onclick="AddProducts();"><%= GetGlobalResourceObject("Resource","SaveProduct").ToString() %></a>
        <a href="javascript:void(0)" onclick="closeme()" class="btn01"><%= GetGlobalResourceObject("Resource","Cancel").ToString() %></a>
    </div>

    <script type="text/javascript">
        //GoPage(1);
    </script>

    </form>
</body>
</html>
