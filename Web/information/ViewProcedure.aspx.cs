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
public partial class GeeStar_ViewProcedure : PageBase
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
                GetProcedureList(orderID);
            }
           
        }

    }

    private void GetProcedureList(string orderID)
    {
        int iCount = 6;
        int iPageIndex = yeyRequest.Params("pageindex") == null ? 1 : Convert.ToInt32(yeyRequest.Params("pageindex"));
        int iPageCount = yeyRequest.Params("hPageNum") == null ? 0 : Convert.ToInt32(yeyRequest.Params("hPageNum"));
        ccWhere.Clear();


        ccWhere.AddComponent("Order_ID", orderID, SearchComponent.Equals, SearchPad.NULL);
        servCommfac.strOrderString = "id";
        DataTable dtZhujian1 = servCommfac.ExecuteSqlDatatable("exec SPZhijian " + orderID + "," + ((iPageIndex - 1) * iCount + 1).ToString() + "," + (iPageIndex * iCount).ToString());

        string zhijiansql = "SELECT b.itemname as ProductName,a.Order_ID,a.itemQty,a.dept,a.producer,a.getin,isnull(a.Finish, a.PreTime) as finishTime,c.name as procedureName,isnull(itemQty, 0) as itemQty,productid,a.Gserial"
           + ",case when a.Id = (select MIN(id) from[zhijian] zj where a.productid = zj.productid and zj.Order_ID = @oderID and zj.serial = 0 and zj.overflag <> 'F' ) then 'M' else  a.overflag end overflag,rtrim(a.a_teeth) + ',' + rtrim(a.b_teeth) + ',' + rtrim(a.c_teeth) + ',' + rtrim(a.d_teeth) as teethPosition, "
+ " case when a.Upweb = 'Y' then '传完' else '未传' end Upweb,a_teeth, b_teeth, c_teeth, d_teeth FROM [zhijian] a inner join products b on a.productid = b.id inner join working_procedure c on a.Gserial = c.serial where a.Order_ID = '" + orderID + "' and a.serial = 0 order by ProductName,Gserial";
        DataTable dtZhujian = servCommfac.ExecuteSqlDatatable(zhijiansql);

        dtZhujian.TableName = "DataZhiJian";
        if (iPageCount <= 1 && dtZhujian.Rows.Count>0)
        {
            int AllRowCount = int.Parse(dtZhujian.Rows[0]["AllRowCout"].ToString());
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
