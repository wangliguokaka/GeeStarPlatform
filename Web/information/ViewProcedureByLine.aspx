<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewProcedureByLine.aspx.cs" Inherits="GeeStar_ViewProcedureByLine" %>

<%@ Register Src="~/ascx/pagecut.ascx" TagName="pagecut" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
       <!-- 引入ystep样式 -->
    <link rel="stylesheet" href="css/ystep.css"/>
    <script type="text/javascript" src="js/ystep.js"></script>
    <script type="text/javascript">

        var stepsJson = '<%=zNodes%>';
        var strPositionM = '<%=strPositionM%>';
        var arrayPositionM = strPositionM.split(',');
        var zNodesJson = $.parseJSON(stepsJson)
        var count = zNodesJson.length;
        //根据jQuery选择器找到需要加载ystep的容器
        //loadStep 方法可以初始化ystep
       
        $(function () {
            $.each(zNodesJson, function (i, item) {
                var teethPostion = item.teethPosition.split(',')
                var teethHtml = "<div><table id=\"teethStyle\" style=\"width:200px;height:40px;\"><tr><td style=\"border-right-style:solid;border-bottom-style:solid;width:100px;height:20px;text-align:right;\">" + teethPostion[0] + "</td><td style=\"border-bottom-style:solid;text-align:left;\">" + teethPostion[1] + "</td></tr><tr><td style=\"border-right-style:solid;text-align:right;\">" + teethPostion[2] + "</td><td style=\"text-align:left;\">" + teethPostion[3] + "</td></tr></table></div>";
                $("#stepCollection").append("<div style=\"font-weight:bold;font-size:14px;padding-top:15px;float:left;height:40px;\"> <%= GetGlobalResourceObject("SystemResource","ProductName").ToString() %>:" + item.productName + "</div><div  style=\"float:left;font-weight:bold;font-size:14px;padding-top:15px;\">&nbsp;&nbsp;&nbsp;&nbsp;<%= GetGlobalResourceObject("SystemResource","Count").ToString() %>:" + item.productNumber + "&nbsp;&nbsp;&nbsp;&nbsp;<%= GetGlobalResourceObject("SystemResource","ToothPosition").ToString() %>:</div>" + teethHtml)
                $("#stepCollection").append("<div style=\"float:left;clear:both;\"><table class=\"batchNo\" id=\"batchNo" + i + "\" style=\"width:600px;\"><tr><td><%= GetGlobalResourceObject("SystemResource","MaterialComposition").ToString() %>:</td><td><%= GetGlobalResourceObject("SystemResource","Manufacturer").ToString() %>:</td><td><%= GetGlobalResourceObject("SystemResource","LotNumber").ToString() %>:</td></tr>");
                $.ajax(
                    {
                        url: "ViewProcedureByLine.aspx?type=GetBatch&BatchOrderID=<%=orderID%>&BatchSerialID=<%=serialID%>&BatchProductID=" + item.productID,
                        async: false,
                        success: function (data)
                        {                            
                            $.each($.parseJSON(data), function (k, itemData)
                            {
                                $("#batchNo" + i).append("<tr><td>" + itemData.ElementName + "</td><td>" + itemData.ElementMaker + "</td><td>" + itemData.BatchNo + "</td></tr>");
                            }
                            )
                    

                        },
                        cache:false
                    }
                    )
                $("#stepCollection").append("</table></div>");
                $("#stepCollection").append("<div class=\"ystep" + i + "\" style=\"clear:both;\" ></div>")

                $(".ystep" + i).loadStep({
                    //ystep的外观大小
                    //可选值:small,large
                    size: "large",
                    //ystep配色方案
                    //可选值:green,blue
                    color: "blue",
                    //ystep中包含的步骤
                    steps: item.procedure
                });

                $(".ystep" + i).setStep(parseInt(arrayPositionM[i]));
            });
            
            
        })
       
        function BackPage() {
            art.dialog.list['PIC011'].close();
        }         
    </script>
        <style type="text/css">
          
        .tlist tr {
         height:45px;
        }

        #dcommentbox td.title {
            text-align:right;
            border-bottom-style:solid;
            border-bottom-width:1px;
        }

         #dcommentbox td.titlevalue {
            text-align:left;
            border-bottom-style:solid;
            border-bottom-width:1px;
            padding-left:8px;
        }

            #teethStyle td {
                border-width:1px;
            }
        .batchNo td
        {
            border-style:solid;
            border-width:1px;
            text-align:center;
        }

        .batchNo tr
        {
            border-style:solid;
            border-width:1px;
        }
        .expertnumber
        {
	        margin: 10px 0px 5px 0px;
	        height: 40px;
	        font-size: 16px;
	        font-family: 微软雅黑;
	        color: #555;
	        background-color: #f1f7e6;
             width:860px;
	        line-height: 39px;
	        padding-left: 15px;
	        margin-top: 5px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
     <div class="expertnumber"><%= GetGlobalResourceObject("SystemResource","OrderMessage").ToString() %></div>
     <table id="dcommentbox" style="width:880px; line-height:40px;" >
            <tr>
                <td  style="width:60px; " class="title"><%= GetGlobalResourceObject("SystemResource","BarCode").ToString() %>:</td>
                <td style="width:190px;" class="titlevalue"><%=orderModel.Order_ID %></td>                
                <td style="width:100px;"class="title"><%= GetGlobalResourceObject("SystemResource","Hospital").ToString() %>:</td>
                <td style="width:190px;" class="titlevalue"><%=orderModel.hospital %></td>
                <td style="width:100px;"class="title"><%= GetGlobalResourceObject("SystemResource","Doctor").ToString() %>:</td>
                <td class="titlevalue"><%=orderModel.doctor %></td>
            </tr>
             <tr>
                <td class="title"><%= GetGlobalResourceObject("SystemResource","Patient").ToString() %>:</td>
                <td class="titlevalue"><%=orderModel.Patient %></td>
                <td class="title"><%= GetGlobalResourceObject("SystemResource","Sex").ToString() %>:</td>
                <td class="titlevalue"><%=orderModel.Sex %></td>
                <td style="width:100px;"class="title"><%= GetGlobalResourceObject("SystemResource","Age").ToString() %>:</td>
                <td class="titlevalue"><%=orderModel.Age %></td>
            </tr>
          <tr>
                <td class="title"><%= GetGlobalResourceObject("SystemResource","DateToFactory").ToString() %>:</td>
                <td class="titlevalue" colspan="5"><%=orderModel.indate %></td>
               <%-- <td class="title">保修期:</td>
                <td class="titlevalue" colspan="3"><%=orderModel. %></td>--%>
            </tr>
         </table>
     <div class="expertnumber"><%= GetGlobalResourceObject("SystemResource","ProductMessage").ToString() %>:</div>
     <div id="stepCollection" style="height:400px; width:880px; margin-left:10px;  margin-top:20px; overflow:scroll; ">
         
     </div>
    
    <div>
    <a href="javascript:void(0)" onclick="closeme()" class="btn01"><%= GetGlobalResourceObject("SystemResource","Return").ToString() %></a>
    </div>

    </form>
</body>
</html>
