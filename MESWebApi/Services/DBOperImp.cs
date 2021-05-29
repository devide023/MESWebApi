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
    public class DBOperImp<T> :OracleBaseFixture,IDBOper<T> where T : class, new()
    {
        private ILog log;
        private string constr = string.Empty;
        public DBOperImp(string constr="tjmes"):base(constr)
        {
            log = LogManager.GetLogger(this.GetType());
            this.constr = constr;
        }
        public virtual T Add(T entity)
        {
            try
            {
               var ret = this.Db.Insert<T>(entity);
                if (ret != null)
                {
                    return new T();
                }
                else
                {
                    return null;
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
                List<dynamic> list = new List<dynamic>();
                foreach (var item in entitys)
                {
                    var ret = Db.Insert<T>(item);
                    list.Add(ret);
                }
                if(list.Count == entitys.Count)
                {
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
        /// 默认通过id更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Modify(T entity)
        {
            try
            {
               return Db.Update<T>(entity)?1:0;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
    }
}