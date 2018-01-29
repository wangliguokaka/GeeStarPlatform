using System;
using System.Web;
using System.Data;
using System.Text.RegularExpressions;
using System.Security;
using System.Text;

namespace D2012.Common
{
    /// <summary>
    /// 字符串验证类
    /// </summary>
    public class MetarnetRegex
    {

        private static MetarnetRegex instance = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        private MetarnetRegex()
        {
        }

        /// <summary>
        /// 单例
        /// </summary>
        /// <returns></returns>
        public static MetarnetRegex GetInstance()
        {
            if (MetarnetRegex.instance == null)
            {
                MetarnetRegex.instance = new MetarnetRegex();
            }
            return MetarnetRegex.instance;
        }



        /// <summary>
        /// 判断输入的字符串只包含汉字
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static bool IsChineseCh(string input)
        {
            Regex regex = new Regex("^[\u4e00-\u9fa5]+$");
            return regex.IsMatch(input);
        }

        /// <summary>
        /// 匹配3位或4位区号的电话号码，其中区号可以用小括号括起来，
        /// 也可以不用，区号与本地号间可以用连字号或空格间隔，
        /// 也可以没有间隔
        /// \(0\d{2}\)[- ]?\d{8}|0\d{2}[- ]?\d{8}|\(0\d{3}\)[- ]?\d{7}|0\d{3}[- ]?\d{7}
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static bool IsPhone(string input)
        {
            string pattern = "^\\(0\\d{2}\\)[- ]?\\d{8}$|^0\\d{2}[- ]?\\d{8}$|"
                + "^\\(0\\d{3}\\)[- ]?\\d{7}$|^0\\d{3}[- ]?\\d{7}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        /// <summary>
        /// 判断输入的字符串是否是一个合法的手机号
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static bool IsMobilePhone(string input)
        {
            Regex regex = new Regex("^13\\d{9}$");
            return regex.IsMatch(input);

        }


        /// <summary>
        /// 判断输入的字符串只包含数字
        /// 可以匹配整数和浮点数
        /// ^-?\d+$|^(-?\d+)(\.\d+)?$
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static bool IsNumber(string input)
        {
            string pattern = "^-?\\d+$|^(-?\\d+)(\\.\\d+)?$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        /// <summary>
        /// 匹配非负整数
        ///
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static bool IsNotNagtive(string input)
        {
            Regex regex = new Regex(@"^\d+$");
            return regex.IsMatch(input);
        }

        /// <summary>
        /// 匹配正整数
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static bool IsUint(string input)
        {
            Regex regex = new Regex("^[0-9]*[1-9][0-9]*$");
            return regex.IsMatch(input);
        }

        /// <summary>
        /// 判断输入的字符串字包含英文字母
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static bool IsEnglisCh(string input)
        {
            Regex regex = new Regex("^[A-Za-z]+$");
            return regex.IsMatch(input);
        }


        /// <summary>
        /// 判断输入的字符串是否是一个合法的Email地址
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static bool IsEmail(string input)
        {
            string pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|"
                    + @"(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }


        /// <summary>
        /// 判断输入的字符串是否只包含数字和英文字母
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static bool IsNumAndEnCh(string input)
        {
            string pattern = @"^[A-Za-z0-9]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }


        /// <summary>
        /// 判断输入的字符串是否是一个超链接
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static bool IsURL(string input)
        {
            //string pattern = @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
            string pattern = @"^[a-zA-Z]+://(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }


        /// <summary>
        /// 判断输入的字符串是否是表示一个IP地址
        /// </summary>
        /// <param name="input">被比较的字符串</param>
        /// <returns>是IP地址则为True</returns>
        public static bool IsIPv4(string input)
        {

            string[] strIPs = input.Split('.');
            Regex regex = new Regex(@"^\d+$");
            for (int i = 0; i < strIPs.Length; i++)
            {
                if (!regex.IsMatch(strIPs[i]))
                {
                    return false;
                }
                if (Convert.ToUInt16(strIPs[i]) > 255)
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 计算字符串的字符长度，一个汉字字符将被计算为两个字符
        /// </summary>
        /// <param name="input">需要计算的字符串</param>
        /// <returns>返回字符串的长度</returns>
        public static int GetCount(string input)
        {
            return Regex.Replace(input, @"[\u4e00-\u9fa5/g]", "aa").Length;
        }

        /// <summary>
        /// 调用Regex中IsMatch函数实现一般的正则表达式匹配
        /// </summary>
        /// <param name="pattern">要匹配的正则表达式模式。</param>
        /// <param name="input">要搜索匹配项的字符串</param>
        /// <returns>如果正则表达式找到匹配项，则为 true；否则，为 false。</returns>
        public static bool IsMatch(string pattern, string input)
        {
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        /// <summary>
        /// 从输入字符串中的第一个字符开始，用替换字符串替换指定的正则表达式模式的所有匹配项。
        /// </summary>
        /// <param name="pattern">模式字符串</param>
        /// <param name="input">输入字符串</param>
        /// <param name="replacement">用于替换的字符串</param>
        /// <returns>返回被替换后的结果</returns>
        public static string Replace(string pattern, string input, string replacement)
        {
            Regex regex = new Regex(pattern);
            return regex.Replace(input, replacement);
        }

        /// <summary>
        /// 在由正则表达式模式定义的位置拆分输入字符串。
        /// </summary>
        /// <param name="pattern">模式字符串</param>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        public static string[] Split(string pattern, string input)
        {
            Regex regex = new Regex(pattern);
            return regex.Split(input);
        }

        /// <summary>
        /// 判断输入的字符串是否是合法的IPV6 地址
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static bool IsIPV6(string input)
        {
            string pattern = "";
            string temp = input;
            string[] strs = temp.Split(':');
            if (strs.Length > 8)
            {
                return false;
            }
            int count = MetarnetRegex.GetStringCount(input, "::");
            if (count > 1)
            {
                return false;
            }
            else if (count == 0)
            {
                pattern = @"^([\da-f]{1,4}:){7}[\da-f]{1,4}$";

                Regex regex = new Regex(pattern);
                return regex.IsMatch(input);
            }
            else
            {
                pattern = @"^([\da-f]{1,4}:){0,5}::([\da-f]{1,4}:){0,5}[\da-f]{1,4}$";
                Regex regex1 = new Regex(pattern);
                return regex1.IsMatch(input);
            }

        }

        /* *******************************************************************
         * 1、通过“:”来分割字符串看得到的字符串数组长度是否小于等于8
         * 2、判断输入的IPV6字符串中是否有“::”。
         * 3、如果没有“::”采用 ^([\da-f]{1,4}:){7}[\da-f]{1,4}$ 来判断
         * 4、如果有“::” ，判断"::"是否止出现一次
         * 5、如果出现一次以上 返回false
         * 6、^([\da-f]{1,4}:){0,5}::([\da-f]{1,4}:){0,5}[\da-f]{1,4}$
         * ******************************************************************/
        /// <summary>
        /// 判断字符串compare 在 input字符串中出现的次数
        /// </summary>
        /// <param name="input">源字符串</param>
        /// <param name="compare">用于比较的字符串</param>
        /// <returns>字符串compare 在 input字符串中出现的次数</returns>
        private static int GetStringCount(string input, string compare)
        {
            int index = input.IndexOf(compare);
            if (index != -1)
            {
                return 1 + GetStringCount(input.Substring(index + compare.Length), compare);
            }
            else
            {
                return 0;
            }

        }

        /// <summary>
        /// 是否为SQL语句
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns></returns>
        public static bool IsSQL(string str)
        {
            Regex regex1 = new Regex(@"\?|select%20|select\s+|insert%20|insert\s+|"
                + @"delete%20|delete\s+|count\(|drop%20|drop\s+|update%20|update\s+");
            return regex1.IsMatch(str.ToLower());
        }

        /// <summary>
        /// 过虑HTML
        /// </summary>
        /// <param name="strHtmlstring">源字符串</param>
        /// <returns></returns>
        public static string NoHTML(string strHtmlstring)
        {
            strHtmlstring = Regex.Replace(strHtmlstring, "<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            strHtmlstring = Regex.Replace(strHtmlstring, "<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            strHtmlstring = Regex.Replace(strHtmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            strHtmlstring = Regex.Replace(strHtmlstring, "-->", "", RegexOptions.IgnoreCase);
            strHtmlstring = Regex.Replace(strHtmlstring, "<!--.*", "", RegexOptions.IgnoreCase);
            strHtmlstring = Regex.Replace(strHtmlstring, "&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            strHtmlstring = Regex.Replace(strHtmlstring, "&(amp|#38);", "&", RegexOptions.IgnoreCase);
            strHtmlstring = Regex.Replace(strHtmlstring, "&(lt|#60);", "<", RegexOptions.IgnoreCase);
            strHtmlstring = Regex.Replace(strHtmlstring, "&(gt|#62);", ">", RegexOptions.IgnoreCase);
            strHtmlstring = Regex.Replace(strHtmlstring, "&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            strHtmlstring = Regex.Replace(strHtmlstring, "&(iexcl|#161);", "\x00a1", RegexOptions.IgnoreCase);
            strHtmlstring = Regex.Replace(strHtmlstring, "&(cent|#162);", "\x00a2", RegexOptions.IgnoreCase);
            strHtmlstring = Regex.Replace(strHtmlstring, "&(pound|#163);", "\x00a3", RegexOptions.IgnoreCase);
            strHtmlstring = Regex.Replace(strHtmlstring, "&(copy|#169);", "\x00a9", RegexOptions.IgnoreCase);
            strHtmlstring = Regex.Replace(strHtmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            strHtmlstring.Replace("<", "");
            strHtmlstring.Replace(">", "");
            strHtmlstring.Replace("\r\n", "");
            strHtmlstring = HttpContext.Current.Server.HtmlEncode(strHtmlstring).Trim();
            return strHtmlstring;
        }

        /// <summary>
        /// 过虑危险字符
        /// </summary>
        /// <param name="strChar">源字符串</param>
        /// <returns></returns>
        public static string ReplaceBadChar(string strChar)
        {
            if (strChar.Trim() == "")
            {
                return "";
            }
            strChar = strChar.Replace("'", "");
            strChar = strChar.Replace("*", "");
            strChar = strChar.Replace("?", "");
            strChar = strChar.Replace("(", "");
            strChar = strChar.Replace(")", "");
            strChar = strChar.Replace("<", "");
            strChar = strChar.Replace("=", "");
            return strChar.Trim();
        }


        /// <summary>
        /// UBB语法支持
        /// </summary>
        /// <param name="content">源字符串</param>
        /// <returns></returns>
        public static string UBB(string content)
        {
            content = Regex.Replace(content, @"\[center\](?<x>[^\]]*)\[/center\]",
                "<center>$1</center>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[url=(?<x>[^\]]*)\](?<y>[^\]]*)\[/url\]",
                "<a href=$1 target=_blank>$2</a>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[email\](?<x>[^\]]*)\[/email\]",
                "<a href=mailto:$1>$1</a>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[flash](?<x>[^\]]*)\[/flash\]",
                "<OBJECT codeBase=http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0 "
             + "classid=clsid:D27CDB6E-AE6D-11cf-96B8-444553540000 width=500 height=400><PARAM NAME=movie VALUE=\"$1\">"
             + "<PARAM NAME=quality VALUE=high><embed src=\"$1\" quality=high pluginspage="
             + "http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version="
             + "ShockwaveFlash' type='application/x-shockwave-flash' width=500 height=400>$1"
             + "</embed></OBJECT>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[img](?<x>[^\]]*)\[/img]", "<IMG SRC=\"$1\" border=0>",
                RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[color=(?<x>[^\]]*)\](?<y>[^\]]*)\[/color\]",
                "<font color=$1>$2</font>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[face=(?<x>[^\]]*)\](?<y>[^\]]*)\[/face\]",
                "<font face=$1>$2</font>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[size=(?<x>[^\]]*)\](?<y>[^\]]*)\[/size\]",
                "<font size=$1>$2</font>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[align=(?<x>[^\]]*)\](?<y>[^\]]*)\[/align\]",
                "<div align=$1>$2</div>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[fly](?<x>[^\]]*)\[/fly\]",
                "<marquee width=90% behavior=alternate scrollamount=3>$1</marquee>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[move](?<x>[^\]]*)\[/move\]",
                "<marquee scrollamount=3>$1</marquee>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[glow=(?<x>[^\]]*),(?<y>[^\]]*),(?<z>[^\]]*)\](?<w>[^\]]*)\[/glow\]",
                "<table width=$1 style=\"filter:glow(color=$2, strength=$3)\">$4</table>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[shadow=(?<x>[^\]]*),(?<y>[^\]]*),(?<z>[^\]]*)\](?<w>[^\]]*)\[/shadow\]",
                "<table width=$1 style=\"filter:shadow(color=$2, strength=$3)\">$4</table>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[b\](?<x>[^\]]*)\[/b\]", "<b>$1</b>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[i\](?<x>[^\]]*)\[/i\]", "<i>$1</i>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[u\](?<x>[^\]]*)\[/u\]", "<u>$1</u>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[h1\](?<x>[^\]]*)\[/h1\]", "<h1>$1</h1>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[h2\](?<x>[^\]]*)\[/h2\]", "<h2>$1</h2>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[h3\](?<x>[^\]]*)\[/h3\]", "<h3>$1</h3>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[h4\](?<x>[^\]]*)\[/h4\]", "<h4>$1</h4>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[h5\](?<x>[^\]]*)\[/h5\]", "<h5>$1</h5>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[h6\](?<x>[^\]]*)\[/h6\]", "<h6>$1</h6>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[small\](?<x>[^\]]*)\[/small\]", "<small>$1</small>",
                RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[big\](?<x>[^\]]*)\[/big\]", "<big>$1</big>",
                RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[del\](?<x>[^\]]*)\[/del\]", "<del>$1</del>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[blink\](?<x>[^\]]*)\[/blink\]", "<blink>$1</blink>",
                RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[sub\](?<x>[^\]]*)\[/sub\]", "<sub>$1</sub>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[sup\](?<x>[^\]]*)\[/sup\]", "<sup>$1</sup>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[list\](?<x>[^\]]*)\[/list\]", "<li>$1</li>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[indent\](?<x>[^\]]*)\[/indent\]", "<blockquote><p>$1</p></blockquote>",
                RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[quote\](?<x>[^\]]*)\[/quote\]",
                "\u4ee5\u4e0b\u5185\u5bb9\u4e3a\u5f15\u7528\uff1a<table border=0 width=95% cellpadding=10 "
            + "cellspacing=1 bgcolor=#000000><tr><td bgcolor=#FFFFFF>$1</td></tr></table>", RegexOptions.IgnoreCase);
            return content;
        }
    }
}
