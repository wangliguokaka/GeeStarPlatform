using D2012.Domain.Entities;
using D2012.Domain.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Weixin.App_Master
{
    
    public partial class all_master : System.Web.UI.MasterPage
    {
        ServiceCommon servCommfac = new ServiceCommon();
        protected string allMenuBM = "";
        protected string nextMenuBM = "";
        protected DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (Session["objUser"] != null)
            {
                WUSERS loginUser = (WUSERS)(Session["objUser"]);
                if (loginUser.BelongFactory != null)
                {
                    DataTable dtSetCheck = servCommfac.ExecuteSqlDatatable("select * from W_Weixin_Info where [UserName] like '"+loginUser.BelongFactory+"%'");
                    if (dtSetCheck.Rows.Count > 0)
                    {
                        DataTable dtMenu = servCommfac.ExecuteSqlDatatable("SELECT a.[MenuBM],b.ParentBM,b.MenuLevel FROM [W_Weixin_Info] a inner join W_WeixinMenu b on a.menubm = b.MenuBM where a.[UserName] = '"+loginUser.UserName+"' and a.status = 'E'");
                        if (dtMenu.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtMenu.Rows)
                            {
                                allMenuBM = allMenuBM + "," + dr["MenuBM"].ToString() + "," + dr["ParentBM"].ToString();
                                if (dr["MenuLevel"].ToString() == "2")
                                {
                                    nextMenuBM = nextMenuBM + "," + dr["MenuBM"].ToString();
                                }
                            }
                            dt = servCommfac.ExecuteSqlDatatable("exec GetWXMenu '" + nextMenuBM + "'");
                        }
                        else
                        {
                            dt = servCommfac.ExecuteSqlDatatable("exec GetWXMenu 'ALL' ");
                        }                  
                            
                    }
                    else
                    {
                        dt = servCommfac.ExecuteSqlDatatable("exec GetWXMenu 'ALL' ");
                    }
                }

                Session["allMenuBM"] = allMenuBM;
            }
               
        }

        public bool IsCN
        {
            get
            {
                return Session["Language"] == "zh-cn";
            }
        }

        protected string ControlByMenuBM(string menuBM, string menuLevel,string url)
        {
            if (menuLevel == "1")
            {
                if ((allMenuBM + ",").Contains("," + menuBM + ","))
                {
                    return "data-href=" + (url == "None" ? "" : url);
                   
                }
                else
                {
                    return "";
                }
               
            }
            else if (menuLevel == "2")
            {
                if ((allMenuBM + ",").Contains("," + menuBM + ","))
                {
                    return "";
                }
                else
                {
                    return "style=\"display:none;\"";
                }
               
            }
            else
            {
                return "style=\"display:none;\"";
            } 
        }
    }
}