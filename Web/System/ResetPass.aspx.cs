using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;

using D2012.Common.DbCommon;
using D2012.Domain.Services;
using D2012.Common;
using D2012.Domain.Entities;
using System.Drawing;

public partial class System_ResetPass : PageBase
{
    ServiceCommon servComm = new ServiceCommon();
    ConditionComponent ccWhere = new ConditionComponent();
    protected WUSERS userModel = new WUSERS();
    protected string strOtherList = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if(Request["action"]=="CheckUserName")
        {
            string txtUserName = Request["txtUserName"];
            ccWhere.AddComponent("UserName",txtUserName, SearchComponent.Equals, SearchPad.And);
            DataTable dtUser = servComm.GetListTop(1, "W_USERS", ccWhere);
            if (dtUser.Rows.Count>0)
            {
                Response.Write("1");
            }
            else{
                Response.Write("0");
            }
            Response.End();
        }

        if (yeyRequest.Params("haddinfo") == "1")
        {
            string UserName = Request["txtUserName"];
            servComm.ExecuteSql("update W_USERS set Passwd = '123' where UserName = '" + UserName+"'");
        }
    }
}
