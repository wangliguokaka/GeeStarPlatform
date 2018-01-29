using D2012.Common.DbCommon;
using D2012.Domain.Entities;
using D2012.Domain.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FrontRcvResponse : PageBase
{
    protected int tcount;
    protected string hddpnumbers;
    ServiceCommon servComm = new ServiceCommon();
    ConditionComponent ccWhere = new ConditionComponent();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.HttpMethod == "POST")
        {
            // 使用Dictionary保存参数
            Hashtable myMap = new Hashtable();

            NameValueCollection coll = Request.Form;

            string[] requestItem = coll.AllKeys;

            for (int i = 0; i < requestItem.Length; i++)
            {
                myMap.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            if (myMap.ContainsKey("MerId"))
            {
                chinapaysecure.SecssUtil obj = new chinapaysecure.SecssUtil();
                ccWhere.Clear();
                ccWhere.AddComponent("PayNoCardMerId",myMap["MerId"].ToString(), SearchComponent.Equals, SearchPad.NULL);
                int count = servComm.GetCount("JX_USERS",ccWhere);
                if (count>0)
                {
                    obj.init(Request.PhysicalApplicationPath + "ChinaPay/" + myMap["MerId"].ToString() + "/security.properties"); //初始化安全控件：
                }
                else
                {
                    //B2C支付
                    //myMap.Add("BankInstNo", "700000000000010");
                    //myMap.Add("MerId", "481601512177911");
                    obj.init(Request.PhysicalApplicationPath + "ChinaPay/" + myMap["MerId"].ToString() + "/securityb2c.properties"); //初始化安全控件：
                }
                obj.verify(myMap);
                // 返回报文中不包含UPOG,表示Server端正确接收交易请求,则需要验证Server端返回报文的签名
                if ("00" == obj.getErrCode())
                {

                    servComm.ExecuteSql("update W_NetPay set PayDateTime = getdate() ,PayStatus = 2 where OrderID = '" + myMap["MerOrderNo"].ToString() + "'");


                    //Response.Write("商户端验证返回报文签名成功\n");

                    //商户端根据返回报文内容处理自己的业务逻辑 ,DEMO此处只输出报文结果
                    //StringBuilder builder = new StringBuilder();

                    //builder.Append("<tr><td align=\"center\" colspan=\"2\"><b>商户端接收银联返回报文并按照表格形式输出结果</b></td></tr>");

                    //for (int i = 0; i < requestItem.Length; i++)
                    //{
                    //    builder.Append("<tr><td width=\"30%\" align=\"right\">" + requestItem[i] + "</td><td style='word-break:break-all'>" + Request.Form[requestItem[i]] + "</td></tr>");
                    //}

                    //builder.Append("<tr><td width=\"30%\" align=\"right\">商户端验证银联返回报文结果</td><td>验证签名成功.</td></tr>");
                    //Response.Write(builder.ToString());

                }
                else
                {
                    servComm.ExecuteSql("update W_NetPay set PayDateTime = getdate() ,PayStatus = 9 where OrderID = '" + myMap["MerOrderNo"].ToString() + "'");

                    Response.Write("<tr><td width=\"30%\" align=\"right\">商户端验证银联返回报文结果</td><td>验证签名失败.</td></tr>");
                }
            }

            
        }

        ccWhere.Clear();
        ccWhere.AddComponent("UserID", CurrentUserID.ToString(), SearchComponent.Equals, SearchPad.NULL);
        if (!String.IsNullOrEmpty(Request["txtOrderNumner"]))
        {
            ccWhere.AddComponent("OrderID", Request["txtOrderNumner"], SearchComponent.Like, SearchPad.And);
        }

        hddpnumbers = Request["hpnumbers"];
        int iCount = 10;
        if (!string.IsNullOrEmpty(hddpnumbers))
        {
            iCount = Convert.ToInt32(hddpnumbers);
        }
        int iPageIndex = string.IsNullOrEmpty(Request["sPageID"]) ? 1 : Convert.ToInt32(Request["sPageID"]);
        int iPageCount = string.IsNullOrEmpty(Request["sPageNum"]) ? 0 : Convert.ToInt32(Request["sPageNum"]);

        servComm.strOrderString = " ID desc ";
        IList<WNetPay> ilist = servComm.GetList<WNetPay>(WNetPay.STRTABLENAME, "*", WNetPay.STRKEYNAME, iCount, iPageIndex, iPageCount, ccWhere);

        repUserList.DataSource = ilist;
        repUserList.DataBind();
        pagecut1.iPageNum = servComm.PageCount;
    }

    protected string Transfer(string transferType, string typeValue)
    {
        if (transferType == "PayMethod")
        {
            if (typeValue == "0")
            {
                return "快捷支付";
            }
            else {
                return "B2C支付";
            }
        }
        else if (transferType == "PayStatus")
        {
            if (typeValue == "0")
            {
                return "发起支付";
            }
            else if (typeValue == "2")
            {
                return "支付成功";
            }
            else {
                return "支付异常";
            }

        }
        else {
            return "";
        }
    }
}
