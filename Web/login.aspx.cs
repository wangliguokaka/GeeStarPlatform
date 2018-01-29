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

using Weixin.Mp.Sdk.Domain;
using System.Text.RegularExpressions;
using LabManageModels;
using Tencent;
using System.Xml;
using System.Threading;
using System.Configuration;

public partial class cic_login : PageBase
{
    ServiceCommon servComm = new ServiceCommon();
    StringBuilder strWhere = new StringBuilder();
    ConditionComponent ccWhere = new ConditionComponent();
    protected int id;

    LabManageBLL.selectOrder select = new LabManageBLL.selectOrder();
    string token = ConfigurationManager.AppSettings["token"];//你的Token
    string corpId = ConfigurationManager.AppSettings["corpId"];//企业号corpId
    string encodingAESKey = ConfigurationManager.AppSettings["encodingAESKey"];//token中配置url
    string corpsecret = ConfigurationManager.AppSettings["corpsecret"];//新建管理组id
    protected string loginPlatForm = "";
    protected string CopyRight = "";
    protected string PlatformTitile = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        string type = Request["type"];

        DataTable dtConfig = servComm.ExecuteSqlDatatable(" select * from W_Config ");
        if (dtConfig != null)
        {
            Session["Config"] = dtConfig;
            if (dtConfig.Rows.Count > 0)
            {
                PlatformTitile = dtConfig.Rows[0]["PlatformTitile"].ToString();
                loginPlatForm = dtConfig.Rows[0]["PlatformLogin"].ToString();
                CopyRight = dtConfig.Rows[0]["CopyRight"].ToString();
            }
        }

        if (type!=null && type.ToLower() == "weixin")
        {
            wx.Visible = true;
            login.Visible = false;
            Timer1.Enabled = true;

            string sVerifyMsgSig = HttpContext.Current.Request.QueryString["msg_signature"];//企业号的 msg_signature
            string sVerifyTimeStamp = HttpContext.Current.Request.QueryString["timestamp"];
            string sVerifyNonce = HttpContext.Current.Request.QueryString["nonce"];
            string sVerifyEchoStr = HttpContext.Current.Request.QueryString["echoStr"];
            WXBizMsgCrypts wxcpt = new WXBizMsgCrypts(token, encodingAESKey, corpId);
            int ret = 0;
            string postString = string.Empty;
            #region
            if (HttpContext.Current.Request.HttpMethod.ToUpper() == "POST")
            {
                string msg = "";
                using (Stream stream = HttpContext.Current.Request.InputStream)
                {
                    Byte[] postBytes = new Byte[stream.Length];
                    stream.Read(postBytes, 0, (Int32)stream.Length);
                    postString = Encoding.UTF8.GetString(postBytes);
                }
                if (!string.IsNullOrEmpty(postString))
                {
                    ret = wxcpt.DecryptMsg(sVerifyMsgSig, sVerifyTimeStamp, sVerifyNonce, postString, ref msg);
                    if (ret != 0)
                    {
                        HttpContext.Current.Response.Write("ERR: VerifyURL fail, ret: " + ret);
                        return;
                    }

                    WeiXinRequest requestXML = new WeiXinRequest();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(msg);
                    XmlNode root = doc.FirstChild;
                    requestXML.FromUserName = root["FromUserName"].InnerText;
                    requestXML.MsgType = root["MsgType"].InnerText;
                    if (requestXML.MsgType == "text")
                    {
                        requestXML.Content = root["Content"].InnerText;
                    }
                    if (requestXML.MsgType == "event")
                    {
                        requestXML.Wxevent = root["Event"].InnerText;
                        requestXML.EventKey = root["EventKey"].InnerText;
                    }
                    #region
                    string database = "";
                    string sql = "";
                    if (requestXML.MsgType == "text")
                    {
                        string orderid = requestXML.Content.Trim();//订单号
                        string UserName = requestXML.FromUserName.Trim();//微信号
                        Order order = new Order();
                        DataSet ds = new DataSet();
                        DataSet dss = select.DatabaseName(UserName);//当前数据库
                        if (dss != null && dss.Tables[0].Rows.Count > 0)
                        {
                            for (int a = 0; a < dss.Tables[0].Rows.Count; a++)
                            {
                                database = dss.Tables[0].Rows[a]["DataBaseName"].ToString();
                                if (database != "")
                                {
                                    database = database + ".";
                                    #region
                                    ds = select.getOrder(orderid, UserName, database);//质保卡
                                    if (ds != null)
                                    {
                                        order = select.fillorder(ds.Tables[0].Rows);
                                        sql += "产品的质保卡信息\n";
                                        sql += "订单编号：" + order.orderId + "\n";
                                        sql += "医院名称：" + order.hospital + "\n";
                                        sql += "患者姓名：" + order.patient + "\n";
                                        sql += "医生姓名：" + order.doctor + "\n";
                                        string indte = "";
                                        if (order.indate != null)
                                        {
                                            indte = Convert.ToDateTime(order.indate).ToString("yyyy-MM-dd");

                                        }
                                        sql += "生产日期：" + indte + "\n";
                                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                        {
                                            sql += "产品名称:" + ds.Tables[0].Rows[i]["itemname"].ToString() + "\n";
                                            sql += "保修日期:" + ds.Tables[0].Rows[i]["Valid"].ToString() + "\n";
                                            sql += "牙位如下" + "\n";
                                            sql += "上右位:" + ds.Tables[0].Rows[i]["a_teeth"].ToString() + "\n";
                                            sql += "上左位:" + ds.Tables[0].Rows[i]["b_teeth"].ToString() + "\n";
                                            sql += "下右位:" + ds.Tables[0].Rows[i]["c_teeth"].ToString() + "\n";
                                            sql += "下左位:" + ds.Tables[0].Rows[i]["d_teeth"].ToString() + "\n\n";

                                        }
                                    }
                                    #endregion
                                    #region
                                    ds = select.getOrdered(orderid, UserName, database);//材料成分
                                    if (ds != null)
                                    {
                                        order = select.fillorder(ds.Tables[0].Rows);
                                        sql += "产品的材料成分等可追溯信息\n";
                                        sql += "订单编号：" + order.orderId + "\n";
                                        sql += "医院名称：" + order.hospital + "\n";
                                        sql += "患者姓名：" + order.patient + "\n";
                                        sql += "医生姓名：" + order.doctor + "\n";
                                        string indte = "";
                                        if (order.indate != null)
                                        {
                                            indte = Convert.ToDateTime(order.indate).ToString("yyyy-MM-dd");

                                        }
                                        sql += "生产日期：" + indte + "\n";
                                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                        {
                                            sql += "产品名称:" + ds.Tables[0].Rows[i]["itemname"].ToString() + "\n";
                                            sql += "材料名称:" + ds.Tables[0].Rows[i]["Name"].ToString() + "\n";
                                            sql += "生产厂家及材料注册证号:" + ds.Tables[0].Rows[i]["Maker"].ToString() + "\n";
                                            sql += "材料批号:" + ds.Tables[0].Rows[i]["batchNo"].ToString() + "\n\n";

                                        }

                                    }
                                    #endregion

                                }
                            }
                        }
                        else
                        {
                            sql += "欢迎关注\n吉星义齿--义齿加工行业首选管理软件。\n\n没有所需的订单信息！！\n";

                        }
                        if (sql == "")
                        {
                            sql += "欢迎关注\n吉星义齿--义齿加工行业首选管理软件。\n\n请正确输入订单号：\n根据输入订单号可以查询\n1、产品的质保卡信息\n2、产品的材料成分信息\n";
                        }

                    }
                    #endregion

                    #region
                    else if (requestXML.MsgType == "event")
                    {
                        if (requestXML.Wxevent == "unsubscribe")
                        {
                            //取消关注                        
                        }
                        else
                        {

                            if (requestXML.Wxevent != null)
                            {
                                //菜单单击事件
                                if (requestXML.Wxevent.Equals("click"))
                                {
                                    if (requestXML.EventKey.Equals("subkey1"))
                                    {
                                        #region
                                        string orderid = "";
                                        int num = 0;
                                        sql = "";
                                        DataSet ds = new DataSet();
                                        DataSet dss = select.DatabaseName(requestXML.FromUserName);//当前数据库
                                        if (dss != null && dss.Tables[0].Rows.Count > 0)
                                        {
                                            for (int a = 0; a < dss.Tables[0].Rows.Count; a++)
                                            {
                                                database = dss.Tables[0].Rows[a]["DataBaseName"].ToString();
                                                if (database != "")
                                                {
                                                    database = database + ".";
                                                    ds = select.getOrdering(requestXML.FromUserName, database);//查询产品的加工进程
                                                    if (ds != null)
                                                    {
                                                        sql += "产品的加工进程\n";
                                                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                                        {
                                                            if (orderid != ds.Tables[0].Rows[i]["order_id"].ToString())
                                                            {
                                                                num = num + 1;
                                                                sql += "-----------------\n";
                                                                sql += "(" + num + ")订单编号:" + ds.Tables[0].Rows[i]["order_id"].ToString() + "\n";
                                                                sql += "医院名称:" + ds.Tables[0].Rows[i]["hospital"].ToString() + "\n";
                                                                sql += "医生姓名:" + ds.Tables[0].Rows[i]["doctor"].ToString() + "\n";
                                                                string indte = "";
                                                                if (ds.Tables[0].Rows[i]["inDate"].ToString() != "")
                                                                {
                                                                    indte = Convert.ToDateTime(ds.Tables[0].Rows[i]["inDate"].ToString()).ToString("yyyy-MM-dd");

                                                                }
                                                                sql += "到厂日期:" + indte + "\n";
                                                                string preoutdate = "";
                                                                if (ds.Tables[0].Rows[i]["preoutdate"].ToString() != "")
                                                                {
                                                                    preoutdate = Convert.ToDateTime(ds.Tables[0].Rows[i]["preoutdate"].ToString()).ToString("yyyy-MM-dd");

                                                                }
                                                                sql += "出厂日期:" + preoutdate + "\n";
                                                                sql += "患者姓名:" + ds.Tables[0].Rows[i]["patient"].ToString() + "\n";
                                                                orderid = ds.Tables[0].Rows[i]["order_id"].ToString();
                                                            }
                                                            sql += "产品名称:" + ds.Tables[0].Rows[i]["itemname"].ToString() + "\n";
                                                            sql += "工序:" + ds.Tables[0].Rows[i]["process"].ToString() + "\n";

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            sql += "当前没有您的产品的加工进程信息！！\n";
                                        }
                                        if (sql == "")
                                        {
                                            sql += "当前没有您的产品的加工进程信息！";
                                        }
                                        #endregion
                                    }
                                    else if (requestXML.EventKey.Equals("subkey3"))
                                    {
                                        sql += "结算单汇总查询统计(分析情况报告)";
                                    }
                                    else if (requestXML.EventKey.Equals("key3"))
                                    {
                                        sql += "欢迎关注\n吉星义齿--义齿加工行业首选管理软件。\n\n请正确输入订单号：\n根据输入订单号可以查询\n1、产品的质保卡信息\n2、产品的材料成分信息\n";
                                    }
                                }

                            }
                        }
                    }
                    #endregion
                    //发送消息，把收到的内容回复给发送者
                    string responeJsonStr = "";
                    if (requestXML.Wxevent != null && requestXML.Wxevent.Equals("click") && requestXML.EventKey.Equals("subkey3"))
                    {

                        responeJsonStr = "{";
                        responeJsonStr += "\"touser\": \"" + requestXML.FromUserName + "\",";
                        responeJsonStr += "\"msgtype\": \"news\",";
                        responeJsonStr += "\"agentid\": \"0\",";
                        responeJsonStr += "\"news\": {";
                        responeJsonStr += "\"articles\":[{";
                        responeJsonStr += "\"title\": \"结算单查询\",";
                        responeJsonStr += "\"description\": \"" + sql + "\",";
                        responeJsonStr += "\"url\": \"http://www.chaya8.com/WebSearchs.aspx?UserName=" + requestXML.FromUserName + "\",";
                        responeJsonStr += "\"picurl\": \"http://down.cnshu.cn/2011/cnshu_images/huanglu/1106241144.jpg\"";
                        responeJsonStr += "}]";
                        responeJsonStr += "}";
                        responeJsonStr += "}";

                    }
                    else
                    {
                        responeJsonStr = "{";
                        responeJsonStr += "\"touser\": \"" + requestXML.FromUserName + "\",";
                        responeJsonStr += "\"msgtype\": \"text\",";
                        responeJsonStr += "\"agentid\": \"0\",";
                        responeJsonStr += "\"text\": {";
                        responeJsonStr += "\"content\": \"" + sql + "\"";
                        responeJsonStr += "},";
                        responeJsonStr += "\"safe\":\"0\"";
                        responeJsonStr += "}";
                    }
                    weixinsendmessage wxsend = new weixinsendmessage();
                    wxsend.SendQYMessage(corpId, corpsecret, responeJsonStr, Encoding.UTF8);
                }


            }
            #endregion
            else //回调验证
            {
                string sEchoStr = "";
                ret = wxcpt.VerifyURL(sVerifyMsgSig, sVerifyTimeStamp, sVerifyNonce, sVerifyEchoStr, ref sEchoStr);
                if (ret != 0)
                {
                    HttpContext.Current.Response.Write("ERR: VerifyURL fail, ret: " + ret);
                    return;
                }
                HttpContext.Current.Response.Write(sEchoStr);

            }
        }
        else
        {
            wx.Visible = false;
            login.Visible = true;
            Timer1.Enabled = false;

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
        }
        
    }
    protected void CheckLogin()
    {
        string txtUserName = yeyRequest.Params("txtUser");
        string txtPassword = yeyRequest.Params("txtPassword");

        if (!string.IsNullOrEmpty(txtUserName))
        {
            try
            {
                //根据用户名查询数据检测是否有匹配数据
                int result = doLogin(txtUserName, txtPassword);
                if (result == 0)
                {
                    Response.Redirect("OrderManagement/OrderList.aspx?type=Order");
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
            catch(Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "sucess", "art.dialog({title: false,time: 2,icon: 'error',content: '" + ex.Message+ "'});", true);
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
            if (objUser.Kind == "S")
            {
                accessList = "Admin";
                Session["accessList"] = accessList;
                Response.Redirect("System/JGCList.aspx?type=System");
            }
            else
            {
                ccWhere.Clear();
                ccWhere.AddComponent("JGCBM", objUser.BelongFactory, SearchComponent.Equals, SearchPad.NULL);
                JX_USERS jxUser = servComm.GetEntity<JX_USERS>(null, ccWhere);
                if (jxUser != null && !String.IsNullOrEmpty(jxUser.DBUser))
                {
                    string DBName = "JJ2011";
                    if (jxUser.DBsameJGCBM == "Y")
                    {
                        DBName = jxUser.JGCBM;
                    }
                    string factoryConnection = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", jxUser.DBServerIP, DBName ,jxUser.DBUser, jxUser.DBPassword);
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
                    }
                    else
                    {
                        Session["IDRule"] = "B";
                    }

                    WriteCookie(UserConstant.COOKIE_SAVEDUSERNAME, EncryptClass.Encode(strUserName));
                    WriteCookie(UserConstant.COOKIE_SAVEDPASSWORD, EncryptClass.Encode(strPassword));

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
                        Response.Redirect("/Weixinclient/WXLogin.aspx");
                        Response.End();
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


    protected void Timer1_Tick(object sender, EventArgs e)
    {
        DataSet dss = select.SendContent();
        if (dss != null && dss.Tables[0].Rows.Count > 0)
        {
            string responeJsonStr = "{";
            responeJsonStr += "\"touser\": \"" + dss.Tables[0].Rows[0]["weixinNum"].ToString() + "\",";
            responeJsonStr += "\"msgtype\": \"text\",";
            responeJsonStr += "\"agentid\": \"0\",";
            responeJsonStr += "\"text\": {";
            responeJsonStr += "\"content\": \"" + dss.Tables[0].Rows[0]["Contents"].ToString() + "\"";
            responeJsonStr += "},";
            responeJsonStr += "\"safe\":\"0\"";
            responeJsonStr += "}";
            weixinsendmessage wxsend = new weixinsendmessage();
            wxsend.SendQYMessage(corpId, corpsecret, responeJsonStr, Encoding.UTF8);
            int id = Convert.ToInt32(dss.Tables[0].Rows[0]["id"].ToString());
            select.Delete(id);

        }

        DataSet dc = select.AddDataUser();
        if (dc != null && dc.Tables[0].Rows.Count > 0)
        {
            string responeJsonStr = "{";
            responeJsonStr += "\"userid\": \"" + dc.Tables[0].Rows[0]["weixinNum"].ToString() + "\",";
            responeJsonStr += "\"name\": \"" + dc.Tables[0].Rows[0]["UserName"].ToString() + "\",";
            responeJsonStr += "\"department\": \"[2]\",";
            responeJsonStr += "\"mobile\": \"" + dc.Tables[0].Rows[0]["Tel"].ToString() + "\",";
            string sex = "";
            if (dc.Tables[0].Rows[0]["Sex"].ToString() == "男")
            {
                sex = "1";
            }
            else
            {
                sex = "0";
            }
            responeJsonStr += "\"gender\": \"" + sex + "\",";
            responeJsonStr += "\"email\": \"" + dc.Tables[0].Rows[0]["Email"].ToString() + "\",";
            responeJsonStr += "\"weixinid\": \"" + dc.Tables[0].Rows[0]["UserId"].ToString() + "\"";
            responeJsonStr += "}";
            weixinsendmessage wxsend = new weixinsendmessage();
            wxsend.newadduser(corpId, corpsecret, responeJsonStr, Encoding.UTF8);
            int id = Convert.ToInt32(dc.Tables[0].Rows[0]["id"].ToString());
            select.Update(id);

        }


    }

    public static void CreateMenuTest(string corpId, string corpsecret)
    {

        Weixin.Mp.Sdk.Domain.Menu menu = new Weixin.Mp.Sdk.Domain.Menu();

        List<Weixin.Mp.Sdk.Domain.Button> button = new List<Weixin.Mp.Sdk.Domain.Button>();

        Weixin.Mp.Sdk.Domain.Button subBtn1 = new Weixin.Mp.Sdk.Domain.Button()
        {
            key = "subkey1",
            name = "订单跟踪",
            sub_button = null,
            type = "click",
            url = "http://"
        };
        Weixin.Mp.Sdk.Domain.Button subBtn2 = new Weixin.Mp.Sdk.Domain.Button()
        {
            key = "subkey2",
            name = "义齿资料",
            sub_button = null,
            type = "view",
            url = ""
        };
        Weixin.Mp.Sdk.Domain.Button subBtn3 = new Weixin.Mp.Sdk.Domain.Button()
        {
            key = "subkey3",
            name = "义齿新闻",
            sub_button = null,
            type = "click",
            url = "http://"
        };
        List<Weixin.Mp.Sdk.Domain.Button> subBtnAll = new List<Weixin.Mp.Sdk.Domain.Button>();
        List<Weixin.Mp.Sdk.Domain.Button> subBtnAls = new List<Weixin.Mp.Sdk.Domain.Button>();
        subBtnAll.Add(subBtn1);
        subBtnAls.Add(subBtn2);
        subBtnAls.Add(subBtn3);
        Weixin.Mp.Sdk.Domain.Button btn = new Weixin.Mp.Sdk.Domain.Button()
        {
            key = "key1",
            name = "专享服务",
            url = "httpbig",
            type = "click",
            sub_button = subBtnAll
        };
        button.Add(btn);

        btn = new Weixin.Mp.Sdk.Domain.Button()
        {
            key = "key2",
            name = "关于义齿",
            url = "httpbig",
            type = "click",
            sub_button = subBtnAls
        };
        button.Add(btn);

        btn = new Weixin.Mp.Sdk.Domain.Button()
        {
            key = "key3",
            name = "问题解答",
            url = "httpbig",
            type = "click",
        };
        button.Add(btn);

        menu.button = button;

        MenuGroup mg = new MenuGroup()
        {
            menu = menu
        };

        string postData = mg.ToJsonString();
        weixinsendmessage wxsend = new weixinsendmessage();
        wxsend.CreateMenu(corpId, corpsecret, postData, Encoding.UTF8);
    }

    public bool b(string s)
    {
        string pattern = "^[0-9]*$";
        Regex rx = new Regex(pattern);
        return rx.IsMatch(s);
    }

    public class WeiXinRequest
    {
        private string toUserName;
        /// <summary>
        /// 消息接收方微信号，一般为公众平台账号微信号
        /// </summary>
        public string ToUserName
        {
            get { return toUserName; }
            set { toUserName = value; }
        }

        private string fromUserName;
        /// <summary>
        /// 消息发送方微信号
        /// </summary>
        public string FromUserName
        {
            get { return fromUserName; }
            set { fromUserName = value; }
        }

        private string createTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }

        private string msgType;
        /// <summary>
        /// 信息类型 地理位置:location,文本消息:text,消息类型:image，音频：voice，视频：video,取消关注：Action
        /// </summary>
        public string MsgType
        {
            get { return msgType; }
            set { msgType = value; }
        }

        private string content;
        /// <summary>
        /// 信息内容
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        private string msgId;
        /// <summary>
        /// 消息ID（文本）
        /// </summary>
        public string MsgId
        {
            get { return msgId; }
            set { msgId = value; }
        }

        private string wxevent;
        /// <summary>
        /// 取消关注时的Event节点
        /// </summary>
        public string Wxevent
        {
            get { return wxevent; }
            set { wxevent = value; }
        }

        private string eventKey;
        /// <summary>
        /// 取消关注时的EventKey节点
        /// </summary>
        public string EventKey
        {
            get { return eventKey; }
            set { eventKey = value; }
        }


        private string location_X;
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public string Location_X
        {
            get { return location_X; }
            set { location_X = value; }
        }

        private string location_Y;
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Location_Y
        {
            get { return location_Y; }
            set { location_Y = value; }
        }

        private string scale;
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public string Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        private string label;
        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label
        {
            get { return label; }
            set { label = value; }
        }

        private string picUrl;
        /// <summary>
        /// 图片链接，开发者可以用HTTP GET获取
        /// </summary>
        public string PicUrl
        {
            get { return picUrl; }
            set { picUrl = value; }
        }

    }


    protected void Language_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlLanguage.SelectedValue == "0")
        {
            Session["Language"] = "zh-cn";
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-cn") ;
        }
        else
        {
            Session["Language"] = "en";
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
        }
          
    }
}
