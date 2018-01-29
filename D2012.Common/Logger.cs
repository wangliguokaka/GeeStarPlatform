using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D2012.DBUtility.Data.Core.SQLCore;
using D2012.Common.DbCommon;
using D2012.Domain.Entities;
using D2012.Domain.Services;
using System.Web;

namespace D2012.Common
{

    public class Logger
    {

        //
        public static void SMSLog(string userid, string username, string phone, string smsmessge, string ip)
        {
            //ServiceCommon servComm = new ServiceCommon();
            //ConditionComponent ccWhere = new ConditionComponent();

            //loan_smslog smslog = new loan_smslog();
            //smslog.smsmessge = smsmessge;
            //smslog.phone = phone;
            //smslog.isdel = 0;
            //smslog.createid = Convert.ToInt32(userid);
            //smslog.createname = username;
            //smslog.createdate = DateTime.Now;
            //smslog.createip = ip;
            //servComm.Add(smslog);
        }
    }
}
