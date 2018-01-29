using D2012.Common;
using D2012.Common.DbCommon;
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
    public partial class SellerSummary : PageBase
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
        protected int widthgrid = 500;
        protected DataTable dtSource = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ((HtmlContainerControl)Master.FindControl("HTitle")).InnerText = IsCN ? "来模加工统计表" : "Seller Summary";
            }
           
            servCommfac = new ServiceCommon(base.factoryConnectionString);
            ccWhere.Clear();
            ccWhere = querycontrolSM.GetCondtion();
            string condition = ccWhere.sbComponent.ToString() == "" ? "" : " and " + ccWhere.sbComponent.ToString();
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
                
            }

            string viewsql = "select orders.seller,products.itemname as itemname ,'' as amount,'' as ordernumber,sum(OrdersDetail.Qty) as Qty,'1' as Flg from orders inner join ordersdetail on "
+ "orders.Order_ID = ordersdetail.Order_ID and orders.serial = ordersdetail.serial inner join products on products.id = ordersdetail.productid inner join classSet on classSet.ClassID = products.smallclass where (ordersdetail.Amount is not null or ordersdetail.nobleAmount is not null) " + condition
+ " group by orders.seller,products.itemname union all select orders.seller,'',sum(isnull(ordersdetail.Amount, 0) + isnull(ordersdetail.nobleAmount, 0)) as amount,count(distinct orders.Order_ID) as ordernumber,'' as Qty,'2' as Flg "
+ " from orders inner join ordersdetail on orders.Order_ID = ordersdetail.Order_ID and orders.serial = ordersdetail.serial inner join products on products.id = ordersdetail.productid inner join classSet on classSet.ClassID = products.smallclass where (ordersdetail.Amount is not null or ordersdetail.nobleAmount is not null) " + condition
+ " group by orders.seller union all select '合计数量',products.itemname as itemname,sum(isnull(ordersdetail.Amount, 0) + isnull(ordersdetail.nobleAmount, 0)) as amount,'' as ordernumber,sum(OrdersDetail.Qty) as Qty,'3' as Flg from orders inner join ordersdetail on orders.Order_ID = ordersdetail.Order_ID and orders.serial = ordersdetail.serial inner join products on products.id = ordersdetail.productid inner join classSet on classSet.ClassID = products.smallclass "
+ " where (ordersdetail.Amount is not null or ordersdetail.nobleAmount is not null) "+ condition + " group by products.itemname union all select '合计金额','' as itemname,sum(isnull(ordersdetail.Amount, 0) + isnull(ordersdetail.nobleAmount, 0)) as amount,count(distinct orders.Order_ID) as ordernumber,'' as Qty,'4' as Flg "
+ " from orders inner join ordersdetail on orders.Order_ID = ordersdetail.Order_ID and orders.serial = ordersdetail.serial inner join products on products.id = ordersdetail.productid inner join classSet on classSet.ClassID = products.smallclass where (ordersdetail.Amount is not null or ordersdetail.nobleAmount is not null) " + condition + " order by Flg,orders.seller ";

  
           dtAllFinaceSummary = servCommfac.ExecuteSqlDatatable(viewsql);            

            DataView dataView = dtAllFinaceSummary.DefaultView;
            dataView.Sort = "itemname";
            dataView.RowFilter = "itemname <> ''";
            DataTable dataTableDistinct = dataView.ToTable(true, "itemname");

            widthgrid = 240 + (dataTableDistinct.Rows.Count + 2) * 40;

            dtSource.Columns.Add("序号");
            dtSource.Columns.Add("营销员");
            dtSource.Columns.Add("应收金额");
            dtSource.Columns.Add("订单数量");
            for (int i = 0; i < dataTableDistinct.Rows.Count; i++)
            {
                if (dataTableDistinct.Rows[i][0].ToString() != "")
                {
                    dtSource.Columns.Add(dataTableDistinct.Rows[i][0].ToString());
                }                
            }
            int rowAdd = dataTableDistinct.Rows.Count % 3;
            if (rowAdd > 0)
            {
                for (int i = 0; i < (3- rowAdd); i++)
                {
                    dataTableDistinct.Rows.Add(dataTableDistinct.NewRow());
                }
            }
            
            this.querycontrolSM.dtSource = dataTableDistinct;
            DataRow dr = null;
            DataRow dr2 = null;
            string seller = "";
            string flg = "";
            int rowIndex = 0;
            foreach (DataRow item in dtAllFinaceSummary.Rows)
            {
                if (item["Flg"].ToString() == "1")
                {
                    if (seller != item["Seller"].ToString())
                    {
                        flg = "1";
                        seller = item["Seller"].ToString();
                        dtSource.Rows.Add(dtSource.NewRow());
                       
                        dr = dtSource.Rows[rowIndex];
                        dr["营销员"] = item["seller"];
                        rowIndex = rowIndex + 1;
                        dr["序号"] = rowIndex;
                    }
                    dr[item["itemname"].ToString()] = item["Qty"];
                }
                if (item["Flg"].ToString() == "2")
                {
                    if (flg == "1")
                        rowIndex = 0;
                    flg = "2";
                    dr = dtSource.Rows[rowIndex];
                    dr["应收金额"] = decimal.Parse(item["amount"].ToString()).ToString("N0");
                    dr["订单数量"] = item["ordernumber"];
                    rowIndex = rowIndex + 1;
                }
                if (item["Flg"].ToString() == "3" )
                {
                    if (seller != item["Seller"].ToString())
                    {
                        
                        seller = item["Seller"].ToString();
                        dtSource.Rows.Add(dtSource.NewRow());

                        dr = dtSource.Rows[rowIndex];
                        rowIndex = rowIndex + 1;
                        dtSource.Rows.Add(dtSource.NewRow());
                        dr2 = dtSource.Rows[rowIndex];
                        dr["营销员"] = "合计数量";
                        dr2["营销员"] = "合计金额";

                    }
                    dr[item["itemname"].ToString()] = item["Qty"];
                    dr2[item["itemname"].ToString()] = decimal.Parse(item["amount"].ToString()).ToString("N0");
                }

                if ( item["Flg"].ToString() == "4" && rowIndex>0)
                {

                    dtSource.Rows[rowIndex-1]["订单数量"] = item["ordernumber"];
                    dtSource.Rows[rowIndex]["应收金额"] = decimal.Parse(item["amount"].ToString()).ToString("N0"); ;
                }

            }

            //sumAmount = dtAllFinaceSummary.Compute("sum(amount)", "").ToString();
           
            //int rowcount = dtAllFinaceSummary.Rows.Count;
            //this.pagecutID.iPageNum = (rowcount - 1) / 10 + 1;
            //if (dtAllFinaceSummary.Rows.Count > 0)
            //{
            //    dtFinaceSummary = dtAllFinaceSummary.Select(" rowIndex>=" + ((pagecutID.iPageIndex - 1) * 10 + 1).ToString() + " and rowIndex <=" + pagecutID.iPageIndex * 10).CopyToDataTable();      
            //}
            //else
            //{
            //    dtFinaceSummary = dtAllFinaceSummary.Clone();
            //}
        }
    }
}