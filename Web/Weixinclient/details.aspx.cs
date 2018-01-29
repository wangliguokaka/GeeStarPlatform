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
using System.Collections;
using System.Web.UI.HtmlControls;

public partial class Weixinclient_details : PageBase
{
    ServiceCommon servCommfac;
    ConditionComponent ccWhere = new ConditionComponent();
    protected ORDERS orderModel = new ORDERS();
    protected ORDERSDETAIL orderDetail = new ORDERSDETAIL();
    protected PRODUCTS productModel = new PRODUCTS();
    protected string orderID = "1511110001";
    protected string serialID = "0";
    protected string zNodes = "";
    protected string strPositionM = "";
    protected string itemname = "";
    protected string ProductId = "";
    protected List<string> itemnamelist = new List<string>();
    protected List<string> ProductIdlist = new List<string>();
    protected List<int> positionM = new List<int>();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ((HtmlContainerControl)Master.FindControl("HTitle")).InnerText = "订单详细";
            }
            servCommfac = new ServiceCommon(base.factoryConnectionString);
            orderID = Request["orderID"];
            serialID = Request["serial"];
            ccWhere.Clear();
            ccWhere.AddComponent("Order_ID", orderID, SearchComponent.Equals, SearchPad.NULL);
            ccWhere.AddComponent("serial", serialID, SearchComponent.Equals, SearchPad.And);
            IList<ORDERS> listOrder = servCommfac.GetListTop<ORDERS>(0, "seller,hospital,doctor,Patient,Order_ID,indate,preoutDate,OutDate,Sex,Age,Fenge,Require,Courier,CourierNo,OutSay,ModelNo", "orders", ccWhere);
            if (listOrder.Count > 0)
            {
                orderModel = listOrder[0];
            }
            DataTable dtProduct = servCommfac.ExecuteSqlDatatable("SELECT Order_ID,serial,ProductId,itemname FROM OrdersDetail a left join products b on a.productid = b.id where Order_ID = '" + orderID + "' and serial = '" + serialID + "'");
            for (int i = 0; i < dtProduct.Rows.Count; i++)
            {
                ProductId = dtProduct.Rows[i]["ProductId"].ToString();
                itemname = dtProduct.Rows[i]["itemname"].ToString();
                itemnamelist.Add(itemname);
                ProductIdlist.Add(ProductId);
                for(int j=0;j<itemnamelist.Count();j++){
                    itemname = dtProduct.Rows[i]["itemname"].ToString();
                    ProductId = dtProduct.Rows[i]["ProductId"].ToString();
                }
            }

        }
        catch (Exception ex)
        {

        }

    }          
  
    }

 