using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeeStar.Common.Logging
{
    public class LoggingCategory
    {
        public LoggingCategory()
        {
            this.Category = "EventLogCategory";
        }

        public string Category { get; set; }
    }
}
