using D2012.Common.DbCommon;
using D2012.Domain.Common;
using System;
namespace D2012.Domain.Entities
{
    /// <summary>
    /// 管理角色权限表
    /// </summary>
    [Serializable]
    [Table("W_MANAGER_ROLE_VALUE", "id", false, false)]
    public partial class MANAGERROLEVALUE:BaseEntity
    {
        public MANAGERROLEVALUE()
        { }
        #region Model

        public static string STRTABLENAME = "W_MANAGER_ROLE_VALUE";
        public static string STRKEYNAME = "id";


        /// <summary>
        /// 
        /// </summary>
        [Column("ID", DataType.Int, true, true)]
        public int ID { set; get; }

        
        /// <summary>
        /// 
        /// </summary>
        [Column("JGCID", DataType.Int, false, false)]
        public int JGCID { set; get; }
        

        /// <summary>
        /// 
        /// </summary>
        [Column("NavID", DataType.Nvarchar, false, false)]
        public int NavID { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("StartTime", DataType.Datetime, false, false)]
        public DateTime StartTime { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("Valid", DataType.Int, false, false)]
        public int Valid { set; get; }

        #endregion Model

    }
}