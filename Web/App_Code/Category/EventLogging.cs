using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeeStar.Common.Logging
{
    public class EventLogging : LoggingCategory
    {
        public EventLogging()
        {
            base.Category = "EventLogCategory";
        }
    }
}
