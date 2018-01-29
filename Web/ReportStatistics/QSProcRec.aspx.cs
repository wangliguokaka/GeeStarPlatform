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

public partial class ReportStatistics_DisinRec : PageBase
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
            ccWhere.AddComponent("QSProcRec.Order_ID", Order_ID, SearchComponent.Equals, SearchPad.NULL);
            ccWhere.AddComponent("QSProcRec.serial", serial, SearchComponent.Equals, SearchPad.And);
            
            //DataTable dtOrdersElement = facservComm.GetListTop(0, "* ", "VWQSProcRec", ccWhere);

            //DataTable dtOrdersElementBaseInfo = facservComm.GetListTop(1, "* ", "VWQSProcRec", ccWhere);

            DataTable dtOrdersElement = facservComm.ExecuteSqlDatatable(VWQSProcRec + " and " + ccWhere.sbComponent);

            ccWhere.Clear();
            ccWhere.AddComponent("Order_ID", Order_ID, SearchComponent.Equals, SearchPad.NULL);
            ccWhere.AddComponent("serial", serial, SearchComponent.Equals, SearchPad.And);

            DataTable dtOrdersElementBaseInfo = facservComm.ExecuteSqlDatatable("select top 1 * from (" + VWQSProcRec +" ) t" + " where " + ccWhere.sbComponent);
            

            //this.FinanceReportViewer.LocalReport.SetParameters(new ReportParameter("paraIndate", "2015年01月01日"));
            //this.FinanceReportViewer.LocalReport.SetParameters(new ReportParameter("paraOutDate", DateTime.Now.Year.ToString() + "年" + DateTime.Now.Month.ToString() + "月" + DateTime.Now.Day + "日"));
            this.FinanceReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dtOrdersElement));
            this.FinanceReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", dtOrdersElementBaseInfo));
        }
    }
}