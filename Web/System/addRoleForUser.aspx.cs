using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using D2012.Domain.Services;
using D2012.Domain.Entities;
using D2012.Common.DbCommon;
using System.Text;
using D2012.Common;
using System.Data;
public partial class System_AddRoleForUser : PageBase
{
    ServiceCommon servComm = new ServiceCommon();
    ConditionComponent ccWhere = new ConditionComponent();
    protected string pid;
    protected string cid;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (yeyRequest.Params("type") == "GetJson")
        {
            GetRoleList();
        }

    }

    private void GetRoleList()
    {
        int iCount = 6;
        int iPageIndex = yeyRequest.Params("pageindex") == null ? 1 : Convert.ToInt32(yeyRequest.Params("pageindex"));
        int iPageCount = yeyRequest.Params("hPageNum") == null ? 0 : Convert.ToInt32(yeyRequest.Params("hPageNum"));
        ccWhere.Clear();
        //ccWhere.AddComponent("isdel", "0", SearchComponent.Equals, SearchPad.NULL);

        servComm.strOrderString = " ID desc ";
        IList<JX_USERS> scenerylist = servComm.GetList<JX_USERS>(JX_USERS.STRTABLENAME, "*", JX_USERS.STRKEYNAME, iCount, iPageIndex, iPageCount, ccWhere);

        if (iPageCount <= 1)
        {
            iPageCount = servComm.PageCount;
            iPageIndex = 1;
        }

        string backstr = Json.ListToJson("replyJson", scenerylist, string.Format("\"PageCount\":\"{0}\",", iPageCount)).Replace("\n", "<br/>").Replace("\r", "<br/>");

        if (backstr.IndexOf("\\") >= 0)
        {
            backstr = backstr.Replace("\\", "\\\\");
        }

        Response.Write(backstr);
        Response.End();
    }


}
