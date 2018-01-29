using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.IO;
//using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using System.Reflection;
using D2012.Domain.Services;
using D2012.Domain.Entities;
using D2012.Common.DbCommon;
//using System.Net.Http.Headers;

public class Utils
{
    public static string priKeyPath = System.Configuration.ConfigurationManager.AppSettings["priKeyPath"].ToString();
    public static string pubKeyPath = System.Configuration.ConfigurationManager.AppSettings["pubKeyPath"].ToString();

    public Utils()
    {
    }

    static Utils()
    {
        
    }

    //2007签名
    public static string signData(string merId, string plain)
    {
        byte[] StrRes = Encoding.GetEncoding("GBK").GetBytes(plain);
        plain = Convert.ToBase64String(StrRes);

        //NetPayClientClass a = new NetPayClientClass();
        NetPay a = new NetPay();
        //设置密钥文件地址
        bool retu = a.buildKey(merId, 0, priKeyPath);

        // 对一段字符串的签名
        return a.Sign(plain);

    }
    /*
    public static string signDataTest(string merId, string plain)
    {
        byte[] StrRes = Encoding.GetEncoding("utf-8").GetBytes(plain);
        //plain = Convert.ToBase64String(StrRes);
        sbyte[] test = new sbyte[StrRes.Length];
        for (int i = 0; i < StrRes.Length; i++)
        {
            if (StrRes[i] > 127)
                test[i] = (sbyte)(StrRes[i] - 256);
            else
                test[i] = (sbyte)StrRes[i];
        }
        plain = Encoding.GetEncoding("utf-8").GetString(test);
        NetPayClientClass a = new NetPayClientClass();
        //设置密钥文件地址
        a.setMerKeyFile(priKeyPath);

        // 对一段字符串的签名
        return a.signData(merId, plain);

    }

     */
    //2004签名
    public static string sign(string MerId, string OrdId, string TransAmt, string CuryId, string TransDate, string TransType)
    {

        //NetPayClientClass a = new NetPayClientClass();
        NetPay a = new NetPay();
        //设置密钥文件地址
        a.buildKey(MerId, 0, priKeyPath);

        // 对一段字符串的签名
        return a.signOrder(MerId, OrdId, TransAmt, CuryId, TransDate, TransType);

    }




    //验签
    public static string checkData(string plain, string ChkValue)
    {
        byte[] StrRes = Encoding.GetEncoding("GBK").GetBytes(plain);
        plain = Convert.ToBase64String(StrRes);

        NetPay a = new NetPay();

        //设置密钥文件地址
        a.buildKey("999999999999999", 0, pubKeyPath);


        // 对一段字符串的签名
        if (a.verifyAuthToken(plain, ChkValue))
        {
            return "0";
        }
        else
        {
            return "-118";
        }



    }


    //得到交易日期
    public static string getMerDate()
    {
        return DateTime.Now.ToString("yyyyMMdd");
    }

    //得到订单号16位
    public static string getMerSeqId()
    {
        return DateTime.Now.ToString("yyyyMMHHmmffffff");
    }

    //16位退款号
    public static string getMerRefId()
    {
        return DateTime.Now.ToString("yyyyMMHHmmffffff");
    }

    public static string getBase64(string str)
    {
        byte[] StrRes = Encoding.GetEncoding("utf-8").GetBytes(str);
        return Convert.ToBase64String(StrRes);
    }




    public static string postData(string str, string url)
    {
        try
        {
            byte[] data = System.Text.Encoding.GetEncoding("GBK").GetBytes(str);
            ////   准备请求
            //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(@url);
            //req.Method = "Post";
            //req.ContentType = "application/x-www-form-urlencoded";
            //req.ContentLength = data.Length;
            //Stream stream = req.GetRequestStream();
            ////   发送数据   
            //stream.Write(data, 0, data.Length);
            //stream.Close();
            //str = HttpUtility.UrlEncode(str, System.Text.Encoding.GetEncoding("GBK"));
            //HttpContent httpContent = new StringContent(str, System.Text.Encoding.GetEncoding("GBK"));
            //httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            //var httpClient = new HttpClient();

            //var responseJson = httpClient.PostAsync(url, httpContent).Result.Content.ReadAsStringAsync().Result;
            //return responseJson;

            return "";

            //HttpWebResponse rep = (HttpWebResponse)req.GetResponse();
            //Stream receiveStream = rep.GetResponseStream();
            //Encoding encode = System.Text.Encoding.GetEncoding("GBK");  
            //StreamReader readStream = new StreamReader(receiveStream, encode);

            //Char[] read = new Char[256];
            //int count = readStream.Read(read, 0, 256);
            //StringBuilder sb = new StringBuilder("");
            //while (count > 0)
            //{
            //    String readstr = new String(read, 0, count);
            //    sb.Append(readstr);
            //    count = readStream.Read(read, 0, 256);
            //}

            //rep.Close();
            //readStream.Close();

            //return sb.ToString();

        }
        catch (Exception ex)
        {
            return "";

        }
    }

    public static IList<T> ConvertTo<T>(DataTable table)

    {

        if (table == null)

        {

            return null;

        }



        List<DataRow> rows = new List<DataRow>();



        foreach (DataRow row in table.Rows)

        {

            rows.Add(row);

        }



        return ConvertTo<T>(rows);

    }



    public static IList<T> ConvertTo<T>(IList<DataRow> rows)

    {

        IList<T> list = null;



        if (rows != null)

        {

            list = new List<T>();



            foreach (DataRow row in rows)

            {

                T item = CreateItem<T>(row);

                list.Add(item);

            }

        }



        return list;

    }



    public static T CreateItem<T>(DataRow row)

    {

        T obj = default(T);

        if (row != null)

        {

            obj = Activator.CreateInstance<T>();



            foreach (DataColumn column in row.Table.Columns)

            {

                PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName, BindingFlags.IgnoreCase| BindingFlags.Public | BindingFlags.Instance);

                try

                {

                    object value = row[column.ColumnName];

                    prop.SetValue(obj, value, null);

                }

                catch(Exception ex)
                {  //You can log something here     

                    //throw;    

                }

            }

        }



        return obj;

    }

    public static bool IsNoSP
    {
        get
        {
            if (ConfigurationManager.AppSettings["IsNoSP"] == null)
            {
                return true;
            }
            else
            {
                return bool.Parse(ConfigurationManager.AppSettings["IsNoSP"].ToString());
            }
        
        }
    }

    public static string factoryConnectionString
    {
        get;
        set;
    }
    

}
