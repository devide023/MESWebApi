using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using MESWebApi.DB;
using Dapper;
using Dapper.Oracle;
using MESWebApi.InterFaces;
using MESWebApi.Models.BaseInfo;
using log4net;
using MESWebApi.Util;
using MESWebApi.Models;
namespace MESWebApi.Services
{
    public class DBOperImp<T> : IDBOper<T> where T : class, new()
    {
        private ILog log;
        private string constr = string.Empty;
        private sys_entity_sql entity_sql = new sys_entity_sql();
        public DBOperImp(string constr="tjmes")
        {
            log = LogManager.GetLogger(this.GetType());
            this.constr = constr;
            this.entity_sql = Tool.EntityToSql<T>(new T());
        }
        public virtual T Add(T entity)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(entity_sql.insertsql);
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    int cnt = conn.Execute(sql.ToString(), entity);
                    if (cnt > 0)
                    {
                        return new T();
                    }
                    else {
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
        public virtual int Add(List<T> entitys)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(entity_sql.insertsql);
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    return conn.Execute(sql.ToString(), entitys.ToArray());
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public int Delete(List<int> ids)
        {
            throw new NotImplementedException();
        }

        public T Find(int id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 传递更新条件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="updateexp"></param>
        /// <returns></returns>
        public virtual int Modify(T entity,string updateexp)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(entity_sql.updatesql);
                sql.Append(" where ");
                sql.Append(updateexp);
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    return conn.Execute(sql.ToString(), entity);
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
        /// <summary>
        /// 默认通过id更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Modify(T entity)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(entity_sql.updatesql);
                sql.Append(" where id=:id");
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    return conn.Execute(sql.ToString(), entity);
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
    }
}