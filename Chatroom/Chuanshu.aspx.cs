using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Chuanshu : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }
    
    protected void SaveBtn_Click(object sender, EventArgs e)
    {
        
            String username,FileTitle1;
            username = Accept_User.Text;
            FileTitle1 = FileTitle.Text;
            String Send_user1 = Send_User.Text;
            //String TitleStr = Send_user1 + "To" + username;
            String TimeNow = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
            MyUpload MyUpload = new MyUpload();
            MyUpload.Sizes = 2048;
            string Pic1 = "File/";
            //Cnn = new SqlConnection(ConfigurationSettings.AppSettings["connection"]);
            //Cnn.Open();
            MyUpload.Path = "File";
            MyUpload.Sizes = 2048;
            MyUpload.FileType = "jpg|gif|bmp|JPG|GIF|BMP|doc|DOC|xls|XLS|FLV|flv|SWF|swf";

            MyUpload.PostedFile = FileUpload1.PostedFile;
           String Pic2 = MyUpload.Upload();
            if (Pic2 == null)
            {
                Page.RegisterStartupScript("", "<script>alert('上传失败!')</script>");
                Response.AddHeader("Refresh", "0.0001");
            }
            else
            {
                Pic2 = Pic1 + Pic2;
                String Sql = "insert into SendFile(Accept_UserName,Send_UserName,Send_File,Send_Time,Send_FileName) values('" + username + "','" + Send_user1 + "','" + Pic2 + "','" + TimeNow + "','" + FileTitle1 + "')";
                if (db.insert(Sql))
                    Page.RegisterStartupScript("", "<script>alert('上传成功!')</script>");
                Accept_User.Text = "";
                Send_User.Text = "";
            }

       
    }
    protected void C_Set_Click(object sender, EventArgs e)
    {
        Accept_User.Text = "";
        Send_User.Text = "";
    }
}
