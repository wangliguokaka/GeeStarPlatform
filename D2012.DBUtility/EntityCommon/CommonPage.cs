using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D2012.DBUtility.EntityCommon
{
    /// <summary>
    /// 分页用
    /// </summary>
    [Serializable]
    public class CommonPage
    {
        /// <summary>
        /// 该字段为页数索引，第几页
        /// </summary>
        private int pageIndex_ = 1;

        /// <summary>
        /// 该字段为总页数
        /// </summary>
        private int pageCount_ = 0;

        /// <summary>
        /// 该字段为每页大小
        /// </summary>
        private int pageSize_ = 5;

        /// <summary>
        /// 该字段为数据总行数
        /// </summary>
        private int total_ = 0;

        /// <summary>
        /// 该字段为分页内容(可以用作分页控件代码)
        /// </summary>
        private string content_;

        /// <summary>
        /// 无参数构造方法
        /// </summary>
        public CommonPage()
        {
        }

        /// <summary>
        /// 全参数构造方法,构造该类实例的时候
        /// 初始化所有属性信息
        /// </summary>
        /// <param name="pageIndex">该字段为页数索引，第几页</param>
        /// <param name="pageCount">该字段为总页数</param>
        /// <param name="pageSize">该字段为每页大小</param>
        /// <param name="total">该字段为数据总行数</param>
        /// <param name="content">该字段为分页内容(可以用作分页控件代码)</param>
        /// <returns></returns>
        public CommonPage(int pageIndex, int pageCount, int pageSize, int total, string content)
        {
            this.pageIndex_ = PageIndex;
            this.pageCount_ = pageCount;
            this.pageSize_ = PageSize;
            this.total_ = total;
            this.content_ = content;
        }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex
        {
            get { return pageIndex_; }
            set { pageIndex_ = value; }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get { return pageCount_; }
            set { pageCount_ = value; }
        }

        /// <summary>
        /// 每页数
        /// </summary>
        public int PageSize
        {
            get { return pageSize_; }
            set { pageSize_ = value; }
        }

        /// <summary>
        /// 总共
        /// </summary>
        public int Total
        {
            get { return total_; }
            set { total_ = value; }
        }

        /// <summary>
        /// Content
        /// </summary>
        public string Content
        {
            get { return content_; }
            set { content_ = value; }
        }


    }
}
