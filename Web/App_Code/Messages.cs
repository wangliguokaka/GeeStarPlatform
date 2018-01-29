using D2012.Common;
using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for Messages
/// </summary>
public class Messages
{
    //记录所有请求的客户端
   static List<myAsynResult> clients = new List<myAsynResult>();
   static Dictionary<string, myAsynResult> dicUser = new Dictionary<string, myAsynResult>();
    #region 实现该类的单例
    private static readonly Messages _Instance = new Messages();
	private Messages()
	{
	}
    public static Messages Instance()
    {
        return _Instance;
    }
    #endregion

    public void AddMessage(string orderNumber,string BelongFactory,string userName,string content,myAsynResult asyncResult)
    {
        try
        {
            string key = orderNumber + ":" + userName;
            //当传入的内容为"-1"时，表示为建立连接请求，即为了维持一个从客户端到服务器的连接而建立的连接
            //此时将该连接保存到 List<myAsynResult> clients中，待再有消息发送过来时，该连接将会被遍历，并且会将该连接输出内容后，结束该连接
            if (content == "-1")
            {
                if (dicUser.ContainsKey(key))
                {
                    dicUser[key] = asyncResult;
                }
                else {
                    dicUser.Add(key, asyncResult);
                }
            }
            else
            {
                
                //将当前请求的内容输出到客户端
                asyncResult.Content = "";
                asyncResult.Send(null);
                string CurrentDateTime = DateTime.Now.ToString();
                string lastModifyDate = ConvertDateTimeInt(DateTime.Parse(CurrentDateTime));
                content = content.Replace("\n", "");
                content = content.Replace("\r\n", "");
                CacheHelper.InsertIdentify(orderNumber, CurrentDateTime.ToString() + "|" + userName + "|" + content, BelongFactory);
                //否则将遍历所有已缓存的client,并将当前内容输出到客户端
                foreach (string  keys in dicUser.Keys)
                {
                    if (keys.IndexOf(orderNumber) > -1) {
                        try
                        {
                            dicUser[keys].Content = lastModifyDate + "|" + userName + ":" + content;
                            dicUser[keys].Send(null);
                        }
                        catch { 
                        
                        }
                    }
                    
                }

                //清空所有缓存
                clients.Clear();
            }
        }
        catch (Exception ex)
        { 
        }
    }

    public static string ConvertDateTimeInt(System.DateTime time)
    {
        double intResult = 0;
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        intResult = (time - startTime).TotalSeconds;
        return intResult.ToString();
    }
}