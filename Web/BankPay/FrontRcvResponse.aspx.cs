using D2012.Common.DbCommon;
using D2012.Domain.Entities;
using D2012.Domain.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FrontRcvResponse : PageBase
{
    protected int tcount;
    protected string hddpnumbers;
    ServiceCommon servComm = new ServiceCommon();
    ConditionComponent ccWhere = new ConditionComponent();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.HttpMethod == "POST")
        {
            // ʹ��Dictionary�������
            Hashtable myMap = new Hashtable();

            NameValueCollection coll = Request.Form;

            string[] requestItem = coll.AllKeys;

            for (int i = 0; i < requestItem.Length; i++)
            {
                myMap.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            if (myMap.ContainsKey("MerId"))
            {
                chinapaysecure.SecssUtil obj = new chinapaysecure.SecssUtil();
                ccWhere.Clear();
                ccWhere.AddComponent("PayNoCardMerId",myMap["MerId"].ToString(), SearchComponent.Equals, SearchPad.NULL);
                int count = servComm.GetCount("JX_USERS",ccWhere);
                if (count>0)
                {
                    obj.init(Request.PhysicalApplicationPath + "ChinaPay/" + myMap["MerId"].ToString() + "/security.properties"); //��ʼ����ȫ�ؼ���
                }
                else
                {
                    //B2C֧��
                    //myMap.Add("BankInstNo", "700000000000010");
                    //myMap.Add("MerId", "481601512177911");
                    obj.init(Request.PhysicalApplicationPath + "ChinaPay/" + myMap["MerId"].ToString() + "/securityb2c.properties"); //��ʼ����ȫ�ؼ���
                }
                obj.verify(myMap);
                // ���ر����в�����UPOG,��ʾServer����ȷ���ս�������,����Ҫ��֤Server�˷��ر��ĵ�ǩ��
                if ("00" == obj.getErrCode())
                {

                    servComm.ExecuteSql("update W_NetPay set PayDateTime = getdate() ,PayStatus = 2 where OrderID = '" + myMap["MerOrderNo"].ToString() + "'");


                    //Response.Write("�̻�����֤���ر���ǩ���ɹ�\n");

                    //�̻��˸��ݷ��ر������ݴ����Լ���ҵ���߼� ,DEMO�˴�ֻ������Ľ��
                    //StringBuilder builder = new StringBuilder();

                    //builder.Append("<tr><td align=\"center\" colspan=\"2\"><b>�̻��˽����������ر��Ĳ����ձ����ʽ������</b></td></tr>");

                    //for (int i = 0; i < requestItem.Length; i++)
                    //{
                    //    builder.Append("<tr><td width=\"30%\" align=\"right\">" + requestItem[i] + "</td><td style='word-break:break-all'>" + Request.Form[requestItem[i]] + "</td></tr>");
                    //}

                    //builder.Append("<tr><td width=\"30%\" align=\"right\">�̻�����֤�������ر��Ľ��</td><td>��֤ǩ���ɹ�.</td></tr>");
                    //Response.Write(builder.ToString());

                }
                else
                {
                    servComm.ExecuteSql("update W_NetPay set PayDateTime = getdate() ,PayStatus = 9 where OrderID = '" + myMap["MerOrderNo"].ToString() + "'");

                    Response.Write("<tr><td width=\"30%\" align=\"right\">�̻�����֤�������ر��Ľ��</td><td>��֤ǩ��ʧ��.</td></tr>");
                }
            }

            
        }

        ccWhere.Clear();
        ccWhere.AddComponent("UserID", CurrentUserID.ToString(), SearchComponent.Equals, SearchPad.NULL);
        if (!String.IsNullOrEmpty(Request["txtOrderNumner"]))
        {
            ccWhere.AddComponent("OrderID", Request["txtOrderNumner"], SearchComponent.Like, SearchPad.And);
        }

        hddpnumbers = Request["hpnumbers"];
        int iCount = 10;
        if (!string.IsNullOrEmpty(hddpnumbers))
        {
            iCount = Convert.ToInt32(hddpnumbers);
        }
        int iPageIndex = string.IsNullOrEmpty(Request["sPageID"]) ? 1 : Convert.ToInt32(Request["sPageID"]);
        int iPageCount = string.IsNullOrEmpty(Request["sPageNum"]) ? 0 : Convert.ToInt32(Request["sPageNum"]);

        servComm.strOrderString = " ID desc ";
        IList<WNetPay> ilist = servComm.GetList<WNetPay>(WNetPay.STRTABLENAME, "*", WNetPay.STRKEYNAME, iCount, iPageIndex, iPageCount, ccWhere);

        repUserList.DataSource = ilist;
        repUserList.DataBind();
        pagecut1.iPageNum = servComm.PageCount;
    }

    protected string Transfer(string transferType, string typeValue)
    {
        if (transferType == "PayMethod")
        {
            if (typeValue == "0")
            {
                return "���֧��";
            }
            else {
                return "B2C֧��";
            }
        }
        else if (transferType == "PayStatus")
        {
            if (typeValue == "0")
            {
                return "����֧��";
            }
            else if (typeValue == "2")
            {
                return "֧���ɹ�";
            }
            else {
                return "֧���쳣";
            }

        }
        else {
            return "";
        }
    }
}
