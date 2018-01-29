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

public partial class ReportStatistics_QSProcRec : PageBase
{
    ConditionComponent ccWhere = new ConditionComponent();
    ServiceCommon facservComm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsCN == false)
        {
            FinanceReportViewer.LocalReport.ReportPath = FinanceReportViewer.LocalReport.ReportPath.Replace("En", "").Replace(".rdlc", "En.rdlc");
        }
        if (!IsPostBack)
        {
            string Order_ID = Request["Order_ID"];
            string serial = Request["serial"];
            facservComm = new ServiceCommon(base.factoryConnectionString);
            ccWhere.Clear();
            ccWhere.AddComponent("Order_ID", Order_ID, SearchComponent.Equals, SearchPad.NULL);
            ccWhere.AddComponent("serial", serial, SearchComponent.Equals, SearchPad.And);

            //DataTable dtVWDisinRec = facservComm.GetListTop(0, "* ", "VWDisinRec", ccWhere);
            DataTable dtVWDisinRec = facservComm.ExecuteSqlDatatable(VWDisinRec + " where " + ccWhere.sbComponent);


            //this.FinanceReportViewer.LocalReport.SetParameters(new ReportParameter("paraIndate", "2015年01月01日"));
            //this.FinanceReportViewer.LocalReport.SetParameters(new ReportParameter("paraOutDate", DateTime.Now.Year.ToString() + "年" + DateTime.Now.Month.ToString() + "月" + DateTime.Now.Day + "日"));
            this.FinanceReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dtVWDisinRec));
        }
    }
}