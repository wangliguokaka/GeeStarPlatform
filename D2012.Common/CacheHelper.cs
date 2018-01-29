using D2012.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace D2012.Common
{
    /// <head>
    ///<function>
    ///   存储类(存储UserInfo信息)
    ///</function>
    ///<description>
    ///   用Cache存储用户信息
    ///   在指定间隔(TimeOut)内取，则可以从Cache中取，
    ///   如果超出存储时间,则从数据库取用户信息数据
    ///   作為所有用户信息的存儲類.
    ///   Add 和 Insert 方法具有相同的签名，但它们之间存在细微的差别。
    ///   首先，调用 Add 方法返回表示缓存项的对象，而调用 Insert 方法不是。
    ///   其次，如果您调用这些方法并将已存储在 Cache 中的某个项添加到 Cache 中，那么它们的行为是不同的。Insert 方法替换该项，而 Add 方法失败。
    ///</description>
    ///<author>
    ///<name>ChengKing</name>   
    ///</author>
    /// </head>
    public class CacheHelper
    {
        #region 方法
        //实现“一键一值”存储方法,最普通的存储方法   
        //（“一键一值”指一个Identify存储一个值,下面还有一个“一键多值”方法，因为有时候需要一个键存储多个变量对象值）
 
        public static bool InsertIdentify(string strIdentify, string Info, string BelongFactory)
        {
            Info = Info.Replace("'", "&apos;");
            strIdentify = BelongFactory +  "|"+strIdentify ;
            List<string> talkList;
            if(strIdentify != null && strIdentify.Length != 0 )
            {
                //建立回调委托的一个实例
　　                  CacheItemRemovedCallback callBack =new CacheItemRemovedCallback(onRemove);
                string contentTalk;
                if (HttpContext.Current.Cache[strIdentify] != null)
                {
                    talkList = (List<string>)HttpContext.Current.Cache[strIdentify];
                    talkList.Add(Info);
                    //contentTalk = oldValue + Info;
                }
                else
                {
                    talkList = new List<string>();
                    talkList.Add(Info);
                }
                //以Identify为标志，将userInfo存入Cache
                HttpContext.Current.Cache.Insert(strIdentify, talkList, null, 
　　　　　　                  System.DateTime.Now.AddSeconds(30),
　　　　　　                  System.Web.Caching.Cache.NoSlidingExpiration, 
　　　　　　                  System.Web.Caching.CacheItemPriority.Default,
　　　　　　                  callBack);
                return true;
            }   
            else
            {
                return false;
            }
        }
  
        //判断存储的"一键一值"值是否还存在（有没有过期失效或从来都未存储过）
        public static bool ExistIdentify(string strIdentify)
        {
            if(HttpContext.Current.Cache[strIdentify] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<string> GetInfoByIdentify(string strIdentify)
        {
            if (HttpContext.Current.Cache[strIdentify] != null)
            {
                return (List<string>)HttpContext.Current.Cache[strIdentify];
            }
            else
            {
                return null;
            }
        }

        //插入"一键多值"方法
        //***其中 StorageInfType是一个Enum,里面存有三种类型: UserInf SysInf PageInf 
        //这个枚举如下:
                            
        public enum StorageInfType
        {
            /// <summary>用户信息</summary>
            UserInf = 0,
  
            /// <summary>页面信息</summary>
            PageInf = 1,  
  
            /// <summary>系统信息</summary>
            SysInf = 2
        }
        //此枚举是自己定义的.可根据需要定义不同的枚举  
        //加个枚举目的是实现“一键多值”存储方法，事实上Cache中是存放了多个变量的，只不过被这个类封装了，
        //程序员感到就好像是“一键一值”.   这样做目的是可以简化开发操作,否则程序员要存储几个变量就得定义几个Identify.
        public static bool InsertCommonInf(string strIdentify,StorageInfType enumInfType,object objValue)
        {   
            if(strIdentify != null && strIdentify != "" && strIdentify.Length != 0 && objValue != null)
            {
            //RemoveCommonInf(strIdentify,enumInfType); 
    
            //建立回调委托的一个实例
  　　          CacheItemRemovedCallback callBack =new CacheItemRemovedCallback(onRemove);

                if(enumInfType == StorageInfType.UserInf)
                {    
                    //以用户UserID+信息标志(StorageInfType枚举)，将userInfo存入Cache
                    HttpContext.Current.Cache.Insert(strIdentify+StorageInfType.UserInf.ToString(),objValue,null, 
  　　　　　　                System.DateTime.Now.AddSeconds(18000),       //单位秒
  　　　　　　                System.Web.Caching.Cache.NoSlidingExpiration, 
  　　　　　　                System.Web.Caching.CacheItemPriority.Default,
  　　　　　　                callBack);  
                }
                if(enumInfType == StorageInfType.PageInf)
                {
                    //以用户UserID+信息标志(StorageInfType枚举)，将PageInfo存入Cache
                        HttpContext.Current.Cache.Insert(strIdentify+StorageInfType.PageInf.ToString(),objValue,null, 
  　　　　　　                    System.DateTime.Now.AddSeconds(18000),
  　　　　　　                    System.Web.Caching.Cache.NoSlidingExpiration, 
  　　　　　　                    System.Web.Caching.CacheItemPriority.Default,
  　　　　　　                    callBack);  
                }
                if(enumInfType == StorageInfType.SysInf)
                {
                    //以用户UserID+信息标志(StorageInfType枚举)，将SysInfo存入Cache
                        HttpContext.Current.Cache.Insert(strIdentify+StorageInfType.SysInf.ToString(),objValue,null, 
  　　　　　　                    System.DateTime.Now.AddSeconds(18000),
     　　　　　　                     System.Web.Caching.Cache.NoSlidingExpiration, 
  　　　　　　                    System.Web.Caching.CacheItemPriority.Default,
  　　　　　　                    callBack);  
                }
                return true;
            }
            return false;
        }
                    //读取“一键多值”Identify的值
        //              public static bool ReadIdentify(string strIdentify,out UserInfo userInfo)
        //{
        // //取出值
        // if((UserInfo)HttpContext.Current.Cache[strIdentify] != null)
        // {
        //  userInfo = (UserInfo)HttpContext.Current.Cache[strIdentify];
        //  if(userInfo == null)
        //  {
        //   return false;
        //  }
        //  return true;
        // }   
        // else
        // {
        //  userInfo = null;
        //  return false;
        // }   
        //}

        //手动移除“一键一值”对应的值
        public static bool RemoveIdentify(string strIdentify)
        {
            //取出值
            if (HttpContext.Current.Cache[strIdentify] != null)
            {
                HttpContext.Current.Cache.Remove(strIdentify);
            }
            return true;
        }

        //此方法在值失效之前调用，可以用于在失效之前更新数据库，或从数据库重新获取数据
        private static void onRemove(string strIdentify, object talkInfo,CacheItemRemovedReason reason)
        {
            if (reason.ToString() == "Expired")
            {
                
                ServiceCommon sc = new ServiceCommon();
               string strSerializer   = XmlSerializerHelper.XmlSerializer(talkInfo);
               sc.ExecuteSql("insert into OrderMessage(OrderNumber,TalkContent,ModifyDate,BelongFactory)values('" + strIdentify.Split('|')[1] + "','" + strSerializer + "',getdate(),'" + strIdentify.Split('|')[0] + "')");

            }
        }
        #endregion
        }

    public static class ConvertTypeHelper
    {
        public static string ToDigitalString(this String strValue)
        {
            if (String.IsNullOrEmpty(strValue) || Convert.ToDecimal(strValue) == 0)
            {
                return "";
            }
            return decimal.Parse(strValue).ToString("N");
        }
    }
}
    

