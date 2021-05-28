﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Dapper;
using MESWebApi.Models.BaseInfo;
using Dapper.Oracle;
using MESWebApi.DB;
using MESWebApi.InterFaces;
using log4net;
namespace MESWebApi.Services.BaseInfo
{
    /// <summary>
    /// 岗位站点
    /// </summary>
    public class StationService:IDBOper<zxjc_gxzd>
    {
        private ILog log;
        private string constr = string.Empty;
        public StationService()
        {
            log = LogManager.GetLogger(this.GetType());
            constr = "tjmes";
        }

        public zxjc_gxzd Add(zxjc_gxzd entity)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                using (var conn = new OraDBHelper(constr).Conn)
                {
                   int cnt = conn.Execute(sql.ToString(), entity);
                    if (cnt > 0) {
                        return new zxjc_gxzd();
                    }
                    else
                    {
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
        public int Add(List<zxjc_gxzd> entitys)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                using (var conn = new OraDBHelper(constr).Conn)
                {
                   return conn.Execute(sql.ToString(), entitys);
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

        public zxjc_gxzd Find(int id)
        {
            throw new NotImplementedException();
        }

        public int Modify(zxjc_gxzd entity)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
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