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

public partial class Information_ProcedureQuery : PageBase
{
    ServiceCommon servCommfac;
    ConditionComponent ccWhere = new ConditionComponent();
    protected int tcount;
    protected string hddpnumbers;
    protected string queryaction;
    protected void Page_Load(object sender, EventArgs e)
    {
        servCommfac = new ServiceCommon(base.factoryConnectionString);
        if (!IsPostBack)
        {
        }

         queryaction = Request["queryaction"];
        //产品进程与患者查询使用不同的过滤条件
         if (queryaction == "procedure")
         {
             searchboxpatient.Visible = false;
         }
         else
         {
             searchbox.Visible = false;
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
 
        string txtDateFrom = Request["starttime"];
        string txtDateTo = Request["endtime"];
        ccWhere.AddComponent("OutFlag ","N", SearchComponent.Equals, SearchPad.NULL);
        if (!string.IsNullOrEmpty(txtDateFrom))
        {
            ccWhere.AddComponent("indate ", txtDateFrom, SearchComponent.GreaterOrEquals, SearchPad.And);
        }

        if (!string.IsNullOrEmpty(txtDateTo))
        {
            ccWhere.AddComponent("indate ", DateTime.Parse(txtDateTo).AddDays(1).ToString(), SearchComponent.Less, SearchPad.And);
        }

        string txtPatient = Request["txtPatient"];
        if (!string.IsNullOrEmpty(txtPatient))
        {
            ccWhere.AddComponent("patient ", txtPatient, SearchComponent.Like, SearchPad.And);
        }

        string txtOrderNo = Request["txtOrderNo"];
        if (!string.IsNullOrEmpty(txtOrderNo))
        {
            ccWhere.AddComponent("Order_ID ", txtOrderNo, SearchComponent.Like, SearchPad.And);
        }

        GetFilterByKind(ref ccWhere, "orders");

        servCommfac.strOrderString = " regtime desc ";
        IList<ORDERS> ilist = servCommfac.GetList<ORDERS>(ORDERS.STRTABLENAME, "Order_ID,seller,sellerid,hospital,ModelNo,hospitalid,doctor,serial,patient,orderclass,indate", ORDERS.STRKEYNAME, iCount, iPageIndex, iPageCount, ccWhere);

        this.repOrderList.DataSource = ilist;
        repOrderList.DataBind();
        pagecut1.iPageNum = servCommfac.PageCount;

    }

    protected string GetTypeClass(string typeClass)
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