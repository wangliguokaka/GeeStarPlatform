using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;

using D2012.DBUtility.EntityCommon;
using D2012.Common.DbCommon;

namespace D2012.DBUtility.Data.Core.OracleCore
{
    /// <summary>
    /// 数据库访问

    /// </summary>
    public class OraSqlFactory : IDbFactory
    {

        private int isDel_ = 2;

        private string strOrderBy_ = null;

        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDel
        {
            set { isDel_ = value; }
            get { return Convert.ToInt32(isDel_); }
        }

        /// <summary>
        /// 删除列字段名称
        /// </summary>
        public string strDelCol
        {
            set;
            get;
        }

        /// <summary>
        /// 排序
        /// </summary>
        public string StrOrderBy
        {
            get { return strOrderBy_; }
            set { strOrderBy_ = value; }
        }

        /// <summary>
        /// 根据实体对象公共接口创建插入的sql语句
        /// 在创建插入的sql语句的时候，判断这一列

        /// 是不是自动标识列，如果是自动标识列则
        /// 不需要人工插入该值

        /// </summary>
        /// <param name="entity">实体公共接口</param>
        /// <param name="param">创建sql语句对应占位符参数</param>
        /// <returns></returns>
        public string CreateInsertSql(IEntity entity, out IDataParameter[] param)
        {
            return CreateInsertSql(entity.GetType(), entity, out param);
        }

        /// <summary>
        /// 根据实体类型创建插入sql语句
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="value">实体对象</param>
        /// <param name="param">创建sql语句对应占位符参数</param>
        /// <returns></returns>
        public string CreateInsertSql(Type type, object value, out IDataParameter[] param)
        {
            if (value == null)
            {
                throw new NullReferenceException("the save entity is null");
            }

            string strAutoIncrement = "";
            StringBuilder sbColumn = new StringBuilder("");
            StringBuilder sbValues = new StringBuilder("");
            TableInfo tfInfo = EntityTypeCache.GetTableInfo(type);
            IDictionary<string, ColumnAttribute> dicColumn = tfInfo.DicColumns;

            //有列数

            if (dicColumn.Keys.Count > 0)
            {
                sbColumn.AppendFormat("insert into {0} (", tfInfo.Table.Name);
                sbValues.AppendFormat(" values (");
                IList<IDataParameter> listParams = new List<IDataParameter>();
                foreach (string key in dicColumn.Keys)
                {
                    object strTemp = EntityFactory.GetPropertyValue(value, key);

                    string strSeqName = tfInfo.Table.Name;
                    if (strSeqName.IndexOf(".") > 0)
                    {
                        strSeqName = strSeqName.ToLower().Replace("_v", "_s");
                    }
                    else
                    {
                        strSeqName = "S_" + strSeqName;
                    }

                    if (!dicColumn[key].AutoIncrement && strTemp != null)
                    {
                        sbColumn.AppendFormat("{0},", dicColumn[key].Name);
                        sbValues.AppendFormat(":{0},", dicColumn[key].Name);
                    }
                    else if (dicColumn[key].AutoIncrement)
                    {
                        sbColumn.AppendFormat("{0},", dicColumn[key].Name);
                        sbValues.AppendFormat("{0}.nextval,", strSeqName);

                        strAutoIncrement = string.Format(";select {0}.currval  from dual", strSeqName);
                    }

                    //object  strTemp=EntityFactory.GetPropertyValue(value, key) ;
                    if (strTemp != null)
                    //{
                    //    listParams.Add(CreateParameter(":" + key, System.DBNull.Value));
                    //}
                    //else
                    {
                        listParams.Add(CreateParameter(":" + key, dicColumn[key].DataType, 0, strTemp));
                    }
                }
                sbColumn.Replace(",", ")", sbColumn.Length - 1, 1);
                sbValues.Replace(",", ")", sbValues.Length - 1, 1);
                param = ListToArray(listParams);
                return sbColumn.ToString() + sbValues.ToString() + strAutoIncrement;
            }
            else //没有列数
            {
                param = null;
                return null;
            }
        }


        ///// <summary>
        ///// 根据泛型类型创建插入sql语句
        ///// </summary>
        ///// <typeparam name="T">泛型类型</typeparam>
        ///// <param name="t">泛型实体类</param>
        ///// <param name="param">创建sql语句对应占位符参数</param>
        ///// <returns></returns>
        //public string CreateInsertSql<T>(T t, out IDataParameter[] param) where T : IEntity
        //{
        //    return CreateInsertSql(typeof(T), t, out param);
        //}


        ///// <summary>
        ///// 根据实体对象公共接口创建修改的的sql语句
        ///// 该sql语句是根据主键列修改的

        ///// </summary>
        ///// <param name="entity">实体公共接口</param>
        ///// <param name="param">创建sql语句对应占位符参数</param>
        ///// <returns></returns>
        //public string CreateUpdateSql(IEntity entity, out IDataParameter[] param)
        //{
        //    return CreateUpdateSql(entity.GetType(), entity, out param);
        //}

        /// <summary>
        /// 根据实体对象类型创建修改的的sql语句
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="value">实体对象</param>
        /// <param name="param">创建sql语句对应占位符参数</param>
        /// <returns></returns>
        public string CreateUpdateSql(IEntity entity, out IDataParameter[] param)
        {
            //if (value == null)
            //{
            //    throw new NullReferenceException("the update entity is null");
            //}


            StringBuilder sbColumn = new StringBuilder("");
            StringBuilder sbValues = new StringBuilder("");
            TableInfo tfInfo = EntityTypeCache.GetTableInfo(entity);

            IDictionary<string, ColumnAttribute> dicColumn = tfInfo.DicColumns;

            if (EntityFactory.GetPropertyValue(entity, tfInfo.Table.PrimaryKeyName) == null)
            {
                return CreateInsertSql(entity, out param);
            }

            //有列
            if (dicColumn.Keys.Count > 0)
            {
                sbColumn.AppendFormat("update {0} set ", tfInfo.Table.Name);
                sbValues.AppendFormat(" where ");
                IList<IDataParameter> listParams = new List<IDataParameter>();
                foreach (string key in dicColumn.Keys)
                {
                    if (dicColumn[key].IsPrimaryKey)
                    {
                        sbValues.AppendFormat("{0}=:{1}", dicColumn[key].Name, dicColumn[key].Name);
                        listParams.Add(CreateParameter(":" + dicColumn[key].Name,
                            EntityFactory.GetPropertyValue(entity, key) == null ?
                            System.DBNull.Value : EntityFactory.GetPropertyValue(entity, key)));
                    }
                    else
                    {
                        object objValue = EntityFactory.GetPropertyValue(entity, key);
                        if (objValue != null)
                        {
                            sbColumn.AppendFormat("{0}=:{1},", dicColumn[key].Name, dicColumn[key].Name);
                            listParams.Add(CreateParameter(":" + dicColumn[key].Name, objValue));
                        }
                    }
                }
                sbColumn.Remove(sbColumn.Length - 1, 1);
                param = ListToArray(listParams);
                return sbColumn.ToString() + sbValues.ToString();
            }
            else //没有列
            {
                param = null;
                return null;
            }
        }

        /// <summary>
        /// 根据实体对象公共接口创建修改的的sql语句
        /// 该sql语句是根据一个特定的属性作为修改条件的
        /// </summary>
        /// <param name="entity">实体公共接口</param>
        /// <param name="param">创建sql语句对应占位符参数</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        public string CreateUpdateSql(IEntity entity, out IDataParameter[] param, string propertyName)
        {
            if (entity == null)
            {
                throw new NullReferenceException("the update entity is null");
            }
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new NullReferenceException("this property must not be null");
            }
            IDictionary<string, ColumnAttribute> dicColumn = EntityTypeCache.GetTableInfo(entity).DicColumns;
            if (!dicColumn.Keys.Contains(propertyName))
            {
                throw new Exception("this entity do not contain the property:" + propertyName);
            }
            return CreateUpdateSql(entity, out param, new string[] { propertyName });
        }

        ///// <summary>
        ///// 根据实体对象类型创建修改的的sql语句
        ///// 该sql语句是根据一个特定的属性作为修改条件的
        ///// </summary>
        ///// <param name="type">实体类型</param>
        ///// <param name="value">实体对象</param>
        ///// <param name="param">创建sql语句对应占位符参数</param>
        ///// <param name="propertyName">属性名称</param>
        ///// <returns></returns>
        //public string CreateUpdateSql(Type type, object value, out IDataParameter[] param, string propertyName)
        //{
        //    if (value == null)
        //    {
        //        throw new NullReferenceException("the update entity is null");
        //    }
        //    if (string.IsNullOrEmpty(propertyName))
        //    {
        //        throw new NullReferenceException("this property must not be null");
        //    }
        //    IDictionary<string, ColumnAttribute> dicColumn = EntityTypeCache.GetTableInfo(type).DicColumns;
        //    if (!dicColumn.Keys.Contains(propertyName))
        //    {
        //        throw new Exception("this entity do not contain the property:" + propertyName);
        //    }
        //    return CreateUpdateSql(type, value, out param, new string[] { propertyName });
        //}

        /// <summary>
        /// 根据实体对象类型创建修改的的sql语句
        /// 该sql语句是根据多个特定的属性作为修改条件的
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="value">实体对象</param>
        /// <param name="param">创建sql语句对应占位符参数</param>
        /// <param name="propertyNames">属性名称</param>
        /// <returns></returns>
        public string CreateUpdateSql(IEntity entity, out IDataParameter[] param, string[] propertyNames)
        {
            //if (value == null)
            //{
            //    throw new NullReferenceException("the update entity is null");
            //}
            if (propertyNames == null)
            {
                throw new NullReferenceException("this property array is null");
            }

            TableInfo tfInfo = EntityTypeCache.GetTableInfo(entity);



            if (string.IsNullOrEmpty(EntityFactory.GetPropertyValue(entity, tfInfo.Table.PrimaryKeyName).ToString()))
            {
                return CreateInsertSql(entity, out param);
            }


            IDictionary<string, ColumnAttribute> dicColumn = tfInfo.DicColumns;

            StringBuilder sbColumn = new StringBuilder("");
            StringBuilder sbValues = new StringBuilder("");
            if (dicColumn.Keys.Count > 0)
            {
                sbColumn.AppendFormat("update {0} set ", EntityTypeCache.GetTableInfo(entity).Table.Name);
                sbValues.AppendFormat(" where 1=1 ");
                IList<IDataParameter> listParams = new List<IDataParameter>();
                foreach (string key in dicColumn.Keys)
                {
                    if (FindArray(propertyNames, key))
                    //if (dicColumn[key].IsPrimaryKey && !string.IsNullOrEmpty(EntityFactory.GetPropertyValue(entity, key)))
                    {
                        sbValues.AppendFormat("and {0}={1} ", dicColumn[key].Name, ":" + dicColumn[key].Name);
                        listParams.Add(CreateParameter(":" + dicColumn[key].Name,
                            EntityFactory.GetPropertyValue(entity, key) == null ? DBNull.Value :
                            EntityFactory.GetPropertyValue(entity, key)));
                    }
                    else
                    {
                        //    if (dicColumn[key].IsPrimaryKey || dicColumn[key].IsUnique || dicColumn[key].AutoIncrement)
                        //    {
                        //    }
                        //    else
                        //    {
                        object strKeyValue = EntityFactory.GetPropertyValue(entity, key);
                        if (strKeyValue != null)
                        {
                            sbColumn.AppendFormat("{0}=:{1},", dicColumn[key].Name, dicColumn[key].Name);
                            listParams.Add(CreateParameter(":" + dicColumn[key].Name, strKeyValue.ToString()));
                            //    }
                        }
                    }
                }
                sbColumn.Remove(sbColumn.Length - 1, 1);
                param = ListToArray(listParams);
                return sbColumn.ToString() + sbValues.ToString();
            }
            else
            {
                param = null;
                return null;
            }
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="str">数组</param>
        /// <param name="str1">字符串</param>
        /// <returns></returns>
        private bool FindArray(string[] str, string str1)
        {
            foreach (string strTemp in str)
            {
                if (strTemp == str1)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 根据实体对象公共接口创建修改的的sql语句
        /// 该sql语句是根据查询组建创建的
        /// </summary>
        /// <param name="entity">实体公共接口</param>
        /// <param name="param">创建sql语句对应占位符参数</param>
        /// <param name="component">查询条件组件</param>
        /// <returns></returns>
        public string CreateUpdateSql(IEntity entity, out IDataParameter[] param, ConditionComponent component)
        {

            StringBuilder sbColumn = new StringBuilder("");
            StringBuilder sbValues = new StringBuilder("");
            StringBuilder sbValuesCondition = new StringBuilder("");
            IList<IDataParameter> listParams = new List<IDataParameter>();
            sbValues.AppendFormat("update {0} set ", EntityTypeCache.GetTableInfo(entity).Table.Name);

            if (component != null && component.sbComponent.Length > 0)
            {
                sbValuesCondition.Append(" where ");

                sbValuesCondition.Append(component.sbComponent.ToString());
            }

            foreach (string propertyName in EntityTypeCache.GetTableInfo(entity).DicProperties.Keys)
            {
                //    //包含则作为条件

                //    if (component.DicComponent.Keys.Contains(propertyName))
                //    {
                //        switch (component.DicComponent[propertyName])
                //        {
                //            case SearchComponent.Equals:
                //                sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, "=", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name);
                //                break;
                //            case SearchComponent.UnEquals:
                //                sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, "!=", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name);
                //                break;
                //            case SearchComponent.Between:
                //                break;
                //            case SearchComponent.Greater:
                //                sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, ">", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name);
                //                break;
                //            case SearchComponent.GreaterOrEquals:
                //                sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, ">=", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name);
                //                break;
                //            case SearchComponent.GroupBy:
                //                break;
                //            case SearchComponent.In:
                //                break;
                //            case SearchComponent.Less:
                //                sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, "<", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name);
                //                break;
                //            case SearchComponent.LessOrEquals:
                //                sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, "<=", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name);
                //                break;
                //            case SearchComponent.Like:
                //                break;
                //            case SearchComponent.Or:
                //                break;
                //            case SearchComponent.OrderAsc:
                //                break;
                //            case SearchComponent.OrderDesc:
                //                break;
                //        }
                //        listParams.Add(CreateParameter(":" + EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, EntityFactory.GetPropertyValue(entity, propertyName) == null ? DBNull.Value : EntityFactory.GetPropertyValue(entity, propertyName)));
                //    }
                //    else  //判断主键和唯一列，主键和唯一列不能被修改
                //    {
                //        if (EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].IsPrimaryKey ||
                //            EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].IsUnique ||
                //            EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].AutoIncrement)
                //        {

                //        }
                //        else
                //        {
                //            sbColumn.AppendFormat("{0}=:{1},", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name);
                //            listParams.Add(CreateParameter(":" + EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, EntityFactory.GetPropertyValue(entity, propertyName) == null ? DBNull.Value : EntityFactory.GetPropertyValue(entity, propertyName)));
                //        }
                //    }

                object objColumnValue = EntityFactory.GetPropertyValue(entity, propertyName);
                if (EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].IsPrimaryKey ||
                    EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].IsUnique ||
                    EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].AutoIncrement || objColumnValue == null)
                {

                }
                else
                {
                    sbColumn.AppendFormat("{0}=:{1},",
                        EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name,
                        EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name);
                    listParams.Add(CreateParameter(":"
                        + EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, objColumnValue));
                }

            }
            sbColumn.Remove(sbColumn.Length - 1, 1);
            param = ListToArray(listParams);
            return sbValues.ToString() + sbColumn.ToString() + sbValuesCondition.ToString();
        }

        ///// <summary>
        ///// 根据实体对象公共接口创建删除sql语句
        ///// 该sql语句是根据实体主键删除

        ///// </summary>
        ///// <param name="entity">实体公共接口</param>
        ///// <param name="param">创建sql语句对应占位符参数</param>
        ///// <returns></returns>
        //public string CreateDeleteSql(IEntity entity, out IDataParameter[] param)
        //{
        //    return CreateDeleteSql(entity.GetType(), entity, out param);
        //}

        ///// <summary>
        ///// 根据实体对象类型创建删除sql语句
        ///// 该sql语句是根据实体主键删除

        ///// </summary>
        ///// <param name="type">实体类型</param>
        ///// <param name="value">实体对象</param>
        ///// <param name="param">创建sql语句对应占位符参数</param>
        ///// <returns></returns>
        //public string CreateDeleteSql(Type type, object value, out IDataParameter[] param)
        //{
        //    if (value == null)
        //    {
        //        throw new NullReferenceException("the delete entity is null");
        //    }
        //    IDictionary<string, ColumnAttribute> dicColumn = EntityTypeCache.GetTableInfo(type).DicColumns;

        //    StringBuilder sbColumn = new StringBuilder("");
        //    StringBuilder sbValues = new StringBuilder("");
        //    if (dicColumn.Keys.Count > 0)
        //    {
        //        sbColumn.AppendFormat("delete from {0} ", EntityTypeCache.GetTableInfo(type).Table.Name);
        //        IList<IDataParameter> listParams = new List<IDataParameter>();
        //        foreach (string key in dicColumn.Keys)
        //        {
        //            if (dicColumn[key].IsPrimaryKey)
        //            {
        //                sbValues.AppendFormat("where {0}=:{1}", dicColumn[key].Name, dicColumn[key].Name);
        //                listParams.Add(CreateParameter(":" + dicColumn[key].Name, EntityFactory.GetPropertyValue(value, key) == null ? DBNull.Value : EntityFactory.GetPropertyValue(value, key)));
        //                break;
        //            }
        //        }
        //        param =ListToArray(listParams);
        //        return sbColumn.ToString() + sbValues.ToString();
        //    }
        //    else
        //    {
        //        param = null;
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// 根据实体对象公共接口的某个属性创建删除sql语句
        ///// 该sql语句是根据实体属性删除

        ///// </summary>
        ///// <param name="entity">实体公共接口</param>
        ///// <param name="param">创建sql语句对应占位符参数</param>
        ///// <param name="propertyName">实体属性名称</param>
        ///// <returns></returns>
        //public string CreateDeleteSql(IEntity entity, out IDataParameter[] param, string propertyName)
        //{
        //    if (entity == null)
        //    {
        //        throw new NullReferenceException("the delete entity is null");
        //    }
        //    if (string.IsNullOrEmpty(propertyName))
        //    {
        //        throw new NullReferenceException("this property must not be null");
        //    }
        //    return CreateDeleteSql(entity, out param, new string[] { propertyName });
        //}

        /// <summary>
        /// 根据实体对象类型的某个属性创建删除sql语句
        /// 该sql语句是根据实体属性删除

        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="value">实体对象</param>
        /// <param name="param">创建sql语句对应占位符参数</param>
        /// <param name="propertyName">实体属性名称</param>
        /// <returns></returns>
        public string CreateDeleteSql(Type type, out IDataParameter[] param, string strKeyValue, bool bolType)
        {
            //return CreateDeleteSql(type, out param, ConditionComponent.Instance().AddComponent() ); ;

            StringBuilder sbValues = new StringBuilder("");
            TableInfo tiInfo = EntityTypeCache.GetTableInfo(type);
            IList<IDataParameter> listParams = new List<IDataParameter>();

            //param = new IDataParameter[1];
            if (bolType)
            {
                sbValues.AppendFormat("delete from {0} ", tiInfo.Table.Name);
            }
            else
            {
                sbValues.AppendFormat("update  {0} set isdel=1 ", tiInfo.Table.Name);
            }


            //if (component != null && component.sbComponent.Length > 0)
            {
                sbValues.AppendFormat(" where {0}=:{0}", tiInfo.Table.PrimaryKeyName);
            }

            listParams.Add(CreateParameter(tiInfo.Table.PrimaryKeyName, strKeyValue));
            //param[0] = CreateParameter(tiInfo.Table.PrimaryKeyName, strKeyValue);
            param = ListToArray(listParams);
            return sbValues.ToString();

        }

        ///// <summary>
        ///// 根据实体对象公共接口的多个属性创建删除sql语句
        ///// 该sql语句是根据实体多个属性删除

        ///// </summary>
        ///// <param name="entity">实体公共接口</param>
        ///// <param name="param">创建sql语句对应占位符参数</param>
        ///// <param name="propertyName">实体属性名称数组</param>
        ///// <returns></returns>
        //public string CreateDeleteSql(IEntity entity, out IDataParameter[] param, string[] propertyNames)
        //{
        //    return CreateDeleteSql(entity.GetType(), entity, out param, propertyNames);
        //}

        /// <summary>
        /// 根据实体对象类型的多个属性创建删除sql语句
        /// 该sql语句是根据实体多个属性删除

        /// </summary>
        /// <param name="type">实体了姓</param>
        /// <param name="value">实体对象</param>
        /// <param name="param">创建sql语句对应占位符参数</param>
        /// <param name="propertyNames">实体属性名称数组</param>
        /// <returns></returns>
        public string CreateDeleteSql(Type type, ConditionComponent component,
            out IDataParameter[] param, bool bolType)
        {
            //if (value == null)
            //{
            //    throw new NullReferenceException("the delete entity is null");
            //}
            //if (propertyNames == null)
            //{
            //    throw new NullReferenceException("this property array is null");
            //}
            //   IDictionary<string, ColumnAttribute> dicColumn = EntityTypeCache.GetTableInfo(type).DicColumns;

            // StringBuilder sbColumn = new StringBuilder("");
            StringBuilder sbValues = new StringBuilder("");

            if (bolType)
            {
                sbValues.AppendFormat("delete from {0} ", EntityTypeCache.GetTableInfo(type).Table.Name);
            }
            else
            {
                sbValues.AppendFormat("update  {0} set isdel=1 ", EntityTypeCache.GetTableInfo(type).Table.Name);
            }


            //if (component != null && component.sbComponent.Length > 0)
            {
                sbValues.AppendFormat(" where {0}", component.sbComponent.ToString());
            }

            param = null;

            return sbValues.ToString();


            //if (dicColumn.Keys.Count > 0)
            //{
            //    sbColumn.AppendFormat("delete from {0} ", EntityTypeCache.GetTableInfo(type).Table.Name);
            //    sbValues.AppendFormat(" where 1=1 ");
            //    IList<IDataParameter> listParams = new List<IDataParameter>();
            //    foreach (string key in dicColumn.Keys)
            //    {
            //        //if (propertyNames.Contains(key))
            //        {
            //            sbValues.AppendFormat("and {0}=:{1} ", dicColumn[key].Name, dicColumn[key].Name);
            //            if (EntityFactory.GetPropertyValue(value, key) != null)
            //            {
            //                listParams.Add(CreateParameter(":" + dicColumn[key].Name, EntityFactory.GetPropertyValue(value, key) == null ? DBNull.Value : EntityFactory.GetPropertyValue(value, key)));
            //            }
            //            else
            //            {
            //                throw new NullReferenceException("this column " + dicColumn[key].Name + " value is null");
            //            }
            //        }
            //    }
            //    param =ListToArray(listParams);
            //    return sbColumn.ToString() + sbValues.ToString();
            //}
            //else
            //{
            //    param = null;
            //    return null;
            //}
        }


        ///// <summary>
        ///// 根据实体对象公共接口的多个属性创建删除sql语句
        ///// 该sql语句使根据查询组建来创建的

        ///// </summary>
        ///// <param name="entity">实体公共接口</param>
        ///// <param name="param">创建sql语句对应占位符参数</param>
        ///// <param name="component">实体属性名称数组</param>
        ///// <returns></returns>
        //public string CreateDeleteSql(IEntity entity, out IDataParameter[] param, ConditionComponent component)
        //{
        //    if (entity == null)
        //    {
        //        throw new NullReferenceException("the delete entity is null");
        //    }
        //    IList<IDataParameter> listParams = new List<IDataParameter>();
        //   // StringBuilder sbColumn = new StringBuilder("");
        //    StringBuilder sbValues = new StringBuilder("");
        //    sbValues.AppendFormat("delete from {0} where 1=1 and ", EntityTypeCache.GetTableInfo(entity).Table.Name);

        //    if (component!=null && component.sbComponent.Length > 0)
        //    {
        //        sbValues.Append(component.sbComponent.ToString());
        //    }
        //    //foreach (string propertyName in component.DicComponent.Keys)
        //    //{
        //    //    switch (component.DicComponent[propertyName])
        //    //    {
        //    //        case SearchComponent.Equals:
        //    //            sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, "=", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name);
        //    //            break;
        //    //        case SearchComponent.UnEquals:
        //    //            sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, "!=", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name);
        //    //            break;
        //    //        case SearchComponent.Between:
        //    //            break;
        //    //        case SearchComponent.Greater:
        //    //            sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, ">", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name);
        //    //            break;
        //    //        case SearchComponent.GreaterOrEquals:
        //    //            sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, ">=", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name);
        //    //            break;
        //    //        case SearchComponent.GroupBy:
        //    //            break;
        //    //        case SearchComponent.In:
        //    //            break;
        //    //        case SearchComponent.Less:
        //    //            sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, "<", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name);
        //    //            break;
        //    //        case SearchComponent.LessOrEquals:
        //    //            sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, "<=", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name);
        //    //            break;
        //    //        case SearchComponent.Like:
        //    //            break;
        //    //        case SearchComponent.Or:
        //    //            break;
        //    //        case SearchComponent.OrderAsc:
        //    //            break;
        //    //        case SearchComponent.OrderDesc:
        //    //            break;
        //    //    }
        //    //    listParams.Add(CreateParameter(":" + EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, EntityFactory.GetPropertyValue(entity, propertyName) == null ? DBNull.Value : EntityFactory.GetPropertyValue(entity, propertyName)));
        //    //}
        //    param =ListToArray(listParams);
        //    return  sbValues.ToString();
        //}

        /// <summary>
        /// 组合条件
        /// </summary>
        /// <param name="sbWhere">条件</param>
        /// <param name="tb">表信息</param>
        private void genWhereEx(StringBuilder sbWhere, TableInfo tb)
        {
            if (IsDel == 0 && tb.Table.IsDel)
            {
                sbWhere.Append(" and nvl(isdel,0)=0");
            }
            else if (IsDel == 1 && tb.Table.IsDel)
            {
                sbWhere.Append(" and nvl(isdel,0)=1");
            }
            if (!string.IsNullOrEmpty(strOrderBy_))
            {
                sbWhere.AppendFormat(" order by {0}", strOrderBy_);
            }
        }

        /// <summary>
        /// 根据实体的公共接口创建查询单行数据的接口
        /// 该sql语句是根据数据库表的主键来查询的
        /// </summary>
        /// <param name="entity">实体公共接口</param>
        /// <param name="param">创建sql语句对应占位符参数</param>
        /// <returns></returns>
        public string CreateSingleSql(IEntity entity, out IDataParameter[] param)
        {
            if (entity == null)
            {
                throw new NullReferenceException("the entity is null");
            }
            TableInfo tfInfo = EntityTypeCache.GetTableInfo(entity);
            StringBuilder sbValues = new StringBuilder("");
            sbValues.AppendFormat("select * from {0} ", tfInfo.Table.Name);
            IList<IDataParameter> listParams = new List<IDataParameter>();
            sbValues.AppendFormat("where {0}=:{1}", tfInfo.Table.PrimaryKeyName, tfInfo.Table.PrimaryKeyName);

            genWhereEx(sbValues, tfInfo);

            listParams.Add(CreateParameter(":" + tfInfo.Table.PrimaryKeyName,
                EntityFactory.GetPropertyValue(entity, tfInfo.Table.PrimaryKeyName) == null ?
                DBNull.Value : EntityFactory.GetPropertyValue(entity, tfInfo.Table.PrimaryKeyName)));
            param = ListToArray(listParams);
            return sbValues.ToString();
        }

        /// <summary>
        /// 根据实体的公共接口创建查询单行数据的接口
        /// 该sql语句是根据实体的相应属性来查询
        /// </summary>
        /// <param name="entity">实体公共接口</param>
        /// <param name="param">创建sql语句对应占位符参数</param>
        /// <param name="propertyNames">属性名称数组</param>
        /// <returns></returns>
        public string CreateSingleSql(IEntity entity, out IDataParameter[] param, string[] propertyNames)
        {
            return CreateSingleSql(entity.GetType(), entity, out param, propertyNames);
        }

        /// <summary>
        /// 根据实体类型创建查询单行数据的sql语句
        /// 该sql语句是根据实体的相应属性来查询
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="value">实体对象</param>
        /// <param name="param">创建sql语句对应占位符参数</param>
        /// <param name="propertyNames">属性名称数组</param>
        /// <returns></returns>
        public string CreateSingleSql(Type type, object value, out IDataParameter[] param, string[] propertyNames)
        {
            if (value == null)
            {
                throw new NullReferenceException("the entity is null");
            }

            TableInfo tfInfo = EntityTypeCache.GetTableInfo(type);
            IList<IDataParameter> listParams = new List<IDataParameter>();
            StringBuilder sbValues = new StringBuilder("");
            sbValues.AppendFormat("select * from {0} ", tfInfo.Table.Name);
            sbValues.AppendFormat("where 1=1 ");
            foreach (string propertyName in propertyNames)
            {
                sbValues.AppendFormat(" and {0}=:{1}", tfInfo.DicProperties[propertyName].Name,
                    tfInfo.DicProperties[propertyName].Name);
                if (EntityFactory.GetPropertyValue(value, propertyName) != null)
                {
                    listParams.Add(CreateParameter(":" + tfInfo.DicProperties[propertyName].Name,
                        EntityFactory.GetPropertyValue(value, propertyName) == null ? DBNull.Value :
                        EntityFactory.GetPropertyValue(value, propertyName)));
                }
                else
                {
                    throw new NullReferenceException("this column " + tfInfo.DicProperties[propertyName].Name + " value is null");
                }
            }
            //sbValues.Remove(sbValues.Length - 4, 4);

            genWhereEx(sbValues, tfInfo);

            param = ListToArray(listParams);
            return sbValues.ToString();
        }

        /// <summary>
        /// 根据实体的类型创建查询sql语句
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns></returns>
        public string CreateSingleSql(Type entityType)
        {
            StringBuilder sbValues = new StringBuilder();
            sbValues.AppendFormat("select * from {0} where {1}=:{2}",
                EntityTypeCache.GetTableInfo(entityType).Table.Name,
                EntityTypeCache.GetTableInfo(entityType).Table.PrimaryKeyName,
                EntityTypeCache.GetTableInfo(entityType).Table.PrimaryKeyName);
            genWhereEx(sbValues, EntityTypeCache.GetTableInfo(entityType));
            return sbValues.ToString();
        }

        /// <summary>
        /// 根据实体的类型创建查询sql语句,
        /// 该方法指定主键值

        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="pkPropertyValue">主键值</param>
        /// <param name="param">创建sql语句对应占位符参数</param>
        /// <returns></returns>
        public string CreateSingleSql(Type type, object pkPropertyValue, out IDataParameter[] param)
        {
            IList<IDataParameter> listParams = new List<IDataParameter>();
            StringBuilder sbValues = new StringBuilder();
            TableInfo tfInfo = EntityTypeCache.GetTableInfo(type);
            sbValues.AppendFormat("select * from {0} where {1}=:{2}",
                tfInfo.Table.Name, tfInfo.Table.PrimaryKeyName, tfInfo.Table.PrimaryKeyName);
            listParams.Add(CreateParameter(":" + tfInfo.Table.PrimaryKeyName, pkPropertyValue));

            genWhereEx(sbValues, tfInfo);


            param = ListToArray(listParams);
            return sbValues.ToString();
        }


        /// <summary>
        /// 根据实体的类型创建查询该实体对象对应数据库表的所有数据的sql语句
        /// 该sql语句用于查询所有数据，并转换为相应List 集合
        /// </summary>
        /// <param name="type">实体的类型</param>
        /// <returns></returns>
        public string CreateQuerySql(Type type)
        {
            StringBuilder sbValues = new StringBuilder();
            sbValues.AppendFormat("select * from {0}", EntityTypeCache.GetTableInfo(type).Table.Name);

            genWhereEx(sbValues, EntityTypeCache.GetTableInfo(type));
            return sbValues.ToString();
        }

        /// <summary>
        /// 根据实体的类型创建查询该实体对象对应数据库表的所有数据的sql语句
        /// 该sql语句用于查询所有数据，并转换为相应List 集合
        /// </summary>
        /// <param name="type">实体的类型</param>
        /// <param name="strWhere">SQL</param>
        /// <returns></returns>
        public string CreateQuerySql(Type type, string fieldShow, string strWhere, int iTop)
        {
            //StringBuilder sbValues = new StringBuilder();
            //sbValues.AppendFormat("select * from {0}", EntityTypeCache.GetTableInfo(type).Table.Name);
            ////if (iTop > 0)
            ////{
            ////    sbValues.AppendFormat(" where rownum<={0}", iTop);
            ////    if (!string.IsNullOrEmpty(strWhere))
            ////    {
            ////        sbValues.AppendFormat(" and ", strWhere); 
            ////    }
            ////}
            //if (!string.IsNullOrEmpty(strWhere))
            //{
            //    sbValues.AppendFormat(" where {0}", strWhere);
            //}

            //genWhereEx(sbValues, EntityTypeCache.GetTableInfo(type));
            //return sbValues.ToString();
            return CreateQuerySql(type, fieldShow, EntityTypeCache.GetTableInfo(type).Table.Name, strWhere, iTop);
        }

        /// <summary>
        /// 根据实体的类型创建查询该实体对象对应数据库表的所有数据的sql语句
        /// 该sql语句用于查询所有数据，并转换为相应List 集合
        /// </summary>
        /// <param name="type">实体的类型</param>
        /// <param name="strWhere">SQL</param>
        /// <returns></returns>
        public string CreateQuerySql(Type type, string fieldShow, string strTableName, string strWhere, int iTop)
        {
            StringBuilder sbValues = new StringBuilder();
            sbValues.AppendFormat("select * from {0}", strTableName);
            //if (iTop > 0)
            //{
            //    sbValues.AppendFormat(" where rownum<={0}", iTop);
            //    if (!string.IsNullOrEmpty(strWhere))
            //    {
            //        sbValues.AppendFormat(" and ", strWhere); 
            //    }
            //}
            if (!string.IsNullOrEmpty(strWhere))
            {
                sbValues.AppendFormat(" where {0}", strWhere);
            }

            genWhereEx(sbValues, EntityTypeCache.GetTableInfo(type));
            return sbValues.ToString();
        }

        /// <summary>
        /// 根据实体的某个属性创建根据该属性字段查询数据的sql语句
        /// 该sql语句是使用参数中属性对应字段作为条件查询的
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">属性值</param>
        /// <param name="param">sql语句占位符参数</param>
        /// <param name="strWhere">SQL</param>
        /// <returns></returns>
        public string CreateQueryByPropertySql(Type type, string propertyName,
            object value, out IDataParameter[] param, string strWhere)
        {
            StringBuilder sbValues = new StringBuilder();
            IList<IDataParameter> listParams = new List<IDataParameter>();
            TableInfo tfInfo = EntityTypeCache.GetTableInfo(type);
            sbValues.AppendFormat("select * from {0} where {1}=:{2}",
                tfInfo.Table.Name, tfInfo.DicColumns[propertyName].Name, tfInfo.DicColumns[propertyName].Name);
            listParams.Add(CreateParameter(":" + tfInfo.DicColumns[propertyName].Name, value));
            param = ListToArray(listParams);

            if (!string.IsNullOrEmpty(strWhere))
            {
                sbValues.AppendFormat(" and {0}", strWhere);
            }

            genWhereEx(sbValues, tfInfo);

            return sbValues.ToString();
        }

        /// <summary>
        /// 根据实体的某些属性创建根据该些属性字段查询数据的sql语句
        /// 该sql语句是使用参数中属性对应字段作为条件查询的，并且该
        /// 属性集合都是根据and条件组装的

        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="dic">属性-值集合</param>
        /// <param name="param">sql语句占位符参数</param>
        /// <returns></returns>
        public string CreateQueryByPropertySql(Type type,
            IDictionary<string, object> dic, out IDataParameter[] param)
        {
            StringBuilder sbValues = new StringBuilder();
            IList<IDataParameter> listParams = new List<IDataParameter>();
            TableInfo tfInfo = EntityTypeCache.GetTableInfo(type);
            sbValues.AppendFormat("select * from {0} where 1=1", tfInfo.Table.Name);
            foreach (string key in dic.Keys)
            {
                sbValues.AppendFormat(" and {0}=:{1}", tfInfo.DicColumns[key].Name, tfInfo.DicColumns[key].Name);
                listParams.Add(CreateParameter(":" + tfInfo.DicColumns[key].Name, dic[key]));
            }
            genWhereEx(sbValues, tfInfo);
            param = ListToArray(listParams);
            return sbValues.ToString();
        }

        /// <summary>
        /// 根据实体的某些属性创建根据该些属性字段查询数据的sql语句
        /// 该sql语句是使用参数中属性对应字段作为条件查询的，并且查
        /// 询是根据查询组建来创建

        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="dic">属性-值集合</param>
        /// <param name="param">sql语句占位符参数</param>
        /// <param name="component">查询组建</param>
        /// <returns></returns>
        public string CreateQueryByPropertySql(Type type,
            IDictionary<string, object> dic, out IDataParameter[] param, ConditionComponent component)
        {
            IList<IDataParameter> listParams = new List<IDataParameter>();
            // StringBuilder sbColumn = new StringBuilder("");
            StringBuilder sbValues = new StringBuilder("");
            sbValues.AppendFormat("select * from {0} ", EntityTypeCache.GetTableInfo(type).Table.Name);

            if (component != null && component.sbComponent.Length > 0)
            {
                sbValues.Append(" where ");
                sbValues.Append(component.sbComponent.ToString());
            }

            //foreach (string propertyName in dic.Keys)
            //{
            //    switch (component.DicComponent[propertyName])
            //    {
            //        case SearchComponent.Equals:
            //            sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name, "=", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name);
            //            break;
            //        case SearchComponent.UnEquals:
            //            sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name, "!=", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name);
            //            break;
            //        case SearchComponent.Between:
            //            break;
            //        case SearchComponent.Greater:
            //            sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name, ">", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name);
            //            break;
            //        case SearchComponent.GreaterOrEquals:
            //            sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name, ">=", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name);
            //            break;
            //        case SearchComponent.GroupBy:
            //            break;
            //        case SearchComponent.In:
            //            break;
            //        case SearchComponent.Less:
            //            sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name, "<", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name);
            //            break;
            //        case SearchComponent.LessOrEquals:
            //            sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name, "<=", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name);
            //            break;
            //        case SearchComponent.Like:
            //            break;
            //        case SearchComponent.Or:
            //            break;
            //        case SearchComponent.OrderAsc:
            //            break;
            //        case SearchComponent.OrderDesc:
            //            break;
            //    }
            //    listParams.Add(CreateParameter(":" + EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name, dic[propertyName]));
            //}

            genWhereEx(sbValues, EntityTypeCache.GetTableInfo(type));
            param = ListToArray(listParams);
            return sbValues.ToString();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="type">实例</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="currentPageIndex">当前页</param>
        /// <param name="CountPage">总页数</param>
        /// <param name="strSQL">SQL</param>
        /// <returns></returns>
        public string CreatePageSql(Type type, int pageSize, int currentPageIndex, int intCountPage, string strSQL)
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append(" select * from ( select row_.*, rownum rownum_ from (");
            sbSQL.Append(strSQL);
            genWhereEx(sbSQL, EntityTypeCache.GetTableInfo(type));
            sbSQL.AppendFormat(" ) row_ where rownum <= {1}) where rownum_ >= {0}",
                pageSize * (currentPageIndex - 1) + 1, pageSize * currentPageIndex);
            return sbSQL.ToString();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="currentPageIndex">当前页</param>
        /// <param name="CountPage">总页数</param>
        /// <param name="strSQL">SQL</param>
        /// <param name="isDel">isDel</param>
        /// <returns></returns>
        public string CreatePageSql(int pageSize, int currentPageIndex, int intCountPage, string strSQL, int isDel)
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append(" select * from ( select row_.*, rownum rownum_ from (");
            sbSQL.Append(strSQL);

            if (isDel == 0)
            {
                sbSQL.Append(" and nvl(isdel,0)=0");
            }
            else if (isDel == 1)
            {
                sbSQL.Append(" and nvl(isdel,0)=1");
            }
            if (!string.IsNullOrEmpty(strOrderBy_))
            {
                sbSQL.AppendFormat(" order by {0}", strOrderBy_);
            }

            sbSQL.AppendFormat(" ) row_ where rownum <= {1}) where rownum_ >= {0}",
                pageSize * (currentPageIndex - 1) + 1, pageSize * currentPageIndex);
            return sbSQL.ToString();
        }

        public string CreatePageSql(string strTableName, string strFieldShow, string strFieldKey, string strOrder,
            int pageSize, int currentPageIndex, int intCountPage, string strWhere, out IDataParameter[] param)
        {
            param = null;
            return null;
        }
        /// <summary>
        /// 根据实体类型来创建该实体对应数据库表的聚合函数查询sql语句
        /// 该方法创建的sql语句主要是用于查询数据行数

        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="converage">聚合函数枚举类型</param>
        /// <returns></returns>
        public string CreateConverageSql(Type type, Converage converage)
        {
            StringBuilder sbValues = new StringBuilder();
            if (Converage.Count == converage)
            {
                sbValues.AppendFormat("select count() from {0}", EntityTypeCache.GetTableInfo(type).Table.Name);
            }
            else if (Converage.CountNotNll == converage)
            {
                sbValues.AppendFormat("select count(*) from {0}", EntityTypeCache.GetTableInfo(type).Table.Name);
            }
            genWhereEx(sbValues, EntityTypeCache.GetTableInfo(type));

            return sbValues.ToString();
        }

        /// <summary>
        /// 根据实体类型来创建该实体对应数据库表的聚合函数查询sql语句
        /// 该方法创建的sql语句主要是用于统计查询(最大值,最小值,求和,平均值,数据行数)
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="converage">聚合函数枚举类型</param>
        /// <param name="propertyName">聚合函数作用的属性名称</param>
        /// <returns></returns>
        public string CreateConverageSql(Type type, Converage converage, string propertyName)
        {
            StringBuilder sbValues = new StringBuilder();
            if (string.IsNullOrEmpty(propertyName))
            {
                converage = Converage.Count;
            }
            if (Converage.Avg == converage)
            {
                sbValues.AppendFormat("select avg({0}) from {1}",
                    EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name, EntityTypeCache.GetTableInfo(type).Table.Name);
            }
            else if (Converage.Max == converage)
            {
                sbValues.AppendFormat("select max({0}) from {1}",
                    EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name, EntityTypeCache.GetTableInfo(type).Table.Name);
            }
            else if (Converage.Min == converage)
            {
                sbValues.AppendFormat("select min({0}) from {1}",
                    EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name, EntityTypeCache.GetTableInfo(type).Table.Name);
            }
            else
            {
                sbValues.AppendFormat("select count(*) from {1}",
                    EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name, EntityTypeCache.GetTableInfo(type).Table.Name);
            }
            genWhereEx(sbValues, EntityTypeCache.GetTableInfo(type));
            return sbValues.ToString();
        }

        /// <summary>
        /// 根据实体类型来创建该实体对应数据库表的聚合函数查询sql语句
        /// 该方法创建的sql语句主要是用于统计查询(最大值,最小值,求和,平均值,数据行数)，

        /// 同时该sql是有条件查询的

        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="converage">聚合函数枚举类型</param>
        /// <param name="propertyName">聚合函数作用的属性名称</param>
        /// <param name="dic">查询条件属性键值</param>
        /// <param name="param">查询条件组建对象</param>
        /// <param name="component">查询条件组建对象</param>
        /// <returns></returns>
        public string CreateConverageSql(Type type, Converage converage,
            string propertyName, IDictionary<string, object> dic,
            out IDataParameter[] param, ConditionComponent component)
        {
            StringBuilder sbValues = new StringBuilder();
            if (string.IsNullOrEmpty(propertyName))
            {
                converage = Converage.Count;
            }
            if (Converage.Avg == converage)
            {
                sbValues.AppendFormat("select avg({0}) from {1}  ",
                    EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name,
                    EntityTypeCache.GetTableInfo(type).Table.Name);
            }
            else if (Converage.Max == converage)
            {
                sbValues.AppendFormat("select max({0}) from {1} ",
                    EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name,
                    EntityTypeCache.GetTableInfo(type).Table.Name);
            }
            else if (Converage.Min == converage)
            {
                sbValues.AppendFormat("select min({0}) from {1}  ",
                    EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name,
                    EntityTypeCache.GetTableInfo(type).Table.Name);
            }
            else
            {
                sbValues.AppendFormat("select count(*) from {0}  ",
                    EntityTypeCache.GetTableInfo(type).Table.Name);
            }

            IList<IDataParameter> listParams = new List<IDataParameter>();


            if (component != null && component.sbComponent.Length > 0)
            {
                sbValues.Append(" where ");
                sbValues.Append(component.sbComponent.ToString());
            }


            //foreach (string key in dic.Keys)
            //{
            //    switch (component.DicComponent[key])
            //    {
            //        case SearchComponent.Equals:
            //            sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name, "=", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name);
            //            break;
            //        case SearchComponent.UnEquals:
            //            sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name, "!=", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name);
            //            break;
            //        case SearchComponent.Between:
            //            break;
            //        case SearchComponent.Greater:
            //            sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name, ">", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name);
            //            break;
            //        case SearchComponent.GreaterOrEquals:
            //            sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name, ">=", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name);
            //            break;
            //        case SearchComponent.GroupBy:
            //            break;
            //        case SearchComponent.In:
            //            break;
            //        case SearchComponent.Less:
            //            sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name, "<", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name);
            //            break;
            //        case SearchComponent.LessOrEquals:
            //            sbValues.AppendFormat("and {0}{1}:{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name, "<=", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name);
            //            break;
            //        case SearchComponent.Like:
            //            break;
            //        case SearchComponent.Or:
            //            break;
            //        case SearchComponent.OrderAsc:
            //            break;
            //        case SearchComponent.OrderDesc:
            //            break;
            //    }
            //    listParams.Add(CreateParameter(":" + EntityTypeCache.GetTableInfo(type).DicColumns[key].Name, dic[key]));
            //}
            genWhereEx(sbValues, EntityTypeCache.GetTableInfo(type));
            param = ListToArray(listParams);
            return sbValues.ToString();
        }



        /// <summary>
        /// 数据库类型的转化
        /// </summary>
        /// <param name="type">程序中的类型</param>
        /// <returns></returns>
        private OracleType ConvertType(DataType type)
        {
            OracleType sqlType = OracleType.Number;
            switch (type)
            {
                case DataType.Int:
                    sqlType = OracleType.Number;
                    break;
                case DataType.Char:
                    sqlType = OracleType.Char;
                    break;
                case DataType.Datetime:
                    sqlType = OracleType.DateTime;
                    break;
                case DataType.Nvarchar:
                    sqlType = OracleType.NVarChar;
                    break;
                case DataType.Varchar:
                    sqlType = OracleType.VarChar;
                    break;
                case DataType.Clob:
                    sqlType = OracleType.Clob;
                    break;
                case DataType.Blob:
                    sqlType = OracleType.Blob;
                    break;
                default:
                    sqlType = OracleType.NVarChar;
                    break;

            }
            return sqlType;
        }


        /// <summary>
        /// 根据占位符名称创建参数

        /// </summary>
        /// <param name="name">占位符名称</param>
        /// <returns></returns>
        public IDataParameter CreateParameter(string name)
        {
            return CreateParameter(name, null);
        }

        /// <summary>
        /// 根据占位符和值创建参数

        /// </summary>
        /// <param name="name">占位符名称</param>
        /// <param name="value">占位符的值</param>
        /// <returns></returns>
        public IDataParameter CreateParameter(string name, object value)
        {
            OracleParameter dpPara = new OracleParameter(name, value);
            return dpPara;
        }

        /// <summary>
        /// 根据占位符名称，类型和值创建参数

        /// </summary>
        /// <param name="name">占位符名称</param>
        /// <param name="type">参数的类型</param>
        /// <param name="value">参数的值</param>
        /// <returns></returns>
        public IDataParameter CreateParameter(string name, DataType type, object value)
        {
            OracleParameter dpPara = new OracleParameter(name, ConvertType(type));
            dpPara.Value = value;
            return dpPara;
        }

        /// <summary>
        /// 根据占位符的名称，类型和大小创建参数
        /// </summary>
        /// <param name="name">占位符名称</param>
        /// <param name="type">参数类型</param>
        /// <param name="size">参数值大小</param>
        /// <returns></returns>
        public IDataParameter CreateParameter(string name, DataType type, int size)
        {
            OracleParameter dpPara = new OracleParameter(name, ConvertType(type));
            if (size > 0)
            {
                dpPara.Size = size;
            }
            return dpPara;
        }

        /// <summary>
        /// 根据占位符的名称，类型，大小和值创建参数

        /// </summary>
        /// <param name="name">占位符名称</param>
        /// <param name="type">参数类型</param>
        /// <param name="size">参数大小</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public IDataParameter CreateParameter(string name, DataType type, int size, object value)
        {
            OracleParameter dpPara = new OracleParameter(name, ConvertType(type));
            if (size > 0)
            {
                dpPara.Size = size;
            }
            dpPara.Value = value;
            return dpPara;
        }

        /// <summary>
        /// 根据占位符名称和类型创建参数
        /// </summary>
        /// <param name="name">占位符名称</param>
        /// <param name="type">占位符类型</param>
        /// <returns></returns>
        public IDataParameter CreateParameter(string name, DataType type)
        {
            OracleParameter dpPara = new OracleParameter(name, ConvertType(type));
            return dpPara;
        }

        /// <summary>
        /// list转数组

        /// </summary>
        /// <param name="listPara">List</param>
        /// <returns></returns>
        private IDataParameter[] ListToArray(IList<IDataParameter> listPara)
        {
            IDataParameter[] arrPara = new OracleParameter[listPara.Count];

            for (int i = 0; i < listPara.Count; i++)
            {
                arrPara[i] = listPara[i];
            }

            return arrPara;
        }

        /// <summary>
        /// 释放对象占用的内存

        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
