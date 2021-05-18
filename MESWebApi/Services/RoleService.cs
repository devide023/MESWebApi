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
using Newtonsoft.Json;
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
                sql.Append("insert into sys_role (id,status,title,code,adduser,addtime,addusername) values (SEQ_ROLEID.nextval,:status,:title,:code,:adduser,sysdate,(select name from sys_user where id = :adduser)) returning id into :id ");
                OracleDynamicParameters p = new OracleDynamicParameters();
                p.Add(":status", entity.status, OracleMappingType.Int32, ParameterDirection.Input);
                p.Add(":title", entity.title, OracleMappingType.NVarchar2, ParameterDirection.Input);
                p.Add(":code", MaxCode(), OracleMappingType.NVarchar2, ParameterDirection.Input);
                p.Add(":adduser", entity.adduser, OracleMappingType.Int32, ParameterDirection.Input);
                p.Add(":addusername", entity.addusername, OracleMappingType.NVarchar2, ParameterDirection.Input);                p.Add(":id", null, OracleMappingType.Int32, ParameterDirection.ReturnValue);
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
                using (var conn = new OraDBHelper().Conn)
                {
                    conn.Open();
                    var p = new { id = id };
                    using (var trans = conn.BeginTransaction())
                    {
                        int r1 = conn.Execute(sql.ToString(), p, transaction: trans);
                        int r2 = conn.Execute("delete from sys_user_role where roleid = :id", p, transaction: trans);
                        int r3 = conn.Execute("delete from sys_role_menu where roleid = :id", p, transaction: trans);
                        trans.Commit();
                        return r1 > 0 ? true : false;
                    }
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
                sql.Append("select ta.id,");
                sql.Append(" ta.code,");
                sql.Append(" ta.status,");
                sql.Append(" ta.title,");
                sql.Append(" ta.adduser,");
                sql.Append(" ta.addusername,");
                sql.Append(" tc.id,");
                sql.Append(" tc.pid,");
                sql.Append(" tc.title,");
                sql.Append(" tc.menutype,");
                sql.Append(" tc.code,");
                sql.Append(" tc.seq");
                sql.Append(" from sys_role ta, sys_role_menu tb, sys_menu tc");
                sql.Append(" where ta.id = tb.roleid ");
                sql.Append(" and tb.menuid = tc.id ");
                sql.Append(" and tc.status = 1 ");
                sql.Append(" and ta.status = 1 ");
                sql.Append(" and ta.id = :id ");
                using (var conn = new OraDBHelper().Conn)
                {
                    Dictionary<int, sys_role> dic_role = new Dictionary<int, sys_role>();
                    var role_menus = conn.Query<sys_role_menu>("select roleid,menuid,permis from sys_role_menu where roleid=:id", new { id = id });
                    var query = conn.Query<sys_role, sys_menu, sys_role>(sql.ToString(), (role, menu) =>
                    {
                        sys_role roleentity = new sys_role();
                        if(!dic_role.TryGetValue(role.id,out roleentity))
                        {
                            roleentity = role;
                            roleentity.role_menus = new List<sys_menu>();
                            dic_role.Add(roleentity.id, roleentity);
                        }
                        var permis = role_menus.Where(t => t.menuid == menu.id).Select(t => t.permis).FirstOrDefault();
                        if (permis != null)
                        {
                            menu.menu_permission = JsonConvert.DeserializeObject<sys_permission>(permis);
                        }
                        else
                        {
                            menu.menu_permission = new sys_permission();
                        }
                        roleentity.role_menus.Add(menu);
                        return roleentity;
                    }, new { id=id});
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
                sql.Append(" select id,code,status,title,adduser,addtime,addusername from sys_role where 1=1 ");
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
                sql.Append("update sys_role set title=:title,status=:status,updatetime=sysdate,updateuser=:updateuser,updateusername=:upusername where id = :id");
                using (var db = new OraDBHelper())
                {
                    entity.updateusername = db.Conn.ExecuteScalar<string>("select name from sys_user where id=:id", new { id = entity.updateuser });
                   return db.Conn.Execute(sql.ToString(), new {id=entity.id,title=entity.title,status=entity.status, updateuser=entity.updateuser, upusername=entity.updateusername });
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
        public int Save_RoleMenus(List<sys_menu_permission> list) {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into sys_role_menu(id,roleid,menuid,permis) \r\n");
                sql.Append("values \r\n");
                sql.Append(" (SEQ_ROLEMENU_ID.nextval,:roleid,:menuid,:permis) \r\n");
                List<int> roleid = list.Select(t => t.roleid).ToList();
                List<dynamic> p = new List<dynamic>();
                foreach (var item in list)
                {
                    var json = JsonConvert.SerializeObject(item.permission);
                    p.Add(new
                    {
                        roleid = item.roleid,
                        menuid = item.menuid,
                        permis = json
                    });
                }
                using (var conn = new OraDBHelper().Conn)
                {
                    conn.Open();
                    using (var trans = conn.BeginTransaction())
                    {
                        conn.Execute("delete from sys_role_menu where roleid in :roleid", new { roleid = roleid }, trans);
                        int ret = conn.Execute(sql.ToString(), p, trans);
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
        public int Save_RolePermis(List<sys_menu_permission> list)
        {
            try
            {
                var roleids = list.Select(t => t.roleid).ToList();
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into sys_role_permis(id,roleid,menuid,permis) ");
                sql.Append(" values ");
                sql.Append(" (seq_rolepromis_id.nextval,:roleid,:menuid,:permis)");
                List<dynamic> p = new List<dynamic>();
                foreach (var item in list)
                {
                    var json = JsonConvert.SerializeObject(item.permission);
                    p.Add(new
                    {
                        roleid=item.roleid,
                        menuid=item.menuid,
                        permis=json
                    });
                }
                using (var conn = new OraDBHelper().Conn)
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        conn.Execute("delete from sys_role_permis where roleid in :roleid", new { roleid = roleids }, transaction: tran);
                        int cnt = conn.Execute(sql.ToString(), p,transaction:tran);
                        tran.Commit();
                        return cnt;
                    }
                    
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public string MaxCode()
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select LPad(max(code) + 1, 2, '0') as maxcode from Sys_Role");
                using (var conn = new OraDBHelper().Conn)
                {
                    return conn.ExecuteScalar<string>(sql.ToString());
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public IEnumerable<sys_menu> Get_Role_Menus(int roleid) {

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select tb.id,");
                sql.Append(" tb.pid,");
                sql.Append(" tb.title,");
                sql.Append(" tb.menutype,");
                sql.Append(" tb.code,");
                sql.Append(" tb.seq,");
                sql.Append(" ta.permis");
                sql.Append(" from   sys_role_menu ta, sys_menu tb");
                sql.Append(" where  ta.roleid = :id");
                sql.Append(" and tb.status = 1");
                sql.Append(" and tb.id = ta.menuid");

                StringBuilder sql1 = new StringBuilder();
                sql1.Append(" select ta.id, ta.pid, ta.title, ta.menutype, ta.code ");
                sql1.Append(" from   SYS_MENU ta, sys_role_menu tb ");
                sql1.Append(" where  ta.pid = tb.menuid ");
                sql1.Append(" and ta.menutype in ('03', '04') ");
                sql1.Append(" and tb.roleid = :id ");

                using (var conn = new OraDBHelper().Conn)
                {
                   var list = conn.Query(sql.ToString(), new { id = roleid });
                    var list_fun = conn.Query<sys_menu>(sql1.ToString(), new { id = roleid });
                    int maxid = int.MaxValue;
                    List<sys_menu> menulist = new List<sys_menu>();
                    
                    foreach (var item in list)
                    {
                        sys_menu mitem = new sys_menu()
                        {
                            id = Convert.ToInt32(item.ID),
                            pid = Convert.ToInt32(item.PID),
                            title = item.TITLE,
                            menutype = item.MENUTYPE,
                            code = item.CODE,
                            seq = Convert.ToInt32(item.SEQ)
                        };
                        int funpageid = maxid - mitem.id;
                        int eidtpageid = funpageid - 1;
                        int hidepageid = funpageid - 2;
                        if ( item.PERMIS!=null)
                        {
                            sys_permission jsonobj = JsonConvert.DeserializeObject<sys_permission>(item.PERMIS.ToString());
                            mitem.menu_permission = jsonobj;
                            if (jsonobj.funs.Count > 0)
                            {
                                menulist.Add(new sys_menu()
                                {
                                    id = funpageid,
                                    pid = mitem.id,
                                    title="页面功能",
                                    menutype="03",
                                }) ;

                                foreach (var obj in jsonobj.funs)
                                {
                                    var fun_temp = list_fun.Where(t => t.pid == mitem.id && t.code == obj).FirstOrDefault();
                                    if (fun_temp == null)
                                    {
                                        continue;
                                    }
                                    sys_menu funitem = new sys_menu();
                                    funitem.menutype = "03";
                                    funitem.code = fun_temp.code;
                                    funitem.title = fun_temp.title;
                                    funitem.pid = funpageid;
                                    funitem.id = fun_temp.id;
                                    menulist.Add(funitem);
                                }
                            }
                            if (jsonobj.editfields.Count > 0)
                            {
                                menulist.Add(new sys_menu()
                                {
                                    id = eidtpageid,
                                    pid = mitem.id,
                                    title = "编辑字段",
                                    menutype = "04",
                                });
                                foreach (var obj in jsonobj.editfields)
                                {
                                    var edit_temp = list_fun.Where(t => t.pid == mitem.id && t.code == obj).FirstOrDefault();
                                    if (edit_temp == null)
                                    {
                                        continue;
                                    }
                                    sys_menu editf = new sys_menu();
                                    editf.menutype = "04";
                                    editf.code = edit_temp.code;
                                    editf.title = edit_temp.title;
                                    editf.pid = eidtpageid;
                                    editf.id = edit_temp.id;
                                    menulist.Add(editf);
                                }
                            }
                            if (jsonobj.hidefields.Count > 0)
                            {
                                menulist.Add(new sys_menu()
                                {
                                    id= hidepageid,
                                    pid = mitem.id,
                                    title = "隐藏字段",
                                    menutype = "04",
                                });
                                foreach (var obj in jsonobj.hidefields)
                                {
                                    var hide_temp = list_fun.Where(t => t.pid == mitem.id && t.code == obj).FirstOrDefault();
                                    if (hide_temp == null)
                                    {
                                        continue;
                                    }
                                    sys_menu hidef = new sys_menu();
                                    hidef.menutype = "04";
                                    hidef.code = hide_temp.code;
                                    hidef.title = hide_temp.title;
                                    hidef.pid = hidepageid;
                                    hidef.id = hide_temp.id;
                                    menulist.Add(hidef);
                                }
                            }
                        }
                        else
                        {
                            mitem.menu_permission = new sys_permission();
                        }
                        menulist.Add(mitem);
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

    }
}