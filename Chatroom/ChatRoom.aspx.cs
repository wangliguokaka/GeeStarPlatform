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

namespace book09
{
	//��Դ��������www.51aspx.com(���������������)
	/// <summary>
	/// ChatRoom ��ժҪ˵����
	/// </summary>
	public partial class ChatRoom : System.Web.UI.Page
	{
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			//ע��Ajax����
			Ajax.Utility.RegisterTypeForAjax(typeof(ChatRoom));
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

		public string UserName
		{
			get
			{
				return User.Identity.Name;
			}
		}

		/// <summary>
		/// ��ȡ����Ϣ��html�ַ���
		/// </summary>
		/// <returns>�ͻ��������html�ַ���</returns>
		[Ajax.AjaxMethod()]
		public string GetNewMsgString()
		{
			string strMsgHTML = "";

			SqlConnection conn = new SqlConnection(
				ConfigurationSettings.AppSettings["ConnectionString"]);

			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "GetNewMsg";
			cmd.Parameters.Add("@username", UserName);
            
			conn.Open();
			using (SqlDataReader dr = cmd.ExecuteReader())
			{
				while (dr.Read())
				{
					if (dr.GetString(1) != "")
					{
						strMsgHTML += string.Format(
							"<span class='chatmsg' style='COLOR: #{0}'>{1}&nbsp;{2}&nbsp;{3}&nbsp;{4}&nbsp;>>&nbsp;{5}</span><br>",
							dr.GetString(5),
							dr.GetString(1),
							TestIsPublic(dr.GetBoolean(6)),
							TestYourself(dr.GetString(2)),
							dr.GetString(4),
							Replace_GTLT(dr.GetString(3)));
					}
					else
					{
						strMsgHTML += string.Format(
							"<span class='chatmsg' style='COLOR: #{0}'>{1}</span><br>",
							dr.GetString(5),
							dr.GetString(3));
					}
				}
			}
			conn.Close();

			SetMsgPos();
			
			return strMsgHTML;
		}

		/// <summary>
		/// �滻�ַ����е�'<','>'�ַ�
		/// </summary>
		/// <param name="strInput">�����ַ���</param>
		/// <returns>�滻����ַ���</returns>
		private string Replace_GTLT(string strInput)
		{
			string strOutput = strInput.Replace("<", "&lt;");
			strOutput = strOutput.Replace(">", "&gt;");
			return strOutput;
		}

		/// <summary>
		/// ����û����Ƿ��ǵ�ǰ��¼���û���
		/// </summary>
		/// <param name="strInput">�û���</param>
		/// <returns>�����滻���û���</returns>
		private string TestYourself(string strInput)
		{
			if (strInput == UserName)
				return "��";
			else
				return strInput;
		}

		private string TestIsPublic(bool IsPublic)
		{
			if (IsPublic)
				return "��";
			else
				return "���ĵض�";
		}

		/// <summary>
		/// ��¼�Ѿ��Ķ�������Ϣid
		/// </summary>
        private void SetMsgPos()
		{
			SqlConnection conn = new SqlConnection(
				ConfigurationSettings.AppSettings["ConnectionString"]);
			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "SetMsgPos";
			cmd.Parameters.Add("@username", UserName);
            
			conn.Open();

			cmd.ExecuteNonQuery();

			conn.Close();
		}

		[Ajax.AjaxMethod()]
		public void SendMsg(string strMsg, string strUserTo, string strColor, string strExpression, bool bIsPublic)
		{
			SqlConnection conn = new SqlConnection(
				ConfigurationSettings.AppSettings["ConnectionString"]);

			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "SendMsg";
			cmd.Parameters.Add("@user_from", UserName);
			cmd.Parameters.Add("@user_to", strUserTo);
			cmd.Parameters.Add("@content", strMsg);
			cmd.Parameters.Add("@expression", strExpression);
			cmd.Parameters.Add("@color", strColor);
			cmd.Parameters.Add("@ispublic", bIsPublic);

			conn.Open();
			cmd.ExecuteNonQuery();
			conn.Close();
		}

		[Ajax.AjaxMethod()]
		public string GetOnlineUserString()
		{
			string strUserlist = "";

			SqlConnection conn = new SqlConnection(
				ConfigurationSettings.AppSettings["ConnectionString"]);

			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "GetOnlineUsers";
            
			conn.Open();
			using (SqlDataReader dr = cmd.ExecuteReader())
			{
				while (dr.Read())
				{
					strUserlist += dr.GetString(1) + ",";
				}
			}
			conn.Close();
			return strUserlist.TrimEnd(',');	
		}

		[Ajax.AjaxMethod()]
		public void Logout()
		{
			SqlConnection conn = new SqlConnection(
				ConfigurationSettings.AppSettings["ConnectionString"]);

			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "UserLogout";
			cmd.Parameters.Add("@username", UserName);

			conn.Open();
			cmd.ExecuteNonQuery();
			conn.Close();

			//FormsAuthentication.SignOut();
			
		}

	}
}
