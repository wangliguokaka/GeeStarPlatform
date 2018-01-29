 using D2012.Common;
using D2012.Common.DbCommon;
using D2012.Domain.Entities;
using D2012.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Weixinclient_product : PageBase
{  
    ServiceCommon servCommfac;
    ConditionComponent ccWhere = new ConditionComponent();
    protected PRODUCTS productsModel = new PRODUCTS();
    protected ORDERSDETAIL orderdatailsModel = new ORDERSDETAIL();
    protected string orderID = "";
    protected string serialID = "";
    protected string productid = "";
    protected string zNodes = "";
    protected string toothposition = "";
    protected List<int> positionM = new List<int>();
    protected string strPositionM = "";
    protected DataRow[] drZhujian = new DataRow[] { };
    protected string preOutDate;
    protected void Page_Load(object sender, EventArgs e)
    { 
        try
        {
            if (!IsPostBack)
            {
                ((HtmlContainerControl)Master.FindControl("HTitle")).InnerText = "产品详细";
            }
            servCommfac = new ServiceCommon(base.factoryConnectionString);
            orderID = Request["orderID"];
            serialID = Request["serial"];
            productid = Request["productid"];
            ccWhere.Clear();
            ccWhere.AddComponent("Order_ID", orderID, SearchComponent.Equals, SearchPad.NULL);
            ccWhere.AddComponent("serial", serialID, SearchComponent.Equals, SearchPad.And);
            IList<ORDERS> listOrder = servCommfac.GetListTop<ORDERS>(0, "*", "Orders", ccWhere);
            if (listOrder.Count > 0)
            {
                preOutDate = listOrder[0].preoutDate.ToShortDateString();
            }
            //ccWhere.AddComponent("productid", productid, SearchComponent.Equals, SearchPad.And);
            ccWhere.AddComponent("ProductID", productid, SearchComponent.Equals, SearchPad.And);
            DataTable dtProduct = servCommfac.ExecuteSqlDatatable("SELECT itemname FROM products b where id = '" + productid + "'");
            IList<ORDERSDETAIL> listProducts = servCommfac.GetListTop<ORDERSDETAIL>(0, "*", "OrdersDetail", ccWhere);
            if (listProducts.Count > 0)
            {
                productsModel.ItemName = dtProduct.Rows.Count>0?dtProduct.Rows[0]["itemname"].ToString():"";
                productsModel.Number = listProducts[0].Qty.ToString();
                productsModel.righttop = listProducts[0].a_teeth.ToString().Trim();
                productsModel.lefttop = listProducts[0].b_teeth.ToString().Trim();
                productsModel.rightbottom = listProducts[0].c_teeth.ToString().Trim();
                productsModel.leftbottom = listProducts[0].d_teeth.ToString().Trim();
               // listProducts[0].pr
            }
           
            GetProcedureList(orderID, serialID, productid);
        }
        catch (Exception ex) { 
        
        }
    }
    private void GetProcedureList(string orderID, string serialID, string productid)
    {
        int iCount = 6;
        int iPageIndex = yeyRequest.Params("pageindex") == null ? 1 : Convert.ToInt32(yeyRequest.Params("pageindex"));
        int iPageCount = yeyRequest.Params("hPageNum") == null ? 0 : Convert.ToInt32(yeyRequest.Params("hPageNum"));
        ccWhere.Clear();
        int ProductIndex = 0;
        int MIndex = 0;
        ccWhere.AddComponent("Order_ID", orderID, SearchComponent.Equals, SearchPad.NULL);
        servCommfac.strOrderString = "id";
        string zhijiansql = "SELECT b.itemname as ProductName,a.Order_ID,a.itemQty,a.dept,a.producer,a.getin,isnull(a.Finish, a.PreTime) as finishTime,c.name as procedureName,isnull(itemQty, 0) as itemQty,productid,a.Gserial"
            + ",case when a.Id = (select MIN(id) from[zhijian] zj where a.productid = zj.productid and zj.Order_ID = @oderID and zj.serial = @serialID and zj.overflag <> 'F' ) then 'M' else  a.overflag end overflag,rtrim(a.a_teeth) + ',' + rtrim(a.b_teeth) + ',' + rtrim(a.c_teeth) + ',' + rtrim(a.d_teeth) as teethPosition, "
 + " case when a.Upweb = 'Y' then '传完' else '未传' end Upweb,a_teeth, b_teeth, c_teeth, d_teeth FROM[zhijian] a inner join products b on a.productid = b.id inner join working_procedure c on a.Gserial = c.serial where a.Order_ID = '"+ orderID + "' and a.serial = '"+ serialID + "' order by ProductName,Gserial";
        DataTable dtZhujian = servCommfac.ExecuteSqlDatatable(zhijiansql);

        drZhujian = dtZhujian.Select("productid = '" + productid + "'","");
        //string strProductName = "";
        //for (int i = 0; i < dtZhujian.Rows.Count; i++)
        //{
        //    string productName = dtZhujian.Rows[i]["ProductName"].ToString();
        //    string teethPosition = dtZhujian.Rows[i]["teethPosition"].ToString();
        //    string productNumber = dtZhujian.Rows[i]["itemQty"].ToString();
        //    string productID = dtZhujian.Rows[i]["productid"].ToString();
        //    if (strProductName != productName)
        //    {
        //         strProductName=productName;
        //        if (zNodes != "")
        //        {
        //            zNodes = zNodes.Trim(',');
        //            zNodes = zNodes + "]},";
        //        }
        //        MIndex = 0;
        //        zNodes = zNodes + "{" + "\"productName\":" + "\"" + productName + "\"," + "\"productNumber\":" + "\"" + productNumber + "\"," + "\"productID\":" + "\"" + productID + "\"," + "\"teethPosition\":" + "\"" + teethPosition + "\"" + ",\"procedure\":[";
        //        positionM.Add(0);
        //        ProductIndex = ProductIndex+1;
        //    }
        //    MIndex = MIndex + 1;
        //    string title = dtZhujian.Rows[i]["procedureName"].ToString();
        //    string overflag = dtZhujian.Rows[i]["overflag"].ToString();
        //    string finishTime = TrimWithNull(dtZhujian.Rows[i]["finishTime"]);
        //    string content = "";
        //    if (IsCN)
        //    {
        //        if (overflag == "F")
        //        {
        //            content = "完成时间：" + finishTime;
        //        }
        //        else if (overflag == "M")
        //        {
        //            positionM[ProductIndex - 1] = MIndex;
        //            content = "预计完成时间：" + finishTime;
        //        }
        //        else if (overflag == "N")
        //        {
        //            content = "预计完成时间：" + finishTime;
        //        }
        //    }
        //    else
        //    {
        //        if (overflag == "F")
        //        {
        //            content = "FinishTime：" + finishTime;
        //        }
        //        else if (overflag == "M")
        //        {
        //            positionM[ProductIndex - 1] = MIndex;
        //            content = "PredictFinishTime：" + finishTime;
        //        }
        //        else if (overflag == "N")
        //        {
        //            content = "PredictFinishTime：" + finishTime;
        //        }
        //    }
           
        //    zNodes = zNodes + "{" + "\"title\":" + "\"" + title + "\"" + ",\"content\":" + "\"" + content + "\"},";
            

        //}
        //zNodes = zNodes.Trim(',');
        //if (zNodes == "")
        //{
        //    zNodes = "{}";

        //}
        //else
        //{
        //    zNodes = "[" + zNodes + "]}]";
        //}
        //strPositionM = string.Join(",", positionM.ToArray());
    }
    }

 