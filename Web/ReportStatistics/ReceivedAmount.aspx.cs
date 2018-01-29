using D2012.Common.DbCommon;
using D2012.Domain.Services;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ReportStatistics_ReceivedAmount : PageBase
{
    ConditionComponent ccWhere = new ConditionComponent();
    ServiceCommon facservComm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsCN == false)
        {
            FinanceReportViewer.LocalReport.ReportPath = FinanceReportViewer.LocalReport.ReportPath.Replace("En", "").Replace(".rdlc", "En.rdlc");
        }
        facservComm = new ServiceCommon(base.factoryConnectionString);
        BindSeller();
        if (!IsPostBack)
        {
            
            if (GetOrganization.Count > 0)
            {
                this.seller.SelectedValue = GetOrganization["sellerid"].ToString();
                this.seller.Enabled = false;
            }

            BindRecivedAmount(true);
        }

        if (!String.IsNullOrEmpty(Request["refresh"]))
        {
            BindRecivedAmount();
        }
    }

    private void BindSeller()
    {
        if (ccWhere.sbComponent.ToString() == "")
            ccWhere.AddComponent("1", "1", SearchComponent.Equals, SearchPad.NULL);
        DataTable dtSeller = facservComm.ExecuteSqlDatatable("select distinct sellerid,Seller from " + vieworganizationorigin + " where " + ccWhere.sbComponent);
        this.seller.DataTextField = "Seller";
        this.seller.DataValueField = "sellerid";
        this.seller.DataSource = dtSeller;
        this.seller.DataBind();
        if (IsCN)
        {
            this.seller.Items.Insert(0, new ListItem("请选择", ""));
        }
        else
        {
            this.seller.Items.Insert(0, new ListItem("Select", ""));
        }
    }

    private void BindRecivedAmount(bool initFlg = false)
    {
        this.FinanceReportViewer.LocalReport.DataSources.Clear();
        if (initFlg)
        {
            FinanceReportViewer.Visible = false;
        }
        else {
            FinanceReportViewer.Visible = true;
            string HospitalList = Request["HospitalList"];
            if (!String.IsNullOrEmpty(this.seller.SelectedValue))
            {
                ccWhere.AddComponent("sellerid", this.seller.SelectedValue, SearchComponent.Equals, SearchPad.NULL);
            }

            if (!String.IsNullOrEmpty(HospitalList))
            {
                ccWhere.AddComponent("hospitalid", "(" + HospitalList.Replace(":", ",") + ")", SearchComponent.In, SearchPad.And);
            }

            string Order_ID = Request["Order_ID"];
            string serial = Request["serial"];
   
            //ccWhere.Clear();
            //ccWhere.AddComponent("Order_ID", Order_ID, SearchComponent.Equals, SearchPad.NULL);
            //ccWhere.AddComponent("serial", serial, SearchComponent.Equals, SearchPad.And);
            GetFilterByKind(ref ccWhere, "Report");
           // DataTable dtReceivedAmoun = facservComm.ExecuteSqlDatatable("exec [SPReceivedAmount] " + "'"+ccWhere.sbComponent.ToString().Replace("'","''")+"'");

            string spreceivesql = "select orders.*,gather.Favorable,gather.GatherTotal,ltrim(rtrim(gather.remark)) as remark,gather.BalanceDate from (select sellerid, seller, hospitalID, hospital, Order_ID, serial, ReceiveYearMonth, sum(RecivedAmount) as RecivedAmount from("
       + " select a.sellerid, a.seller,case when b.settlement = 'A' then convert(nvarchar(7), indate, 120) else cast(outdate as nvarchar(7)) end ReceiveYearMonth,a.order_ID, a.serial, a.hospitalID, a.hospital, b.settlement, isnull(c.amount, 0) + isnull(c.nobleAmount, 0) as RecivedAmount from(select * from orders where " + ccWhere.sbComponent + " )  a "
      + " left join Hospital b on a.hospitalID = b.id left join ordersdetail c on a.Order_ID = c.Order_ID and a.serial = c.serial where case when b.settlement = 'A' then indate else outdate end is not null ) t group by sellerid, seller, hospitalID, hospital, Order_ID, serial, ReceiveYearMonth) orders left join gather on orders.hospitalID = gather.hospitalID and orders.Order_ID = gather.Order_ID and orders.serial = gather.serial";
            DataTable dtReceivedAmoun = facservComm.ExecuteSqlDatatable(spreceivesql);

            //this.FinanceReportViewer.LocalReport.SetParameters(new ReportParameter("paraIndate", "2015年01月01日"));
            //this.FinanceReportViewer.LocalReport.SetParameters(new ReportParameter("paraOutDate", DateTime.Now.Year.ToString() + "年" + DateTime.Now.Month.ToString() + "月" + DateTime.Now.Day + "日"));
            this.FinanceReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dtReceivedAmoun));
        }
    }
}