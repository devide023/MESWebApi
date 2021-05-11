using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MESWebApi.Models;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using MESWebApi.DB;
using Dapper;
using Dapper.Oracle;
using System.Data;
using MESWebApi.InterFaces;
using Webdiyer.WebControls.Mvc;
using log4net;
using MESWebApi.Models.QueryParm;
namespace MESWebApi.Services
{
    public class MenuService:IDBOper<sys_menu>,IComposeQuery<sys_menu,MenuQueryParm>
    {
        private ILog log;
        public MenuService()
        {
            log = LogManager.GetLogger(this.GetType());
        }

        public IEnumerable<sys_menu> MenuTree(MenuQueryParm parm,out int resultcount) {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT id,");
                sql.Append("pid,");
                sql.Append("title,");
                sql.Append("menutype,");
                sql.Append("code,");
                sql.Append("icon,");
                sql.Append("path,");
                sql.Append("viewpath,");
                sql.Append("seq,");
                sql.Append("adduser,");
                sql.Append("addtime,");
                sql.Append("status FROM sys_menu where status =1 ");
                using (var conn = new OraDBHelper().Conn)
                {
                    var list = conn.Query<sys_menu>(sql.ToString());
                    IEnumerable<sys_menu> menulist = list.Where(t => t.pid == 0);
                    foreach (var item in menulist)
                    {
                        item.children = Create_Child(list, item.id);
                        if (item.children.Count() > 0)
                        {
                            item.hasChildren = true;
                        }
                        else
                        {
                            item.hasChildren = false;
                        }
                    }
                    var q = menulist.OrderBy(t=>t.id).ToPagedList(parm.pageindex,parm.pagesize);
                    resultcount = q.TotalItemCount;
                    return q;
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
        public IEnumerable<sys_menu> User_Menus(int userid)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select t1.*, t2.permission from (");
                sql.Append("select tc.id, \r\n");
                sql.Append("tc.title, \r\n");
                sql.Append("tc.pid, \r\n");
                sql.Append("tc.code, \r\n");
                sql.Append("tc.path, \r\n");
                sql.Append("tc.viewpath, \r\n");
                sql.Append("tc.icon, \r\n");
                sql.Append("tc.seq \r\n");
                sql.Append("from sys_user_role ta, sys_role_menu tb, sys_menu tc \r\n");
                sql.Append("where ta.userid = :userid \r\n");
                sql.Append("and ta.roleid = tb.roleid \r\n");
                sql.Append("and tb.menuid = tc.id) t1,sys_menu_data t2 \r\n");
                sql.Append(" where t1.id = t2.menuid(+) ");

                using (var db = new OraDBHelper())
                {
                    List<sys_menu> menulist = new List<sys_menu>();
                    var list = db.Conn.Query<sys_menu>(sql.ToString(), new { userid = userid });
                    foreach (var item in list.Where(t => t.pid == 0))
                    {
                        if (!string.IsNullOrEmpty(item.permission))
                        {
                            item.menu_permission = JsonConvert.DeserializeObject<sys_permission>(item.permission);
                        }
                        menulist.Add(item);
                        bool haschild = list.Where(t => t.pid == item.id).Count() > 0 ? true : false;
                        if (haschild)
                        {
                            item.children = Create_Child(list, item.id);
                            item.hasChildren = true;
                        }
                        else
                        {
                            item.children = new List<sys_menu>();
                            item.hasChildren = false;
                        }
                    }
                    return menulist;
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
        public IEnumerable<sys_menu> SubMenuByPid(MenuQueryParm parm, out int resultcount) {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT id,");
                sql.Append("pid,");
                sql.Append("title,");
                sql.Append("menutype,");
                sql.Append("code,");
                sql.Append("icon,");
                sql.Append("path,");
                sql.Append("viewpath,");
                sql.Append("seq,");
                sql.Append("adduser,");
                sql.Append("addtime,");
                sql.Append("status FROM sys_menu where status =1 and pid=:pid ");
                using (var conn = new OraDBHelper().Conn)
                {
                    var q = conn.Query<sys_menu>(sql.ToString(), new { pid = parm.pid })
                        .OrderBy(t => t.id)
                        .ToPagedList(parm.pageindex, parm.pagesize);
                    foreach (var item in q)
                    {
                        item.children = new List<sys_menu>();
                        item.hasChildren = true;
                    }
                    resultcount = q.TotalItemCount;
                    return q;
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }
        private List<sys_menu> Create_Child(IEnumerable<sys_menu> list,int id)
        {
            List<sys_menu> children = new List<sys_menu>();
            foreach (var menu in list.Where(t => t.pid == id))
            {
                if (!string.IsNullOrEmpty(menu.permission))
                {
                    menu.menu_permission = JsonConvert.DeserializeObject<sys_permission>(menu.permission);
                }
                children.Add(menu);
                bool haschild = list.Where(t => t.pid == menu.id).Count() > 0 ? true : false;
                if (haschild)
                {
                    menu.children = Create_Child(list, menu.id);
                    menu.hasChildren = true;
                }
                else
                {
                    menu.children = new List<sys_menu>();
                    menu.hasChildren = false;
                }
            }
            return children;
        }
        public sys_menu Add(sys_menu menu)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into sys_menu(id,");
                sql.Append("title,");
                sql.Append("pid,");
                sql.Append("icon,");
                sql.Append("code,");
                sql.Append("path,");
                sql.Append("menutype,");
                sql.Append("viewpath,");
                sql.Append("addtime,");
                sql.Append("adduser,");
                sql.Append("seq");
                sql.Append(")");
                sql.Append("values");
                sql.Append("(SEQ_MENUID.NEXTVAL,");
                sql.Append(":title,");
                sql.Append(":pid,");
                sql.Append(":icon,");
                sql.Append(":code,");
                sql.Append(":path,");
                sql.Append(":menutype,");
                sql.Append(":viewpath,");
                sql.Append("sysdate,");
                sql.Append(":adduser,");
                sql.Append(":seq");
                sql.Append(") returning id into :id");
                using (var db = new OraDBHelper())
                {
                    OracleDynamicParameters param = new OracleDynamicParameters();
                    param.Add(":title", menu.title, OracleMappingType.NVarchar2, ParameterDirection.Input);
                    param.Add(":pid", menu.pid, OracleMappingType.Int32, ParameterDirection.Input);
                    param.Add(":icon", menu.icon, OracleMappingType.NVarchar2, ParameterDirection.Input);
                    param.Add(":code", menu.code, OracleMappingType.NVarchar2, ParameterDirection.Input);
                    param.Add(":path", menu.path, OracleMappingType.NVarchar2, ParameterDirection.Input);
                    param.Add(":menutype", menu.menutype, OracleMappingType.NVarchar2, ParameterDirection.Input);
                    param.Add(":viewpath", menu.viewpath, OracleMappingType.NVarchar2, ParameterDirection.Input);
                    param.Add(":adduser", menu.adduser, OracleMappingType.Int32, ParameterDirection.Input);
                    param.Add(":seq", menu.seq, OracleMappingType.Int32, ParameterDirection.Input);
                    param.Add(":id", null, OracleMappingType.Int32, ParameterDirection.Output);
                    var ret = db.Conn.Execute(sql.ToString(), param);
                    var menuid = param.Get<int>(":id");
                    menu.id = menuid;
                    return menu;
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public int Modify(sys_menu entity)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("update sys_menu set title=:title where id=:id ");
                using (var db = new OraDBHelper())
                {
                   return db.Conn.Execute(sql.ToString(), new {title=entity.title,id=entity.id });
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
                sql.Append("delete from sys_menu where id =:id ");
                using (var conn = new OraDBHelper().Conn)
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        int cnt = conn.Execute(sql.ToString(), new { id = id },transaction:tran);
                        int cnt2 = conn.Execute("delete from sys_role_menu where menuid=:id", new { id = id }, transaction: tran);
                        tran.Commit();
                        return cnt > 0 ? true : false;
                    }
                    
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public sys_menu Find(int id)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select id,pid,title,code,path,viewpath from sys_menu where id = :id ");
                using (var db = new OraDBHelper())
                {
                    var query = db.Conn.Query<sys_menu>(sql.ToString(), new { id = id });
                    return query.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public IEnumerable<sys_menu> Find(sys_page parm, out int resultcount)
        {
            try
            {
                OracleDynamicParameters para = new OracleDynamicParameters();
                StringBuilder sql = new StringBuilder();
                sql.Append("select * from sys_menu where 1=1 ");
                if (!string.IsNullOrEmpty(parm.keyword))
                {
                    sql.Append(" and title like :title ");
                    para.Add(":title", parm.keyword, OracleMappingType.Varchar2, ParameterDirection.Input);
                }
                if (!string.IsNullOrEmpty(parm.keyword))
                {
                    para.Add(":index", parm.pageindex, OracleMappingType.Int32, ParameterDirection.Input);
                }
                if (!string.IsNullOrEmpty(parm.keyword))
                {
                    para.Add(":pagesize", parm.pagesize, OracleMappingType.Int32, ParameterDirection.Input);
                }
                if (!string.IsNullOrEmpty(parm.keyword))
                {
                    para.Add(":resultcount", parm.resultcount, OracleMappingType.Int32, ParameterDirection.Output);
                }
                using (var db = new OraDBHelper())
                {
                   var resutl =  db.Conn.Query<sys_menu>(sql.ToString(), para);
                    var q = resutl.OrderByDescending(t=>t.id).ToPagedList(parm.pageindex,parm.pagesize);
                    resultcount = q.TotalItemCount;
                    return q;
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
                sql.Append("delete from sys_menu where id in :ids ");
                using (var db = new OraDBHelper())
                {
                   return  db.Conn.Execute(sql.ToString(), new { ids = ids });
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public int Save_MenuRole(int menuid, List<int> roleids)
        {
            try
            {
                int ret = 0;
                List<dynamic> mr = new List<dynamic>();
                foreach (var item in roleids)
                {
                    mr.Add(new { menuid = menuid, roleid = item });
                }
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into sys_role_menu(id,roleid,menuid) \r\n");
                sql.Append("values \r\n");
                //sql.Append
                using (var conn = new OraDBHelper().Conn)
                {
                    conn.Open();
                    using (var trans = conn.BeginTransaction())
                    {
                        conn.Execute("delete from sys_role_menu where menuid=:menuid", new { menuid = menuid }, trans);
                        conn.Execute(sql.ToString(), mr.ToArray(), trans);
                        trans.Commit();
                    }
                }
                return ret;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public IEnumerable<sys_menu> Search(MenuQueryParm parm, out int resultcount)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" select ta.id,");
                sql.Append("ta.pid,");
                sql.Append("ta.title,");
                sql.Append("ta.menutype,");
                sql.Append("ta.code,");
                sql.Append("ta.icon,");
                sql.Append("ta.path,");
                sql.Append("ta.viewpath,");
                sql.Append("ta.seq,");
                sql.Append("ta.adduser,");
                sql.Append("ta.addtime,");
                sql.Append("ta.status,");
                sql.Append("tc.id,");
                sql.Append("tc.title,");
                sql.Append("tc.status,");
                sql.Append("tc.code ");
                sql.Append(" from   SYS_MENU ta, sys_role_menu tb,sys_role tc ");
                sql.Append(" where ta.id = tb.menuid ");
                sql.Append(" and tb.roleid = tc.id ");
                OracleDynamicParameters p = new OracleDynamicParameters();
                if (parm.pid != -1)
                {
                    sql.Append(" and ta.pid = :pid ");
                    p.Add(":pid", parm.pid, OracleMappingType.Int32, ParameterDirection.Input);
                }
                if (!string.IsNullOrEmpty(parm.keyword))
                {
                    sql.Append(" and ta.title like :title ");
                    p.Add(":title", parm.keyword, OracleMappingType.NVarchar2, ParameterDirection.Input);
                }
                using (var conn = new OraDBHelper().Conn)
                {
                    var menudic = new Dictionary<int, sys_menu>();
                    var query = conn.Query<sys_menu, sys_role, sys_menu>(sql.ToString(), (menu, role) =>
                    {
                        sys_menu menuEntry;
                        if (!menudic.TryGetValue(menu.id, out menuEntry))
                        {
                            menuEntry = menu;
                            menuEntry.roles = new List<sys_role>();
                            menudic.Add(menuEntry.id, menuEntry);
                        }
                        menuEntry.roles.Add(role);
                        return menuEntry;
                    }, p,splitOn: "id")
                        .Distinct()
                        .OrderBy(t=>t.id)
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
    }
}