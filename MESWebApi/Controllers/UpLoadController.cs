using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MESWebApi.Controllers
{
    [RoutePrefix("api/upload")]
    public class UpLoadController : ApiController
    {
        [Route("files")]
        [HttpPost]
        public IHttpActionResult uploadfiles()
        {
            try
            {
                HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                string savepath = HttpContext.Current.Server.MapPath("~/UpLoad");
                List<dynamic> list = new List<dynamic>();
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[i];
                    string client_filename = file.FileName;
                    int fileszie = file.ContentLength;

                    int pos = client_filename.LastIndexOf(".");
                    string filetype = client_filename.Substring(pos, client_filename.Length - pos);
                    string guid = Guid.NewGuid().ToString() + filetype;
                    string fullfilename = savepath + "\\" + guid;
                    file.SaveAs(fullfilename);
                    list.Add(new { fileid = guid, filename = client_filename, filesize = fileszie });
                }
                return Json(new { code = 1, msg = "上传成功", files = list });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("pdf")]
        [HttpPost]
        public IHttpActionResult extuploadfiles()
        {
            try
            {
                Dictionary<string, object> kv = new Dictionary<string, object>();
                var extdata = HttpContext.Current.Request.Form;
                if (extdata != null)
                {
                    foreach (var item in extdata.AllKeys)
                    {
                        kv.Add(item, extdata.Get(item));
                    }
                }
                HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                string savepath = HttpContext.Current.Server.MapPath("~/UpLoad");
                List<dynamic> list = new List<dynamic>();
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[i];
                    string client_filename = file.FileName;
                    int fileszie = file.ContentLength;
                    int pos = client_filename.LastIndexOf(".");
                    string filetype = client_filename.Substring(pos, client_filename.Length - pos);
                    string guid = Guid.NewGuid().ToString() + filetype;
                    string fullfilename = savepath + "\\" + guid;
                    file.SaveAs(fullfilename);
                    list.Add(new { fileid = guid, filename = client_filename, filesize = fileszie });
                }
                return Json(new { code = 1, msg = "上传成功", files = list,extdata= kv });
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}