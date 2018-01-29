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
    public partial class FinaceSummary : PageBase
    {
        protected DataTable dtFinaceSummary = new DataTable();
        private DataTable dtAllFinaceSummary = new DataTable();
        ConditionComponent ccWhere = new ConditionComponent();
        protected int tcount;
        protected string hddpnumbers;
        ServiceCommon servCommfac;
        protected string sumQty = "";
        protected string sumAmount = "";
        protected string sumNobleWeight = "";
        protected string sumNobleAmount = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ((HtmlContainerControl)Master.FindControl("HTitle")).InnerText = IsCN ? "结算单汇总表" : "Finace Summary";
                
            }
            querycontrol.DispalyType = "1";
            servCommfac = new ServiceCommon(base.factoryConnectionString);
            ccWhere.Clear();
            ccWhere = querycontrol.GetCondtion();
            servCommfac.strOrderString = querycontrol.OrderString == "" ? " regtime desc " : querycontrol.OrderString;

            hddpnumbers = yeyRequest.Params("hpnumbers");
            int iCount = 5;
            if (!string.IsNullOrEmpty(hddpnumbers))
            {
                iCount = Convert.ToInt32(hddpnumbers);
            }

            int iPageIndex = string.IsNullOrEmpty(Request["sPageID"]) ? 1 : Convert.ToInt32(Request["sPageID"]);
            int iPageCount = string.IsNullOrEmpty(Request["sPageNum"]) ? 0 : Convert.ToInt32(Request["sPageNum"]);


            if (Request["submitflg"] != null && Request["submitflg"] == "1")
            {
                iPageIndex = 1;
                this.pagecutID.iPageIndex = 1;
            }

            string condition = ccWhere.sbComponent.ToString() == "" ? "" : " and " + ccWhere.sbComponent.ToString();

            if (GetOrganization.Count >= 2)
            {

                string userHospitalID = GetOrganization["hospitalid"].ToString();

                DataTable dt = servCommfac.ExecuteSqlDatatable("SELECT PrntPrices FROM [Hospital] where id =" + userHospitalID);
                string PrntPrices = "";
                if (dt.Rows.Count > 0)
                {
                    string serial = "";
                    PrntPrices = dt.Rows[0][0].ToString().Trim();
                    if (PrntPrices == "产品模板")
                    {
                        serial = PrntPrices;
                    }
                    else
                    {
                      
                        if (PrntPrices != null && !"".Equals(PrntPrices))
                        {
                            for (int i = 0; i < PrntPrices.ToCharArray().Length; i++)
                            {
                                if (PrntPrices.ToCharArray()[i] >= 48 && PrntPrices.ToCharArray()[i] <= 57)
                                {
                                    serial = serial + PrntPrices.ToCharArray()[i];
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    if (Utils.IsNoSP)
                    {
                        if (serial == "")
                        {
                            string sql = "SELECT row_number()over(order by products.id) as rowIndex, products.itemname,SUM(ordersdetail.Qty) as Qty ,null as price,null as amount,SUM(ordersdetail.NobleWeight) as NobleWeight,SUM(ordersdetail.nobleAmount) as nobleAmount FROM orders, OrdersDetail, products WHERE(OrdersDetail.Order_ID = orders.Order_ID) and (OrdersDetail.serial = orders.serial) and "
                             + " (OrdersDetail.ProductId = products.id) and products.itemname is not null  "+ condition + " group by products.id,products.itemname";
                            dtAllFinaceSummary = servCommfac.ExecuteSqlDatatable(sql);

                        }
                        else
                        {
                            string sql = "SELECT row_number()over(order by products.id) as rowIndex, products.itemname,SUM(ordersdetail.Qty) as Qty ,sum(products.price) as price,SUM(ordersdetail.Qty * prices.price) as amount,SUM(ordersdetail.NobleWeight) as NobleWeight,SUM(ordersdetail.nobleAmount) as nobleAmount "
        + " FROM orders, OrdersDetail, products, prices WHERE(OrdersDetail.Order_ID = orders.Order_ID) and (OrdersDetail.serial = orders.serial) and (OrdersDetail.ProductId = products.id) and products.id = prices.productid and prices.serial = '" + serial + "' and products.itemname is not null " + condition
         + " group by products.id,products.itemname ";
                            dtAllFinaceSummary = servCommfac.ExecuteSqlDatatable(sql);
                        }

                    }
                    else
                    {
                        dtAllFinaceSummary = servCommfac.ExecuteSqlDatatable(" exec [SPWXFinaceSummaryByHospital] '" + condition.Replace("'", "''") + "' , N'" + serial + "'");

                    }
                }
            }
            else
            {
                if (Utils.IsNoSP)
                {
                    string sql = "SELECT row_number()over(order by products.id) as rowIndex, products.itemname,SUM(ordersdetail.Qty) as Qty ,SUM(ordersdetail.price) as price,SUM(ordersdetail.amount) as amount,SUM(ordersdetail.NobleWeight) as NobleWeight,SUM(ordersdetail.nobleAmount) as nobleAmount "
                    + " FROM orders, OrdersDetail, products WHERE(OrdersDetail.Order_ID = orders.Order_ID) and (OrdersDetail.serial = orders.serial) and (OrdersDetail.ProductId = products.id) and products.itemname is not null " + condition + " group by products.id,products.itemname";
                    dtAllFinaceSummary = servCommfac.ExecuteSqlDatatable(sql);
                }
                else
                {
                    dtAllFinaceSummary = servCommfac.ExecuteSqlDatatable(" exec [SPWXFinaceSummaryBySeller]  '" + condition.Replace("'", "''") + "'");
                }
                    
            }

            sumQty = dtAllFinaceSummary.Compute("sum(Qty)", "").ToString();
            sumAmount = dtAllFinaceSummary.Compute("sum(amount)", "").ToString();
            sumNobleWeight = dtAllFinaceSummary.Compute("sum(NobleWeight)", "").ToString();
            sumNobleAmount = dtAllFinaceSummary.Compute("sum(NobleAmount)", "").ToString();
            int rowcount = dtAllFinaceSummary.Rows.Count;
            this.pagecutID.iPageNum = (rowcount - 1) / 10 + 1;
            if (dtAllFinaceSummary.Rows.Count > 0)
            {
                dtFinaceSummary = dtAllFinaceSummary.Select(" rowIndex>=" + ((pagecutID.iPageIndex - 1) * 10 + 1).ToString() + " and rowIndex <=" + pagecutID.iPageIndex * 10).CopyToDataTable();
            }
            else
            {
                dtFinaceSummary = dtFinaceSummary.Clone();
            }
        }

        
    }
}