using System;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.Security;

namespace book09
{
	[
	ParseChildren(true),
	Description("Login Control"),
	Designer(typeof(LoginControlDesigner)),
	ToolboxData("<{0}:LoginCustomControl runat=server></{0}:LoginCustomControl>")
	]
	/// <summary>
	/// LoginControl ��ժҪ˵����
	/// </summary>
	public class LoginCustomControl : System.Web.UI.WebControls.WebControl, INamingContainer	
	{
		public static string USERNAME_STRING = "�û���:";
		public static string PASSWORD_STRING = "��&nbsp;&nbsp;��:";//�������м����ո�
		public static string LOGIN_STRING = "��¼";
		public static string RESET_STRING = "����";
		public static string CAPTION_STRING = "�û���¼";
		public static string REGISTER_STRING = "ע�����û�";

		private TextBox tbUserName;
		private TextBox tbPassword;
		private Button btnLogin;
		private Button btnReset;
		private LiteralControl vUserName;
		private LiteralControl vPassword;
		private LinkButton lbRegister;
		private bool _show_register;

		public event EventHandler Login;
		public event EventHandler Reset;
		public event EventHandler Register;

		protected void OnLogin(EventArgs e)
		{
			if (Login != null)
			{
				Login(this, e);
			}
		}

		protected void OnReset(EventArgs e)
		{
			if (Reset != null)
			{
				Reset(this, e);
			}
		}

		protected void OnRegister(EventArgs e)
		{
			if (Register != null)
			{
				Register(this, e);
			}
		}

		public string UserName 
		{			
			get 
			{
				this.EnsureChildControls();
				return tbUserName.Text;
			}
			set 
			{
				this.EnsureChildControls();
				tbUserName.Text = value;
			}
		}

		public string Password 
		{
			get 
			{
				this.EnsureChildControls();
				return tbPassword.Text;
			}
			set 
			{
				this.EnsureChildControls();
				tbPassword.Text = value.ToString();
			}
		}

		[
		Browsable(true),
		Category("Behaviour"),
		Description("�Ƿ���ʾע�����û�����")
		]
		public bool ShowRegister
		{
			get
			{
				return _show_register;
			}
			set
			{
				_show_register = value;
			}
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls ();

			//�ı�
			string str = @"<table align=""center"" class=""tableBorder"" cellspacing=""1"" cellpadding=""3"" width=""300"">";
			str += @"<tr><td class=""column"" align=""left"" colspan=""2"">" + CAPTION_STRING + "</td></tr>";
			str += @"<tr><td class=""f"" align=""center"" valign=""top"" colspan=""2"">";
			str += @"<table cellspacing=""1"" border=""0"" cellpadding=""2"">";
			str += @"<tr><td align=""right"" class=""txt3"">";
			str += USERNAME_STRING;
			str += "</td><td>";
			this.Controls.Add(new LiteralControl(str));			

			//�û��������
			tbUserName = new TextBox();
			tbUserName.Text = "";
			tbUserName.Width = Unit.Point(80);
			tbUserName.MaxLength = 64;
			tbUserName.Attributes.Add("size", "11");
			this.Controls.Add(tbUserName);

			//��֤
			vUserName = new LiteralControl("<font color=red>*</font>");
			vUserName.Visible = false;
			this.Controls.Add(vUserName);

			//�ı�
			str = @"</td></tr><tr><td align=""right"" class=""txt3"">";
			str += PASSWORD_STRING;
			str += "</td><td>";
			this.Controls.Add(new LiteralControl(str));

			//���������
			tbPassword = new TextBox();
			tbPassword.Text = "";
			tbPassword.Width = Unit.Point(80);
			tbPassword.MaxLength = 64;
			tbPassword.Attributes.Add("size", "11");
			tbPassword.TextMode = TextBoxMode.Password;
			this.Controls.Add(tbPassword);

			//��֤
			vPassword = new LiteralControl("<font color=red>*</font>");
			vPassword.Visible = false;
			this.Controls.Add(vPassword);

			//�ı�
			str = @"</td></tr><tr><td align=""right"" colspan=""2"">";
			this.Controls.Add(new LiteralControl(str));

			//ע�����û�����
			if (ShowRegister)
			{
				lbRegister = new LinkButton();
				lbRegister.Text = REGISTER_STRING;
				lbRegister.ToolTip = REGISTER_STRING;
				lbRegister.Click +=new EventHandler(lbRegister_Click);
				this.Controls.Add(lbRegister);

				//�ı�
				str = "&nbsp;&nbsp;";
				this.Controls.Add(new LiteralControl(str));
			}

			//��½��ť
			btnLogin = new Button();
			btnLogin.Text = LOGIN_STRING;
			btnLogin.ToolTip = LOGIN_STRING;
			btnLogin.Click +=new EventHandler(btnLogin_Click);
			this.Controls.Add(btnLogin);

			//�ı�
			str = "&nbsp;&nbsp;";
			this.Controls.Add(new LiteralControl(str));

			//���ð�ť
			btnReset = new Button();
			btnReset.Text = RESET_STRING;
			btnReset.ToolTip = RESET_STRING;
			btnReset.Click +=new EventHandler(btnReset_Click);
			this.Controls.Add(btnReset);

			//�ı�
			str = "</td></tr></table></td></tr></table>";
			this.Controls.Add(new LiteralControl(str));
		}

		private void btnLogin_Click(object sender, EventArgs e)
		{
			bool b = true;
			if (UserName.Trim() == "")
			{
				vUserName.Visible = true;
				b = false;
			}
			if (Password.Trim() == "")
			{
				vPassword.Visible = true;
				b = false;
			}
			if (b)
			{
				OnLogin(EventArgs.Empty);
			}
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			this.UserName = "";
			this.Password = "";
			OnReset(EventArgs.Empty);
		}

		private void lbRegister_Click(object sender, EventArgs e)
		{
			OnRegister(EventArgs.Empty);
		}
	}

	/// <summary>
	/// �������
	/// </summary>
	public class LoginControlDesigner : ControlDesigner
	{
		public override string GetDesignTimeHtml()
		{
			return @"<h3  style=""COLOR: dodgerblue"">Login Control</h3>";
		}
	}
}
