using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeeStar.Common.Logging
{
    public class TxtLogging : LoggingCategory
    {
        public TxtLogging()
        {
            base.Category = "TxtLogCategory";
        }
    }
}
