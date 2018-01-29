using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace D2012.Common
{
    public static class comutility
    {
        public static string convertToDnum(object num)
        {
            if (num == null)
            {
                return "";
            }
            else
            {
                return convertToDnum(num.ToString());
            }
        }
        public static string convertToDnum(string num)
        {
            string zhengshu = num;
            string xiaoshu = "";

            if (num.IndexOf(".") > 0)
            {
                zhengshu = num.Substring(0, num.IndexOf("."));
                xiaoshu = num.Substring(num.IndexOf("."));
            }

            string newzhengshu = "";
            string spzhengshu = zhengshu;

            while (spzhengshu.Length > 3)
            {
                newzhengshu = "," + spzhengshu.Substring(spzhengshu.Length - 3) + newzhengshu;
                spzhengshu = spzhengshu.Substring(0, spzhengshu.Length - 3);
            }

            newzhengshu = spzhengshu + newzhengshu;

            return newzhengshu + xiaoshu;

        }
        public static string SubStringCN(object str, int intN)
        {
            int intI = 0;
            //int num = 0;
            if (str == null)
            {
                return "";
            }
            if ((System.Text.Encoding.Default.GetByteCount(str.ToString()) <= intN + 2))
            {
                return str.ToString();
            }
            else
            {
                int intT = 0;
                byte[] tmp = System.Text.Encoding.Default.GetBytes(str.ToString());
                int c = 0;
                for (intT = 0; intT <= tmp.Length - 1; intT++)
                {
                    c = tmp[intT];
                    if ((c > 127 | c < 0))
                    {
                        intT = intT + 1;
                    }

                    if (intT >= intN)
                    {
                        break; // TODO: might not be correct. Was : Exit For 
                    }
                    intI = intI + 1;
                }
            }
            return str.ToString().Substring(0, intI) + "…";
        }
    }
}
