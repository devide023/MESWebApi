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
                if (ret > 0)
                {
                    logservice.InsertLog<T>(entity);
                }
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
                if (list.Count == entitys.Count)
                {
                    logservice.InsertLog<T>(entitys);
                    return 1;
                }
                else
                {
                    return 0;
                }
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
                var ok = Db.Delete<T>(entity);
                if (ok)
                {
                    logservice.DeleteLog<T>(entity);
                }
                return ok;
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
                if (list.Count == entitys.Count())
                {
                    logservice.DeleteLog<T>(entitys);
                    return true;
                }
                else
                {
                    return false;
                }

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