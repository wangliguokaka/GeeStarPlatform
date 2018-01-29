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
public partial class OrderManagement_addProduct : PageBase
{
    ServiceCommon servComm = new ServiceCommon();
    ServiceCommon facservComm;
    
    ConditionComponent ccWhere = new ConditionComponent();
    protected WORDERSDETAIL worderDetail = new WORDERSDETAIL();
    protected string pid;
    protected string subID;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        facservComm = new ServiceCommon(factoryConnectionString);
        BindSmallClass();
        subID = Request["subid"];
        if (subID == "" || subID == "0")
        {

        }
        else {
            List<WORDERSDETAIL>  listOrders = (List<WORDERSDETAIL>)Session["productList"];
            worderDetail = listOrders[int.Parse(subID)-1];
            if (!String.IsNullOrEmpty(worderDetail.SmallClass))
            {
                ddlSmallClass.SelectedValue = worderDetail.SmallClass;
            }          
            
        }

    }

    private void BindSmallClass()
    {
        //调用Bll层方法，返回流程分组数据       
        DataTable dtSmallClass = facservComm.ExecuteSqlDatatable("select DictName as SmallClass FROM [DictDetail] where ClassID in(select ClassID from [Dict] where MainCLass = 'L' ) order by sortNo");
        ddlSmallClass.DataSource = dtSmallClass;
        ddlSmallClass.DataValueField = "SmallClass";
        ddlSmallClass.DataTextField = "SmallClass";
        ddlSmallClass.DataBind();
        ddlSmallClass.Items.Insert(0, new ListItem("请选择", "-1"));
    }


}
