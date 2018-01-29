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

public partial class OrderManagement_VerifyOrder : PageBase
{
    ServiceCommon servFacCommfac;
    ServiceCommon servCommfac = new ServiceCommon();
    ConditionComponent ccWhere = new ConditionComponent();
    protected WUSERS userModel = new WUSERS();
    protected string strOtherList = "";
    protected string checkPass = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        servFacCommfac = new ServiceCommon(base.factoryConnectionString);

        if (Request["action"] == "CheckOrder")
        {
            string txtOrder = Request["txtOrder"];
            ccWhere.AddComponent("Order_ID", txtOrder, SearchComponent.Equals, SearchPad.And);
            DataTable dtUser = servFacCommfac.GetListTop(1, "orders", ccWhere);
            if (dtUser.Rows.Count>0)
            {
                Response.Write("0");
            }
            else{
                Response.Write("1");
            }
            Response.End();
        }

        if (Request["action"] == "VerifierOrder") 
        {
            string txtOrder = Request["OrderNumber"];
            //base.IDRule
            servCommfac.ExecuteSql("update W_USERS set Passwd = '123' where UserName = '" + UserName + "'");
            Response.Write("1");
            Response.End();
        }

        if (yeyRequest.Params("haddinfo") == "1")
        {
            try
            {
                string ModelNo = Request["VerifierNo"];
                ModelNo = ModelNo.Replace("Verifier", "");
                string txtOrder = Request["txtOrder"];
                servCommfac.ExecuteSql("exec SPVerifyOrders '" + ModelNo + "','" + txtOrder+"','"+LoginUser.BelongFactory+"','"+LoginUser.UserName+"'");
                checkPass = "1";

            }
            catch (Exception)
            {
                checkPass = "0";
            }
        }
    }
}
