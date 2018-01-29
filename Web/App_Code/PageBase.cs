using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Text.RegularExpressions;

using D2012.Domain.Entities;
using D2012.Common;
using D2012.Domain.Services;
using D2012.Common.DbCommon;
using System.Globalization;
using System.Threading;
using DDTourCommon.WXPay;
using WXPay.V3Demo;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;

/// <summary>
///AdminPageBase 的摘要说明



/// </summary>
public class PageBase : System.Web.UI.Page
{
    protected string SaveFilePath
    {
        get
        {
            return "/uploadedFiles/" + LoginUser.BelongFactory + "/" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + "/";
        }
    }

    protected string GetSignalTicket(string timeStamp, string appId, string APPSECRET)
    {
        string acetoken = GetAccessToken(appId, APPSECRET);
        string signal = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + acetoken + "&type=jsapi";
        ReturnValue retValue = StreamReaderUtils.StreamReader(signal, Encoding.UTF8);

        string ticket = StringUtils.GetJsonValue(retValue.Message, "ticket").ToString();

        string url = "jsapi_ticket=" + ticket + "&noncestr=Wm3WZYTPz0wzccnW&timestamp=" + timeStamp + "&url=" + Request.Url.AbsoluteUri.ToString();
        return SHA1_Hash(url);
    }

    static public string SHA1_Hash(string str_sha1_in)
    {
        SHA1 sha1 = new SHA1CryptoServiceProvider();
        byte[] bytes_sha1_in = UTF8Encoding.Default.GetBytes(str_sha1_in);
        byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
        string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
        str_sha1_out = str_sha1_out.Replace("-", "");
        return str_sha1_out.ToLower();
    }

    //获取微信凭证access_token的接口
    public static string getAccessTokenUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
    #region 获取微信凭证
    public string GetAccessToken(string appid, string APPSECRET)
    {
        string accessToken = "";
        //获取配置信息Datatable



        string respText = "";
        //获取appid和appsercret

        //获取josn数据
        string url = string.Format(getAccessTokenUrl, appid, APPSECRET);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        using (Stream resStream = response.GetResponseStream())
        {
            StreamReader reader = new StreamReader(resStream, Encoding.Default);
            respText = reader.ReadToEnd();
            resStream.Close();
        }
        JavaScriptSerializer Jss = new JavaScriptSerializer();
        Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);
        //通过键access_token获取值
        try
        {
            accessToken = respDic["access_token"].ToString();
        }
        catch (Exception ex)
        {
        }


        return accessToken;
    }

    #endregion 获取微信凭证

    public ReturnValue GetUserInfo(ref string strAccess_Token, string APPID, string APPSECRET)
    {

        string timeStamp = "";
        const string TENPAY = "1";
        const string mchid = "1242234302";//商户号
        const string OAUTH2 = "https://open.weixin.qq.com/connect/oauth2/authorize";
        const string OAUTH2_ACCESS_TOKEN = "https://api.weixin.qq.com/sns/oauth2/access_token";
        string strWeixin_OpenID = "";

        timeStamp = TenpayUtil.getTimestamp();
        string strCode = Request["code"] == null ? "" : Request["code"];
        if (string.IsNullOrEmpty(strCode))
        {
            //参数需要自己处理
            string _OAuth_Url = Interface_WxPay.OAuth2_GetUrl_Pay(Request.Url.ToString(), APPID);
            Response.Redirect(_OAuth_Url);
            return null;
        }
        else
        {
            ReturnValue retValue = Interface_WxPay.OAuth2_Access_Token(strCode, APPID, APPSECRET);
            if (retValue.HasError)
            {
                // WriteFile(Server.MapPath("") + "\\Log.txt", "ewrwrwrwrwrwrer");

                Response.Write("获取code失败：" + retValue.Message);
                return null;
            }

            // WriteFile(Server.MapPath("") + "\\Log.txt", "11111111111111111");
            strWeixin_OpenID = retValue.GetStringValue("Weixin_OpenID");
            string strWeixin_Token = retValue.GetStringValue("Weixin_Token");
            strAccess_Token = strWeixin_Token;
            string refresh_token = retValue.GetStringValue("refresh_token");
            //WriteFile(Server.MapPath("") + "\\Log.txt", "2222222222222222");
            //  Response.Write(strWeixin_OpenID);
            if (string.IsNullOrEmpty(strWeixin_OpenID))
            {
                Response.Write("openid出错");
                return null;
            }

            string userinfotoken = "https://api.weixin.qq.com/sns/userinfo?access_token=" + strWeixin_Token + "&openid=" + strWeixin_OpenID + "&lang=zh_CN";

            retValue = StreamReaderUtils.StreamReader(userinfotoken, Encoding.UTF8);
            return retValue;
        }
    }

    public int CurrentUserID
    {

        get {
            return int.Parse(HttpContext.Current.Session["USERID"].ToString());
        }
    }

    public bool IsCN
    {
        get
        {
            return Session["Language"] == "zh-cn";
        }
    }

    public string vieworganizationsql
    {
        get {
            return "select * from ( select  c.id as sellerid,c.Seller, b.id as hospitalid,b.hospital, a.id as doctorid,a.doctor from doctor a inner join Hospital b on a.Hospitalid = b.id inner join seller c on b.sellerid = c.id ) t ";
        }
    }

    public string distinctvieworganizationsql
    {
        get
        {
            return "select distinct doctorid ,hospitalid,sellerid from ( select  c.id as sellerid,c.Seller, b.id as hospitalid,b.hospital, a.id as doctorid,a.doctor from doctor a inner join Hospital b on a.Hospitalid = b.id inner join seller c on b.sellerid = c.id ) t ";
        }
    }


    public string vieworganizationorigin
    {
        get
        {
            return " ( select  c.id as sellerid,c.Seller, b.id as hospitalid,b.hospital, a.id as doctorid,a.doctor from doctor a inner join Hospital b on a.Hospitalid = b.id inner join seller c on b.sellerid = c.id ) t ";
        }
    }

    public string ModelProcessingSummary
    {
        get
        {
            return "SELECT orders.Order_ID,  orders.serial,orders.orderclass, orders.ModelNo ,  orders.process, orders.producer, orders.sellerid,  orders.Seller,  orders.ClientServer, orders.hospitalid, orders.getman, orders.Sender, "	
            +" orders.charges,   orders.Area,   orders.hospital,  orders.doctor, orders.patient, orders.outname,orders.Editor,products.itemname,  OrdersDetail.Qty,    	( case when len(ltrim(a_teeth))= 0 then '' else '右上' + rtrim(a_teeth) end ) +( case when len(ltrim(b_teeth))= 0 then '' else '左上' + rtrim(b_teeth) end ) +( case when len(ltrim(c_teeth))= 0 then '' else '右下' + rtrim(c_teeth) end ) +( case when len(ltrim(d_teeth))= 0 then '' else '左下' + rtrim(d_teeth) end ) as teeths,  "	
            + "orders.age,     OrdersDetail.bColor,  orders.hurry, orders.TryPut, orders.slow,  orders.sex,   	orders.indate,  orders.preoutdate, orders.OutFlag, orders.regtime, orders.OutDate, orders.OutNo, orders.SN,  orders.RpID,OrdersDetail.subId, OrdersDetail.Productline,  OrdersDetail.ProductId, OrdersDetail.price,  OrdersDetail.a_teeth,  " 	
            + "OrdersDetail.b_teeth,    OrdersDetail.c_teeth,    OrdersDetail.d_teeth, OrdersDetail.Nobleclass ,  	isnull(OrdersDetail.amount, 0) as amount, OrdersDetail.NobleWeight,  OrdersDetail.Nobleprice, isnull(OrdersDetail.nobleAmount, 0) as nobleAmount,  	orders.belong,  products.itemclass,  products.smallclass,    orders.Require   ,   orders.Explain FROM(orders left join OrdersDetail on(OrdersDetail.Order_ID = orders.Order_ID) and(OrdersDetail.serial = orders.serial)) "
            +" left join  products on(products.id = OrdersDetail.ProductId) where products.itemname is not null ";
        }
    }

    public string ConfirmDeList
    {
        get
        {
            return "SELECT orders.sellerid,orders.hospital ,orders.hospitalid ,convert(nvarchar(10), orders.preoutdate, 120) as preoutdate,orders.charges,convert(varchar(8), orders.indate, 11) as indate , "
        + " convert(varchar(8), orders.OutDate, 11) as OutDate,(select DictName from DictDetail where ClassID = 'OrderClass' and Code = orders.orderclass) as orderclass  ,  orders.orderclass as orderclasscode,orders.ModelNo,   orders.doctor as doctorsname,   "
         + " orders.patient as ptname,   OrdersDetail.Qty,  OrdersDetail.Price, OrdersDetail.amount as labfee, OrdersDetail.NobleWeight as gramsofalloy,OrdersDetail.Nobleprice as alloyprice,OrdersDetail.nobleAmount as alloyfee,   products.Ename as descriptionofgoods,  "
         + " OrdersDetail.a_teeth, OrdersDetail.b_teeth, OrdersDetail.c_teeth,  OrdersDetail.d_teeth,orders.order_id,orders.serial,' ' teeth, case when OrdersDetail.a_teeth <> '' and OrdersDetail.b_teeth <> '' and OrdersDetail.c_teeth <> '' and OrdersDetail.d_teeth <> '' then '' else dbo.gf_toNational(OrdersDetail.a_teeth, 'A') + [dbo].[gf_toNational](OrdersDetail.b_teeth, 'B') + [dbo].[gf_toNational](OrdersDetail.c_teeth, 'C') + [dbo].[gf_toNational](OrdersDetail.d_teeth, 'D') end as region "
    + " FROM orders,OrdersDetail,products WHERE(OrdersDetail.Order_ID = orders.Order_ID) and (OrdersDetail.serial = orders.serial) and (OrdersDetail.ProductId = products.id)";

        }
    }

    public string VWDisinRec
    {
        get
        {
            return "select Order_ID,serial,case when DiFlag = 'E' then N'入货消毒' when DiFlag = 'G' then N'出货消毒' else '' end DiFlag ,StartTime,EndTime,Method,BatchNo,Reg from DisinRec ";
        }
    }

    public string VWEquipRec
    {
        get
        {
            return " SELECT EquipRec.Order_ID, EquipRec.Serial, EquipRec.EquipName, EquipRec.Spec, EquipRec.EquipNo, EquipRec.StartTime, EquipRec.EndTime, EquipRec.DBTemp, working_procedure.name , EquipRec.Reg FROM EquipRec left join working_procedure on EquipRec.gserial = working_procedure.serial";
        }
    }

    public string FinanceSummaryDetail
    {
        get
        {
            return "SELECT  orders.Order_ID , orders.serial ,orders.ModelNo , orders.seller ,orders.sellerid , orders.hospital ,orders.hospitalid , orders.orderclass, (select top 1 DictName from dictdetail where Code = orders.orderclass and ClassID = 'OrderClass') as orderclassname  , orders.doctor ,orders.patient ,convert(nvarchar(8), orders.indate, 11) as indate ,orders.Outflag ,	"
+ " convert(nvarchar(8), orders.Outdate, 11) as Outdate , ltrim(orders.OutSay) as Explain , ordersdetail.subid ,ordersdetail.productid , ordersdetail.qty ,ordersdetail.bColor , ordersdetail.price ,ordersdetail.amount ,rtrim(ordersdetail.a_teeth) as a_teeth ,rtrim(ordersdetail.b_teeth) as b_teeth ,"
+ " rtrim(ordersdetail.c_teeth) as c_teeth , rtrim(ordersdetail.d_teeth) as d_teeth ,ordersdetail.Nobleclass , ordersdetail.NobleWeight , ordersdetail.nobleAmount ,products_itemname = products.itemname,    products.unit,(select top 1 corp from base) as corp,orders.charges ,orders.preoutdate FROM orders ,"
+ " ordersdetail ,products WHERE(ordersdetail.Order_ID = orders.Order_ID) and(ordersdetail.serial = orders.serial) and products.id = ordersdetail.productid ";
        }
    }

    public string FinanceSummary
    {
        get
        {
            return "SELECT  orders.sellerid,orders.hospital ,orders.doctor ,orders.hospitalid ,orders.preoutdate,convert(varchar(8),orders.indate,11) as indate ,convert(varchar(8),orders.OutDate,11) as OutDate ,orders.OutFlag ,orders.patient ,OrdersDetail.ProductId ,OrdersDetail.Qty ,OrdersDetail.Price ,OrdersDetail.amount ,rtrim(OrdersDetail.a_teeth) as a_teeth ,rtrim(OrdersDetail.b_teeth) as b_teeth ,rtrim(OrdersDetail.c_teeth) as c_teeth ,rtrim(OrdersDetail.d_teeth) as d_teeth ,orders.Order_ID , "
+" orders.serial ,(select top 1 DictName from dictdetail where Code = orders.orderclass and ClassID = 'OrderClass') as orderclass ,products.itemname,orders.charges,(select top 1 corp from base) as corp   FROM orders, OrdersDetail, products WHERE(OrdersDetail.Order_ID = orders.Order_ID) and (OrdersDetail.serial = orders.serial) and (OrdersDetail.ProductId = products.id)  ";
        }
    }

    public string VWORDERS
    {
        get
        {
            return "SELECT [Order_ID],[serial] ,case when [orderclass] = 'A' then N'正常' when [orderclass] = 'B' then N'返修' when [orderclass] = 'C' then N'返工' when [orderclass] = 'D' then N'退货' when [orderclass] = 'E' then N'订单作废' "
	  + " else '' end as orderclass,[ModelNo],[seller],[sellerid],[hospital] ,[hospitalid],[doctor] ,[tel] ,[patient],[age] ,[sex],[priceclass] ,[indate],[preoutDate],case when [hurry] = 'Y' then N'加急' else N'默认' end as hurry ,[Slow],[Require] "
      +" ,[Explain],[danzuo] ,[fenge],[regname],[regtime] ,[process],[OutFlag] ,[OutDate] ,[OutName] ,[OutNo],[PrntCount] ,[QlyName],[Qly],[QlyTime],[QlyRemark],[AuthResult],[Author],[AuthTime],[Editor] ,[TryPut] ,[ClientServer],[wCheck],[wCheckMan] "
      +" ,[wCheckTime],[getman],[Sender],[qaflag],[upflag] ,[producer],[Upweb],[Upwebdate],case when [charges]='A' then N'月结' when [charges]='B' then N'优惠' when [charges]='C' then N'现金' else '' end as charges ,[Area],[belong],[Courier],[CourierNo] ,[SN],[RpID] FROM [orders]";
        }
    }

    public string VWOrdersDetail
    {
        get
        {
            return "SELECT  a.[Order_ID],a.[serial],a.[subId],a.[ProductId],a.[Qty],a.[Price],a.[topPrice],a.[amount],rtrim(a.[a_teeth]) as [a_teeth],rtrim(a.[b_teeth]) as [b_teeth],rtrim(a.[c_teeth]) as [c_teeth],rtrim(a.[d_teeth]) as [d_teeth] "
      +" ,a.[Nobleclass],a.[NobleWeight] ,a.[Nobleprice],a.[nobleAmount] ,a.[Modi],a.[Valid],a.[modulus],a.[bColor],a.[MakeDays],a.[ProductLine] ,a.[ingredient],b.itemName FROM[OrdersDetail] a left join products b on a.productid = b.id";
        }
    }

    public string VWOrdersElement
    {
        get
        {
            return "  SELECT OrdersElement.Order_ID,  OrdersElement.serial,   OrdersElement.SubId,  OrdersElement.Name,  OrdersElement.batchNo,   OrdersElement.Productid,   OrdersElement.gserial,   OrdersElement.code,    OrdersElement.provider,    OrdersElement.Note,   OrdersElement.Maker,  OrdersElement.Kind,  " 
  + " OrdersElement.Spec,b.name as ProcedureName,c.itemname FROM OrdersElement OrdersElement left join working_procedure b on OrdersElement.gserial = b.serial left join products c on OrdersElement.Productid = c.id";
        }
    }

    public string VWPrice
    {
        get
        {
            return " select a.id,a.itemname,b.price,a.unit,a.sortno,b.topprice,a.valid,b.Serial from products a, prices b where a.id = b.productid and b.stopflag = 'N'     ";
        }
    }

    public string VWPriceByTemplate
    {
        get
        {
            return " select  distinct a.*,c.hospitalid,c.hospital from products a inner join OrdersDetail b on a.id = b.productid inner join Orders c on b.Order_ID = c.Order_ID and b.serial = c.serial where c.hospital is not null";
        }
    }

    public string VWProductCheckReport
    {
        get
        {
            return "  SELECT QSrec.ID,QSrec.Order_ID, QSrec.Serial,  QSrec.class,  rtrim(QSrec.QSItemname) as QSItemname,  QSrec.QSQty,  QSrec.qsserial,  rtrim(QSrec.DocuNo) as DocuNo,  rtrim(QSrec.QSitem) as QSitem,  QSrec.QSrely,  rtrim(QSrec.QSask) as QSask,   QSrec.Remark1,   QSrec.Remark2,  QSrec.a_teeth,   QSrec.b_teeth,   QSrec.c_teeth,   QSrec.d_teeth,   QSrec.QSresult,   QSrec.conclusion, "
            +" QSrec.AllConclusion,   QSrec.QSman,   QSrec.QSdept,   QSrec.processing,   QSrec.QSleader,   QSrec.QStime FROM QSrec ";
        }
    }

    public string VWQSProcRec
    {
        get
        {
            return "  SELECT QSProcRec.id,   QSProcRec.Order_ID,   QSProcRec.Serial, QSProcRec.QSItemname,   QSProcRec.QSQty, QSProcRec.QSserial,   QSProcRec.QSitem,   QSProcRec.QSask,  QSProcRec.Gserial,   QSProcRec.Gname,  QSProcRec.GetIn,   QSProcRec.Finish,   QSProcRec.Producer,   QSProcRec.Checker,  QSProcRec.Special,   QSProcRec.Result,  QSProcRec.OutDisiTime, QSProcRec.OutDisiOper,   QSProcRec.Remark,   orders.hospital, " 
	 +" orders.patient,   orders.indate,   orders.regtime,   orders.regname ,(select  name from working_procedure where  working_procedure.serial = QSProcRec.QSserial) as ProcedureName FROM QSProcRec, orders  WHERE(QSProcRec.Order_ID = orders.Order_ID) and(QSProcRec.Serial = orders.serial)";
        }
    }

    
    private CultureInfo cultureInfo = null;

    public CultureInfo GetCurrentCulture
    {
        get
        {
            
            if (cultureInfo == null)
            {
                if (Request["languageType"] == "1")
                {
                    Session["Language"] = "en";

                }
                else if (Request["languageType"] == "0")
                {
                    Session["Language"] = "zh-cn";
                }
                else if (Session["Language"] == null && Request["Language"] == null)
                {
                    Session["Language"] = "zh-cn";
                }
                else if (Request["Language"]!= null && Request["Language"].ToString() == "zhcn")
                {
                    Session["Language"] = "zh-cn";
                   
                }
                else if (Request["Language"] != null && Request["Language"].ToString() == "en")
                {
                    Session["Language"] = "en";
                }

                return new CultureInfo(Session["Language"] == null ? "zh-cn" : Session["Language"].ToString());
            }
            return cultureInfo;
        }
    }

    protected override void InitializeCulture()
    {
        Thread.CurrentThread.CurrentUICulture = GetCurrentCulture;
    }

    public WUSERS LoginUser
    {
        get {
            return (WUSERS)(Session["objUser"]);
        }
    }

    public string IDRule
    {
        get{
            return Session["IDRule"].ToString();
        }
    }
    /// <summary>
    /// doAjax
    /// </summary>
    protected virtual void doAjax()
    {
        Response.End();
    }

    //protected override void Render(HtmlTextWriter writer)
    //{
    //    if (Request.Params["_xml"] == null)
    //    {
    //        base.Render(writer);
    //    }
        
    //}

    protected static string FormatName()
    {
        int RanNum = 0;
        string TempStr = "";
        Random Rnd = new Random();
        RanNum = Rnd.Next(100000, 900000);
        //TempStr = Year(now) & Month(now) & Day(now) & RanNum & "." & FileExt 
        TempStr = DateTime.Now.ToString("MMddHHmm") + RanNum;
        return TempStr;
    }
    protected string factoryConnectionString
    {
        get{
            return Session["factoryConnectionString"].ToString();
        }
    }

    public bool CreatDBScript(string connectonstring)
    {
        try
        {
            FileInfo file = new FileInfo(Request.PhysicalApplicationPath+ "uploadedFiles\\script.sql");
        string script = file.OpenText().ReadToEnd();

       
            //执行脚本
            SqlConnection conn = new SqlConnection(connectonstring);
            Microsoft.SqlServer.Management.Smo.Server server = new Server(new ServerConnection(conn));
            int i = server.ConnectionContext.ExecuteNonQuery(script);

            return true;

        }
        catch (Exception es)
        {
            return false;
        }
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreLoad(object sender, EventArgs e)
    {
        if (Request.RawUrl.Contains("Weixinclient"))
        {
            Session["FromWeixin"] = "1";
        }
        if (Session["UserName"] == null && (!Request.RawUrl.Contains("WXLogin.aspx") && !Request.RawUrl.Contains("WXKQLogin.aspx")) && Request.RawUrl.Contains("GeestarWeixinclient"))
        {
            Response.Write("<script>top.location='/Weixinclient/WXLogin.aspx'</script>");
            Response.End();
        }
        else if (Session["UserName"] == null && (!Request.RawUrl.Contains("WXLogin.aspx") && !Request.RawUrl.Contains("WXKQLogin.aspx")) && Request.RawUrl.Contains("Weixinclient"))
        {           
            Response.Write("<script>top.location='/Weixinclient/WXLogin.aspx'</script>");
            Response.End();           
        }

        if (Session["UserName"] == null && !Request.RawUrl.Contains("login.aspx") && Session["FromWeixin"] == null && !Request.RawUrl.Contains("Weixinclient"))
        {
            //HttpContext.Current.Response.Redirect("/login.aspx");
            Response.Write("<script>top.location='/login.aspx'</script>");
            Response.End();
        }
        else
        {
           
        }



    }

    protected string UserName {
        get {
            return Session["USERNAME"].ToString();
        }
    }

    

    protected string TrimWithNull(object strValue)
    {
        if(strValue == null)
        {
            return "";
        }
        else{
            return strValue.ToString().Trim();
        }
    }

    protected Hashtable GetOrganization
    {
        get
        {
            return (Hashtable)(Session["Organization"]);
        }
    }

    protected void GetFilterByKind(ref ConditionComponent paraWhere,string inDataBase = "")
    {
        if (paraWhere != null)
        {
            string dataFilterByKind = inDataBase==""?" BelongFactory = '"+LoginUser.BelongFactory+"'":" 1=1 ";
            if (LoginUser.Kind == "B")
            {
                dataFilterByKind += " and sellerid =" + LoginUser.AssocNo;
            }
            else if (LoginUser.Kind == "C")
            {
                dataFilterByKind += " and HospitalID =" + LoginUser.AssocNo;
            }
            else if (LoginUser.Kind == "D" && inDataBase =="")
            {
                dataFilterByKind += " and DoctorID =" + LoginUser.AssocNo;
            }
            else if (LoginUser.Kind == "D" && inDataBase != "")
            {
                dataFilterByKind += " and doctor = (select top 1 doctor from doctor where id = " + LoginUser.AssocNo + ")";
            }

            if (dataFilterByKind != "")
            {
                if (paraWhere.sbComponent.Length == 0)
                {
                    paraWhere.sbComponent.Append(dataFilterByKind);
                }
                else
                {
                    paraWhere.sbComponent.Append(" and " + dataFilterByKind);
                }
            }
           
        }
    }

    /// <summary>
    /// 绑定分类
    /// </summary>
    protected DataTable BindDictClass(ServiceCommon facComm,ConditionComponent ccWhere, string ClassID)
    {
        ccWhere.Clear();
        ccWhere.AddComponent("ClassID", ClassID, SearchComponent.Equals, SearchPad.NULL);
        return facComm.GetListTop(0, " Code,DictName ", "DictDetail", ccWhere);

    }

    /// </summary>
    /// <param name="strName">名称</param>
    /// <param name="strValue">值</param>
    public static void WriteCookie(string strName, string strValue)
    {
        HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
        if (cookie == null)
        {
            cookie = new HttpCookie(strName);
            HttpContext.Current.Response.AppendCookie(cookie);
            cookie.Expires = DateTime.Now.AddDays(30);
            cookie.Value = strValue;
        }
        else
        {
            cookie.Expires = DateTime.Now.AddDays(30); 
            cookie.Value = strValue;
        }

        HttpContext.Current.Response.AppendCookie(cookie);
    }

    public static string GetCookie(string strName)
    {
        if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            return HttpContext.Current.Request.Cookies[strName].Value.ToString();

        return "";
    }

    protected string ConvertShortDate(string paraDate)
    {
        if (paraDate.Trim() == "")
        {
            return "";
        }
       // return " CONVERT(varchar(8),cast('" + paraDate + "' as datetime),11)";
        return paraDate.Substring(2,8).Replace("-","/");
    }

    public static string GetRandom()
    {
        string num = "";
        Random raninit = new Random(DateTime.Now.Millisecond);
        for (int i = 0; i < 6; i++)
        {
            Random ran = new Random(raninit.Next(i, int.MaxValue));
            num = num + ran.Next(1, int.MaxValue).ToString().Substring(1, 1);
        }
        return num;
    }

    protected static string LimitStringLength(String str, int limitLength)
    {
        if (str.Length > limitLength)
        {
            return str.Substring(0, limitLength);
        }
        else
        {
            return str;
        }
    }
}
