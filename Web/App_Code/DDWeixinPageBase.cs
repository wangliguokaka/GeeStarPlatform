using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Text.RegularExpressions;

using D2012.Domain.Entities;
using D2012.Common;
using D2012.Domain.Services;
using D2012.Common.DbCommon;
using WXPay.V3Demo;
using DDTourCommon.WXPay;
using System.Text;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;

/// <summary>
///AdminPageBase 的摘要说明



/// </summary>
public class DDWeixinPageBase : System.Web.UI.Page
{

    public string WeixinOpenID
    {

        get
        {
            if (Session["WeixinOpenID"] == null) {
                return null;
            }
            else {
                return Session["WeixinOpenID"].ToString();
            }
        }
    }


    /// <summary>
    /// doAjax
    /// </summary>
    protected virtual void doAjax()
    {
        Response.End();
    }

    protected override void Render(HtmlTextWriter writer)
    {
        if (Request.Params["_xml"] == null)
        {
            base.Render(writer);
        }
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreLoad(object sender, EventArgs e)
    {
        if (Session["WeixinOpenID"] == null && (Request.RawUrl.IndexOf("/Weixinclient/index.aspx") == -1 && Request.RawUrl.IndexOf("/Weixinclient/ShareSceneryOrder.aspx") == -1))
        {
            HttpContext.Current.Response.Redirect("/Weixinclient/index.aspx");
        }
        else
        {
           
        }
    }

    public ReturnValue GetUserInfo(ref string strAccess_Token,string APPID, string APPSECRET)
    { 
        
        string timeStamp = "";

        const string OAUTH2 = "https://open.weixin.qq.com/connect/oauth2/authorize";
        const string OAUTH2_ACCESS_TOKEN = "https://api.weixin.qq.com/sns/oauth2/access_token";
        string strWeixin_OpenID = "";

        timeStamp = TenpayUtil.getTimestamp();
        string strCode = Request["code"] == null ? "" : Request["code"];
        if (string.IsNullOrEmpty(strCode))
        {
            //参数需要自己处理
            string _OAuth_Url = Interface_WxPay.OAuth2_GetUrl_Pay(Request.Url.ToString(), APPID);
            Response.Redirect(_OAuth_Url);
            return null;
        }
        else
        {
            ReturnValue retValue = Interface_WxPay.OAuth2_Access_Token(strCode,APPID, APPSECRET);
            if (retValue.HasError)
            {
               // WriteFile(Server.MapPath("") + "\\Log.txt", "ewrwrwrwrwrwrer");
               
                Response.Write("获取code失败：" + retValue.Message);
                return null;
            }

           // WriteFile(Server.MapPath("") + "\\Log.txt", "11111111111111111");
            strWeixin_OpenID = retValue.GetStringValue("Weixin_OpenID");
            string strWeixin_Token = retValue.GetStringValue("Weixin_Token");
            strAccess_Token = strWeixin_Token;
            string refresh_token = retValue.GetStringValue("refresh_token");
            //WriteFile(Server.MapPath("") + "\\Log.txt", "2222222222222222");
            //  Response.Write(strWeixin_OpenID);
            if (string.IsNullOrEmpty(strWeixin_OpenID))
            {
                Response.Write("openid出错");
                return null;
            }

            string userinfotoken = "https://api.weixin.qq.com/sns/userinfo?access_token=" + strWeixin_Token + "&openid=" + strWeixin_OpenID + "&lang=zh_CN";

            retValue = StreamReaderUtils.StreamReader(userinfotoken, Encoding.UTF8);
            return retValue;
        }
    }

    public static void WriteFile(string pathWrite, string content)
    {
        if (File.Exists(pathWrite))
        {
            //File.Delete(pathWrite);
        }
        File.AppendAllText(pathWrite, content + "\r\n支付信息\r\n",
            Encoding.GetEncoding("utf-8"));
    }

    //获取微信凭证access_token的接口
    public static string getAccessTokenUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";

    #region 获取微信凭证
    public string GetAccessToken()
    {
        string accessToken = "";
        //获取配置信息Datatable



        string respText = "";
        //获取appid和appsercret

        string appid = "wx41e370706463eb12";
        string APPSECRET = "2243b9424f6f954cb42a7f56232ac549";
        //获取josn数据
        string url = string.Format(getAccessTokenUrl, appid, APPSECRET);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        using (Stream resStream = response.GetResponseStream())
        {
            StreamReader reader = new StreamReader(resStream, Encoding.Default);
            respText = reader.ReadToEnd();
            resStream.Close();
        }
        JavaScriptSerializer Jss = new JavaScriptSerializer();
        Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);
        //通过键access_token获取值
        accessToken = respDic["access_token"].ToString();


        return accessToken;
    }

    #endregion 获取微信凭证
    public static string MessageCheckCode(string telephoneNumber)
    {
        string messageCode =  GetRandom();
        string url = InitUrl("69105:admin", "12869200", telephoneNumber, "您的短信验证码是:" + messageCode);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "get";
        request.ContentType = "text/html;charset=gbk";
        HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
        Stream s=resp.GetResponseStream();
        StreamReader sr = new StreamReader(s);
        string msg = sr.ReadToEnd();
        msg = HttpUtility.UrlDecode(msg,Encoding.GetEncoding("gbk"));
        return messageCode+"|"+msg;
    }

    static string InitUrl(string username, string password, string phone, string content)
    {
        content=HttpUtility.UrlEncode(content,Encoding.GetEncoding("gbk"));
        string url = string.Format(@"http://GATEWAY.IEMS.NET.CN/GsmsHttp?username={0}&password={1}&to={2}&content={3}",username,password,phone,content);
        return url;
    }

    public static string GetRandom() { 
        string num = "";
        Random raninit = new Random(DateTime.Now.Millisecond);
        for (int i = 0; i < 6; i++) {
            Random ran = new Random(raninit.Next(i,int.MaxValue));
            num = num + ran.Next(1, int.MaxValue).ToString().Substring(1, 1);
        }
        return num;
    }

    protected static string LimitStringLength(String str, int limitLength)
    {
        if (str.Length > limitLength)
        {
            return str.Substring(0, limitLength);
        }
        else
        {
            return str;
        }
    }
}
