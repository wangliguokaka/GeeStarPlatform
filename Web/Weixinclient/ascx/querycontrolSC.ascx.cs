using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using D2012.Common;
using D2012.Common.DbCommon;
using D2012.Domain.Services;

namespace Weixin.RGPWEB.ascx
{
   
    public partial class querycontrolSC : System.Web.UI.UserControl
    {

        public string userHospitalID = "";
        public string userDoctorID = "";
        public string userSellerID = "";
        ConditionComponent ccWhere = new ConditionComponent();
        protected DataTable dtCategory = null;
        public string datefrom;
        public string dateto;
        ServiceCommon facComm = null;
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

        public string Permission()
        {
            string permissionCondion = "";
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

            if (!string.IsNullOrEmpty(userSellerID))
            {

                permissionCondion = permissionCondion + " and sellerid='"+ userSellerID +"'";
                
            }

            if (!string.IsNullOrEmpty(userHospitalID))
            {

                permissionCondion = permissionCondion + " and Hospitalid='" + userHospitalID + "'";

            }
           
            if (!string.IsNullOrEmpty(userDoctorID))
            {
                permissionCondion = permissionCondion + " and doctor in (select top 1 doctor from doctor where id = " + userDoctorID + ")";
            }
            return permissionCondion;
        }


        public ConditionComponent GetCondtion()
        {
            ccWhere.Clear();
           

            if (!String.IsNullOrEmpty(Request["datefrom"]))
            {
                datefrom = Request["datefrom"].Replace("-", "");
            }
            else
            {
                datefrom = "1953-01-01";
            }

            if (!String.IsNullOrEmpty(Request["dateto"]))
            {
                dateto = Request["dateto"].Replace("-","");
            }
            else
            {
                dateto = "2050-12-31";
            }

            if (Request["submitflg"] == null)
            {
                datefrom = DateTime.Now.ToString("yyyy-MM-dd");
            }
            

            return ccWhere;
        }

        protected string ConvertShortDate(string paraDate, int adddays = 0)
        {
            if (paraDate == null || paraDate.Trim() == "" || paraDate.Trim() == "''")
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
