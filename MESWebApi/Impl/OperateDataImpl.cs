using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using MESWebApi.Models;
using MESWebApi.DB;
using MESWebApi.InterFaces;
using MESWebApi.Services;
using Newtonsoft.Json;
namespace MESWebApi.Impl
{
    public class OperateDataImpl<T> : OracleBaseFixture, IOperateData<T> where T : class, new()
    {
        private ILog log;
        LogService logservice;
        public OperateDataImpl(string constr) : base(constr)
        {
            log = LogManager.GetLogger(this.GetType());
            logservice = new LogService();
        }
        public virtual int Add(T entity)
        {
            try
            {
                var ret = Db.Insert<T>(entity);
                return ret;
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
                List<dynamic> list = new List<dynamic>();
                foreach (var item in entitys)
                {
                    var ret = Db.Insert<T>(item);
                    list.Add(ret);
                }
                return list.Count == entitys.Count ? 1 : 0;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public virtual bool Delete(T entity)
        {
            try
            {
               return Db.Delete<T>(entity);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public virtual bool Delete(List<T> entitys)
        {
            try
            {
                List<bool> list = new List<bool>();
                foreach (T item in entitys)
                {
                   var isok = Db.Delete<T>(item);
                    list.Add(isok);
                }
                return list.Count == entitys.Count() ? true : false;
                
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public virtual bool Modify(T entity)
        {
            try
            {
               return Db.Update<T>(entity);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
    }
}