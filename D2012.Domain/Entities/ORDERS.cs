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
    [Table("orders", "Order_ID,serial", false, false)]
    public partial class ORDERS : BaseEntity
    {
        public ORDERS()
        { }
        #region Model
        public static string STRTABLENAME = "orders";
        public static string STRKEYNAME = "Order_ID,serial";

        /// <summary>
        /// 
        /// </summary>
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
        [Column("orderclass", DataType.Char, false, false)]
        public string OrderClass { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("sellerid", DataType.Numeric, false, false)]
        public decimal SellerID { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("seller", DataType.Varchar, false, false)]
        public string seller { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("hospitalID", DataType.Numeric, false, false)]
        public decimal HospitalID { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("hospital", DataType.Varchar, false, false)]
        public string hospital { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("DoctorId", DataType.Numeric, false, false)]
        public decimal DoctorId { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("doctor", DataType.Varchar, false, false)]
        public string doctor { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("patient", DataType.Varchar, false, false)]
        public string Patient { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("Age", DataType.Int, false, false)]
        public int Age { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("Sex", DataType.Char, false, false)]
        public string Sex { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("Require", DataType.Char, false, false)]
        public string Require { set; get; }



        /// <summary>
        /// 
        /// </summary>
        [Column("danzuo", DataType.Char, false, false)]
        public string danzuo { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("Fenge", DataType.Char, false, false)]
        public string Fenge { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("regname", DataType.Char, false, false)]
        public string RegName { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("regtime", DataType.Datetime, false, false)]
        public DateTime RegTime { set; get; }

        [Column("indate", DataType.Datetime, false, false)]
        public DateTime indate { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [Column("preoutDate", DataType.Datetime, false, false)]
        public DateTime preoutDate { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("OutDate", DataType.Datetime, false, false)]
        public DateTime OutDate { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("process", DataType.Char, false, false)]
        public string process { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("CourierNo", DataType.Char, false, false)]
        public string CourierNo { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column("Courier", DataType.Char, false, false)]
        public string Courier { set; get; }

        /// </summary>
        [Column("OutSay", DataType.Char, false, false)]
        public string OutSay { set; get; }
        
        #endregion Model
    }
}
