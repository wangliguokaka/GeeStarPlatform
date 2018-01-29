using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace D2012.Common
{
    public class yeyRequest
    {

        public string this[string str]
        {
            get
            {
                return HttpContext.Current.Request[str];
            }
        }


        public static string Params(string str)
        {
                return HttpContext.Current.Request.Params[str];


        }

    }
}
