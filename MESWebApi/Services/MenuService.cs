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

namespace MESWebApi.Services
{
    public class MenuService
    {
        public MenuService()
        {

        }

        public int Get_Menuid()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("declare \r\n");
            sql.Append("id number;\r\n");
            sql.Append("begin\r\n");
            sql.Append("id := 11;\r\n");
            sql.Append("dbms_output.put_line(id);\r\n");
            sql.Append("end;\r\n");
            using (var db = new OraDBHelper())
            {
               var ret = db.Conn.Query(sql.ToString()).FirstOrDefault();
               int cnt = db.Conn.ExecuteScalar<int>(sql.ToString());

                return cnt;
            }
        }

        public int Add_Menu(sys_menu menu)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into sys_menu(");
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
            sql.Append("(");
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
            using (var db = new OraDBHelper()) {
                OracleDynamicParameters param = new OracleDynamicParameters();
                param.Add(":title", menu.title, OracleMappingType.NVarchar2, ParameterDirection.Input);
                param.Add(":pid", menu.pid, OracleMappingType.Int32, ParameterDirection.Input);
                param.Add(":icon", menu.icon, OracleMappingType.NVarchar2, ParameterDirection.Input);
                param.Add(":code", menu.code, OracleMappingType.NVarchar2, ParameterDirection.Input);
                param.Add(":path", menu.path, OracleMappingType.NVarchar2, ParameterDirection.Input);
                param.Add(":menutype", menu.menutype, OracleMappingType.NVarchar2, ParameterDirection.Input);
                param.Add(":viewpath", menu.viewpath, OracleMappingType.NVarchar2, ParameterDirection.Input);
                param.Add(":adduser", menu.adduser, OracleMappingType.Int32, ParameterDirection.Input);
                param.Add(":seq", menu.seq, OracleMappingType.Int32,ParameterDirection.Input);
                param.Add(":id", null, OracleMappingType.Int32,ParameterDirection.Output);
                var ret = db.Conn.Execute(sql.ToString(), param);
                var menuid = param.Get<int>(":id");
                return menuid;
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