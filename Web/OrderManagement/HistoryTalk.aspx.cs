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
public partial class OrderManagement_HistoryTalk : PageBase
{
    ServiceCommon servComm = new ServiceCommon();
    ConditionComponent ccWhere = new ConditionComponent();
    protected string pid;
    protected string cid;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (yeyRequest.Params("type") == "GetJson")
        {
            getSceneryList();
        }

    }

    private void getSceneryList()
    {
        int iCount = 6;
        int iPageIndex = yeyRequest.Params("pageindex") == null ? 1 : Convert.ToInt32(yeyRequest.Params("pageindex"));
        int iPageCount = yeyRequest.Params("hPageNum") == null ? 0 : Convert.ToInt32(yeyRequest.Params("hPageNum"));
        ccWhere.Clear();
        ccWhere.AddComponent("OrderNumber", Request["OrderNumber"], SearchComponent.Equals, SearchPad.NULL);
        ccWhere.AddComponent("BelongFactory", LoginUser.BelongFactory, SearchComponent.Equals, SearchPad.And);
        servComm.strOrderString = " ModifyDate desc ";
        DataTable dtTalk = servComm.GetListTop(0, "OrderMessage", ccWhere);

      

        List<string> talkList = new List<string>();
        List<string> talkListTemp = new List<string>();
        List<string> pageTalkList = new List<string>();
        if (CacheHelper.GetInfoByIdentify(LoginUser.BelongFactory + "|" + Request["OrderNumber"]) != null)
        {
            talkListTemp = CacheHelper.GetInfoByIdentify(LoginUser.BelongFactory + "|" + Request["OrderNumber"]);
            //talkListTemp.Reverse();
            talkList.AddRange(talkListTemp.AsEnumerable<string>());
            talkList.Reverse();
        }
        for (int i = 0; i < dtTalk.Rows.Count; i++)
        {
            
            talkListTemp = XmlSerializerHelper.DeserializeObject(dtTalk.Rows[i]["TalkContent"].ToString());
            talkListTemp.Reverse();
            talkList.AddRange(talkListTemp.AsEnumerable<string>());
        }


        pageTalkList = talkList.GetRange((iPageIndex - 1) * iPageCount, (talkList.Count - (iPageIndex-1) * iPageCount) > 10 ? 10 : talkList.Count - (iPageIndex-1) * iPageCount);
        pageTalkList.Reverse();

            if (iPageCount <= 1)
            {
                iPageCount = servComm.PageCount;
                iPageIndex = 1;
            }
        
            int PageCount = (talkList.Count -1) / iPageCount+1;
            string talkJson = "";
            //string backstr = Json.ListToJson("replyJson", talkList, string.Format("\"PageCount\":\"{0}\",", iPageCount)).Replace("\n", "<br/>").Replace("\r", "<br/>");
            for (int i = 0; i < pageTalkList.Count; i++)
            {
                string talkTime = pageTalkList[i].Split('|')[0];
                string talkUser = pageTalkList[i].Split('|')[1];

                string talkMessage = pageTalkList[i].Replace(talkTime + "|" + talkUser + "|", "").Replace("\"", "\\\"").Replace("\n", "").Replace("\r", "");
                talkJson = talkJson + ",{" + "\"TalkMessage\":" + "\"" + talkMessage + "\"" + ",\"TalkTime\":" + "\"" + talkTime + "\"" + ",\"TalkUser\":" + "\"" + talkUser + "\"" + "}";
            }
            if (talkJson == "")
            {
                Response.Write("{\"PageCount\":0}");
                Response.End();
            }
            else {
                talkJson = talkJson.Substring(1);
                talkJson = "{" + "\"PageCount\":" + PageCount + ",\"replyJson\"" + ":[" + talkJson + "]}";
                //if (talkJson.IndexOf("\\") >= 0)
                //{
                //    talkJson = talkJson.Replace("\\", "\\\\");
                //}

                Response.Write(talkJson);
                Response.End();
            }
    }


}
