using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace D2012.Common
{
    public class sPager
    {
        public static string generatePage(string url, int pagenum, int pagecount)
        {

            if (pagecount == 0)
            {
                return "";
            }

            if (url.Substring(url.Length - 1) != "/")
            {
                url = url + "/";
            }

            string tempurl = url.Substring(0, url.Length - 1);
            string lastpstr = url.Substring(tempurl.LastIndexOf("/"));
            if (Regex.IsMatch(lastpstr, "/p([0-9]{1,6})"))
            {
                url = url.Substring(0, tempurl.LastIndexOf("/") + 1);
            }

            string pageformat = "<span><a class=\"apage\" href=\"{2}p{0}\">{1}</a></span>";
            string pageformat2 = "<span>{0}</span>";
            string pageformat3 = "<span>…</span>";

            string pageformat4 = "<span class=\"sxspage\">上一页</span>";
            string pageformat5 = "<span><a class=\"apage\" href=\"{1}p{0}\">上一页</a></span>";
            string pageformat6 = "<span class=\"sxxpage\">下一页</span>";
            string pageformat7 = "<span><a class=\"apage\" href=\"{1}p{0}\">下一页</a></span>";
            StringBuilder sb = new StringBuilder();

            //上一页

            if (pagenum == 1 || pagecount == 1)
            {
                sb.Append(pageformat4);
            }
            else
            {
                sb.AppendFormat(pageformat5, (pagenum - 1).ToString(), url);
            }
            //小于8页

            if (pagecount <= 8)
            {

                for (int i = 1; i <= pagecount; i++)
                {
                    if (i == pagenum)
                    {
                        sb.AppendFormat(pageformat2, i.ToString(), url);
                    }
                    else
                    {
                        sb.AppendFormat(pageformat, i.ToString(), i.ToString(), url);
                    }
                }

            }
            else
            {
                if (pagenum <= 5)
                {
                    for (int j = 1; j <= 9; j++)
                    {
                        if (j == pagenum)
                        {
                            sb.AppendFormat(pageformat2, j.ToString(), url);
                        }
                        else if (j == 8)
                        {
                            sb.Append(pageformat3);
                        }
                        else if (j == 9)
                        {
                            sb.AppendFormat(pageformat, pagecount.ToString(), pagecount.ToString(), url);
                        }
                        else
                        {
                            sb.AppendFormat(pageformat, j.ToString(), j.ToString(), url);
                        }
                    }
                }
                else if (pagecount - pagenum <= 3)
                {
                    for (int k = 1; k <= 9; k++)
                    {
                        if (k == 1)
                        {
                            sb.AppendFormat(pageformat, k.ToString(), k.ToString(), url);
                        }
                        else if (k == 2)
                        {
                            sb.Append(pageformat3);
                        }
                        else if (pagecount + k - 9 == pagenum)
                        {
                            sb.AppendFormat(pageformat2, pagenum.ToString(), url);
                        }
                        else
                        {
                            sb.AppendFormat(pageformat, (pagecount + k - 9).ToString(), (pagecount + k - 9).ToString(), url);
                        }
                    }
                }
                else
                {

                    for (int n = 1; n <= 10; n++)
                    {
                        if (n == 1)
                        {
                            sb.AppendFormat(pageformat, n.ToString(), n.ToString(), url);
                        }
                        else if (n == 10)
                        {
                            sb.AppendFormat(pageformat, pagecount.ToString(), pagecount.ToString(), url);
                        }
                        else if (n == 2 || n == 9)
                        {
                            sb.Append(pageformat3);
                        }
                        else if (n == 6)
                        {
                            sb.AppendFormat(pageformat2, pagenum.ToString(), url);
                        }
                        else
                        {
                            sb.AppendFormat(pageformat, (pagenum - 6 + n).ToString(), (pagenum - 6 + n).ToString(), url);
                        }
                    }
                }
            }

            //下一页

            if (pagenum == pagecount || pagecount == 1)
            {
                sb.Append(pageformat6);
            }
            else
            {
                sb.AppendFormat(pageformat7, (pagenum + 1).ToString(), url);
            }

            return sb.ToString();
        }
    }
}
