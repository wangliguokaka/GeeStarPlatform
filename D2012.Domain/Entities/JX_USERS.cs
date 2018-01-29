using D2012.Common.DbCommon;
using D2012.Domain.Common;
using System;
using System.Collections.Generic;

namespace D2012.Domain.Entities
{
    /// <summary>
    /// 管理角色表
    /// </summary>
    [Serializable]
    [Table("JX_USERS", "ID", false, false)]
    public partial class JX_USERS : BaseEntity
    {
        public JX_USERS()
        { }
        #region Model

        public static string STRTABLENAME = "JX_USERS";
        public static string STRKEYNAME = "ID";


        /// <summary>
        /// 
        /// </summary>
        [Column("ID", DataType.Int, true, true)]
        public int ID { set; get; }

        
        /// <summary>
        /// 
        /// </summary>
        [Column("JGCBM", DataType.Nvarchar, false, false)]
        public string JGCBM { set; get; }
        

        /// <summary>
        /// 
        /// </summary>
        [Column("JGCName", DataType.Nvarchar, false, false)]
        public string JGCName { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("Address", DataType.Nvarchar, false, false)]
        public string Address { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("Contact", DataType.Nvarchar, false, false)]
        public string Contact { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("Email", DataType.Nvarchar, false, false)]
        public string Email { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("ContactQQ", DataType.Nvarchar, false, false)]
        public string ContactQQ { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("WeiXin", DataType.Nvarchar, false, false)]
        public string WeiXin { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("Telephone", DataType.Nvarchar, false, false)]
        public string Telephone { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("DBUser", DataType.Nvarchar, false, false)]
        public string DBUser { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("DBPassword", DataType.Nvarchar, false, false)]
        public string DBPassword { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("DBServerIP", DataType.Nvarchar, false, false)]
        public string DBServerIP { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("StartTime", DataType.Datetime, false, false)]
        public DateTime? StartTime { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("ValidDay", DataType.Int, false, false)]
        public int ValidDay { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("EndTime", DataType.Datetime, false, false)]
        public DateTime? EndTime { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("Remark", DataType.Nvarchar, false, false)]
        public string Remark { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("LastModifyBy", DataType.Nvarchar, false, false)]
        public string LastModifyBy { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("LastModifyTime", DataType.Datetime, false, false)]
        public DateTime? LastModifyTime { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("DBsameJGCBM", DataType.Nvarchar, false, false)]
        public string DBsameJGCBM { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("UserName", DataType.Nvarchar, false, false)]
        public string UserName { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("UserPasswd", DataType.Nvarchar, false, false)]
        public string UserPasswd { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("PayNoCardMerId", DataType.Nvarchar, false, false)]
        public string PayNoCardMerId { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("PayNoCardPass", DataType.Nvarchar, false, false)]
        public string PayNoCardPass { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("PayNoCardSignPath", DataType.Nvarchar, false, false)]
        public string PayNoCardSignPath { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("PayNocardCPPath", DataType.Nvarchar, false, false)]
        public string PayNocardCPPath { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("PayB2CMerId", DataType.Nvarchar, false, false)]
        public string PayB2CMerId { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("PayB2CPass", DataType.Nvarchar, false, false)]
        public string PayB2CPass { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("PayB2CSignPath", DataType.Nvarchar, false, false)]
        public string PayB2CSignPath { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("PayB2CCPPath", DataType.Nvarchar, false, false)]
        public string PayB2CCPPath { set; get; }

        #endregion Model

    }
}