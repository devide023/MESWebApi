using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using DapperExtensions;
using MESWebApi.DB;
using MESWebApi.InterFaces;
using log4net;
using MESWebApi.Models;

namespace MESWebApi.Services.BaseInfo
{
    public class DeviceService:IDBOper<sys_device>
    {
        private ILog log;
        public DeviceService()
        {
            log = LogManager.GetLogger(this.GetType());
        }

        public sys_device Add(sys_device entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public int Delete(List<int> ids)
        {
            throw new NotImplementedException();
        }

        public sys_device Find(int id)
        {
            throw new NotImplementedException();
        }

        public int Modify(sys_device entity)
        {
            throw new NotImplementedException();
        }
    }
}