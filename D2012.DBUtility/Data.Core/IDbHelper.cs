using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using D2012.DBUtility.EntityCommon;
using D2012.Common.DbCommon;

namespace D2012.DBUtility.Data.Core
{
    /// <summary>
    /// IDbHelper
    /// </summary>
    public interface IDbHelper : IDisposable
    {
        /// <summary>
        /// 添加实体对象
        /// 将实体数据信息映射为一条插入sql语句
        /// </summary>
        /// <param name="entity">实体公共接口</param>
        /// <returns></returns>
        int Add(IEntity entity);

        /// <summary>
        /// 添加实体对象
        /// value 的类型必须和type一致，这样才能保存值

        /// </summary>
        /// <param name="type">实体公共接口</param>
        /// <returns></returns>
        int AddOrUpdate(IEntity entity);


        /// <summary>
        /// 根据实体公共接口修改该实体信息

        /// 该实体是根据主键修改
        /// </summary>
        /// <param name="entity">实体公共接口</param>
        /// <returns></returns>
        int Update(IEntity entity);

        /// <summary>
        /// 根据实体的某个属性修改数据

        /// entity 中必须包含该属性，而且该属性的
        /// 值不能为空

        /// </summary>
        /// <param name="entity">实体公共接口</param>
        /// <param name="propertyName">实体属性名称</param>
        /// <returns></returns>
        int Update(IEntity entity, string propertyName);

        /// <summary>
        /// 根据实体的多个属性修改数据

        /// 数组中的属性名称必须存在.
        /// 传递参数数组不能为null
        /// </summary>
        /// <param name="entity">实体公共接口</param>
        /// <param name="propertyNames">属性名称数组</param>
        /// <returns></returns>
        int Update(IEntity entity, string[] propertyNames);

        /// <summary>
        /// 根据实体的多个属性来修改数据信息
        /// 该方法使用Type 来确定需要修改的实体对象
        /// 数组中的属性名称在泛型类中必须存在，

        /// 而且数组传递参数不能为null
        /// </summary>
        /// <param name="entity">实体类型</param>
        /// <param name="component">实体实例</param>
        /// <returns></returns>
        int Update(IEntity entity, ConditionComponent component);


        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">泛型类</typeparam>
        /// <param name="strKeyValue">主键></param>
        /// <param name="bolType">删除标记</param>
        /// <returns></returns>
        int Delete<T>(string strKeyValue, bool bolType) where T : IEntity;

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">泛型类</typeparam>
        /// <param name="component">条件</param>
        /// <param name="bolType">删除标记</param>
        /// <returns></returns>
        int Delete<T>(ConditionComponent component, bool bolType) where T : IEntity;

        /// <summary>
        /// 根据主键查询实体对象
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="pkPropertyValue">主键值</param>
        /// <returns></returns>
        T GetEntity<T>(object pkPropertyValue) where T : class, new();

        /// <summary>
        /// 根据表名和条件获取实体对象

        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="strTableName">表名</param>
        /// <param name="component">条件</param>
        /// <returns>实体</returns>
        T GetEntity<T>(string strTableName, ConditionComponent component) where T : class, new();

        /// <summary>
        /// 查询该类型实体数据行数

        /// </summary>
        /// <param name="type">实体类型</param>
        /// <returns></returns>
        int GetCount(Type type);

        /// <summary>
        /// 查询该类型实体数据行数

        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <returns></returns>
        int GetCount<T>() where T : class;

        /// <summary>
        /// 根据条件查询实体数据行数
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="dic">实体属性键值对</param>
        /// <param name="component">查询组件</param>
        /// <returns></returns>
        int GetCount(string strTableName, ConditionComponent component);

        /// <summary>
        /// 查询所有实体集合

        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="iTop">实体类型</param>
        /// <param name="component">实体属性键值对</param>
        /// <returns></returns>
        IList<T> GetListTop<T>(int iTop, ConditionComponent component) where T : class;

        /// <summary>
        /// 根据某个实体属性查询实体集合
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="iTop">属性名称</param>
        /// <param name="strTableName">属性名称</param>
        /// <param name="component">属性值</param>
        /// <returns></returns>
        IList<T> GetListTop<T>(int iTop, string fieldShow, ConditionComponent component) where T : class;
        /// <summary>
        /// 根据某个实体属性查询实体集合
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="iTop">属性名称</param>
        /// <param name="strTableName">属性名称</param>
        /// <param name="component">属性值</param>
        /// <returns></returns>
        IList<T> GetListTop<T>(int iTop, string fieldShow, string strTableName, ConditionComponent component) where T : class;

        /// <summary>
        /// 根据多个属性查询实体集合

        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="dic">实体公共接口</param>
        /// <returns></returns>
        IList<T> GetListTop<T>(IDictionary<string, object> dic) where T : class;

        /// <summary>
        /// 根据多个属性查询实体集合

        /// 该查询方式附加查询组建

        /// </summary>
        /// <typeparam name="T">类型类</typeparam>
        /// <param name="dic">属性键值对</param>
        /// <param name="component">查询组件</param>
        /// <returns></returns>
        IList<T> GetListTop<T>(IDictionary<string, object> dic, ConditionComponent component) where T : class;
    }
}
