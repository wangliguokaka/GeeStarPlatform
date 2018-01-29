using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using D2012.Common;
using D2012.Common.DbCommon;
using D2012.Domain.Services;
using D2012.Domain.Entities;
using System.Collections.Generic;

namespace Weixin.RGPWEB.ascx
{
   
    public partial class querycontrolsm : System.Web.UI.UserControl
    {

        protected string userHospitalID = "";
        protected string userDoctorID = "";
        protected string userSellerID = "";
        ConditionComponent ccWhere = new ConditionComponent();
        protected DataTable dtCategory = null;
        ServiceCommon facComm = null;
        public DataTable dtSource = new DataTable();
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session["factoryConnectionString"] == null)
            {
                Response.Redirect(Request.Url.GetLeftPart(UriPartial.Authority) + "//Weixinclient//WXLogin.aspx");
            }
            facComm = new ServiceCommon(Session["factoryConnectionString"].ToString());
            ccWhere.Clear();
            ccWhere.AddComponent("ClassID", "OrderClass", SearchComponent.Equals, SearchPad.NULL);
            dtCategory = facComm.GetListTop(0, " Code,DictName ", "DictDetail", ccWhere);




        }
        public string DispalyType
        {
            set;
            get;
        }
        public bool IsCN
        {
            get
            {
                return Session["Language"] == "zh-cn";
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        protected string TranslatChinese(string strChinese, string strEnglish)
        {
            if (IsCN)
            {
                return strChinese;
            }
            else
            {
                return strEnglish;
            }
        }

        protected Hashtable GetOrganization
        {
            get
            {
                return (Hashtable)(Session["Organization"]);
            }
        }

        public string OrderString
        {
            get {
                string sortList = Request["sortList"];
                string sortType = Request["ordercheck"] == null ? "0" : Request["ordercheck"].ToString();
                if (sortType == "0")
                {
                    sortType = " asc ";
                }
                else
                {
                    sortType = " desc ";
                }
                if (String.IsNullOrEmpty(sortList) || sortList == "0")
                {
                    return " Order_ID " + sortType;
                }
                else if (sortList == "1")
                {
                    return " indate  " + sortType;
                }
                else if (sortList == "2")
                {
                    return " outdate  " + sortType;
                }
                else if (sortList == "3")
                {
                    return " expire  " + sortType;
                }
                else if (sortList == "4")
                {
                    return " Hurry  " + sortType;
                }
                else if (sortList == "5")
                {
                    return " TryPut  " + sortType;
                }
                else if (sortList == "6")
                {
                    return " Slow  " + sortType;
                }
                else if (sortList == "7")
                {
                    return " orderclass  " + sortType;
                }
                else
                {
                    return "";
                }
            }
        }
        public ConditionComponent GetCondtion()
        {
            ccWhere.Clear();
            if (GetOrganization.Count > 0)
            {
                userSellerID = GetOrganization["sellerid"].ToString(); ;
            }
            if (GetOrganization.Count == 2)
            {
                userHospitalID = GetOrganization["hospitalid"].ToString();
            }
            else if (GetOrganization.Count == 3)
            {
                userHospitalID = GetOrganization["hospitalid"].ToString();
                userDoctorID = GetOrganization["doctorid"].ToString();
            }
            string datetype = Request["datetype"]==null ?"0": Request["datetype"];
            string datefrom = Request["datefrom"];
            string dateto = Request["dateto"];
            
            string category ;
            if (Request["ddlOrderType"] != null)
            {
                category = Request["ddlOrderType"];
            }
            else if (String.IsNullOrEmpty(DispalyType))
            {
                category = "A,B,C,D";
            }
            else 
            {
                category = "A";
            }

            string smallClass = Request["ddlSmallClass"];
            string productLine = Request["ddlProductLine"];

            if (String.IsNullOrEmpty(DispalyType))
            {
               // ccWhere.AddComponent("OutFlag", "N", SearchComponent.Equals, SearchPad.And);
            }

            if (!String.IsNullOrEmpty(datetype))
            {
                ccWhere.Clear();
                if (datetype == "0")
                {
                    if (Request["submitflg"] == null)
                    {
                        datefrom = DateTime.Now.ToString("yyyy/MM/") + "01";
                    }
                    ccWhere.AddComponent("indate", ConvertShortDate(datefrom), SearchComponent.GreaterOrEquals, SearchPad.And);
                    ccWhere.AddComponent("indate", ConvertShortDate(dateto, 1), SearchComponent.Less, SearchPad.And);
                }
                else if (datetype == "1")
                {
                    // ccWhere.AddComponent("OutFlag", "Y", SearchComponent.Equals, SearchPad.And);
                    ccWhere.AddComponent("outdate", ConvertShortDate(datefrom), SearchComponent.GreaterOrEquals, SearchPad.And);
                    ccWhere.AddComponent("outdate", ConvertShortDate(dateto, 1), SearchComponent.Less, SearchPad.And);
                }
                else if (datetype == "2")
                {
                    // ccWhere.AddComponent("OutFlag", "Y", SearchComponent.Equals, SearchPad.And);
                    ccWhere.AddComponent("preoutdate", ConvertShortDate(datefrom), SearchComponent.GreaterOrEquals, SearchPad.And);
                    ccWhere.AddComponent("preoutdate", ConvertShortDate(dateto, 1), SearchComponent.Less, SearchPad.And);
                }
                else if (datetype == "3")
                {
                    if (dateto == "")
                    {
                        dateto = "2048-12-31";
                    }
                    // ccWhere.AddComponent("OutFlag", "Y", SearchComponent.Equals, SearchPad.And);
                    ccWhere.sbComponent.AppendFormat(" {0}  ",
                        "exists (select 1 from OrdersTry where [orders].Order_ID = OrdersTry.Order_ID and orders.serial = OrdersTry.serial and OrdersTry.TryGo >= '"+ ConvertShortDate(datefrom)+ "' and TryGo < '"+ ConvertShortDate(dateto, 1) + "')", "", "");
                
                }
            }
           

            if (!string.IsNullOrEmpty(category))
            {
                if (!category.Equals("-1"))
                {
                    ccWhere.AddComponent("orderclass", "('"+category.Replace(",","','")+"')", SearchComponent.In, SearchPad.And);
                }
            }

            if (!string.IsNullOrEmpty(smallClass))
            {
                if (!smallClass.Equals("-1"))
                {
                    if ((bool)Session["IsGMP"] == true)
                    {
                        smallClass = ((List<ClassSet>)Session["ListClassSet"]).Where(item => item.SmallClass == smallClass).First().ClassID;
                    }
                                      
                    ccWhere.AddComponent("products.smallClass", smallClass, SearchComponent.Equals, SearchPad.And);
                   
                }
                
            }
            

            return ccWhere;
        }

        protected string ConvertShortDate(string paraDate, int adddays = 0)
        {
            if (paraDate == null || paraDate.Trim() == "")
            {
                return "";
            }
            if (adddays != 0)
            {
                paraDate = DateTime.Parse(paraDate).AddDays(adddays).ToString("yyyy/MM/dd");
            }
            // return " CONVERT(varchar(8),cast('" + paraDate + "' as datetime),11)";
            return paraDate.Substring(2, 8).Replace("-", "/");
        }        
    }
}
