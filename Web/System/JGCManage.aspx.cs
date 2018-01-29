using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;

using D2012.Common.DbCommon;
using D2012.Domain.Services;
using D2012.Common;
using D2012.Domain.Entities;
using System.Drawing;
using System.Data.SqlClient;
using System.IO;

public partial class System_JGCManage : PageBase
{
    ServiceCommon servComm = new ServiceCommon();
    ConditionComponent ccWhere = new ConditionComponent();
    ServiceCommon facComm;
    protected JX_USERS jxuserModel = new JX_USERS();
    protected string strOtherList = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if(Request["action"]=="CheckJGCBM")
        {
            string txtJGCBM = Request["JGCBM"];
            ccWhere.AddComponent("JGCBM",txtJGCBM, SearchComponent.Equals, SearchPad.NULL);
            if (!String.IsNullOrEmpty(Request["ID"]))
            {
                ccWhere.AddComponent("ID", Request["ID"], SearchComponent.UnEquals, SearchPad.And);
            }
            if (servComm.GetListTop(1, "JX_USERS", ccWhere).Rows.Count > 0)
            {
                Response.Write("1");
            }
            else
            {
                Response.Write("0");
            }
            Response.End();
        }


        if (Request["action"] == "CheckConnection")
        {
            string txtJGCBM = Request["JGCBM"];
            string txtDBUser = Request["DBUser"];
            string txtDBPassword = Request["DBPassword"];
            string txtDBServerIP = Request["DBServerIP"];
            string strConnection = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", txtDBServerIP, "JJ2011", txtDBUser, txtDBPassword);

            if (Request["SameJGCBM"] == "true")
            {
                strConnection = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", txtDBServerIP, txtJGCBM, txtDBUser, txtDBPassword);
            }
           
            SqlConnection connection = new SqlConnection(strConnection);

            try
            {
                connection.Open();
            }
            catch (Exception)
            {
                Response.Write("1");
                Response.End();
            }

            Response.Write("0");
            Response.End();
            
        }

        if (Request["action"] == "CheckMerID")
        {
            string txtNoCardMerId = Request["NoCardMerId"];
            string txtB2CMerId= Request["B2CMerId"];
            if (!String.IsNullOrEmpty(txtNoCardMerId.Trim()))
            {
                ccWhere.Clear();
                ccWhere.AddComponent("PayNoCardMerId", txtNoCardMerId, SearchComponent.Equals, SearchPad.Ex);
                ccWhere.AddComponent("PayB2CMerId", txtNoCardMerId, SearchComponent.Equals, SearchPad.OrExB);
                if (!String.IsNullOrEmpty(Request["ID"]))
                {
                    ccWhere.AddComponent("ID", Request["ID"], SearchComponent.UnEquals, SearchPad.And);
                }
                if (servComm.GetListTop(1, "JX_USERS", ccWhere).Rows.Count > 0)
                {
                    Response.Write("2");
                    Response.End();
                }
            }

            if (!String.IsNullOrEmpty(txtB2CMerId.Trim()))
            {
                ccWhere.Clear();
                ccWhere.AddComponent("PayNoCardMerId", txtB2CMerId, SearchComponent.Equals, SearchPad.Ex);
                ccWhere.AddComponent("PayB2CMerId", txtB2CMerId, SearchComponent.Equals, SearchPad.OrExB);
                if (!String.IsNullOrEmpty(Request["ID"]))
                {
                    ccWhere.AddComponent("ID", Request["ID"], SearchComponent.UnEquals, SearchPad.And);
                }
                if (servComm.GetListTop(1, "JX_USERS", ccWhere).Rows.Count > 0)
                {
                    Response.Write("3");
                    Response.End();
                }
            }

            Response.Write("0");
            Response.End();
        }

        
        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(Request["ID"]))
            {
                jxuserModel = servComm.GetEntity<JX_USERS>(Request["ID"]);
            }
        }
        if (yeyRequest.Params("haddinfo") == "1")
        {
            string NoCardMerIdPass = Request["txtNoCardMerIdPass"];
            string txtNoCardMerId = Request["txtNoCardMerId"];
            string noCardCPPath = "";
            string noCardSignPath = "";
            if (GetFileExtends(this.NoCardCP.FileName) == "cer" && GetFileExtends(this.NoCardSign.FileName) == "pfx" && this.NoCardCP.HasFile && this.NoCardSign.HasFile && !String.IsNullOrEmpty(txtNoCardMerId) && !String.IsNullOrEmpty(NoCardMerIdPass))
            {

                noCardSignPath = Request.PhysicalApplicationPath + "certs\\" + txtNoCardMerId + "_Sign.pfx";
                noCardCPPath = Request.PhysicalApplicationPath + "certs\\" + txtNoCardMerId + "_Cp.cer";
                this.NoCardCP.SaveAs(noCardCPPath);
                this.NoCardSign.SaveAs(noCardSignPath);
               
                string filePath = Request.PhysicalApplicationPath + "/ChinaPay/" + txtNoCardMerId + "/security.properties";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                else
                {
                    Directory.CreateDirectory(Request.PhysicalApplicationPath + "/ChinaPay/" + txtNoCardMerId);
                    Stream fileStream = File.Create(filePath);
                    fileStream.Dispose();
                    fileStream.Close();
                }
                FileStream stream = new FileStream(filePath, FileMode.Create);//fileMode指定是读取还是写入
                StreamWriter writer = new StreamWriter(stream, Encoding.Default);
                writer.WriteLine("[CHINAPAY]");//写入一行，写完后会自动换行
                writer.WriteLine("sign.file=" + noCardSignPath);
                writer.WriteLine("sign.file.password=" + NoCardMerIdPass);
                writer.WriteLine("sign.cert.type=PKCS12");
                writer.WriteLine("sign.invalid.fields=Signature,CertId");
                writer.WriteLine("verify.file=" + noCardCPPath);
                writer.Write("signature.field=Signature");//写完后不会换行
                writer.Close();//释放内存
                stream.Close();//释放内存

                //保存文件时没有指定编码格式，所以出现了乱码，只要在StreamWriter writer = new StreamWriter(stream, Encoding.Default); 指定了编码为Encoding.Default文件乱码的问题就解决了
            }


            string B2CMerIdPass = Request["txtB2CMerIdPass"];
            string txtB2CMerId = Request["txtB2CMerId"];
            string B2CCPPath = "";
            string B2CSignPath = "";
            if (GetFileExtends(this.B2CCP.FileName) == "cer" && GetFileExtends(this.B2CSign.FileName) == "pfx" && this.B2CCP.HasFile && this.B2CSign.HasFile && !String.IsNullOrEmpty(txtB2CMerId) && !String.IsNullOrEmpty(B2CMerIdPass))
            {

                B2CSignPath = Request.PhysicalApplicationPath + "certs\\" + txtB2CMerId + "_Sign.pfx";
                B2CCPPath = Request.PhysicalApplicationPath + "certs\\" + txtB2CMerId + "_Cp.cer";
                this.B2CCP.SaveAs(B2CCPPath);
                this.B2CSign.SaveAs(B2CSignPath);

                string filePath = Request.PhysicalApplicationPath + "/ChinaPay/" + txtB2CMerId + "/securityb2c.properties";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                else
                {
                    Directory.CreateDirectory(Request.PhysicalApplicationPath + "/ChinaPay/" + txtB2CMerId);
                    Stream fileStream = File.Create(filePath);
                    fileStream.Dispose();
                    fileStream.Close();
                }
                FileStream stream = new FileStream(filePath, FileMode.Create);//fileMode指定是读取还是写入
                StreamWriter writer = new StreamWriter(stream, Encoding.Default);
                writer.WriteLine("[CHINAPAY]");//写入一行，写完后会自动换行
                writer.WriteLine("sign.file=" + B2CSignPath);
                writer.WriteLine("sign.file.password=" + B2CMerIdPass);
                writer.WriteLine("sign.cert.type=PKCS12");
                writer.WriteLine("sign.invalid.fields=Signature,CertId");
                writer.WriteLine("verify.file=" + B2CCPPath);
                writer.Write("signature.field = Signature");//写完后不会换行
                writer.Close();//释放内存
                stream.Close();//释放内存

                //保存文件时没有指定编码格式，所以出现了乱码，只要在StreamWriter writer = new StreamWriter(stream, Encoding.Default); 指定了编码为Encoding.Default文件乱码的问题就解决了
            }
            

            if (!String.IsNullOrEmpty(Request["ID"]))
            {
                jxuserModel.ID = int.Parse(Request["ID"]);
            }
            jxuserModel.JGCBM = Request["txtJGCBM"];
            jxuserModel.JGCName = Request["txtJGCName"];
            jxuserModel.Contact = Request["txtContact"];
            jxuserModel.Address = Request["txtAddress"];
            jxuserModel.ContactQQ = Request["txtContactQQ"];
            jxuserModel.DBPassword = Request["txtDBPassword"];
            jxuserModel.DBUser = Request["txtDBUser"];
            jxuserModel.DBServerIP = Request["txtDBServerIP"];
            jxuserModel.Email = Request["txtEmail"];
            jxuserModel.Telephone = Request["txtTelephone"];
            jxuserModel.WeiXin = Request["txtWeiXin"];
            jxuserModel.Remark = Request["txtRemark"];
            jxuserModel.LastModifyBy = LoginUser.UserName;
            jxuserModel.LastModifyTime = DateTime.Now;

            jxuserModel.UserPasswd = Request["txtUserPasswd"];
            jxuserModel.DBsameJGCBM = Request["SameJGCBM"]=="on"?"Y":"N";

            jxuserModel.PayNoCardMerId = txtNoCardMerId;
            jxuserModel.PayNoCardPass = NoCardMerIdPass;
            jxuserModel.PayNoCardSignPath = noCardSignPath;
            jxuserModel.PayNocardCPPath = noCardCPPath;

            jxuserModel.PayB2CMerId = txtB2CMerId;
            jxuserModel.PayB2CPass = B2CMerIdPass;
            jxuserModel.PayB2CSignPath = B2CSignPath;
            jxuserModel.PayB2CCPPath = B2CCPPath;

            servComm.AddOrUpdate(jxuserModel);            

            Response.Redirect("JGCList.aspx?type=System");
        }
    }

    public static string GetFileExtends(string filename)
    {
        string ext = null;
        if (filename.IndexOf('.') > 0)
        {
            string[] fs = filename.Split('.');
            ext = fs[fs.Length - 1];
        }
        return ext;
    }
}
