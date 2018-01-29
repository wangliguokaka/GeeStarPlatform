using D2012.Common.DbCommon;
using D2012.Domain.Entities;
using D2012.Domain.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OrderManagement_FileManageByOrderNo : PageBase
{
    ServiceCommon servComm = new ServiceCommon();
    ConditionComponent ccWhere = new ConditionComponent();
    protected int tcount;
    protected string hddpnumbers;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        { 
            this.btnSave.Text = GetGlobalResourceObject("Resource","Upload").ToString();
        }
        if (!string.IsNullOrEmpty(Request["hDelete"]))
        {
            
            ccWhere.AddComponent("ID",Request["hDelete"], SearchComponent.Equals, SearchPad.NULL);
            DataTable dt = servComm.GetListTop(0, "W_OrderAttachements", ccWhere);
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["FilePath"]!=null && !String.IsNullOrEmpty(dr["FilePath"].ToString()))
                {
                    string strFileName = dr["FilePath"].ToString().Substring(dr["FilePath"].ToString().LastIndexOf("/") + 1);
                    strFileName = Request.PhysicalApplicationPath+SaveFilePath + strFileName;
                    if (File.Exists(strFileName))
                    {                        
                        File.Delete(strFileName);
                    }
                }               
            }
            servComm.ExecuteSql(" delete from W_OrderAttachements where ID='" + Request["hDelete"] + "'");
            BindList();
        }

        BindList();
       
    }

    private void BindList()
    {
        int iCount = 10;
        if (!string.IsNullOrEmpty(hddpnumbers))
        {
            iCount = Convert.ToInt32(hddpnumbers);
        }
        int iPageIndex = string.IsNullOrEmpty(Request["sPageID"]) ? 1 : Convert.ToInt32(Request["sPageID"]);
        int iPageCount = string.IsNullOrEmpty(Request["sPageNum"]) ? 0 : Convert.ToInt32(Request["sPageNum"]);

        if (!String.IsNullOrEmpty(Request["ModelNo"]))
        {
            ccWhere.Clear();
            ccWhere.AddComponent("ModelNo", Request["ModelNo"], SearchComponent.Equals, SearchPad.NULL);
            servComm.strOrderString = " CreateDate desc ";
            IList<WORDERSATTACHMENTS> ilist = servComm.GetList<WORDERSATTACHMENTS>(WORDERSATTACHMENTS.STRTABLENAME, "*", WORDERSATTACHMENTS.STRKEYNAME, iCount, iPageIndex, iPageCount, ccWhere);
            this.repOrderList.DataSource = ilist;
            this.pagecut1.iPageNum = servComm.PageCount;

        }       
        repOrderList.DataBind();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (AttachFile.HasFile)
        {
            string FileName = this.AttachFile.FileName;//获取上传文件的文件名,包括后缀
            string ExtenName = System.IO.Path.GetExtension(FileName);//获取扩展名
            string SaveFileName = System.IO.Path.Combine(
                   Request.PhysicalApplicationPath + SaveFilePath,
                    DateTime.Now.ToString("yyyyMMddhhmmss") + ExtenName);//合并两个路径为上传到服务器上的全路径
            AttachFile.MoveTo(SaveFileName, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
            string url = Request.Url.GetLeftPart(UriPartial.Authority) + SaveFilePath + DateTime.Now.ToString("yyyyMMddhhmmss") + ExtenName;  //文件保存的路径
            float FileSize = (float)System.Math.Round((float)AttachFile.ContentLength / 1024000, 1); //获取文件大小并保留小数点后一位,单位是M

            WORDERSATTACHMENTS model = new WORDERSATTACHMENTS();
            model.ModelNo = Request["ModelNo"];
            model.Filename = FileName;
            model.FilePath = url;
            model.CreateDate = DateTime.Now;
            model.BelongFactory = LoginUser.BelongFactory;
            servComm.Add(model);

            BindList();

        }
    }
}