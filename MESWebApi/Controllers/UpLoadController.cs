using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Configuration;
using System.Net.Http.Headers;
using System.Text;

namespace MESWebApi.Controllers
{
    /// <summary>
    /// 文档上传接口
    /// </summary>
    [RoutePrefix("api/upload")]
    public class UpLoadController : ApiController
    {
        /// <summary>
        /// 模板上传,主要是excel模板
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// pdf文件上传，主要是电子工艺，技术通知
        /// 上传到ftp服务器
        /// </summary>
        /// <returns></returns>
        [Route("pdf")]
        [HttpPost]
        public IHttpActionResult extuploadfiles()
        {
            try
            {
                string ftppath = ConfigurationManager.AppSettings["ftp"].ToString();
                string ftpusername = ConfigurationManager.AppSettings["ftpname"].ToString();
                string ftpuserpwd = ConfigurationManager.AppSettings["ftppwd"].ToString();
                string jstzfolder = ConfigurationManager.AppSettings["jtfolder"].ToString();
                string dzgyfolder = ConfigurationManager.AppSettings["dzgyfolder"].ToString();
                Dictionary<string, object> kv = new Dictionary<string, object>();
                //文件类型，区分是电子通知还是技术通知
                object pdftype = string.Empty;
                string save_pdfpath = string.Empty;
                var extdata = HttpContext.Current.Request.Form;
                if (extdata != null)
                {
                    foreach (var item in extdata.AllKeys)
                    {
                        kv.Add(item, extdata.Get(item));
                    }
                    if (kv.TryGetValue("filetype", out pdftype))
                    {
                        var ft = pdftype.ToString();
                        switch (ft)
                        {
                            case "jstz":
                                save_pdfpath = ftppath + "/" + jstzfolder;
                                break;
                            case "dzgy":
                                save_pdfpath = ftppath + "/" + dzgyfolder;
                                break;
                            default:
                                break;
                        }

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
                    Util.FtpHelper ftphelper = new Util.FtpHelper();
                    ftphelper.UploadFile(file.InputStream, save_pdfpath, client_filename, ftpusername, ftpuserpwd);
                    int pos = client_filename.LastIndexOf(".");
                    string filetype = client_filename.Substring(pos, client_filename.Length - pos);
                    string guid = Guid.NewGuid().ToString() + filetype;
                    list.Add(new { fileid = guid, filename = client_filename, filesize = fileszie });
                }
                return Json(new { code = 1, msg = "上传成功", files = list, extdata = kv });
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// ftp配置
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("ftpcnf")]
        public IHttpActionResult GetFtpCnf()
        {
            try
            {
                string ftppath = ConfigurationManager.AppSettings["ftp"].ToString();
                string jstzfolder = ConfigurationManager.AppSettings["jtfolder"].ToString();
                string dzgyfolder = ConfigurationManager.AppSettings["dzgyfolder"].ToString();
                return Json(new { code = 1, msg = "ok", domain = ftppath, jstzfolder = jstzfolder, dzgyfolder = dzgyfolder });
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 获取ftp,url地址编码
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost, Route("ftpurl_encode")]
        public IHttpActionResult Encode_FtpUrl(dynamic obj)
        {
            try
            {
                string filename = (obj.filename ?? "").ToString();
                string ftppath = ConfigurationManager.AppSettings["ftp"].ToString();
                string jstzfolder = ConfigurationManager.AppSettings["jtfolder"].ToString();
                string dzgyfolder = ConfigurationManager.AppSettings["dzgyfolder"].ToString();
                string filename_rul = HttpUtility.UrlEncode(filename, System.Text.Encoding.GetEncoding("GB2312"));
                string jstz_url = HttpUtility.UrlEncode(jstzfolder, System.Text.Encoding.GetEncoding("GB2312"));
                string dzgy_url = HttpUtility.UrlEncode(dzgyfolder, System.Text.Encoding.GetEncoding("GB2312"));
                string jturl = $"ftp://{ftppath}/{jstz_url}/{filename_rul}";
                string dzgyurl = $"ftp://{ftppath}/{dzgy_url}/{filename_rul}";
                return Json(new { code = 1, msg = "ok", jstzurl = jturl, dzgyurl = dzgyurl });
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 从ftp下载文件暂存到web服务器
        /// </summary>
        /// <param name="type">技术通知：jstz,电子工艺：dzgy</param>
        /// <param name="filename">文件名</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous,Route("downfromftp")]
        public IHttpActionResult DownLoadFilefromFtp(string type, string filename)
        {
            try
            {
                if(string.IsNullOrEmpty(type) || string.IsNullOrEmpty(filename))
                {
                    return Json(new { code = 0, msg = "文件类型或文件名为空" });
                }
                string ftppath = ConfigurationManager.AppSettings["ftp"].ToString();
                string jstzfolder = ConfigurationManager.AppSettings["jtfolder"].ToString();
                string dzgyfolder = ConfigurationManager.AppSettings["dzgyfolder"].ToString();
                string folder = string.Empty;
                switch (type)
                {
                    case "jstz":
                        folder = jstzfolder;
                        break;
                    case "dzgy":
                        folder = dzgyfolder;
                        break;
                    default:
                        break;
                }
                string path = $"ftp://{ftppath}/{folder}/{filename}";
                string downpath = HttpContext.Current.Server.MapPath("~/DownLoad/");
                string localfile = downpath + filename;
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));
                using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (FileStream fs = new FileStream(localfile, FileMode.OpenOrCreate))
                        {
                            try
                            {
                                byte[] buffer = new byte[20480];
                                int read = 0;
                                do
                                {
                                    read = responseStream.Read(buffer, 0, buffer.Length);
                                    fs.Write(buffer, 0, read);
                                } while (!(read == 0));
                                responseStream.Close();
                                fs.Flush();
                                fs.Close();
                            }
                            catch (Exception)
                            {
                                fs.Close();
                                File.Delete(localfile);
                                throw;
                            }
                        }
                        responseStream.Close();
                    }
                    response.Close();
                }
                return Json(new { code = 1, msg = "文件从ftp下载成功", downloadid = filename });
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 从web服务器下载文件到浏览器
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous,Route("downpdf")]
        public HttpResponseMessage DownLoadPdf(string filename)
        {
            try
            {
                var strPath = HttpContext.Current.Server.MapPath("~/DownLoad/" + filename);
                var stream = new FileStream(strPath, FileMode.Open);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = filename
                };
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}