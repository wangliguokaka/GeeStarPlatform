using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D2012.Common.DbCommon;
using D2012.Domain.Common;

namespace D2012.Domain.Entities
{
    /// <summary>
    /// loan_user:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    [Table("loan_user", "uid", true, false)]
    public partial class loan_user : BaseEntity
    {
        public loan_user()
        { }
        #region Model
        public static string STRTABLENAME = "loan_user";
        public static string STRKEYNAME = "uid";

        /// <summary>
        /// 主键ID
        /// </summary>
        [Column("UID", DataType.Int, true, true)]
        public int uid { set; get; }


        /// <summary>
        /// 公司ID : 1：大连 2：北京 3：烟台

        /// </summary>
        [Column("companyid", DataType.Int, false, false)]
        public int? companyid { set; get; }

        /// <summary>
        /// 所属城市

        /// </summary>
        [Column("CITYID", DataType.Int, false, false)]
        public int? cityid { set; get; }

        /// <summary>
        /// 区域
        /// </summary>
        [Column("AREAID", DataType.Int, false, false)]
        public int? areaid { set; get; }

        /// <summary>
        /// 部门
        /// </summary>
        [Column("DEPARTMENTID", DataType.Int, false, false)]
        public int? departmentid { set; get; }

        /// <summary>
        /// 所属理财经理ID
        /// </summary>
        [Column("BELONGLICAIID", DataType.Int, false, false)]
        public int? belongLicaiid { set; get; }

        /// <summary>
        /// 理财经理名称
        /// </summary>
        [Column("LICAIJINGLI", DataType.Varchar, false, false)]
        public string licaijingli { set; get; }

        /// <summary>
        /// 所属借款经理ID
        /// </summary>
        [Column("BELONGJIEKUANID", DataType.Int, false, false)]
        public int? belongjiekuanid { set; get; }

        /// <summary>
        /// 借款经理名称
        /// </summary>
        [Column("JIEKUANJINGLI", DataType.Varchar, false, false)]
        public string jiekuanjingli { set; get; }

        /// <summary>
        /// 是否是线下业务客户：0、线上注册会员 1、线下业务客户

        /// </summary>
        [Column("ISOFFLINE", DataType.Int, false, false)]
        public int? ISOFFLINE { set; get; }

        /// <summary>
        /// 数据是否提供给客服：0、数据不提供给客服 1、数据提供给客服
        /// </summary>
        [Column("TOKEFU", DataType.Int, false, false)]
        public int? tokefu { set; get; }

        /// <summary>
        /// 会员级别：0、普通用户 1、VIP用户
        /// </summary>
        [Column("VIP", DataType.Int, false, false)]
        public int? VIP { set; get; }

        /// <summary>
        /// 会员截至日期
        /// </summary>
        [Column("VDEADLINE", DataType.Datetime, false, false)]
        public DateTime? Vdeadline { set; get; }

        /// <summary>
        /// 当前启用状态 0、未启用 1、启用

        /// </summary>
        [Column("VSTATUS", DataType.Int, false, false)]
        public int? Vstatus { set; get; }

        /// <summary>
        /// 推广人

        /// </summary>
        [Column("INVITE", DataType.Varchar, false, false)]
        public string invite { set; get; }

        /// <summary>
        /// 用户登录名称
        /// </summary>
        [Column("USERNAME", DataType.Varchar, false, false)]
        public string username { set; get; }

        /// <summary>
        /// 密码
        /// </summary>
        [Column("PASSWORD", DataType.Varchar, false, false)]
        public string password { set; get; }

        /// <summary>
        /// 逾期次数
        /// </summary>
        [Column("STALEDATED", DataType.Int, false, false)]
        public int? staledated { set; get; }

        /// <summary>
        /// 严重逾期次数
        /// </summary>
        [Column("YZSTALEDATED", DataType.Int, false, false)]
        public int? yzstaledated { set; get; }

        /// <summary>
        /// 逾期金额
        /// </summary>
        [Column("YQSUM", DataType.Decimal, false, false)]
        public decimal? yqsum { set; get; }

        /// <summary>
        /// 系统分配分数
        /// </summary>
        [Column("PSCORE", DataType.Int, false, false)]
        public int? pscore { set; get; }

        /// <summary>
        /// 用户级别
        /// </summary>
        [Column("PLEVEL", DataType.Int, false, false)]
        public int? plevel { set; get; }

        /// <summary>
        /// 系统分配人ID
        /// </summary>
        [Column("PCREATEID", DataType.Int, false, false)]
        public int? pcreateid { set; get; }

        /// <summary>
        /// 系统分配时间
        /// </summary>
        [Column("PCREATEDATETIME", DataType.Datetime, false, false)]
        public DateTime? pcreatedatetime { set; get; }

        /// <summary>
        /// 系统分配人名称

        /// </summary>
        [Column("PCREATENAME", DataType.Varchar, false, false)]
        public string pcreatename { set; get; }

        /// <summary>
        /// 系统分配人IP
        /// </summary>
        [Column("PCREATEIP", DataType.Varchar, false, false)]
        public string pcreateip { set; get; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Column("EMAIL", DataType.Varchar, false, false)]
        public string email { set; get; }

        /// <summary>
        /// 邮箱是否验证
        /// </summary>
        [Column("EMAILVALID", DataType.Int, false, false)]
        public int? emailvalid { set; get; }

        /// <summary>
        /// 邮箱验证码

        /// </summary>
        [Column("EMAILCODE", DataType.Varchar, false, false)]
        public string emailcode { set; get; }

        /// <summary>
        /// 验证日期
        /// </summary>
        [Column("EMAILDATE", DataType.Datetime, false, false)]
        public DateTime? emaildate { set; get; }

        /// <summary>
        /// 找回密码code
        /// </summary>
        [Column("FINDPWDCODE", DataType.Varchar, false, false)]
        public string findpwdcode { set; get; }

        /// <summary>
        /// 找回密码验证时间
        /// </summary>
        [Column("FINDPWDTIME", DataType.Datetime, false, false)]
        public DateTime? findpwdtime { set; get; }

        /// <summary>
        /// 头像地址
        /// </summary>
        [Column("HEADURL", DataType.Varchar, false, false)]
        public string headurl { set; get; }

        /// <summary>
        /// 信用等级
        /// </summary>
        [Column("ULEVEL", DataType.Int, false, false)]
        public int? ulevel { set; get; }

        /// <summary>
        /// 信用分数
        /// </summary>
        [Column("CREDITMARK", DataType.Int, false, false)]
        public int? creditmark { set; get; }

        /// <summary>
        /// 信用额度
        /// </summary>
        [Column("CREDITLIMIT", DataType.Int, false, false)]
        public int? creditlimit { set; get; }

        /// <summary>
        /// 信用额度截至日期
        /// </summary>
        [Column("CREDITENDDATEIME", DataType.Datetime, false, false)]
        public DateTime? creditenddateime { set; get; }

        /// <summary>
        /// 信用余额
        /// </summary>
        [Column("CREDITBALANCE", DataType.Int, false, false)]
        public int? creditbalance { set; get; }

        /// <summary>
        /// 帐号余额
        /// </summary>
        [Column("ACCOUNTBALANCE", DataType.Decimal, false, false)]
        public decimal? accountbalance { set; get; }

        /// <summary>
        /// 冻结资金
        /// </summary>
        [Column("ACCOUNTFREEZE", DataType.Decimal, false, false)]
        public decimal? accountfreeze { set; get; }

        /// <summary>
        /// 用户登录次数
        /// </summary>
        [Column("LOGINTIMES", DataType.Int, false, false)]
        public int? logintimes { set; get; }

        /// <summary>
        /// 最后登录次数

        /// </summary>
        [Column("LASTLOGINTIME", DataType.Datetime, false, false)]
        public DateTime? lastlogintime { set; get; }

        /// <summary>
        /// 最后登录IP
        /// </summary>
        [Column("LASTLOGINIP", DataType.Varchar, false, false)]
        public string lastloginIP { set; get; }

        /// <summary>
        /// 用户提现银行
        /// </summary>
        [Column("TIXIANYINHANG", DataType.Varchar, false, false)]
        public string tixianyinhang { set; get; }

        /// <summary>
        /// 用户提现账号
        /// </summary>
        [Column("TIXIANCARDNUM", DataType.Varchar, false, false)]
        public string tixiancardnum { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("ISDEL", DataType.Int, false, false)]
        public int? isdel { set; get; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        [Column("CREATEID", DataType.Int, false, false)]
        public int? createid { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("CREATEDATE", DataType.Datetime, false, false)]
        public DateTime? createdate { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("CREATEIP", DataType.Varchar, false, false)]
        public string createip { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("UPDATEID", DataType.Int, false, false)]
        public int? updateid { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("UPDATEDATE", DataType.Datetime, false, false)]
        public DateTime? updatedate { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("UPDATEIP", DataType.Varchar, false, false)]
        public string updateip { set; get; }
        #endregion Model

    }
}


