<%@ WebHandler Language="C#" Class="ImageHandler" %>

using System;
using System.Web;
using System.Xml.Serialization;
using System.IO;

public class ImageHandler : IHttpHandler,System.Web.SessionState.IRequiresSessionState {

    string result = "";
    public void ProcessRequest (HttpContext context) {

        if (context.Request["hdnCount"] != null && context.Request["hdnCount"] != "")
        {
            string random = context.Request["random"].ToString();
            int  hiddenCount = 0;
            if(!String.IsNullOrEmpty(context.Request["hdnCount"]))
            {
                hiddenCount = int.Parse(context.Request["hdnCount"]);
            }
           string strData = "";
            for(int i =1;i<=hiddenCount;i++){
                strData = strData + context.Request["fileCaptureImg" + i];
            }
  
            //string datefile = context.Request["fileCaptureImg"];
            string str = HttpUtility.UrlDecode(strData);

            XmlSerializer xs = new XmlSerializer(typeof(byte[]));
            StringReader sr = new StringReader(str);
            object obj = xs.Deserialize(sr);

            string picName = HttpContext.Current.Request.MapPath("/")+"uploadedFiles\\" + random+".png";
            ByteArrayToImage(picName, (byte[])obj, ((byte[])obj).Length);
            //File.WriteAllText("D:\\error.txt", "ok");

            result = context.Request.Url.GetLeftPart(UriPartial.Authority) + "/uploadedFiles/" + random+".png"; ; //注意：结果的格式必须是 “ok|图片url地址” ！！！

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

    public void ByteArrayToImage(string picName,byte[] byteArrayIn, int count)
    {
        MemoryStream ms = new MemoryStream(byteArrayIn, 0, count);
        System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
        returnImage.Save(picName);
    }

}