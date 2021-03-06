using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MESWebApi.Services.BaseInfo;
using MESWebApi.Models;
using MESWebApi.Models.BaseInfo;
using MESWebApi.Models.QueryParm;
using System.Web;
using System.Text;
using MESWebApi.Util;

namespace MESWebApi.Controllers.BaseInfo
{
    /// <summary>
    /// 技术通知,特殊技术通知接口
    /// </summary>
    /// 
    [RoutePrefix("api/baseinfo/notice")]
    public class TecNoticeController : ApiController
    {
        /// <summary>
        /// 技术通知列表（zxjc_t_jstc）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost, Route("list")]
        public IHttpActionResult JTList(sys_page parm)
        {
            try
            {
                int resultcount = 0;
                JTService jts = new JTService();
                var list = jts.Search(parm, out resultcount);
                return Json(new { code = 1, msg = "ok", list = list, resultcount = resultcount });
            }
            catch (Exception)
            {
                throw;
            }

        }
        /// <summary>
        /// 新增技通（zxjc_t_jstc）
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        [HttpPost, Route("add")]
        public IHttpActionResult JTAdd(List<zxjc_t_jstc> entitys)
        {
            try
            {
                JTService jts = new JTService();
                int ret = jts.Add(entitys);
                if (ret > 0)
                {
                    return Json(new { code = 1, msg = "数据保存成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "数据保存失败" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 删除（zxjc_t_jstc）
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        [HttpPost, Route("del")]
        public IHttpActionResult JTDel(List<zxjc_t_jstc> entitys) {
            try
            {
                JTService jts = new JTService();
                int ret = jts.Delete(entitys);
                if (ret > 0)
                {
                    return Json(new { code = 1, msg = "数据删除成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "数据删除失败" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 批量编辑（zxjc_t_jstc）
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        [HttpPost, Route("batedit")]
        public IHttpActionResult JTBatEdit(List<zxjc_t_jstc> entitys)
        {
            try
            {
                JTService jts = new JTService();
                int ret = jts.Modify(entitys);
                if (ret > 0)
                {
                    return Json(new { code = 1, msg = "数据修改成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "数据修改失败" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 编辑（zxjc_t_jstc）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost, Route("edit")]
        public IHttpActionResult JTEdit(zxjc_t_jstc entity)
        {
            try
            {
                JTService jts = new JTService();
                int ret = jts.Modify(entity);
                if (ret > 0)
                {
                    return Json(new { code = 1, msg = "数据修改成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "数据修改失败" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 更新文件名称及大小（zxjc_t_jstc）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost, Route("changefiles")]
        public IHttpActionResult ModifyFileName(dynamic obj)
        {
            try
            {
                string jtid = (obj.jtid ?? "").ToString();
                List<sys_file> files = obj.files != null ? obj.files.ToObject<List<sys_file>>() : new List<sys_file>();
                string filenames = string.Empty;
                files.ForEach(t => filenames = filenames + t.filename + ",");
                if (filenames.Length > 0)
                {
                    filenames = filenames.Remove(filenames.Length - 1, 1);
                }
                sys_user user = CacheManager.Instance().Current_User;
                JTService jts = new JTService();
                zxjc_t_jstc entity = new zxjc_t_jstc()
                {
                    gcdm="9100",
                    jtid = jtid,
                    wjlj = filenames,
                    jwdx = files.Sum(t => t.filesize),
                    scry = user.name,
                    scsj = DateTime.Now
                };
                int ret = jts.ModifyFileNames(entity);
                if (ret > 0)
                {
                    return Json(new { code = 1, msg = "文件保存成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "文件保存失败" });
                }                
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 获取技通编号（zxjc_t_jstc）
        /// </summary>
        /// <returns></returns>
        [HttpGet,Route("jtno")]
        public IHttpActionResult GetJtNumber()
        {
            try
            {
                JTService jts = new JTService();
                string jtno = jts.GetJTNumber();
                return Json(new { code = 1, msg = "ok",jtno = jtno,jtid=Guid.NewGuid().ToString() });
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 读取技通文件模板数据（zxjc_t_jstc）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost,Route("readxls")]
        public IHttpActionResult ReadNoticeXls(dynamic obj)
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("~/UpLoad/");
                string filename = (obj.filename ?? "").ToString();
                if (!string.IsNullOrEmpty(filename))
                {
                    string fullpath = path + filename;
                    JTService jts = new JTService();
                    var list = jts.FromExcel(fullpath);
                    return Json(new { code = 1, msg = "ok", list = list });
                }
                else
                {
                    return Json(new { code = 0, msg = "error" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 特殊技术通知列表（zxjc_t_tstc）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost, Route("tsjt/list")]
        public IHttpActionResult TsjtList(sys_page parm)
        {
            try
            {
                int resultcount = 0;
                TsJTService tsjts = new TsJTService();
                var list = tsjts.Search(parm, out resultcount);
                return Json(new { code = 1, msg = "ok", list = list, resultcount = resultcount });
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 获取特殊技通编号（zxjc_t_tstc）
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("tsjt/tsjtno")]
        public IHttpActionResult TsjtNo()
        {
            try
            {
                TsJTService tsjts = new TsJTService();
                string no = tsjts.SpecialNoticeNo();
                string tcid = Guid.NewGuid().ToString();
                if (!string.IsNullOrEmpty(no))
                {
                    return Json(new { code = 1, msg = "ok", no = no, tcid=tcid }) ;
                }
                else
                {
                    return Json(new { code = 0, msg = "编号获取失败", no = no,tcid = tcid });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 新增特殊技通（zxjc_t_tstc）
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        [HttpPost, Route("tsjt/add")]
        public IHttpActionResult TsjtAdd(List<zxjc_t_tstc> entitys)
        {
            try
            {
                sys_user user = CacheManager.Instance().Current_User;
                entitys.ForEach(t => { t.lrr = user.name; t.lrsj = DateTime.Now; });
                TsJTService tsjts = new TsJTService();
                int ret = tsjts.Add(entitys);
                if (ret > 0)
                {
                    return Json(new { code = 1, msg = "数据保存成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "数据保存失败" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 特殊技通删除（zxjc_t_tstc）
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        [HttpPost, Route("tsjt/del")]
        public IHttpActionResult TsjtDel(List<zxjc_t_tstc> entitys)
        {
            try
            {
                TsJTService tsjts = new TsJTService();
                int ret = tsjts.Delete(entitys);
                if (ret > 0)
                {
                    return Json(new { code = 1, msg = "数据删除成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "数据删除失败" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 特殊技通编辑（zxjc_t_tstc）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost, Route("tsjt/edit")]
        public IHttpActionResult TsjtEdit(zxjc_t_tstc entity)
        {
            try
            {
                TsJTService tsjts = new TsJTService();
                int ret = tsjts.Modify(entity);
                if (ret > 0)
                {
                    return Json(new { code = 1, msg = "数据修改成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "数据修改失败" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 特殊技通批量编辑（zxjc_t_tstc）
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        [HttpPost, Route("tsjt/batedit")]
        public IHttpActionResult TsjtBatEdit(List<zxjc_t_tstc> entitys)
        {
            try
            {
                TsJTService tsjts = new TsJTService();
                int ret = tsjts.Modify(entitys);
                if (ret > 0)
                {
                    return Json(new { code = 1, msg = "数据修改成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "数据修改失败" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 技通阅读记录（zxjc_t_ydjl）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost, Route("jtyd/list")]
        public IHttpActionResult JtydList(sys_page parm)
        {
            try
            {
                int resultcount = 0;
                JTYDService jtyds = new JTYDService();
                var list = jtyds.Search(parm,out resultcount);
                return Json(new { code = 1, msg = "ok", list = list, resultcount = resultcount });
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 新增技通阅读记录（zxjc_t_ydjl）
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        [HttpPost, Route("jtyd/add")]
        public IHttpActionResult JtydAdd(List<zxjc_t_ydjl> entitys)
        {
            try
            {
                JTYDService jtyds = new JTYDService();
                int ret = jtyds.Add(entitys);
                if (ret > 0)
                {
                    return Json(new { code = 1, msg = "数据保存成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "数据保存失败" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}