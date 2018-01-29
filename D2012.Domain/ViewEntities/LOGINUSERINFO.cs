using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using D2012.Domain.Common;
using D2012.Domain.Entities;

namespace D2012.Domain.ViewEntities
{
    public class LOGINUSERINFO :BaseEntity
    {
        public loan_user _USERINFO { set; get; }
    }
}
