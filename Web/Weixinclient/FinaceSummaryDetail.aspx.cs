using D2012.Common;
using D2012.Common.DbCommon;
using D2012.Domain.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Weixin
{
    public partial class FinaceSummaryDetail : PageBase
    {
        protected DataTable dtFinaceSummary = new DataTable();
        private DataTable dtAllFinaceSummary = new DataTable();
        ConditionComponent ccWhere = new ConditionComponent();
        protected int tcount;
        protected string hddpnumbers;
        ServiceCommon servCommfac;
        protected string sumQty = "";
        protected string sumAmount = "";
        protected string sumNobleWeight = "";
        protected string sumNobleAmount = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ((HtmlContainerControl)Master.FindControl("HTitle")).InnerText = IsCN ? "结算单明细表" : "FinaceSummary Detail";
            }
            querycontrol.DispalyType = "0";
            servCommfac = new ServiceCommon(base.factoryConnectionString);
            ccWhere.Clear();
            ccWhere = querycontrol.GetCondtion();
            ccWhere.sbComponent = ccWhere.sbComponent.Replace("orders.", "");
            ccWhere.sbComponent = ccWhere.sbComponent.Replace("ordersdetail.", "");
            servCommfac.strOrderString = querycontrol.OrderString == "" ? " regtime desc " : querycontrol.OrderString;

            hddpnumbers = yeyRequest.Params("hpnumbers");
            int iCount = 5;
            if (!string.IsNullOrEmpty(hddpnumbers))
            {
                iCount = Convert.ToInt32(hddpnumbers);
            }

            int iPageIndex = string.IsNullOrEmpty(Request["sPageID"]) ? 1 : Convert.ToInt32(Request["sPageID"]);
            int iPageCount = string.IsNullOrEmpty(Request["sPageNum"]) ? 0 : Convert.ToInt32(Request["sPageNum"]);
            if (Request["submitflg"] != null && Request["submitflg"] == "1")
            {
                iPageIndex = 1;
                this.pagecutID.iPageIndex = 1;
            }

            string viewsql = "SELECT  orders.Order_ID , orders.serial ,orders.ModelNo , orders.seller ,orders.sellerid , orders.hospital ,orders.hospitalid , orders.orderclass, (select top 1 DictName from dictdetail where Code = orders.orderclass and ClassID = 'OrderClass') as orderclassname  , orders.doctor ,orders.patient ,convert(nvarchar(8), orders.indate, 11) as indate ,orders.Outflag ,	"
+ " convert(nvarchar(8), orders.Outdate, 11) as Outdate , ltrim(orders.OutSay) as Explain , ordersdetail.subid ,ordersdetail.productid , ordersdetail.qty ,ordersdetail.bColor , ordersdetail.price ,ordersdetail.amount ,rtrim(ordersdetail.a_teeth) as a_teeth ,rtrim(ordersdetail.b_teeth) as b_teeth ,"
+ " rtrim(ordersdetail.c_teeth) as c_teeth , rtrim(ordersdetail.d_teeth) as d_teeth ,ordersdetail.Nobleclass , ordersdetail.NobleWeight , ordersdetail.nobleAmount ,products_itemname = products.itemname,    products.unit,(select top 1 corp from base) as corp,orders.charges ,orders.preoutdate FROM orders ,"
+ " ordersdetail ,products WHERE(ordersdetail.Order_ID = orders.Order_ID) and(ordersdetail.serial = orders.serial) and products.id = ordersdetail.productid ";


            if (GetOrganization.Count >= 2)
            {

                string userHospitalID = GetOrganization["hospitalid"].ToString();

                DataTable dt = servCommfac.ExecuteSqlDatatable("SELECT PrntPrices FROM [Hospital] where id =" + userHospitalID);
                string PrntPrices = "";
                if (dt.Rows.Count > 0)
                {
                    string serial = "";
                    PrntPrices = dt.Rows[0][0].ToString().Trim();
                    if (PrntPrices == "产品模板")
                    {
                        serial = PrntPrices;
                    }
                    else
                    {
                        if (PrntPrices != null && !"".Equals(PrntPrices))
                        {
                            for (int i = 0; i < PrntPrices.ToCharArray().Length; i++)
                            {
                                if (PrntPrices.ToCharArray()[i] >= 48 && PrntPrices.ToCharArray()[i] <= 57)
                                {
                                    serial = serial + PrntPrices.ToCharArray()[i];
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    ConditionComponent productWhere = new ConditionComponent();
                    ServiceCommon servCommProduct = new ServiceCommon(base.factoryConnectionString);
                    productWhere.Clear();
                    servCommProduct.strOrderString = "";
                    productWhere.AddComponent("serial", serial, SearchComponent.Equals, SearchPad.And);
                    // ccWhere.AddComponent("productid", productID, SearchComponent.Equals, SearchPad.And);
                    //DataTable dtPrice = servCommProduct.GetListTop(1, "price", "prices", productWhere);
                    //string price = "0";
                    //if (dtPrice.Rows.Count == 1)
                    //{
                    //    price = dtPrice.Rows[0][0].ToString();
                    //}
                    //ccWhere.Clear();
                    //GetFilterByKind(ref ccWhere, "Report");
                    if (String.IsNullOrEmpty(serial))
                    {
                        if (Utils.IsNoSP)
                        {
                            dtAllFinaceSummary = servCommfac.ExecuteSqlDatatable(" select row_number()over(order by Order_ID,serial) as rowIndex ,Order_ID,hospital,doctor,patient,indate,Outdate,orderclassname,products_itemname,qty,null as NobleWeight,a_teeth,b_teeth,c_teeth,d_teeth,null as price,null as amount from (" + viewsql + ") as VWFinaceProductDetail where " + ccWhere.sbComponent);

                        }
                        else
                        {
                            dtAllFinaceSummary = servCommfac.GetListTop(0, " row_number()over(order by Order_ID,serial) as rowIndex ,Order_ID,hospital,doctor,patient,indate,Outdate,orderclassname,products_itemname,qty,null as NobleWeight,a_teeth,b_teeth,c_teeth,d_teeth,null as price,null as amount ", "VWFinaceProductDetail", ccWhere);

                        }
                    }
                    else if (serial == "产品模板")
                    {
                        if (Utils.IsNoSP)
                        {
                            dtAllFinaceSummary = servCommfac.ExecuteSqlDatatable(" select row_number()over(order by Order_ID,serial) as rowIndex ,Order_ID,hospital,doctor,patient,indate,Outdate,orderclassname,products_itemname,qty,NobleWeight,a_teeth,b_teeth,c_teeth,d_teeth,price,amount from (" + viewsql + ") as VWFinaceProductDetail where " + ccWhere.sbComponent);

                        }
                        else
                        {
                            dtAllFinaceSummary = servCommfac.GetListTop(0, " row_number()over(order by Order_ID,serial) as rowIndex ,Order_ID,hospital,doctor,patient,indate,Outdate,orderclassname,products_itemname,qty,NobleWeight,a_teeth,b_teeth,c_teeth,d_teeth,price,amount ", "VWFinaceProductDetail", ccWhere);
                        }
                    }
                    else
                    {
                        if (Utils.IsNoSP)
                        {
                            dtAllFinaceSummary = servCommfac.ExecuteSqlDatatable(" select row_number()over(order by Order_ID,serial) as rowIndex ,Order_ID,hospital,doctor,patient,indate,Outdate,orderclassname,products_itemname,qty,NobleWeight,a_teeth,b_teeth,c_teeth,d_teeth,(select price from prices where productid = VWFinaceProductDetail.productid and Serial = " + serial + " ) as price,(select price from prices where productid = VWFinaceProductDetail.productid and Serial = " + serial + " )*qty as amount from (" + viewsql + ") as VWFinaceProductDetail where " + ccWhere.sbComponent);

                        }
                        else
                        {
                            dtAllFinaceSummary = servCommfac.GetListTop(0, " row_number()over(order by Order_ID,serial) as rowIndex ,Order_ID,hospital,doctor,patient,indate,Outdate,orderclassname,products_itemname,qty,NobleWeight,a_teeth,b_teeth,c_teeth,d_teeth,(select price from prices where productid = VWFinaceProductDetail.productid and Serial = " + serial + " ) as price,(select price from prices where productid = VWFinaceProductDetail.productid and Serial = " + serial + " )*qty as amount ", "VWFinaceProductDetail", ccWhere);
                        }
                    }
                }
            }
            else
            {
                if (Utils.IsNoSP)
                {
                    dtAllFinaceSummary = servCommfac.ExecuteSqlDatatable(" select row_number()over(order by Order_ID,serial) as rowIndex ,* from (" + viewsql + ") as VWFinaceProductDetail where " + ccWhere.sbComponent);

                }
                else
                {
                    dtAllFinaceSummary = servCommfac.GetListTop(0, " row_number()over(order by Order_ID,serial) as rowIndex ,* ", "VWFinaceProductDetail", ccWhere);
                }
            }       
            

            sumAmount = dtAllFinaceSummary.Compute("sum(amount)", "").ToString();
           
            int rowcount = dtAllFinaceSummary.Rows.Count;
            this.pagecutID.iPageNum = (rowcount - 1) / 10 + 1;
            if (dtAllFinaceSummary.Rows.Count > 0)
            {
                dtFinaceSummary = dtAllFinaceSummary.Select(" rowIndex>=" + ((pagecutID.iPageIndex - 1) * 10 + 1).ToString() + " and rowIndex <=" + pagecutID.iPageIndex * 10).CopyToDataTable();      
            }
            else
            {
                dtFinaceSummary = dtAllFinaceSummary.Clone();
            }
        }
    }
}