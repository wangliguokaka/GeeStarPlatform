<%@ Page Language="C#" CodeFile="Capture.aspx.cs" Inherits="Capture" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <title></title>
</head>
<body>
<div>
    <object id="myActiveX" classid="clsid:21B1549F-F3C6-4624-BE7F-AC576C976719">
        <param name="Message" value="当前属性由param参数赋值成功" />
    </object>
	<input type='button' onclick='myActiveX.Message = "当前属性由js赋值成功"; myActiveX.ShowMessage()'
	    value='用js和控件交互' />
</div>

</body>
</html>
