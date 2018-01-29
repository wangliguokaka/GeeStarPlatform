using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WXPay.V3Demo
{
    public class StringUtils
    {
        #region 处理Json格式数据
        /// <summary>
        /// 读取Json数据
        /// </summary>
        /// <param name="strJson">Json格式数据</param>
        /// <returns>Json对象</returns>
        public static LitJson.JsonData GetJsonObject(string strJson)
        {
            return LitJson.JsonMapper.ToObject(strJson);
        }

        /// <summary>
        /// 根据Key获取
        /// </summary>
        /// <param name="strJson">Json格式数据</param>
        /// <param name="Key">Json Key</param>
        /// <returns>值</returns>
        public static LitJson.JsonData GetJsonValue(string strJson, string Key)
        {
            return GetJsonObject(strJson)[Key];
        }
        #endregion
    }
}