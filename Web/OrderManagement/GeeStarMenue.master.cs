using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using D2012.Common.DbCommon;
using D2012.Domain.Services;
using D2012.Common;
using D2012.Domain.Entities;
using System.Data;

public partial class GeeStar_GeeStar : System.Web.UI.MasterPage
{
    protected string ulevel;
    protected string PlatformName = "";
    protected string CopyRight = "";
    protected string PlatformTitile = "";
    ServiceCommon servComm = new ServiceCommon();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Config"] != null)
        {
            DataTable dtConfig = (DataTable)Session["Config"];
            if (dtConfig.Rows.Count > 0)
            {
                PlatformTitile = dtConfig.Rows[0]["PlatformTitile"].ToString();
                PlatformName = dtConfig.Rows[0]["PlatformName"].ToString();
                CopyRight = dtConfig.Rows[0]["CopyRight"].ToString();
            }
        }
        linkClearHistory.Text = GetGlobalResourceObject("Resource", "DeleteHistory").ToString();
        linkInitDB.Text = GetGlobalResourceObject("Resource", "InitData").ToString();
        if (Session["Language"] == null || Session["Language"].ToString() == "zh-cn")
        {
            LbEnglish.PostBackUrl = Request.Url.ToString().Replace("Language=zhcn", "").Replace("Language=en", "") + "&Language=en";
            this.LbChinese.Visible = false;
            this.LbEnglish.Visible = true;
        }
        else
        {
            LbChinese.PostBackUrl = Request.Url.ToString().Replace("Language=zhcn", "").Replace("Language=en", "") + "&Language=zhcn";
            this.LbEnglish.Visible = false;
            this.LbChinese.Visible = true;
        }
        
        if (Session["objUser"]!=null && ((WUSERS)Session["objUser"]).Kind == "S")
        {
            resetPassword.Visible = true;
            ClearHistory.Visible = true;
            InitDB.Visible = true;
        }
        if (Request.Path.IndexOf("RoleEdit.aspx") > -1 || Request.Path.IndexOf("ReportStatistics") > -1)
        {
            this.form1.EnableViewState = true;
        }
        else {
            this.form1.EnableViewState = false;
        }
        
        if (Session["SYSADMINUSERID"] == null)
        {
           // HttpContext.Current.Response.Redirect("/login.aspx");
        }

        if (yeyRequest.Params("hlogout") == "1")
        {
            doLogout();
           // Response.Redirect("/login.aspx");
        }
        
    }

    public static void doLogout()
    {
        HttpContext.Current.Session["SYSADMINUSERID"] = null;
        HttpContext.Current.Session["SYSADMINUSERNAME"] = null;
        HttpCookie aCookie;
        int iCount = HttpContext.Current.Request.Cookies.Count;
        //清除Cookies
        for (int i = 0; i < iCount; i++)
        {
            aCookie = HttpContext.Current.Request.Cookies[i];
            aCookie.Value = "";
            aCookie.Expires = DateTime.Now.AddYears(-10);
            HttpContext.Current.Response.Cookies.Add(aCookie);
        }
        HttpContext.Current.Response.Redirect("/login.aspx");
    }
    protected void linkClearHistory_Click(object sender, EventArgs e)
    {
        servComm.ExecuteSqlDatatable("exec SPAccessMenu '0'");
    }
    protected void linkInitDB_Click(object sender, EventArgs e)
    {
        servComm.ExecuteSqlDatatable("exec SPAccessMenu '1'");
    }
}
