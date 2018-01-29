using D2012.Common.DbCommon;
using D2012.Domain.Entities;
using D2012.Domain.Services;
using GeeStar.Workflow.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Information_PatientQuery : PageBase
{
    ServiceCommon servComm;
    ConditionComponent ccWhere = new ConditionComponent();
    protected int tcount;
    protected string hddpnumbers;
    protected void Page_Load(object sender, EventArgs e)
    {
        servComm = new ServiceCommon(base.factoryConnectionString);
        if (!IsPostBack)
        {
        }

        ccWhere.Clear();
        hddpnumbers = Request["hpnumbers"];
        int iCount = 10;
        //if (!string.IsNullOrEmpty(hddpnumbers))
        //{
        //    iCount = Convert.ToInt32(hddpnumbers);
        //}
        int iPageIndex = string.IsNullOrEmpty(Request["sPageID"]) ? 1 : Convert.ToInt32(Request["sPageID"]);
        int iPageCount = string.IsNullOrEmpty(Request["sPageNum"]) ? 0 : Convert.ToInt32(Request["sPageNum"]);

        string txttxtPatient = Request["txtPatient"];
        if (!string.IsNullOrEmpty(txttxtPatient))
        {
            ccWhere.AddComponent("patient ", txttxtPatient, SearchComponent.Like, SearchPad.And);
        }

        string selectedID = Request["selectedID"];
        string selectedLevel = Request["selectedLevel"];
        if (!String.IsNullOrEmpty(selectedLevel) && !String.IsNullOrEmpty(selectedID))
        {
            if (selectedLevel == "0")
            {
                ccWhere.AddComponent("sellerid ", selectedID, SearchComponent.Equals, SearchPad.And);
            }
            else if (selectedLevel == "1")
            {
                ccWhere.AddComponent("HospitalID ", selectedID, SearchComponent.Equals, SearchPad.And);
            }
            else if (selectedLevel == "2")
            {
                ccWhere.AddComponent("DoctorID ", selectedID, SearchComponent.Equals, SearchPad.And);
            }
        }

        GetFilterByKind(ref ccWhere, "orders");

        servComm.strOrderString = " regtime desc ";
        IList<ORDERSDETAIL> ilist = servComm.GetList<ORDERSDETAIL>(ORDERSDETAIL.STRTABLENAME, "regtime,Order_ID,seller,sellerid,hospital,hospitalid,doctor,patient,productName,Valid", ORDERSDETAIL.STRKEYNAME, iCount, iPageIndex, iPageCount, ccWhere);

        this.repOrderList.DataSource = ilist;
        repOrderList.DataBind();
        pagecut1.iPageNum = servComm.PageCount;

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