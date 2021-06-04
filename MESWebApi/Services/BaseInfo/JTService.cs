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
    public class JTService:DBOperImp<zxjc_t_jstc>,IComposeQuery<zxjc_t_jstc,sys_page>
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
                         .OrderByDescending(t => t.scsj)
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
            try
            {
                List<zxjc_t_jstc> list = new List<zxjc_t_jstc>();
                ExcelHelper xls = new ExcelHelper();
                IWorkbook book = xls.ReadExcel(path);
                ISheet sheet = book.GetSheetAt(0);
                int rows = sheet.LastRowNum;
                for (int i = 1; i < rows; i++)
                {
                    IRow row = sheet.GetRow(i);
                    string jtbh = string.Empty;
                    int jtid = Conn.ExecuteScalar<int>("SELECT seq_telnotice_id.nextval FROM dual");
                    jtbh = "JT-" + jtid.ToString().PadLeft(9, '0');
                    DateTime rq1 = row.GetCell(3).DateCellValue;
                    DateTime rq2 = row.GetCell(4).DateCellValue;
                    zxjc_t_jstc entity = new zxjc_t_jstc()
                    {
                        gcdm = "9100",
                        jtid = Guid.NewGuid().ToString(),
                        jcbh = jtbh,
                        scx = row.GetCell(8).StringCellValue,
                        jcmc = row.GetCell(1).StringCellValue,
                        jcms = row.GetCell(2).StringCellValue,
                        yxqx1 = rq1,
                        yxqx2 = rq2,
                        wjfl="技术通知",
                        fp_flg="N"
                    };
                    list.Add(entity);
                }
                return list;
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
        private string constr = string.Empty;
        public TsJTService()
        {
            log = LogManager.GetLogger(this.GetType());
            constr = "tjmes";
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
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    var q = conn.Query<zxjc_t_tstc>(sql.ToString(), p)
                         .OrderByDescending(t => t.tcid)
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
    }
    /// <summary>
    /// 技术通知查看记录
    /// </summary>
    public class JTYDService : DBOperImp<zxjc_t_ydjl>,IComposeQuery<zxjc_t_ydjl,sys_page>
    {
        private ILog log;
        private string constr = string.Empty;
        public JTYDService()
        {
            log = LogManager.GetLogger(this.GetType());
            constr = "tjmes";
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
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    var q = conn.Query<zxjc_t_ydjl>(sql.ToString(), p)
                         .OrderByDescending(t => t.ydid)
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
    }
}