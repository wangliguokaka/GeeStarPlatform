using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeeStar.Common.Logging
{
    public class DBLogging : LoggingCategory
    {
        public DBLogging()
        {
            base.Category = "DBLogCategory";
        }
    }
}
