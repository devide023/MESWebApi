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
using MESWebApi.Models;
using System.Web.Http;
namespace MESWebApi.Services
{
    public class LogService
    {
        ILog log;
        public LogService()
        {
            log = LogManager.GetLogger(this.GetType());
        }
        public sys_log EntityFields<T>(T entity)
        {
            sys_log slog = new sys_log();
            Dictionary<string, object> fields = new Dictionary<string, object>();
            Type type = entity.GetType();
            slog.tablename = type.Name;
            PropertyInfo[] proinfos = type.GetProperties();
            foreach (var item in proinfos)
            {
                string colname = string.Empty;
                object colval = null;
                var attrs = item.GetCustomAttributes(typeof(DbFieldAttribute));
                if (attrs.Count() > 0)
                {
                    DbFieldAttribute attr = attrs.First() as DbFieldAttribute;
                    colname = attr.FieldName;
                    colval = item.GetValue(entity);
                    fields.Add(colname, colval);
                }
            }
            slog.fields = fields.Where(t => t.Value != null).ToDictionary(t => t.Key, t => t.Value);
            StringBuilder cols = new StringBuilder();
            StringBuilder vals = new StringBuilder();
            slog.fields.ToList().ForEach(t => cols.Append(t.Key + ","));
            slog.fields.ToList().ForEach(t => vals.Append($"'{t.Value}',"));
            StringBuilder sql = new StringBuilder();
            sql.Append("select ");
            sql.Append(cols.ToString().Substring(0, cols.Length - 1));
            sql.Append($" from {slog.tablename} where id = :id ");
            slog.querysql = sql.ToString();
            slog.insertsql = $"insert into {slog.tablename} ({cols.ToString().Substring(0, cols.Length - 1)}) values ({vals.ToString().Substring(0, vals.Length - 1)})";
            return slog;
        }
        public void UpdateLog<T>(T entity,T orginalobj)
        {
            object id = 0;
            StringBuilder txt = new StringBuilder();
            sys_log slog = EntityFields<T>(entity);
            slog.fields.TryGetValue("id", out id);
            Models.sys_user user = CacheManager.Instance().Current_User;
            txt.Append($"[{user.name}]更新{slog.tablename},");
            var cnames = slog.fields.Select(t => t.Key);
                Type orgtype = orginalobj.GetType();
                PropertyInfo[] orgproinfos = orgtype.GetProperties().Where(t => cnames.Contains(t.Name)).ToArray();
                foreach (var item in orgproinfos)
                {
                    string fn = string.Empty;
                    object fv = null;
                    object fvnew = null;
                    var orgattrs = item.GetCustomAttributes(typeof(DbFieldAttribute));
                    if (orgattrs.Count() > 0)
                    {
                        DbFieldAttribute attr = orgattrs.First() as DbFieldAttribute;
                        fn = attr.FieldName;
                        fv = item.GetValue(orginalobj);
                        string coltype = item.PropertyType.Name;
                        slog.fields.TryGetValue(fn, out fvnew);
                        switch (coltype)
                        {
                            case "Int32":
                                if (Convert.ToInt32(fv) != Convert.ToInt32(fvnew??0))
                                {
                                    txt.Append($"[{attr.Label}]:{fv}->{fvnew},");
                                }
                                break;
                            case "String":
                                if (fv.ToString() != (fvnew ?? "").ToString())
                                {
                                    txt.Append($"[{attr.Label}]:{fv}->{fvnew},");
                                }
                                break;
                            case "DateTime":
                                if (Convert.ToDateTime(fv) != Convert.ToDateTime(fvnew))
                                {
                                    txt.Append($"[{attr.Label}]:{fv}->{fvnew},");
                                }
                                break;
                            case "Double":
                                if (Convert.ToDouble(fv) != Convert.ToDouble(fvnew??0))
                                {
                                    txt.Append($"[{attr.Label}]:{fv}->{fvnew},");
                                }
                                break;
                            case "Float":
                                if (Convert.ToSingle(fv) != Convert.ToSingle(fvnew??0))
                                {
                                    txt.Append($"[{attr.Label}]:{fv}->{fvnew},");
                                }
                                break;
                            case "Decimal":
                                if (Convert.ToDecimal(fv) != Convert.ToDecimal(fvnew??0))
                                {
                                    txt.Append($"[{attr.Label}]:{fv}->{fvnew},");
                                }
                                break;
                            default:
                                break;
                        }

                    }
                }
                log.Info(txt.ToString());
            

        }
        public void UpdateLogJson<T>(T entity, T orginalobj)
        {
            Models.sys_user user = CacheManager.Instance().Current_User;
            Type type = entity.GetType();
            string tablename = type.Name;
            StringBuilder json = new StringBuilder();
            json.Append($"用户：{user.name},编号：{user.code},更新表{tablename},");
            json.Append($"原数据：{JsonConvert.SerializeObject(orginalobj)},");
            json.Append($"现数据：{JsonConvert.SerializeObject(entity)}");
            log.Info(json.ToString());
        }
        public void InsertLog<T>(T entity) {
            sys_log slog = EntityFields<T>(entity);
            log.Info(slog.insertsql);
        }

        public void InsertLogJson<T>(T entity)
        {
            Models.sys_user user = CacheManager.Instance().Current_User;
            Type type = entity.GetType();
            string tablename = type.Name;
            StringBuilder json = new StringBuilder();
            json.Append($"用户：{user.name},编号：{user.code},插入表{tablename},");
            json.Append($"{JsonConvert.SerializeObject(entity)}");
            log.Info(json.ToString());
        }

        public void DeleteLog<T>(int id) {
            

        }
    }
}