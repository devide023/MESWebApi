using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using log4net;
using Newtonsoft.Json;
namespace MESWebApi.Util
{
    public class ApiExceptionAttribute : ExceptionFilterAttribute
    {
        private ILog log;
        public ApiExceptionAttribute()
        {
            log = LogManager.GetLogger(this.GetType());
        }
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            string ip = Util.Tool.GetIP();
            string brow = Util.Tool.GetBrowser();
            string os = Util.Tool.GetOSVersion();
            var method = actionExecutedContext.Request.Method.Method;
            var ctrl = actionExecutedContext.ActionContext.ControllerContext.Controller.ToString();
            log.Error(actionExecutedContext.Exception.Message);
            string jsonResult = JsonConvert.SerializeObject(new
            {
                code = 0,
                browser = brow,
                os = os,
                ip=ip,
                action = method,
                controller = ctrl,
                msg = actionExecutedContext.Exception.Message
            });
            HttpResponseMessage result = new HttpResponseMessage();
            result.Content = new StringContent(jsonResult, System.Text.Encoding.GetEncoding("UTF-8"), "application/json");
            actionExecutedContext.Response = result;
            base.OnException(actionExecutedContext);
        }
    }
}