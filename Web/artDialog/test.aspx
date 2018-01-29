<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="artDialog_test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="../jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="artDialog.source.js?skin=default" type="text/javascript"></script>
    
    <script type="text/javascript">
//        function test01() {
//            var dialog = art.dialog({
//                title: '欢迎',
//                content: '欢迎使用artDialog对话框组件！',
//                icon: 'succeed',
//                follow: document.getElementById('btn2'),
//                ok: function() {
//                    this.title('警告').content('请注意artDialog两秒后将关闭！').lock().time(2);
//                    return false;
//                }
//            });
//        }
        function test01() {
//            var dialog = art.dialog({ id: 'N3880', title: '登录' });

//            // jQuery ajax   
//            $.ajax({
//            url: '/login/poplogin.aspx',
//                success: function(data) {
//                    dialog.content(data);
//                },
//                cache: false
//            });
            document.getElementById("ddd").style.zIndex = 10;
            alert(document.getElementById("ddd").style.zIndex);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="ddd">
        <input id="Button1" type="button" onclick="test01();" value="button" />
    </div>
    </form>
</body>
</html>
