using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D2012.Common.DbCommon;
using D2012.Domain.Common;

namespace D2012.Domain.Entities
{
    /// <summary>
    /// ACCOUNT:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    [Table("W_ordersOther", "ModelNo", false, false)]
    public partial class WORDERSOTHER : BaseEntity
    {
        public WORDERSOTHER()
        { }
        #region Model
        public static string STRTABLENAME = "W_Orders";
        public static string STRKEYNAME = "ModelNo";

        /// <summary>
        /// 
        /// </summary>
        [Column("ModelNo", DataType.Varchar, true, false)]
        public string ModelNo { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("SubId", DataType.Decimal, false, false)]
        public decimal SubId { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("Code", DataType.Nvarchar, false, false)]
        public string Code { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("name", DataType.Nvarchar, false, false)]
        public string name { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("qty", DataType.Numeric, false, false)]
        public decimal qty { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("BelongFactory", DataType.Varchar, false, false)]
        public string BelongFactory { set; get; }

        #endregion Model
    }
}
