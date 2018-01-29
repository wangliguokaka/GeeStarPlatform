using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace Frank.Expressage
{
    /// <summary>
    /// ExpressageHelper类 
    /// </summary>
    public class ExpressageHelper
    {
        /// <summary>
        /// 返回订单查询的json字符串
        /// </summary>
        /// <param name="para">查询对象[物流公司+快递单号]</param>
        /// <returns>查询结果</returns>
        public static MResultMsg GetExpressageMessage(MQueryParameter para)
        {
            // 获取配置文件
            Configuration config = null;
            string queryUrl = string.Empty;
            string com = string.Empty;
            MResultMsg msg = new MResultMsg();

            Queue<Action<MResultMsg, MQueryParameter>> myQueue = new Queue<Action<MResultMsg, MQueryParameter>>();

            if (string.IsNullOrEmpty(para.TypeCom))
            {
                msg.Result = false;
                msg.Error = new ErrorMsg() { ErrorCode = "001", ErrorMessage = "物流公司不能为空" };
                return msg;
            }

            if (string.IsNullOrEmpty(para.OrderId))
            {
                msg.Result = false;
                msg.Error = new ErrorMsg() { ErrorCode = "002", ErrorMessage = "订单号不能为空" };
                return msg;
            }

            try
            {
                string configPath = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Expressage.config");
                ExeConfigurationFileMap map = new ExeConfigurationFileMap();
                map.ExeConfigFilename = configPath;
                config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
                queryUrl = config.AppSettings.Settings["queryUrl"] == null ? string.Empty : config.AppSettings.Settings["queryUrl"].Value;
                com = config.AppSettings.Settings[para.TypeCom] == null ? string.Empty : config.AppSettings.Settings[para.TypeCom].Value;
            }
            catch (Exception ex)
            {
                msg.Result = false;
                msg.Error = new ErrorMsg() { ErrorCode = "003", ErrorMessage = ex.Message };
                return msg;
            }

            if (string.IsNullOrEmpty(com))
            {
                msg.Result = false;
                msg.Error = new ErrorMsg() { ErrorCode = "004", ErrorMessage = "配置文件缺少对于的物流公司类型" };
                return msg;
            }

            // 网上获取物流信息    
            string jsonResult = null;
            try
            {
                queryUrl = string.Format(queryUrl, com, para.OrderId);
                WebRequest request = WebRequest.Create(@queryUrl);
                WebResponse response = request.GetResponse();
                string message = string.Empty;
                using (Stream stream = response.GetResponseStream())
                {
                    Encoding encode = Encoding.UTF8;
                    using (StreamReader reader = new StreamReader(stream, encode))
                    {
                        message = reader.ReadToEnd();
                    }
                    jsonResult = message;
                }
            }
            catch (Exception ex)
            {
                msg.Result = false;
                msg.Error = new ErrorMsg() { ErrorCode = "005", ErrorMessage = ex.Message };
                return msg;
            }

            msg = JSONStringToObj<MResultMsg>(jsonResult);
            msg.JsonMessage = jsonResult;
            msg.Result = true;
            return msg;
        }

        private static T JSONStringToObj<T>(string JsonStr)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            T objs = Serializer.Deserialize<T>(JsonStr);
            return objs;
        }
    }
}
