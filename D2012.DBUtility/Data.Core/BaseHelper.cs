using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Web;
using System.Collections;

using D2012.DBUtility.EntityCommon;
using D2012.DBUtility.Data.Core.SQLCore;
using D2012.DBUtility.Data.Core.OracleCore;
using D2012.Common.DbCommon;

namespace D2012.DBUtility.Data.Core
{
    /// <summary>
    /// 数据库访问类
    /// </summary>
    public class BaseHelper : IBaseHelper
    {

        private IDbFactory factory_ = null;
        private IDbProvider provider = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="providertemp">providertemp</param>
        public BaseHelper(IDbProvider providertemp)
        {
            //IDbProvider provider
            provider = providertemp;
        }

        /// <summary>
        /// sql语句构造工厂对象
        /// </summary>
        public IDbFactory Factory
        {
            get
            {
                if (factory_ == null)
                {
                    factory_ = new OraSqlFactory();
                }
                return factory_;
            }

            set { factory_ = value; }
        }

        /// <summary>
        /// 标记是否事务
        /// </summary>
        public bool IsRransaction
        {
            get
            {
                return provider.IsRransaction;
            }
        }

        /// <summary>
        /// Dispose函数
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            provider.BeginTransaction();
        }

        /// <summary>
        /// 提交
        /// </summary>
        public void Commit()
        {
            provider.Commit();
        }

        /// <summary>
        /// 回滚
        /// </summary>
        public void RollBack()
        {
            provider.RollBack();
        }

        /// <summary>
        /// 返回受影响行数
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="strSQLString">sql语句</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlString)
        {
            return ExecuteNonQuery(sqlString, false, null);
        }

        /// <summary>
        /// 返回受影响行数
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <param name="isProcedure">是否为存储过程</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlString, bool isProcedure)
        {
            return ExecuteNonQuery(sqlString, isProcedure, null);
        }

        /// <summary>
        /// 返回受影响行数
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <param name="param">sql语句对应参数</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlString, params IDataParameter[] param)
        {
            return ExecuteNonQuery(sqlString, false, param);
        }

        /// <summary>
        /// 返回受影响行数
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <param name="isProcedure">是否为存储过程,true 为存储过程</param>
        /// <param name="param">sql语句对应参数</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlString, bool isProcedure, params IDataParameter[] param)
        {
            try
            {
                provider.Open();
                provider.Command.CommandText = sqlString;
                if (isProcedure)
                {
                    provider.Command.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    provider.Command.CommandType = CommandType.Text;
                }
                provider.Command.Parameters.Clear();
                if (param != null)
                {
                    for (int i = 0; i < param.Length; i++)
                    {
                        provider.Command.Parameters.Add(param[i]);
                    }
                }
                return (int)provider.Command.ExecuteNonQuery();


            }
            finally
            {
                provider.Close();
            }


        }

        /// <summary>
        /// 返回查询语句第一行第一列
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlString)
        {
            return ExecuteScalar(sqlString, false, null);
        }

        /// <summary>
        /// 返回查询语句第一行第一列
        /// </summary>
        /// <param name="sqlString">sql语句</param>
        /// <param name="isProcedure">是否是存储过程</param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlString, bool isProcedure)
        {
            return ExecuteScalar(sqlString, isProcedure, null);
        }

        /// <summary>
        /// 返回查询语句第一行第一列
        /// </summary>
        /// <param name="sqlString">sql语句</param>
        /// <param name="param">sql语句对应输入参数</param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlString, params IDataParameter[] param)
        {
            return ExecuteScalar(sqlString, false, param);
        }

        /// <summary>
        /// 返回查询语句第一行第一列
        /// </summary>
        /// <param name="sqlString">sql语句</param>
        /// <param name="isProcedure">是否为存储过程</param>
        /// <param name="param">sql语句对应输入参数</param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlString, bool isProcedure, params IDataParameter[] param)
        {
            try
            {
                provider.Open();
                provider.Command.CommandText = sqlString;
                if (isProcedure)
                {
                    provider.Command.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    provider.Command.CommandType = CommandType.Text;
                }
                provider.Command.Parameters.Clear();
                if (param != null)
                {
                    for (int i = 0; i < param.Length; i++)
                    {
                        provider.Command.Parameters.Add(param[i]);
                    }
                }
                //provider.Command.Parameters.AddRange(param);
                return provider.Command.ExecuteScalar();

            }
            finally
            {
                provider.Close();
            }

        }

        /// <summary>
        /// 返回数据只读游标集
        /// </summary>
        /// <param name="sqlString">sql语句</param>
        /// <returns></returns>
        public IDataReader ExecuteDataReader(string sqlString)
        {
            return ExecuteDataReader(sqlString, false, null);
        }

        /// <summary>
        /// 返回数据只读游标集
        /// </summary>
        /// <param name="sqlString">sql语句</param>
        /// <param name="isProcedure">是否为存储过程</param>
        /// <returns></returns>
        public IDataReader ExecuteDataReader(string sqlString, bool isProcedure)
        {
            return ExecuteDataReader(sqlString, isProcedure, null);
        }

        /// <summary>
        /// 返回数据只读游标集
        /// </summary>
        /// <param name="sqlString">sql语句</param>
        /// <param name="param">sql语句对应输入参数</param>
        /// <returns></returns>
        public IDataReader ExecuteDataReader(string sqlString, params IDataParameter[] param)
        {
            return ExecuteDataReader(sqlString, false, param);
        }

        /// <summary>
        /// 返回数据只读游标集
        /// </summary>
        /// <param name="sqlString">sql语句</param>
        /// <param name="isProcedure">是否为存储过程</param>
        /// <param name="param">sql语句对应输入参数</param>
        /// <returns></returns>
        public IDataReader ExecuteDataReader(string sqlString, bool isProcedure, params IDataParameter[] param)
        {
            try
            {
                provider.Open();
                provider.Command.CommandText = sqlString;
                if (isProcedure)
                {
                    provider.Command.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    provider.Command.CommandType = CommandType.Text;
                }
                provider.Command.Parameters.Clear();
                if (param != null)
                {
                    for (int i = 0; i < param.Length; i++)
                    {
                        provider.Command.Parameters.Add(param[i]);
                    }
                }
                return provider.Command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            finally
            {
                //provider.Close();
            }

        }

        /// <summary>
        /// 获得数据表结构集合
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <returns></returns>
        public DataTable ExecuteTable(string sqlString)
        {

            return ExecuteTable(sqlString, false, null);
        }

        /// <summary>
        ///  获得数据表结构集合
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <param name="isProcedure">是否为存储过程</param>
        /// <returns></returns>
        public DataTable ExecuteTable(string sqlString, bool isProcedure)
        {
            return ExecuteTable(sqlString, isProcedure, null);
        }

        /// <summary>
        /// 获得数据表结构集合
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <param name="param">sql语句对应参数</param>
        /// <returns></returns>
        public DataTable ExecuteTable(string sqlString, params IDataParameter[] param)
        {
            return ExecuteTable(sqlString, false, param);
        }

        /// <summary>
        /// 获得数据表结构集合
        /// </summary>
        /// <param name="provider">数据提供加载驱动</param>
        /// <param name="sqlString">sql语句</param>
        /// <param name="isProcedure">是否为存储过程</param>
        /// <param name="param">sql语句对应参数</param>
        /// <returns></returns>
        public DataTable ExecuteTable(string sqlString, bool isProcedure, params IDataParameter[] param)
        {
            try
            {
                provider.Open();
                provider.Command.CommandText = sqlString;
                if (isProcedure)
                {
                    provider.Command.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    provider.Command.CommandType = CommandType.Text;
                }
                provider.Command.Parameters.Clear();
                if (param != null)
                {
                    for (int i = 0; i < param.Length; i++)
                    {
                        provider.Command.Parameters.Add(param[i]);
                    }
                }
                DataSet ds = new DataSet();
                provider.Adapter.Fill(ds);


                //时区处理
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //ArrayList altColIndex = new ArrayList();
                    List<int> lstColIndex = new List<int>();
                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        if (ds.Tables[0].Columns[i].DataType == Type.GetType("System.DateTime"))
                        {
                            lstColIndex.Add(i);
                        }

                    }

                    //存在datetime列的时候
                    if (lstColIndex.Count > 0)
                    {
                        //循环行和datetime列，将值改为对应时区的时间
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            for (int j = 0; j < lstColIndex.Count; j++)
                            {
                                if (ds.Tables[0].Rows[i][lstColIndex[j]] != DBNull.Value)
                                {

                                    int intTimeZone = 8;
                                    if (HttpContext.Current.Request.Cookies["ZTETIMEZONE"] != null)
                                    {
                                        intTimeZone = int.Parse(HttpContext.Current.Request.Cookies["ZTETIMEZONE"].Value);
                                    }
                                    //DateTime dtTemp = Convert.ToDateTime(reader[key]).AddHours(intTimeZone - 8);

                                    DateTime dtTemp = Convert.ToDateTime(ds.Tables[0].Rows[i][lstColIndex[j]]);

                                    ds.Tables[0].Rows[i][lstColIndex[j]] = dtTemp.AddHours(intTimeZone - 8);
                                }
                            }
                        }
                    } //datetime列处理结束
                }//时区处理结束

                return ds.Tables[0];

            }
            finally
            {
                provider.Close();
            }

        }

        /// <summary>
        /// 根据一个泛型类型获得实体对象
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="reader">只读数据流</param>
        /// <returns></returns>
        public T ConvertToEntity<T>(IDataReader reader) where T : class, new()
        {
            T entity = default(T);
            object result = ConvertToEntity(typeof(T), reader);
            if (result != null)
            {
                entity = (T)result;
            }
            else
            {
                entity = new T();
            }
            return entity;
        }

        /// <summary>
        /// 将数据流装化为对象                                                
        /// </summary>
        /// <param name="type">转化的目标类型</param>
        /// <param name="reader">只读数据流</param>
        /// <returns></returns>
        public object ConvertToEntity(Type type, IDataReader reader)
        {
            try
            {
                object entity = null;
                if (reader.Read())
                {
                    TableInfo tfInfo = EntityTypeCache.GetTableInfo(type);
                    entity = EntityFactory.CreateInstance(type);
                    bool ble = reader.GetSchemaTable().Columns.Contains("name");

                    //所有列处理
                    foreach (string key in tfInfo.DicColumns.Keys)
                    {
                        //列存在，而且不为空
                        if (reader.GetSchemaTable().Select("ColumnName='" + key + "'").Length > 0
                            && reader[key] != DBNull.Value)
                        {
                            //if (tfInfo.DicColumns[key].DataType == Common.DbCommon.DataType.Datetime)
                            //{
                            //    int intTimeZone = 8;
                            //    if (HttpContext.Current.Request.Cookies["ZTETIMEZONE"] != null)
                            //    {
                            //        intTimeZone = int.Parse(HttpContext.Current.Request.Cookies["ZTETIMEZONE"].Value);
                            //    }
                            //    DateTime dt = Convert.ToDateTime(reader[key]).AddHours(intTimeZone - 8);
                            //    tfInfo.DicProperties[key].SetValue(entity, dt, null);
                            //}
                            //else
                            //{


                            //}

                            tfInfo.DicProperties[key].SetValue(entity, reader[key], null);

                            //tfInfo.DicProperties[key].SetValue(entity, reader[key], null);
                        }//列存在赋值结束
                    }

                    //关联表处理
                    if (tfInfo.DicLinkTable.Keys.Count > 0)
                    {
                        //处理每一列
                        foreach (string key in tfInfo.DicLinkTable.Keys)
                        {
                            Type entityType = tfInfo.DicLinkTable[key].DataType;
                            string sql = Factory.CreateSingleSql(entityType);
                            IDataParameter[] param = new IDataParameter[] { new SqlParameter() };
                            param[0].ParameterName = "@"
                                + EntityTypeCache.GetTableInfo(tfInfo.DicLinkTable[key].DataType).Table.PrimaryKeyName;
                            param[0].Value = EntityFactory.GetPropertyValue(entity, tfInfo.DicLinkTable[key].KeyName);
                            using (IDbProvider provider = new SqlProvider())
                            {
                                IDataReader read = ExecuteDataReader(sql, param);
                                object entityChild = EntityFactory.CreateInstance(entityType, false);
                                if (read.Read())
                                {
                                    foreach (string propertyName in
                                        EntityTypeCache.GetTableInfo(entityType).DicProperties.Keys)
                                    {
                                        EntityTypeCache.GetTableInfo(entityType).DicProperties[propertyName].SetValue(
                                            entityChild,
                                            read[EntityTypeCache.GetTableInfo(entityType).DicColumns[propertyName].Name],
                                            null);
                                    }
                                }
                                tfInfo.DicProperties[key].SetValue(entity, entityChild, null);
                            }
                        }
                    }//关联表处理结束
                }
                return entity;
            }
            finally
            {
                reader.Close();
            }
        }

        /// <summary>
        /// 根据一个泛型类型查询一个集合
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="reader">只读数据流</param>
        /// <returns></returns>
        public IList<T> ConvertToList<T>(IDataReader reader) where T : class
        {
            try
            {

                T entity = default(T);
                IList<T> list = new List<T>();
                TableInfo tfInfo = EntityTypeCache.GetTableInfo(typeof(T));

                //行循环处理
                while (reader.Read())
                {
                    entity = EntityFactory.CreateInstance<T>();

                    //所有列处理
                    foreach (string key in tfInfo.DicColumns.Keys)
                    {
                        //列存在，而且不为空
                        if (reader.GetSchemaTable().Select("ColumnName='" + key + "'").Length > 0
                            && reader[key] != DBNull.Value)
                        {
                            //if (tfInfo.DicColumns[key].DataType == Common.DbCommon.DataType.Datetime)
                            //{
                            //    //int intTimeZone = 8;
                            //    //if (HttpContext.Current.Request.Cookies["ZTETIMEZONE"]!=null)
                            //    //{
                            //    //    intTimeZone = int.Parse(HttpContext.Current.Request.Cookies["ZTETIMEZONE"].Value);
                            //    //}
                            //    //DateTime dt = Convert.ToDateTime(reader[key]).AddHours(intTimeZone-8);
                            //    tfInfo.DicProperties[key].SetValue(entity, reader[key], null);
                            //}
                            //else
                            //{


                            //}

                            tfInfo.DicProperties[key].SetValue(entity, reader[key], null);
                        }

                    }

                    //关联表处理
                    if (tfInfo.DicLinkTable.Keys.Count > 0)
                    {
                        foreach (string key in tfInfo.DicLinkTable.Keys)
                        {
                            Type entityType = tfInfo.DicLinkTable[key].DataType;
                            string sql = Factory.CreateSingleSql(entityType);
                            IDataParameter[] param = new IDataParameter[]{
                            new SqlParameter()
                        };
                            param[0].ParameterName = "@"
                                + EntityTypeCache.GetTableInfo(tfInfo.DicLinkTable[key].DataType).Table.PrimaryKeyName;
                            param[0].Value = EntityFactory.GetPropertyValue(entity, tfInfo.DicLinkTable[key].KeyName);
                            using (IDbProvider provider = new SqlProvider())
                            {
                                IDataReader read = ExecuteDataReader(sql, param);
                                object entityChild = EntityFactory.CreateInstance(entityType, false);
                                if (read.Read())
                                {
                                    foreach (string propertyName
                                        in EntityTypeCache.GetTableInfo(entityType).DicProperties.Keys)
                                    {
                                        EntityTypeCache.GetTableInfo(entityType).DicProperties[propertyName].SetValue(
                                            entityChild,
                                            read[EntityTypeCache.GetTableInfo(entityType).DicColumns[propertyName].Name],
                                            null);
                                    }
                                }
                                tfInfo.DicProperties[key].SetValue(entity, entityChild, null);
                            }
                        }
                    }
                    list.Add(entity);
                }
                return list;
            }
            finally
            {
                reader.Close();
            }
        }
    }
}
