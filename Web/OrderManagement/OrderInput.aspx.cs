using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;

using D2012.Common.DbCommon;
using D2012.Domain.Services;
using D2012.Common;
using D2012.Domain.Entities;
using System.Drawing;
using GeeStar.Workflow.Common;

public partial class OrderManagement_OrderInput : PageBase
{
    ServiceCommon servComm = new ServiceCommon();
    ConditionComponent ccWhere = new ConditionComponent();
    ServiceCommon facComm;   
    protected WORDERS ordreModel = new WORDERS();
    protected string strOtherList = "";
    protected string strPhotoList = "";
    protected string userHospitalID = "";
    protected string userDoctorID = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        
        facComm = new ServiceCommon(factoryConnectionString);
        if (!IsPostBack)
        {
	        if(Request["action"] == null)
            {
                Session["productList"] = null;
            }

            ddlOrderType.DataSource = BindDictClass(facComm, ccWhere, "OrderClass");
            ddlOrderType.DataBind();

            
        }

       
       

        if (Request["action"] == "autoNo")
        {
            DataTable dtNumber = servComm.ExecuteSqlDatatable("select dbo.fn_GetAutoOrderNo(N'" + LoginUser.UserName + "','" + DateTime.Now.ToString("yyMMdd") + "','" + LoginUser.BelongFactory + "')");
            if (dtNumber.Rows.Count > 0)
            {
                Response.Write(dtNumber.Rows[0][0].ToString().Trim());
                Response.End();
            }
            else
            {
                Response.Write("");
                Response.End();
            }
        }


        if (Request["action"] == "checkUnique")
        {
            DataTable dtNumber = servComm.ExecuteSqlDatatable("select ModelNo from W_Orders where ModelNo='" + Request["orderNo"].Replace("'", "") + "' and BelongFactory='"+LoginUser.BelongFactory+"'");
            if (dtNumber.Rows.Count > 0)
            {
                Response.Write("1");
            }
            else
            {
                Response.Write("0");
            }
            Response.End();
        }

        if (Request["action"] == "GetRequireTemplate")
        {
            string temlplate = Request["temlplate"];
            if(String.IsNullOrEmpty(temlplate))
            {
             Response.Write("");
            }
            else
            {
                DataTable dtRequire = servComm.ExecuteSqlDatatable("select RequireTemplateContent from W_RequireTemplate where id =" + temlplate );
                if (dtRequire.Rows.Count > 0)
                {
                    Response.Write(dtRequire.Rows[0][0].ToString());
                }
                else
                {
                    Response.Write("");
                }
            }
            Response.End();
        }

        BindRequireTemplate();
        BindSeller();

        if (String.IsNullOrEmpty(yeyRequest.Params("haddinfo")) && !String.IsNullOrEmpty(Request["ModelNo"])) 
        {
            autoOrderNo.Visible = false;
            autoLable.Visible = false;
            ccWhere.Clear();
            ccWhere.AddComponent("ModelNo", Request["ModelNo"], SearchComponent.Equals, SearchPad.NULL);
            ordreModel = servComm.GetListTop<WORDERS>(1, ccWhere)[0];
           
            if (ordreModel.SellerID!=0)
            {
                this.ddlSeller.SelectedValue = ordreModel.SellerID.ToString();
            }

            ddlOrderType.Value = ordreModel.OrderClass;
            //strOtherList
            ccWhere.Clear();
            ccWhere.AddComponent("ModelNo", Request["ModelNo"], SearchComponent.Equals, SearchPad.NULL);
            IList<WORDERSOTHER> listOther = servComm.GetListTop<WORDERSOTHER>(0, ccWhere);
            foreach (WORDERSOTHER item in listOther)
            {
                strOtherList = strOtherList + ":" + item.Code + "," + item.qty;
            }
            if (strOtherList != "")
            {
                strOtherList = strOtherList.Substring(1);
            }
            servComm.strOrderString = " subId asc ";
            List<WORDERSDETAIL> listOrders = (List<WORDERSDETAIL>)servComm.GetListTop<WORDERSDETAIL>(0, "*", ccWhere);
            foreach (WORDERSDETAIL item in listOrders) {
                string strValue = GetProductName(item.ProductId);
                if (strValue != "")
                {
                    item.ItemName = strValue.Split(',')[0];
                    item.SmallClass = strValue.Split(',')[1];
                }
               
            }
            this.repProductList.DataSource = listOrders;
            this.repProductList.DataBind();
            Session["productList"] = listOrders;
            servComm.strOrderString = "";
            ccWhere.Clear();
            ccWhere.AddComponent("ModelNo", Request["ModelNo"], SearchComponent.Equals, SearchPad.NULL);
            IList<WORDERSPHOTOS> listPhotos = servComm.GetListTop<WORDERSPHOTOS>(0, ccWhere);
            foreach (WORDERSPHOTOS item in listPhotos)
            {
                strPhotoList = strPhotoList + "," + item.picpath;
            }
            if (strPhotoList != "")
            {
                strPhotoList = strPhotoList.Substring(1);
            }

        }       

        if (yeyRequest.Params("haddinfo") == "1")
        {
             
            ordreModel.ModelNo = Request["txtModelNo"];
            ordreModel.OrderClass = Request[this.ddlOrderType.UniqueID];
            ordreModel.SellerID = decimal.Parse(Request[this.ddlSeller.UniqueID]);
            ordreModel.HospitalID = decimal.Parse(Request[this.ddlHosipital.UniqueID]);
            ordreModel.DoctorId = decimal.Parse(Request[this.ddlDoctor.UniqueID]);
            ordreModel.Patient = Request["txtpatient"];
            if (Request["txtAge"] != "")
            {
                ordreModel.Age = decimal.Parse(Request["txtAge"]);
            }
            ordreModel.Sex = Request["ddlSex"];
            ordreModel.danzuo = Request["ddlSingle"];
            ordreModel.Fenge = Request["ddlDivision"];
            ordreModel.Require = Request["Require"].Replace("'", "");
            ordreModel.RegTime = DateTime.Now;
            ordreModel.RegName = LoginUser.UserName;
            ordreModel.BelongFactory = LoginUser.BelongFactory;
            if (!String.IsNullOrEmpty(yeyRequest.Params("keyID")))
            {
                servComm.Update(ordreModel);
            }
            else
            {
                servComm.Add(ordreModel);
            }
           

            string AccessoryList = Request["OtherList"];
            if (AccessoryList != "")
            {
                servComm.ExecuteSql(" delete from W_ordersOther where ModelNo='" + Request["txtModelNo"] + "'and BelongFactory = '" + LoginUser.BelongFactory + "'");
                string[] otherSplit = AccessoryList.Split(':');
                WORDERSOTHER otherModel = new WORDERSOTHER();
                ccWhere.Clear();
                ccWhere.AddComponent("ClassID","Accessory", SearchComponent.Equals, SearchPad.NULL);
                DataTable dtAccessory =  facComm.GetListTop(0, "DictDetail", ccWhere);
                for (int i = 0; i < otherSplit.Length; i++)
                {
                    string accessoryName = "";
                    if (dtAccessory.Select("Code = '" + otherSplit[i].Split(',')[0] + "'").Length >0)
                    {
                        accessoryName = dtAccessory.Select("Code = '" + otherSplit[i].Split(',')[0] + "'")[0]["DictName"].ToString();
                    }
                    otherModel.ModelNo = Request["txtModelNo"];
                    otherModel.SubId = i + 1;
                    otherModel.Code = otherSplit[i].Split(',')[0];
                    otherModel.name = accessoryName;
                    otherModel.qty = decimal.Parse(otherSplit[i].Split(',')[1]);
                    otherModel.BelongFactory = LoginUser.BelongFactory;
                    servComm.Add(otherModel);
                }
            }
            string photoList = Request["photoList"];
            if (photoList != "")
            {
                servComm.ExecuteSql(" delete from W_OrderPhotos where ModelNo='" + Request["txtModelNo"] + "' and BelongFactory = '" + LoginUser.BelongFactory+"'");
                string[] otherSplit = photoList.Split(',');
                WORDERSPHOTOS photoModel = new WORDERSPHOTOS();
                for (int i = 0; i < otherSplit.Length; i++)
                {
                    photoModel.ModelNo = Request["txtModelNo"];
                    photoModel.SubId = i + 1;
                    photoModel.picpath = otherSplit[i].Split(',')[0];
                    photoModel.BelongFactory = LoginUser.BelongFactory;
                    servComm.Add(photoModel);
                }
            }


            int index = 1;
            IList<WORDERSDETAIL>  listOrders = (IList<WORDERSDETAIL>)Session["productList"];

            if (listOrders != null)
            {

                servComm.ExecuteSql(" delete from W_OrdersDetail where ModelNo='" + Request["txtModelNo"] + "'and BelongFactory = '" + LoginUser.BelongFactory + "'");

                foreach (WORDERSDETAIL item in listOrders)
                {

                    item.subId = index;
                    item.ModelNo = Request["txtModelNo"];
                    item.BelongFactory = LoginUser.BelongFactory;
                    servComm.Add(item);
                    index = index + 1;
                }
            }           

            Response.Redirect("OrderList.aspx?type=Order");
        }
    }

  

    protected string GetProductName(object productID)
    {
        if (productID == null)
        {
            return "";
        }
        ServiceCommon facComm = new ServiceCommon(factoryConnectionString);

        ccWhere.Clear();
        ccWhere.AddComponent("id", productID.ToString(), SearchComponent.Equals, SearchPad.NULL);
        DataTable dt = facComm.GetListTop(0, " itemname,SmallClass ", "products", ccWhere);
        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0]["itemname"].ToString() + "," + dt.Rows[0]["SmallClass"].ToString();
        }
        else {
            return "";
        }
       
    }
    

    /// <summary>
    /// 绑定业务员
    /// </summary>
    private void BindSeller()
    {
        //调用Bll层方法，返回流程分组数据       
        DataTable dtSeller = facComm.GetListTop(0, " id,Seller ", "seller", null);
        ddlSeller.DataSource = dtSeller;
        ddlSeller.DataValueField = "ID";
        ddlSeller.DataTextField = "Seller";
        ddlSeller.DataBind();
        if (IsCN)
        {
            ddlSeller.Items.Insert(0, new ListItem("全部", "-1"));
        }
        else
        {
            ddlSeller.Items.Insert(0, new ListItem("All", "-1"));
        }

        if (GetOrganization.Count >0)
        {
            this.ddlSeller.SelectedValue = GetOrganization["sellerid"].ToString();
            this.ddlSeller.Enabled = false;
        }
        if(GetOrganization.Count == 2)
        {
            userHospitalID = GetOrganization["hospitalid"].ToString();
        }
        else if(GetOrganization.Count == 3)
        {
            userHospitalID = GetOrganization["hospitalid"].ToString();
            userDoctorID = GetOrganization["doctorid"].ToString();
        }
    }

    private void BindRequireTemplate()
    {
        DataTable dtRequireTemplate = facComm.ExecuteSqlDatatable("select dictname from dictdetail where classid='Phrase' ");
        ddlRequireTemplate.DataSource = dtRequireTemplate;
        ddlRequireTemplate.DataValueField = "dictname";
        ddlRequireTemplate.DataTextField = "dictname";
        ddlRequireTemplate.DataBind();
        ddlRequireTemplate.Items.Insert(0, new ListItem("", ""));
    }
}
