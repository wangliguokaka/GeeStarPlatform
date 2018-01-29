using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace WXPay.V3Demo
{
    public class Interface_WxPay
    {


        //public const string APPID = Session["APPID"];
        public const string TENPAY = "1";
        public const string PARTNER = "1242234302";//商户号
        //public const string APPSECRET = Session["APPSECRET"].ToString();
        public static string PARTNER_KEY = "1242234302";//APPSECRET
        public const string OAUTH2 = "https://open.weixin.qq.com/connect/oauth2/authorize";
        public const string OAUTH2_ACCESS_TOKEN = "https://api.weixin.qq.com/sns/oauth2/access_token";
        //支付密钥Key(原来的Paysignkey )
        public const string PAY_SIGNKEY = "dfasfffffffffffa";//paysignkey(非appkey) 

        //服务器异步通知页面路径(流量卡)
        public static readonly string NOTIFY_URL_Card_Store = "/PaySuccess.aspx";// ConfigurationManager.AppSettings["WXPayNotify_URL_CardStore"].ToString();
        public static readonly string NOTIFY_URL_Card_User = "/PaySuccess.aspx"; //ConfigurationManager.AppSettings["WXPayNotify_URL_CardUser"].ToString();
        public static readonly string NOTIFY_URL_HB_Store = "/PaySuccess.aspx";// ConfigurationManager.AppSettings["WXPayNotify_URL_CardStore"].ToString();

        public static string Get_RequestHtml(string openid, string Bill_No, decimal Charge_Amt, string Body , string PayType,string APPID)
        {
            if (Body == "")
            {
                Body = "测试充值";
            }
            if (PayType == "") { 
                PayType = "card_store";
            }
            HttpContext Context = System.Web.HttpContext.Current;

            if (openid.Length == 0)
            {
                return "";
            }

            //支付完成后的回调处理页面,*替换成notify_url.asp所在路径
            string TENPAY_NOTIFY = NOTIFY_URL_Card_Store;
            if (PayType == "card_user")
            {
                //用户购买支付
                TENPAY_NOTIFY = NOTIFY_URL_Card_User;
            }
            else if (PayType == "hb_store")
            {
                //店铺红包充值
                TENPAY_NOTIFY = NOTIFY_URL_HB_Store;
            }

            //设置package订单参数
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            string total_fee = (Charge_Amt*100).ToString("f0");
            string wx_timeStamp = "";
            string wx_nonceStr = Interface_WxPay.getNoncestr();

            dic.Add("appid", APPID);
            dic.Add("mch_id", PARTNER);//财付通帐号商家
            dic.Add("device_info", "1000");//可为空
            dic.Add("nonce_str", wx_nonceStr);
            dic.Add("trade_type", "JSAPI");
            dic.Add("attach", "att1");
            dic.Add("openid", openid);
            dic.Add("out_trade_no", Bill_No);		//商家订单号
            dic.Add("total_fee", total_fee); //商品金额,以分为单位(money * 100).ToString()
            dic.Add("notify_url", TENPAY_NOTIFY.ToLower());//接收财付通通知的URL
            dic.Add("body", Body);//商品描述
            dic.Add("spbill_create_ip", Context.Request.UserHostAddress);   //用户的公网ip，不是商户服务器IP

            string get_sign = BuildRequest(dic, PARTNER_KEY);

          

            string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            string _req_data = "<xml>";
            _req_data += "<appid>" + APPID + "</appid>";
            _req_data += "<attach><![CDATA[att1]]></attach>";
            _req_data += "<body><![CDATA[" + Body + "]]></body> ";
            _req_data += "<device_info><![CDATA[1000]]></device_info> ";
            _req_data += "<mch_id><![CDATA[" + PARTNER + "]]></mch_id> ";
            _req_data += "<openid><![CDATA[" + openid + "]]></openid> ";
            _req_data += "<nonce_str><![CDATA[" + wx_nonceStr + "]]></nonce_str> ";
            _req_data += "<notify_url><![CDATA[" + TENPAY_NOTIFY.ToLower() + "]]></notify_url> ";
            _req_data += "<out_trade_no><![CDATA[" + Bill_No + "]]></out_trade_no> ";
            //_req_data += "<spbill_create_ip><![CDATA[" + Context.Request.UserHostAddress + "]]></spbill_create_ip> ";
            _req_data += "<spbill_create_ip><![CDATA[59.45.67.233]]></spbill_create_ip> ";
            _req_data += "<total_fee><![CDATA[" + total_fee + "]]></total_fee> ";
            _req_data += "<trade_type><![CDATA[JSAPI]]></trade_type> ";
            //_req_data += "<sign><![CDATA[" + get_sign + "]]></sign> ";
            _req_data += "</xml>";

            //通知支付接口，拿到prepay_id
            ReturnValue retValue = StreamReaderUtils.StreamReader(url, Encoding.UTF8.GetBytes(_req_data), System.Text.Encoding.UTF8, true);

            //设置支付参数
            XmlDocument xmldoc = new XmlDocument();

            xmldoc.LoadXml(retValue.Message);

            XmlNode Event = xmldoc.SelectSingleNode("/xml/prepay_id");

            string return_json = "";

            if (Event != null)
            {
                string _prepay_id = Event.InnerText;

                SortedDictionary<string, string> pay_dic = new SortedDictionary<string, string>();

                wx_timeStamp = Interface_WxPay.getTimestamp();
                wx_nonceStr = Interface_WxPay.getNoncestr();

                string _package = "prepay_id=" + _prepay_id;

                pay_dic.Add("appId", APPID);
                pay_dic.Add("timeStamp", wx_timeStamp);
                pay_dic.Add("nonceStr", wx_nonceStr);
                pay_dic.Add("package", _package);
                pay_dic.Add("signType", "MD5");

                string get_PaySign = BuildRequest(pay_dic, PARTNER_KEY);

                return_json = JsonUtils.SerializeToJson(new
                {
                    appId = APPID,
                    timeStamp = wx_timeStamp,
                    nonceStr = wx_nonceStr,
                    package = _package,
                    paySign = get_PaySign,
                    signType = "MD5"
                });
            }
            else {
                return retValue.Message;
            }

            return return_json;
        }

        #region 取得OAuth2 URL地址
        public static string OAuth2_GetUrl_Pay(string URL,string APPID)
        {
            int Scope = 0;
            if (URL.IndexOf("ShareSceneryOrder.aspx") > -1)
            {
                Scope = 1;
            }
            StringBuilder sbCode = new StringBuilder(OAUTH2);
            sbCode.Append("?appid=" + APPID);
            sbCode.Append("&scope=" + (Scope == 0 ? "snsapi_userinfo" : "snsapi_base"));
            sbCode.Append("&state=" + "STATE");
            sbCode.Append("&redirect_uri=" + Uri.EscapeDataString(URL));
            sbCode.Append("&response_type=code#wechat_redirect");

            return sbCode.ToString();
        }
        #endregion

        #region 取得OAuth2 Access_Token
        public static ReturnValue OAuth2_Access_Token(string Code,string APPID,string APPSECRET)
        {
            StringBuilder sbCode = new StringBuilder(OAUTH2_ACCESS_TOKEN);
            sbCode.Append("?appid=" + APPID);
            sbCode.Append("&secret=" + APPSECRET);
            sbCode.Append("&code=" + Code);
            sbCode.Append("&grant_type=authorization_code");

            ReturnValue retValue = StreamReaderUtils.StreamReader(sbCode.ToString(), Encoding.UTF8);

            if (retValue.HasError)
            {
                return retValue;
            }

            try
            {
                int intWeixin_ExpiresIn = Convert.ToInt32(StringUtils.GetJsonValue(retValue.Message, "expires_in").ToString());
                retValue.PutValue("Weixin_OpenID", StringUtils.GetJsonValue(retValue.Message, "openid").ToString());
                retValue.PutValue("Weixin_Token", StringUtils.GetJsonValue(retValue.Message, "access_token").ToString());
                retValue.PutValue("Weixin_ExpiresIn", intWeixin_ExpiresIn);
                retValue.PutValue("Weixin_ExpiresDate", DateTime.Now.AddSeconds(intWeixin_ExpiresIn));
                retValue.PutValue("refresh_token", StringUtils.GetJsonValue(retValue.Message, "refresh_token").ToString());
                retValue.PutValue("scope", StringUtils.GetJsonValue(retValue.Message, "scope").ToString());
            }
            catch
            {
                retValue.HasError = true;
                retValue.Message = retValue.Message;
                retValue.ErrorCode = "";
            }

            return retValue;
        }
        #endregion

      
        public static string getNoncestr()
        {
            Random random = new Random();
            return GetMD5(random.Next(1000).ToString(), "GBK");
        }

        public static string getTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        public static string BuildRequest(SortedDictionary<string, string> sParaTemp, string key)
        {
            //获取过滤后的数组
            Dictionary<string, string> dicPara = new Dictionary<string, string>();
            dicPara = FilterPara(sParaTemp);

            //组合参数数组
            string prestr = CreateLinkString(dicPara);
            //拼接支付密钥
            string stringSignTemp = prestr + "&key=" + key;

            //获得加密结果
            string myMd5Str = GetMD5(stringSignTemp);

            //返回转换为大写的加密串
            return myMd5Str.ToUpper();
        }

        /// <summary>
        /// 除去数组中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        public static Dictionary<string, string> FilterPara(SortedDictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in dicArrayPre)
            {
                if (temp.Key != "sign" && !string.IsNullOrEmpty(temp.Value))
                {
                    dicArray.Add(temp.Key, temp.Value);
                }
            }

            return dicArray;
        }

        //组合参数数组
        public static string CreateLinkString(Dictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }

            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        //加密
        public static string GetMD5(string pwd)
        {
            MD5 md5Hasher = MD5.Create();

            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(pwd));

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        /** 获取大写的MD5签名结果 */
        public static string GetMD5(string encypStr, string charset)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch (Exception ex)
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }
            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }
    }
}