<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectTeethPosition.aspx.cs" Inherits="OrderManagement_SelectTeethPositio" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
  
    <script type="text/javascript" src="../jQuery/jquery.form.js"></script>

    <script src="../jQuery/Jcrop/js/jquery.Jcrop.js" type="text/javascript"></script>

    <link href="images/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var listPosition = '';
        var srcGallery = new Array();
        var initedGallery = new Array();
        $(document).ready(function () {
           var selectedPosition = '<%=Request["PositionList"]%>';
            
            var topToothPosition = '';
            for (var i = 1; i <= 8; i++) {
                if (i == 1) {
                    topToothPosition = "<td style=\"cursor:pointer;\" id=\"righttop" + i.toString() + "\">" + i.toString() + "</td><td style=\" width:12px;margin:0 auto; border-style:none;padding:0px 0px 0px 0px;height:100%;\" rowspan=\"3\"><div style=\" magin-right:3px; border-right:1px solid #696969;width:5px;height:85px\"> </div></td><td style=\"cursor:pointer;\"  id=\"lefttop" + i.toString() + "\">" + i.toString() + "</td>";
                } else {
                    topToothPosition = "<td style=\"cursor:pointer;\"  id=\"righttop" + i.toString() + "\">" + i.toString() + "</td>" + topToothPosition + "<td style=\"cursor:pointer;\" id=\"lefttop" + i.toString() + "\">" + i.toString() + "</td>";
                }
            }
            topToothPosition = "<tr>" + topToothPosition + "</tr>";
            topToothPosition = topToothPosition + "<tr><td style=\" border-style:none;padding:0px 0px 0px 0px;height:1px;\" colspan=\"17\"><hr/></td></tr>";
            var bottomToothPosition = '';
            for (var i = 1; i <= 8; i++) {
                bottomToothPosition = "<td style=\"cursor:pointer;\" id=\"rightbottom" + i.toString() + "\">" + i.toString() + "</td>" + bottomToothPosition + "<td style=\"cursor:pointer;\" id=\"leftbottom" + i.toString() + "\">" + i.toString() + "</td>";
            }
            topToothPosition = topToothPosition + "<tr>" + bottomToothPosition + "</tr>";
            $("#dcommentbox").html(topToothPosition);
          

            $("#dcommentbox td").click(function () {
                if (typeof($(this).attr("id")) != "undefined") {
                    if ($(this).hasClass("selectedPosition")) {
                        $(this).removeClass("selectedPosition");
                    }
                    else {
                        $(this).addClass("selectedPosition");
                    }
                }               
            });

            $("#dcommentbox td").each(function (index, element) {
                if (typeof ($(this).attr("id")) != "undefined") {
                    if (selectedPosition.indexOf($(this).attr("id").toString()) > -1) {
                        $(this).addClass("selectedPosition");
                    }
                }
            })
          
        });

        function CancelPage()
        {
            art.dialog.list['PIC0021'].close();
        }

        var positionList = "";
        function setMember() {
            $("#righttop").val("");
            $("#lefttop").val("");
            $("#rightbottom").val("");
            $("#leftbottom").val("");
            $("#dcommentbox td").each(function (index, element) {
               
                if ($(this).hasClass("selectedPosition")) {
                    if ($(this).attr("id").toString().indexOf("righttop") > -1) {
                        $("#righttop").val($("#righttop").val() + "," + $(this).html());
                    }
                    else if ($(this).attr("id").toString().indexOf("lefttop") > -1) {
                        $("#lefttop").val($("#lefttop").val() + "," + $(this).html());
                    }
                    else if ($(this).attr("id").toString().indexOf("rightbottom") > -1) {
                        $("#rightbottom").val($("#rightbottom").val() + "," + $(this).html());
                    }
                    else {
                        $("#leftbottom").val($("#leftbottom").val() + "," + $(this).html());
                    }
                    positionList = positionList + "," + $(this).attr("id");
                }
            })
            TrimDot("righttop");
            TrimDot("lefttop");
            TrimDot("rightbottom");
            TrimDot("leftbottom");
            $("#PositionList").val(positionList);
            
            $("#productNumber").val($(".selectedPosition").length);
            art.dialog.list['PIC0021'].close();
            //dialog.close();
        }

        function TrimFistDot(textID) {
            var stringValue = $("#" + textID).val().toString();
            if (stringValue == "") {
            } else {
                $("#" + textID).val(stringValue.substr(1, stringValue.length-1));
            }

        }

        function TrimDot(textID) {
            var stringValue = $("#" + textID).val().toString();
            stringValue = stringValue.replace(/,/gm,'');
            $("#" + textID).val(stringValue);
        }


    </script>

    <style type="text/css">
        #dcommentbox td
        {
             width:30px;
        }
        
        .selectedPosition
        {
           background-color:#d6aa23;            
        }

        .expertnumber
        {
	        margin: 10px 0px 5px 0px;
	        height: 40px;
	        font-size: 16px;
	        font-family: 微软雅黑;
	        color: #555;
	        background-color: #f1f7e6;
             width:950px;
	        line-height: 39px;
	        padding-left: 15px;
	        margin-top: 5px;
        }

   
    </style>
</head>
<body>
   
    <form id="form1" runat="server">
    <input id="hsysuserid" name="hsysuserid" type="hidden" />
    <div style="width:100%;">
        <div class="expertnumber"><%= GetGlobalResourceObject("Resource","SelectTeeth").ToString() %></div>
        <div style="margin:0 auto; text-align:center;">
            <table id="dcommentbox" style="margin:auto;border-collapse: separate" class="tablestyle">
            </table>
        </div>
                <div style="clear:both; margin-top:20px;margin-left:40px;">
          <%--  <a class="btn01" href="javascript:void(0)"  onclick="uploadAttachment()">上传附件</a>
            <a class="btn01" href="javascript:void(0)" onclick="onCropClick()">保存图片</a>
            <input type="hidden" id="picfile" name="picfile" value="" />--%>
            <a href="javascript:void(0)" onclick="setMember();" class="btn01"><%= GetGlobalResourceObject("Resource","OK").ToString() %></a>
            <a href="javascript:void(0)" onclick="CancelPage()" class="btn01"><%= GetGlobalResourceObject("Resource","Cancel").ToString() %></a>
        </div>
        <input type="hidden"  id="x" name="x" />
        <input type="hidden"  id="y" name="y" />
        <input type="hidden"  id="w" name="w" />
        <input type="hidden"  id="h" name="h" />
     </div>
    </form>

</body>
</html>
