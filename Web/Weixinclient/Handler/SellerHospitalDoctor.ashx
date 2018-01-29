<%@ WebHandler Language="C#" Class="SellerHospitalDoctor" Debug="true"%>

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

public class SellerHospitalDoctor : IHttpHandler, System.Web.SessionState.IRequiresSessionState{
    
        ServiceCommon servComm = new ServiceCommon();
    
        ConditionComponent ccWhere = new ConditionComponent();
        public string factoryConnectionString = "";
        public void ProcessRequest(HttpContext context)
        {
            factoryConnectionString = context.Session["factoryConnectionString"].ToString();
            GeeStar.Workflow.Common.Log.LogInfo("Upload");
            try
            {
                string returnValue = "";
                string IsCN = context.Request["IsCN"];
                servComm.strConnectionString = factoryConnectionString;
                string strAction = context.Request["Action"];
                
                string strType = context.Request["ddlType"];
                string subID = context.Request["subID"];
              
                    string strParaValue = context.Request["ddlId"];
                   
                   
                    DataTable dtProcess = null;
                    servComm.strConnectionString = factoryConnectionString;
                    servComm.strOrderString = " NameCN collate Chinese_PRC_CS_AS_KS_WS ";
                    if (strType == "Seller")
                    {
			            servComm.strOrderString = " Seller collate Chinese_PRC_CS_AS_KS_WS ";
                         dtProcess = servComm.GetListTop(0, " id,Seller as NameCN,Seller as NameEN ", "seller", null);
                    }
                    else if (strType == "Process")
                    {
			            servComm.strOrderString = " hospital collate Chinese_PRC_CS_AS_KS_WS ";
                        ccWhere.Clear();
                        ccWhere.AddComponent("sellerid", strParaValue, SearchComponent.Equals, SearchPad.NULL);
                        dtProcess = servComm.GetListTop(0, " id,hospital as NameCN ,hospital as NameEN", "Hospital", ccWhere);
                    }
                    else
                    {
			            servComm.strOrderString = " doctor collate Chinese_PRC_CS_AS_KS_WS ";
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
                            if (strType == "Seller")
                            {
                                strClass.Append("\"Cname\":\"" + (IsCN == "False" ? dtProcess.Rows[i]["NameEN"].ToString() : dtProcess.Rows[i]["NameCN"].ToString()) + "\"");
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
            catch(Exception ex)
            {
                 GeeStar.Workflow.Common.Log.LogError(ex.Message, ex);
            }
            finally
            {
                context.Response.End();
            }

        }

       
         
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }


       
