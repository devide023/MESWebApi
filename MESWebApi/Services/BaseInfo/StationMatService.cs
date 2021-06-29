using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MESWebApi.Models;
using MESWebApi.Models.BaseInfo;
using MESWebApi.DB;
using Dapper;
using Dapper.Oracle;
using DapperExtensions;
using MESWebApi.InterFaces;
using log4net;
using MESWebApi.Util;
using System.Text;
using Webdiyer.WebControls.Mvc;
using NPOI.SS.UserModel;

namespace MESWebApi.Services.BaseInfo
{
    /// <summary>
    /// 岗位物料
    /// </summary>
    public class StationMatService : DBOperImp<base_gwbj1>, IComposeQuery<base_gwbj1, sys_page>
    {
        ILog log;
        public StationMatService(string constr = "tjmes") : base(constr)
        {
            log = LogManager.GetLogger(this.GetType());
        }

        public override int Modify(base_gwbj1 entity)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update base_gwbj ");
                sql.Append(" set gzzx = :gzzx, ");
                sql.Append("        gwh = :gwh, ");
                sql.Append("        wlbm = :wlbm, ");
                sql.Append("        lxpd = :lxpd, ");
                sql.Append("        sfdy = :sfdy, ");
                sql.Append("        lxlx = :lxlx, ");
                sql.Append("        dxsl = :dxsl, ");
                sql.Append("        wlsx = :wlsx, ");
                sql.Append("        bz = :bz, ");
                sql.Append("        scx = :scx, ");
                sql.Append("        gwpb = :gwpb, ");
                sql.Append("        qwwbm = :qwwbm, ");
                sql.Append("        zcqld = :zcqld, ");
                sql.Append("        psfs = :psfs, ");
                sql.Append("        zgkc = :zgkc, ");
                sql.Append("        jx_no = :jx_no ");
                sql.Append(" where  gcdm = '9100' ");
                sql.Append(" and scx = :scx ");
                sql.Append(" and    gwh = :gwh ");
                sql.Append(" and    jx_no = :jx_no ");
                sql.Append(" and    wlbm = :wlbm ");
                return this.Conn.Execute(sql.ToString(),entity);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public int Delete(List<base_gwbj1> entitys)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" delete from base_gwbj ");
                sql.Append(" where  gcdm = :gcdm ");
                sql.Append(" and scx = :scx ");
                sql.Append(" and    gwh = :gwh ");
                sql.Append(" and    jx_no = :jx_no ");
                sql.Append(" and    wlbm = :wlbm ");
                return Conn.Execute(sql.ToString(), entitys.ToArray());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<base_gwbj1> Search(sys_page parm, out int resultcount)
        {
            try
            {
                OracleDynamicParameters p = new OracleDynamicParameters();
                StringBuilder sql = new StringBuilder();
                sql.Append("select ta.gcdm, ta.scx, ta.gwh,(select work_name from zxjc_gxzd where work_no = ta.gwh) as gwmc, ta.wlbm,(SELECT wljc FROM base_wlxx where wlbm = ta.wlbm) as wlmc,ta.lxpd, ta.lxlx, ta.dxsl, ta.gwpb, ta.qwwbm, ta.wlsx, ta.zcqld,");
                sql.Append(" ta.psfs, ta.gdbh, ta.gdc, ta.zgkc, ta.sfdy, ta.bz, ta.lrr, ta.lrsj, ta.jx_no, ta.gzzx");
                sql.Append(" from base_gwbj1 ta where 1=1 ");
                if (!string.IsNullOrEmpty(parm.keyword))
                {
                    sql.Append(" and (gwh like :key or wlbm like :key or lower(jx_no) like :key) ");
                    p.Add(":key", "%" + parm.keyword.ToLower() + "%", OracleMappingType.NVarchar2, System.Data.ParameterDirection.Input);
                }
                if (parm.explist.Count > 0)
                {
                    sql.Append(" and ");
                    sql.Append(Tool.ComQueryExp(parm.explist));
                }
                var q = this.Conn.Query<base_gwbj1>(sql.ToString(), p)
                    .OrderBy(t => t.wlbm)
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

        public IEnumerable<base_gwbj1> FromExcel(string path)
        {
            try
            {
                List<base_gwbj1> list = new List<base_gwbj1>();
                ExcelHelper xls = new ExcelHelper();
                IWorkbook book = xls.ReadExcel(path);
                ISheet sheet = book.GetSheetAt(0);
                int rows = sheet.LastRowNum;
                for (int i = 1; i <= rows; i++)
                {
                    IRow row = sheet.GetRow(i);
                    int dxsl = 0;
                    int.TryParse(row.GetCell(11).StringCellValue, out dxsl);
                    base_gwbj1 entity = new base_gwbj1()
                    {
                        gcdm = "9100",
                        scx = row.GetCell(0).StringCellValue,
                        gwh = row.GetCell(1).StringCellValue,
                        gwmc = row.GetCell(2).StringCellValue,
                        jx_no = row.GetCell(3).StringCellValue,
                        wlbm = row.GetCell(4).StringCellValue,
                        wlmc = row.GetCell(5).StringCellValue,
                        qwwbm = "",
                        wlsx = row.GetCell(7).StringCellValue,
                        lxpd = row.GetCell(8).StringCellValue,
                        sfdy = "Y",
                        lxlx = row.GetCell(9).StringCellValue,
                        dxsl = dxsl,
                        bz  = row.GetCell(14).StringCellValue,
                        gzzx= ""
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