using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D2012.DBUtility.Data.Core.SQLCore;
using D2012.Common.DbCommon;
using D2012.Domain.Entities;
using D2012.Domain.Services;
using System.Web;
using D2012.Domain.ViewEntities;

namespace D2012.Common
{
    public class LoginService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strPassword"></param>
        /// <param name="bolAutoSave"></param>
        /// <returns>0:成功 1：失败</returns>
        public static int doLogin(string strUserName, string strPassword, bool bolAutoSave)
        {
            int iRet = doLoginEx(strUserName, EncryptClass.MD5(strPassword), bolAutoSave);
            if (iRet == 0)
            {
                CookieHelper.WriteCookie(HttpContext.Current.Response.Cookies["EMAIL"].ToString(), EncryptClass.Encode(strUserName));
                CookieHelper.WriteCookie(HttpContext.Current.Response.Cookies["PASSWORD"].ToString(), EncryptClass.Encode(strPassword));

                if (bolAutoSave)
                {
                    HttpContext.Current.Response.Cookies["EMAIL"].Expires = DateTime.Now.AddMonths(1);
                    HttpContext.Current.Response.Cookies["PASSWORD"].Expires = DateTime.Now.AddMonths(1);
                }
            }
            return iRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strPassword"></param>
        /// <param name="bolAutoSave"></param>
        /// <returns>0:成功 1：失败</returns>
        public static int doLoginEx(string strUserName, string strPassword, bool bolAutoSave)
        {
            ServiceCommon servComm = new ServiceCommon();
            ConditionComponent condComponent = new ConditionComponent();
            if (strUserName.IndexOf("@") > 0)
            {
                condComponent.AddComponent("UPPER(email)", strUserName.ToUpper(), SearchComponent.Equals, SearchPad.NULL);
            }
            else
            {
                condComponent.AddComponent("UPPER(username)", strUserName.ToUpper(), SearchComponent.Equals, SearchPad.NULL);
            }
            condComponent.AddComponent("password", strPassword, SearchComponent.Equals, SearchPad.And);

            loan_user objUser = servComm.GetEntity<loan_user>(null, condComponent);

            //查询用户大于0
            if (objUser.uid > 0)
            {
                HttpCookie cookie = new HttpCookie("d2012");
                //写入Cookie
                cookie.Values["EMAIL"] = objUser.email.ToString();
                cookie.Values["PASSWORD"] = objUser.password.ToString();

                LOGINUSERINFO loginUser = new LOGINUSERINFO();
                loginUser._USERINFO = objUser;

                HttpContext.Current.Session[UserConstant.SESSION_USERINFO] = loginUser;

                HttpContext.Current.Session["email"] = objUser.email;
                HttpContext.Current.Session["userid"] = objUser.uid;
                HttpContext.Current.Session["emailvalid"] = objUser.emailvalid;
                HttpContext.Current.Session["username"] = objUser.username;
                HttpContext.Current.Session["password"] = objUser.password;
                HttpContext.Current.Session["ulevel"] = objUser.ulevel;


                //更新最后登录时间 最后登录IP 总登录次数
                loan_user luser = servComm.GetEntity<loan_user>(objUser.uid);
                luser.lastloginIP = HttpContext.Current.Request.UserHostAddress;
                luser.lastlogintime = DateTime.Now;
                luser.logintimes = luser.logintimes + 1;
                servComm.Update(luser);
            }
            else
            {
                return 1;
            }
            return 0;
        }


        public static int doAutoLogin(HttpContext context)
        {
            if (HttpContext.Current.Session[UserConstant.SESSION_USERINFO] != null)
            {
                return 0;
            }
            if (context.Request.Cookies["EMAIL"] == null || context.Request.Cookies["PASSWORD"] == null)
            {
                return 1;
            }
            else
            {
                //保存密码 将密码解码后重新登陆一下
                string strName = EncryptClass.Decode(context.Request.Cookies["EMAIL"].Value);
                string strPass = EncryptClass.Decode(context.Request.Cookies["PASSWORD"].Value);

                if ((strName.Trim() != "" && strPass.Trim() != "") && LoginService.doLogin(strName.Trim(), strPass.Trim(), false) == 0)
                {
                    return 0;
                }
            }
            return 1;

        }
    }
}
