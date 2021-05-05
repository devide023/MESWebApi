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
using System.Data;

namespace MESWebApi.Services
{
    public class MenuService
    {
        public MenuService()
        {

        }

        public int Add_Menu(sys_menu menu)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("declare \r\n l_id number; \r\n");
            sql.Append("begin \r\n");
            sql.Append("insert into sys_menu(");
            sql.Append("title,");
            sql.Append("pid,");
            sql.Append("controller,");
            sql.Append("action,");
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
            sql.Append("(");
            sql.Append(":title,");
            sql.Append(":pid,");
            sql.Append(":controller,");
            sql.Append(":action,");
            sql.Append(":icon,");
            sql.Append(":code,");
            sql.Append(":path,");
            sql.Append(":menutype,");
            sql.Append(":viewpath,");
            sql.Append("sysdate,");
            sql.Append(":adduser,");
            sql.Append(":seq");
            sql.Append(");returning adduser into l_id;select l_id from dual;\r\n");
            sql.Append("end;");
            using (var db = new OraDBHelper()) {
               
                int cnt = db.Conn.ExecuteScalar<int>(sql.ToString(),menu);
                return cnt;
            }
        }

        public IEnumerable<sys_menu> User_Menus(int userid)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select id,");
            sql.Append("title,");
            sql.Append("pid,");
            sql.Append("controller,");
            sql.Append("action,");
            sql.Append("icon,");
            sql.Append("url,");
            sql.Append("code,");
            sql.Append("path,");
            sql.Append("menutype,");
            sql.Append("viewpath,");
            sql.Append("addtime,");
            sql.Append("adduser,");
            sql.Append("seq \r\n");
            sql.Append("from SYS_MENU");

            using (var db = new OraDBHelper())
            {
               return db.Conn.Query<sys_menu>(sql.ToString());
            }
        }

        public IEnumerable<sys_menu> Get_User_Menus(int userid)
        {
            string path = HttpContext.Current.Server.MapPath("~/menus.json");
            string json = File.ReadAllText(path, Encoding.UTF8);
            return JsonConvert.DeserializeObject<IEnumerable<sys_menu>>(json);
        }
    }
}