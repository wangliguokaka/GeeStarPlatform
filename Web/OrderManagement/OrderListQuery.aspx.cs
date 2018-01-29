using D2012.Common;
using D2012.Common.DbCommon;
using D2012.Domain.Entities;
using D2012.Domain.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OrderManagement_OrderListQuery : PageBase
{
    ServiceCommon servComm = new ServiceCommon();
    ConditionComponent ccWhere = new ConditionComponent();
    ConditionComponent ccMessageWhere = new ConditionComponent();
    protected int tcount;
    protected string hddpnumbers;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["action"] == "CompareTalkRecord")
        {
            string unReadOrderNo = "";
            if (Session["allOrder"] == null)
            {
                Response.Write("");
                Response.End();
            }
            else
            {               
                string[] orderNumber = Session["allOrder"].ToString().Trim(',').Split(',');
                for (int i = 0; i < orderNumber.Length; i++) {
                    if (Request.Cookies[LoginUser.BelongFactory + orderNumber[i]] != null && Request.Cookies[LoginUser.BelongFactory + orderNumber[i]].Value!="")
                    {
                        unReadOrderNo = GetLastedTimeByOrder(unReadOrderNo, orderNumber[i]);
                    }
                    else
                    {
                        unReadOrderNo = unReadOrderNo + "," + orderNumber[i];
                    }
                }
                Response.Write(unReadOrderNo.Trim(','));
                Response.End();
            }

        }

        if (Request["action"] == "ReadTalkRecord")
        {
            string orderNumber = Request["orderNumber"];
            string modifyDate = GetLastedTimeByOrder("", orderNumber, "1");
            Response.Write(modifyDate);
            Response.End();
        }

        if (!IsPostBack)
        {
        }
        if (!string.IsNullOrEmpty(Request["hDelete"]))
        {
            servComm.ExecuteSql(" delete from W_Orders where ModelNo='" + Request["hDelete"] + "' and BelongFactory='"+LoginUser.BelongFactory+"'");
        }

        ccWhere.Clear();
        hddpnumbers = Request["hpnumbers"];
        int iCount = 10;
        if (!string.IsNullOrEmpty(hddpnumbers))
        {
            iCount = Convert.ToInt32(hddpnumbers);
        }
        int iPageIndex = string.IsNullOrEmpty(Request["sPageID"]) ? 1 : Convert.ToInt32(Request["sPageID"]);
        int iPageCount = string.IsNullOrEmpty(Request["sPageNum"]) ? 0 : Convert.ToInt32(Request["sPageNum"]);


        string txtModelNumber = Request["txtModelNumber"];
        if (!string.IsNullOrEmpty(txtModelNumber))
        {
            ccWhere.AddComponent("ModelNo ", "%"+txtModelNumber+"%", SearchComponent.Like, SearchPad.And);
        }

        string selectedID =  Request["selectedID"];
        string selectedLevel = Request["selectedLevel"];
        if (!String.IsNullOrEmpty(selectedLevel) && !String.IsNullOrEmpty(selectedID))
        {
            if (selectedLevel == "0")
            {
                
                ccWhere.AddComponent("sellerid ", selectedID, SearchComponent.Equals, SearchPad.And);
                ccMessageWhere.AddComponent("sellerid ", selectedID, SearchComponent.Equals, SearchPad.And);
            }
            else if (selectedLevel == "1")
            {
                ccWhere.AddComponent("HospitalID ", selectedID, SearchComponent.Equals, SearchPad.And);
                ccMessageWhere.AddComponent("HospitalID ", selectedID, SearchComponent.Equals, SearchPad.And);
            }
            else if (selectedLevel == "2")
            {
                ccWhere.AddComponent("DoctorID ", selectedID, SearchComponent.Equals, SearchPad.And);
                ccMessageWhere.AddComponent("DoctorID ", selectedID, SearchComponent.Equals, SearchPad.And);
            }
        }

        //被接收的订单不展示
        //ccWhere.AddComponent("isnull(Auth,'0') ", "0", SearchComponent.Equals, SearchPad.And);

        GetFilterByKind(ref ccWhere);
        GetFilterByKind(ref ccMessageWhere);
        
        servComm.strOrderString = " regtime desc ";
        IList<WORDERS> ilist = servComm.GetList<WORDERS>(WORDERS.STRTABLENAME, "*", WORDERS.STRKEYNAME, iCount, iPageIndex, iPageCount, ccWhere);

        ccMessageWhere.AddComponent("ModelNo", "(select OrderNumber from OrderMessage )", SearchComponent.In, SearchPad.And);
        IList<WORDERS> alllist = servComm.GetListTop<WORDERS>(0, ccMessageWhere);
        string strModelNo = "";
        alllist.ToList().ForEach(item => strModelNo = strModelNo + "," + item.ModelNo);
        Session["allOrder"] = strModelNo;
        this.repOrderList.DataSource = ilist;
        repOrderList.DataBind();
        pagecut1.iPageNum = servComm.PageCount;
       
    }

    private string GetLastedTimeByOrder(string unReadOrderNo, string orderNumber,string readRecord="")
    {
        if (Request.Cookies[LoginUser.BelongFactory + orderNumber]!=null && Request.Cookies[LoginUser.BelongFactory + orderNumber].Value != "")
        {


            string cookieTime = Request.Cookies[LoginUser.BelongFactory + orderNumber].Value;
            if (CacheHelper.ExistIdentify(LoginUser.BelongFactory + "|" + orderNumber))
            {
                string content = CacheHelper.GetInfoByIdentify(LoginUser.BelongFactory + "|" + orderNumber).Last<string>();
                DateTime modifyTime = DateTime.Parse(content.Split('|')[0]);
                if (int.Parse(cookieTime) < int.Parse(ConvertDateTimeInt(modifyTime)))
                {
                    unReadOrderNo = unReadOrderNo + "," + orderNumber;
                    if (readRecord == "1")
                    {
                        return ConvertDateTimeInt(modifyTime);
                        //WriteCookie(LoginUser.BelongFactory + orderNumber, ConvertDateTimeInt(modifyTime));
                    }
                }
            }
            else
            {
                ccWhere.Clear();
                ccWhere.AddComponent("OrderNumber", orderNumber, SearchComponent.Equals, SearchPad.NULL);
                ccWhere.AddComponent("BelongFactory", LoginUser.BelongFactory, SearchComponent.Equals, SearchPad.And);
                servComm.strOrderString = " ModifyDate desc ";
                DataTable dtTalk = servComm.GetListTop(1, "OrderMessage", ccWhere);
                if (dtTalk.Rows.Count > 0)
                {
                    List<string> talkList = XmlSerializerHelper.DeserializeObject(dtTalk.Rows[0]["TalkContent"].ToString());
                    string content = talkList.Last<string>();
                    DateTime modifyTime = DateTime.Parse(content.Split('|')[0]);
                    if (int.Parse(cookieTime) < int.Parse(ConvertDateTimeInt(modifyTime)))
                    {
                        unReadOrderNo = unReadOrderNo + "," + orderNumber;
                        if (readRecord == "1")
                        {
                            return ConvertDateTimeInt(modifyTime);
                            // WriteCookie(LoginUser.BelongFactory + orderNumber, ConvertDateTimeInt(modifyTime));
                        }
                    }
                }

            }
            return unReadOrderNo;
        }
        else if (readRecord == "1")
        {
            if (CacheHelper.ExistIdentify(LoginUser.BelongFactory + "|" + orderNumber))
            {
                string content = CacheHelper.GetInfoByIdentify(LoginUser.BelongFactory + "|" + orderNumber).Last<string>();
                DateTime modifyTime = DateTime.Parse(content.Split('|')[0]);          
                   
                return ConvertDateTimeInt(modifyTime);
            }
            else
            {
                ccWhere.Clear();
                ccWhere.AddComponent("OrderNumber", orderNumber, SearchComponent.Equals, SearchPad.NULL);
                ccWhere.AddComponent("BelongFactory", LoginUser.BelongFactory, SearchComponent.Equals, SearchPad.And);
                servComm.strOrderString = " ModifyDate desc ";
                DataTable dtTalk = servComm.GetListTop(1, "OrderMessage", ccWhere);
                if (dtTalk.Rows.Count > 0)
                {
                    List<string> talkList = XmlSerializerHelper.DeserializeObject(dtTalk.Rows[0]["TalkContent"].ToString());
                    string content = talkList.Last<string>();
                    DateTime modifyTime = DateTime.Parse(content.Split('|')[0]);
                    return ConvertDateTimeInt(modifyTime);
                }
                else
                {
                    return "";
                }
            }
        }
        else
        {
            return "";
        }
    }
    public static string ConvertDateTimeInt(System.DateTime time)
    {
        double intResult = 0;
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        intResult = (time - startTime).TotalSeconds;
        return intResult.ToString();
    }
    protected string GetTypeClass(object typeClass)
    {

        if (typeClass == "A")
        {
            return "正常";
        }
        else if (typeClass == "B")
        {
            return "返修";
        }
        else if (typeClass == "C")
        {
            return "返工";
        }
        else if (typeClass == "D")
        {
            return "退货";
        }
        else
        {
            return "订单作废";
        }
    }

    protected string GetSeller(object sllerID)
    {
        if (sllerID == null)
        {
            return "";
        }
        ServiceCommon facComm = new ServiceCommon(factoryConnectionString);

        ccWhere.Clear();
        ccWhere.AddComponent("id", sllerID.ToString(), SearchComponent.Equals, SearchPad.NULL);
        DataTable dt = facComm.GetListTop(0, " Seller ", "seller", ccWhere);
        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0]["Seller"].ToString();
        }
        else
        {
            return "";
        }

    }

    protected string GetHospital(object hospitalID)
    {

        if (hospitalID == null)
        {
            return "";
        }
        ServiceCommon facComm = new ServiceCommon(factoryConnectionString);

        ccWhere.Clear();
        ccWhere.AddComponent("id", hospitalID.ToString(), SearchComponent.Equals, SearchPad.NULL);
        DataTable dt = facComm.GetListTop(0, " hospital ", "Hospital", ccWhere);
        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0]["hospital"].ToString();
        }
        else
        {
            return "";
        }


    }

    protected string GetDoctor(object doctorID)
    {

        if (doctorID == null)
        {
            return "";
        }
        ServiceCommon facComm = new ServiceCommon(factoryConnectionString);

        ccWhere.Clear();
        ccWhere.AddComponent("id", doctorID.ToString(), SearchComponent.Equals, SearchPad.NULL);
        DataTable dt = facComm.GetListTop(0, " doctor ", "doctor", ccWhere);
        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0]["doctor"].ToString();
        }
        else
        {
            return "";
        }
    }

}