using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using D2012.Common.DbCommon;
using D2012.DBUtility.Data.Core;
using D2012.DBUtility.Data;
using D2012.DBUtility.Data.Core.SQLCore;

namespace D2012.DBUtility.Data.Core.SQLCore
{
    /// <summary>
    /// SqlHelper
    /// </summary>
    public class SqlHelper : IDbHelper
    {
        private int intPageCount_ = 1;
        private int intRowCount_ = 1;
        private int isDel_ = 0;
        private int intPageNum_ = 1;
        private string strOrderString_ = null;
        private string strDelCol_;

        private IBaseHelper baseHelper_;
        private string strConnectionString_ = null;
        private IDbFactory factory_;

        /// <summary>
        /// 数据库操作公共接口



        /// </summary>
        public IBaseHelper BaseHelper
        {
            get
            {
                if (baseHelper_ == null)
                {
                    SqlProvider sp = new SqlProvider();
                    if (!string.IsNullOrEmpty(strConnectionString))
                    {
                        sp.ConnectionString = strConnectionString;
                    }
                    baseHelper_ = new BaseHelper(sp);
                }
                return baseHelper_;
            }

            set
            {
                baseHelper_ = value;
            }
        }

        /// <summary>
        /// 连接串



        /// </summary>
        public string strConnectionString
        {
            set { strConnectionString_ = value; }
            get { return strConnectionString_; }
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
                    factory_ = new SqlFactory();
                }
                factory_.StrOrderBy = strOrderString_;
                return factory_;
            }

            set
            {
                factory_ = value;
            }
        }

        /// <summary>
        /// 总页数



        /// </summary>
        public int PageCount
        {
            set { intPageCount_ = value; }
            get { return intPageCount_ > 1 ? intPageCount_ : ((intRowCount_ + intPageNum_ - 1) / intPageNum_); }
        }

        /// <summary>
        /// 总行数



        /// </summary>
        public int RowCount
        {
            set { intRowCount_ = value; }
            get { return intRowCount_; }
        }

        /// <summary>
        /// 排序
        /// </summary>
        public string strOrderString
        {
            set { strOrderString_ = value; }
            get { return strOrderString_; }
        }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDel
        {
            set { isDel_ = value; Factory.IsDel = value; }
            get { return Convert.ToInt32(isDel_); }
        }

        /// <summary>
        /// 删除列字段名称



        /// </summary>
        public string strDelCol
        {
            set { strDelCol_ = value; Factory.strDelCol = value; }
            get { return strDelCol_; }
        }


        /// <summary>
        /// 开始事务



        /// </summary>
        public void BeginTransaction()
        {
            BaseHelper.BeginTransaction();
        }

        /// <summary>
        /// 提交
        /// </summary>
        public void Commit()
        {
            BaseHelper.Commit();
        }

        /// <summary>
        /// 回滚
        /// </summary>
        public void RollBack()
        {
            BaseHelper.RollBack();
        }

        /// <summary>
        /// 添加实体对象
        /// 将实体数据信息映射为一条插入sql语句
        /// </summary>
        /// <param name="entity">实体公共接口</param>
        /// <returns></returns>
        public int Add(IEntity entity)
        {
            IDataParameter[] param = null;
            string sql = Factory.CreateInsertSql(entity, out param);

            return Convert.ToInt32(BaseHelper.ExecuteScalar(sql, param as SqlParameter[]));

        }

        /// <summary>
        /// 添加实体对象
        /// 将泛型数据信息映射为一条插入sql语句
        /// 该泛型类必须实现IEntity 接口
        /// </summary>
        /// <typeparam name="T">实体泛型类型</typeparam>
        /// <param name="t">泛型实体值</param>
        /// <returns></returns>
        //public int Add<T>(T t) where T : IEntity
        //{
        //    IDataParameter[] param = null;
        //    string sql = Factory.CreateInsertSql<T>(t,out param);
        //    using (IDbProvider provider = new SqlProvider())
        //    {
        //        return BaseHelper.ExecuteNonQuery( sql, param);
        //    }
        //}

        /// <summary>
        /// 添加实体对象
        /// value 的类型必须和type一致，这样才能保存值



        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="value">实体类型实例</param>
        /// <returns></returns>
        //public int Add(Type type, object value)
        //{
        //    IDataParameter[] param = null;
        //    string sql = Factory.CreateInsertSql(type,value,out param);
        //    using (IDbProvider provider = new SqlProvider())
        //    {
        //        return BaseHelper.ExecuteNonQuery( sql, param);
        //    }
        //}

        /// <summary>
        /// 根据实体公共接口修改该实体信息



        /// 该实体是根据主键修改
        /// </summary>
        /// <param name="entity">实体公共接口</param>
        /// <returns></returns>
        public int AddOrUpdate(IEntity entity)
        {
            IDataParameter[] param = null;
            string sql = Factory.CreateUpdateSql(entity, out param);
            //多条语句的场合



            if (sql.IndexOf(";") > 0)
            {
                return Convert.ToInt32(BaseHelper.ExecuteScalar(sql, param as SqlParameter[]));
            }
            else
            {
                return BaseHelper.ExecuteNonQuery(sql, param as SqlParameter[]);
            }

        }


        /// <summary>
        /// 根据实体公共接口修改该实体信息



        /// 该实体是根据主键修改
        /// </summary>
        /// <param name="entity">实体公共接口</param>
        /// <returns></returns>
        public int Update(IEntity entity)
        {
            IDataParameter[] param = null;
            string sql = Factory.CreateUpdateSql(entity, out param);

            return BaseHelper.ExecuteNonQuery(sql, param);

        }

        /// <summary>
        /// 根据实体的某个属性修改数据



        /// entity 中必须包含该属性，而且该属性的
        /// 值不能为空



        /// </summary>
        /// <param name="entity">实体公共接口</param>
        /// <param name="propertyName">实体属性名称</param>
        /// <returns></returns>
        public int Update(IEntity entity, string propertyName)
        {
            IDataParameter[] param = null;
            string sql = Factory.CreateUpdateSql(entity, out param, propertyName);

            return BaseHelper.ExecuteNonQuery(sql, param);

        }

        /// <summary>
        /// 根据实体的多个属性修改数据



        /// 数组中的属性名称必须存在.
        /// 传递参数数组不能为null
        /// </summary>
        /// <param name="entity">实体公共接口</param>
        /// <param name="propertyNames">属性名称数组</param>
        /// <returns></returns>
        public int Update(IEntity entity, string[] propertyNames)
        {
            IDataParameter[] param = null;
            string sql = Factory.CreateUpdateSql(entity, out param, propertyNames);

            return BaseHelper.ExecuteNonQuery(sql, param);

        }

        ///// <summary>
        ///// 修改实体信息
        ///// 该实体是根据主键修改
        ///// </summary>
        ///// <typeparam name="T">实体泛型类型</typeparam>
        ///// <param name="t">泛型实例</param>
        ///// <returns></returns>
        //public int Update<T>(T t) where T : IEntity
        //{
        //    IDataParameter[] param = null;
        //    string sql = Factory.CreateUpdateSql(typeof(T),t,out param);
        //    using (IDbProvider provider = new SqlProvider())
        //    {
        //        return BaseHelper.ExecuteNonQuery( sql, param);
        //    }
        //}

        ///// <summary>
        ///// 根据泛型类的某个实体属性来修改该数据。



        ///// 泛型实体实例中该属性不能为空



        ///// </summary>
        ///// <typeparam name="T">泛型类</typeparam>
        ///// <param name="t">泛型实例</param>
        ///// <param name="propertyName"></param>
        ///// <returns></returns>
        //public int Update<T>(T t, string propertyName) where T : IEntity
        //{
        //    IDataParameter[] param = null;
        //    string sql = Factory.CreateUpdateSql(typeof(T), t, out param,propertyName);
        //    using (IDbProvider provider = new SqlProvider())
        //    {
        //        return BaseHelper.ExecuteNonQuery( sql, param);
        //    }
        //}

        ///// <summary>
        ///// 根据实体的多个属性修改数据



        ///// 数组中的属性名称在泛型类中必须存在，



        ///// 数组不能传递null值



        ///// </summary>
        ///// <typeparam name="T">泛型类</typeparam>
        ///// <param name="t">泛型实例</param>
        ///// <param name="propertyNames">属性名称数组</param>
        ///// <returns></returns>
        //public int Update<T>(T t, string[] propertyNames) where T : IEntity
        //{
        //    IDataParameter[] param = null;
        //    string sql = Factory.CreateUpdateSql(typeof(T), t, out param, propertyNames);
        //    using (IDbProvider provider = new SqlProvider())
        //    {
        //        return BaseHelper.ExecuteNonQuery( sql, param);
        //    }
        //}

        /// <summary>
        /// 修改实体信息
        /// 该实体是根据主键修改
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="value">实体对象实例</param>
        /// <returns></returns>
        //public int Update(Type type, object value)
        //{
        //    IDataParameter[] param = null;
        //    string sql = Factory.CreateUpdateSql(type,value,out param);
        //    using (IDbProvider provider = new SqlProvider())
        //    {
        //        return BaseHelper.ExecuteNonQuery( sql, param);
        //    }
        //}

        /// <summary>
        /// 根据实体的某个属性来修改数据信息
        /// 该方法使用Type 来确定需要修改的实体对象
        /// value 实例中必须包含propertyName 这个属性



        /// 而且propertyName 属性值不能为空



        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="value">实体实例</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        //public int Update(Type type, object value, string propertyName)
        //{
        //    IDataParameter[] param = null;
        //    string sql = Factory.CreateUpdateSql(type, value, out param, propertyName);
        //    using (IDbProvider provider = new SqlProvider())
        //    {
        //        return BaseHelper.ExecuteNonQuery( sql, param);
        //    }
        //}

        /// <summary>
        /// 根据实体的多个属性来修改数据信息
        /// 该方法使用Type 来确定需要修改的实体对象
        /// 数组中的属性名称在泛型类中必须存在，



        /// 而且数组传递参数不能为null
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="value">实体实例</param>
        /// <param name="propertyNames">属性名称数组</param>
        /// <returns></returns>
        //public int Update(Type type, object value, string[] propertyNames)
        //{
        //    IDataParameter[] param = null;
        //    string sql = Factory.CreateUpdateSql(type, value, out param, propertyNames);
        //    using (IDbProvider provider = new SqlProvider())
        //    {
        //        return BaseHelper.ExecuteNonQuery( sql, param);
        //    }
        //}


        /// <summary>
        /// 修改实体信息
        /// 该实体是根据主键修改
        /// </summary>
        /// <typeparam name="T">实体泛型类型</typeparam>
        /// <param name="entity">泛型实例</param>
        /// <param name="component">泛型实例</param>
        /// <returns></returns>
        public int Update(IEntity entity, ConditionComponent component)
        {
            IDataParameter[] param = null;
            string sql = Factory.CreateUpdateSql(entity, out param, component);


            return BaseHelper.ExecuteNonQuery(sql, param);


        }

        ///// <summary>
        ///// 修改实体信息
        ///// 该实体是根据主键修改
        ///// </summary>
        ///// <typeparam name="T">实体泛型类型</typeparam>
        ///// <param name="t">泛型实例</param>
        ///// <returns></returns>
        //public int Update<T>(T t, ConditionComponent component) where T : IEntity
        //{
        //    IDataParameter[] param = null;
        //    string sql = Factory.CreateUpdateSql(typeof(T), t, out param, component);

        //    using (IDbProvider provider = new SqlProvider())
        //    {
        //        return BaseHelper.ExecuteNonQuery( sql, param);
        //    }

        //}


        /// <summary>
        /// 根据某个实体属性删除数据



        /// 该实体必须包含指定的属性名称



        /// 而且实体属性值不能为空



        /// </summary>
        /// <param name="strKeyValue">实体属性公共接口</param>
        /// <param name="bolType">实体属性名称</param>
        /// <returns></returns>
        public int Delete<T>(string strKeyValue, bool bolType) where T : IEntity
        {

            IDataParameter[] param = null;
            string sql = Factory.CreateDeleteSql(typeof(T), out param, strKeyValue, bolType);


            return BaseHelper.ExecuteNonQuery(sql, param);


        }



        /// <summary>
        /// 删除实体对象
        /// 该方法是根据实体对象主键删除数据
        /// </summary>
        /// <param name="entity">实体公共接口</param>
        /// <returns></returns>
        //public int Delete(IEntity entity)
        //{
        //    IDataParameter[] param = null;
        //    string sql = Factory.CreateDeleteSql(entity,out param);
        //    using (IDbProvider provider = new SqlProvider())
        //    {
        //        return BaseHelper.ExecuteNonQuery( sql, param);
        //    }
        //}

        /// <summary>
        /// 根据实体多个属性删除数据



        /// 实体中必须包含该属性



        /// 传递参数数组不能为空



        /// </summary>
        /// <param name="component">实体属性公共接口</param>
        /// <param name="bolType">实体属性名称数组</param>
        /// <returns></returns>
        public int Delete<T>(ConditionComponent component, bool bolType) where T : IEntity
        {
            IDataParameter[] param = null;
            string sql = Factory.CreateDeleteSql(typeof(T), component, out param, bolType);

            return BaseHelper.ExecuteNonQuery(sql, param);


        }



        ///// <summary>
        ///// 根据某个实体属性删除数据



        ///// 该实体必须包含指定的属性名称



        ///// 而且实体属性值不能为空



        ///// </summary>
        ///// <param name="entity">实体属性公共接口</param>
        ///// <param name="propertyName">实体属性名称</param>
        ///// <returns></returns>
        //public int Delete(IEntity entity, string propertyName)
        //{
        //    IDataParameter[] param = null;
        //    string sql = Factory.CreateDeleteSql(entity,out param,propertyName);
        //    using (IDbProvider provider = new SqlProvider())
        //    {
        //        return BaseHelper.ExecuteNonQuery( sql, param);
        //    }
        //}

        ///// <summary>
        ///// 根据实体多个属性删除数据



        ///// 实体中必须包含该属性



        ///// 传递参数数组不能为空



        ///// </summary>
        ///// <param name="entity">实体属性公共接口</param>
        ///// <param name="propertyNames">实体属性名称数组</param>
        ///// <returns></returns>
        //public int Delete(IEntity entity, string[] propertyNames)
        //{
        //    IDataParameter[] param = null;
        //    string sql = Factory.CreateDeleteSql(entity, out param, propertyNames);
        //    using (IDbProvider provider = new SqlProvider())
        //    {
        //        return BaseHelper.ExecuteNonQuery( sql, param);
        //    }
        //}

        ///// <summary>
        ///// 根据泛型类删除数据



        ///// 该方法是根据实体的主键删除的
        ///// </summary>
        ///// <typeparam name="T">泛型类</typeparam>
        ///// <param name="t">泛型实例</param>
        ///// <returns></returns>
        //public int Delete<T>(T t) where T : class
        //{
        //    IDataParameter[] param = null;
        //    string sql = Factory.CreateDeleteSql(typeof(T), t, out param);
        //    using (IDbProvider provider = new SqlProvider())
        //    {
        //        return BaseHelper.ExecuteNonQuery( sql, param);
        //    }
        //}

        ///// <summary>
        ///// 根据泛型类的某个属性删除数据



        ///// 泛型类中必须存在该属性，而且
        ///// 属性值不能为空



        ///// </summary>
        ///// <typeparam name="T">泛型类型</typeparam>
        ///// <param name="t">泛型类实例</param>
        ///// <param name="propertyName">属性名称</param>
        ///// <returns></returns>
        //public int Delete<T>(T t, string propertyName) where T : class
        //{
        //    IDataParameter[] param = null;
        //    string sql = Factory.CreateDeleteSql(typeof(T), t, out param,propertyName);
        //    using (IDbProvider provider = new SqlProvider())
        //    {
        //        return BaseHelper.ExecuteNonQuery( sql, param);
        //    }
        //}

        ///// <summary>
        ///// 根据泛型类型的多个属性删除数据



        ///// 泛型类型中必须存在这些属性，传



        ///// 递参数的时候不能为null
        ///// </summary>
        ///// <typeparam name="T">泛型类</typeparam>
        ///// <param name="t">泛型实例</param>
        ///// <param name="propertyNames">属性名称数组</param>
        ///// <returns></returns>
        //public int Delete<T>(T t, string[] propertyNames) where T : class
        //{
        //    IDataParameter[] param = null;
        //    string sql = Factory.CreateDeleteSql(typeof(T), t, out param, propertyNames);
        //    using (IDbProvider provider = new SqlProvider())
        //    {
        //        return BaseHelper.ExecuteNonQuery( sql, param);
        //    }
        //}

        ///// <summary>
        ///// 根据实体的类型删除数据。



        ///// value 中的类型由type确定
        ///// </summary>
        ///// <param name="type">实体类型</param>
        ///// <param name="value">实体对象实例</param>
        ///// <returns></returns>
        //public int Delete(Type type, object value)
        //{
        //    IDataParameter[] param = null;
        //    string sql = Factory.CreateDeleteSql(type,value,out param);
        //    using (IDbProvider provider = new SqlProvider())
        //    {
        //        return BaseHelper.ExecuteNonQuery( sql, param);
        //    }
        //}

        ///// <summary>
        ///// 根据实体的类型的某个属性删除数据。



        ///// value 中的类型由type确定
        ///// propertyName 属性名称必须在value 
        ///// 对象中存在



        ///// </summary>
        ///// <param name="type">实体类型</param>
        ///// <param name="value">实体对象实例</param>
        ///// <param name="propertyName">属性名称</param>
        ///// <returns></returns>
        //public int Delete(Type type, object value, string propertyName)
        //{
        //    IDataParameter[] param = null;
        //    string sql = Factory.CreateDeleteSql(type, value, out param,propertyName);
        //    using (IDbProvider provider = new SqlProvider())
        //    {
        //        return BaseHelper.ExecuteNonQuery( sql, param);
        //    }
        //}

        ///// <summary>
        ///// 根据实体的类型的某个属性删除数据。



        ///// value 中的类型由type确定
        ///// propertyName 属性名称必须在value 
        ///// 对象中存在



        ///// </summary>
        ///// <param name="type">实体类型</param>
        ///// <param name="value">实体对象实例</param>
        ///// <param name="propertyNames">属性名称数组</param>
        ///// <returns></returns>
        //public int Delete(Type type, object value, string[] propertyNames)
        //{
        //    IDataParameter[] param = null;
        //    string sql = Factory.CreateDeleteSql(type, value, out param, propertyNames);
        //    using (IDbProvider provider = new SqlProvider())
        //    {
        //        return BaseHelper.ExecuteNonQuery( sql, param);
        //    }
        //}

        /// <summary>
        /// 根据主键查询实体对象
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="pkPropertyValue">主键值</param>
        /// <returns></returns>
        public T GetEntity<T>(object pkPropertyValue) where T : class, new()
        {
            IDataParameter[] param = null;
            string sql = Factory.CreateSingleSql(typeof(T), pkPropertyValue, out param);

            return BaseHelper.ConvertToEntity<T>(BaseHelper.ExecuteDataReader(sql, param));

        }

        /// <summary>
        /// 根据表名和条件获取实体对象



        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="strTableName">表名</param>
        /// <param name="component">条件</param>
        /// <returns>实体</returns>
        public T GetEntity<T>(string pkPropertyValue, ConditionComponent component) where T : class, new()
        {
            IDataParameter[] param = null;
            //string sql = string.Format("select top 1 * from {0} ", strTableName);

            string strWhere = Factory.CreateSingleSql(typeof(T), pkPropertyValue, out param);

            if (component != null && component.sbComponent.Length > 0)
            {

                strWhere += (pkPropertyValue == null && strWhere.IndexOf(" where ") < 0 ? " where " : " and ") + component.sbComponent.ToString();
            }

            if (strOrderString != null && strOrderString.Trim() != "")
            {
                strWhere = strWhere + " order by " + strOrderString;
                strOrderString = "";
            }
            return BaseHelper.ConvertToEntity<T>(BaseHelper.ExecuteDataReader(strWhere, param));


        }


        ///// <summary>
        ///// 根据实体类的类型和主键值查询实体对象



        ///// 使用 type 确定实体，主键确定数据的唯



        ///// 一性



        ///// </summary>
        ///// <param name="type">实体类型</param>
        ///// <param name="pkPropertyValue">主键值</param>
        ///// <returns></returns>
        //public object GetEntity(Type type, object pkPropertyValue)
        //{
        //    IDataParameter[] param = null;
        //    string sql = Factory.CreateSingleSql(type,pkPropertyValue,out param);
        //    using (IDbProvider provider = new SqlProvider())
        //    {
        //        return baseHelper_.ConvertToEntity(type,BaseHelper.ExecuteDataReader(sql,param));
        //    }
        //}

        ///// <summary>
        ///// 根据某个实体的属性来查询实体对象
        ///// 该属性值能确定数据库唯一数据行



        ///// </summary>
        ///// <typeparam name="T">实体泛型类</typeparam>
        ///// <param name="propertyName">属性名称</param>
        ///// <param name="propertyValue">属性值</param>
        ///// <returns></returns>
        //public T GetEntity<T>(string propertyName, object propertyValue) where T : class
        //{
        //    IDataParameter[] param = null;
        //    string sql = Factory.CreateQueryByPropertySql(typeof(T),propertyName,propertyValue,out param,null);
        //    using (IDbProvider provider = new SqlProvider())
        //    {
        //        return BaseHelper.ConvertToEntity<T>(BaseHelper.ExecuteDataReader(sql,param));
        //    }
        //}

        ///// <summary>
        ///// 根据某个实体的属性来查询实体对象
        ///// 该属性值能确定数据库唯一数据行



        ///// </summary>
        ///// <param name="type">实体类型</param>
        ///// <param name="propertyName">属性名称</param>
        ///// <param name="propertyValue">属性值</param>
        ///// <returns></returns>
        //public object GetEntity(Type type, string propertyName, object propertyValue)
        //{
        //    IDataParameter[] param = null;
        //    string sql = Factory.CreateQueryByPropertySql(type, propertyName, propertyValue, out param,null);
        //    using (IDbProvider provider = new SqlProvider())
        //    {
        //        return BaseHelper.ConvertToEntity(type,BaseHelper.ExecuteDataReader(sql,param));
        //    }
        //}

        ///// <summary>
        ///// 根据某个实体的多个属性来查询实体对象
        ///// </summary>
        ///// <typeparam name="T">泛型类</typeparam>
        ///// <param name="entity">实体公共接口</param>
        ///// <param name="propertyNames">属性名称数组</param>
        ///// <returns></returns>
        //public T GetEntity<T>(IEntity entity, string[] propertyNames) where T : class
        //{
        //    IDataParameter[] param = null;
        //    string sql = Factory.CreateSingleSql(entity,out param,propertyNames);
        //    using (IDbProvider provider = new SqlProvider())
        //    {
        //        return BaseHelper.ConvertToEntity<T>(BaseHelper.ExecuteDataReader(sql,param));
        //    }
        //}

        ///// <summary>
        ///// 根据某个实体的多个属性来查询实体对象
        ///// 实体根据type类型来确定



        ///// </summary>
        ///// <param name="type">实体类型</param>
        ///// <param name="entity">实体公共接口</param>
        ///// <param name="propertyNames">属性名称数组</param>
        ///// <returns></returns>
        //public object GetEntity(Type type, IEntity entity, string[] propertyNames)
        //{
        //    IDataParameter[] param = null;
        //    string sql = Factory.CreateSingleSql(type,entity,out param,propertyNames);
        //    using (IDbProvider provider = new SqlProvider())
        //    {
        //        return BaseHelper.ConvertToEntity(type,BaseHelper.ExecuteDataReader(sql,param));
        //    }
        //}


        /// <summary>
        /// 查询该类型实体数据行数



        /// </summary>
        /// <param name="type">实体类型</param>
        /// <returns></returns>
        public int GetCount(Type type)
        {
            string sql = Factory.CreateConverageSql(type, Converage.Count);

            return (int)BaseHelper.ExecuteScalar(sql);

        }

        /// <summary>
        /// 查询该类型实体数据行数



        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <returns></returns>
        public int GetCount<T>() where T : class
        {
            string sql = Factory.CreateConverageSql(typeof(T), Converage.Count);

            return (int)BaseHelper.ExecuteScalar(sql);

        }

        /// <summary>
        /// 根据条件查询实体数据行数
        /// </summary>
        /// <param name="strTableName">实体类型</param>
        /// <param name="component">查询组件</param>
        /// <returns></returns>
        public int GetCount(string strTableName, ConditionComponent component)
        {

            IDataParameter[] param = null;
            string sql = string.Format(" select count(*) from {0} where 1=1 ", strTableName);
            if (component != null && component.sbComponent.Length > 0)
            {
                sql += " and " + component.sbComponent.ToString();
            }

            sql += (string.IsNullOrEmpty(strDelCol) ? "" : " and ") + strDelCol;

            return (int)BaseHelper.ExecuteScalar(sql, param);

        }


        /// <summary>
        /// 根据某个表名查询实体集合（分页）
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="strTableName">属性名称</param>
        /// <param name="fieldShow">属性名称</param>
        /// <param name="pageSize">属性名称</param>
        /// <param name="currentPageIndex">属性名称</param>
        /// <param name="intCountPage">属性值</param>
        /// <param name="component">属性值</param>
        /// <param name="isDel">是否删除0 未删除 1 已删除 2 所有</param>
        /// <returns></returns>
        public IList<T> GetList<T>(string strTableName, string fieldShow, string strKey, int pageSize,
            int currentPageIndex, int intCountPage, ConditionComponent component) where T : class
        {
            IDataParameter[] param = null;

            //StringBuilder sbSQL = new StringBuilder();
            //sbSQL.Append(" select  ");
            //sbSQL.Append(fieldShow);     //此功能暂未实现




            //sbSQL.Append(" from ");
            //sbSQL.Append(strTableName);

            //if (component != null && component.sbComponent.Length > 0)
            //{
            //    sbSQL.Append(" where ");

            //    sbSQL.Append(component.sbComponent.ToString());
            //}

            //intRowCount_ = int.Parse(BaseHelper.ExecuteScalar(sbSQL.ToString().Replace(fieldShow, "count(*)")).ToString());
            intPageNum_ = pageSize;
            //string strSql = Factory.CreatePageSql(typeof(T),pageSize, currentPageIndex, intCountPage, sbSQL.ToString());

            //if (isDel == 0)
            //{
            //    component.AddComponent("isnull(isdel,0)", "0", SearchComponent.Equals, SearchPad.And);
            //}
            //else if (isDel == 1)
            //{
            //    component.AddComponent("isdel", "1", SearchComponent.Equals, SearchPad.And);
            //}

            string strSql = Factory.CreatePageSql(strTableName, fieldShow, strKey,
                strOrderString_, pageSize, currentPageIndex,
                intCountPage, component.sbComponent.ToString(), out param);
            strOrderString_ = "";
            // return BaseHelper.Query(strSql);
            IList<T> lstRet = BaseHelper.ConvertToList<T>(BaseHelper.ExecuteDataReader(strSql, true, param));

            if (intCountPage <= 1 && (param[param.Length - 1] as SqlParameter) != null && (param[param.Length - 1] as SqlParameter).Value.ToString() != "")
            {
                intRowCount_ = Int32.Parse((param[param.Length - 1] as SqlParameter).Value.ToString());
            }
            else
            {
                intPageCount_ = intCountPage;
            }

            return lstRet;

        }

        /// <summary>
        ///// 根据某个表名查询实体集合（分页）
        ///// </summary>
        ///// <typeparam name="T">泛型类型</typeparam>
        ///// <param name="strTableName">属性名称</param>
        ///// <param name="fieldShow">属性名称</param>
        ///// <param name="pageSize">属性名称</param>
        ///// <param name="currentPageIndex">属性名称</param>
        ///// <param name="intCountPage">属性值</param>
        ///// <param name="component">属性值</param>
        ///// <returns></returns>
        //public IList<T> GetList<T>(string strTableName, string fieldShow,string strKey, int pageSize,
        //    int currentPageIndex, int intCountPage, ConditionComponent component) where T : class
        //{


        //    return GetList<T>(strTableName, fieldShow, strKey, pageSize, currentPageIndex, intCountPage, component, 0);
        //   // IDataParameter[] param = null;

        //   // //StringBuilder sbSQL = new StringBuilder();
        //   // //sbSQL.Append(" select  ");
        //   // //sbSQL.Append(fieldShow);     //此功能暂未实现



        //   // //sbSQL.Append(" from ");
        //   // //sbSQL.Append(strTableName);

        //   // //if (component != null && component.sbComponent.Length > 0)
        //   // //{
        //   // //    sbSQL.Append(" where ");

        //   // //    sbSQL.Append(component.sbComponent.ToString());
        //   // //}

        //   // //intRowCount_ = int.Parse(BaseHelper.ExecuteScalar(sbSQL.ToString().Replace(fieldShow, "count(*)")).ToString());
        //   // intPageNum_ = pageSize;
        //   // //string strSql = Factory.CreatePageSql(typeof(T),pageSize, currentPageIndex, intCountPage, sbSQL.ToString());
        //   // string strSql = Factory.CreatePageSql(strTableName, fieldShow, strKey, 
        //   //     strOrderString_, pageSize, currentPageIndex,
        //   //     intCountPage, component.sbComponent.ToString(),out param);

        //   // // return BaseHelper.Query(strSql);
        //   // IList<T> lstRet= BaseHelper.ConvertToList<T>(BaseHelper.ExecuteDataReader(strSql,true, param));

        //   // if (intCountPage <= 1 && (param[param.Length - 1] as SqlParameter) != null)
        //   // {
        //   //     intRowCount_ = Int32.Parse((param[param.Length - 1] as SqlParameter).Value.ToString());
        //   // }

        //   //return lstRet;

        //}


        ///// <summary>
        ///// 查询所有实体集合



        ///// </summary>
        ///// <typeparam name="T">泛型类型</typeparam>
        ///// <param name="iTop">泛型类型</param>
        ///// <param name="component">泛型类型</param>
        ///// <returns></returns>
        //public IList<T> GetListTop<T>(int iTop, ConditionComponent component) where T : class
        //{
        //    string strWhere=((component==null||component.sbComponent == null) ? "" : component.sbComponent.ToString());
        //    string sql = Factory.CreateQuerySql(typeof(T), strWhere,iTop);

        //        return BaseHelper.ConvertToList<T>(BaseHelper.ExecuteDataReader(sql));

        //}

        /// <summary>
        /// 根据表名查询列表
        /// </summary>
        /// <param name="iTop">返回个数 ：0 全部返回</param>
        /// <param name="strTableName">表名</param>
        /// <param name="component">条件</param>
        /// <returns></returns>
        public DataTable GetListTop(int iTop, string strTableName, ConditionComponent component)
        {
            return GetListTop(iTop, "*", strTableName, component);
        }

        /// <summary>
        /// 根据表名查询列表
        /// </summary>
        /// <param name="iTop">返回个数 ：0 全部返回</param>
        /// <param name="fieldShow">表名</param>
        /// <param name="strTableName">条件</param>
        /// <param name="component">排序</param>
        /// <returns></returns>
        public DataTable GetListTop(int iTop, string fieldShow, string strTableName, ConditionComponent component)
        {
            string strWhere = " 1=1 ";
            if (iTop > 0)
            {
                fieldShow = "top " + iTop.ToString() + " " + fieldShow;
            }
            if (component != null && component.sbComponent.Length > 0)
            {
                strWhere += " and " + component.sbComponent.ToString();
            }

            strWhere += (string.IsNullOrEmpty(strDelCol) ? "" : " and ") + strDelCol;
            //string sql = Factory.CreateQuerySql(typeof(T), strWhere);
            if (strOrderString != null && strOrderString.Trim() != "")
            {
                strWhere = strWhere + " order by " + strOrderString;
                strOrderString = "";
            }
            return BaseHelper.ExecuteTable(string.Format(" select {2} from {0} where {1}",
                strTableName, strWhere, fieldShow));

        }

        /// <summary>
        /// 根据某个实体属性查询实体集合



        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="iTop">属性名称</param>
        /// <param name="strTableName">属性值</param>
        /// <param name="component">属性值</param>
        /// <returns></returns>
        public IList<T> GetListTop<T>(int iTop, string fieldShow, string strTableName, ConditionComponent component) where T : class
        {
            IDataParameter[] param = null;
            string strWhere = ((component == null || component.sbComponent == null) ? "" : component.sbComponent.ToString());
            string sql = Factory.CreateQuerySql(typeof(T), fieldShow, strTableName, strWhere, iTop);
            strOrderString = "";
            return BaseHelper.ConvertToList<T>(BaseHelper.ExecuteDataReader(sql, param));

        }

        /// <summary>
        /// 根据某个实体属性查询实体集合




        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="iTop">属性名称</param>
        /// <param name="strTableName">属性值</param>
        /// <param name="component">属性值</param>
        /// <returns></returns>
        public IList<T> GetListTop<T>(int iTop, string fieldShow, ConditionComponent component) where T : class
        {
            IDataParameter[] param = null;
            string strWhere = ((component == null || component.sbComponent == null) ? "" : component.sbComponent.ToString());
            string sql = Factory.CreateQuerySql(typeof(T), fieldShow, strWhere, iTop);
            strOrderString = "";
            return BaseHelper.ConvertToList<T>(BaseHelper.ExecuteDataReader(sql, param));

        }
        /// <summary>
        /// 根据某个实体属性查询实体集合



        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="iTop">属性名称</param>
        /// <param name="strTableName">属性名称</param>
        /// <param name="component">属性值</param>
        /// <returns></returns>
        public IList<T> GetListTop<T>(int iTop, ConditionComponent component) where T : class
        {
            IDataParameter[] param = null;
            string strWhere = ((component == null || component.sbComponent == null) ? "" : component.sbComponent.ToString());
            string sql = Factory.CreateQuerySql(typeof(T), "*", strWhere, iTop);
            strOrderString = "";
            return BaseHelper.ConvertToList<T>(BaseHelper.ExecuteDataReader(sql, param));

        }


        /// <summary>
        /// 根据多个属性查询实体集合



        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="dic">实体公共接口</param>
        /// <returns></returns>
        public IList<T> GetListTop<T>(IDictionary<string, object> dic) where T : class
        {
            IDataParameter[] param = null;
            string sql = Factory.CreateQueryByPropertySql(typeof(T), dic, out param);
            strOrderString = "";
            return BaseHelper.ConvertToList<T>(BaseHelper.ExecuteDataReader(sql, param));

        }

        /// <summary>
        /// 根据多个属性查询实体集合



        /// 该查询方式附加查询组建



        /// </summary>
        /// <typeparam name="T">类型类</typeparam>
        /// <param name="dic">属性键值对</param>
        /// <param name="component">查询组件</param>
        /// <returns></returns>
        public IList<T> GetListTop<T>(IDictionary<string, object> dic, ConditionComponent component) where T : class
        {
            IDataParameter[] param = null;
            string sql = Factory.CreateQueryByPropertySql(typeof(T), dic, out param, component);
            strOrderString = "";
            return BaseHelper.ConvertToList<T>(BaseHelper.ExecuteDataReader(sql, param));

        }


        public int ExecuteSql(string strSql)
        {
            return BaseHelper.ExecuteNonQuery(strSql);
        }

        /// <summary>
        /// 释放对象内存
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public DataTable ExecuteSqlDatatable(string sql)
        {
            return BaseHelper.ExecuteTable(sql);
        }
    }
}
