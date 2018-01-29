using D2012.Common.DbCommon;
using D2012.Domain.Entities;
using D2012.Domain.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using D2012.Common;
using System.Web.UI.HtmlControls;

public partial class System_RoleEdit : PageBase
{
    ServiceCommon servComm = new ServiceCommon();
    ConditionComponent ccWhere = new ConditionComponent();
    protected int tcount;
    protected string hddpnumbers;
    private int roleid = 0;
    protected string txtRoleName = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!String.IsNullOrEmpty(Request["roleid"]))
        {
            roleid = int.Parse(Request["roleid"]);
        }

        if (!IsPostBack)
        {
            

             DataTable dt = GetList(0,"System");

            rptList.DataSource = dt;
            rptList.DataBind();

            ShowInfo(roleid);
        }
        if (!String.IsNullOrEmpty(Request["haddinfo"]))
        {
            DoAdd();
        }
       
      

    }

    #region 增加操作=================================
    private void DoAdd()
    {
        //管理权限
        string navID = "";
        string month = "";
        string txtdatefrom = "";
        string validCheck = "";
        List<MANAGERROLEVALUE> ls = new List<MANAGERROLEVALUE>();
        for (int i = 0; i < rptList.Items.Count; i++)
        {
            string hidLayer = ((HiddenField)rptList.Items[i].FindControl("hidLayer")).Value;
            navID = ((HiddenField)rptList.Items[i].FindControl("hidID")).Value;
            if (hidLayer == "1")
            {
                month = ((HtmlInputText)rptList.Items[i].FindControl("validmonth")).Value;
                txtdatefrom = ((HtmlInputText)rptList.Items[i].FindControl("txtdatefrom")).Value;
                ls.Add(new MANAGERROLEVALUE { NavID = int.Parse(navID), Valid = int.Parse(month), StartTime = DateTime.Parse(txtdatefrom) });
            }
            else 
            {               
                validCheck = ((HtmlInputCheckBox)rptList.Items[i].FindControl("checkValid")).Checked == true?"1":"0";
            }

            if (!String.IsNullOrEmpty(month) && !String.IsNullOrEmpty(txtdatefrom) && hidLayer == "2" && validCheck=="1")
            {
                ls.Add(new MANAGERROLEVALUE { NavID = int.Parse(navID), Valid = int.Parse(month), StartTime = DateTime.Parse(txtdatefrom) });
            }
               
        }

        //if (roleid == 0)
        //{
        //    roleid = servComm.AddOrUpdate(model);
        //}
        //else {
        //    servComm.AddOrUpdate(model);
        //}

        servComm.ExecuteSql(" delete from W_MANAGER_ROLE_VALUE where JGCID = " + roleid.ToString());
        servComm.ExecuteSql(" update JX_USERS set StartTime= '" + txtdatefrom + "',ValidDay=" + month + ",EndTime='" + DateTime.Parse(txtdatefrom).AddMonths(int.Parse(month)) + "' where ID = " + roleid.ToString());
        foreach (MANAGERROLEVALUE modelRole in ls)
        {
            modelRole.JGCID = roleid;
            servComm.Add(modelRole);
        }

        servComm.ExecuteSql(" exec SynchronizeDataBase " + roleid.ToString());

        Response.Redirect("RoleList.aspx?type=System");
    }
    #endregion


    #region 赋值操作=================================
    private void ShowInfo(int roleid)
    {

        
    }
    #endregion


    protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            Literal LitFirst = (Literal)e.Item.FindControl("LitFirst");
            HiddenField hidLayer = (HiddenField)e.Item.FindControl("hidLayer");
            HiddenField hidMonth = (HiddenField)e.Item.FindControl("hidMonth");
            HiddenField hidDatefrom = (HiddenField)e.Item.FindControl("hidDatefrom");
            HiddenField hidNavID = (HiddenField)e.Item.FindControl("hidNavID");

            HtmlInputText validMonth = (HtmlInputText)e.Item.FindControl("validmonth");
            HtmlInputText txtdatefrom = (HtmlInputText)e.Item.FindControl("txtdatefrom");
            HtmlInputCheckBox validCheck = (HtmlInputCheckBox)e.Item.FindControl("checkValid");
            HtmlGenericControl divSet = (HtmlGenericControl)e.Item.FindControl("divSet");

            string LitStyle = "<span style=\"display:inline-block;width:{0}px;\"></span>{1}{2}";
            string LitImg1 = "<span class=\"folder-open\"></span>";
            string LitImg2 = "<span class=\"folder-line\"></span>";

            int classLayer = Convert.ToInt32(hidLayer.Value);
            if (classLayer == 1)
            {
                LitFirst.Text = LitImg1;
                validCheck.Visible = false;
                validMonth.Value = hidMonth.Value;
                txtdatefrom.Value = hidDatefrom.Value;
            }
            else
            {
                divSet.Visible = false;
                LitFirst.Text = string.Format(LitStyle, (classLayer - 2) * 24, LitImg2, LitImg1);
                validCheck.Checked = hidNavID.Value != "";
            }
        }
    }

    /// <summary>
    /// 获取类别列表
    /// </summary>
    /// <param name="parent_id">父ID</param>
    /// <param name="nav_type">导航类别</param>
    /// <returns>DataTable</returns>
    public DataTable GetList(int parent_id, string nav_type)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("select a.*,convert(nvarchar(10),b.StartTime,120) StartTime,b.Valid,b.NavID");
        strSql.Append(" FROM " + "dt_" + "navigation a left join W_MANAGER_ROLE_VALUE b on a.id = b.NavID and JGCID = " + roleid.ToString());
        strSql.Append(" where nav_type='" + nav_type + "'");
        strSql.Append(" order by sort_id asc,a.id desc");
        DataTable oldData = servComm.ExecuteSqlDatatable(strSql.ToString());
        //重组列表
        if (oldData == null)
        {
            return null;
        }
        //创建一个新的DataTable增加一个深度字段
        DataTable newData = new DataTable();
        newData.Columns.Add("id", typeof(int));
        newData.Columns.Add("parent_id", typeof(int));
        newData.Columns.Add("channel_id", typeof(int));
        newData.Columns.Add("class_layer", typeof(int));
        newData.Columns.Add("nav_type", typeof(string));
        newData.Columns.Add("name", typeof(string));
        newData.Columns.Add("title", typeof(string));
        newData.Columns.Add("sub_title", typeof(string));
        newData.Columns.Add("icon_url", typeof(string));
        newData.Columns.Add("link_url", typeof(string));
        newData.Columns.Add("sort_id", typeof(int));
        newData.Columns.Add("is_lock", typeof(int));
        newData.Columns.Add("remark", typeof(string));
        newData.Columns.Add("action_type", typeof(string));
        newData.Columns.Add("is_sys", typeof(int));
        newData.Columns.Add("StartTime", typeof(string));
        newData.Columns.Add("Valid", typeof(string));
        newData.Columns.Add("NavID", typeof(string));
        

        //调用迭代组合成DAGATABLE
        GetChilds(oldData, newData, parent_id, 0);
        return newData;
    }


    /// <summary>
    /// 从内存中取得所有下级类别列表（自身迭代）
    /// </summary>
    private void GetChilds(DataTable oldData, DataTable newData, int parent_id, int class_layer)
    {
        class_layer++;
        DataRow[] dr = oldData.Select("parent_id=" + parent_id);
        for (int i = 0; i < dr.Length; i++)
        {
            //添加一行数据
            DataRow row = newData.NewRow();
            row["id"] = int.Parse(dr[i]["id"].ToString());
            row["parent_id"] = int.Parse(dr[i]["parent_id"].ToString());
            row["channel_id"] = int.Parse(dr[i]["channel_id"].ToString());
            row["class_layer"] = class_layer;
            row["nav_type"] = dr[i]["nav_type"].ToString();
            row["name"] = dr[i]["name"].ToString();
            row["title"] = dr[i]["title"].ToString();
            row["sub_title"] = dr[i]["sub_title"].ToString();
            row["icon_url"] = dr[i]["icon_url"].ToString();
            row["link_url"] = dr[i]["link_url"].ToString();
            row["sort_id"] = int.Parse(dr[i]["sort_id"].ToString());
            row["is_lock"] = int.Parse(dr[i]["is_lock"].ToString());
            row["remark"] = dr[i]["remark"].ToString();
            row["action_type"] = dr[i]["action_type"].ToString();
            row["is_sys"] = int.Parse(dr[i]["is_sys"].ToString());
            row["StartTime"] = dr[i]["StartTime"].ToString();
            row["Valid"] = dr[i]["Valid"].ToString();
            row["NavID"] = dr[i]["NavID"].ToString();

            newData.Rows.Add(row);
            //调用自身迭代
            this.GetChilds(oldData, newData, int.Parse(dr[i]["id"].ToString()), class_layer);
        }
    }
    protected void saveAction_Click(object sender, EventArgs e)
    {
        DoAdd();
    }
}
