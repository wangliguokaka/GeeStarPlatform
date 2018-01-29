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
    [Table("W_OrderAttachements", "ModelNo", false, false)]
    public partial class WORDERSATTACHMENTS : BaseEntity
    {
        public WORDERSATTACHMENTS()
        { }
        #region Model
        public static string STRTABLENAME = "W_OrderAttachements";
        public static string STRKEYNAME = "ID";


        /// <summary>
        /// 
        /// </summary>
        [Column("ID", DataType.Int, true, true)]
        public int ID { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("ModelNo", DataType.Varchar, false, false)]
        public string ModelNo { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("Filename", DataType.Nvarchar, false, false)]
        public string Filename { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("FilePath", DataType.Nvarchar, false, false)]
        public string FilePath { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("CreateDate", DataType.Datetime, false, false)]
        public DateTime CreateDate { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("BelongFactory", DataType.Varchar, false, false)]
        public string BelongFactory { set; get; }

        #endregion Model
    }
}
