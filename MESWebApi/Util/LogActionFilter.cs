using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MESWebApi.Util
{
    public class LogActionFilter:ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var paramss = actionContext.ActionArguments;
            Models.sys_user user = CacheManager.Instance().Current_User;
            if (paramss != null && paramss.Count > 0)
            {
                foreach (var item in paramss)
                {
                   string key = item.Key;
                   object par = item.Value;
                }
            }
                base.OnActionExecuting(actionContext);
        }
    }
}