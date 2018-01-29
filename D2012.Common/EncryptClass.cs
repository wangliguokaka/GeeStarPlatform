using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace D2012.Common
{
    /// <summary>
    /// EncryptClass 的摘要说明。
    /// </summary>
    public class EncryptClass
    {
        //加密字符串
        Byte[] byIv64 = { 12, 25, 87, 33, 05, 56, 85, 8 };
        Byte[] byKey64 = { 11, 45, 85, 28, 58, 78, 50, 89 };

        //DSE加密
        byte[] desKey = new byte[] { 0x12, 0x50, 0x43, 0x75, 0x97, 0x04, 0x05, 0x05 };
        byte[] desIV = new byte[] { 0x2, 0x35, 0x87, 0x56, 0x12, 0x85, 0x24, 0x36 };

        //自定义双向加密函数使用数组
        private static Char[] phKey = { 'O', 'M', 'T', '5', 'S', '2', 'Q', 'V', 'A', '3', 'X', 'L', '4', '1', 'D', '7', 'W', '6', 'R', '9', 'J', '8', 'H', 'C', 'K' };


        //默认密钥向量
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };



        public static string Encode(string encryptString)
        {
             return Encode(encryptString, "5156bbDL");
           
        }
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串,失败返回源串</returns>
        public static string Encode(string encryptString, string encryptKey)
        {
            encryptKey = encryptKey.Substring(0, 8);
            encryptKey = encryptKey.PadRight(8, ' ');
          byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
           
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());

        }


        public static string Decode(string decryptString)
        {
           return Decode(decryptString, "5156bbDL");
           
        }
        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串,失败返源串</returns>
        public static string Decode(string decryptString, string decryptKey)
        {
            try
            {
                decryptKey = decryptKey.Substring(0, 8);
                decryptKey = decryptKey.PadRight(8, ' ');
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();

                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return "";
            }

        }


        /// <summary>
        /// MD5函数
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string MD5(string str)
        {
            byte[] b = System.Text.Encoding.UTF8.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            //string ret = "";
            //for (int i = 0; i < b.Length; i++)
            //    ret += b[i].ToString("x").PadLeft(2, '0');
            return Convert.ToBase64String(b);
        }


        # region byte //字符串加密

        /// <summary>
        /// 字符串加密
        /// </summary>
        /// <param name="strText">strText</param>
        /// <returns></returns>
        public string Encrypt1(string strText)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                Byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey64, byIv64), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        /// <summary>
        /// 字符串解密
        /// </summary>
        /// <param name="strText">strText</param>
        /// <returns></returns>
        public string Decrypt1(string strText)
        {
            Byte[] inputByteArray = new byte[strText.Length];
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey64, byIv64), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string phEncode(string uid)
        {
            string phCode = "";
            string[] tmp=new string[uid.Length];
            for (int i = 0; i < uid.Length; i++)
            {
                tmp[i] = uid.Substring(i, 1);
            }

            Random rd = new Random(); 　　　　
            int ikey = rd.Next(0,50);

            phCode += phKey[ikey % 25].ToString();

            int iseed=Convert.ToInt32(uid.Substring(uid.Length-2));

            phCode += phKey[iseed % 25];

            for (int k = 0; k < uid.Length - 2; k++)
            {

                ikey = rd.Next(0,50);

                phCode += phKey[ikey % 25].ToString();

                phCode += phKey[(iseed + k +Convert.ToInt32( tmp[k]) ) % 25];

            }
            
            return phCode;
        }

        public string phDecode(string code)
        {
            string[] tmp = new string[code.Length];
            for (int i = 0; i < code.Length; i++)
            {
                tmp[i] = code.Substring(i, 1);
            }

            string mycode = "";

          mycode=  phGetNumber(tmp[1]);
          int iseed = Convert.ToInt32(mycode);

          for (int k = 2; k < code.Length ; k++)
          {

              if (k % 2 != 0)
              {
                  mycode = ((Convert.ToInt32(phGetNumber(tmp[k])) - iseed - k - 2) % 25).ToString() +mycode;
              }

          }


            return mycode;
        }

        private string phGetNumber(string tcode)
        {
            string pnumer = "";
            for (int i = 0; i < 25; i++)
            {
                if (tcode == phKey[i].ToString())
                {
                    pnumer= i.ToString();
                }
            }

            if (pnumer.Length == 1)
            {
                pnumer = "0" + pnumer;
            }

            return pnumer;
        }
        #endregion


    }
}
