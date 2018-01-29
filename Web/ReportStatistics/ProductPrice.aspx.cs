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

public partial class ReportStatistics_ProductPrice : PageBase
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
        if (!IsPostBack)
        {
            if (ccWhere.sbComponent.ToString() == "")
                ccWhere.AddComponent("1", "1", SearchComponent.Equals, SearchPad.NULL);
            DataTable dtSeller = facservComm.ExecuteSqlDatatable("select distinct sellerid,Seller from " + vieworganizationorigin + " where " + ccWhere.sbComponent);
            this.seller.DataTextField = "Seller";
            this.seller.DataValueField = "sellerid";
            this.seller.DataSource = dtSeller;
            this.seller.DataBind();
            if (GetOrganization.Count > 0)
            {
                this.seller.SelectedValue = GetOrganization["sellerid"].ToString();
                this.seller.Enabled = false;
            }
            DataTable dtPrice = new ReportDataSet.ProductPriceDataTable();
            this.FinanceReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dtPrice));
            //BindProductPrice();
        }
        if (!String.IsNullOrEmpty(Request["refresh"]))
        {
            BindProductPrice();
        }
    }

    private void BindProductPrice()
    {
        this.FinanceReportViewer.LocalReport.DataSources.Clear();
        string HospitalList = Request["HospitalList"];
        string hospitalName = Request["HidHospitalName"];
        string BalanceClass = "";
        DataTable dt = facservComm.ExecuteSqlDatatable("SELECT BalanceClass FROM [Hospital] where id =" + HospitalList.Trim(':'));

        if (dt.Rows.Count > 0)
        {
            BalanceClass = dt.Rows[0][0].ToString();
        }
        DataTable dtPrice;
       
        if (BalanceClass == "产品模板")
        {
            dtPrice = facservComm.ExecuteSqlDatatable(VWPriceByTemplate + " and hospitalid = " + HospitalList.Trim(':') + " order by Sortno,id ");
        }
        else
        {
            facservComm.strOrderString = " Sortno,id ";
            ccWhere.Clear();
            ccWhere.AddComponent("Serial", BalanceClass.Substring(0, 1), SearchComponent.Equals, SearchPad.NULL);
           // dtPrice = facservComm.GetListTop(0, "'"+hospitalName+"' as hospital ,* ", "VWPrice", ccWhere);
            dtPrice = facservComm.ExecuteSqlDatatable(VWPrice + " and " + ccWhere.sbComponent);
        }
        
        this.FinanceReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dtPrice));
    }
}