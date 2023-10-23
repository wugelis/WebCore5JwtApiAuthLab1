using EasyArchitect.OutsideManaged.AuthExtensions.Models;
using EasyArchitectCore.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EasyArchitect.OutsideControllerBase
{
    /// <summary>
    /// 需要登入的 Action Filter
    /// </summary>
    public class NeedLoginAttribute: ActionFilterAttribute
    {
        /// <summary>
        /// 攔截 Action 執行前進行權限檢核的處理
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var cookieString = context.HttpContext.Request.Cookies[User.LOGIN_USER_INFO];

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                base.OnActionExecuting(context);
            }
            else
            {
                var loginUrl = string.Concat(
                        context.HttpContext.Request.Scheme,
                        "://",
                        context.HttpContext.Request.Host.ToUriComponent(),
                        context.HttpContext.Request.PathBase.ToUriComponent(),
                        ConfigurationManager.AppSettings["LoginPage"]);

                context.Result = new RedirectResult(loginUrl);

#pragma warning disable S3626 // Jump statements should not be redundant
                return;
#pragma warning restore S3626 // Jump statements should not be redundant
            }
        }
    }
}