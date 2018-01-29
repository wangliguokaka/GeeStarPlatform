using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

using D2012.DBUtility.EntityCommon;
using D2012.Common.DbCommon;

namespace D2012.DBUtility.Data.Core.SQLCore
{
    /// <summary>
    /// 映射SQL Server 数据库。


    /// </summary>
    public class SqlFactory : IDbFactory
    {

        private int isDel_;
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

        private string StrDel
        {
            get
            {
                //if (IsDel == 0)
                //{
                //   // return string.Format(" {0}=0 ", string.IsNullOrEmpty(strDelCol) ? "isnull(isdel,0)" : strDelCol);
                //}
                //else if (IsDel == 1)
                //{
                //    return string.Format(" {0}=1 ", string.IsNullOrEmpty(strDelCol) ? "isnull(isdel,0)" : strDelCol);
                //}
                return "";
            }
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
            StringBuilder sbColumn = new StringBuilder("");
            StringBuilder sbValues = new StringBuilder("");
            IDictionary<string, ColumnAttribute> dicColumn = EntityTypeCache.GetTableInfo(type).DicColumns;
            bool indentity = false;
            //dicColumn
            if (dicColumn.Keys.Count > 0)
            {
                sbColumn.AppendFormat("insert into {0} (", EntityTypeCache.GetTableInfo(type).Table.Name);
                sbValues.AppendFormat(" values (");
                IList<IDataParameter> listParams = new List<IDataParameter>();
                foreach (string key in dicColumn.Keys)
                {
                    //if (!dicColumn[key].AutoIncrement)
                    //{
                    //    sbColumn.AppendFormat("{0},", dicColumn[key].Name);
                    //    sbValues.AppendFormat("{0},", "@" + dicColumn[key].Name);
                    //}
                    if (EntityFactory.GetPropertyValue(value, key) == null)
                    {
                        //listParams.Add(CreateParameter("@" + key, System.DBNull.Value));
                    }
                    else
                    {
                        if (!dicColumn[key].AutoIncrement)
                        {
                            sbColumn.AppendFormat("{0},", dicColumn[key].Name);
                            sbValues.AppendFormat("{0},", "@" + dicColumn[key].Name);
                        }
                        else {
                            indentity = true;
                        }
                        listParams.Add(CreateParameter("@" + key, EntityFactory.GetPropertyValue(value, key)));
                    }
                }
                sbColumn.Replace(",", ")", sbColumn.Length - 1, 1);
                sbValues.Replace(",", ")", sbValues.Length - 1, 1);
                param = ListToArray(listParams);
                if (indentity)
                {
                    return sbColumn.ToString() + sbValues.ToString() + ";select @@IDENTITY";
                }
                else {
                    return sbColumn.ToString() + sbValues.ToString();
                }
                
            }
            else //dicColumn
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


        /// <summary>
        /// 根据实体对象公共接口创建修改的的sql语句
        /// 该sql语句是根据主键列修改的


        /// </summary>
        /// <param name="entity">实体公共接口</param>
        /// <param name="param">创建sql语句对应占位符参数</param>
        /// <returns></returns>
        public string CreateUpdateSql(IEntity entity, out IDataParameter[] param)
        {
            return CreateUpdateSql(entity, out param, new ConditionComponent());
        }

        /// <summary>
        /// 根据实体对象类型创建修改的的sql语句
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="value">实体对象</param>
        /// <param name="param">创建sql语句对应占位符参数</param>
        /// <returns></returns>
        //public string CreateUpdateSql(Type type, object value, out IDataParameter[] param)
        //{
        //    if (value == null)
        //    {
        //        throw new NullReferenceException("the update entity is null");
        //    }
        //    StringBuilder sbColumn = new StringBuilder("");
        //    StringBuilder sbValues = new StringBuilder("");
        //    IDictionary<string, ColumnAttribute> dicColumn = EntityTypeCache.GetTableInfo(type).DicColumns;
        //    if (dicColumn.Keys.Count > 0)
        //    {
        //        sbColumn.AppendFormat("update {0} set ", EntityTypeCache.GetTableInfo(type).Table.Name);
        //        sbValues.AppendFormat(" where ");
        //        IList<IDataParameter> listParams = new List<IDataParameter>();
        //        foreach (string key in dicColumn.Keys)
        //        {
        //            if (dicColumn[key].IsPrimaryKey)
        //            {
        //                sbValues.AppendFormat("{0}=@{1}", dicColumn[key].Name, dicColumn[key].Name);
        //                listParams.Add(CreateParameter("@" + dicColumn[key].Name, EntityFactory.GetPropertyValue(value, key) == null ? System.DBNull.Value : EntityFactory.GetPropertyValue(value, key)));
        //            }
        //            else
        //            {
        //                if (dicColumn[key].IsPrimaryKey || dicColumn[key].IsUnique || dicColumn[key].AutoIncrement)
        //                {
        //                }
        //                else
        //                {
        //                    sbColumn.AppendFormat("{0}=@{1},", dicColumn[key].Name, dicColumn[key].Name);
        //                    listParams.Add(CreateParameter("@" + dicColumn[key].Name, EntityFactory.GetPropertyValue(value, key) == null ? DBNull.Value : EntityFactory.GetPropertyValue(value, key)));
        //                }
        //            }
        //        }
        //        sbColumn.Remove(sbColumn.Length - 1, 1);
        //        param =ListToArray(listParams);
        //        return sbColumn.ToString() + sbValues.ToString();
        //    }
        //    else
        //    {
        //        param = null;
        //        return null;
        //    }
        //}

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

        /// <summary>
        /// 根据实体对象类型创建修改的的sql语句
        /// 该sql语句是根据一个特定的属性作为修改条件的
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="value">实体对象</param>
        /// <param name="param">创建sql语句对应占位符参数</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
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
        /// 根据实体对象公共接口创建修改的的sql语句
        /// 该sql语句是根据多个特定的属性作为修改条件的
        /// </summary>
        /// <param name="entity">实体公共接口</param>
        /// <param name="param">创建sql语句对应占位符参数</param>
        /// <param name="propertyNames">属性名称</param>
        /// <returns></returns>
        public string CreateUpdateSql(IEntity entity, out IDataParameter[] param, string[] propertyNames)
        {
            return CreateUpdateSql(entity, out param, propertyNames);
        }

        /// <summary>
        /// 根据实体对象类型创建修改的的sql语句
        /// 该sql语句是根据多个特定的属性作为修改条件的
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="value">实体对象</param>
        /// <param name="param">创建sql语句对应占位符参数</param>
        /// <param name="propertyNames">属性名称</param>
        /// <returns></returns>
        //public string CreateUpdateSql(Type type, object value, out IDataParameter[] param, string[] propertyNames)
        //{
        //    if (value == null)
        //    {
        //        throw new NullReferenceException("the update entity is null");
        //    }
        //    if (propertyNames == null)
        //    {
        //        throw new NullReferenceException("this property array is null");
        //    }
        //    IDictionary<string, ColumnAttribute> dicColumn = EntityTypeCache.GetTableInfo(type).DicColumns;

        //    StringBuilder sbColumn = new StringBuilder("");
        //    StringBuilder sbValues = new StringBuilder("");
        //    if (dicColumn.Keys.Count > 0)
        //    {
        //        sbColumn.AppendFormat("update {0} set ", EntityTypeCache.GetTableInfo(type).Table.Name);
        //        sbValues.AppendFormat(" where 1=1 ");
        //        IList<IDataParameter> listParams = new List<IDataParameter>();
        //        foreach (string key in dicColumn.Keys)
        //        {
        //           // if (propertyNames.Contains(key))
        //            {
        //                sbValues.AppendFormat("and {0}={1} ", dicColumn[key].Name, "@" + dicColumn[key].Name);
        //                listParams.Add(CreateParameter("@" + dicColumn[key].Name, EntityFactory.GetPropertyValue(value, key) == null ? DBNull.Value : EntityFactory.GetPropertyValue(value, key)));
        //            }
        //            //else
        //            //{
        //            //    if (dicColumn[key].IsPrimaryKey || dicColumn[key].IsUnique || dicColumn[key].AutoIncrement)
        //            //    {
        //            //    }
        //            //    else
        //            //    {
        //            //        sbColumn.AppendFormat("{0}={1},", dicColumn[key].Name, "@" + dicColumn[key].Name);
        //            //        listParams.Add(CreateParameter("@" + dicColumn[key].Name, EntityFactory.GetPropertyValue(value, key) == null ? DBNull.Value : EntityFactory.GetPropertyValue(value, key)));
        //            //    }
        //            //}
        //        }
        //        sbColumn.Remove(sbColumn.Length - 1, 1);
        //        param =ListToArray(listParams);
        //        return sbColumn.ToString() + sbValues.ToString();
        //    }
        //    else
        //    {
        //        param = null;
        //        return null;
        //    }
        //}

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
            TableInfo tfInfo = EntityTypeCache.GetTableInfo(entity);

            IDictionary<string, ColumnAttribute> dicColumn = tfInfo.DicColumns;

            if ((EntityFactory.GetPropertyValue(entity, tfInfo.Table.PrimaryKeyName) == null || EntityFactory.GetPropertyValue(entity, tfInfo.Table.PrimaryKeyName).ToString() == "0") && (component.sbComponent == null || component.sbComponent.ToString() == ""))
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
                        sbValues.AppendFormat("{0}=@{1}", dicColumn[key].Name, dicColumn[key].Name);
                        listParams.Add(CreateParameter("@" + dicColumn[key].Name,
                            EntityFactory.GetPropertyValue(entity, key) == null ?
                            System.DBNull.Value : EntityFactory.GetPropertyValue(entity, key)));
                    }
                    else
                    {
                        object objValue = EntityFactory.GetPropertyValue(entity, key);
                        if (objValue != null)
                        {
                            sbColumn.AppendFormat("{0}=@{1},", dicColumn[key].Name, dicColumn[key].Name);
                            listParams.Add(CreateParameter("@" + dicColumn[key].Name, objValue));
                        }
                    }
                }
                sbColumn.Remove(sbColumn.Length - 1, 1);
                param = ListToArray(listParams);

                //isdel 


                if (component.sbComponent != null && !string.IsNullOrEmpty(component.sbComponent.ToString()))
                {
                    sbValues.Append(" and ");
                    sbValues.Append(component.sbComponent.ToString());
                }

                return sbColumn.ToString() + sbValues.ToString();
            }
            else //没有列

            {
                param = null;
                return null;
            }
        }

        /// <summary>
        /// 根据实体对象公共接口创建删除sql语句
        /// 该sql语句是根据实体主键删除


        /// </summary>
        /// <param name="entity">实体公共接口</param>
        /// <param name="param">创建sql语句对应占位符参数</param>
        /// <returns></returns>
        //public string CreateDeleteSql(IEntity entity, out IDataParameter[] param)
        //{
        //    return CreateDeleteSql(entity.GetType(), entity, out param);
        //}

        /// <summary>
        /// 根据实体对象类型创建删除sql语句
        /// 该sql语句是根据实体主键删除


        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="value">实体对象</param>
        /// <param name="param">创建sql语句对应占位符参数</param>
        /// <returns></returns>
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
        //                sbValues.AppendFormat("where {0}=@{1}", dicColumn[key].Name, dicColumn[key].Name);
        //                listParams.Add(CreateParameter("@" + dicColumn[key].Name, EntityFactory.GetPropertyValue(value, key) == null ? DBNull.Value : EntityFactory.GetPropertyValue(value, key)));
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

        /// <summary>
        /// 根据实体对象公共接口的某个属性创建删除sql语句
        /// 该sql语句是根据实体属性删除


        /// </summary>
        /// <param name="entity">实体公共接口</param>
        /// <param name="param">创建sql语句对应占位符参数</param>
        /// <param name="propertyName">实体属性名称</param>
        /// <returns></returns>
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
        //public string CreateDeleteSql(Type type, object value, out IDataParameter[] param, string propertyName)
        //{
        //    return CreateDeleteSql(type, value, out param, new string[] { propertyName }); ;
        //}

        /// <summary>
        /// 根据实体对象公共接口的多个属性创建删除sql语句
        /// 该sql语句是根据实体多个属性删除


        /// </summary>
        /// <param name="entity">实体公共接口</param>
        /// <param name="param">创建sql语句对应占位符参数</param>
        /// <param name="propertyName">实体属性名称数组</param>
        /// <returns></returns>
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
        //public string CreateDeleteSql(Type type, object value, out IDataParameter[] param, string[] propertyNames)
        //{
        //    if (value == null)
        //    {
        //        throw new NullReferenceException("the delete entity is null");
        //    }
        //    if (propertyNames == null)
        //    {
        //        throw new NullReferenceException("this property array is null");
        //    }
        //    IDictionary<string, ColumnAttribute> dicColumn = EntityTypeCache.GetTableInfo(type).DicColumns;

        //    StringBuilder sbColumn = new StringBuilder("");
        //    StringBuilder sbValues = new StringBuilder("");
        //    if (dicColumn.Keys.Count > 0)
        //    {
        //        sbColumn.AppendFormat("delete from {0} ", EntityTypeCache.GetTableInfo(type).Table.Name);
        //        sbValues.AppendFormat(" where 1=1 ");
        //        IList<IDataParameter> listParams = new List<IDataParameter>();
        //        foreach (string key in dicColumn.Keys)
        //        {
        //            //if (propertyNames.Contains(key))
        //            {
        //                sbValues.AppendFormat("and {0}=@{1} ", dicColumn[key].Name, dicColumn[key].Name);
        //                if (EntityFactory.GetPropertyValue(value, key) != null)
        //                {
        //                    listParams.Add(CreateParameter("@" + dicColumn[key].Name, EntityFactory.GetPropertyValue(value, key) == null ? DBNull.Value : EntityFactory.GetPropertyValue(value, key)));
        //                }
        //                else
        //                {
        //                    throw new NullReferenceException("this column " + dicColumn[key].Name + " value is null");
        //                }
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
        //    StringBuilder sbColumn = new StringBuilder("");
        //    StringBuilder sbValues = new StringBuilder("");
        //    sbColumn.AppendFormat("delete from {0} where 1=1 and ", EntityTypeCache.GetTableInfo(entity).Table.Name);

        //    if (component != null && component.sbComponent.Length > 0)
        //    {
        //        sbValues.Append(component.sbComponent.ToString());
        //    }


        //    //foreach (string propertyName in component.DicComponent.Keys)
        //    //{
        //    //    switch (component.DicComponent[propertyName])
        //    //    {
        //    //        case SearchComponent.Equals:
        //    //            sbValues.AppendFormat("and {0}{1}@{2} ", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, "=", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name);
        //    //            break;
        //    //        case SearchComponent.UnEquals:
        //    //            sbValues.AppendFormat("and {0}{1}@{2} ", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, "!=", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name);
        //    //            break;
        //    //        case SearchComponent.Between:
        //    //            break;
        //    //        case SearchComponent.Greater:
        //    //            sbValues.AppendFormat("and {0}{1}@{2} ", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, ">", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name);
        //    //            break;
        //    //        case SearchComponent.GreaterOrEquals:
        //    //            sbValues.AppendFormat("and {0}{1}@{2} ", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, ">=", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name);
        //    //            break;
        //    //        case SearchComponent.GroupBy:
        //    //            break;
        //    //        case SearchComponent.In:
        //    //            break;
        //    //        case SearchComponent.Less:
        //    //            sbValues.AppendFormat("and {0}{1}@{2} ", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, "<", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name);
        //    //            break;
        //    //        case SearchComponent.LessOrEquals:
        //    //            sbValues.AppendFormat("and {0}{1}@{2} ", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, "<=", EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name);
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
        //    //    listParams.Add(CreateParameter("@" + EntityTypeCache.GetTableInfo(entity).DicColumns[propertyName].Name, EntityFactory.GetPropertyValue(entity, propertyName) == null ? DBNull.Value : EntityFactory.GetPropertyValue(entity, propertyName)));
        //    //}
        //    param =ListToArray(listParams);
        //    return sbColumn.ToString() + sbValues.ToString();
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
            param = new IDataParameter[1];
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
                sbValues.AppendFormat(" where {0}=@{0}", tiInfo.Table.PrimaryKeyName);
            }
            param[0] = new SqlParameter(tiInfo.Table.PrimaryKeyName, strKeyValue);

            return sbValues.ToString();

        }


        /// <summary>
        /// 根据实体对象类型的多个属性创建删除sql语句
        /// 该sql语句是根据实体多个属性删除


        /// </summary>
        /// <param name="type">实体了姓</param>
        /// <param name="value">实体对象</param>
        /// <param name="param">创建sql语句对应占位符参数</param>
        /// <param name="propertyNames">实体属性名称数组</param>
        /// <returns></returns>
        public string CreateDeleteSql(Type type, ConditionComponent component, out IDataParameter[] param, bool bolType)
        {
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
            StringBuilder sbValues = new StringBuilder("");
            sbValues.AppendFormat("select * from {0} ", EntityTypeCache.GetTableInfo(entity).Table.Name);
            IList<IDataParameter> listParams = new List<IDataParameter>();
            sbValues.AppendFormat("where {0}=@{1}",
                EntityTypeCache.GetTableInfo(entity).Table.PrimaryKeyName,
                EntityTypeCache.GetTableInfo(entity).Table.PrimaryKeyName);
            listParams.Add(CreateParameter("@"
                + EntityTypeCache.GetTableInfo(entity).Table.PrimaryKeyName,
                EntityFactory.GetPropertyValue(entity,
                EntityTypeCache.GetTableInfo(entity).Table.PrimaryKeyName) == null ?
                DBNull.Value : EntityFactory.GetPropertyValue(entity,
                EntityTypeCache.GetTableInfo(entity).Table.PrimaryKeyName)));

            param = ListToArray(listParams);
            return sbValues.ToString() + (StrDel == "" ? "" : " and ") + StrDel;
        }

        /// <summary>
        /// 根据实体的公共接口创建查询单行数据的接口
        /// 该sql语句是根据实体的相应属性来查询
        /// </summary>
        /// <param name="entity">实体公共接口</param>
        /// <param name="param">创建sql语句对应占位符参数</param>
        /// <param name="propertyNames">创建sql语句对应占位符参数</param>
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
            IList<IDataParameter> listParams = new List<IDataParameter>();
            StringBuilder sbValues = new StringBuilder("");
            sbValues.AppendFormat("select * from {0} ", EntityTypeCache.GetTableInfo(type).Table.Name);
            sbValues.AppendFormat("where ");

            //propertyNames
            foreach (string propertyName in propertyNames)
            {
                sbValues.AppendFormat("{0}=@{1} and ",
                    EntityTypeCache.GetTableInfo(type).DicProperties[propertyName].Name,
                    EntityTypeCache.GetTableInfo(type).DicProperties[propertyName].Name);
                if (EntityFactory.GetPropertyValue(value, propertyName) != null)
                {
                    listParams.Add(CreateParameter("@"
                        + EntityTypeCache.GetTableInfo(type).DicProperties[propertyName].Name,
                        EntityFactory.GetPropertyValue(value, propertyName) == null ?
                        DBNull.Value : EntityFactory.GetPropertyValue(value, propertyName)));
                }
                else
                {
                    throw new NullReferenceException("this column "
                        + EntityTypeCache.GetTableInfo(type).DicProperties[propertyName].Name + " value is null");
                }
            }
            sbValues.Remove(sbValues.Length - 4, 4);



            param = ListToArray(listParams);
            return sbValues.ToString() + (StrDel == "" ? "" : " and ") + StrDel;
        }

        /// <summary>
        /// 根据实体的类型创建查询sql语句
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns></returns>
        public string CreateSingleSql(Type entityType)
        {
            StringBuilder sbValues = new StringBuilder();
            sbValues.AppendFormat("select * from {0} where {1}=@{2}",
                EntityTypeCache.GetTableInfo(entityType).Table.Name,
                EntityTypeCache.GetTableInfo(entityType).Table.PrimaryKeyName,
                EntityTypeCache.GetTableInfo(entityType).Table.PrimaryKeyName);
            return sbValues.ToString() + (StrDel == "" ? "" : " and ") + StrDel;
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
            sbValues.AppendFormat("select * from {0}",
                EntityTypeCache.GetTableInfo(type).Table.Name);

            if (pkPropertyValue != null)
            {
                sbValues.AppendFormat(" where {0}=@{1}",
                EntityTypeCache.GetTableInfo(type).Table.PrimaryKeyName,
                EntityTypeCache.GetTableInfo(type).Table.PrimaryKeyName);
                listParams.Add(CreateParameter("@"
                    + EntityTypeCache.GetTableInfo(type).Table.PrimaryKeyName, pkPropertyValue));

            }

            if (StrDel != "")
            {
                if (pkPropertyValue == null)
                {
                    sbValues.Append(" where " + StrDel);
                }
                else
                {
                    sbValues.Append(" and " + StrDel);
                }
            }

            param = ListToArray(listParams);
            return sbValues.ToString();
        }


        /// <summary>
        /// 根据实体的类型创建查询该实体对象对应数据库表的所有数据的sql语句
        /// 该sql语句用于查询所有数据，并转换为相应List 集合
        /// </summary>
        /// <param name="type">实体的类型</param>
        /// <param name="strWhere">实体的类型</param>
        /// <returns></returns>
        public string CreateQuerySql(Type type, string fieldShow, string strWhere, int iTop)
        {
            //StringBuilder sbValues = new StringBuilder();
            //if (string.IsNullOrEmpty(fieldShow))
            //{
            //    fieldShow = "*";
            //}
            //if (!string.IsNullOrEmpty(strWhere))
            //{
            //    fieldShow = " where " + fieldShow;
            //}
            //sbValues.AppendFormat("select top {1} {2} from {0} {3}", EntityTypeCache.GetTableInfo(type).Table.Name, iTop, fieldShow, strWhere);
            //return sbValues.ToString();

            return CreateQuerySql(type, fieldShow, EntityTypeCache.GetTableInfo(type).Table.Name, strWhere, iTop);
        }

        /// <summary>
        /// 根据实体的类型创建查询该实体对象对应数据库表的所有数据的sql语句
        /// 该sql语句用于查询所有数据，并转换为相应List 集合
        /// </summary>
        /// <param name="type">实体的类型</param>
        /// <param name="strWhere">实体的类型</param>
        /// <returns></returns>
        public string CreateQuerySql(Type type, string fieldShow, string strTableName, string strWhere, int iTop)
        {
            StringBuilder sbValues = new StringBuilder();
            if (string.IsNullOrEmpty(fieldShow))
            {
                fieldShow = "*";
            }

            strWhere += (string.IsNullOrEmpty(strWhere) || StrDel == "" ? "" : " and ") + StrDel;

            if (!string.IsNullOrEmpty(strWhere))
            {
                strWhere = " where " + strWhere;
            }

            if (!string.IsNullOrEmpty(StrOrderBy))
            {
                strWhere += " order by  " + StrOrderBy;

            }

            if (iTop == 0) { iTop = 100000; }
            sbValues.AppendFormat("select top {1} {2} from {0} {3}", strTableName, iTop, fieldShow, strWhere);
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
        /// <param name="strWhere">sql语句占位符参数</param>
        /// <returns></returns>
        public string CreateQueryByPropertySql(Type type, string propertyName,
            object value, out IDataParameter[] param, string strWhere)
        {
            StringBuilder sbValues = new StringBuilder();
            IList<IDataParameter> listParams = new List<IDataParameter>();
            sbValues.AppendFormat("select * from {0} where {1}=@{2}",
                EntityTypeCache.GetTableInfo(type).Table.Name,
                EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name,
                EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name);
            listParams.Add(CreateParameter("@"
                + EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name, value));
            param = ListToArray(listParams);
            return sbValues.ToString() + (StrDel == "" ? "" : " and ") + StrDel;
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
        public string CreateQueryByPropertySql(Type type, IDictionary<string, object> dic, out IDataParameter[] param)
        {
            StringBuilder sbValues = new StringBuilder();
            IList<IDataParameter> listParams = new List<IDataParameter>();
            sbValues.AppendFormat("select * from {0} where 1=1", EntityTypeCache.GetTableInfo(type).Table.Name);
            foreach (string key in dic.Keys)
            {
                sbValues.AppendFormat(" and {0}=@{1}",
                    EntityTypeCache.GetTableInfo(type).DicColumns[key].Name,
                    EntityTypeCache.GetTableInfo(type).DicColumns[key].Name);
                listParams.Add(CreateParameter("@"
                    + EntityTypeCache.GetTableInfo(type).DicColumns[key].Name, dic[key]));
            }
            param = ListToArray(listParams);
            return sbValues.ToString() + (StrDel == "" ? "" : " and ") + StrDel;
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
        public string CreateQueryByPropertySql(Type type, IDictionary<string, object> dic,
            out IDataParameter[] param, ConditionComponent component)
        {
            IList<IDataParameter> listParams = new List<IDataParameter>();
            // StringBuilder sbColumn = new StringBuilder("");
            StringBuilder sbValues = new StringBuilder("");
            sbValues.AppendFormat("select * from {0} where 1=1 ", EntityTypeCache.GetTableInfo(type).Table.Name);

            if (component != null && component.sbComponent.Length > 0)
            {
                sbValues.Append(" and ");
                sbValues.Append(component.sbComponent.ToString());
            }

            //foreach (string propertyName in dic.Keys)
            //{
            //    switch (component.DicComponent[propertyName])
            //    {
            //        case SearchComponent.Equals:
            //            sbValues.AppendFormat("and {0}{1}@{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name, "=", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name);
            //            break;
            //        case SearchComponent.UnEquals:
            //            sbValues.AppendFormat("and {0}{1}@{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name, "!=", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name);
            //            break;
            //        case SearchComponent.Between:
            //            break;
            //        case SearchComponent.Greater:
            //            sbValues.AppendFormat("and {0}{1}@{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name, ">", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name);
            //            break;
            //        case SearchComponent.GreaterOrEquals:
            //            sbValues.AppendFormat("and {0}{1}@{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name, ">=", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name);
            //            break;
            //        case SearchComponent.GroupBy:
            //            break;
            //        case SearchComponent.In:
            //            break;
            //        case SearchComponent.Less:
            //            sbValues.AppendFormat("and {0}{1}@{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name, "<", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name);
            //            break;
            //        case SearchComponent.LessOrEquals:
            //            sbValues.AppendFormat("and {0}{1}@{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name, "<=", EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name);
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
            //    listParams.Add(CreateParameter("@" + EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name, dic[propertyName]));
            //}
            param = ListToArray(listParams);
            return sbValues.ToString() + (StrDel == "" ? "" : " and ") + StrDel;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="currentPageIndex">页码</param>
        /// <param name="CountPage">总页数</param>
        /// <param name="strSQL">SQL</param>
        /// <returns></returns>
        public string CreatePageSql(Type type, int pageSize, int currentPageIndex, int intCountPage, string strSQL)
        {
            return "";
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="currentPageIndex">页码</param>
        /// <param name="CountPage">总页数</param>
        /// <param name="strSQL">SQL</param>
        /// <param name="isDel">删除</param>
        /// <returns></returns>
        public string CreatePageSql(int pageSize, int currentPageIndex, int intCountPage, string strSQL, int isDel)
        {
            return "";
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="currentPageIndex">页码</param>
        /// <param name="CountPage">总页数</param>
        /// <param name="strSQL">SQL</param>
        /// <param name="isDel">删除</param>
        /// <returns></returns>
        public string CreatePageSql(string strTableName, string strFieldShow, string strFieldKey, string strOrder,
            int pageSize, int currentPageIndex, int intCountPage, string strWhere, out IDataParameter[] param)
        {
            IList<IDataParameter> listParams = new List<IDataParameter>();
            listParams.Add(new SqlParameter("tbname", strTableName));
            //表名 
            listParams.Add(new SqlParameter("FieldKey", strFieldKey));
            //主键 
            listParams.Add(new SqlParameter("PageCurrent", currentPageIndex));
            //当前页 
            listParams.Add(new SqlParameter("PageSize", pageSize));
            // 每页大小 
            listParams.Add(new SqlParameter("FieldShow", strFieldShow));
            //显示列 
            listParams.Add(new SqlParameter("FieldOrder", strOrder));
            //排序列 
            listParams.Add(new SqlParameter("Where", strWhere + (StrDel == "" || string.IsNullOrEmpty(strWhere) ? "" : " and ") + StrDel));
            //条件 
            //sqlParam[7] = new SqlParameter("PageCount", CountPage);
            listParams.Add(new SqlParameter("PageCount", intCountPage));
            ////总页 
            SqlParameter sqlParamOut = new SqlParameter("RowCount", 0);
            sqlParamOut.Direction = ParameterDirection.Output;
            listParams.Add(sqlParamOut);
            //总页 

            param = ListToArray(listParams);
            //return "sp_PageGetCommNew";
            return "sp_PageGetCommNew";
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
                    EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name,
                    EntityTypeCache.GetTableInfo(type).Table.Name);
            }
            else if (Converage.Max == converage)
            {
                sbValues.AppendFormat("select max({0}) from {1}",
                    EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name,
                    EntityTypeCache.GetTableInfo(type).Table.Name);
            }
            else if (Converage.Min == converage)
            {
                sbValues.AppendFormat("select min({0}) from {1}",
                    EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name,
                    EntityTypeCache.GetTableInfo(type).Table.Name);
            }
            else
            {
                sbValues.AppendFormat("select count(*) from {1}",
                    EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name,
                    EntityTypeCache.GetTableInfo(type).Table.Name);
            }

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
                sbValues.AppendFormat("select avg({0}) from {1} where 1=1 ",
                    EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name,
                    EntityTypeCache.GetTableInfo(type).Table.Name);
            }
            else if (Converage.Max == converage)
            {
                sbValues.AppendFormat("select max({0}) from {1} where 1=1 ",
                    EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name,
                    EntityTypeCache.GetTableInfo(type).Table.Name);
            }
            else if (Converage.Min == converage)
            {
                sbValues.AppendFormat("select min({0}) from {1} where 1=1 ",
                    EntityTypeCache.GetTableInfo(type).DicColumns[propertyName].Name,
                    EntityTypeCache.GetTableInfo(type).Table.Name);
            }
            else
            {
                sbValues.AppendFormat("select count(*) from {0} where 1=1 ",
                    EntityTypeCache.GetTableInfo(type).Table.Name);
            }

            IList<IDataParameter> listParams = new List<IDataParameter>();

            if (component != null && component.sbComponent.Length > 0)
            {
                sbValues.Append(" and ");
                sbValues.Append(component.sbComponent.ToString());
            }

            //foreach (string key in dic.Keys)
            //{
            //    switch (component.DicComponent[key])
            //    {
            //        case SearchComponent.Equals:
            //            sbValues.AppendFormat("and {0}{1}@{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name, "=", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name);
            //            break;
            //        case SearchComponent.UnEquals:
            //            sbValues.AppendFormat("and {0}{1}@{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name, "!=", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name);
            //            break;
            //        case SearchComponent.Between:
            //            break;
            //        case SearchComponent.Greater:
            //            sbValues.AppendFormat("and {0}{1}@{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name, ">", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name);
            //            break;
            //        case SearchComponent.GreaterOrEquals:
            //            sbValues.AppendFormat("and {0}{1}@{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name, ">=", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name);
            //            break;
            //        case SearchComponent.GroupBy:
            //            break;
            //        case SearchComponent.In:
            //            break;
            //        case SearchComponent.Less:
            //            sbValues.AppendFormat("and {0}{1}@{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name, "<", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name);
            //            break;
            //        case SearchComponent.LessOrEquals:
            //            sbValues.AppendFormat("and {0}{1}@{2} ", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name, "<=", EntityTypeCache.GetTableInfo(type).DicColumns[key].Name);
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
            //    listParams.Add(CreateParameter("@" + EntityTypeCache.GetTableInfo(type).DicColumns[key].Name, dic[key]));
            //}
            param = ListToArray(listParams);
            return sbValues.ToString() + (StrDel == "" ? "" : " and ") + StrDel;
        }



        /// <summary>
        /// 数据库类型的转化
        /// </summary>
        /// <param name="type">程序中的类型</param>
        /// <returns></returns>
        private SqlDbType ConvertType(DataType type)
        {
            SqlDbType sqlType = SqlDbType.BigInt;

            //程序中的类型
            switch (type)
            {
                case DataType.Bigint:
                    sqlType = SqlDbType.BigInt;
                    break;
                case DataType.Binary:
                    sqlType = SqlDbType.Binary;
                    break;
                case DataType.Bit:
                    sqlType = SqlDbType.Bit;
                    break;
                case DataType.Char:
                    sqlType = SqlDbType.Char;
                    break;
                case DataType.Datetime:
                    sqlType = SqlDbType.DateTime;
                    break;
                case DataType.Decimal:
                    sqlType = SqlDbType.Decimal;
                    break;
                case DataType.Float:
                    sqlType = SqlDbType.Float;
                    break;
                case DataType.Image:
                    sqlType = SqlDbType.Image;
                    break;
                case DataType.Int:
                    sqlType = SqlDbType.Int;
                    break;
                case DataType.Money:
                    sqlType = SqlDbType.Money;
                    break;
                case DataType.Nchar:
                    sqlType = SqlDbType.NChar;
                    break;
                case DataType.Ntext:
                    sqlType = SqlDbType.NText;
                    break;
                case DataType.Numeric:
                    sqlType = SqlDbType.Decimal;
                    break;
                case DataType.Nvarchar:
                    sqlType = SqlDbType.NVarChar;
                    break;
                case DataType.Real:
                    sqlType = SqlDbType.Float;
                    break;
                case DataType.Smalldatetime:
                    sqlType = SqlDbType.SmallDateTime;
                    break;
                case DataType.Smallint:
                    sqlType = SqlDbType.SmallInt;
                    break;
                case DataType.Smallmoney:
                    sqlType = SqlDbType.SmallMoney;
                    break;
                case DataType.Text:
                    sqlType = SqlDbType.Text;
                    break;
                case DataType.Timestamp:
                    sqlType = SqlDbType.Timestamp;
                    break;
                case DataType.Tinyint:
                    sqlType = SqlDbType.TinyInt;
                    break;
                case DataType.Uniqueidentifier:
                    sqlType = SqlDbType.UniqueIdentifier;
                    break;
                case DataType.Varbinary:
                    sqlType = SqlDbType.VarBinary;
                    break;
                case DataType.Varchar:
                    sqlType = SqlDbType.VarChar;
                    break;
                case DataType.Variant:
                    sqlType = SqlDbType.Variant;
                    break;
                default:
                    sqlType = SqlDbType.NVarChar;
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
            SqlParameter spPara = new SqlParameter(name, value);
            return spPara;
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
            SqlParameter spPara = new SqlParameter(name, ConvertType(type));
            spPara.Value = value;
            return spPara;
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
            SqlParameter spPara = new SqlParameter(name, ConvertType(type));
            if (size > 0)
            {
                spPara.Size = size;
            }
            return spPara;
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
            SqlParameter spPara = new SqlParameter(name, ConvertType(type));
            if (size > 0)
            {
                spPara.Size = size;
            }
            spPara.Value = value;
            return spPara;
        }

        /// <summary>
        /// 根据占位符名称和类型创建参数
        /// </summary>
        /// <param name="name">占位符名称</param>
        /// <param name="type">占位符类型</param>
        /// <returns></returns>
        public IDataParameter CreateParameter(string name, DataType type)
        {
            SqlParameter spPara = new SqlParameter(name, ConvertType(type));
            return spPara;
        }

        /// <summary>
        /// List转数组


        /// </summary>
        /// <param name="listPara">List</param>
        /// <returns></returns>
        private IDataParameter[] ListToArray(IList<IDataParameter> listPara)
        {
            IDataParameter[] arrPara = new SqlParameter[listPara.Count];

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
