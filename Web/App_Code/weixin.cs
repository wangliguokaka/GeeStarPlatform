using System;
using System.Data;
using System.Text;
using System.Web;
using System.Configuration;

using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Collections.Generic;
using System.Xml;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

using D2012.Domain.Services;
using D2012.Common.DbCommon;
using D2012.Common;

namespace WeixinSDK
{
    public class weixin
    {
        private string Token = "caocaojiahanguodaigou";

        ServiceCommon servComm = new ServiceCommon();
        ConditionComponent ccWhere = new ConditionComponent();
        public void Auth()
        {
            string echoStr = System.Web.HttpContext.Current.Request.QueryString["echoStr"];

            if (CheckSignature())
            {
                if (!string.IsNullOrEmpty(echoStr))
                {
                    System.Web.HttpContext.Current.Response.Write(echoStr);
                    System.Web.HttpContext.Current.Response.End();
                }
            }
            else
            {
                System.Web.HttpContext.Current.Response.Write("false");
                System.Web.HttpContext.Current.Response.End();
            }
        }

        public string Handle(string postStr, string schoolid)
        {
            //log
            //ServiceCommon servComm = new ServiceCommon();
            //ERROR_MESSAGEINFO einfo = new ERROR_MESSAGEINFO();
            //einfo.CHANNEL = "微信调用";
            //einfo.DISCRIPTION = "Handle：in " + postStr;
            //einfo.CREATEDATE = DateTime.Now;

            //einfo.ISDEL = 0;

            //servComm.Add(einfo);

            //封装请求类，将数据post过来的数据封装到xml中，以便解析返回。

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(postStr);
            XmlElement rootElement = doc.DocumentElement;

            XmlNode MsgType = rootElement.SelectSingleNode("MsgType");

            RequestXML requestXML = new RequestXML();
            requestXML.ToUserName = rootElement.SelectSingleNode("ToUserName").InnerText;
            requestXML.FromUserName = rootElement.SelectSingleNode("FromUserName").InnerText;
            requestXML.CreateTime = rootElement.SelectSingleNode("CreateTime").InnerText;
            requestXML.MsgType = MsgType.InnerText;
            

           
           
           
            if (requestXML.MsgType == "text")
            {
                requestXML.Content = rootElement.SelectSingleNode("Content").InnerText;
            }
            else if (requestXML.MsgType == "location")
            {
                requestXML.Location_X = rootElement.SelectSingleNode("Location_X").InnerText;
                requestXML.Location_Y = rootElement.SelectSingleNode("Location_Y").InnerText;
                requestXML.Scale = rootElement.SelectSingleNode("Scale").InnerText;
                requestXML.Label = rootElement.SelectSingleNode("Label").InnerText;
            }
            else if (requestXML.MsgType == "image")
            {
                requestXML.PicUrl = rootElement.SelectSingleNode("PicUrl").InnerText;
            }
            else if (requestXML.MsgType == "event")
            {
                requestXML.Event = rootElement.SelectSingleNode("Event").InnerText;
                requestXML.EventKey = rootElement.SelectSingleNode("EventKey").InnerText;

                if (requestXML.Event == "scancode_waitmsg")
                {
                    requestXML.ScanResult = rootElement.SelectSingleNode("ScanCodeInfo").SelectSingleNode("ScanResult").InnerText;
                    //einfo.CHANNEL = "微信调用";
                    //einfo.DISCRIPTION = "::::" + requestXML.Event + " - " + requestXML.ScanResult;
                    //einfo.CREATEDATE = DateTime.Now;
                    //einfo.ISDEL = 0;

                    //servComm.Add(einfo);
                }
            }
            else if (requestXML.MsgType == "video")
            {
                requestXML.MediaId = rootElement.SelectSingleNode("MediaId").InnerText;
            }
            else
            {
                requestXML.Content = rootElement.SelectSingleNode("Content").InnerText;
            }
            GeeStar.Workflow.Common.Log.LogError("FromUserName:" + requestXML.FromUserName);
            GeeStar.Workflow.Common.Log.LogError("weixin.cs ip:" + HttpContext.Current.Request.UserHostAddress);
            //回复消息
            ResponseMsg(requestXML, schoolid);
           
            return requestXML.FromUserName;
        }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// * 将token、timestamp、nonce三个参数进行字典序排序


        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。

        
        /// <returns></returns>
        private bool CheckSignature()
        {
            string signature = System.Web.HttpContext.Current.Request.QueryString["signature"];
            string timestamp = System.Web.HttpContext.Current.Request.QueryString["timestamp"];
            string nonce = System.Web.HttpContext.Current.Request.QueryString["nonce"];

            //System.Web.HttpContext.Current.Response.End();
            string[] ArrTmp = { Token, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 回复消息(微信信息返回)
        /// </summary>
        /// <param name="weixinXML"></param>
        private void ResponseMsg(RequestXML requestXML, string schoolid)
        {

            //ERROR_MESSAGEINFO einfo = new ERROR_MESSAGEINFO();
            //einfo.CHANNEL = "微信调用";
            //einfo.DISCRIPTION = "ResponseMsg：in " + requestXML.Event + requestXML.EventKey;
            //einfo.CREATEDATE = DateTime.Now;
            //einfo.ISDEL = 0;
            //servComm.Add(einfo);

            try
            {
                string resxml = "";
                //mijiya mi = new mijiya(requestXML.Content, requestXML.FromUserName);

                if (requestXML.MsgType == "text")
                {
                    #region 客户端提交文本逻辑

                    if (requestXML.Content.ToLower().IndexOf("tp") == 0)
                    {

                        //投票出错，请稍后再试。
                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + "投票失败，请稍后再试！<a href=\"http://www.51yey.com/weixin/data/popbaby.aspx?schoolid=" + "\">点击查看详情</a>。" + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                    }
                    else if (requestXML.Content.IndexOf("#") == 0 && requestXML.Content.Length > 1)
                    {

                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + "设置失败，请重新设置！]]></Content><FuncFlag>1</FuncFlag></xml>";

                    }
                    else
                    {
                        resxml = getSubscribedata(requestXML.FromUserName, requestXML.ToUserName, schoolid);

                    }
                    #endregion 客户端提交文本逻辑
                }
                else if (requestXML.MsgType == "event")
                {
                    #region 客户端事件逻辑
                    if (requestXML.Event == "subscribe")
                    {
                        //用户关注时的动作
                        resxml = getSubscribedata(requestXML.FromUserName, requestXML.ToUserName, schoolid);
                    }
                }

                if (resxml == "")
                {
                }
                else
                {
                    System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    System.Web.HttpContext.Current.Response.Write(resxml);
                    System.Web.HttpContext.Current.Response.End();
                }
               
                //WriteToDB(requestXML, resxml, mi.pid);
            }
            catch (Exception ex)
            {
                //WriteTxt("异常：" + ex.Message + "Struck:" + ex.StackTrace.ToString());
                //wx_logs.MyInsert("异常：" + ex.Message + "Struck:" + ex.StackTrace.ToString());

                //codelog mylog = new codelog();

                //mylog.isdel = 0;
                //mylog.logtxt = "异常：" + ex.Message + "Struck:" + ex.StackTrace.ToString();
                //servComm.Add(mylog);
            }
        }

        private string getVxml(string toUsername, string fromUsername, string media_id)
        {
            StringBuilder sb = new StringBuilder();



            return sb.ToString();
        }

        private string getSubscribedata(string toUsername, string fromUsername, string schoolid)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<xml>");
            sb.AppendFormat("<ToUserName><![CDATA[{0}]]></ToUserName>", toUsername);
            sb.AppendFormat("<FromUserName><![CDATA[{0}]]></FromUserName>", fromUsername);
            sb.AppendFormat("<CreateTime>{0}</CreateTime>", ConvertDateTimeInt(DateTime.Now).ToString());
            sb.Append("<MsgType><![CDATA[news]]></MsgType>");
            sb.Append("<ArticleCount>1</ArticleCount>");
            sb.Append("<Articles>");
            sb.Append("<item>");
            sb.AppendFormat("<Title><![CDATA[{0}]]></Title> ", "欢迎使用吉星客户平台");
            sb.AppendFormat("<Description><![CDATA[{0}]]></Description>", "欢迎使用吉星客户平台欢迎使用吉星客户平台欢迎使用吉星客户平台欢迎使用吉星客户平台欢迎使用吉星客户平台欢迎使用吉星客户平台欢迎使用吉星客户平台欢迎使用吉星客户平台，详情垂询：400 8888 999！");
            sb.AppendFormat("<PicUrl><![CDATA[{0}]]></PicUrl>", "http://www.chaya8.com/images/tooth.jpg");
            sb.AppendFormat("<Url><![CDATA[{0}]]></Url>", "http://www.chaya8.com/");
            sb.Append("</item>");
            sb.Append("</Articles>");
            sb.Append("</xml>");

            return sb.ToString();

        }

        /// <summary>
        /// unix时间转换为datetime
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        private DateTime UnixTimeToTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// datetime转换为unixtime
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

                    #endregion
    }

    //微信请求类


    public class RequestXML
    {
        private string toUserName = "";
        /// <summary>
        /// 消息接收方微信号，一般为公众平台账号微信号


        /// </summary>
        public string ToUserName
        {
            get { return toUserName; }
            set { toUserName = value; }
        }

        private string fromUserName = "";
        /// <summary>
        /// 消息发送方微信号


        /// </summary>
        public string FromUserName
        {
            get { return fromUserName; }
            set { fromUserName = value; }
        }

        private string createTime = "";
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }

        private string msgType = "";
        /// <summary>
        /// 信息类型 地理位置:location,文本消息:text,消息类型:image
        /// </summary>
        public string MsgType
        {
            get { return msgType; }
            set { msgType = value; }
        }

        private string eventType = "";
        /// <summary>
        /// 信息类型 事件
        /// </summary>
        public string Event
        {
            get { return eventType; }
            set { eventType = value; }
        }

        private string eventKey = "";
        /// <summary>
        /// 信息类型 事件关键词

        /// </summary>
        public string EventKey
        {
            get { return eventKey; }
            set { eventKey = value; }
        }

        private string scanResult = "";

        public string ScanResult
        {
            get { return scanResult; }
            set { scanResult = value; }
        }

        private string content = "";
        /// <summary>
        /// 信息内容
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        private string mediaid = "";

        public string MediaId
        {
            get { return mediaid; }
            set { mediaid = value; }
        }

        private string location_X = "";
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public string Location_X
        {
            get { return location_X; }
            set { location_X = value; }
        }

        private string location_Y = "";
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Location_Y
        {
            get { return location_Y; }
            set { location_Y = value; }
        }

        private string scale = "";
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public string Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        private string label = "";
        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label
        {
            get { return label; }
            set { label = value; }
        }

        private string picUrl = "";
        /// <summary>
        /// 图片链接，开发者可以用HTTP GET获取
        /// </summary>
        public string PicUrl
        {
            get { return picUrl; }
            set { picUrl = value; }
        }
    }

}
