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

public partial class ReportStatistics_DailyReport : PageBase
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
            BindDailyReport(true);
        }

        if (!String.IsNullOrEmpty(Request["refresh"]))
        {
            BindDailyReport();
        }    
    }

    private void BindDailyReport(bool initFlg = false)
    {
       
        this.FinanceReportViewer.LocalReport.DataSources.Clear();
        if (initFlg)
        {
            this.FinanceReportViewer.Visible = false;
        }
        else
        {
            this.FinanceReportViewer.Visible = true;
            ccWhere.AddComponent("indate", Request["txtdatefrom"], SearchComponent.GreaterOrEquals, SearchPad.And);
            ccWhere.AddComponent("indate", Request["txtdateto"], SearchComponent.LessOrEquals, SearchPad.And);

            GetFilterByKind(ref ccWhere, "Report");

            if (ccWhere.sbComponent.ToString() == "")
                ccWhere.AddComponent("1", "1", SearchComponent.Equals, SearchPad.NULL);

            DataTable dtModelProcessing = facservComm.ExecuteSqlDatatable(ModelProcessingSummary +" and " + ccWhere.sbComponent);


            this.FinanceReportViewer.LocalReport.SetParameters(new ReportParameter("paraIndate", "2015年01月01日"));
            this.FinanceReportViewer.LocalReport.SetParameters(new ReportParameter("paraOutDate", DateTime.Now.Year.ToString() + "年" + DateTime.Now.Month.ToString() + "月" + DateTime.Now.Day + "日"));
            this.FinanceReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dtModelProcessing));
        }

    }
}