using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace D2012.DBUtility.Data.Core
{
    /// <summary>
    /// BaseHelper接口
    /// </summary>
    public interface IBaseHelper : IDisposable
    {
        /// <summary>
        /// 是否事务中
        /// </summary>
        bool IsRransaction { get; }

        /// <summary>
        /// 开始事务
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 提交事务
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollBack();

        /// <summary>
        /// 返回受影响行数
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <returns></returns>
        int ExecuteNonQuery(string sqlString);

        /// <summary>
        /// 返回受影响行数
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <param name="isProcedure">是否为存储过程</param>
        /// <returns></returns>
        int ExecuteNonQuery(string sqlString, bool isProcedure);

        /// <summary>
        /// 返回受影响行数
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <param name="param">sql语句对应参数</param>
        /// <returns></returns>
        int ExecuteNonQuery(string sqlString, params IDataParameter[] param);


        /// <summary>
        /// 返回受影响行数
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <param name="isProcedure">是否为存储过程,true 为存储过程</param>
        /// <param name="param">sql语句对应参数</param>
        /// <returns></returns>
        int ExecuteNonQuery(string sqlString, bool isProcedure, params IDataParameter[] param);

        /// <summary>
        /// 返回查询语句第一行第一列
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <returns></returns>
        object ExecuteScalar(string sqlString);

        /// <summary>
        /// 返回查询语句第一行第一列
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <param name="isProcedure">是否是存储过程</param>
        /// <returns></returns>
        object ExecuteScalar(string sqlString, bool isProcedure);

        /// <summary>
        /// 返回查询语句第一行第一列
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <param name="param">sql语句对应输入参数</param>
        /// <returns></returns>
        object ExecuteScalar(string sqlString, params IDataParameter[] param);

        /// <summary>
        /// 返回查询语句第一行第一列
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <param name="isProcedure">是否为存储过程</param>
        /// <param name="param">sql语句对应输入参数</param>
        /// <returns></returns>
        object ExecuteScalar(string sqlString, bool isProcedure, params IDataParameter[] param);

        /// <summary>
        /// 返回数据只读游标集
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <returns></returns>
        IDataReader ExecuteDataReader(string sqlString);

        /// <summary>
        /// 返回数据只读游标集
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <param name="isProcedure">是否为存储过程</param>
        /// <returns></returns>
        IDataReader ExecuteDataReader(string sqlString, bool isProcedure);

        /// <summary>
        /// 返回数据只读游标集
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <param name="param">sql语句对应输入参数</param>
        /// <returns></returns>
        IDataReader ExecuteDataReader(string sqlString, params IDataParameter[] param);

        /// <summary>
        /// 返回数据只读游标集
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <param name="isProcedure">是否为存储过程</param>
        /// <param name="param">sql语句对应输入参数</param>
        /// <returns></returns>
        IDataReader ExecuteDataReader(string sqlString, bool isProcedure, params IDataParameter[] param);

        /// <summary>
        /// 获得数据表结构集合
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <returns></returns>
        DataTable ExecuteTable(string sqlString);

        /// <summary>
        ///  获得数据表结构集合
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <param name="isProcedure">是否为存储过程</param>
        /// <returns></returns>
        DataTable ExecuteTable(string sqlString, bool isProcedure);

        /// <summary>
        /// 获得数据表结构集合
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <param name="param">sql语句对应参数</param>
        /// <returns></returns>
        DataTable ExecuteTable(string sqlString, params IDataParameter[] param);

        /// <summary>
        /// 获得数据表结构集合
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <param name="isProcedure">是否为存储过程</param>
        /// <param name="param">sql语句对应参数</param>
        /// <returns></returns>
        DataTable ExecuteTable(string sqlString, bool isProcedure, params IDataParameter[] param);



        /// <summary>
        /// 根据一个泛型类型获得实体对象
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="reader">只读数据流</param>
        /// <returns></returns>
        T ConvertToEntity<T>(IDataReader reader) where T : class, new();

        /// <summary>
        /// 将数据流装化为对象                                                
        /// </summary>
        /// <param name="type">转化的目标类型</param>
        /// <param name="reader">只读数据流</param>
        /// <returns></returns>
        object ConvertToEntity(Type type, IDataReader reader);

        /// <summary>
        /// 根据一个泛型类型查询一个集合
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="reader">只读数据流</param>
        /// <returns></returns>
        IList<T> ConvertToList<T>(IDataReader reader) where T : class;
    }
}
