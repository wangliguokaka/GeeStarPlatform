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

public partial class ReportStatistics_ProductCheckReport : PageBase
{
    ConditionComponent ccWhere = new ConditionComponent();
    ServiceCommon facservComm;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            BindFinanceView();
            BindItemClass();
        }
    }

    private void BindItemClass()
    {
        ItemClass.Items.Add(new ListItem(GetGlobalResourceObject("SystemResource", "SolidClass").ToString(), "B"));
        ItemClass.Items.Add(new ListItem(GetGlobalResourceObject("SystemResource", "ActiveClass").ToString(), "A"));
    }

    private void BindFinanceView()
    {
        this.FinanceReportViewer.LocalReport.DataSources.Clear();

        string Order_ID = Request["Order_ID"];
        string serial = Request["serial"];
        facservComm = new ServiceCommon(base.factoryConnectionString);
        ccWhere.Clear();
        ccWhere.AddComponent("Order_ID", Order_ID, SearchComponent.Equals, SearchPad.NULL);
        ccWhere.AddComponent("serial", serial, SearchComponent.Equals, SearchPad.And);
       
        facservComm.strOrderString = "cast(qsserial as int)";

        ccWhere.AddComponent("class", this.ItemClass.SelectedValue, SearchComponent.Equals, SearchPad.And);
        //DataTable dtProductCheckReport = facservComm.GetListTop(0, "* ", "VWProductCheckReport", ccWhere);
        
        DataTable dtProductCheckReport = facservComm.ExecuteSqlDatatable(VWProductCheckReport + " where " + ccWhere.sbComponent);
        //this.FinanceReportViewer.LocalReport.SetParameters(new ReportParameter("paraIndate", "2015年01月01日"));
        //this.FinanceReportViewer.LocalReport.SetParameters(new ReportParameter("paraOutDate", DateTime.Now.Year.ToString() + "年" + DateTime.Now.Month.ToString() + "月" + DateTime.Now.Day + "日"));
        this.FinanceReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dtProductCheckReport));
    }
    protected void ItemClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindFinanceView();
    }
}