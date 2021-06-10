﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Configuration;
using System.Net.Http.Headers;

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
                    if( kv.TryGetValue("filetype", out pdftype))
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
                return Json(new { code = 1, msg = "上传成功", files = list,extdata= kv });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet,Route("ftpcnf")]
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
        [HttpPost,Route("ftpurl_encode")]
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
        [HttpGet]
        [AllowAnonymous]
        [Route("downpdf")]
        public HttpResponseMessage DownLoadPdf(string type,string filename)
        {
            try
            {
                //var strPath = @"D:\workspace\MESWebApi\MESWebApi\UpLoad\f2d5bfc1-b014-4241-ac59-7403cf93a625.pdf";
                //var stream = new FileStream(strPath, FileMode.Open);
                //HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                //response.Content = new StreamContent(stream);
                //response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                //response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                //{
                //    FileName = "test.pdf"
                //};

                //return response;
                string ftppath = ConfigurationManager.AppSettings["ftp"].ToString();
                string ftpusername = ConfigurationManager.AppSettings["ftpname"].ToString();
                string ftpuserpwd = ConfigurationManager.AppSettings["ftppwd"].ToString();
                string jstzfolder = ConfigurationManager.AppSettings["jtfolder"].ToString();
                string dzgyfolder = ConfigurationManager.AppSettings["dzgyfolder"].ToString();
                string folderpath = string.Empty;
                switch (type)
                {
                    case "jstz":
                        folderpath = jstzfolder;
                        break;
                    case "dzgy":
                        folderpath = dzgyfolder;
                        break;
                    default:
                        break;
                }
                Util.FtpHelper ftphelper = new Util.FtpHelper();
                var resmsg = ftphelper.DownloadFile(folderpath, filename, ftppath, ftpusername, ftpuserpwd);
                return resmsg;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}