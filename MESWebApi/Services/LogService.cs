﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Text;
using MESWebApi.DB;
using Dapper;
using DapperExtensions;
using log4net;
using MESWebApi.Util;
using MESWebApi.DBAttr;
using System.Reflection;
namespace MESWebApi.Services
{
    public class LogService
    {
        ILog log;
        public LogService()
        {
            log = LogManager.GetLogger(this.GetType());
        }

        public void UpdateLog<T>(T entity)
        {
            object id = 0;
            StringBuilder sql = new StringBuilder();
            Dictionary<string, object> fields = new Dictionary<string, object>();
            StringBuilder cols = new StringBuilder();
            Type type = entity.GetType();
            string tablename = type.Name;
            PropertyInfo[] proinfos = type.GetProperties();
            
            foreach (var item in proinfos)
            {
                string colname = string.Empty;
                object colval = null;
                var attrs = item.GetCustomAttributes(typeof(DbFieldAttribute));
                if (attrs.Count() > 0) {
                    DbFieldAttribute attr = attrs.First() as DbFieldAttribute;
                    colname = attr.FieldName;
                    colval = item.GetValue(entity);
                    cols.Append(colname + ",");
                    fields.Add(colname, colval);
                }
            }
            fields.TryGetValue("id", out id);
            sql.Append("select ");
            sql.Append(cols.ToString().Substring(0,cols.Length-1));
            sql.Append($" from {tablename} where id = {id} ");
            using (var conn = new OraDBHelper().Conn )
            {
               T orginalobj = conn.Query<T>(sql.ToString()).FirstOrDefault();
            }

        }
    }
}