using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using D2012.DBUtility.Data.Core.SQLCore;

namespace D2012.Domain.Services
{
    public class ServiceCommon : SqlHelper
    {
        public ServiceCommon():base() { 
        
        }

        public ServiceCommon(string ConnectionString)
        {
            this.strConnectionString = ConnectionString;
        }
    }
}
