using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

/// <summary>
/// db 的摘要说明
/// </summary>
public class db
{
    public db()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    //定义了这些供以后要用时再用吧~
    public string c1;
    public string c2;
    public string c3;
    public string c4;
    public string c5;
    public string c6;
    public string c7;
    public string c8;
    public string c9;
    public string c10;
    public string c11;
    public string c12;
    public string c13;
    public string c14;

    //我不太喜欢把ＳＱＬ语句做在内中，感觉不灵活 

    public static SqlConnection con()
    {
        SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["ConnectionString"]);
        return con;
    }
    public static bool insert(string que)
    {
        SqlConnection con = db.con();
        con.Open();
        SqlCommand cmd = new SqlCommand(que, con);
        int count = Convert.ToInt32(cmd.ExecuteNonQuery());
        if (count > 0)
        { return true; }
        else
        { return false; }

    }
    public static DataTable ds(string que)
    {
        SqlConnection con = db.con();
        SqlDataAdapter da = new SqlDataAdapter();
        da.SelectCommand = new SqlCommand(que, con);
        DataSet ds = new DataSet();
        da.Fill(ds, "ConnectionString");
        return ds.Tables["ConnectionString"];
    }
    public static string scr(string que)
    {
        SqlConnection con = db.con();
        con.Open();
        SqlCommand cmd = new SqlCommand(que, con);
        return cmd.ExecuteScalar().ToString();

    }

    
    
//解决后台Session漏洞之一，这里面有以前ASP的思想，晕~~
//  public static string error()
//{
// if (Request.Url.ToString().IndexOf("Default.aspx", 1) < 0 && (Request.UrlReferrer == null))
//{

//      Response.Redirect("Default.aspx");
//  return;

// }
//}

   
}
