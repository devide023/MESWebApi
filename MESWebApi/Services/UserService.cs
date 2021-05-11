using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MESWebApi.DB;
using Dapper;
using System.Threading.Tasks;
using MESWebApi.Util;
using MESWebApi.Models;
using MESWebApi.InterFaces;
using log4net;
using Dapper;
using Dapper.Oracle;
using System.Text;
using System.Data;
using MESWebApi.Models.QueryParm;
using Webdiyer.WebControls.Mvc;
namespace MESWebApi.Services
{
    public class UserService : IDBOper<sys_user>, IComposeQuery<sys_user, UserQueryParm>
    {
        private ILog log;
        public UserService()
        {
            log = LogManager.GetLogger(this.GetType());
        }

        public sys_user Add(sys_user entity)
        {
            try
            {
                string token = new JWTHelper().CreateToken();
                entity.token = token;
                var md5pwd = Tool.Str2MD5(entity.pwd);
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into sys_user(id, status, code, name, pwd, token, adduser,addtime) \r\n");
                sql.Append("values \r\n");
                sql.Append("(seq_userid.nextval,:status,:code,:name,:pwd,:token,:adduser,sysdate) returning id into :id \r\n");
                OracleDynamicParameters p = new OracleDynamicParameters();
                p.Add(":status", entity.status, OracleMappingType.Int32, ParameterDirection.Input);
                p.Add(":code", entity.code, OracleMappingType.NVarchar2, ParameterDirection.Input);
                p.Add(":name", entity.name, OracleMappingType.NVarchar2, ParameterDirection.Input);
                p.Add(":pwd", md5pwd, OracleMappingType.NVarchar2, ParameterDirection.Input);
                p.Add(":token", token, OracleMappingType.NVarchar2, ParameterDirection.Input);
                p.Add(":adduser", entity.adduser, OracleMappingType.Int32, ParameterDirection.Input);
                p.Add(":id", null, OracleMappingType.Int32, ParameterDirection.ReturnValue);
                using (var db = new OraDBHelper())
                {
                    int cnt = db.Conn.Execute(sql.ToString(), p);
                    int userid = p.Get<int>(":id");
                    entity.id = userid;
                    return entity;
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public string CheckUserLogin(string username, string userpwd)
        {
            try
            {
                string md5pwd = Tool.Str2MD5(userpwd);
                StringBuilder sql = new StringBuilder();
                sql.Append("select count(*) as cnt from sys_user where status=1 and code=:code and pwd=:pwd");
                var p = new { code = username, pwd = md5pwd };
                using (var db = new OraDBHelper())
                {
                    var isok = db.Conn.ExecuteScalar<int>(sql.ToString(), p);
                    if (isok > 0)
                    {
                        return db.Conn.ExecuteScalar<string>("select token from sys_user where code=:code and pwd=:pwd", p);
                    }
                    else
                    {
                        return string.Empty;
                    }
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

        public sys_user Find(int id)
        {
            throw new NotImplementedException();
        }

        public int Modify(sys_user entity)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("update sys_user set name=:name where id=:id");
                using (var conn = new OraDBHelper().Conn)
                {
                    return conn.Execute(sql.ToString(), new {
                    id=entity.id,
                    name=entity.name
                    });
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public sys_user UserInfo(string token)
        {
            try
            {
                string imgurl = "http://" + HttpContext.Current.Request.Url.Authority + "/Images/headimg/";
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT id,code,name,status,headimg FROM sys_user where token=:token");
                using (var db = new OraDBHelper())
                {
                    var query = db.Conn.Query<sys_user>(sql.ToString(), new { token = token }).FirstOrDefault();
                    query.headimg = imgurl + (string.IsNullOrEmpty(query.headimg) ? "default.jpg" : query.headimg);
                    return query;
                }

            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public int SaveUserRoles(int userid, List<int> roleids)
        {
            try
            {
                int ret = 0;
                List<dynamic> ur = new List<dynamic>();
                foreach (var item in roleids)
                {
                    ur.Add(new { userid = userid, roleid = item });
                }
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into sys_user_role \r\n");
                sql.Append("(id,");
                sql.Append("userid,");
                sql.Append("roleid)");
                sql.Append("values");
                sql.Append("(seq_userrole_id.nextval,");
                sql.Append("  :userid,");
                sql.Append("  :roleid");
                sql.Append(")\r\n");
                using (var conn = new OraDBHelper().Conn)
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        conn.Execute("delete from sys_user_role where userid=:userid", new { userid = userid }, transaction);
                        ret = conn.Execute(sql.ToString(), ur.ToArray(), transaction);
                        transaction.Commit();
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

        public int FreshToken(int userid)
        {
            try
            {
                var token = new JWTHelper().CreateToken();
                StringBuilder sql = new StringBuilder();
                sql.Append("update sys_user set token=:token where id=:userid");
                using (var conn = new OraDBHelper().Conn)
                {
                    return conn.Execute(sql.ToString(), new { userid = userid, token = token });
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public IEnumerable<sys_user> Search(UserQueryParm parm, out int resultcount)
        {
            try
            {
                OracleDynamicParameters p = new OracleDynamicParameters();
                StringBuilder sql = new StringBuilder();
                sql.Append("select ta.id, ta.code, ta.name, ta.pwd, ta.token, ta.adduser, ta.addtime, ta.status, ta.headimg,tc.id,tc.code,tc.title,tc.status \r\n");
                sql.Append(" from sys_user ta, sys_user_role tb, sys_role tc where ta.id = tb.userid and tb.roleid = tc.id \r\n");
                if (!string.IsNullOrEmpty(parm.keyword))
                {
                    sql.Append(" and (ta.code like :key or ta.name like :key) \r\n");
                    p.Add(":key", parm.keyword, OracleMappingType.NVarchar2, ParameterDirection.Input);
                }
                using (var conn = new OraDBHelper().Conn)
                {
                    var userdic = new Dictionary<int, sys_user>();
                    var q = conn.Query<sys_user, sys_role, sys_user>(sql.ToString(),
                        (user, role) =>
                        {
                            sys_user userEntry;
                            if (!userdic.TryGetValue(user.id, out userEntry))
                            {
                                userEntry = user;
                                userEntry.roles = new List<sys_role>();
                                userdic.Add(userEntry.id, userEntry);
                            }

                            userEntry.roles.Add(role);
                            return userEntry;
                        }, p,splitOn:"id")
                        .Distinct()
                        .OrderByDescending(t => t.id)
                        .ToPagedList(parm.pageindex, parm.pagesize);
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

        public bool IsUserCodeExist(string usercode)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select count(*) cnt from sys_user where code = :code");
                using (var conn = new OraDBHelper().Conn)
                {
                    return conn.ExecuteScalar<int>(sql.ToString(), new { code = usercode }) > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public int ChangePwd(int userid,string pwd)
        {
            try
            {
                string pwd1 = Tool.Str2MD5(pwd);
                StringBuilder sql = new StringBuilder();
                sql.Append("update sys_user set pwd=:pwd where id=:userid");
                using (var conn = new OraDBHelper().Conn)
                {
                   return conn.Execute(sql.ToString(), new {userid=userid,pwd=pwd1 });
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