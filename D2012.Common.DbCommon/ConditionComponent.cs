using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;

namespace D2012.Common.DbCommon
{
    /// <summary>
    /// 查询条件用


    /// </summary>
    public class ConditionComponent
    {

        private IDataParameterCollection dpList = null;
        private StringBuilder _sbComponent = null;
        private static string[] strEs = { " ( ", " ) ", " and ", " and ( ", " ) and ", 
            " and  ", " or ", " or ( ", " ) or  ", " or ", "" };
        private static string[] strPads = { "=", "!=", ">", ">=", "<", "<=", 
            "LIKE", "IN","NOT IN", "IS", "IS NOT" ,"","","","GROUP BY",""};
        private ConditionComponent component;

        /// <summary>
        /// 用于存储属性查询类型
        public ConditionComponent Clone()
        {
            return (ConditionComponent)this.MemberwiseClone();
        }

        /// </summary>
        public StringBuilder sbComponent
        {
            get { return _sbComponent; }
            set { _sbComponent = value; }
        }


        /// <summary>
        /// 私有构造方法,禁止外部类构造此类的实例
        /// 使用私有构造方式主要实现单例模式


        /// </summary>
        public ConditionComponent()
        {

            //component = new ConditionComponent();
            _sbComponent = new StringBuilder();


        }

        public void Clear()
        {
            sbComponent.Remove(0, sbComponent.Length);
        }

        /// <summary>
        /// 构造ConditionComponent的实例，当实例不存在是则创建该对象


        /// 这个是单例模式的实现
        /// </summary>
        /// <returns></returns>
        //public static ConditionComponent Instance()
        //{
        //    if (component == null)
        //    {

        //    }
        //    return component;
        //}

        /// <summary>
        /// 添加属性查询类型


        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <param name="component">查询类型</param>
        /// <returns></returns>
        public void AddComponent(string propertyName, string propertyValue,
            SearchComponent component, SearchPad intAndOr)
        {

            //if (component == null)
            //{
            //    ConditionComponent.component = Instance();
            //}
            if (sbComponent == null)
            {
                sbComponent = new StringBuilder();
            }

            if ((component.ToString() != "IS" && component.ToString() != "ISNOT")
                && String.IsNullOrEmpty(propertyValue))
            {
                return;
            }

            //in or not in 的时候


            if ((!string.IsNullOrEmpty(propertyValue))
                && ((strPads[(Int32)component] != "IN" && strPads[(Int32)component] != "NOT IN")
                || !propertyValue.EndsWith(")")))
            {
                //去除单引号


                propertyValue = propertyValue.Replace("'", "''");
            }


            if ((intAndOr == SearchPad.Ex || intAndOr == SearchPad.AndEx
                || intAndOr == SearchPad.OrEx) && sbComponent.Length <= 0)
            {
                sbComponent.Append(" ( ");
            }
            else if (intAndOr != SearchPad.ExB && intAndOr != SearchPad.NULL)
            {
                sbComponent.Append((sbComponent.Length <= 0 ? "" : (strEs[(Int32)intAndOr])));
            }

            if (strPads[(Int32)component] == "LIKE" && propertyValue.IndexOf("%") < 0)
            {
                if (propertyValue.IndexOf("_") < 0)
                {
                    sbComponent.AppendFormat(" {0} {1} '%{2}%' ",
                        propertyName, strPads[(Int32)component], propertyValue);
                }
                else
                {
                    sbComponent.AppendFormat(" {0} {1} '{2}' ",
                        propertyName, strPads[(Int32)component], propertyValue);

                }

            }
            else if (strPads[(Int32)component] == "GROUP BY")
            {
                if (sbComponent.ToString() != "")
                {
                    sbComponent.AppendFormat(" {0} {1}  ", strPads[(Int32)component], propertyValue);
                }
                else
                {
                    sbComponent.AppendFormat(" 1=1 {0} {1}  ", strPads[(Int32)component], propertyValue);
                }
            }
            else
            {
                if (strPads[(Int32)component] == "IN" || strPads[(Int32)component] == "NOT IN")
                {
                    sbComponent.AppendFormat(" {0} {1} {2} ",
                        propertyName, strPads[(Int32)component], propertyValue);
                }
                else
                {
                    if (propertyValue == null)
                    {
                        sbComponent.AppendFormat(" {0} {1} NULL",
                            propertyName, strPads[(Int32)component], propertyValue);
                    }
                    else
                    {
                        sbComponent.AppendFormat(" {0} {1} '{2}' ",
                            propertyName, strPads[(Int32)component], propertyValue);
                    }
                }


            }

            if (intAndOr == SearchPad.AndExB || intAndOr == SearchPad.OrExB)
            {
                sbComponent.Append(")");
            }
            // dpList.Add(new IDataParameter(propertyName, propertyValue));
            return;
        }

    }
}
