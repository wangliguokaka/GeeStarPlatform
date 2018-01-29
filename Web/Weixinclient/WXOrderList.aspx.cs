using D2012.Common;
using D2012.Common.DbCommon;
using D2012.Domain.Entities;
using D2012.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;
using System.Web.UI.HtmlControls;
using WXPay.V3Demo;
using DDTourCommon.WXPay;
using System.Text;
using Frank.Expressage;

public partial class Weixinclient_WXOrderList : PageBase
{

    ServiceCommon servCommfac;
    protected IList<ORDERS> ilist;
    protected int tcount;
    protected string hddpnumbers;
    protected WORDERS ordreModel = new WORDERS();
    protected string strOtherList = "";
    protected string strPhotoList = "";
    protected string timeStamp = "";
    protected string signalticket = "";
    protected int iPageIndex = 1;
    protected int iPageAllCount = 1;
    ConditionComponent ccWhere = new ConditionComponent();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["type"] == "GetTrace")
        {
            MQueryParameter para = new MQueryParameter();
            para.TypeCom = "申通快递";
            para.OrderId = "3323401314893";
            MResultMsg msg = ExpressageHelper.GetExpressageMessage(para);
            Response.Write(msg.JsonMessage);
            Response.End();
        }
      

        if (!IsPostBack)
        {
            ((HtmlContainerControl)Master.FindControl("HTitle")).InnerText = IsCN ? "订单查询" : "Order Query";
        }

        string acetoken = GetAccessToken(Session["APPID"].ToString(), Session["APPSECRET"].ToString());
        timeStamp = TenpayUtil.getTimestamp();
        string signal = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + acetoken + "&type=jsapi";
        ReturnValue retValue = StreamReaderUtils.StreamReader(signal, Encoding.UTF8);

        string ticket = StringUtils.GetJsonValue(retValue.Message, "ticket").ToString();

       string url = "jsapi_ticket=" + ticket + "&noncestr=Wm3WZYTPz0wzccnW&timestamp=" + timeStamp + "&url=" + Request.Url.AbsoluteUri.ToString();
        signalticket = SHA1_Hash(url);

        try
        {

            servCommfac = new ServiceCommon(base.factoryConnectionString);
            if (!IsPostBack)
            {
                if (Request["returnValue"] == null)
                {
                    Session["returnValue"] = null;
                }

            }

         

            ccWhere.Clear();
            ccWhere = querycontrol.GetCondtion();
            //默认显示的是在厂的订单，不要显示已经出厂的订单
           
            servCommfac.strOrderString = querycontrol.OrderString == "" ? " regtime desc " : querycontrol.OrderString;

            hddpnumbers = yeyRequest.Params("hpnumbers");
            int iCount = 5;
            if (!string.IsNullOrEmpty(hddpnumbers))
            {
                iCount = Convert.ToInt32(hddpnumbers);
            }            

            iPageIndex = string.IsNullOrEmpty(Request["sPageID"]) ? 1 : Convert.ToInt32(Request["sPageID"]);
            int iPageCount = string.IsNullOrEmpty(Request["sPageNum"]) ? 0 : Convert.ToInt32(Request["sPageNum"]);
            if (Request["submitflg"] != null && Request["submitflg"] == "1")
            {
                iPageIndex = 1;
                this.pagecutID.iPageIndex = 1;
                iPageCount = 0;
            }
           
            string sortList = Request["sortList"];
           

            if (Utils.IsNoSP == false)
            {
                ilist = servCommfac.GetList<ORDERS>(ORDERS.STRTABLENAME, "Order_ID,seller,sellerid,hospital,ModelNo,hospitalid,doctor,serial,patient,orderclass,indate,process," +
    "case when Hurry = 'Y' then 0 else '1' end as Hurry,case when TryPut = 'Y' then 0 else '1' end as TryPut,case when Slow = 'Y' then 0 else '1' end as Slow,preoutDate,case when preoutDate > GETDATE()then 1 else 0 end as expire ", ORDERS.STRKEYNAME, iCount, iPageIndex, iPageCount, ccWhere);
                this.pagecutID.iPageNum = servCommfac.PageCount;
            }
            else
            {
                int rowCount = servCommfac.GetCount("ORDERS", ccWhere);
                string filedshow = " row_number()over(order by Order_ID,serial) as rowIndex ,Order_ID, seller, sellerid, hospital, ModelNo, hospitalid, doctor, serial, patient, orderclass, indate, process,case when Hurry = 'Y' then 0 else '1' end as Hurry,case when TryPut = 'Y' then 0 else '1' end as TryPut,case when Slow = 'Y' then 0 else '1' end as Slow,preoutDate,case when preoutDate > GETDATE()then 1 else 0 end as expire";
                DataTable dt = servCommfac.ExecuteSqlDatatable("select * from (select " + filedshow + " from ORDERS  where 1=1 and " + ccWhere.sbComponent + ") t where rowIndex>=" + ((iPageIndex - 1) * iCount + 1).ToString() + " and rowIndex <=" + iPageIndex * iCount);
                ilist = Utils.ConvertTo<ORDERS>(dt);
                servCommfac.RowCount = rowCount;
                this.pagecutID.iPageNum = (rowCount - 1) / iCount + 1;
            }

            if (ilist.Count == 0)
            {
                servCommfac.PageCount = 0;
            }
           
            iPageAllCount = this.pagecutID.iPageNum;
        }
        catch (Exception ex){ 
        
        }
    }
}
 