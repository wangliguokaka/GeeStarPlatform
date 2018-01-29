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

public partial class ReportStatistics_ModelProcessingSummary : PageBase
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

            orderType.DataSource = BindDictClass(facservComm, ccWhere, "OrderClass");
            orderType.DataBind();

            BindModelProcessingSummary(true);
            
        }

        if (!String.IsNullOrEmpty(Request["refresh"]))
        {
            BindModelProcessingSummary();
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

    private void BindModelProcessingSummary(bool initFlg = false)
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
            ccWhere.Clear();
            if (!String.IsNullOrEmpty(this.seller.SelectedValue))
            {
                ccWhere.AddComponent("sellerid", this.seller.SelectedValue, SearchComponent.Equals, SearchPad.NULL);
            }


            if (!String.IsNullOrEmpty(HospitalList))
            {
                ccWhere.AddComponent("hospitalid", "(" + HospitalList.Replace(":", ",") + ")", SearchComponent.In, SearchPad.And);
            }


            if (!String.IsNullOrEmpty(this.OrderNo.Text))
            {
                ccWhere.AddComponent("Order_ID", this.OrderNo.Text, SearchComponent.Like, SearchPad.And);
            }

            string orderClass = "";

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

            string dateselect = Request["dateselect"];

            if (!String.IsNullOrEmpty(dateselect))
            {
                if (dateselect == "0")
                {
                    ccWhere.AddComponent("indate", Request["txtdatefrom"], SearchComponent.GreaterOrEquals, SearchPad.And);
                    ccWhere.AddComponent("indate", Request["txtdateto"], SearchComponent.LessOrEquals, SearchPad.And);
                }
                else
                {
                    ccWhere.AddComponent("outdate", Request["txtdatefrom"], SearchComponent.GreaterOrEquals, SearchPad.And);
                    ccWhere.AddComponent("outdate", Request["txtdateto"], SearchComponent.LessOrEquals, SearchPad.And);
                }
            }

            GetFilterByKind(ref ccWhere, "Report");
          //  DataTable dtModelProcessing = facservComm.GetListTop(0, "* ", "ModelProcessingSummary", ccWhere);
            DataTable dtModelProcessing = facservComm.ExecuteSqlDatatable(ModelProcessingSummary + " and " + ccWhere.sbComponent);

            this.FinanceReportViewer.LocalReport.SetParameters(new ReportParameter("paraIndate", "2015年01月01日"));
            this.FinanceReportViewer.LocalReport.SetParameters(new ReportParameter("paraOutDate", DateTime.Now.Year.ToString() + "年" + DateTime.Now.Month.ToString() + "月" + DateTime.Now.Day + "日"));
            this.FinanceReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dtModelProcessing));
        }

    }
}