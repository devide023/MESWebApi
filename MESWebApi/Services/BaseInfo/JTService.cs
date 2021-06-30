using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MESWebApi.Models;
using MESWebApi.Models.BaseInfo;
using MESWebApi.Models.QueryParm;
using System.Text;
using Dapper;
using Dapper.Oracle;
using MESWebApi.DB;
using MESWebApi.InterFaces;
using log4net;
using Webdiyer.WebControls.Mvc;
using MESWebApi.Util;
using DapperExtensions;
using DapperExtensions.Sql;
using DapperExtensions.Mapper;
using System.Reflection;
using NPOI.SS.UserModel;
using System.IO;
namespace MESWebApi.Services.BaseInfo
{
    /// <summary>
    /// 技术通知
    /// </summary>
    public class JTService : DBOperImp<zxjc_t_jstc>, IComposeQuery<zxjc_t_jstc, sys_page>
    {
        private ILog log;
        private string constr = string.Empty;
        public JTService()
        {
            log = LogManager.GetLogger(this.GetType());
            constr = "tjmes";
        }

        public IEnumerable<zxjc_t_jstc> Search(sys_page parm, out int resultcount)
        {
            try
            {
                OracleDynamicParameters p = new OracleDynamicParameters();
                StringBuilder sql = new StringBuilder();
                sql.Append("select jtid,jcbh,jcmc,jcms,wjlj,jwdx,scry,scpc,scsj,yxqx1,yxqx2,gcdm,fp_flg,fp_sj,fpr,wjfl,scx from zxjc_t_jstc where 1=1 ");
                if (!string.IsNullOrEmpty(parm.keyword))
                {
                    sql.Append(" and (jcmc like :key or jcbh like :key or jcms like :key) ");
                    p.Add(":key", "%" + parm.keyword + "%", OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                }
                if (parm.explist.Count > 0)
                {
                    sql.Append(" and ");
                    sql.Append(Tool.ComQueryExp(parm.explist));
                }
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    var q = conn.Query<zxjc_t_jstc>(sql.ToString(), p)
                         .OrderByDescending(t => t.jcbh)
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

        public IEnumerable<zxjc_t_jstc> FromExcel(string path)
        {
            FileInfo fi = new FileInfo(path);
            try
            {
                List<zxjc_t_jstc> list = new List<zxjc_t_jstc>();
                ExcelHelper xls = new ExcelHelper();
                IWorkbook book = xls.ReadExcel(path);
                ISheet sheet = book.GetSheetAt(0);
                int rows = sheet.LastRowNum;
                for (int i = 1; i <= rows; i++)
                {
                    IRow row = sheet.GetRow(i);
                    string jtbh = GetJTNumber();
                    DateTime rq1 = row.GetCell(2).DateCellValue;
                    DateTime rq2 = row.GetCell(3).DateCellValue;
                    zxjc_t_jstc entity = new zxjc_t_jstc()
                    {
                        gcdm = "9100",
                        jtid = Guid.NewGuid().ToString(),
                        jcbh = jtbh,
                        scx = row.GetCell(4).NumericCellValue.ToString(),
                        jcmc = row.GetCell(0).StringCellValue,
                        jcms = row.GetCell(1).StringCellValue,
                        yxqx1 = rq1,
                        yxqx2 = rq2,
                        wjlj = row.GetCell(0).StringCellValue,
                        wjfl = "技术通知",
                        fp_flg = "N"
                    };
                    list.Add(entity);
                }
                fi.Delete();
                return list;
            }
            catch (Exception e)
            {
                fi.Delete();
                log.Error(e.Message);
                throw;
            }
        }
        public int ModifyFileNames(zxjc_t_jstc entity)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update ZXJC_T_JSTC");
                sql.Append("  set ");
                sql.Append("         wjlj = :wjlj,");
                sql.Append("         jwdx = :jwdx,");
                sql.Append("         scry = :scry,");
                sql.Append("         scpc = :scpc,");
                sql.Append("         scsj = :scsj ");
                sql.Append("  where  jtid = :jtid");
                return Conn.Execute(sql.ToString(), entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public override int Modify(zxjc_t_jstc entity)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update ZXJC_T_JSTC");
                sql.Append("  set jcmc = :jcmc,");
                sql.Append("         jcms = :jcms,");
                sql.Append("         wjlj = :wjlj,");
                sql.Append("         jwdx = :jwdx,");
                sql.Append("         scry = :scry,");
                sql.Append("         scpc = :scpc,");
                sql.Append("         scsj = :scsj,");
                sql.Append("         yxqx1 = :yxqx1,");
                sql.Append("         yxqx2 = :yxqx2,");
                sql.Append("         gcdm = :gcdm,");
                sql.Append("         fp_flg = :fp_flg,");
                sql.Append("         fp_sj = :fp_sj,");
                sql.Append("         fpr = :fpr,");
                sql.Append("         wjfl = :wjfl,");
                sql.Append("         scx = :scx");
                sql.Append("  where  jtid = :jtid");
                return Conn.Execute(sql.ToString(), entity);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int Modify(List<zxjc_t_jstc> entitys)
        {
            try
            {
                List<int> li = new List<int>();
                foreach (var item in entitys)
                {
                   int ret = Modify(item);
                    li.Add(ret);
                }
                return li.Count == entitys.Count ? 1 : 0;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
        public int Delete(List<zxjc_t_jstc> entitys)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("delete from ZXJC_T_JSTC where jcbh in :jcbh ");
                return Conn.Execute(sql.ToString(), entitys.ToArray());
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
        public string GetJTNumber()
        {
            try
            {
                int jtseq = Conn.ExecuteScalar<int>("SELECT seq_telnotice_id.nextval FROM dual");
                return "JT-" + jtseq.ToString().PadLeft(9, '0');
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

    }
    /// <summary>
    /// 特殊技术通知
    /// </summary>
    public class TsJTService : DBOperImp<zxjc_t_tstc>, IComposeQuery<zxjc_t_tstc, sys_page>
    {
        private ILog log;
        public TsJTService(string constr = "tjmes") : base(constr)
        {
            log = LogManager.GetLogger(this.GetType());
        }
        /// <summary>
        /// 特殊技术通知编号
        /// </summary>
        /// <returns></returns>
        public string SpecialNoticeNo()
        {
            try
            {
                int no = Conn.ExecuteScalar<int>("SELECT seq_special_noticno.nextval from dual");
                return "Tstz" + no.ToString().PadLeft(5, '0');
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IEnumerable<zxjc_t_tstc> Search(sys_page parm, out int resultcount)
        {
            try
            {
                OracleDynamicParameters p = new OracleDynamicParameters();
                StringBuilder sql = new StringBuilder();
                sql.Append("select tcid,tcbh,tcms,gcdm,scx,gwh,jx_no,status_no,yxbz,lrr,lrsj from zxjc_t_tstc where 1=1 ");
                if (!string.IsNullOrEmpty(parm.keyword))
                {
                    sql.Append(" and (tcbh like :key or jx_no like :key or status_no like :key) ");
                    p.Add(":key", "%" + parm.keyword + "%", OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                }
                if (parm.explist.Count > 0)
                {
                    sql.Append(" and ");
                    sql.Append(Tool.ComQueryExp(parm.explist));
                }

                var q = Conn.Query<zxjc_t_tstc>(sql.ToString(), p)
                     .OrderByDescending(t => t.tcid)
                     .ToPagedList(parm.pageindex, parm.pagesize);
                resultcount = q.TotalItemCount;
                return q;

            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
    }
    /// <summary>
    /// 技术通知查看记录
    /// </summary>
    public class JTYDService : DBOperImp<zxjc_t_ydjl>, IComposeQuery<zxjc_t_ydjl, sys_page>
    {
        private ILog log;
        public JTYDService(string constr = "tjmes") : base(constr)
        {
            log = LogManager.GetLogger(this.GetType());
        }        

        public IEnumerable<zxjc_t_ydjl> Search(sys_page parm, out int resultcount)
        {
            try
            {
                OracleDynamicParameters p = new OracleDynamicParameters();
                StringBuilder sql = new StringBuilder();
                sql.Append("select ydid,jtid,gcdm,scx,gwh,order_no,engine_no,user_name,ydsj,lrr,lrsj from zxjc_t_ydjl where 1=1 ");
                if (!string.IsNullOrEmpty(parm.keyword))
                {
                    sql.Append(" and (order_no like :key or engine_no like :key or user_name like :key) ");
                    p.Add(":key", "%" + parm.keyword + "%", OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                }
                if (parm.explist.Count > 0)
                {
                    sql.Append(" and ");
                    sql.Append(Tool.ComQueryExp(parm.explist));
                }
                var q = Conn.Query<zxjc_t_ydjl>(sql.ToString(), p)
                     .OrderByDescending(t => t.ydid)
                     .ToPagedList(parm.pageindex, parm.pagesize);
                resultcount = q.TotalItemCount;
                return q;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
    }
}