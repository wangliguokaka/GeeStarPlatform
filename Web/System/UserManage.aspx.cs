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

public partial class System_UserManage : PageBase
{
    ServiceCommon servComm = new ServiceCommon();
    ConditionComponent ccWhere = new ConditionComponent();
    ServiceCommon facComm;
    protected WUSERS userModel = new WUSERS();
    protected string strOtherList = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if(Request["action"]=="CheckPassword")
        {
            string oldPass = Request["oldpassword"];
            if(oldPass == LoginUser.Passwd)
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
          
            userModel.ID = LoginUser.ID;

            userModel.Passwd = Request["txtNewPassword"];
            userModel.AssocNo = LoginUser.AssocNo;

            LoginUser.Passwd = userModel.Passwd;
            Session["LoginUser"] = LoginUser;
           
            servComm.AddOrUpdate(userModel);
        }
    }
}
