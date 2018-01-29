using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D2012.Common.DbCommon;
using D2012.Domain.Common;

namespace D2012.Domain.ViewEntities
{
    /// <summary>
    /// 实体类loan_user 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    [Table("LoginInfo", "USERID", true, false)]
    public class LoginInfo : BaseEntity
    {
        /// <summary>
        ///  用户ID
        /// </summary>
        [Column("USERID", DataType.Int, true, true)]
        public int USERID { set; get; }

        /// <summary>
        /// 用户名称
        /// </summary>
        [Column("USERNAME", DataType.Varchar, false, false)]
        public string USERNAME { set; get; }

        /// <summary>
        ///  用户邮箱
        /// </summary>
        [Column("EMAIL", DataType.Varchar, false, false)]
        public string EMAIL { set; get; }

        /// <summary>
        ///  用户邮箱是否通过认证
        /// </summary>    
        [Column("EMAILVALID", DataType.Int, false, false)]
        public int? EMAILVALID { set; get; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Column("PASSWORD", DataType.Int, false, false)]
        public int? PASSWORD { set; get; }

        /// <summary>
        ///  用户头像
        /// </summary>
        [Column("PHOTOPATH", DataType.Varchar, false, false)]
        public string PHOTOPATH { set; get; }

        /// <summary>
        /// 登录状态
        /// </summary>
        [Column("ISLOGIN", DataType.Int, false, false)]
        public int? ISLOGIN { set; get; }

        /// <summary>
        /// 短消息
        /// </summary>
        [Column("MSGCOUNT", DataType.Int, false, false)]
        public int? MSGCOUNT { set; get; }
    }
}

