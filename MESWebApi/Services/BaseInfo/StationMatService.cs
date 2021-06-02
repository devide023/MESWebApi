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
    public class StationMatService : DBOperImp<base_gwbj>, IComposeQuery<base_gwbj, sys_page>
    {
        ILog log;
        public StationMatService(string constr = "tjmes") : base(constr)
        {
            log = LogManager.GetLogger(this.GetType());
        }

        public override int Modify(base_gwbj entity)
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

        public IEnumerable<base_gwbj> Search(sys_page parm, out int resultcount)
        {
            try
            {
                OracleDynamicParameters p = new OracleDynamicParameters();
                StringBuilder sql = new StringBuilder();
                sql.Append("select ta.gcdm, ta.scx, ta.gwh,(select work_name from zxjc_gxzd where work_no = ta.gwh) as gwmc, ta.wlbm,(SELECT wljc FROM base_wlxx where wlbm = ta.wlbm) as wlmc,ta.lxpd, ta.lxlx, ta.dxsl, ta.gwpb, ta.qwwbm, ta.wlsx, ta.zcqld,");
                sql.Append(" ta.psfs, ta.gdbh, ta.gdc, ta.zgkc, ta.sfdy, ta.bz, ta.lrr, ta.lrsj, ta.jx_no, ta.gzzx");
                sql.Append(" from base_gwbj ta where 1=1 ");
                if (parm.explist.Count > 0)
                {
                    sql.Append(Tool.ComQueryExp(parm.explist));
                }
                var q = this.Conn.Query<base_gwbj>(sql.ToString(), p)
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

        public IEnumerable<base_gwbj> FromExcel(string path)
        {
            try
            {
                List<base_gwbj> list = new List<base_gwbj>();
                ExcelHelper xls = new ExcelHelper();
                IWorkbook book = xls.ReadExcel(path);
                ISheet sheet = book.GetSheetAt(0);
                int rows = sheet.LastRowNum;
                for (int i = 1; i < rows; i++)
                {
                    IRow row = sheet.GetRow(i);
                    int dxsl = 0;
                    int.TryParse(row.GetCell(13).StringCellValue, out dxsl);
                    base_gwbj entity = new base_gwbj()
                    {
                        gcdm = "9100",
                        scx = row.GetCell(1).StringCellValue,
                        gwh = row.GetCell(3).StringCellValue,
                        gwmc = row.GetCell(4).StringCellValue,
                        jx_no = row.GetCell(5).StringCellValue,
                        wlbm = row.GetCell(6).StringCellValue,
                        wlmc = row.GetCell(7).StringCellValue,
                        qwwbm = row.GetCell(8).StringCellValue,
                        wlsx = row.GetCell(9).StringCellValue,
                        lxpd = "N",
                        sfdy = "Y",
                        lxlx = row.GetCell(11).StringCellValue,
                        dxsl = dxsl,
                        bz  = row.GetCell(16).StringCellValue,
                        gzzx= row.GetCell(1).StringCellValue
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