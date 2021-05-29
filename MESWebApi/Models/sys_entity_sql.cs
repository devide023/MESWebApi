using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
namespace MESWebApi.Models
{
    public class sys_entity_sql
    {
        public StringBuilder insertsql { get; set; }
        public StringBuilder updatesql { get; set; }
    }
}