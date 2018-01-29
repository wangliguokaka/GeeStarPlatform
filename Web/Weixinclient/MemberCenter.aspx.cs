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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Weixin
{
    public partial class MemberCenter : PageBase
    {
        ServiceCommon servComm = new ServiceCommon();
        ConditionComponent ccWhere = new ConditionComponent();
        ServiceCommon facComm;
        protected WUSERS userModel = new WUSERS();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                ((HtmlContainerControl)Master.FindControl("HTitle")).InnerText = IsCN ? "用户个人信息" : "User Center";
            }

            if (Session["NickName"] == null)
            {
                Response.Redirect(Request.Url.GetLeftPart(UriPartial.Authority) + "//Weixinclient//WXLogin.aspx");
            }
            if (Request["action"] == "CheckPassword")
            {
                string oldPass = Request["oldpassword"];
                if (oldPass == LoginUser.Passwd)
                {
                    Response.Write("1");
                }
                else
                {
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
            else if (yeyRequest.Params("haddinfo") == "2")
            {
                Session["UserName"] = null;
                if (LoginUser.UserName.ToUpper().Contains("KQW"))
                {
                    Response.Redirect(Request.Url.GetLeftPart(UriPartial.Authority) + "//Weixinclient//WXKQLogin.aspx");
                }
                else {
                   
                    Response.Redirect(Request.Url.GetLeftPart(UriPartial.Authority) + "//Weixinclient//WXLogin.aspx");
                }
                    
            }           
        }
    }
}