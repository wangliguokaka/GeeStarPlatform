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
        try
        {
            string strAction = context.Request["Action"];
            string returnValue = "";
            string strType = context.Request["ddlType"];
            string subID = context.Request["subID"];
            if (strType == "AddProducts" || strType == "RemoveProduct")
            {

                List<WORDERSDETAIL> listOrders = new List<WORDERSDETAIL>();
                if (context.Session["productList"] == null)
                {
                    listOrders = new List<WORDERSDETAIL>();
                }
                else
                {
                    listOrders = (List<WORDERSDETAIL>)context.Session["productList"];
                }
                if (strType == "RemoveProduct")
                {
                    int rowIndex = int.Parse(context.Request["index"])-1;
                    listOrders.RemoveAt(rowIndex);

                }
                else if (strType == "AddProducts")
                {

                    string itemID = context.Request["itemID"];
                    string number = context.Request["number"];
                    //string productclass =  context.Server.UrlDecode(context.Request["productclass"]);
                    string righttop = context.Request["righttop"];
                    string rightbottom = context.Request["rightbottom"];
                    string lefttop = context.Request["lefttop"];
                    string leftbottom = context.Request["leftbottom"];
                    string colorName = context.Request["ColorTypeName"];
                    string PositionList = context.Request["PositionList"];
                    ServiceCommon facComm = new ServiceCommon(factoryConnectionString);

                    PRODUCTS productItem = facComm.GetEntity<PRODUCTS>(itemID);

                    WORDERSDETAIL orderItem = new WORDERSDETAIL();
                    orderItem.ProductId = productItem.ID;
                    orderItem.ItemName = productItem.ItemName;
                    orderItem.SmallClass = productItem.SmallClass;
                    orderItem.Qty = number == "" ? 0 : int.Parse(number);
                    orderItem.Nobleclass = productItem.Nobleclass;
                    orderItem.bColor = colorName;
                    orderItem.a_teeth = righttop;
                    orderItem.b_teeth = lefttop;
                    orderItem.c_teeth = rightbottom;
                    orderItem.d_teeth = leftbottom;
                    orderItem.PositionList = PositionList;
                    if (subID != "0")
                    {
                        listOrders.RemoveAt(int.Parse(subID) - 1);
                        listOrders.Insert(int.Parse(subID) - 1, orderItem);
                    }
                    else
                    {
                        listOrders.Add(orderItem);
                    }


                }
                context.Session["productList"] = listOrders;
                returnValue = BindProducts(listOrders);


                context.Response.ContentType = "text";
                context.Response.ContentEncoding = Encoding.UTF8;
                context.Response.Write(returnValue);
                context.Response.End();
                //HttpContext.Current.ApplicationInstance.CompleteRequest();

            }
            else
            {

                string strParaValue = context.Request["ddlId"];

                string IsCN = context.Request["IsCN"];
                DataTable dtProcess = null;
                servComm.strConnectionString = factoryConnectionString;
                servComm.strOrderString = "";
                if (strType == "BindSmallClass")
                {
                    if ((bool)context.Session["IsGMP"] == true)
                    {
                        servComm.strOrderString = "sortNo";
                        dtProcess = servComm.GetListTop(0, " ClassID as id,SmallClass as NameCN,SmallClass as NameEN ", "ClassSet", ccWhere);
                    }
                    else
                    {
                        ccWhere.Clear();
                        ccWhere.AddComponent("ClassID", "(select ClassID from [Dict] where MainCLass = 'L' )", SearchComponent.In, SearchPad.NULL);
                        servComm.strOrderString = "sortNo";
                        dtProcess = servComm.GetListTop(0, " Code as id,DictName as NameCN,DictName as NameEN ", "DictDetail", ccWhere);
                    }
                   
                }
                else if (strType == "SmallClass")
                {
                    ccWhere.Clear();
                    if ((bool)context.Session["IsGMP"] == true)
                    {
                        string SmallClassID =  ((List<ClassSet>)context.Session["ListClassSet"]).Where(item => item.SmallClass == strParaValue).First().ClassID;
                        ccWhere.AddComponent("SmallClass", SmallClassID, SearchComponent.Equals, SearchPad.NULL);
                    }
                    else
                    {
                        ccWhere.AddComponent("SmallClass", strParaValue, SearchComponent.Equals, SearchPad.NULL);
                    }
                    dtProcess = servComm.GetListTop(0, " id,itemname as NameCN,itemname as NameEN ", "products", ccWhere);
                }
                else if (strType == "ProductLine")
                {
                    ccWhere.Clear();
                    ccWhere.AddComponent("ClassID", "ProductLine", SearchComponent.Equals, SearchPad.NULL);
                    servComm.strOrderString = "sortNo";
                    dtProcess = servComm.GetListTop(0, " Code as id,DictName as NameCN,DictName as NameEN ", "DictDetail", ccWhere);
                }
                else if (strType == "Group")
                {

                    dtProcess = servComm.GetListTop(0, " id,Seller as NameCN,Seller as NameEN ", "seller", null);
                }
                else if (strType == "Process")
                {
                    ccWhere.Clear();
                    ccWhere.AddComponent("sellerid", strParaValue, SearchComponent.Equals, SearchPad.NULL);
                    dtProcess = servComm.GetListTop(0, " id,hospital as NameCN ,hospital as NameEN", "Hospital", ccWhere);
                }
                else
                {
                    ccWhere.Clear();
                    ccWhere.AddComponent("Hospitalid", strParaValue, SearchComponent.Equals, SearchPad.NULL);
                    dtProcess = servComm.GetListTop(0, " id,doctor as NameCN,doctor as NameEN ", "doctor", ccWhere);
                }


                StringBuilder strClass = new StringBuilder();
                if (dtProcess != null)
                {
                    strClass.Append("[");
                    for (int i = 0; i < dtProcess.Rows.Count; i++)
                    {
                        strClass.Append("{");
                        strClass.Append("\"ID\":\"" + dtProcess.Rows[i]["ID"].ToString() + "\",");
                        if (strType == "SmallClass")
                        {
                            strClass.Append("\"Cname\":\"" + (IsCN == "False" ? dtProcess.Rows[i]["NameEN"].ToString() : dtProcess.Rows[i]["NameCN"].ToString() + "--" + dtProcess.Rows[i]["ID"].ToString()) + "\"");

                        }
                        else
                        {
                            strClass.Append("\"Cname\":\"" + (IsCN == "False" ? dtProcess.Rows[i]["NameEN"].ToString() : dtProcess.Rows[i]["NameCN"].ToString()) + "\"");

                        }


                        if (i != dtProcess.Rows.Count - 1)
                        {
                            strClass.Append("},");
                        }
                    }

                    strClass.Append("}");
                    strClass.Append("]");
                    returnValue = strClass.ToString();
                }

                context.Response.ContentType = "application/json";
                context.Response.ContentEncoding = Encoding.UTF8;
                context.Response.Write(returnValue);
                context.Response.End();

            }

        }
        catch(Exception ex)
        {
            GeeStar.Workflow.Common.Log.LogError(ex.Message, ex);
        }
        finally
        {
            context.Response.End();
        }

    }

    private static string BindProducts(List<WORDERSDETAIL> listOrders)
    {
        int index = 1;
        string returnValue = "";
        foreach (WORDERSDETAIL item in listOrders)
        {
            returnValue = returnValue + "<tr><td><a href=\"javascript:void(0)\"  onclick=\"RemoveProducts(" + (index).ToStringExtention() + ");\">删除</a>&nbsp;&nbsp;<a href=\"javascript:void(0)\"  onclick=\"ProductAdd(" + (index).ToStringExtention() + ");\">编辑</a></td><td>" + index.ToString() + "</td><td>"
                + item.ItemName.ToStringExtention() + "</td><td>"+item.Qty.ToStringExtention()+"</td>" +
                "<td style=\"padding:0px 0px 0px 0px;\"><table style=\"width:100%;border-collapse:separate\"  border=\"0\" >"+
                "<tr><td style=\"width:50%;text-align:right;\">" + item.a_teeth + "</td><td>" + item.b_teeth + "</td></tr><tr><td style=\"text-align:right;\">" + item.c_teeth + "</td><td>" + item.d_teeth + "</td></tr></table></td>"
                + "<td>" + item.bColor + "</td><td>" + item.Nobleclass.ToStringExtention() + "</td><td>" + item.ProductId.ToStringExtention() + "</td>";
            index = index + 1;
        }
        return returnValue;
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



