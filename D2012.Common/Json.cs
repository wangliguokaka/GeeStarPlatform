using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using D2012.Common.DbCommon;

namespace D2012.Common
{
    public class Json
    {
        //dataset转json  
        public static string DataTable2Json(DataTable dt, string strUserJson="")
        {
        StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"");
            jsonBuilder.Append(dt.TableName);
            jsonBuilder.Append("\":");
            jsonBuilder.Append("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            if (strUserJson != "")
            {
                jsonBuilder.Append(strUserJson);
            }
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }
        //list转json

        public static string ListToJson<T>(string jsonName, IList<T> entity, string strUserJson)
        {

            StringBuilder Json = new StringBuilder();

            Json.Append("{" + strUserJson + "\"" + jsonName + "\":[");

            if (entity.Count > 0)
            {

                for (int i = 0; i < entity.Count; i++)
                {

                    // T obj = Activator.CreateInstance<T>();

                    //Type type = obj.GetType();

                    //PropertyInfo[] pis = type.GetProperties();

                    TableInfo tfInfo = EntityTypeCache.GetTableInfo(typeof(T));
                    IDictionary<string, ColumnAttribute> dicColumn = tfInfo.DicColumns;

                    Json.Append("{");
                    int j = 0;

                    foreach (string key in dicColumn.Keys)
                    {

                        Json.Append("\"" + dicColumn[key].Name + "\":\"" + (EntityFactory.GetPropertyValue(entity[i], key) == null ?
                            System.DBNull.Value : (object)System.Web.HttpUtility.HtmlEncode(EntityFactory.GetPropertyValue(entity[i], key).ToString())) + "\"");

                        if (j++ < dicColumn.Keys.Count - 1)
                        {

                            Json.Append(",");

                        }

                        else
                        {

                            Json.Append("}");

                        }

                    }

                    if (i < entity.Count - 1)
                    {

                        Json.Append(",");

                    }
                }

            }

            Json.Append("]}");

            return Json.ToString();

        }


    }
}
