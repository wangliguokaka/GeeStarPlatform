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

public partial class ReportStatistics_Detail : PageBase
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
            //GetFilterByKind(ref ccWhere);
            //DataTable dtOrder = facservComm.GetListTop(0, "* ", "VWORDERS", ccWhere);
            DataTable dtOrder = facservComm.ExecuteSqlDatatable(VWORDERS + " where " + ccWhere.sbComponent);

           // DataTable dtOrderDetail = facservComm.GetListTop(0, "* ", "VWOrdersDetail", ccWhere);
            DataTable dtOrderDetail = facservComm.ExecuteSqlDatatable(VWOrdersDetail + " where " + ccWhere.sbComponent);

            DataTable dtOrderOther = facservComm.GetListTop(0, "* ", "ordersOther", ccWhere);
            int count = dtOrderOther.Rows.Count;
            DataTable dtConstruction = new ReportDataSet.OrderOthersDataTable();
            for (int i = 1; i <= count / 3; i++)
            {
                dtConstruction.Rows.Add(dtConstruction.NewRow());
                dtConstruction.Rows[i - 1]["name1"] = dtOrderOther.Rows[(i - 1) * 3]["name"];
                dtConstruction.Rows[i - 1]["qty1"] = dtOrderOther.Rows[(i - 1) * 3]["qty"];
                dtConstruction.Rows[i - 1]["name2"] = dtOrderOther.Rows[i * 3 - 1]["name"];
                dtConstruction.Rows[i - 1]["qty2"] = dtOrderOther.Rows[i * 3 - 1]["qty"];
                dtConstruction.Rows[i - 1]["name3"] = dtOrderOther.Rows[i * 3 - 2]["name"];
                dtConstruction.Rows[i - 1]["qty3"] = dtOrderOther.Rows[i * 3 - 2]["qty"];
            }

            if (count % 3 == 2)
            {
                dtConstruction.Rows.Add(dtConstruction.NewRow());
                dtConstruction.Rows[count / 3]["name1"] = dtOrderOther.Rows[count - 2]["name"];
                dtConstruction.Rows[count / 3]["qty1"] = dtOrderOther.Rows[count - 2]["qty"];
                dtConstruction.Rows[count / 3]["name2"] = dtOrderOther.Rows[count - 1]["name"];
                dtConstruction.Rows[count / 3]["qty2"] = dtOrderOther.Rows[count - 1]["qty"];
                dtConstruction.Rows[count / 3]["name3"] = "";
                dtConstruction.Rows[count / 3]["qty3"] = "";
            }

            if (count % 3 == 1)
            {
                dtConstruction.Rows.Add(dtConstruction.NewRow());
                dtConstruction.Rows[count / 3]["name1"] = dtOrderOther.Rows[count - 1]["name"];
                dtConstruction.Rows[count / 3]["qty1"] = dtOrderOther.Rows[count - 1]["qty"];
                dtConstruction.Rows[count / 3]["name2"] = "";
                dtConstruction.Rows[count / 3]["qty2"] = "";
                dtConstruction.Rows[count / 3]["name3"] = "";
                dtConstruction.Rows[count / 3]["qty3"] = "";
            }

           


            //this.FinanceReportViewer.LocalReport.SetParameters(new ReportParameter("paraIndate", "2015年01月01日"));
            //this.FinanceReportViewer.LocalReport.SetParameters(new ReportParameter("paraOutDate", DateTime.Now.Year.ToString() + "年" + DateTime.Now.Month.ToString() + "月" + DateTime.Now.Day + "日"));
            this.FinanceReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dtOrder));
            this.FinanceReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", dtOrderDetail));
            this.FinanceReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet3", dtConstruction));
        }
    }
}