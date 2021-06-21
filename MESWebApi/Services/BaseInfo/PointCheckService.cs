using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
using NPOI.SS.UserModel;

namespace MESWebApi.Services.BaseInfo
{
    /// <summary>
    /// 点检
    /// </summary>
    public class PointCheckService : IDBOper<zxjc_djgw>, IComposeQuery<zxjc_djgw, CheckPointQueryParm>
    {
        ILog log;
        string constr = String.Empty;
        public PointCheckService()
        {
            log = LogManager.GetLogger(this.GetType());
            constr = "tjmes";
        }

        public zxjc_djgw Add(zxjc_djgw entity)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into zxjc_djgw ");
                sql.Append("(gcdm, scx, gwh, jx_no, status_no, djno, djxx, scbz, lrr, lrsj, djlx)");
                sql.Append(" values ");
                sql.Append(" (:gcdm,:scx,:gwh,:jx_no,:status_no,:djno,:djxx,:scbz,:lrr, sysdate,:djlx)");
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    int ret = conn.Execute(sql.ToString(), entity);
                    if (ret > 0)
                    {
                        return new zxjc_djgw();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
        public int Add(List<zxjc_djgw> entitys)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into zxjc_djgw ");
                sql.Append("(gcdm, scx, gwh, jx_no, status_no, djno, djxx, scbz, lrr, lrsj, djlx)");
                sql.Append(" values ");
                sql.Append(" (:gcdm,:scx,:gwh,:jx_no,:status_no,:djno,:djxx,:scbz,:lrr, sysdate,:djlx)");
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    int ret = conn.Execute(sql.ToString(), entitys.ToArray());
                    return ret;
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
        public int Delete(List<zxjc_djgw> entitys) {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("delete from zxjc_djgw where djno in :djno ");
                using (var conn = new OraDBHelper(constr).Conn)
                {
                   return conn.Execute(sql.ToString(), entitys.ToArray());
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
        public zxjc_djgw Find(int id)
        {
            throw new NotImplementedException();
        }

        public int Modify(zxjc_djgw entity)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("update zxjc_djgw ");
                sql.Append("  set gcdm = :gcdm, ");
                sql.Append("      scx = :scx, ");
                sql.Append("      gwh = :gwh, ");
                sql.Append("      jx_no = :jx_no, ");
                sql.Append("      status_no = :status_no, ");
                sql.Append("      djno = :djno, ");
                sql.Append("      djxx = :djxx, ");
                sql.Append("      scbz = :scbz, ");
                sql.Append("      djlx = :djlx ");
                sql.Append(" where djno = :djno ");
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    return conn.Execute(sql.ToString(), entity);
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public IEnumerable<zxjc_djgw> Search(CheckPointQueryParm parm, out int resultcount)
        {
            try
            {
                OracleDynamicParameters p = new OracleDynamicParameters();
                StringBuilder sql = new StringBuilder();
                sql.Append("select ta.gcdm, ta.scx, ta.gwh,(select work_name from zxjc_gxzd where work_no = ta.gwh) as gwmc,ta.jx_no, ta.status_no, ta.djno, ta.djxx, ta.scbz, ta.lrr, ta.lrsj, ta.djlx ");
                sql.Append(" from zxjc_djgw ta where 1 = 1 ");
                if (!string.IsNullOrEmpty(parm.keyword))
                {
                    sql.Append(" and (ta.djxx like :key or ta.status_no like :key )");
                    p.Add(":key", "%" + parm.keyword + "%", OracleMappingType.Varchar2, System.Data.ParameterDirection.Input);
                }
                if (parm.explist.Count > 0)
                {
                    sql.Append(" and ");
                    sql.Append(Util.Tool.ComQueryExp(parm.explist));
                }
                using (var conn = new OraDBHelper(constr).Conn)
                {
                    var q = conn.Query<zxjc_djgw>(sql.ToString(), p)
                         .OrderByDescending(t => t.djno)
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
        /// <summary>
        /// 点检编号
        /// </summary>
        public string GetDJNo()
        {
            try
            {
                using (var conn = new OraDBHelper(constr).Conn)
                {
                   var no = conn.ExecuteScalar<int>("select seq_pointcheck_no.nextval from dual");
                    return "DJ" + no.ToString().PadLeft(4, '0');
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
        /// <summary>
        /// 读取点检岗位模板数据
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IEnumerable<zxjc_djgw> FromExcel(string path)
        {
            try
            {
                List<zxjc_djgw> list = new List<zxjc_djgw>();
                ExcelHelper xls = new ExcelHelper();
                IWorkbook book = xls.ReadExcel(path);
                ISheet sheet = book.GetSheetAt(0);
                int rows = sheet.LastRowNum;
                for (int i = 1; i < rows; i++)
                {
                    IRow row = sheet.GetRow(i);
                    zxjc_djgw entity = new zxjc_djgw()
                    {
                        gcdm = "9100",
                        scx = row.GetCell(0).StringCellValue,
                        gwh = row.GetCell(1).StringCellValue,
                        jx_no= row.GetCell(2).StringCellValue,
                        status_no = row.GetCell(3).StringCellValue,
                        djno = GetDJNo(),
                        scbz = "N",
                        djxx = row.GetCell(5).StringCellValue,
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
}