using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D2012.Common.DbCommon
{
    /// <summary>
    /// 查询条件
    /// </summary>
    public enum SearchComponent
    {
        /// <summary>
        /// 对应数据库中的 "="
        /// </summary>
        Equals,

        /// <summary>
        /// 对应数据库中的 "!="
        /// </summary>
        UnEquals,

        /// <summary>
        /// 对应数据库中的 ">"
        /// </summary>
        Greater,

        /// <summary>
        /// 对应数据库中的 ">="
        /// </summary>
        GreaterOrEquals,

        /// <summary>
        /// 对应数据库中的 "<"
        /// </summary>
        Less,

        /// <summary>
        /// 对应数据库中的 "<="
        /// </summary>
        LessOrEquals,

        /// <summary>
        /// 对应数据库中的 "like"
        /// </summary>
        Like,

        /// <summary>
        /// 对应数据库中的 "in"
        /// </summary>
        In,

        /// <summary>
        /// 对应数据库中的 "In"
        /// </summary>

        NotIn,

        /// <summary>
        /// 对应数据库中的 "NotIn"
        /// </summary>

        IS,


        /// <summary>
        /// 对应数据库中的 "IS NOT"
        /// </summary>
        ISNOT,


        /// <summary>
        /// 对应数据库中的 "between and"
        /// </summary>
        Between,


        /// <summary>
        /// 对应数据库中的 "order by asc"
        /// </summary>
        OrderAsc,

        /// <summary>
        /// 对应数据库中的 "order by desc"
        /// </summary>
        OrderDesc,

        /// <summary>
        /// 对应数据库中的 "group by"
        /// </summary>
        GroupBy,

        /// <summary>
        /// 对应数据库中的 "or"
        /// </summary>
        Or


    }

    public enum SearchPad
    {
        /// <summary>
        /// (  在条件前添加
        /// </summary>
        Ex,
        /// <summary>
        /// ) 在条件后添加
        /// </summary>
        ExB,
        /// <summary>
        /// AND 在条件前添加
        /// </summary>
        And,
        /// <summary>
        /// AND ( 在条件前添加
        /// </summary>
        AndEx,
        /// <summary>
        ///) AND  在条件前添加
        /// </summary>
        ExBAnd,
        /// <summary>
        ///AND 条件 ) 
        /// </summary>
        AndExB,
        /// <summary>
        /// OR  在条件前添加
        /// </summary>
        Or,
        /// <summary>
        /// OR ( 在条件前添加
        /// </summary>
        OrEx,
        /// <summary>
        /// ) OR 在条件前添加
        /// </summary>
        ExBOr,
        /// <summary>
        /// OR 条件 ) 
        /// </summary>
        OrExB,
        /// <summary>
        ///   空 
        /// </summary>
        NULL

    }
}
