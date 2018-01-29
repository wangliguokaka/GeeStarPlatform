using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using D2012.Common;

namespace RGPWEB.ascx
{
    public partial class Phonepagecut : System.Web.UI.UserControl
    {   
        public int iPageNum; 
        public bool _bPageClear = false; 
        public int iPageIndex = 1;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (yeyRequest.Params("sPageID") != null) { 
                iPageIndex = int.Parse(yeyRequest.Params("sPageID"));
            }
            if (yeyRequest.Params("hPageNum") != null) { 
                iPageNum = int.Parse(yeyRequest.Params("hPageNum"));
            }
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (bPageClear) { 
                iPageIndex = 1; 
            } 
        }
        public bool bPageClear { 
            get { return _bPageClear; } 
            set { 
                if (value) { 
                    iPageIndex = 1; 
                    iPageNum = 0; 
                } 
                else { 
                    _bPageClear = value; 
                } 
            } 
        } 
        
    }
}
