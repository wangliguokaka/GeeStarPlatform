using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.OracleClient;
using System.Data;

namespace D2012.DBUtility.Data.Core.OracleCore
{
    /// <summary>
    /// 数据库操作基本对象
    /// </summary>
    public class OraProvider : IDbProvider
    {
        private string connectionString_ = System.Configuration.ConfigurationSettings.AppSettings["ConnectString"];
        private IDbConnection connection_ = null;
        private IDbCommand command_ = null;
        private IDbDataAdapter adapter_ = null;

        /// <summary>
        /// 数据库事务对象
        /// </summary>
        private IDbTransaction transaction_ = null;

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                if (connectionString_ == null)
                {
                    connectionString_ = System.Configuration.ConfigurationSettings.AppSettings["ConnectString"];
                }
                return connectionString_;
            }

            set
            {
                connectionString_ = value;
            }
        }

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        public IDbConnection Connection
        {
            get
            {
                if (connection_ == null)
                {
                    connection_ = new OracleConnection(connectionString_);
                }
                return connection_;
            }

            set
            {
                connection_ = value;
            }
        }

        /// <summary>
        /// 数据库命令操作对象
        /// </summary>
        public IDbCommand Command
        {
            get
            {
                if (command_ == null)
                {
                    command_ = new OracleCommand();
                    command_.Connection = connection_;
                }
                return command_;
            }

            set
            {
                command_ = value;
            }
        }


        /// <summary>
        /// 数据库适配器对象
        /// </summary>
        public IDbDataAdapter Adapter
        {
            get
            {
                if (adapter_ == null)
                {
                    adapter_ = new OracleDataAdapter(command_ as OracleCommand);
                }
                return adapter_;
            }

            set
            {
                adapter_ = value;
            }
        }



        /// <summary>
        /// 数据库适配器对象
        /// </summary>
        public bool IsRransaction
        {
            get
            {

                return transaction_ != null;
            }
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public void Open()
        {
            if (Connection != null && Connection.State != ConnectionState.Open)
            {
                connection_.Open();
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            if (connection_ != null && transaction_ == null)
            {
                connection_.Close();
            }
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            Open();
            if (Command.Transaction == null)
            {
                transaction_ = Connection.BeginTransaction();
                Command.Transaction = transaction_;
            }
        }

        /// <summary>
        /// 事务回滚
        /// </summary>
        public void RollBack()
        {
            if (Connection != null)
            {
                transaction_.Rollback();
                Command.Transaction = null;
                transaction_.Dispose();
            }
        }

        /// <summary>
        /// 事务提交
        /// </summary>
        public void Commit()
        {
            if (Connection != null)
            {
                transaction_.Commit();
                Command.Transaction = null;
                transaction_.Dispose();
            }
        }

        /// <summary>
        /// 创建数据库加载驱动
        /// </summary>
        /// <returns></returns>
        public IDbProvider CreateInstance()
        {
            return new OraProvider();
        }

        /// <summary>
        /// 释放内存空间
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
