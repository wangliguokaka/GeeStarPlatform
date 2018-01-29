using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Weixinclient_AnalysisPie : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((HtmlContainerControl)Master.FindControl("HTitle")).InnerText = IsCN ? "分析表" : "AnalysisTable";
        }
    }
}