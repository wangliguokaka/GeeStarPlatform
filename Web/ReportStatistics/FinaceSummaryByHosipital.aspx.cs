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

public partial class ReportStatistics_FinaceSummaryByHosipital : PageBase
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

            DataTable dtFinaceSummary = new ReportDataSet.FinaceSummaryDataTable();
            DataTable dtOneRow = new ReportDataSet.OneRowTableDataTable();
            this.FinanceReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dtFinaceSummary));
            this.FinanceReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", dtOneRow));

            DataTable dtCharge = facservComm.ExecuteSqlDatatable("SELECT Code,DictName FROM [DictDetail] where ClassID  = 'charges' order by sortno");
            this.charges.DataSource = dtCharge;
            this.charges.DataBind();

            orderType.DataSource = BindDictClass(facservComm,ccWhere,"OrderClass");
            orderType.DataBind();
        }

        if (!String.IsNullOrEmpty(Request["refresh"]))
        {
            BindReportView();
        }        
    }

    private void BindReportView()
    {
        this.FinanceReportViewer.LocalReport.DataSources.Clear();
        string HospitalList = Request["HospitalList"];
        string dateselect = Request["dateselect"];
        ccWhere.Clear();
        ccWhere.AddComponent("sellerid", this.seller.SelectedValue, SearchComponent.Equals, SearchPad.NULL);
        if (!String.IsNullOrEmpty(HospitalList))
        {
            ccWhere.AddComponent("hospitalid", "(" + HospitalList.Replace(":", ",") + ")", SearchComponent.In, SearchPad.And);
        }
        if (!String.IsNullOrEmpty(dateselect))
        {
            if (dateselect == "0")
            {
                ccWhere.AddComponent("indate", ConvertShortDate(Request["txtdatefrom"]), SearchComponent.GreaterOrEquals, SearchPad.And);
                ccWhere.AddComponent("indate", ConvertShortDate(Request["txtdateto"]), SearchComponent.LessOrEquals, SearchPad.And);
            }
            else if (dateselect == "1")
            {
                ccWhere.AddComponent("outdate", ConvertShortDate(Request["txtdatefrom"]), SearchComponent.GreaterOrEquals, SearchPad.And);
                ccWhere.AddComponent("outdate", ConvertShortDate(Request["txtdateto"]), SearchComponent.LessOrEquals, SearchPad.And);
            }
            else {
                ccWhere.AddComponent("preoutdate", Request["txtpredate"], SearchComponent.Equals, SearchPad.And);
            }
        }

        if (!String.IsNullOrEmpty(this.OrderNo.Text))
        {
            ccWhere.AddComponent("orders.Order_ID", this.OrderNo.Text, SearchComponent.Like, SearchPad.And);
        }

        //if (!String.IsNullOrEmpty(Request["txtpredate"]))
        //{
        //    ccWhere.AddComponent("preoutdate", Request["txtpredate"], SearchComponent.Equals, SearchPad.And);
        //}

    
        string charges = "";
        string orderClass = "";

        for (int i = 0; i < this.charges.Items.Count; i++)
        {
            if (this.charges.Items[i].Selected == true)
            {
                charges = charges+",'" + this.charges.Items[i].Value + "'";
            }
        }
        if (!String.IsNullOrEmpty(charges))
        {
            charges = "("+charges.Trim(',')+")";
            ccWhere.AddComponent("charges", charges, SearchComponent.In, SearchPad.And);
        }

        for (int i = 0; i < this.orderType.Items.Count; i++)
        {
            if (this.orderType.Items[i].Selected == true)
            {
                orderClass = orderClass + ",'" + this.orderType.Items[i].Value + "'";
            }
        }
        if (!String.IsNullOrEmpty(orderClass))
        {
            orderClass = "(" + orderClass.Trim(',') + ")";
            ccWhere.AddComponent("orderclass", orderClass, SearchComponent.In, SearchPad.And);
        }


        GetFilterByKind(ref ccWhere,"Report");
        //DataTable dtFinaceSummary = facservComm.GetListTop(0, "* ", "VWFinaceSummary", ccWhere);


        DataTable dtFinaceSummary = facservComm.ExecuteSqlDatatable(FinanceSummary + " and " + ccWhere.sbComponent);
        
        DataTable dtOneRow = new ReportDataSet.OneRowTableDataTable();
        dtOneRow.Rows.Add(dtOneRow.NewRow());
        dtOneRow.Rows[0][0] = dtFinaceSummary.Compute("min(indate)","");
        dtOneRow.Rows[0][1] = dtFinaceSummary.Compute("max(indate)", "");
        dtOneRow.Rows[0][2] = Request["HidHospitalName"];

        this.FinanceReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dtFinaceSummary));
        this.FinanceReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", dtOneRow));
        this.FinanceReportViewer.DataBind();
        this.FinanceReportViewer.LocalReport.Refresh();

        
    }
}