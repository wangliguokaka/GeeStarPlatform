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

public partial class ReportStatistics_OrderListQuery : PageBase
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
        if (!string.IsNullOrEmpty(hddpnumbers))
        {
            iCount = Convert.ToInt32(hddpnumbers);
        }
        int iPageIndex = string.IsNullOrEmpty(Request["sPageID"]) ? 1 : Convert.ToInt32(Request["sPageID"]);
        int iPageCount = string.IsNullOrEmpty(Request["sPageNum"]) ? 0 : Convert.ToInt32(Request["sPageNum"]);


        string txtOrder_ID = Request["txtOrder_ID"];
        if (!string.IsNullOrEmpty(txtOrder_ID))
        {
            ccWhere.AddComponent("Order_ID ", txtOrder_ID, SearchComponent.Like, SearchPad.And);
        }
        GetFilterByKind(ref ccWhere, "Report");
        servComm.strOrderString = " regtime desc ";
        IList<ORDERS> ilist = servComm.GetList<ORDERS>(ORDERS.STRTABLENAME, "*", ORDERS.STRKEYNAME, iCount, iPageIndex, iPageCount, ccWhere);

        this.repOrderList.DataSource = ilist;
        repOrderList.DataBind();
        pagecut1.iPageNum = servComm.PageCount;

    }

}