//===========================================================================
// ���ļ�����Ϊ ASP.NET 2.0 Web ��Ŀת����һ�����޸ĵġ�
// �����Ѹ��ģ��������޸�Ϊ���ļ���App_Code\Migrated\Stub_Login_aspx_cs.cs���ĳ������ 
// �̳С�
// ������ʱ�������������� Web Ӧ�ó����е�������ʹ�øó������󶨺ͷ��� 
// ��������ҳ��
// ����������ҳ��Login.aspx��Ҳ���޸ģ��������µ�������
// �йش˴���ģʽ�ĸ�����Ϣ����ο� http://go.microsoft.com/fwlink/?LinkId=46995 
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
	/// Login ��ժҪ˵����
	/// </summary>
	public partial class Migrated_Login : Login
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
		}

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
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

			//�洢���̷���ֵ
			SqlParameter paramOut = cmd.Parameters.Add("@RETURN_VALUE", "");
			paramOut.Direction = ParameterDirection.ReturnValue;
            
			conn.Open();
			cmd.ExecuteNonQuery();
			conn.Close();			
			iRet = (int)cmd.Parameters["@RETURN_VALUE"].Value;

			if (iRet == 0) //��¼�ɹ�
			{
				FormsAuthentication.RedirectFromLoginPage(
					LoginCustomControl1.UserName, false);
                Session["Username"] = LoginCustomControl1.UserName;
			}
			else if (iRet == 1) //���벻��ȷ
			{
				Response.Write("<script>alert('���벻��ȷ')</script>");
			}
			else if (iRet == 2) //��ע���û�
			{
				FormsAuthentication.RedirectFromLoginPage(
					LoginCustomControl1.UserName, false);
                Session["Username"] = LoginCustomControl1.UserName;
			}


		}
	}
}
