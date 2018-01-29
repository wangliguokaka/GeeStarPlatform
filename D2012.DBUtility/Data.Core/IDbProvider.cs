using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace D2012.DBUtility.Data.Core
{
    /// <summary>
    /// 接口
    /// </summary>
    public interface IDbProvider : IDisposable
    {
        /// <summary>
        /// 数据库链接语句
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        IDbConnection Connection { get; set; }

        /// <summary>
        /// 数据库操作命令
        /// </summary>
        IDbCommand Command { get; set; }

        /// <summary>
        /// 数据库操作适配器
        /// </summary>
        IDbDataAdapter Adapter { get; set; }

        /// <summary>
        /// 是否事务
        /// </summary>
        bool IsRransaction { get; }

        /// <summary>
        /// 打开数据库连接方法
        /// </summary>
        void Open();

        /// <summary>
        /// 关闭数据库连接方法
        /// </summary>
        void Close();

        /// <summary>
        /// 开始事务控制方法
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 事务回滚方法
        /// </summary>
        void RollBack();

        /// <summary>
        /// 事务提交方法
        /// </summary>
        void Commit();
    }
}
