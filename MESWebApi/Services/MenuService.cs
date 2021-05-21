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
                sql.Append(" order by pid,seq asc");

                using (var conn = new OraDBHelper().Conn)
                {
                    var list = conn.Query<sys_menu>(sql.ToString(), op);
                    IEnumerable<sys_menu> menulist = list.Where(t => t.pid == 0);
                    foreach (var item in menulist)
                    {
                        item.children = Create_Child(new List<sys_role_menu>(),list, item.id);
                        item.hasChildren = false;
                    }
                    var q = menulist.OrderBy(t => t.pid).ThenBy(t=>t.seq).ToPagedList(parm.pageindex, parm.pagesize);
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
        public sys_permission Get_UniMenu_Permission(IEnumerable<sys_role_menu> permislist, int menuid)
        {
            try
            {
                var menupermis_list = permislist.Where(t => t.menuid == menuid);
                sys_permission unitpermis = new sys_permission();
                //过滤菜单下的所有权限合并到unitpermis对象
                foreach (var permisitem in menupermis_list)
                {
                    sys_permission permission = new sys_permission();
                    if (permisitem.permis != null)
                    {
                        permission = JsonConvert.DeserializeObject<sys_permission>(permisitem.permis);
                        unitpermis.funs.AddRange(permission.funs);
                        unitpermis.editfields.AddRange(permission.editfields);
                        unitpermis.hidefields.AddRange(permission.hidefields);
                    }
                }
                unitpermis.funs = unitpermis.funs.Distinct().ToList();
                unitpermis.editfields = unitpermis.editfields.Distinct().ToList();
                unitpermis.hidefields = unitpermis.hidefields.Distinct().ToList();
                return unitpermis;
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
                sql.Append("select distinct tc.id, \r\n");
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
                sql.Append("and tb.menuid = tc.id order by tc.pid,tc.seq asc\r\n");


                StringBuilder permis_sql = new StringBuilder();
                permis_sql.Append("SELECT ta.roleid,ta.menuid,ta.permis FROM sys_role_menu ta,sys_user_role tb ");
                permis_sql.Append(" where ta.roleid = tb.roleid");
                permis_sql.Append(" and tb.userid = :userid ");

                using (var db = new OraDBHelper())
                {
                    List<sys_menu> menulist = new List<sys_menu>();
                    var list = db.Conn.Query<sys_menu>(sql.ToString(), new { userid = userid });
                    var permislist = db.Conn.Query<sys_role_menu>(permis_sql.ToString(), new { userid = userid });
                    foreach (var item in list.Where(t => t.pid == 0))
                    {
                        item.menu_permission = Get_UniMenu_Permission(permislist, item.id);
                        menulist.Add(item);
                        bool haschild = list.Where(t => t.pid == item.id).Count() > 0 ? true : false;
                        if (haschild)
                        {
                            item.children = Create_Child(permislist, list, item.id);
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
                log.Error(e.Message);
                throw;
            }
        }
        private List<sys_menu> Create_Child(IEnumerable<sys_role_menu> permislist, IEnumerable<sys_menu> list, int id)
        {
            List<sys_menu> children = new List<sys_menu>();
            foreach (var menu in list.Where(t => t.pid == id))
            {
                menu.menu_permission = Get_UniMenu_Permission(permislist,menu.id);
                children.Add(menu);
                bool haschild = list.Where(t => t.pid == menu.id).Count() > 0 ? true : false;
                if (haschild)
                {
                    menu.children = Create_Child(permislist, list, menu.id);
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
                sql.Append("code,title,menutype");
                sql.Append(" FROM sys_menu where status =1 order by pid,seq asc");
                using (var conn = new OraDBHelper().Conn)
                {
                    List<sys_menu> tree = new List<sys_menu>();
                    var list = conn.Query<sys_menu>(sql.ToString());
                    var pages = list.Where(t => t.menutype == "02");
                    int maxid = int.MaxValue;
                    List<sys_menu> alllist = new List<sys_menu>();
                    foreach (var item in list.Where(t => t.menutype == "01"))
                    {
                        alllist.Add(item);
                    } 
                    foreach (var item in pages)
                    {
                        alllist.Add(item);
                        sys_menu node = new sys_menu();
                        node.id = maxid - item.id*123;
                        node.pid = item.id;
                        node.title = "页面功能";
                        alllist.Add(node);
                        var flist = list.Where(t => t.menutype == "03" && t.pid == item.id).ToList();
                        foreach (var fitem in flist)
                        {
                            fitem.pid = node.id;
                            alllist.Add(fitem);
                        }
                        sys_menu node1 = new sys_menu();
                        node1.id = maxid - item.id *234;
                        node1.pid = item.id;
                        node1.title = "编辑字段";
                        alllist.Add(node1);
                        var elist = list.Where(t => t.menutype == "04" && t.pid == item.id).ToList();
                        string hjson = JsonConvert.SerializeObject(elist);
                        foreach (var eitem in elist)
                        {
                            eitem.pid = node1.id;
                            alllist.Add(eitem);
                        }
                        sys_menu node2 = new sys_menu();
                        node2.id = maxid - item.id * 345;
                        node2.pid = item.id;
                        node2.title = "隐藏字段";
                        alllist.Add(node2);
                        var hlist = JsonConvert.DeserializeObject<List<sys_menu>>(hjson);
                        foreach (var hitem in hlist)
                        {
                            hitem.id = hitem.id * 10;
                            hitem.pid = node2.id;
                            alllist.Add(hitem);
                        }
                    }
                    //foreach (var item in alllist)
                    //{
                    //    System.Console.WriteLine($"{item.id}----{item.pid}----{item.title}");
                    //}
                    foreach (var item in alllist.Where(t => t.pid == 0))
                    {
                        //System.Console.WriteLine($"{item.id}----{item.pid}----{item.title}");
                        item.children = SubPermissionTree(alllist, item.id);
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

        private List<sys_menu> SubPermissionTree(IEnumerable<sys_menu> list, int id)
        {
            try
            {
                List<sys_menu> children = new List<sys_menu>();
                foreach (var item in list.Where(t => t.pid == id))
                {
                    //System.Console.WriteLine($"{item.id}----{item.pid}----{item.title}");
                    item.children = SubPermissionTree(list, item.id);
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
            int fun_nodeid = int.MaxValue - id;
            int edit_nodeid = fun_nodeid - 1;
            int hide_nodeid = fun_nodeid - 2;
            var funs = list.Where(t => t.pid == id && t.menutype == "03").Select(t => new sys_menu { id = t.id, pid = fun_nodeid,code = t.code, title = t.title }).ToList<sys_menu>();
            var fields = list.Where(t => t.pid == id && t.menutype == "04").Select(t => new sys_menu { id = t.id, pid = edit_nodeid,code = t.code, title = t.title }).ToList<sys_menu>();
            List<sys_menu> hidefields = new List<sys_menu>();
            foreach (var item in fields)
            {
                hidefields.Add(new sys_menu()
                {
                    id = item.id,
                    pid = hide_nodeid,
                    title = item.title,
                    code = item.code
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
                sql.Append("addusername,");
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
                sql.Append("(select name from sys_user where id = :adduser),");
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
                sql.Append("update sys_menu set status=:status,");
                sql.Append(" code=:code,");
                sql.Append(" menutype=:menutype,");
                sql.Append(" icon=:icon,");
                sql.Append(" path=:path,");
                sql.Append(" title=:title,");
                sql.Append(" seq=:seq,");
                sql.Append(" updatetime=sysdate,");
                sql.Append(" updateuser=:updateuser,");
                sql.Append(" updateusername=(select name from sys_user where id=:updateuser),");
                sql.Append(" viewpath =:viewpath where id=:id ");
                using (var db = new OraDBHelper())
                {
                    return db.Conn.Execute(sql.ToString(), new { title = entity.title,
                        status=entity.status,
                        code=entity.code,
                        menutype=entity.menutype,
                        icon = entity.icon,
                        path = entity.path,
                        viewpath = entity.viewpath,
                        seq = entity.seq,
                        updateuser=entity.updateuser,
                        id = entity.id });
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
                        .OrderBy(t => t.pid)
                        .ThenBy(t=>t.seq)
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