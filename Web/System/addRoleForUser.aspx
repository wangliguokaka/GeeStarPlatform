<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddRoleForUser.aspx.cs" Inherits="System_AddRoleForUser" %>

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
                url: "AddRoleForUser.aspx",
                cache: false,
                data: "type=GetJson&pageindex=" + iPageIndex + "&hPageNum=" + iPageCount,
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

            iPageCount = data.PageCount;
            var icounti = 0;
            $("#dcommentbox").html("<tr><th><%= GetGlobalResourceObject("SystemResource","Select").ToString() %></th><th><%= GetGlobalResourceObject("SystemResource","RoleName").ToString() %></th></tr>");
            $.each(data.replyJson, function(i, item) {
                $("#dcommentbox").append("<tr><td><input type=\"checkbox\" onclick=\"selsysuser(" + item.id + ",'" + item.role_name + "')\" name=\"radiocheck\" id=" + item.id + " /></td><td>" + item.role_name + "</td></tr>");
                icounti++;
            });
            //加载完评论后，应当重新设置分页

            if (icounti == 0) {
                $("#divPageCut").html("");
            } else {
                SetPageCut(iPageIndex);
            }

            //如果有已经选择的成员，标记一下

            var selusers = $("#hsysuserid").val().split(":");

            if (selusers == "") {
                selusers = '<%=Request["RoleID"]%>';
                $("#hsysuserid").val(selusers);
                selusers = selusers.split(":");
            }
            for (var i = 0; i < selusers.length; i++) {
                if (document.getElementById(selusers[i]) != null) {
                    document.getElementById(selusers[i]).checked = true;
                }

            }
        }
        var checkedFee = 0;    
        var RoleName = $("#RoleName").html()
        if (RoleName != "") {
            RoleName = "+" + RoleName;
        }
        function selsysuser(sysuserid, role_name) {
            var selusers = $("#hsysuserid").val();
            if (document.getElementById(sysuserid).checked == true) {
                selusers = selusers + ":" + sysuserid;
                RoleName = RoleName + "+" + role_name;
            } else {
                selusers = ":" + selusers + ":";
                selusers = selusers.replace(":" + sysuserid + ":", ":");
                RoleName = RoleName.replace("+" + role_name, "");
            }
            if (selusers.length > 0) {
                if (selusers.indexOf(":") == 0) {
                    selusers = selusers.substr(1, selusers.length - 1);
                }
                if (selusers.lastIndexOf(":") == selusers.length - 1) {
                    selusers = selusers.substr(0, selusers.length - 1);
                }
            }
            $("#hsysuserid").val(selusers);
        }

        //设置显示的分页

        function SetPageCut(iPageIndex) {

            var iIndex = iPageIndex;    //当前页数
            var iCount = iPageCount == 0 ? 1 : iPageCount;    //总页数


            var strHtml = "";

            if (iCount <= 8) {

                for (var i = 1; i <= iCount; i++) {
                    if (i == iIndex) {
                        strHtml += "<span style=\"padding-left:10px\">" + i + "</span>";
                    }
                    else {
                        strHtml += "<span><a href=\"javascript:void(0)\" onclick=\"GoPage(" + i + ");\">" + i + "</a></span>";
                    }
                }
            } else {

                if (iIndex <= 5) {
                    for (var j = 1; j <= 9; j++) {
                        if (j == iIndex) {
                            strHtml += "<span style=\"padding-left:10px\">" + j + "</span>";
                        }
                        else if (j == 8) {
                            strHtml += "<span style=\"padding-left:10px\">…</span>";
                        }
                        else if (j == 9) {
                            strHtml += "<span><a href=\"javascript:void(0)\" onclick=\"GoPage(" + iCount + ");\">" + iCount + "</a></span>";
                        }
                        else {
                            strHtml += "<span><a href=\"javascript:void(0)\" onclick=\"GoPage(" + j + ");\">" + j + "</a></span>";
                        }
                    }
                }
                else if (iCount - iIndex <= 3) {
                    for (var k = 1; k <= 9; k++) {
                        if (k == 1) {
                            strHtml += "<span><a href=\"javascript:void(0)\" onclick=\"GoPage(" + k + ");\">" + k + "</a></span>";
                        }
                        else if (k == 2) {
                            strHtml += "<span style=\"padding-left:10px\">…</span>";
                        }
                        else if ((parseInt(iCount) + parseInt(k) - 9) == iIndex) {
                            strHtml += "<span style=\"padding-left:10px\">" + iIndex + "</span>";
                        }
                        else {
                            strHtml += "<span><a href=\"javascript:void(0)\" onclick=\"GoPage(" + (parseInt(iCount) + parseInt(k) - 9) + ");\">" + (parseInt(iCount) + parseInt(k) - 9) + "</a></span>";
                        }
                    }
                }
                else {
                    for (var n = 1; n <= 10; n++) {
                        if (n == 1) {
                            strHtml += "<span><a href=\"javascript:void(0)\" onclick=\"GoPage(" + n + ");\">" + n + "</a></span>";
                        }
                        else if (n == 10) {
                            strHtml += "<span><a href=\"javascript:void(0)\" onclick=\"GoPage(" + iCount + ");\">" + iCount + "</a></span>";
                        }
                        else if (n == 2 || n == 9) {
                            strHtml += "<span style=\"padding-left:10px\">…</span>";
                        }
                        else if (n == 6) {
                            strHtml += "<span  style=\"padding-left:10px\">" + iIndex + "</span>";
                        }
                        else {
                            strHtml += "<span><a href=\"javascript:void(0)\" onclick=\"GoPage(" + (parseInt(iIndex) - 6 + parseInt(n)) + ");\">" + (parseInt(iIndex) - 6 + parseInt(n)) + "</a></span>";
                        }
                    }
                }
            }
            $("#divPageCut").html(strHtml);
        }
        function setMember() {
            $("#RoleID").val($("#hsysuserid").val());
            if (RoleName.length > 1) {
                RoleName = RoleName.substr(1, RoleName.length - 1);
            }
            $("#RoleName").html(RoleName);
            $("#hidRoleName").val(RoleName);         
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
    <input id="hsysuserid" name="hsysuserid" type="hidden" />
    <div>
        <table id="dcommentbox" style="width:400px" class="tlist">
            <tr><td></td></tr>
        </table>
        <div id="divPageCut" class="pagediv">
        </div>
    </div>
    <div>
    <a href="javascript:void(0)" onclick="setMember();" class="btn01"><%= GetGlobalResourceObject("SystemResource","Confirm").ToString() %>确 定</a>
    <a href="javascript:void(0)" onclick="closeme()" class="btn01"><%= GetGlobalResourceObject("SystemResource","Cancel").ToString() %>取 消</a>
    </div>

    <script type="text/javascript">
        GoPage(1);
    </script>

    </form>
</body>
</html>
