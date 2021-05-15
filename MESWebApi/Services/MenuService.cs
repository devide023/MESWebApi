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
    public class MenuService : IDBOper<sys_menu>, IComposeQuery<sys_menu, MenuQueryParm>
    {
        private ILog log;
        public MenuService()
        {
            log = LogManager.GetLogger(this.GetType());
        }

        public IEnumerable<sys_menu> MenuTree(MenuQueryParm parm, out int resultcount)
        {
            try
            {
                OracleDynamicParameters op = new OracleDynamicParameters();
                StringBuilder exp = new StringBuilder();
                if (!string.IsNullOrEmpty(parm.keyword))
                {
                    exp.Append(" and (title like :key or code like :key) ");
                    op.Add(":key", "%" + parm.keyword + "%", OracleMappingType.Varchar2, ParameterDirection.Input);
                }
                if (!string.IsNullOrEmpty(parm.status))
                {
                    exp.Append(" and status = :status ");
                    op.Add(":status", parm.status, OracleMappingType.Int32, ParameterDirection.Input);
                }
                if (parm.pid > 0)
                {
                    exp.Append(" and pid = :pid ");
                    op.Add(":pid", parm.pid, OracleMappingType.Int32, ParameterDirection.Input);
                }
                StringBuilder sql = new StringBuilder();
                sql.Append("with tm (id,pid,title,menutype,code,icon,path,viewpath,seq,adduser,addusername,addtime,status) as");
                sql.Append("(SELECT id,");
                sql.Append("pid,");
                sql.Append("title,");
                sql.Append("menutype,");
                sql.Append("code,");
                sql.Append("icon,");
                sql.Append("path,");
                sql.Append("viewpath,");
                sql.Append("seq,");
                sql.Append("adduser,(select name from sys_user where id = sys_menu.adduser) as addusername,");
                sql.Append("addtime,");
                sql.Append("status FROM sys_menu where ");
                if (exp.Length == 0)
                {
                    sql.Append(" pid = 0 ");
                }
                else
                {
                    sql.Append(" 1=1 ");
                    sql.Append(exp);
                }
                sql.Append(" union all ");
                sql.Append("select t1.id,t1.pid,t1.title,t1.menutype,t1.code,t1.icon,t1.path,t1.viewpath,t1.seq,t1.adduser,");
                sql.Append("(select name from sys_user where id = t1.adduser) as addusername,");
                sql.Append("t1.addtime,t1.status from sys_menu t1,tm where ");
                if (exp.Length == 0)
                {
                    sql.Append(" t1.pid = tm.id ");
                }
                else
                {
                    sql.Append(" t1.id = tm.pid ");
                }
                sql.Append(")");
                sql.Append("select * from tm ");
                sql.Append(" order by pid asc");

                using (var conn = new OraDBHelper().Conn)
                {
                    var list = conn.Query<sys_menu>(sql.ToString(), op);
                    IEnumerable<sys_menu> menulist = list.Where(t => t.pid == 0);
                    foreach (var item in menulist)
                    {
                        item.children = Create_Child(list, item.id);
                        item.hasChildren = false;
                    }
                    var q = menulist.OrderBy(t => t.id).ToPagedList(parm.pageindex, parm.pagesize);
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
        public IEnumerable<sys_menu> SubMenuByPid(MenuQueryParm parm, out int resultcount)
        {
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
        private List<sys_menu> Create_Child(IEnumerable<sys_menu> list, int id)
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
                }
                else
                {
                    menu.children = new List<sys_menu>();
                }
                menu.hasChildren = false;
            }
            return children;
        }

        public IEnumerable<dynamic> PermissionTree()
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT id,");
                sql.Append("pid,");
                sql.Append("title,menutype");
                sql.Append(" FROM sys_menu where status =1 order by pid,seq asc");
                using (var conn = new OraDBHelper().Conn)
                {
                    List<sys_menu> tree = new List<sys_menu>();
                    var list = conn.Query<sys_menu>(sql.ToString());
                    var list1 = list.Where(t => new string[] { "01", "02" }.Contains(t.menutype));
                    foreach (var item in list1.Where(t => t.pid == 0))
                    {
                        item.children = SubPermissionTree(list, list1, item.id);
                        tree.Add(item);
                    }
                    return tree;
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        private List<sys_menu> SubPermissionTree(IEnumerable<sys_menu> alllist, IEnumerable<sys_menu> list, int id)
        {
            try
            {
                List<sys_menu> children = new List<sys_menu>();
                foreach (var item in list.Where(t => t.pid == id))
                {
                    if (item.menutype == "02")
                    {
                        item.children = SubFun_Field(alllist, item.id);
                    }
                    else
                    {
                        item.children = SubPermissionTree(alllist, list, item.id);
                    }
                    children.Add(item);
                }
                return children;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        private List<sys_menu> SubFun_Field(IEnumerable<sys_menu> list, int id)
        {
            List<sys_menu> children = new List<sys_menu>();
            int fun_nodeid = list.Max(t => t.id) * 10;
            int edit_nodeid = fun_nodeid + 1;
            int hide_nodeid = fun_nodeid + 2;
            var funs = list.Where(t => t.pid == id && t.menutype == "03").Select(t => new sys_menu { id = t.id, pid = fun_nodeid, title = t.title }).ToList<sys_menu>();
            var fields = list.Where(t => t.pid == id && t.menutype == "04").Select(t => new sys_menu { id = t.id, pid = edit_nodeid, title = t.title }).ToList<sys_menu>();
            List<sys_menu> hidefields = new List<sys_menu>();
            foreach (var item in fields)
            {
                hidefields.Add(new sys_menu()
                {
                    id = item.id,
                    pid = hide_nodeid,
                    title = item.title
                });
            }

            if (funs.Count > 0)
            {
                children.Add(new sys_menu()
                {
                    id = fun_nodeid,
                    pid = id,
                    title = "页面功能",
                    children = funs,
                    menutype="03"
                });
            }
            if (fields.Count > 0)
            {
                children.Add(new sys_menu()
                {
                    id = edit_nodeid,
                    pid = id,
                    title = "编辑字段",
                    children = fields,
                    menutype = "04"
                }); 
                children.Add(new sys_menu()
                {
                    id = hide_nodeid,
                    pid = id,
                    title = "隐藏字段",
                    menutype="04",
                    children = hidefields
                });
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
                    return db.Conn.Execute(sql.ToString(), new { title = entity.title, id = entity.id });
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
                        int cnt = conn.Execute(sql.ToString(), new { id = id }, transaction: tran);
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
                    var resutl = db.Conn.Query<sys_menu>(sql.ToString(), para);
                    var q = resutl.OrderByDescending(t => t.id).ToPagedList(parm.pageindex, parm.pagesize);
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
                    return db.Conn.Execute(sql.ToString(), new { ids = ids });
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
                    }, p, splitOn: "id")
                        .Distinct()
                        .OrderBy(t => t.id)
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

        public string MenuMaxCode(int id)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select code from sys_menu where id = :pid; \r\n");
                sql.Append("select nvl(max(code)+1,1) as maxcode from SYS_MENU where pid = :pid; \r\n");

                using (var conn = new OraDBHelper().Conn)
                {
                    string code = conn.ExecuteScalar<string>("select code from sys_menu where id = :pid", new { pid = id });
                    code = code == null ? "" : code;
                    string maxcode = conn.ExecuteScalar<string>("select nvl(max(code),0) as maxcode from SYS_MENU where pid = :pid", new { pid = id }).ToString().PadLeft(2, '0');
                    string max = "";
                    if (!string.IsNullOrEmpty(code))
                    {
                        max = (Convert.ToInt32(maxcode.Replace(code, "")) + 1).ToString().PadLeft(2, '0');
                    }
                    else
                    {
                        max = (Convert.ToInt32(maxcode) + 1).ToString().PadLeft(2, '0');
                    }
                    return code + max;
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