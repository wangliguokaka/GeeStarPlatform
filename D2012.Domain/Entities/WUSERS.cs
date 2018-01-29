using D2012.Common.DbCommon;
using D2012.Domain.Common;
using System;
using System.Collections.Generic;

namespace D2012.Domain.Entities
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [Serializable]
    [Table("W_USERS", "ID", false, false)]
    public partial class WUSERS : BaseEntity
    {
        public WUSERS()
        { }
        #region Model

        public static string STRTABLENAME = "W_USERS";
        public static string STRKEYNAME = "ID";


        /// <summary>
        /// 
        /// </summary>
        [Column("ID", DataType.Int, true, true)]
        public int ID { set; get; }

        
        /// <summary>
        /// 
        /// </summary>
        [Column("UserName", DataType.Varchar, false, false)]
        public string UserName { set; get; }
        

        /// <summary>
        /// 
        /// </summary>
        [Column("Alias", DataType.Varchar, false, false)]
        public string Alias { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("Kind", DataType.Varchar, false, false)]
        public string Kind { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("Passwd", DataType.Varchar, false, false)]
        public string Passwd { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("AssocNo", DataType.Numeric, false, false)]
        public decimal AssocNo { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("RoleID", DataType.Varchar, false, false)]
        public string RoleID { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("RoleName", DataType.Varchar, false, false)]
        public string RoleName { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("BelongFactory", DataType.Varchar, false, false)]
        public string BelongFactory { set; get; }

         /// <summary>
        /// 
        /// </summary>
        [Column("JGCName", DataType.Varchar, false, false)]
        public string JGCName { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("Disp_Money", DataType.Varchar, false, false)]
        public string Disp_Money { set; get; }
        
        #endregion Model

    }
}