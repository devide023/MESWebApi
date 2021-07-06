using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace MESWebApi.Util
{
    public class CheckLoginAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var attributes = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
            bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
            if (isAnonymous)
            {
                base.OnAuthorization(actionContext);
            }
            else
            {
                //从http请求的头里面获取身份验证信息，验证是否是请求发起方的token
                var authorization = actionContext.Request.Headers.Authorization;
                if ((authorization != null) && (authorization.Parameter != null))
                {
                    //校验Token合法及是否过期
                    var token = authorization.Parameter;
                    var isok = new JWTHelper().CheckToken(token);
                    if (isok)
                    {
                        //缓存用户信息
                        if (CacheManager.Instance().get(token) == null)
                        {
                            Services.UserService us = new Services.UserService();
                            Models.sys_user userentity = us.UserInfo(token);
                            if (userentity != null)
                            {
                                CacheManager.Instance().add(token, userentity);
                                base.IsAuthorized(actionContext);
                            }
                            else
                            {
                                string jsonResult = JsonConvert.SerializeObject(new
                                {
                                    code = 0,
                                    msg = "Token已失效,请重新登录"
                                });
                                HttpResponseMessage result = new HttpResponseMessage();
                                result.Content = new StringContent(jsonResult, System.Text.Encoding.GetEncoding("UTF-8"), "application/json");
                                actionContext.Response = result;
                                return;
                            }
                        }
                    }
                    else
                    {
                        //HandleUnauthorizedRequest(actionContext);
                        string jsonResult = JsonConvert.SerializeObject(new
                        {
                            code = 0,
                            msg = "Token已过期,请重新登录"
                        });
                        HttpResponseMessage result = new HttpResponseMessage();
                        result.Content = new StringContent(jsonResult, System.Text.Encoding.GetEncoding("UTF-8"), "application/json");
                        actionContext.Response = result;
                    }
                }
                //如果取不到身份验证信息，并且不允许匿名访问，则返回未验证401
                else
                {
                    //HandleUnauthorizedRequest(actionContext);
                    string jsonResult = JsonConvert.SerializeObject(new
                    {
                        code = 0,
                        msg = "验证失败,请重新登录"
                    });
                    HttpResponseMessage result = new HttpResponseMessage();
                    result.Content = new StringContent(jsonResult, System.Text.Encoding.GetEncoding("UTF-8"), "application/json");
                    actionContext.Response = result;
                }
            }
        }
    }
}