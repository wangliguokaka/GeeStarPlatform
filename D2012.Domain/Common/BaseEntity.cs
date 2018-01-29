using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D2012.Common.DbCommon;

namespace D2012.Domain.Common
{
    /// <summary>
    /// Entiry 基类
    /// </summary>
    [Serializable]
    public abstract class BaseEntity : IEntity
    {
        public static string STRTABLENAME;
        public static string STRKEYNAME;
        /// <summary>
        /// 隐式实现接口方法，用于释放内存
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
