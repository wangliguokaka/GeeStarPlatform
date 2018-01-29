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
public partial class ReportStatistics_SelectHospital : PageBase
{
    ServiceCommon servComm = new ServiceCommon();
    ServiceCommon facservComm;
    
    ConditionComponent ccWhere = new ConditionComponent();
    protected string pid;
    protected string cid;
    protected string sellerid;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        sellerid = Request["seller"];
        if (yeyRequest.Params("type") == "GetJson")
        {
            GetSceneryTypeList();
        }

    }

    private void GetSceneryTypeList()
    {
        facservComm = new ServiceCommon(factoryConnectionString);
        string condition = "";
        
        if (LoginUser.Kind == "A")
        {
            condition = " where sellerid =" + sellerid;
        }
        else if (LoginUser.Kind == "B")
        {
            condition = " where sellerid =" + LoginUser.AssocNo;
        }
        else {
            condition = " where id =" + GetOrganization["hospitalid"];
        }
        DataTable dt = facservComm.ExecuteSqlDatatable("SELECT id as HospitalID,hospital as Hospital FROM [Hospital] " + condition);
        //dt.Columns.Add("ColorType");
        //string[] colorType = new string[] { "A1", "A2", "A3", "A3.5", "A4", "B1", "B2", "B3", "B4", "C1", "C2", "C3", "C4", "D2", "D3", "D4", "1M1"
        //    ,"1M2","2L1.5","2L2.5","2M1","2M2","2M3","2R1.5","2R2.5","3L1.5","3L2.5","3M1","3M2","3M3","3R1.5","3R2.5","4L1.5","4L2.5","4M1","4M2","4M3"
        //    ,"4R1.5","4R2.5","5M1","5M2","5M3"};
        dt.TableName = "DataHospital";
        //for(int i = 0;i<colorType.Length;i++){
        //    dt.Rows.Add(dt.NewRow());
        //    dt.Rows[i][0] = colorType[i].ToString();
        //}
       

      

        string backstr = Json.DataTable2Json(dt);

        if (backstr.IndexOf("\\") >= 0)
        {
            backstr = backstr.Replace("\\", "\\\\");
        }

        Response.Write(backstr);
        Response.End();
    }


}
