using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace GeeStar.Common.Helper
{
    public class ConfigHelper
    {
        public static string ReadConfig(string key)
        {
            string val = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(val))
                val = string.Empty;
            return val.Trim();
        }

        public static bool ReadBoolConfig(string key)
        {
            string temp = ReadConfig(key);
            bool val = false;
            bool.TryParse(temp, out val);
            return val;
        }

        public static int ReadIntConfig(string key)
        {
            string temp = ReadConfig(key);
            int val = 0;
            int.TryParse(temp, out val);
            return val;
        }
    }
}
