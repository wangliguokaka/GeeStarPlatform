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
    public partial class SmallClassSummary : PageBase
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
                ((HtmlContainerControl)Master.FindControl("HTitle")).InnerText = IsCN ? "来模一览表" : "SmallClassList";
               
            }
            servCommfac = new ServiceCommon(base.factoryConnectionString);
            ccWhere.Clear();
            ccWhere = querycontrolSC.GetCondtion();

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

            if (Utils.IsNoSP == false)
            {
                dtAllFinaceSummary = servCommfac.ExecuteSqlDatatable(" exec SP_SmallClassSummary '" + querycontrolSC.datefrom + "','" + querycontrolSC.dateto + "','" + querycontrolSC.Permission().Replace("'", "''") + "'");
            }
            else
            {
                string sql = "select row_number() over(order by products.smallclass ) as rowIndex, products.smallclass as [SmallClass],"
+ "cast(sum( case when orders.orderclass = 'A' then qty else null end) as nvarchar) + '/ ' + cast(cast(sum(case when orders.orderclass = 'A' then amount else 0 end) as int) as nvarchar) as[AOrderClass] ,"
+ "cast(sum( case when orders.orderclass = 'B' then qty else null end) as nvarchar) + '/ ' + cast(cast(sum(case when orders.orderclass = 'B' then amount else 0 end) as int) as nvarchar) as[BOrderClass],"
+ "cast(sum( case when orders.orderclass = 'C' then qty else null end) as nvarchar) + ' / ' + cast(cast(sum(case when orders.orderclass = 'C' then amount else 0  end) as int) as nvarchar) as[COrderClass],"
+ "cast(sum( case when orders.OutFlag = 'N' and orders.preoutDate < GETDATE() then qty else null end) as nvarchar) as [DOrderClass] "
+ "from orders inner join ordersdetail on orders.Order_ID = ordersdetail.Order_ID and orders.serial = ordersdetail.serial "
+ "inner join products on ordersdetail.productid = products.id "
+ "where orders.indate >= convert(datetime, '" + querycontrolSC.datefrom + "', 120)"
+ "and orders.indate < DATEADD(DAY, 1, convert(datetime, '" + querycontrolSC.dateto + "', 120))" + querycontrolSC.Permission().Replace("'", "'") + " group by  products.smallclass union all "
+ "select 0 as rowIndex,'订单' as [SmallClass] ,"
+ "cast(sum( case when orders.orderclass = 'A' then qty else null end) as nvarchar) + ' / ' + cast(cast(sum(case when orders.orderclass = 'A' then amount else 0 end) as int) as nvarchar) as[AOrderClass] ,"
+ "cast(sum( case when orders.orderclass = 'B' then qty else null end) as nvarchar) + ' / ' + cast(cast(sum(case when orders.orderclass = 'B' then amount else 0 end) as int) as nvarchar) as[BOrderClass],"
+ "cast(sum( case when orders.orderclass = 'C' then qty else null end) as nvarchar) + ' / ' + cast(cast(sum(case when orders.orderclass = 'C' then amount else 0  end) as int) as nvarchar) as[COrderClass],"
+ "cast(sum( case when orders.OutFlag = 'N' and orders.preoutDate < GETDATE() then qty else null end) as nvarchar) as [DOrderClass] "
+ "from orders inner join ordersdetail on orders.Order_ID = ordersdetail.Order_ID and orders.serial = ordersdetail.serial "
+ "inner join products on ordersdetail.productid = products.id "
+ "where orders.indate >= convert(datetime, '" + querycontrolSC.datefrom + "', 120) and orders.indate < DATEADD(DAY, 1, convert(datetime,'" + querycontrolSC.dateto + "', 120))" + querycontrolSC.Permission().Replace("'", "'") + " order by rowIndex,smallclass";
                dtAllFinaceSummary = servCommfac.ExecuteSqlDatatable(sql);
            }


            int rowcount = dtAllFinaceSummary.Rows.Count;
            this.pagecutID.iPageNum = (rowcount - 1) / 10 + 1;
            if (dtAllFinaceSummary.Rows.Count > 0)
            {
                dtFinaceSummary = dtAllFinaceSummary.Select(" rowIndex>=" + ((pagecutID.iPageIndex - 1) * 10 ).ToString() + " and rowIndex <=" + (pagecutID.iPageIndex * 10-1)).CopyToDataTable();
            }
            else
            {
                dtFinaceSummary = dtFinaceSummary.Clone();
            }
        }

        protected string ConvertSmallClass(string ClassID)
        {
            if (ClassID == "订单")
                return ClassID;
            if ((bool)Session["IsGMP"] == true)
            {
                return ((List<ClassSet>)Session["ListClassSet"]).Where(item => item.ClassID == ClassID).First().SmallClass;
            }
            else
            {
                return ClassID;
            }
        }
    }
}