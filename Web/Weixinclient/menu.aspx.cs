using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Net;
using System.IO;
using D2012.Common;
using D2012.Domain.Services;
using System.Configuration;

public partial class Weixinclient_menu : System.Web.UI.Page
{
    protected string mytoken = "";
    protected string mytokena = "";
    protected string setresult = "";
    ServiceCommon servComm = new ServiceCommon();


    protected string getToken = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        getToken = GetToken();
        if (!string.IsNullOrEmpty(yeyRequest.Params("upinfo")))
        {
            //设置menu
            string padata = yeyRequest.Params("txtmenu").Trim();
            string url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + getToken;//请求的URL
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(padata); // 转化
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "POST";
                webRequest.ContentLength = byteArray.Length;

                Stream newStream = webRequest.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length); //写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                setresult = "返回结果：" + sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        else if (!string.IsNullOrEmpty(yeyRequest.Params("hdel")))
        {

            string url = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token=" + getToken;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "get";
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
            setresult = "返回结果：" + sr.ReadToEnd();


        }      

    }

    private string GetToken()
    {
        string AppID = Session["AppID"].ToString();
        string AppSecret = Session["APPSECRET"].ToString();
        string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppID +"&secret=" + AppSecret;
        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
        webRequest.Method = "get";
        HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
        StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
        mytoken = sr.ReadToEnd();
        mytoken = mytoken.Substring(mytoken.IndexOf("token") + 8);
        mytoken = mytoken.Substring(0, mytoken.IndexOf("\""));
        return mytoken;
    }
}
