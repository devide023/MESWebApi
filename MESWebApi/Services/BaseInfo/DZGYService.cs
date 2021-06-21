using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MESWebApi.InterFaces;
using MESWebApi.DB;
using Dapper;
using Dapper.Oracle;
using DapperExtensions;
using MESWebApi.Models.BaseInfo;
using MESWebApi.Models;
using System.Text;
using log4net;
using Webdiyer.WebControls.Mvc;
using MESWebApi.Util;
using NPOI.SS.UserModel;

namespace MESWebApi.Services.BaseInfo
{
    /// <summary>
    /// 电子工艺服务
    /// </summary>
    public class DZGYService : DBOperImp<zxjc_t_dzgy>, IComposeQuery<zxjc_t_dzgy, sys_page>
    {
        private ILog log;
        public DZGYService(string constr="tjmes"):base(constr)
        {
            log = LogManager.GetLogger(this.GetType());
        }
        public IEnumerable<zxjc_t_dzgy> Search(sys_page parm, out int resultcount)
        {
            try
            {
                OracleDynamicParameters p = new OracleDynamicParameters();
                StringBuilder sql = new StringBuilder();
                sql.Append(" select ta.gyid,");
                sql.Append(" ta.gybh,");
                sql.Append(" ta.gymc,");
                sql.Append(" ta.gyms,");
                sql.Append(" ta.gcdm,");
                sql.Append(" ta.scx,");
                sql.Append(" ta.gwh,");
                sql.Append(" (select work_name from zxjc_gxzd where work_no = ta.gwh ) as gwmc,");
                sql.Append(" ta.jx_no,");
                sql.Append(" ta.status_no,");
                sql.Append(" ta.wjlj,");
                sql.Append(" ta.jwdx,");
                sql.Append(" ta.scry,");
                sql.Append(" ta.scpc,");
                sql.Append(" ta.scsj,");
                sql.Append(" ta.bbbh");
                sql.Append(" from ZXJC_T_DZGY ta where 1=1 ");
                if (!string.IsNullOrEmpty(parm.keyword))
                {
                    sql.Append(" and (ta.gybh like :key or ta.gymc like :key or ta.gyms like :key or ta.jx_no like :key ) ");
                    p.Add(":key", "%" + parm.keyword + "%", OracleMappingType.NVarchar2, System.Data.ParameterDirection.Input);
                }
                if (parm.explist.Count > 0) {
                    sql.Append(" and ");
                    sql.Append(Tool.ComQueryExp(parm.explist));
                }
                var q = Conn.Query<zxjc_t_dzgy>(sql.ToString(), p)
                    .OrderByDescending(t=>t.gybh)
                    .ToPagedList(parm.pageindex, parm.pagesize);
                resultcount = q.TotalItemCount;
                return q;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<zxjc_t_dzgy> FromExcel(string path) {
            try
            {
                List<zxjc_t_dzgy> list = new List<zxjc_t_dzgy>();
                ExcelHelper xls = new ExcelHelper();
                IWorkbook book = xls.ReadExcel(path);
                ISheet sheet = book.GetSheetAt(0);
                int rows = sheet.LastRowNum;
                for (int i = 1; i < rows; i++)
                {
                    IRow row = sheet.GetRow(i);
                    string dzgy_number = this.GetDZGYNumber();
                    zxjc_t_dzgy entity = new zxjc_t_dzgy()
                    {
                        gyid = Guid.NewGuid().ToString(),
                        gcdm = "9100",
                        scx = row.GetCell(3).StringCellValue,
                        gwh = row.GetCell(4).StringCellValue,
                        gymc = row.GetCell(1).StringCellValue,
                        gyms = row.GetCell(2).StringCellValue,
                        gybh = dzgy_number,
                        jx_no = row.GetCell(5).StringCellValue,
                        status_no = row.GetCell(6).StringCellValue,
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

        public string GetDzgyId()
        {
            try
            {
                return Guid.NewGuid().ToString();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public string GetDZGYNumber()
        {
            try
            {
                int gybhid = Conn.ExecuteScalar<int>("SELECT seq_dzgy_no.nextval FROM dual");
                string dzgy_number = "GY-" + gybhid.ToString().PadLeft(8, '0');
                return dzgy_number;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }

        public int ModifyFileName(zxjc_t_dzgy entity)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update ZXJC_T_DZGY");
                sql.Append(" set ");
                sql.Append("        wjlj = :wjlj,");
                sql.Append("        jwdx = :jwdx,scry=:scry,scpc=:scpc,scsj=sysdate");
                sql.Append(" where  gyid = :gyid");
                return Conn.Execute(sql.ToString(), entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public override int Modify(zxjc_t_dzgy entity)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" update ZXJC_T_DZGY");
                sql.Append(" set gymc = :gymc,");
                sql.Append("        gyms = :gyms,");
                sql.Append("        gcdm = :gcdm,");
                sql.Append("        scx = :scx,");
                sql.Append("        gwh = :gwh,");
                sql.Append("        jx_no = :jx_no,");
                sql.Append("        status_no = :status_no,");
                sql.Append("        wjlj = :wjlj,");
                sql.Append("        jwdx = :jwdx,scry=:scry,scpc=:scpc,scsj=sysdate");
                sql.Append(" where  gyid = :gyid");
                return Conn.Execute(sql.ToString(), entity);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}