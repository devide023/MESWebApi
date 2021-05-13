using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models.QueryParm
{
    public class UserQueryParm:sys_page
    {
        public List<sys_query> queryexp { get; set; }
    }
}