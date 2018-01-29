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
    [Table("PRODUCTS", "ID", false, false)]
    public partial class PRODUCTS : BaseEntity
    {
        public PRODUCTS()
        { }
        #region Model
        public static string STRTABLENAME = "PRODUCTS";
        public static string STRKEYNAME = "ID";

        /// <summary>
        /// 
        /// </summary>
        [Column("ID", DataType.Varchar, true, false)]
        public string ID { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("itemname", DataType.Varchar, false, false)]
        public string ItemName { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("price", DataType.Money, false, false)]
        public decimal Price { set; get; }
        /// </summary>
        [Column("Nobleclass", DataType.Varchar, false, false)]
        public string Nobleclass { set; get; }
       
        public string Color { set; get; }
        public string Number { set; get; }
        public string lefttop { set; get; }
        public string righttop { set; get; }
        public string leftbottom { set; get; }
        public string rightbottom { set; get; }
        public string productclass { set; get; }
        /// <summary>
        /// 
        /// </summary>
        [Column("SmallClass", DataType.Varchar, false, false)]
        public string SmallClass { set; get; }

        #endregion Model
    }
}
