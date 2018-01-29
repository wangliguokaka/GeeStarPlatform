using D2012.Common.DbCommon;
using D2012.Domain.Services;
using GeeStar.Workflow.Common;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ReportStatistics_FinanceSummaryDetail : PageBase
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
            BindFinanceSummary(true);

            DataTable dtCharge = facservComm.ExecuteSqlDatatable("SELECT Code,DictName FROM [DictDetail] where ClassID  = 'charges' order by sortno");
            this.charges.DataSource = dtCharge;
            this.charges.DataBind();

            orderType.DataSource = BindDictClass(facservComm, ccWhere, "OrderClass");
            orderType.DataBind();
        }
        if (!String.IsNullOrEmpty(Request["refresh"]))
        {
            BindFinanceSummary();
        }
    }

    private void BindSeller()
    {
        if(ccWhere.sbComponent.ToString() == "" )
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

    private void BindFinanceSummary(bool initFlg = false)
    {
        this.FinanceReportViewer.LocalReport.DataSources.Clear();
        if (initFlg == true)
        {
            this.FinanceReportViewer.Visible = false;
        }
        else
        {
            this.FinanceReportViewer.Visible = true;
            string HospitalList = Request["HospitalList"];
            if (!String.IsNullOrEmpty(this.seller.SelectedValue))
            {
                ccWhere.AddComponent("sellerid", this.seller.SelectedValue, SearchComponent.Equals, SearchPad.NULL);
            }
            string dateselect = Request["dateselect"];
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
                ccWhere.AddComponent("orders.Order_ID ", this.OrderNo.Text, SearchComponent.Like, SearchPad.And);
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
                    charges = charges + ",'" + this.charges.Items[i].Value + "'";
                }
            }
            if (!String.IsNullOrEmpty(charges))
            {
                charges = "(" + charges.Trim(',') + ")";
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


            GetFilterByKind(ref ccWhere, "Report");
           // DataTable dtFinance = facservComm.GetListTop(0, "* ", "VWFinaceProductDetail", ccWhere);

            DataTable dtFinance = facservComm.ExecuteSqlDatatable(FinanceSummaryDetail + " and " + ccWhere.sbComponent);
            //DataTable dt = new ReportDataSet.FinanceSummaryTableDataTable();
            //dt.Rows.Add(dt.NewRow());
            //dt.Rows[0][0] = "P1";
            //dt.Rows[0][1] = "12345";
            //dt.Rows[0][2] = "6598";
            //dt.Rows.Add(dt.NewRow());
            //dt.Rows[1][0] = "P2";
            //dt.Rows[1][1] = "456";
            //dt.Rows[1][2] = "125";

            var query = from t in dtFinance.AsEnumerable()
                        group t by new { t1 = t.Field<string>("products_itemname") } into m
                        select new
                        {
                            productName = m.Key.t1,
                            Qty = m.Sum(t => t.Field<int>("Qty"))
                        };

            var queryList = query.ToList();
            int count = queryList.Count;
            DataTable dtProcutCount = new ReportDataSet.ProductSummaryBySellerDataTable();
            for (int i = 1; i <= count / 2; i++)
            {
                dtProcutCount.Rows.Add(dtProcutCount.NewRow());
                dtProcutCount.Rows[i - 1][0] = queryList[(i - 1) * 2].productName;
                dtProcutCount.Rows[i - 1][1] = queryList[(i - 1) * 2].Qty;
                dtProcutCount.Rows[i - 1][2] = queryList[i * 2 - 1].productName;
                dtProcutCount.Rows[i - 1][3] = queryList[i * 2 - 1].Qty;
            }
            if (count % 2 == 1)
            {
                dtProcutCount.Rows.Add(dtProcutCount.NewRow());
                dtProcutCount.Rows[count / 2][0] = queryList[count - 1].productName;
                dtProcutCount.Rows[count / 2][1] = queryList[count - 1].Qty;
                dtProcutCount.Rows[count / 2][2] = "";
                dtProcutCount.Rows[count / 2][3] = "";
            }
            DataTable dtOneRow = new ReportDataSet.OneRowTableDataTable();
            dtOneRow.Columns.Add("A");
            dtOneRow.Rows.Add(dtOneRow.NewRow());

            this.FinanceReportViewer.LocalReport.SetParameters(new ReportParameter("paraIndate", dtFinance.Compute("min(indate)", "") == null ? "" : dtFinance.Compute("min(indate)", "").ToString()));
            this.FinanceReportViewer.LocalReport.SetParameters(new ReportParameter("paraOutDate", dtFinance.Compute("max(indate)", "") == null ? "" : dtFinance.Compute("max(indate)", "").ToString()));
            this.FinanceReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dtFinance));
            this.FinanceReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", dtProcutCount));
            this.FinanceReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet3", dtOneRow));

            try
            {
                var pageSettings = this.FinanceReportViewer.GetPageSettings();
                //pageSettings.PaperSize = new PaperSize()
                //    {
                //        Height = 100,
                //        Width = 50
                //    };
                //pageSettings.Landscape = true;
                pageSettings.Margins.Left = 15;
                pageSettings.Margins.Right = 5;
                this.FinanceReportViewer.SetPageSettings(pageSettings);
                //FinanceReportViewer.GetPageSettings()
                //System.Drawing.Printing.PageSettings ewr = new System.Drawing.Printing.PageSettings();
                //ewr.Margins.Bottom = 0;
                //ewr.Margins.Top = 0;
                //ewr.Margins.Left = 10;
                //ewr.Margins.Right = 0;
                ////ewr.Landscape = true;
                
                //this.FinanceReportViewer.SetPageSettings(ewr);
            }
            catch (Exception ex)
            {
                Log.LogInfo(ex.Message);               
            }
            

        }
        
        //缩放模式为百分比,以100%方式显示
        //this.FinanceReportViewer.ZoomMode = ZoomMode.Percent;
        //this.FinanceReportViewer.ZoomPercent = 100;
    }
}