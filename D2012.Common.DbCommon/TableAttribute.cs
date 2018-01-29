using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D2012.Common.DbCommon
{
    /// <summary>
    /// 表属性
    /// </summary>
    public sealed class TableAttribute : Attribute
    {
        /// <summary>
        /// 数据表名
        /// 该字段用于描述数据库对应表的名称,而且该值最好与
        /// 数据表名大小写相同。该值有两种类型。
        /// (1)直接自定表的名称
        /// (2)[数据库名].[表名]
        /// 如果是(2)情况，则需要分割字符串,将数据库名分割
        /// 出来赋值给dBName
        /// </summary>
        private string name_;

        /// <summary>
        /// 数据库名
        /// 该字段用于描述数据的名称，而且该值最好与
        /// 数据库名称大小写相同
        /// </summary>
        private string dBName_;

        /// <summary>
        /// 主键字段名
        /// 该实体必须指定对应数据库表的主键
        /// </summary>
        private string primaryKeyName_ = "";

        /// <summary>
        /// 表实体描述信息
        /// 默认为""
        /// </summary>
        private string information_ = "";

        /// <summary>
        /// 表实体是否国际化
        /// 默认为false
        /// true 国际化 false 不国际化
        /// </summary>
        private bool isInternal_ = false;

        /// <summary>
        /// 表实体版本号
        /// 默认为 "V1.0"
        /// </summary>
        private string version_ = "V1.0";

        private bool isDel_ = false;

        private bool isCache_ = false;


        /// <summary>
        /// 数据表名
        /// </summary>
        public string Name
        {
            get { return name_; }
            set { name_ = value; }
        }

        /// <summary>
        /// 数据库名
        /// </summary>
        public string DBName
        {
            get { return dBName_; }
            set { dBName_ = value; }
        }

        /// <summary>
        /// 主键字段名
        /// </summary>
        public string PrimaryKeyName
        {
            get { return primaryKeyName_; }
            set { primaryKeyName_ = value; }
        }

        /// <summary>
        /// 表实体描述信息
        /// </summary>
        public string Information
        {
            get { return information_; }
            set { information_ = value; }
        }

        /// <summary>
        /// 表实体是否国际化
        /// </summary>
        public bool IsInternal
        {
            get { return isInternal_; }
            set { isInternal_ = value; }
        }

        /// <summary>
        /// 表实体是否国际化
        /// </summary>
        public bool IsDel
        {
            get { return isDel_; }
            set { isDel_ = value; }
        }

        /// <summary>
        /// 表实体是否国际化
        /// </summary>
        public bool IsCache
        {
            get { return isCache_; }
            set { isCache_ = value; }
        }

        /// <summary>
        /// 表实体版本号
        /// </summary>
        public string Version
        {
            get { return version_; }
            set { version_ = value; }
        }

        /// <summary>
        /// 无参数构造方法
        /// </summary>
        public TableAttribute()
        {

        }

        /// <summary>
        /// 部分参数构造方法,构造改表特性的实体类
        /// 并实现部分字段的初始化
        /// </summary>
        /// <param name="name">数据表名</param>
        /// <param name="dBName">数据库名</param>
        /// <param name="primaryKeyName">主键字段名</param>
        /// <param name="isInternal">表实体是否国际化</param>
        public TableAttribute(string name,
            string primaryKeyName, bool isDel, bool isCache)
        {
            this.name_ = name;
            this.primaryKeyName_ = primaryKeyName;
            this.isDel_ = isDel;
            this.isCache_ = isCache;
        }

        /// <summary>
        /// 全参数构造方法，构造改表特性的实体类
        /// 并实现所有字段的初始化
        /// </summary>
        /// <param name="name">数据表名</param>
        /// <param name="dBName">数据库名</param>
        /// <param name="primaryKeyName">主键字段名</param>
        /// <param name="information">表实体描述信息</param>
        /// <param name="isInternal">表实体是否国际化</param>
        /// <param name="version">表实体版本号</param>
        public TableAttribute(string name,
            string dBName,
            string primaryKeyName,
            string information,
            bool isInternal,
            string version)
        {
            this.name_ = name;
            this.dBName_ = dBName;
            this.primaryKeyName_ = primaryKeyName;
            this.information_ = information;
            this.isInternal_ = isInternal;
            this.version_ = version;
        }

    }
}
