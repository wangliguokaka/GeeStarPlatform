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
public partial class OrderManagement_addOthers : PageBase
{
    ServiceCommon servComm = new ServiceCommon();
    ServiceCommon facservComm;
    
    ConditionComponent ccWhere = new ConditionComponent();
    protected string pid;
    protected string cid;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (yeyRequest.Params("type") == "GetJson")
        {
            GetSceneryTypeList();
        }

    }

    private void GetSceneryTypeList()
    {
        facservComm = new ServiceCommon(factoryConnectionString);
        DataTable dt = facservComm.ExecuteSqlDatatable("SELECT [Code] ,[DictName] as Accessory FROM [dbo].[DictDetail] where ClassID = 'Accessory' order by cast([Code] as int)");

        dt.TableName = "DataAccessory";      

        string backstr = Json.DataTable2Json(dt);

        if (backstr.IndexOf("\\") >= 0)
        {
            backstr = backstr.Replace("\\", "\\\\");
        }

        Response.Write(backstr);
        Response.End();
    }


}
