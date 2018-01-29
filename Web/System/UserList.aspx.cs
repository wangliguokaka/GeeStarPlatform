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

public partial class System_UserList : PageBase
{
    ServiceCommon servComm = new ServiceCommon();
    ConditionComponent ccWhere = new ConditionComponent();
    protected int tcount;
    protected string hddpnumbers;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["hDelete"]))
        {
            servComm.ExecuteSql(" delete from W_USERS where id='" + Request["hDelete"] + "'");
        }
        
        ccWhere.Clear();
        if (!String.IsNullOrEmpty(Request["txtUserName"]))
        {
            ccWhere.AddComponent("UserName", Request["txtUserName"], SearchComponent.Like, SearchPad.NULL);
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
        IList<WUSERS> ilist = servComm.GetList<WUSERS>(WUSERS.STRTABLENAME, "*", WUSERS.STRKEYNAME, iCount, iPageIndex, iPageCount, ccWhere);

        repUserList.DataSource = ilist;
        repUserList.DataBind();
        pagecut1.iPageNum = servComm.PageCount;
       
    }
}