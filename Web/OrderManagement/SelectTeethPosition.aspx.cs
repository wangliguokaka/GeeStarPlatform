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
public partial class OrderManagement_SelectTeethPositio : PageBase
{
    ServiceCommon servComm = new ServiceCommon();
    ConditionComponent ccWhere = new ConditionComponent();
    protected string pid;
    protected string cid;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["action"] == "savepic")
        {
            int pPartStartPointX = int.Parse(Request["pPartStartPointX"]);
            int pPartStartPointY = int.Parse(Request["pPartStartPointY"]);
            int pPartWidth = int.Parse(Request["pPartWidth"]);
            int pPartHeight = int.Parse(Request["pPartHeight"]);
            string fileName = Request["picfile"].Substring(Request["picfile"].LastIndexOf("/")+1);
            string originalPath = HttpContext.Current.Server.MapPath("~/uploadedFiles/") + fileName;
            string savePath = HttpContext.Current.Server.MapPath("~/uploadedFiles/");
            string filename = ImageHelper.CropImage(originalPath, savePath, pPartWidth, pPartHeight, pPartStartPointX, pPartStartPointY);
            Response.Write(Request.Url.GetLeftPart(UriPartial.Authority) + "/uploadedFiles/" + filename);
            Response.End();
        }       

    }

}
