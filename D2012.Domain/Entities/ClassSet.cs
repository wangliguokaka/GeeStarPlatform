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
    [Table("ClassSet", "ClassID", false, false)]
    public partial class ClassSet : BaseEntity
    {
        public ClassSet()
        { }
        #region Model
        public static string STRTABLENAME = "ClassSet";
        public static string STRKEYNAME = "ClassID";
        /// <summary>
        /// 
        /// </summary>
        [Column("ClassID", DataType.Varchar, true, false)]
        public string ClassID { set; get; }

        [Column("Itemclass", DataType.Varchar, false, false)]
        public string Itemclass { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("SpecName", DataType.Varchar, false, false)]
        public string SpecName { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("SmallClass", DataType.Char, false, false)]
        public string SmallClass { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("sortno", DataType.Int, false, false)]
        public int sortno { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("oper", DataType.Varchar, false, false)]
        public string oper { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("OperTime", DataType.Datetime, false, false)]
        public DateTime OperTime { set; get; }

        #endregion Model
    }
}
