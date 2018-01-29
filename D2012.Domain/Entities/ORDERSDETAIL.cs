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
    [Table("OrdersDetail", "", false, false)]
    public partial class ORDERSDETAIL : BaseEntity
    {
        public ORDERSDETAIL()
        { }
        #region Model
        public static string STRTABLENAME = "VWPatient";
        public static string STRKEYNAME = "Order_ID,serial,subId";

         [Column("Order_ID", DataType.Varchar, true, false)]
        public string Order_ID { set; get; }

         [Column("serial", DataType.Int, true, false)]
         public int serial { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("ModelNo", DataType.Varchar, true, false)]
        public string ModelNo { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("subId", DataType.Int, false, false)]
        public int subId { set; get; }


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


        [Column("SellerID", DataType.Numeric, false, false)]
        public decimal SellerID { set; get; }

         [Column("seller", DataType.Varchar, false, false)]
        public string seller { set; get; }

        [Column("HospitalID", DataType.Numeric, false, false)]
        public decimal HospitalID { set; get; }


        [Column("hospital", DataType.Varchar, false, false)]
        public string hospital { set; get; }


         [Column("SellerID", DataType.Numeric, false, false)]
        public decimal DoctorId { set; get; }

          [Column("doctor", DataType.Varchar, false, false)]
        public string doctor { set; get; }

        [Column("Patient", DataType.Varchar, false, false)]
        public string Patient { set; get; }

         [Column("productName", DataType.Varchar, false, false)]
        public string productName { set; get; }

         [Column("Valid", DataType.Varchar, false, false)]
        public string Valid { set; get; }


         [Column("ItemName", DataType.Varchar, false, false)]
        public string ItemName { set; get; }


        #endregion Model
    }
}
