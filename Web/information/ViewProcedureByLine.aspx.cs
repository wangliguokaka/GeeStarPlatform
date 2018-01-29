using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using D2012.Domain.Services;
using D2012.Domain.Entities;
using D2012.Common.DbCommon;
using System.Text;
using D2012.Common;
using System.Data;
using System.Collections;
public partial class GeeStar_ViewProcedureByLine : PageBase
{
    ServiceCommon servCommfac;
    ConditionComponent ccWhere = new ConditionComponent();
    protected ORDERS orderModel = new ORDERS();
    protected string orderID = "";
    protected string serialID = "";
    protected string zNodes = "";
    protected List<int> positionM = new List<int>();
    protected string strPositionM = "";
   
    protected void Page_Load(object sender, EventArgs e)
    {
        servCommfac = new ServiceCommon(base.factoryConnectionString);


        if (Request["type"] == "GetBatch")
        {

            string batchOrderID = Request["BatchOrderID"];
            string batchSerialID = Request["BatchSerialID"];
            string batchProductID = Request["BatchProductID"];
            string zBatchNodes = "";
            ccWhere.Clear();
            ccWhere.AddComponent("Order_ID", batchOrderID, SearchComponent.Equals, SearchPad.And);
            ccWhere.AddComponent("serial", batchSerialID, SearchComponent.Equals, SearchPad.And);
            ccWhere.AddComponent("Productid", batchProductID, SearchComponent.Equals, SearchPad.And);
            DataTable dt = servCommfac.GetListTop(0, "isnull(Name,'') as Name,isnull(Maker,'') as Maker,isnull(batchNo,'') as batchNo", "OrdersElement", ccWhere);
            foreach (DataRow dr in dt.Rows)
            {
                zBatchNodes = zBatchNodes + "{" + "\"ElementName\":" + "\"" + dr["Name"].ToString() + "\"" + ",\"ElementMaker\":" + "\"" + dr["Maker"].ToString() + "\"" + ",\"BatchNo\":" + "\"" + dr["batchNo"].ToString() + "\"},";

            }
            zBatchNodes = zBatchNodes.Trim(',');
            if (zBatchNodes == "")
            {
                zBatchNodes = "{}";

            }
            else
            {
                zBatchNodes = "[" + zBatchNodes + "]";
            }

            Response.Write(zBatchNodes);
            Response.End();
        }
        else
        {
            orderID = Request["orderID"];
            serialID = Request["serial"];
            ccWhere.Clear();
            ccWhere.AddComponent("Order_ID", orderID, SearchComponent.Equals, SearchPad.NULL);
            ccWhere.AddComponent("serial", serialID, SearchComponent.Equals, SearchPad.And);
            IList<ORDERS> listOrder = servCommfac.GetListTop<ORDERS>(0, "hospital,doctor,Patient,Order_ID,indate,preoutDate,OutDate,Sex,Age", "orders", ccWhere);
            if (listOrder.Count > 0)
            {
                orderModel = listOrder[0];
            }
            GetProcedureList(orderID, serialID);
        }

    }

    private void GetProcedureList(string orderID, string serialID)
    {
        int iCount = 6;
        int iPageIndex = yeyRequest.Params("pageindex") == null ? 1 : Convert.ToInt32(yeyRequest.Params("pageindex"));
        int iPageCount = yeyRequest.Params("hPageNum") == null ? 0 : Convert.ToInt32(yeyRequest.Params("hPageNum"));
        ccWhere.Clear();
        int ProductIndex = 0;
        int MIndex = 0;
        ccWhere.AddComponent("Order_ID", orderID, SearchComponent.Equals, SearchPad.NULL);
        servCommfac.strOrderString = "id";
        //DataTable dtZhujian = servCommfac.ExecuteSqlDatatable("exec SPZhijian " + orderID + ","+ serialID+","+ ((iPageIndex - 1) * iCount +1).ToString() + "," + (iPageIndex * iCount).ToString());
        string zhijiansql = "SELECT b.itemname as ProductName,a.Order_ID,a.itemQty,a.dept,a.producer,a.getin,isnull(a.Finish, a.PreTime) as finishTime,c.name as procedureName,isnull(itemQty, 0) as itemQty,productid,a.Gserial"
           + ",case when a.Id = (select MIN(id) from[zhijian] zj where a.productid = zj.productid and zj.Order_ID =  '" + orderID + "' and zj.serial =  '" + serialID + "' and zj.overflag <> 'F' ) then 'M' else  a.overflag end overflag,rtrim(a.a_teeth) + ',' + rtrim(a.b_teeth) + ',' + rtrim(a.c_teeth) + ',' + rtrim(a.d_teeth) as teethPosition, "
+ " case when a.Upweb = 'Y' then '传完' else '未传' end Upweb,a_teeth, b_teeth, c_teeth, d_teeth FROM[zhijian] a inner join products b on a.productid = b.id inner join working_procedure c on a.Gserial = c.serial where a.Order_ID = '" + orderID + "' and a.serial = '" + serialID + "' order by ProductName,Gserial";
        DataTable dtZhujian = servCommfac.ExecuteSqlDatatable(zhijiansql);
        string strProductName = "";
        for (int i = 0; i < dtZhujian.Rows.Count; i++)
        {
            string productName = dtZhujian.Rows[i]["ProductName"].ToString();
            string teethPosition = dtZhujian.Rows[i]["teethPosition"].ToString();
            string productNumber = dtZhujian.Rows[i]["itemQty"].ToString();
            string productID = dtZhujian.Rows[i]["productid"].ToString();
            if (strProductName != productName)
            {
                 strProductName=productName;
                if (zNodes != "")
                {
                    zNodes = zNodes.Trim(',');
                    zNodes = zNodes + "]},";
                }
                MIndex = 0;
                zNodes = zNodes + "{" + "\"productName\":" + "\"" + productName + "\"," + "\"productNumber\":" + "\"" + productNumber + "\"," + "\"productID\":" + "\"" + productID + "\"," + "\"teethPosition\":" + "\"" + teethPosition + "\"" + ",\"procedure\":[";
                positionM.Add(0);
                ProductIndex = ProductIndex+1;
            }
            MIndex = MIndex + 1;
            string title = dtZhujian.Rows[i]["procedureName"].ToString();
            string overflag = dtZhujian.Rows[i]["overflag"].ToString();
            string finishTime = TrimWithNull(dtZhujian.Rows[i]["finishTime"]);
            string content = "";
            if (IsCN)
            {
                if (overflag == "F")
                {
                    content = "完成时间：" + finishTime;
                }
                else if (overflag == "M")
                {
                    positionM[ProductIndex - 1] = MIndex;
                    content = "预计完成时间：" + finishTime;
                }
                else if (overflag == "N")
                {
                    content = "预计完成时间：" + finishTime;
                }
            }
            else
            {
                if (overflag == "F")
                {
                    content = "FinishTime：" + finishTime;
                }
                else if (overflag == "M")
                {
                    positionM[ProductIndex - 1] = MIndex;
                    content = "PredictFinishTime：" + finishTime;
                }
                else if (overflag == "N")
                {
                    content = "PredictFinishTime：" + finishTime;
                }
            }
           
            zNodes = zNodes + "{" + "\"title\":" + "\"" + title + "\"" + ",\"content\":" + "\"" + content + "\"},";
            

        }
        zNodes = zNodes.Trim(',');
        if (zNodes == "")
        {
            zNodes = "{}";

        }
        else
        {
            zNodes = "[" + zNodes + "]}]";
        }
        strPositionM = string.Join(",", positionM.ToArray());
    }


}
