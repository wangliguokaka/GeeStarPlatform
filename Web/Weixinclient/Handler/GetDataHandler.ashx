<%@ WebHandler Language="C#" Class="GetDataHandler" Debug="true" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using D2012.Domain.Services;
using D2012.Common.DbCommon;
using D2012.Domain.Entities;
using System.Collections;

/// <summary>
/// Summary description for GetWorkFlowHandler
/// </summary>
public class GetDataHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    ServiceCommon servComm = new ServiceCommon();
    ConditionComponent ccWhere = new ConditionComponent();
    public string factoryConnectionString = "";
    public void ProcessRequest(HttpContext context)
    {
        factoryConnectionString = context.Session["factoryConnectionString"].ToString();
        GeeStar.Workflow.Common.Log.LogInfo("Upload");

        string strAction = context.Request["Action"];
        string returnValue = "";

        string strType = context.Request["ddlType"];
        string subID = context.Request["subID"];

        string strParaValue = context.Request["ddlId"];

        string IsCN = context.Request["IsCN"];
        DataTable dtResult = null;
        servComm.strConnectionString = factoryConnectionString;
        servComm.strOrderString = "";
        try
        {
            Hashtable ht = (Hashtable)context.Session["Organization"];

            string permissionCondion = "";
            string userSellerID = "";
            string userHospitalID = "";
            string userDoctorID = "";
            if (ht.Count > 0)
            {
                userSellerID = ht["sellerid"].ToString(); ;
            }
            if (ht.Count == 2)
            {
                userHospitalID = ht["hospitalid"].ToString();
            }
            else if (ht.Count == 3)
            {
                userHospitalID = ht["hospitalid"].ToString();
                userDoctorID = ht["doctorid"].ToString();
            }



            if (!string.IsNullOrEmpty(userSellerID))
            {

                permissionCondion = permissionCondion + " and sellerid='"+ userSellerID +"'";

            }

            if (!string.IsNullOrEmpty(userHospitalID))
            {

                permissionCondion = permissionCondion + " and Hospitalid='" + userHospitalID + "'";

            }



            if (!string.IsNullOrEmpty(userDoctorID))
            {
                permissionCondion = permissionCondion + " and doctor in (select top 1 doctor from doctor where id = " + userDoctorID + ")";
            }






            if (!String.IsNullOrEmpty(context.Request["datefrom"]))
            {
                permissionCondion = permissionCondion + " and indate >='" + context.Request["datefrom"] + "'";
            }

            if (!String.IsNullOrEmpty(context.Request["dateto"]))
            {
                permissionCondion = permissionCondion + " and indate <'" + DateTime.Parse(context.Request["dateto"]).AddDays(1) + "'";
            }





            if (strType == "GetAreaAnalysis")
            {
                ccWhere.Clear();
                // servComm.strOrderString = "sortNo";
                dtResult = servComm.ExecuteSqlDatatable("SELECT area as Name ,cast((select SUM(amount) from OrdersDetail a where a.Order_ID in (select Order_ID from [orders] b where b.Area = c.Area " + permissionCondion + " ))*100/(select SUM(amount) from OrdersDetail a where a.Order_ID in (select Order_ID from [orders] b where 1=1 " + permissionCondion + " ) ) as decimal(10,2) ) as Rate FROM [orders] c  where isnull(Area ,'') <> '' "+permissionCondion+" group by area");
            }
            else if (strType == "GetClassAnalysis")
            {
                if ((bool)context.Session["IsGMP"] == true)
                {
                    dtResult = servComm.ExecuteSqlDatatable("select cast(SUM(b.amount)*100/(select SUM(amount) from OrdersDetail a where a.Order_ID in (select Order_ID from [orders] b where 1=1 " + permissionCondion + " ) ) as decimal(10,2)  ) as Rate,(select SmallClass from ClassSet where ClassID = a.SmallClass ) as Name from products a inner join ordersdetail b on a.id = b.ProductId inner join orders c on b.Order_ID = c.Order_ID and b.Serial = c.Serial    where isnull(a.SmallClass,'')  <>'' " + permissionCondion + " group by a.SmallClass");
                }
                else
                {
                    dtResult = servComm.ExecuteSqlDatatable("select cast(SUM(b.amount)*100/(select SUM(amount) from OrdersDetail a where a.Order_ID in (select Order_ID from [orders] b where 1=1 " + permissionCondion + " ) ) as decimal(10,2)  ) as Rate,a.SmallClass as Name from products a inner join ordersdetail b on a.id = b.ProductId inner join orders c on b.Order_ID = c.Order_ID and b.Serial = c.Serial    where isnull(a.SmallClass,'')  <>'' " + permissionCondion + " group by a.SmallClass");
                }
            }
            else if (strType == "GetProductAnalysis")
            {
                string classname = context.Request["SmallClass"];
                dtResult = servComm.ExecuteSqlDatatable("select cast(SUM(b.amount)*100/(select SUM(amount) from OrdersDetail a where a.Order_ID in (select Order_ID from [orders] b where 1=1" + permissionCondion + " )) as decimal(10,2)  ) as Rate,a.itemname as Name from products a inner join ordersdetail b on a.id = b.ProductId inner join orders c on b.Order_ID = c.Order_ID and b.Serial = c.Serial  where isnull(a.itemname,'') <> '' "+ permissionCondion+" group by a.itemname");
            }


            StringBuilder strClass = new StringBuilder();
            if (dtResult != null)
            {
                strClass.Append("[");
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    strClass.Append("{");
                    strClass.Append("\"Name\":\"" + dtResult.Rows[i]["Name"].ToString() + "\",");

                    strClass.Append("\"Value\":\"" + dtResult.Rows[i]["Rate"].ToString() +"\"");



                    if (i != dtResult.Rows.Count - 1)
                    {
                        strClass.Append("},");
                    }
                }

                strClass.Append("}");
                strClass.Append("]");
                returnValue = strClass.ToString();
            }
        }
        catch(Exception ex)
        {
            GeeStar.Workflow.Common.Log.LogError(ex.Message, ex);
        }

        context.Response.ContentType = "application/json";
        context.Response.ContentEncoding = Encoding.UTF8;
        context.Response.Write(returnValue);
        context.Response.End();

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}

static class ToStringExt
{
    public static string ToStringExtention(this string stirngObject){
        if (stirngObject == null)
        {
            return "";
        }
        else {
            return stirngObject.ToString();
        }
    }

    public static string ToStringExtention(this int intValue)
    {
        if (intValue == null)
        {
            return "";
        }
        else
        {
            return intValue.ToString();
        }
    }

    public static string ToStringExtention(this decimal decimalValue)
    {
        if (decimalValue == null)
        {
            return "";
        }
        else
        {
            return decimalValue.ToString();
        }
    }
}



