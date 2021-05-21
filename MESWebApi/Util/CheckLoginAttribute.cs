using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace MESWebApi.Util
{
    public class CheckLoginAttribute:AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
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
                        CacheManager.Instance().add(token, userentity);
                    }
                    base.IsAuthorized(actionContext);
                }
                else
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
            //如果取不到身份验证信息，并且不允许匿名访问，则返回未验证401
            else
            {
                var attributes = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
                bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
                if (isAnonymous) base.OnAuthorization(actionContext);
                else HandleUnauthorizedRequest(actionContext);
            }
        }
        //校验用户名密码
        private bool ValidateTicket(string encryptTicket)
        {
            //解密Ticket
            var strTicket = Tool.DESDecrypt(encryptTicket);

            //从Ticket里面获取用户名和密码
            var index = strTicket.IndexOf("##");
            string strUser = strTicket.Substring(0, index);
            string strPwd = strTicket.Substring(index + 2);

            if (strUser == "admin" && strPwd == "123456")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}