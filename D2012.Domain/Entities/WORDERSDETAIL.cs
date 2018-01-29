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
    [Table("W_OrdersDetail", "", false, false)]
    public partial class WORDERSDETAIL : BaseEntity
    {
        public WORDERSDETAIL()
        { }
        #region Model
        public static string STRTABLENAME = "W_OrdersDetail";
        public static string STRKEYNAME = "";

        public static string TABLENAME = "VWPatient";
        public static string KEYNAME = "Order_ID,serial,subId";


        
        public string Order_ID { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("ModelNo", DataType.Varchar, true, false)]
        public string ModelNo { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("subId", DataType.Numeric, false, false)]
        public decimal subId { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("ProductId", DataType.Varchar, false, false)]
        public string ProductId { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("Qty", DataType.Int, false, false)]
        public int Qty { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("a_teeth", DataType.Char, false, false)]
        public string a_teeth { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("b_teeth", DataType.Char, false, false)]
        public string b_teeth { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("c_teeth", DataType.Char, false, false)]
        public string c_teeth { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("d_teeth", DataType.Char, false, false)]
        public string d_teeth { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("Nobleclass", DataType.Varchar, false, false)]
        public string Nobleclass { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("bColor", DataType.Char, false, false)]
        public string bColor { set; get; }

               /// <summary>
        /// 
        /// </summary>
        [Column("PositionList", DataType.Varchar, false, false)]
        public string PositionList { set; get; }
        

        public decimal SellerID { set; get; }


        public string seller { set; get; }


        public decimal HospitalID { set; get; }



        public string hospital { set; get; }



        public decimal DoctorId { set; get; }

        public string doctor { set; get; }


        public string Patient { set; get; }

        public string productName { set; get; }

        public string SmallClass { set; get; }

        public string Valid { set; get; }


        public string ItemName { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("BelongFactory", DataType.Varchar, false, false)]
        public string BelongFactory { set; get; }


        #endregion Model
    }
}
