<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>
<%@ Register src="ascx/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function submitUser() {
            if (document.getElementById("loginUser").value == "") {
                alert("用户名不能为空")
                return false;
            }
            document.forms[0].submit();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        用户名<input type="text" name="loginUser"  id="loginUser" value="" /> <input type="button" onclick="submitUser()" value="进入聊天" />
        <uc1:LeftMenu ID="LeftMenu1" runat="server" />
    </div>
    </form>
</body>
</html>

