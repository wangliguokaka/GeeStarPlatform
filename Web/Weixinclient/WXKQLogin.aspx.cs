using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using D2012.Common.DbCommon;
using System.Text;
using D2012.Domain.Services;
using D2012.Common;
using D2012.Domain.Entities;
using System.Collections;
using System.Data;
using System.IO;
using System.Configuration;
using Weixin.Mp.Sdk.Domain;
using System.Text.RegularExpressions;
using LabManageModels;
using Tencent;
using System.Xml;
using System.Threading;
using WXPay.V3Demo;


public partial class Weixinclient_WXKQLogin : PageBase
{
    ServiceCommon servComm = new ServiceCommon();
    StringBuilder strWhere = new StringBuilder();
    ConditionComponent ccWhere = new ConditionComponent();
    protected string picture = "images/logo-img.png";
    protected string userName = "";
    protected string tel1 = "0411-39030189";
    protected string tel2 = "18641123506";
    protected string newopenid = "";
    protected string password = "";
    protected string corp = "用户登录";
    protected string haveResubmit = "";
    static Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        GeeStar.Workflow.Common.Log.LogError("start");
        GeeStar.Workflow.Common.Log.LogError(Request.RawUrl);


        Session["NickName"] = "";
        Session["HeadUrl"] = picture;
        Session["APPID"] = "wx85a350fdad4aea29";
        Session["APPSECRET"] = "1ae1d425eb36ed742984d7b0ea861754";

        userName = GetCookie(UserConstant.COOKIE_SAVEDUSERNAME);
        password = GetCookie(UserConstant.COOKIE_SAVEDPASSWORD);
        if (!String.IsNullOrEmpty(userName))
        {
            tel1 = GetCookie(UserConstant.COOKIE_TEL1);
            tel2 = GetCookie(UserConstant.COOKIE_TEL2);
        }

        if (password == "" && Session["UserName"] != null)
        {
            userName = Session["UserName"].ToString();
            password = Session["PassWord"].ToString();
        }

        corp = GetCookie(UserConstant.COOKIE_SAVEDCORP);
        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(userName))
            {
                //  GetAPPInfo(userName);
            }
        }
        else
        {
            if (Request["action"] == null)
            {
                haveResubmit = "";
            }
            else
            {
                haveResubmit = "true";
            }
            string action = Request["action"];
            if (action == "checkcode")
            {
                string checkcode = Request["checkcode"];
                if (checkcode.ToLower() == Session["CheckCode"].ToString().ToLower())
                {
                    Response.Write("1");
                }
                else
                {
                    Response.Write("0");
                }
                Response.End();
            }
            if (yeyRequest.Params("hlogin") == "1")
            {
                CheckLogin();
            }
            else if (yeyRequest.Params("hlogin") == "2")
            {
                TrykLogin();
            }
        }

    }

    private void GetAPPInfo(string loginUser)
    {
        ccWhere.Clear();
        ccWhere.AddComponent("JGCBM", "(select BelongFactory from W_USERS where UserName='" + loginUser + "')", SearchComponent.In, SearchPad.NULL);
        ccWhere.AddComponent("APPID", null, SearchComponent.ISNOT, SearchPad.And);
        ccWhere.AddComponent("APPSECRET", null, SearchComponent.ISNOT, SearchPad.And);
        DataTable dtFactory = servComm.GetListTop(1, "JX_USERS", ccWhere);
        if (dtFactory.Rows.Count > 0)
        {
            Session["APPID"] = dtFactory.Rows[0]["APPID"];
            Session["APPSECRET"] = dtFactory.Rows[0]["APPSECRET"];
        }

        //Session["NickName"] = "kaka";
        if (Session["APPID"] != null && Session["APPSECRET"] != null)
        {
            //try
            //{
            string accessToken = "";
            ReturnValue retValue = GetUserInfo(ref accessToken, Session["APPID"].ToString(), Session["APPSECRET"].ToString());
            if (retValue == null)
            {
                GeeStar.Workflow.Common.Log.LogError("空引用");
                Response.Redirect(Request.Url.GetLeftPart(UriPartial.Authority) + "//Weixinclient//WXLogin.aspx");
                Response.End();
            }
            GeeStar.Workflow.Common.Log.LogError(retValue.Message);
            if (StringUtils.GetJsonValue(retValue.Message, "nickname") != null)
            {
                string nickName = StringUtils.GetJsonValue(retValue.Message, "nickname").ToString();
                string headUrl = StringUtils.GetJsonValue(retValue.Message, "headimgurl").ToString();
                picture = headUrl;
                string strWeixin_OpenID = StringUtils.GetJsonValue(retValue.Message, "openid").ToString();
                WriteCookie(UserConstant.COOKIE_SAVEDOPENID, strWeixin_OpenID);
                Session["WeixinOpenID"] = strWeixin_OpenID;
                Session["NickName"] = nickName;
                Session["HeadUrl"] = headUrl;
            }
            else
            {
                Response.Redirect(Request.Url.GetLeftPart(UriPartial.Authority) + "//Weixinclient//WXLogin.aspx");
                Response.End();
            }

            //}
            //catch (Exception ex)
            //{
            //    GeeStar.Workflow.Common.Log.LogError(ex.Message, ex);
            //    Response.Redirect(Request.Url.GetLeftPart(UriPartial.Authority) + "//Weixinclient//WXLogin.aspx");
            //    Response.End();
            //}

        }
    }

    protected void TrykLogin()
    {
        CheckLogin("kqwsy", "123");
    }

    protected void CheckLogin(string UserName = "",string Password = "")
    {
        string txtUserName = UserName == ""?yeyRequest.Params("txtUser"): UserName;
        string txtPassword = Password == ""?yeyRequest.Params("txtPassword"): Password;

        if (!string.IsNullOrEmpty(txtUserName))
        {
            try
            {
                //根据用户名查询数据检测是否有匹配数据
                int result = doLogin(txtUserName, txtPassword);
                if (result == 0)
                {
                    Response.Redirect("WXKQLogin.aspx");
                }
                else if (result == 2)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "sucess", "art.dialog({title: false,time: 2,icon: 'error',content: '授权登录已经过期'});", true);

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "sucess", "art.dialog({title: false,time: 2,icon: 'error',content: '用户名或密码错误'});", true);

                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "sucess", "art.dialog({title: false,time: 2,icon: 'error',content: '" + ex.Message + "'});", true);

            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strUserName"></param>
    /// <param name="strPassword"></param>
    /// <param name="bolAutoSave"></param>
    /// <returns>0:成功 1：失败</returns>
    public int doLogin(string strUserName, string strPassword)
    {
        ServiceCommon servComm = new ServiceCommon();
        ConditionComponent condComponent = new ConditionComponent();
        string CheckPass = ConfigurationManager.AppSettings["CheckPass"];
        condComponent.Clear();
        //condComponent.AddComponent("UPPER(Alias)", strUserName.ToUpper(), SearchComponent.Equals, SearchPad.Ex);
        condComponent.AddComponent("UPPER(UserName)", strUserName.ToUpper(), SearchComponent.Equals, SearchPad.NULL);
        if (CheckPass == "1")
        {
            condComponent.AddComponent("Passwd", strPassword, SearchComponent.Equals, SearchPad.And);
        }
        WUSERS objUser = servComm.GetEntity<WUSERS>(null, condComponent);
        if (objUser.ID > 0)
        {
            string accessList = "";
            Session["objUser"] = objUser;

            string path = Server.MapPath(SaveFilePath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }



            HttpContext.Current.Session["USERID"] = objUser.ID.ToString();
            HttpContext.Current.Session["UserName"] = objUser.UserName.ToString();
            HttpContext.Current.Session["PassWord"] = objUser.Passwd.ToString();
            if (objUser.Kind == "S")
            {
                accessList = "Admin";
                Session["accessList"] = accessList;
                Response.Redirect("/Weixinclient/WXOrderList.aspx");
            }
            else
            {
                ccWhere.Clear();
                ccWhere.AddComponent("JGCBM", objUser.BelongFactory, SearchComponent.Equals, SearchPad.NULL);
                JX_USERS jxUser = servComm.GetEntity<JX_USERS>(null, ccWhere);
                if (jxUser != null && !String.IsNullOrEmpty(jxUser.DBUser))
                {
                    string factoryConnection = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", jxUser.DBServerIP, jxUser.JGCBM, jxUser.DBUser, jxUser.DBPassword);
                    Session["factoryConnectionString"] = factoryConnection;
                    ServiceCommon facservComm = new ServiceCommon(base.factoryConnectionString);


                    DataTable dtNeedScript = facservComm.ExecuteSqlDatatable("SELECT* FROM dbo.SysObjects WHERE ID = object_id(N'sp_PageGetCommNew') AND OBJECTPROPERTY(ID, N'IsProcedure') = 1");
                    if (dtNeedScript == null || dtNeedScript.Rows.Count == 0)
                    {
                        if (CreatDBScript(factoryConnection) == false)
                        {
                            return 3;
                        }
                    }

                    DataTable dtClassSet = facservComm.ExecuteSqlDatatable("SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'ClassSet') AND OBJECTPROPERTY(ID, 'IsTable') = 1");

                    if (dtClassSet != null && dtClassSet.Rows.Count > 0)
                    {
                        Session["ListClassSet"] = facservComm.GetListTop<ClassSet>(0, new ConditionComponent());
                        Session["IsGMP"] = true;
                    }
                    else
                    {
                        Session["IsGMP"] = false;
                    }

                    ccWhere.Clear();
                    Hashtable hashOrganization = new Hashtable();
                    if (LoginUser.Kind == "B")
                    {
                        hashOrganization.Add("sellerid", LoginUser.AssocNo);
                    }
                    else if (LoginUser.Kind == "C")
                    {
                        ccWhere.AddComponent("hospitalid", LoginUser.AssocNo.ToString(), SearchComponent.Equals, SearchPad.NULL);
                        DataTable dtHospital = facservComm.ExecuteSqlDatatable(vieworganizationsql + " where " + ccWhere.sbComponent);
                        if (dtHospital.Rows.Count > 0)
                        {
                            hashOrganization.Add("sellerid", dtHospital.Rows[0]["sellerid"]);
                            hashOrganization.Add("hospitalid", dtHospital.Rows[0]["hospitalid"]);
                        }
                        else
                        {
                            return 1;
                        }
                    }
                    else if (LoginUser.Kind == "D")
                    {
                        ccWhere.AddComponent("doctorid", LoginUser.AssocNo.ToString(), SearchComponent.Equals, SearchPad.NULL);
                        DataTable dtDoctor = facservComm.ExecuteSqlDatatable(distinctvieworganizationsql + " where " + ccWhere.sbComponent);
                        if (dtDoctor.Rows.Count > 0)
                        {
                            hashOrganization.Add("sellerid", dtDoctor.Rows[0]["sellerid"]);
                            hashOrganization.Add("hospitalid", dtDoctor.Rows[0]["hospitalid"]);
                            hashOrganization.Add("doctorid", dtDoctor.Rows[0]["doctorid"]);
                        }
                        else
                        {
                            return 1;
                        }
                    }

                    Session["Organization"] = hashOrganization;

                    DataTable dtBase = facservComm.GetListTop(1, "base", null);
                    if (dtBase.Rows.Count > 0)
                    {
                        Session["IDRule"] = dtBase.Rows[0]["IDRule"].ToString();
                        Session["phone"] = dtBase.Rows[0]["phone"].ToString().Trim();
                        Session["corp"] = dtBase.Rows[0]["corp"].ToString().Trim();
                        tel1 = dtBase.Rows[0]["phone"].ToString().Trim();
                        tel2 = dtBase.Rows[0]["fax"].ToString().Trim();
                    }
                    else
                    {
                        Session["IDRule"] = "B";
                        Session["phone"] = "";
                        Session["corp"] = "义齿平台用户登录";
                        tel1 = "";
                        tel2 = "";
                    }

                    WriteCookie(UserConstant.COOKIE_SAVEDUSERNAME, strUserName);
                    WriteCookie(UserConstant.COOKIE_SAVEDPASSWORD, strPassword);
                    WriteCookie(UserConstant.COOKIE_TEL1, tel1);
                    WriteCookie(UserConstant.COOKIE_TEL2, tel2);

                    Session["UserName"] = strUserName;
                    Session["PassWord"] = strPassword;
                    WriteCookie(UserConstant.COOKIE_SAVEDCORP, Session["corp"].ToString());
                    if (Session["APPID"] == null)
                    {
                        Response.Redirect(Request.Url.GetLeftPart(UriPartial.Authority) + "//Weixinclient//WXLogin.aspx?action=resubmit");
                        Response.End();
                    }

                    DataTable dt = servComm.ExecuteSqlDatatable("exec SPAccessMenu '" + LoginUser.BelongFactory + "'");
                    foreach (DataRow dr in dt.Rows)
                    {
                        accessList = accessList + "," + dr["action_type"];
                    }
                    Session["accessList"] = accessList;
                    if (accessList == "")
                    {
                        return 2;
                    }
                    else if (accessList.IndexOf("Weixin") > -1 && Session["FromWeixin"] == "1")
                    {
                        if (servComm.ExecuteSqlDatatable("select * from W_Weixin_Info where [UserName] like '" + objUser.BelongFactory + "%'").Rows.Count > 0)
                        {
                            DataTable dtMenu = servComm.ExecuteSqlDatatable("SELECT b.LinkUrl FROM [W_Weixin_Info] a inner join W_WeixinMenu b on a.menubm = b.MenuBM where a.[UserName] = '" + strUserName + "' and a.status = 'E' and b.LinkUrl != 'None' ");
                            if (dtMenu.Rows.Count > 0)
                            {
                                if (dtMenu.Select("LinkUrl='WXOrderList.aspx'").Length > 0)
                                {
                                    Response.Redirect("/Weixinclient/WXOrderList.aspx");
                                    Response.End();
                                }
                                else
                                {
                                    Response.Redirect("/Weixinclient/" + dtMenu.Rows[0]["LinkUrl"].ToString());
                                    Response.End();
                                }
                            }
                            else
                            {
                                return 2;
                            }
                        }
                        else
                        {
                            return 2;
                            //Response.Redirect("/Weixinclient/WXOrderList.aspx");
                            //Response.End();
                        }
                    }
                    else if (accessList.IndexOf("Order") > -1)
                    {
                        Response.Redirect("/OrderManagement/OrderList.aspx?type=Order");
                        Response.End();
                    }
                    else if (accessList.IndexOf("Procedure") > -1)
                    {
                        Response.Redirect("/Information/ProcedureQuery.aspx?type=Information");
                        Response.End();
                    }
                    else if (accessList.IndexOf("Report") > -1)
                    {
                        Response.Redirect("/ReportStatistics/FinanceSummaryDetail.aspx?type=ReportStatistics");
                        Response.End();
                    }

                }
                else
                {
                    return 1;
                }
            }
            return 0;
        }
        else
        {
            return 1;
        }
    }
}