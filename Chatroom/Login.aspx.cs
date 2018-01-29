//===========================================================================
// 此文件是作为 ASP.NET 2.0 Web 项目转换的一部分修改的。
// 类名已更改，且类已修改为从文件“App_Code\Migrated\Stub_Login_aspx_cs.cs”的抽象基类 
// 继承。
// 在运行时，此项允许您的 Web 应用程序中的其他类使用该抽象基类绑定和访问 
// 代码隐藏页。
// 关联的内容页“Login.aspx”也已修改，以引用新的类名。
// 有关此代码模式的更多信息，请参考 http://go.microsoft.com/fwlink/?LinkId=46995 
//===========================================================================
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;

namespace book09
{
	/// <summary>
	/// Login 的摘要说明。
	/// </summary>
	public partial class Migrated_Login : Login
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
		}

		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		protected void LoginCustomControl1_Login(object sender, System.EventArgs e)
		{
			int iRet = -1;
			SqlConnection conn = new SqlConnection(
				ConfigurationSettings.AppSettings["ConnectionString"]);

			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "UserLogin";
			cmd.Parameters.Add("@username", LoginCustomControl1.UserName);
			cmd.Parameters.Add("@password", LoginCustomControl1.Password);

			//存储过程返回值
			SqlParameter paramOut = cmd.Parameters.Add("@RETURN_VALUE", "");
			paramOut.Direction = ParameterDirection.ReturnValue;
            
			conn.Open();
			cmd.ExecuteNonQuery();
			conn.Close();			
			iRet = (int)cmd.Parameters["@RETURN_VALUE"].Value;

			if (iRet == 0) //登录成功
			{
				FormsAuthentication.RedirectFromLoginPage(
					LoginCustomControl1.UserName, false);
                Session["Username"] = LoginCustomControl1.UserName;
			}
			else if (iRet == 1) //密码不正确
			{
				Response.Write("<script>alert('密码不正确')</script>");
			}
			else if (iRet == 2) //新注册用户
			{
				FormsAuthentication.RedirectFromLoginPage(
					LoginCustomControl1.UserName, false);
                Session["Username"] = LoginCustomControl1.UserName;
			}


		}
	}
}
