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
public partial class GeeStar_ViewPatientDetail : PageBase
{
    ServiceCommon servCommfac;
    ConditionComponent ccWhere = new ConditionComponent();

    protected string orderID = "";
   
    protected void Page_Load(object sender, EventArgs e)
    {
        servCommfac = new ServiceCommon(base.factoryConnectionString);
        orderID = Request["orderID"];
        if (yeyRequest.Params("type") == "GetJson")
        {
          
            if (orderID != "")
            {
                GetPatientList(orderID);
            }
           
        }

    }

    private void GetPatientList(string orderID)
    {
        int iCount = 1;
        int iPageIndex = yeyRequest.Params("pageindex") == null ? 1 : Convert.ToInt32(yeyRequest.Params("pageindex"));
        int iPageCount = yeyRequest.Params("hPageNum") == null ? 0 : Convert.ToInt32(yeyRequest.Params("hPageNum"));
        ccWhere.Clear();


        ccWhere.AddComponent("Order_ID", orderID, SearchComponent.Equals, SearchPad.NULL);
        servCommfac.strOrderString = "id";

        string countsql = "select count(1) FROM orders a inner join OrdersDetail b on a.Order_ID = b.Order_ID and a.serial = b.serial inner join products c on b.ProductId = c.id  where a.outflag = 'N' and  a.Order_ID = '" + orderID + "' and a.serial = 0 ";
        DataTable dtCount = servCommfac.ExecuteSqlDatatable(countsql);
        int AllRowCount = int.Parse(dtCount.Rows[0][0].ToString());

        string sqlview = "select * from ( select ROW_NUMBER() over(order by a.Order_ID) as rowIndex ,a.Order_ID,a.serial,a.ModelNo,a.seller,a.sellerid,a.hospital,a.hospitalid,a.doctor,a.patient,b.subId,c.itemname as productName,c.Valid,b.a_teeth,b.b_teeth,b.c_teeth,b.d_teeth,a.regtime,a.Area,b.bColor,a.OutDate,a.tel,a.QlyName,a.QlyTime,c.LinkCard as Element "
  + " FROM orders a inner join OrdersDetail b on a.Order_ID = b.Order_ID and a.serial = b.serial inner join products c on b.ProductId = c.id  where a.outflag = 'N' and  a.Order_ID = '" + orderID + "' and a.serial = '" + 0 + "' ) t where rowindex between '" + ((iPageIndex - 1) * iCount + 1).ToString() + "' and '" + (iPageIndex * iCount).ToString() + "'";

        DataTable dtZhujian = servCommfac.ExecuteSqlDatatable(sqlview);



        dtZhujian.TableName = "DataPatient";
        if (iPageCount <= 1 && dtZhujian.Rows.Count>0)
        {
            
            iPageCount = ((AllRowCount - 1) / iCount) + 1;
            iPageIndex = 1;
        }

        string backstr = Json.DataTable2Json(dtZhujian, ",\"PageCount\":" + iPageCount.ToString());

        if (backstr.IndexOf("\\") >= 0)
        {
            backstr = backstr.Replace("\\", "\\\\");
        }

        Response.Write(backstr);
        Response.End();
    }


}
