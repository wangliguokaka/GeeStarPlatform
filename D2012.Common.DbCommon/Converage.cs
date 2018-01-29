using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D2012.Common.DbCommon
{
    /// <summary>
    /// 查询用条件
    /// </summary>
    public enum Converage
    {
        /// <summary> 
        /// 聚合函数取最小值
        /// </summary>
        Min,

        /// <summary>
        /// 聚合函数取最大值
        /// </summary>
        Max,

        /// <summary>
        /// 聚合函数取和
        /// </summary>
        Sum,

        /// <summary>
        /// 聚合函数取所有数据行
        /// </summary>
        Count,

        /// <summary>
        /// 聚合函数取所有非空数据行
        /// </summary>
        CountNotNll,

        /// <summary>
        /// 聚合函数取平均值
        /// </summary>
        Avg,
    }
}
