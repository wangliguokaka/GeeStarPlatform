using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D2012.Common.DbCommon
{
    /// <summary>
    /// 实体用类型
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// 对应.NET中的数据类型 Int64 
        /// </summary>
        Bigint,

        /// <summary>
        /// 对应.NET中的数据类型 Int32 
        /// </summary>
        Int,

        /// <summary>
        /// 对应.NET中的数据类型 Int16 
        /// </summary>
        Smallint,

        /// <summary>
        /// 对应.NET中的数据类型 System.Byte 
        /// </summary>
        Tinyint,

        /// <summary>
        /// 对应.NET中的数据类型 bool 
        /// </summary>
        Bit,

        /// <summary>
        /// 对应.NET中的数据类型 System.Decimal 
        /// </summary>
        Decimal,

        /// <summary>
        /// 对应.NET中的数据类型 System.Decimal 
        /// </summary>
        Numeric,

        /// <summary>
        /// 对应.NET中的数据类型 System.Decimal 
        /// </summary>
        Money,

        /// <summary>
        /// 对应.NET中的数据类型 
        /// </summary>
        Smallmoney,

        /// <summary>
        /// 对应.NET中的数据类型 System.Double 
        /// </summary>
        Float,

        /// <summary>
        /// 对应.NET中的数据类型 System.Single 
        /// </summary>
        Real,

        /// <summary>
        /// 对应.NET中的数据类型 System.DateTime 
        /// </summary>
        Datetime,

        /// <summary>
        /// 对应.NET中的数据类型 System.DateTime 
        /// </summary>
        Smalldatetime,

        /// <summary>
        /// 对应.NET中的数据类型 String 
        /// </summary>
        Char,

        /// <summary>
        /// 对应.NET中的数据类型 String 
        /// </summary>
        Varchar,

        /// <summary>
        /// 对应.NET中的数据类型 String 
        /// </summary>
        Text,

        /// <summary>
        /// 对应.NET中的数据类型 String 
        /// </summary>
        Nchar,

        /// <summary>
        /// 对应.NET中的数据类型 String 
        /// </summary>
        Nvarchar,

        /// <summary>
        /// 对应.NET中的数据类型 String
        /// </summary>
        Ntext,

        /// <summary>
        /// 对应.NET中的数据类型 System.Byte[] 
        /// </summary>
        Binary,

        /// <summary> 
        /// 对应.NET中的数据类型 System.Byte[] 
        /// </summary>
        Varbinary,

        /// <summary>
        /// 对应.NET中的数据类型 System.Byte[] 
        /// </summary>
        Image,

        /// <summary>
        /// 对应.NET中的数据类型 System.DateTime 
        /// </summary>
        Timestamp,

        /// <summary>
        /// 对应.NET中的数据类型 System.Guid 
        /// </summary>
        Uniqueidentifier,

        /// <summary>
        /// 对应.NET中的数据类型 Object 
        /// </summary>
        Variant,

        /// <summary>
        /// Clob
        /// </summary>
        Clob,

        /// <summary>
        /// Blob
        /// </summary>
        Blob

    }
}
