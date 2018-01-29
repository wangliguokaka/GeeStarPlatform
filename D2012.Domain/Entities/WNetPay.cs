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
    [Table("W_NetPay", "ID", false, false)]
    public partial class WNetPay : BaseEntity
    {
        public WNetPay()
        { }
        #region Model

        public static string STRTABLENAME = "W_NetPay";
        public static string STRKEYNAME = "ID";


        /// <summary>
        /// 
        /// </summary>
        [Column("ID", DataType.Int, true, true)]
        public int ID { set; get; }

        
        /// <summary>
        /// 
        /// </summary>
        [Column("OrderID", DataType.Varchar, false, false)]
        public string OrderID { set; get; }
        

        /// <summary>
        /// 
        /// </summary>
        [Column("UserID", DataType.Int, false, false)]
        public int UserID { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("PayAmount", DataType.Decimal, false, false)]
        public decimal PayAmount { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("JGCBM", DataType.Nvarchar, false, false)]
        public string JGCBM { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("SubmitDateTime", DataType.Datetime, false, false)]
        public DateTime? SubmitDateTime { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("PayDateTime", DataType.Datetime, false, false)]
        public DateTime? PayDateTime { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("PayMethod", DataType.Varchar, false, false)]
        public string PayMethod { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("PayStatus", DataType.Varchar, false, false)]
        public string PayStatus { set; get; }

         /// <summary>
        /// 
        /// </summary>
        [Column("Remark", DataType.Varchar, false, false)]
        public string Remark { set; get; }
        

        #endregion Model

    }
}