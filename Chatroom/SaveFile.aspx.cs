using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class SaveFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
            Datalist();

        }
       
    }
    public void Datalist()
    {
        String User;
        User = Session["Username"].ToString();
        string que = "select * from SendFile where Accept_UserName='" + User + "'";
        this.Save_DataGrid.DataSource = db.ds(que);
        this.Save_DataGrid.DataBind();

    }
    protected void Save_DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        Save_DataGrid.CurrentPageIndex = e.NewPageIndex;
        Datalist();
    }
    protected void Save_DataGrid_DeleteCommand(object source, DataGridCommandEventArgs e)
    {
      String str1 = e.Item.Cells[0].Text;
         String sql = "delete from SendFile where id =" + str1;
           if(db.insert(sql))
               Page.RegisterStartupScript("", "<script>alert('删除成功!');</script>");
           Datalist();
    }
}
