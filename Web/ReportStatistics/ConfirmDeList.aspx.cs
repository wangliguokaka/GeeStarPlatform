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

public partial class ReportStatistics_ConfirmDeList : PageBase
{
    ConditionComponent ccWhere = new ConditionComponent();
    ServiceCommon facservComm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsCN == false)
        {
            ConfirmDeList.LocalReport.ReportPath = ConfirmDeList.LocalReport.ReportPath.Replace("En", "").Replace(".rdlc", "En.rdlc");
        }
        facservComm = new ServiceCommon(base.factoryConnectionString);
        if (!IsPostBack)
        {
            BindComfirmdeListReport(true);
        }
        if (!IsPostBack)
        {
            //DataTable dtSeller = facservComm.GetListTop(0, " distinct sellerid,Seller ", "vieworganization", null);
            DataTable dtSeller = facservComm.ExecuteSqlDatatable("select distinct sellerid,Seller from " + vieworganizationorigin + " where " + ccWhere.sbComponent);
            this.seller.DataTextField = "Seller";
            this.seller.DataValueField = "sellerid";
            this.seller.DataSource = dtSeller;
            this.seller.DataBind();
            this.seller.Items.Insert(0, new ListItem("请选择", ""));

            if (GetOrganization.Count > 0)
            {
                this.seller.SelectedValue = GetOrganization["sellerid"].ToString();
                this.seller.Enabled = false;
            }
            BindComfirmdeListReport(true);

            DataTable dtConfirmDeList = new NewListDataSet.VWConfirmDeListDataTable();
            this.ConfirmDeList.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dtConfirmDeList));

            DataTable dtCharge = facservComm.ExecuteSqlDatatable("SELECT Code,DictName FROM [DictDetail] where ClassID  = 'charges' order by sortno");
            this.charges.DataSource = dtCharge;
            this.charges.DataBind();

            orderType.DataSource = BindDictClass(facservComm, ccWhere, "OrderClass");
            orderType.DataBind();
        }
        if (!String.IsNullOrEmpty(Request["refresh"]))
        {
            BindComfirmdeListReport();
        }    
    }

    private void BindComfirmdeListReport(bool initFlg = false)
    {
        if (initFlg)
        {
            this.ConfirmDeList.Visible = false;
        }
        else
        { 
            this.ConfirmDeList.Visible = true;
            this.ConfirmDeList.LocalReport.DataSources.Clear();
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
                else
                {
                    ccWhere.AddComponent("preoutdate", Request["txtpredate"], SearchComponent.Equals, SearchPad.And);
                }
            }

            if (!String.IsNullOrEmpty(this.CaseNo.Text))
            {
                ccWhere.AddComponent("ModelNo", this.CaseNo.Text, SearchComponent.Like, SearchPad.And);
            }

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
                ccWhere.AddComponent("orderclasscode", orderClass, SearchComponent.In, SearchPad.And);
            }

       
          
                GetFilterByKind(ref ccWhere, "Report");
            //DataTable dtModelProcessing = facservComm.ExecuteSqlDatatable("select * from VWConfirmDeList");
            DataTable dtConfirmDeList = facservComm.ExecuteSqlDatatable(ConfirmDeList + " where " + ccWhere.sbComponent);
            
           // DataTable dtConfirmDeList = facservComm.GetListTop(0, "* ", "VWConfirmDeList", ccWhere);
                this.ConfirmDeList.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dtConfirmDeList));
                this.ConfirmDeList.DataBind();
                this.ConfirmDeList.LocalReport.Refresh();
            //}
        }
    }
}