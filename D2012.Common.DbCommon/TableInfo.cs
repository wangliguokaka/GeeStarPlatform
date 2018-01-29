using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace D2012.Common.DbCommon
{
    /// <summary>
    /// 实体模型类的一些特新信息
    /// </summary>
    [Serializable]
    public class TableInfo
    {
        /// <summary>
        /// 表实体特性信息
        /// </summary>
        private TableAttribute table_;

        /// <summary>
        /// 表实体对应数据字段特性信息
        /// </summary>
        private ColumnAttribute[] columns_;

        /// <summary>
        /// 表实体对应数据字段特性信息键值存储
        /// </summary>
        private IDictionary<string, ColumnAttribute> dicColumns_ = new Dictionary<string, ColumnAttribute>();

        /// <summary>
        /// 实体模型字段信息
        /// </summary>
        private FieldInfo[] fields_;

        /// <summary>
        /// 实体模型字段信息键值存储
        /// </summary>
        private IDictionary<string, FieldInfo> dicFields_ = new Dictionary<string, FieldInfo>();

        /// <summary>
        /// 实体模型属性信息
        /// </summary>
        private PropertyInfo[] properties_;

        /// <summary>
        /// 实体模型属性信息键值存储
        /// </summary>
        private IDictionary<string, PropertyInfo> dicProperties_ = new Dictionary<string, PropertyInfo>();

        /// <summary>
        /// 表实体 实体特性信息
        /// </summary>
        private LinkTableAttribute[] linkTable_;

        /// <summary>
        /// 表实体 实体特性信息键值存储
        /// </summary>
        private IDictionary<string, LinkTableAttribute> dicLinkTable_ = new Dictionary<string, LinkTableAttribute>();

        /// <summary>
        /// 表实体 实体集合特性信息
        /// </summary>
        private LinkTablesAttribute[] linkTables_;

        /// <summary>
        /// 表实体 实体集合特性信息键值存储
        /// </summary>
        private IDictionary<string, LinkTablesAttribute> dicLinkTables_ = new Dictionary<string, LinkTablesAttribute>();

        /// <summary>
        /// 无参数构造方法
        /// </summary>
        public TableInfo()
        {
        }

        /// <summary>
        /// 全字段构造方法，构造该对象实例的时候
        /// 初始化所有属性
        /// </summary>
        /// <param name="table">表实体特性信息</param>
        /// <param name="columns">表实体对应数据字段特性信息</param>
        /// <param name="fields">实体模型字段信息</param>
        /// <param name="properties">实体模型属性信息</param>
        /// <param name="linkTable">表实体 实体特性信息</param>
        /// <param name="linkTables">表实体 实体集合特性信息</param>
        public TableInfo(TableAttribute table,
            ColumnAttribute[] columns,
            FieldInfo[] fields,
            PropertyInfo[] properties,
            LinkTableAttribute[] linkTable,
            LinkTablesAttribute[] linkTables)
        {
            this.table_ = table;
            this.columns_ = columns;
            this.fields_ = fields;
            this.properties_ = properties;
            this.linkTable_ = linkTable;
            this.linkTables_ = linkTables;
        }

        /// <summary>
        /// 表实体特性信息
        /// </summary>
        public TableAttribute Table
        {
            get { return table_; }
            set { table_ = value; }
        }

        /// <summary>
        /// 表实体对应数据字段特性信息
        /// </summary>
        public ColumnAttribute[] Columns
        {
            get { return columns_; }
            set { columns_ = value; }
        }

        /// <summary>
        /// 实体模型字段信息
        /// </summary>
        public FieldInfo[] Fields
        {
            get { return fields_; }
            set { fields_ = value; }
        }

        /// <summary>
        /// 实体模型属性信息
        /// </summary>
        public PropertyInfo[] Properties
        {
            get { return properties_; }
            set { properties_ = value; }
        }

        /// <summary>
        /// 表实体 实体特性信息
        /// </summary>
        public LinkTableAttribute[] LinkTable
        {
            get { return linkTable_; }
            set { linkTable_ = value; }
        }

        /// <summary>
        /// 表实体 实体集合特性信息
        /// </summary>
        public LinkTablesAttribute[] LinkTables
        {
            get { return linkTables_; }
            set { linkTables_ = value; }
        }

        /// <summary>
        ///  表实体对应数据字段特性信息键值存储
        /// </summary>
        public IDictionary<string, ColumnAttribute> DicColumns
        {
            get { return dicColumns_; }
            set { dicColumns_ = value; }
        }

        /// <summary>
        /// 实体模型字段信息键值存储
        /// </summary>
        public IDictionary<string, FieldInfo> DicFields
        {
            get { return dicFields_; }
            set { dicFields_ = value; }
        }

        /// <summary>
        /// 实体模型属性信息键值存储
        /// </summary>
        public IDictionary<string, PropertyInfo> DicProperties
        {
            get { return dicProperties_; }
            set { dicProperties_ = value; }
        }

        /// <summary>
        ///  表实体 实体特性信息键值存储
        /// </summary>
        public IDictionary<string, LinkTableAttribute> DicLinkTable
        {
            get { return dicLinkTable_; }
            set { dicLinkTable_ = value; }
        }

        /// <summary>
        /// 表实体 实体集合特性信息键值存储
        /// </summary>
        public IDictionary<string, LinkTablesAttribute> DicLinkTables
        {
            get { return dicLinkTables_; }
            set { dicLinkTables_ = value; }
        }
    }
}
