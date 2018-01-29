using D2012.Common;
using D2012.Common.DbCommon;
using D2012.Domain.Entities;
using D2012.Domain.Services;
using DDTourCommon.WXPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WXPay.V3Demo;

public partial class Weixinclient_TicketList : PageBase
{
    ServiceCommon servCommfac;
    ConditionComponent ccWhere = new ConditionComponent();
    protected IList<ORDERS> ilist;
    protected int tcount;
    protected string hddpnumbers;
    protected string timeStamp = "";
    protected string signalticket = "";
    protected void Page_Load(object sender, EventArgs e)
    { 
        try
        {

            string acetoken = GetAccessToken(Session["APPID"].ToString(), Session["APPSECRET"].ToString());
            timeStamp = TenpayUtil.getTimestamp();
            string signal = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + acetoken + "&type=jsapi";
            ReturnValue retValue = StreamReaderUtils.StreamReader(signal, Encoding.UTF8);

            string ticket = StringUtils.GetJsonValue(retValue.Message, "ticket").ToString();

            string url = "jsapi_ticket=" + ticket + "&noncestr=Wm3WZYTPz0wzccnW&timestamp=" + timeStamp + "&url=" + Request.Url.AbsoluteUri.ToString();
            signalticket = SHA1_Hash(url);       
        }
        catch (Exception ex) { 
        
        }
    }
}