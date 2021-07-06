using System;
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
using System.IO;
using MESWebApi.Impl;
namespace MESWebApi.Services
{
    public class LogService
    {
        private ILog log;
        private string current_pageurl = string.Empty;
        private sys_logcnf logcnf = new sys_logcnf();
        private bool islog = false;
        public LogService()
        {
            log = LogManager.GetLogger(this.GetType());
            string logcnf_path = HttpContext.Current.Server.MapPath("~/logconfig.json");
            string logcnf_txt = File.ReadAllText(logcnf_path);
            logcnf = JsonConvert.DeserializeObject<sys_logcnf>(logcnf_txt);
            current_pageurl = HttpContext.Current.Request.Path;
            islog = logcnf.include.Where(t => current_pageurl.Contains(t)).Count() > 0 ? true : false;
        }
        public bool IsLog { 
            get
            {
                return islog;
            }
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
            if (!islog) {
                return;
            }
            object id = 0;
            StringBuilder txt = new StringBuilder();
            sys_log slog = EntityFields<T>(entity);
            slog.fields.TryGetValue("id", out id);
            sys_user user = CacheManager.Instance().Current_User;
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
            if (!islog)
            {
                return;
            }
            mes_log meslog = new mes_log();
            meslog.czlx = Enum.GetName(typeof(PubEnum.CZLX), PubEnum.CZLX.修改);
            meslog.rznr = "{\"oldobj\":"+JsonConvert.SerializeObject(orginalobj)+",\"newobj\":" + JsonConvert.SerializeObject(entity)+"}";
            using (var db = new OracleBaseFixture().DB)
            {
                db.Insert<mes_log>(meslog);
            }
        }
        public void InsertLog<T>(T entity) {
            if (!islog)
            {
                return;
            }
            mes_log meslog = new mes_log();
            meslog.czlx = Enum.GetName(typeof(PubEnum.CZLX), PubEnum.CZLX.新增);
            meslog.rznr = JsonConvert.SerializeObject(entity);
            using (var db = new OracleBaseFixture().DB)
            {
                db.Insert<mes_log>(meslog);
            }
        }
        public void InsertLog<T>(List<T> entitys) {
            if (!islog)
            {
                return;
            }
            mes_log meslog = new mes_log();
            meslog.czlx = Enum.GetName(typeof(PubEnum.CZLX), PubEnum.CZLX.批量插入);
            meslog.rznr = JsonConvert.SerializeObject(entitys);
            using (var db = new OracleBaseFixture().DB)
            {
                db.Insert<mes_log>(meslog);
            }
        }
        public void InsertLogJson<T>(T entity)
        {
            if (!islog)
            {
                return;
            }
            sys_user user = CacheManager.Instance().Current_User;
            Type type = entity.GetType();
            string tablename = type.Name;
            StringBuilder json = new StringBuilder();
            json.Append($"用户：{user.name},编号：{user.code},插入表{tablename}记录,");
            json.Append($"{JsonConvert.SerializeObject(entity)}");
            log.Info(json.ToString());
        }

        public void DeleteLog<T>(T entity) {
            if (!islog)
            {
                return;
            }
            mes_log meslog = new mes_log();
            meslog.czlx = Enum.GetName(typeof(PubEnum.CZLX), PubEnum.CZLX.删除);
            meslog.rznr = JsonConvert.SerializeObject(entity);
            using (var db = new OracleBaseFixture().DB)
            {
                db.Insert<mes_log>(meslog);
            }
        }
        public void DeleteLog<T>(List<T> entitys)
        {
            if (!islog)
            {
                return;
            }
            mes_log meslog = new mes_log();
            meslog.czlx = Enum.GetName(typeof(PubEnum.CZLX),PubEnum.CZLX.批量删除);
            meslog.rznr = JsonConvert.SerializeObject(entitys);
            using (var db = new OracleBaseFixture().DB)
            {
                db.Insert<mes_log>(meslog);
            }
        }
    }
}