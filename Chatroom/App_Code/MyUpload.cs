using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// MyUpload说明
/// </summary>
public class MyUpload
{
    private string path = null;
    private string fileType = null;
    private int sizes = 0;
    private HttpPostedFile postedFile = null;
    /// <summary>
    /// 初始化变量
    /// </summary>
    public MyUpload()
    {
    }
    //
    public HttpPostedFile PostedFile
    {
        get
        {
            return postedFile;
        }
        set
        {
            postedFile = value;
        }
    }
    /// <summary>
    /// 设置上传路径,如:upload
    /// </summary>
    public string Path
    {
        get
        {
            return path;
        }
        set
        {
            path = @"\" + value + @"\";
        }
    }

    /// <summary>
    /// 设置上传文件大小,单位为KB
    /// </summary>
    public int Sizes
    {
        get
        {
            return sizes;
        }
        set
        {
            sizes = value * 1024;
        }
    }

    /// <summary>
    /// 上传文件的类型
    /// </summary>
    public string FileType
    {
        get
        {
            return fileType;
        }
        set
        {
            fileType = value;
        }
    }
    public string PathToName(string path)
    {
        int pos = path.LastIndexOf("\\");
        return path.Substring(pos + 1);
    }

    /// <summary>
    /// 上传
    /// </summary>

    public string Upload()
    {
        try
        {
            string filePath = null;
            
            //以当前时间修改图片的名字或创建文件夹的名字
            string modifyFileName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
            //获得站点的物理路径
            string uploadFilePath = null;

            uploadFilePath = System.Web.HttpContext.Current.Server.MapPath(".") + path;

            //获得文件的上传的路径
            string sourcePath = PathToName(PostedFile.FileName);
            //判断上传文件是否为空
            if (sourcePath == "" || sourcePath == null)
            {

                //message("您没有上传图片！");
                return null;
            }
            //获得文件扩展名
            string tFileType = sourcePath.Substring(sourcePath.LastIndexOf(".") + 1);
            //获得上传文件的大小
            long strLen = PostedFile.ContentLength;
            //分解允许上传文件的格式
            string[] temp = fileType.Split('|');
            //设置上传的文件是否是允许的格式
            bool flag = false;
            //判断上传文件大小
            if (strLen >= sizes)
            {

                message("上传的文件不能大于" + sizes + "KB");
                return null;
            }
            //判断上传的文件是否是允许的格式
            foreach (string data in temp)
            {
                if (data == tFileType)
                {
                    flag = true;
                    break;
                }
            }
            //如果为真允许上传,为假则不允许上传
            if (!flag)
            {
                message("目前本系统支持的格式为:" + fileType);
                message("文件上传不成功!");
                return null;
            }
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(uploadFilePath);
            //判断文件夹否存在,不存在则创建
            if (!dir.Exists)
            {
                dir.Create();
            }
            filePath = uploadFilePath + modifyFileName + "." + tFileType;
            
            PostedFile.SaveAs(filePath);
            filePath =  modifyFileName + "." + tFileType;

            return filePath;

        }
        catch
        {
            //异常
            message("出现未知错误！");
            return null;
        }
    }

    private void message(string msg, string url)
    {
        System.Web.HttpContext.Current.Response.Write("<script language=javascript>alert('" + msg + "');window.location='" + url + "'</script>");
    }

    private void message(string msg)
    {
        System.Web.HttpContext.Current.Response.Write("<script language=javascript>alert('" + msg + "');</script>");
    }
}
