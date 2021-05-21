using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
namespace MESWebApi.Util
{
    public static class StringBuildExtension
    {
        public static string ToSql(this StringBuilder sql,object param)
        {
            string resultsql = string.Empty;
            Type type = param.GetType();

            return resultsql;
        }
    }
}