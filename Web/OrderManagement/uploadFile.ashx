<%@ WebHandler Language="C#" Class="ImageHandler" %>

using System;
using System.Web;

public class ImageHandler : IHttpHandler,System.Web.SessionState.IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {
        //-----------------设置类型和编码-----------------
        context.Response.ContentType = "text/html";
        context.Response.Charset = "utf-8";

        //-----------------初始化模板和变量-----------------
        //配置 iframe 模版
        //其中，wangEditor_uploadImg_assist.html 文件是wangEditor自带的页面，此处写上该页面在客户端的url地址即可；
        //其中，“{0}”是一个占位符，会被下文的代码替换掉；
        //不要忽略了其中的“#”；
        string iframeTemplate = "<iframe src=\"wangEditor_uploadImg_assist.html#{0}\"></iframe>";
        // result 将用来保存返回结果
        string result;

        //-----------------获取文件对象-----------------
        //注意：必须用 "wangEditor_uploadImg" 获取file！！！
        HttpPostedFile file = context.Request.Files["wangEditor_uploadImg"];

        //如果读取不到文件对象
        if (file == null)
        {
            result = string.Format(iframeTemplate, "上传失败，未获取到文件");
            //此时 result 已被赋值为“ <iframe src="http://localhost:8080/wangEditor_uploadImg_assist.html#上传失败，未获取到文件"></iframe> ”
        }
        else
        {
            //-----------------文件验证（此处自由发挥吧）-----------------
            //…大小…
            //…类型…
            //…安全性…
            //……

            //-----------------保存img文件-----------------
            //获取文件夹路径
            string path = context.Server.MapPath("~/uploadedFiles/"); //网站中有一个 uploadedFiles 文件夹，存储上传来的图片
            //生成文件名（系统要重新生成一个文件名，但注意扩展名要相同。千万不要用中文名称！！！）
            string originalFileName = file.FileName;
            string fileExtension = originalFileName.Substring(originalFileName.LastIndexOf('.'), originalFileName.Length - originalFileName.LastIndexOf('.'));
            string currentFileName = (new Random()).Next() + fileExtension;  //例如：可使用“随机数+扩展名”生成文件名
            //生成文件路径
            string imageUrl = path + currentFileName;
            //保存文件
            file.SaveAs(imageUrl);

            //-----------------获取文件url-----------------
            //获取img的url地址（如果 file.FileName 有汉字，要进行url编码）sxsx
            string url = context.Request.Url.GetLeftPart(UriPartial.Authority) + "/uploadedFiles/" + currentFileName;
            result = string.Format(iframeTemplate, "ok|" + url); //注意：结果的格式必须是 “ok|图片url地址” ！！！
            //此时 result 已被赋值为“<iframe src="http://localhost:8080/wangEditor_uploadImg_assist.html#ok|图片url地址"></iframe>”
        }

        //-----------------返回结果result-----------------
        context.Response.Write(result);
        context.Response.End();

    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}