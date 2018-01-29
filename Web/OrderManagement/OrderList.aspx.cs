using D2012.Common.DbCommon;
using D2012.Domain.Entities;
using D2012.Domain.Services;
using GeeStar.Workflow.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OrderManagement_OrderList : PageBase
{
    ServiceCommon servComm = new ServiceCommon();
    ConditionComponent ccWhere = new ConditionComponent();
    ServiceCommon facservComm;
    protected int tcount;
    protected string hddpnumbers;
    protected string zNodes = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Log.LogInfo("OrderList");
        facservComm = new ServiceCommon(base.factoryConnectionString);
        ccWhere.Clear();
        if(LoginUser.Kind == "B"){
         ccWhere.AddComponent("sellerid",LoginUser.AssocNo.ToString(), SearchComponent.Equals, SearchPad.NULL);
        }
        else if (LoginUser.Kind == "C")
        {
            ccWhere.AddComponent("hospitalid", LoginUser.AssocNo.ToString(), SearchComponent.Equals, SearchPad.NULL);
        }
        else if (LoginUser.Kind == "D")
        {
            ccWhere.AddComponent("doctorid", LoginUser.AssocNo.ToString(), SearchComponent.Equals, SearchPad.NULL);
        }
        if (ccWhere.sbComponent.ToString() == "")
            ccWhere.AddComponent("1", "1", SearchComponent.Equals, SearchPad.NULL);

        facservComm.strOrderString = IsCN ? " NameCN  " : " NameEN  ";

       // DataTable dtSeller = facservComm.GetListTop(0, " distinct sellerid as id,Seller as NameCN,Seller as NameEN ", "vieworganization", ccWhere);
        DataTable dtSeller = facservComm.ExecuteSqlDatatable("select distinct sellerid as id,Seller as NameCN,Seller as NameEN from " + vieworganizationorigin + " where " + ccWhere.sbComponent);

        facservComm.strOrderString = IsCN ? " NameCN  " : " NameEN  ";

        //DataTable dtHospital = facservComm.GetListTop(0, " distinct hospitalid as id,hospital as NameCN,hospital as NameEN,sellerid ", "vieworganization", ccWhere);
        DataTable dtHospital = facservComm.ExecuteSqlDatatable("select distinct hospitalid as id,hospital as NameCN,hospital as NameEN,sellerid from " + vieworganizationorigin + " where " + ccWhere.sbComponent);

        facservComm.strOrderString = IsCN ? " NameCN  " : " NameEN  ";

        //DataTable dtDoctor = facservComm.GetListTop(0, " distinct doctorid as id,doctor as NameCN,doctor as NameEN,Hospitalid", "vieworganization", ccWhere);
        DataTable dtDoctor = facservComm.ExecuteSqlDatatable("select distinct doctorid as id,doctor as NameCN,doctor as NameEN,Hospitalid from " + vieworganizationorigin + " where " + ccWhere.sbComponent);

        for (int i = 0; i < dtSeller.Rows.Count; i++)
        {
            string sellid = dtSeller.Rows[i]["id"].ToString();
            zNodes = zNodes + ",{" + "\"id\":" + sellid + ",\"pId\":0" + ",\"name\":" + "\"" + (IsCN ? dtSeller.Rows[i]["NameCN"] : dtSeller.Rows[i]["NameEN"]) + "\",\"open\": true,\"icon\":" + "\"../zTree_v3/css/zTreeStyle/img/diy/seller.png\"" + "}";
            DataRow[] drHospital = dtHospital.Select("sellerid='" + sellid + "'");
            foreach (DataRow itemHosptal in drHospital)
            {
                string hospitalID = itemHosptal["id"].ToString();
                zNodes = zNodes + ",{" + "\"id\":" + hospitalID + ",\"pId\":" + sellid + ",\"name\":" + "\"" + (IsCN?itemHosptal["NameCN"]:itemHosptal["NameEN"]) + "\",\"icon\":" + "\"../zTree_v3/css/zTreeStyle/img/diy/hospital.png\"" + "}";
                DataRow[] drDoctor = dtDoctor.Select("Hospitalid='" + hospitalID + "'");
                foreach (DataRow itemDoctor in drDoctor)
                {
                    zNodes = zNodes + ",{" + "\"id\":" + itemDoctor["id"].ToString() + ",\"pId\":" + hospitalID + ",\"name\":" + "\"" + (IsCN?itemDoctor["NameCN"]: itemDoctor["NameEN"])+ "\",\"icon\":" + "\"../zTree_v3/css/zTreeStyle/img/diy/doctor.png\"" + "}";
                }
            }
            
        }
        zNodes = zNodes.Length > 0 ? zNodes.Substring(1) : zNodes;
        zNodes = "[" + zNodes + "]";
       // zNodes = "[{ \"id\":1, \"pId\":0, \"name\": \"父节点1 - 展开\", \"open\": true }" +
       //",{ \"id\":11, \"pId\":1, \"name\": \"父节点11 - 折叠\" }]";
    }
}