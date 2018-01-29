using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using D2012.Domain.Services;
using D2012.Common.DbCommon;
using System.Data;
using D2012.Domain.Entities;

public partial class ascx_LeftMenu : System.Web.UI.UserControl
{
    ServiceCommon servComm = new ServiceCommon();
    ConditionComponent ccWhere = new ConditionComponent();
    protected string accessOrder = "style=\"display:none;\"";
    protected string accessProcedure = "style=\"display:none;\"";
    protected string accessReport = "style=\"display:none;\"";
    protected string accessSystem = "style=\"display:none;\"";
    protected string accessPay = "style=\"display:none;\"";
    protected string accessWeixin = "style=\"display:none;\"";
    protected string lntitleCss = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Language"] == "zh-cn")
        {
            lntitleCss = "class=\"lntitle\"";
        }
        else
        {
            lntitleCss = "class=\"lntitleEnglist\"";
        }
        if (Session["accessList"] != null)
        {
            string accessList = Session["accessList"].ToString();
            if (accessList.IndexOf("Admin") > -1)
            {
                accessSystem = "";
            }
            if (accessList.IndexOf("Order") > -1)
            {
                accessOrder = "";
            }
            if (accessList.IndexOf("Procedure") > -1)
            {
                accessProcedure = "";
            }
            if (accessList.IndexOf("Report") > -1)
            {
                accessReport = "";
            }
            if (accessList.IndexOf("BankPay") > -1)
            {
                accessPay = "";
            }
            if (accessList.IndexOf("Weixin") > -1)
            {
                accessWeixin = "";
            }
        }
       
    }

}