using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.Data;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Web.Security;
using D2012.Domain.Services;
using WeixinSDK;
using D2012.Common;

public partial class Weixinclient_weixin : System.Web.UI.Page
{
    ServiceCommon servComm = new ServiceCommon();
    protected string openid;
    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            weixin _wx = new weixin();
            string postStr = "";
            if (Request.HttpMethod.ToLower() == "post")
            {
                Stream s = System.Web.HttpContext.Current.Request.InputStream;
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int)s.Length);
                postStr = Encoding.UTF8.GetString(b);

                if (!string.IsNullOrEmpty(postStr)) //请求处理
                {

                    //ERROR_MESSAGEINFO einfo = new ERROR_MESSAGEINFO();
                    //einfo.CHANNEL = "微信调用";
                    //einfo.DISCRIPTION = "微信调用：pageload out" + postStr;
                    //einfo.CREATEDATE = DateTime.Now;
                    //einfo.CREATEIP = Request.UserHostAddress;
                    //einfo.ISDEL = 0;

                    //servComm.Add(einfo);

                    openid = _wx.Handle(postStr, "14057");
                    GeeStar.Workflow.Common.Log.LogError("openid weixinaspx start:" + openid);
                    WriteCookie(UserConstant.COOKIE_SAVEDNEWOPENID, openid);
                    Session["newopenid"] = openid;
                    GeeStar.Workflow.Common.Log.LogError("openid weixinaspx end:" + openid);
                }
            }
            else
            {
                _wx.Auth();
            }

        }

    }
    public static void WriteCookie(string strName, string strValue)
    {
        HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
        if (cookie == null)
        {
            cookie = new HttpCookie(strName);
            HttpContext.Current.Response.AppendCookie(cookie);
            cookie.Expires = DateTime.Now.AddSeconds(30);
            cookie.Value = strValue;
        }
        else
        {
            cookie.Expires = DateTime.Now.AddSeconds(30);
            cookie.Value = strValue;
        }

        HttpContext.Current.Response.AppendCookie(cookie);
    }

}
