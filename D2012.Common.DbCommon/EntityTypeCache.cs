using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D2012.Common.DbCommon
{
    /// <summary>
    /// 内存缓存器
    /// </summary>
    public static class EntityTypeCache
    {
        //表实体信息存储器，用于存储表实体信息
        private static IDictionary<Type, TableInfo> cache = null;

        /// <summary>
        /// 静态构造方法
        /// 使用静态构造方法,确保该构造方法只执行一次,不会还原初始化值
        /// 因为数据不会丢失,而且是一直保存在内存中，这样就达到了一个
        /// 临时存储器的功能
        /// </summary>
        static EntityTypeCache()
        {
            cache = new Dictionary<Type, TableInfo>();
        }


        /// <summary>
        /// 将表实体信息缓存到临时存储器中
        /// </summary>
        /// <param name="type">实体的类型</param>
        /// <param name="tableInfo">表实体信息</param>
        public static void InsertTableInfo(Type type, TableInfo tableInfo)
        {
            if (cache.ContainsKey(type))
            {
                return;
            }
            else
            {
                cache.Add(type, tableInfo);
            }
        }

        /// <summary>
        /// 将表实体信息缓存到临时存储器中
        /// </summary>
        /// <param name="entity">实体公共接口</param>
        /// <param name="tableInfo">表实体信息</param>
        public static void InsertTableInfo(IEntity entity, TableInfo tableInfo)
        {
            Type type = entity.GetType();
            InsertTableInfo(type, tableInfo);
        }

        /// <summary>
        /// 根据实体类的类型获得表特性信息
        /// </summary>
        /// <param name="type">实体类的类型</param>
        /// <returns></returns>
        public static TableInfo GetTableInfo(Type type)
        {
            if (cache.ContainsKey(type))
            {
                return cache[type];
            }
            else
            {

                BaseEntityHelper eh = new BaseEntityHelper();
                return eh.GetTableInfo(type);

            }
        }

        /// <summary>
        /// 根据实体公共接口获得表特性信息
        /// </summary>
        /// <param name="entity">实体公共接口</param>
        /// <returns></returns>
        public static TableInfo GetTableInfo(IEntity entity)
        {
            Type type = entity.GetType();
            return GetTableInfo(type);
        }

        /// <summary>
        /// 根据泛型类型获得表实体信息
        /// </summary>
        /// <typeparam name="T">泛型类</typeparam>
        /// <returns></returns>
        public static TableInfo GetTableInfo<T>() where T : IEntity
        {
            Type type = typeof(T);
            return GetTableInfo(type);
        }
    }
}
