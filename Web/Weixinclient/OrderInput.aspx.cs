using D2012.Common;
using D2012.Common.DbCommon;
using D2012.Domain.Entities;
using D2012.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using DDTourCommon.WXPay;
using System.Net;
using GeeStar.Workflow.Common;
using System.Web.UI.HtmlControls;

public partial class Weixinclient_OrderInput : PageBase
{
    ServiceCommon servCommfac;
    ConditionComponent ccWhere = new ConditionComponent();
    protected ORDERS orderModel = new ORDERS();
    protected ORDERSDETAIL orderDetail = new ORDERSDETAIL();
    protected PRODUCTS productModel = new PRODUCTS();
    protected string orderID = "1511110001";
    protected string serialID = "0";
    protected string zNodes = "";
    protected string timeStamp = "";
    protected string signalticket = "";
    protected string strPositionM = "";
    protected string itemname = "";
    protected string ProductId = "";
    protected List<string> itemnamelist = new List<string>();
    protected List<string> ProductIdlist = new List<string>();
    protected List<int> positionM = new List<int>();
    protected string userHospitalID = "";
    protected string userDoctorID = "";
    protected string userSellerID = "";
    protected DataTable DTAccessory;
    protected DataTable DTColors;
    protected DataTable dtRequireTemplate;
    protected string AccessoryList;
    ServiceCommon servComm = new ServiceCommon();
    protected WORDERS ordreModel = new WORDERS();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            servCommfac = new ServiceCommon(factoryConnectionString);
            if (!IsPostBack)
            {
                ((HtmlContainerControl)Master.FindControl("HTitle")).InnerText = IsCN? "订单输入":"Order Input";

                ddlOrderType.DataSource = BindDictClass(servCommfac, ccWhere, "OrderClass");
                ddlOrderType.DataBind();
            }
            timeStamp = TenpayUtil.getTimestamp();

            signalticket = GetSignalTicket(timeStamp,Session["APPID"].ToString(), Session["APPSECRET"].ToString());

            if (GetOrganization.Count > 0)
            {
                userSellerID = GetOrganization["sellerid"].ToString(); ;
            }
            if (GetOrganization.Count == 2)
            {
                userHospitalID = GetOrganization["hospitalid"].ToString();
            }
            else if (GetOrganization.Count == 3)
            {
                userHospitalID = GetOrganization["hospitalid"].ToString();
                userDoctorID = GetOrganization["doctorid"].ToString();
            }

            
            GetSceneryTypeList();
            GetColors();
            GetDoctorRequire();
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

            string serverImage = Request["uploadimage"];
            string AccessoryList = Request["AccessoryList"];
            string path = Server.MapPath("~" + SaveFilePath); //网站中有一个 uploadedFiles 文件夹，存储上传来的图片
            string photoList = "";
            //生成文件名（系统要重新生成一个文件名，但注意扩展名要相同。千万不要用中文名称！！！）

            Log.LogInfo(serverImage);
            if (!String.IsNullOrEmpty(serverImage))
            {
                string file = string.Empty;
                string content = string.Empty;
                string strpath = string.Empty;
                string savepath = string.Empty;
                for (int i = 0; i < serverImage.Split(',').Length; i++)
                {
                    string stUrl = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=" + GetAccessToken(Session["APPID"].ToString(), Session["APPSECRET"].ToString()) + "&media_id=" + serverImage.Split(',')[i];

                    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(stUrl);

                    req.Method = "GET";
                    using (WebResponse wr = req.GetResponse())
                    {
                        HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();

                        strpath = myResponse.ResponseUri.ToString();

                        WebClient mywebclient = new WebClient();
                        string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + (new Random()).Next().ToString().Substring(0, 4) + ".jpg";
                        savepath = path + "\\" + filename;

                        try
                        {
                            Log.LogInfo(savepath);
                            mywebclient.DownloadFile(strpath, savepath);
                            System.Drawing.Image img = System.Drawing.Image.FromFile(savepath);

                            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
                            {
                                filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + (new Random()).Next().ToString().Substring(0, 4) + ".png";
                                img.Save(path + "\\" + filename);
                                file = Request.Url.GetLeftPart(UriPartial.Authority) + SaveFilePath + filename;
                            }
                            else
                            {
                                file = Request.Url.GetLeftPart(UriPartial.Authority) + SaveFilePath + filename;
                            }
                                
                            photoList = photoList + "," + file;
                            Log.LogInfo(file);
                        }
                        catch (Exception ex)
                        {
                            Log.LogInfo(ex.Message);
                        }

                    }
                    photoList = photoList.Trim(',');
                }                
            }

            if (yeyRequest.Params("haddinfo") == "1")
            {
                string seller = Request["seller"];
                string hospital = Request["hospital"];
                string doctor = Request["doctor"];
                string patient = Request["patient"];
                ordreModel.ModelNo = Request["txtModelNo"];
                ordreModel.OrderClass = ddlOrderType.Value;
                ordreModel.SellerID = decimal.Parse(seller);
                ordreModel.HospitalID = decimal.Parse(hospital);
                ordreModel.DoctorId = decimal.Parse(doctor);
                ordreModel.Patient = Request["txtpatient"];
                if (Request["txtAge"] != "")
                {
                    ordreModel.Age = decimal.Parse(Request["txtAge"]);
                }
                ordreModel.Sex = Request["ddlSex"];
                ordreModel.danzuo = Request["ddlSingle"];
                ordreModel.Fenge = Request["ddlDivision"];
                if (!String.IsNullOrEmpty(Request["Require"])) 
                {
                    ordreModel.Require = Request["Require"].Replace("'", "");
                }
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


                //string AccessoryList = Request["OtherList"];
                if (AccessoryList != "")
                {
                    servComm.ExecuteSql(" delete from W_ordersOther where ModelNo='" + Request["txtModelNo"] + "'and BelongFactory = '" + LoginUser.BelongFactory + "'");
                    string[] otherSplit = AccessoryList.Split(':');
                    WORDERSOTHER otherModel = new WORDERSOTHER();
                    ccWhere.Clear();
                    ccWhere.AddComponent("ClassID", "Accessory", SearchComponent.Equals, SearchPad.NULL);
                    DataTable dtAccessory = servCommfac.GetListTop(0, "DictDetail", ccWhere);
                    for (int i = 0; i < otherSplit.Length; i++)
                    {
                        string accessoryName = "";
                        if (dtAccessory.Select("Code = '" + otherSplit[i].Split(',')[0] + "'").Length > 0)
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
              
                if (photoList != "")
                {
                    servComm.ExecuteSql(" delete from W_OrderPhotos where ModelNo='" + Request["txtModelNo"] + "' and BelongFactory = '" + LoginUser.BelongFactory + "'");
                    string[] photoSplit = photoList.Split(',');
                    WORDERSPHOTOS photoModel = new WORDERSPHOTOS();
                    for (int i = 0; i < photoSplit.Length; i++)
                    {
                        photoModel.ModelNo = Request["txtModelNo"];
                        photoModel.SubId = i + 1;
                        photoModel.picpath = photoSplit[i];
                        photoModel.BelongFactory = LoginUser.BelongFactory;
                        servComm.Add(photoModel);
                    }
                }

                string[] ddlSmallClass = Request["ddlSmallClass"].Split(',');
                string[] ddlItemName = Request["ddlItemName"].Split(',');
                string[] righttop = Request["righttop"].Split(',');
                string[] lefttop = Request["lefttop"].Split(',');
                string[] rightbottom = Request["rightbottom"].Split(',');
                string[] leftbottom = Request["leftbottom"].Split(',');
                string[] ProductColor = Request["hidProductColor"].Split(',');
                string[] ProductCount = Request["ProductCount"].Split(',');

                servComm.ExecuteSql(" delete from W_OrdersDetail where ModelNo='" + Request["txtModelNo"] + "'and BelongFactory = '" + LoginUser.BelongFactory + "'");
                int index = 1;
                for (int i = 0; i < ddlSmallClass.Length; i++)
                {
                    WORDERSDETAIL detailMode = new WORDERSDETAIL();
                    detailMode.a_teeth = righttop[i];
                    detailMode.b_teeth = lefttop[i];
                    detailMode.c_teeth = rightbottom[i];
                    detailMode.d_teeth = leftbottom[i];
                    detailMode.ProductId = ddlItemName[i];
                    detailMode.bColor = ProductColor[i].Replace(":",",");
                    detailMode.Qty = int.Parse(ProductCount[i]);
                    detailMode.subId = index;
                    detailMode.ModelNo = Request["txtModelNo"];
                    detailMode.BelongFactory = LoginUser.BelongFactory;
                    servComm.Add(detailMode);
                    index = index + 1;
                }                   

                Response.Redirect("OrderInput.aspx");
            }
            

        }
        catch (Exception ex)
        {

        }

    }

    private void GetSceneryTypeList()
    {
        
        DTAccessory = servCommfac.ExecuteSqlDatatable("SELECT [Code] ,[DictName] as Accessory FROM [dbo].[DictDetail] where ClassID = 'Accessory' order by cast([Code] as int)");
    }

    private void GetColors()
    {
        DTColors = servCommfac.ExecuteSqlDatatable("SELECT [DictName] as ColorType FROM [DictDetail] where ClassID  = 'Color' order by sortno");
    }

    private void GetDoctorRequire()
    {
        dtRequireTemplate = servCommfac.ExecuteSqlDatatable("select dictname from dictdetail where classid='Phrase' ");
    }
  
}

 