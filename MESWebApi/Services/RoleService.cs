using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MESWebApi.InterFaces;
using MESWebApi.Models;
using log4net;
using Webdiyer.WebControls.Mvc;
using System.Text;
using MESWebApi.DB;
using Dapper;
using Dapper.Oracle;
using System.Data;
namespace MESWebApi.Services
{
    public class RoleService : IDBOper<sys_role>
    {
        private ILog log;
        public RoleService()
        {
            log = LogManager.GetLogger(this.GetType());
        }
        public sys_role Add(sys_role entity)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into sys_role (id,status,title,code,adduser,addtime) values (SEQ_ROLEID.nextval,:status,:title,:code,:adduser,sysdate) returning id into :id ");
                OracleDynamicParameters p = new OracleDynamicParameters();
                p.Add(":status", entity.status, OracleMappingType.Int32, ParameterDirection.Input);
                p.Add(":title", entity.title, OracleMappingType.NVarchar2, ParameterDirection.Input);
                p.Add(":code", entity.code, OracleMappingType.NVarchar2, ParameterDirection.Input);
                p.Add(":adduser", entity.adduser, OracleMappingType.Int32, ParameterDirection.Input);
                p.Add(":id", null, OracleMappingType.Int32, ParameterDirection.ReturnValue);
                using (var db = new OraDBHelper())
                {
                    int cnt = db.Conn.Execute(sql.ToString(), p);
                    entity.id = p.Get<int>(":id");
                    return entity;
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
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("delete from sys_role where id=:id");
                using (var db = new OraDBHelper())
                {
                    return db.Conn.Execute(sql.ToString(), new { id = id }) > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public int Delete(List<int> ids)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("delete from sys_role where id in :ids");
                using (var db = new OraDBHelper())
                {
                    return db.Conn.Execute(sql.ToString(), new { ids = ids });
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public sys_role Find(int id)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select * from sys_role where id=:id");
                using (var db = new OraDBHelper())
                {
                    var query = db.Conn.Query<sys_role>(sql.ToString(), new { id = id });
                    return query.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public IEnumerable<sys_role> Find(sys_page parm, out int resultcount)
        {
            try
            {
                OracleDynamicParameters p = new OracleDynamicParameters();
                StringBuilder sql = new StringBuilder();
                sql.Append(" select id,code,status,title,adduser,addtime from sys_role where 1=1 ");
                if (!string.IsNullOrEmpty(parm.keyword))
                {
                    sql.Append(" and (title like :keyword or code like :keyword) ");
                    p.Add(":keyword", "%"+parm.keyword+"%", OracleMappingType.NVarchar2, ParameterDirection.Input);
                }
                using (var db = new OraDBHelper())
                {
                    var query = db.Conn.Query<sys_role>(sql.ToString(), p)
                        .OrderByDescending(t => t.id)
                        .ToPagedList(parm.pageindex, parm.pagesize);
                    resultcount = query.TotalItemCount;
                    return query;
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public int Modify(sys_role entity)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("update sys_role set title=:title,status=:status,updatetime=sysdate where id = :id");
                using (var db = new OraDBHelper())
                {
                   return db.Conn.Execute(sql.ToString(), new {id=entity.id,title=entity.title,status=entity.status});
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }


        public int Save_RoleMenus(int roleid, List<int> menuids)
        {
            try
            {
                int ret = 0;
                List<dynamic> vals = new List<dynamic>();
                foreach (var item in menuids)
                {
                    vals.Add(new { roleid = roleid, menuid = item });
                }
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into sys_role_menu(id,roleid,menuid) \r\n");
                sql.Append("values \r\n");
                sql.Append(" (SEQ_ROLEMENU_ID.nextval,:roleid,:menuid) \r\n");
                using (var conn = new OraDBHelper().Conn)
                {
                    conn.Open();
                    using (var trans = conn.BeginTransaction())
                    {
                        conn.Execute("delete from sys_role_menu where roleid =:roleid",new { roleid=roleid},trans);
                        ret = conn.Execute(sql.ToString(),vals.ToArray(),trans);
                        trans.Commit();
                        return ret;
                    }
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