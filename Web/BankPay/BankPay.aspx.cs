using chinapaysecure;
using D2012.Common.DbCommon;
using D2012.Domain.Entities;
using D2012.Domain.Services;
using GeeStar.Workflow.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using upacp_sdk_net.com.unionpay.sdk;

public partial class BankPay_BankPay : PageBase
{
    ServiceCommon servComm = new ServiceCommon();
    ConditionComponent ccWhere = new ConditionComponent();
    protected int tcount;
    protected string hddpnumbers;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnConsume.Text = GetGlobalResourceObject("Resource", "ConfirmPay").ToString();
    }

    protected void btnConsume_Click(object sender, EventArgs e)
    {

        ccWhere.Clear();
        ccWhere.AddComponent("JGCBM", LoginUser.BelongFactory, SearchComponent.Equals, SearchPad.NULL);
        JX_USERS jxUser = servComm.GetEntity<JX_USERS>(null, ccWhere);
        if (!String.IsNullOrEmpty(jxUser.PayNoCardMerId) && Request["paymethod"] == "1" || Request["paymethod"] != "1" && !String.IsNullOrEmpty(jxUser.PayB2CMerId))
        {
            //Dictionary<string, string> param = new Dictionary<string, string>();
            //// 随机构造一个订单号（演示用）
            Random rnd = new Random();
            string orderID = DateTime.Now.ToString("yyyyMMddHHmmss") + ((rnd.Next(900) + 100).ToString() + "0").Substring(0, 2).Trim();

            ////填写参数

            //param["version"] = "5.0.0";//版本号
            //param["encoding"] = "UTF-8";//编码方式
            //param["certId"] = CertUtil.GetSignCertId();      //证书ID
            //param["txnType"] = "01";//交易类型
            //param["txnSubType"] = "01";//交易子类
            //param["bizType"] = "000201";//业务类型
            //param["frontUrl"] = "http://localhost:58826/BankPay/FrontRcvResponse.aspx?type=BankPay";    //前台通知地址      
            //param["backUrl"] = "http://222.222.222.222:8080/demo/utf8/BackRcvResponse.aspx";  //后台通知地址，改自己的外网地址
            //param["signMethod"] = "01";//签名方法
            //param["channelType"] = "08";//渠道类型，07-PC，08-手机
            //param["accessType"] = "0";//接入类型
            //param["merId"] = "481601512173917";//商户号，请改成自己的商户号 481601512173917,777290058110097
            //param["orderId"] = orderID;//商户订单号，可任意修改
            //param["txnTime"] = DateTime.Now.ToString("yyyyMMddHHmmss");//订单发送时间
            //param["txnAmt"] = "1";//交易金额，单位分
            //param["currencyCode"] = "156";//交易币种
            ////param["orderDesc"] = "订单描述";//订单描述，暂时不会起作用
            //param["reqReserved"] = "透传信息";//请求方保留域，透传字段，查询、通知、对账文件中均会原样出现

            //SDKUtil.Sign(param, Encoding.UTF8);
            //// 将SDKUtil产生的Html文档写入页面，从而引导用户浏览器重定向   
            //string html = SDKUtil.CreateAutoSubmitForm(SDKConfig.FrontTransUrl, param, Encoding.UTF8);
            //Response.ContentEncoding = Encoding.UTF8; // 指定输出编码
            //Response.Write(html);

            Hashtable myMap = new Hashtable();
            //无卡支付
            string payAmount = Request["actualAmount"];
           
            myMap.Add("MerOrderNo", orderID);
            myMap.Add("TranDate", DateTime.Now.ToString("yyyyMMdd"));
            myMap.Add("TranTime", DateTime.Now.ToString("hhmmss"));
            myMap.Add("OrderAmt", (int)(decimal.Parse(payAmount) * 100));
            //myMap.Add("OrderAmt", 1);
            myMap.Add("BusiType", "0001");
            myMap.Add("AccessType", "0");
            myMap.Add("AcqCode", "000000000000014");
            myMap.Add("MerPageUrl", Request.Url.GetLeftPart(UriPartial.Authority) + "/BankPay/FrontRcvResponse.aspx?type=BankPay");
            myMap.Add("MerBgUrl", Request.Url.GetLeftPart(UriPartial.Authority) + "/BankPay/BgRcvResponse.aspx?type=BankPay");
            myMap.Add("CurryNo", "CNY");

            //myMap.Add("SplitType", "");
            //myMap.Add("SplitMethod", "");
            //myMap.Add("MerSplitMsg", "");
            //myMap.Add("PayTimeOut", "145");
            myMap.Add("Version", "20140728");

            //myMap.Add("CommodityMsg", "ChinaPay测试-商品信息");
            //myMap.Add("MerResv", "ChinaPay测试-商户保留域");
            //myMap.Add("TranReserved", "{\"Referred\":\"www.chinapay.com\",\"BusiId\":\"0001\",\"TimeStamp\":\"1438915150976\",\"Remoteputr\":\"172.16.9.44\"}");  
            myMap.Add("TranType", "0001");
            SecssUtil obj = new SecssUtil();
            WNetPay payModel = new WNetPay();


            //TODO 其他特殊用法请查看 pages/api_01_gateway/special_use_purchase.htm
            if (Request["paymethod"] == "1")
            {
                payModel.PayMethod = "0";
                myMap.Add("MerId", jxUser.PayNoCardMerId);
                obj.init(Request.PhysicalApplicationPath + "/ChinaPay/" + jxUser.PayNoCardMerId + "/security.properties"); //初始化安全控件：
            }
            else
            {
                payModel.PayMethod = "1";
                //B2C支付 
                //myMap.Add("BankInstNo", "700000000000010");
                myMap.Add("MerId", jxUser.PayB2CMerId);
                obj.init(Request.PhysicalApplicationPath + "/ChinaPay/" + jxUser.PayB2CMerId + "/securityb2c.properties"); //初始化安全控件：
            }

            //bool ee = 

            
            obj.sign(myMap);
            String chkValue = obj.getSign();
            myMap.Add("Signature", chkValue);
            //obj.verify(myMap);
            if ("00" != obj.getErrCode())
            {
            }


            
            payModel.OrderID = orderID;
            payModel.UserID = CurrentUserID;
            payModel.PayAmount = decimal.Parse(payAmount);
            payModel.JGCBM = LoginUser.BelongFactory;
            payModel.SubmitDateTime = DateTime.Now;
           
            payModel.PayStatus = "0";
            payModel.Remark = "Remark";

            servComm.AddOrUpdate(payModel);



            //chkValue = DelegatePay(orderID);


            // SDKUtil.Sign(param, System.Text.Encoding.UTF8);
            myMap["Signature"] = chkValue;
            string html = SDKUtil.CreateAutoSubmitFormHash(SDKConfig.FrontTransUrl, myMap, System.Text.Encoding.UTF8);// 将SDKUtil产生的Html文档写入页面，从而引导用户浏览器重定向   
            Response.ContentEncoding = Encoding.UTF8; // 指定输出编码
            Response.Write(html);
            Response.End();
        }
        else
        {
            Response.Write("请配置支付参数");
            Response.End();
        }
    }

    private static string DelegatePay(string orderID)
    {
      //  NetPay netp = new NetPay();
        // bool keyima = netp.buildKey("808080301501009", 0, "D:\\certs\\MerPrK_808080301501009_20151231134610.key"); 808080301501009
        string url = "http://sfj.chinapay.com/dac/SinPayServletGBK";
        string oraMerId = "808080301501009";
        string transDate = DateTime.Now.ToString("yyyyMMdd");
        string orderNo = orderID;
        string cardNo = "6214854110015293";
        string usrName = "王立国";
        string bakName = "招商银行";
        string transAmt = "1";
        string prov = "辽宁";
        string city = "大连";
        string version = "20151207";
        string flag = "00";
        string signFlag = "1";
        string purpose = "测试代付";
        string subBank = "软件园支行";
        string termType = "07";
        string payMode = "1";





        string split = "|";
        //string plain = oraMerId + split + transDate + split + orderNo + split + cardNo + split + usrName + split + bakName + split + prov + split + city + split + transAmt + split + purpose + split + subBank + split + flag + split + version + split + termType;
        string plain = oraMerId + transDate + orderNo + cardNo + usrName + bakName + prov + city + transAmt + purpose + subBank + flag + version;
//        string plain = "merId=" + oraMerId + "&merDate=" + transDate + "&merSeqId=" + orderNo + "&cardNo=" + cardNo + "&usrName=" + usrName
//+ "&transAmt=" + transAmt + "&prov=" + prov + "&city=" + city + "&version=" + version + "&openBank=" + bakName + "&flag=" + flag
//+ "&signFlag=" + signFlag + "&purpose=" + purpose + "&subBank=" + subBank + "&termType=" + termType;
        String chkValue = Utils.signData(oraMerId, plain);
       // String chkValue = Utils.sign(oraMerId, orderNo, "1", usrName, transDate,"07");



        usrName = HttpUtility.UrlEncode(usrName, System.Text.Encoding.GetEncoding("GBK"));
        prov = HttpUtility.UrlEncode(prov, System.Text.Encoding.GetEncoding("GBK"));
        city = HttpUtility.UrlEncode(city, System.Text.Encoding.GetEncoding("GBK"));
        bakName = HttpUtility.UrlEncode(bakName, System.Text.Encoding.GetEncoding("GBK"));
        purpose = HttpUtility.UrlEncode(purpose, System.Text.Encoding.GetEncoding("GBK"));
        subBank = HttpUtility.UrlEncode(subBank, System.Text.Encoding.GetEncoding("GBK"));

//        string reqStr = "merId=" + oraMerId + "&merDate=" + transDate + "&merSeqId=" + orderNo + "&cardNo=" + cardNo + "&usrName=" + usrName + "&openBank=" + bakName + "&prov=" + prov + "&city=" + city
//+ "&transAmt=" + transAmt + "&purpose=" + purpose + "&version=" + version
//+ "&signFlag=" + signFlag + "&termType=" + termType + "&chkValue=" + chkValue;

        string reqStr = "merId=" + oraMerId + "&merDate=" + transDate + "&merSeqId=" + orderNo + "&cardNo=" + cardNo + "&usrName=" + usrName
           + "&transAmt=" + transAmt + "&prov=" + prov + "&city=" + city + "&version=" + version + "&openBank=" + bakName + "&flag=" + flag
           + "&signFlag=" + signFlag + "&purpose=" + purpose + "&subBank=" + subBank + "&chkValue=" + chkValue;
        //发送请求数据并获取返回结果
        string respStr = Utils.postData(reqStr, url);
        return chkValue;
    }

}