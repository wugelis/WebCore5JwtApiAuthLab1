using EasyArchitect.OutsideManaged.AuthExtensions.Loggers;
using EasyArchitect.OutsideManaged.AuthExtensions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Mxic.FrameworkCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyArchitect.OutsideManaged.AuthExtensions.Filters
{
    /// <summary>
    /// 需要驗證是否具備外部人員身分（外部人員的驗證過濾器）
    /// </summary>
    public class NeedAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string errorMessage = "未授權存取此資源 (Unauthorized).\r\n請確認您具備外部人員的帳號與權限。";
        /// <summary>
        /// 驗證過濾器
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (User) context.HttpContext.Items["User"];
            if (user == null)
            {
                var actionName = context.ActionDescriptor.DisplayName;
                //var controllerName = context.HttpContext.Controller.GetType().FullName;

                // 获取 Action 上应用的 Attribute
                var endpointMetadata = context.ActionDescriptor.EndpointMetadata;
                var hasApiLogException = endpointMetadata.Any(x => x.GetType() == typeof(ApiLogExceptionAttribute));

                // 如果存在 ApiLogonInfo，則紀錄 Log 資訊，否则忽略
                if (hasApiLogException)
                {
                    IUriExtensions uriExtensions = context.HttpContext.RequestServices.GetRequiredService<IUriExtensions>();
                    string absoluteUri = context.HttpContext.Request.GetAbsoluteUri(uriExtensions);

                    // 紀錄錯誤的 Error Log Message
                    ApiLogger.WriteErrorLog(
                        context,
                        absoluteUri,
                        absoluteUri,
                        context.HttpContext.Request.Method,
                        context.HttpContext.Request.Path,
                        errorMessage,
                        "");
                }


                // 未登入時複寫 IActionResult
                context.Result = new JsonResult(new { message = errorMessage })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
        }
    }
}
