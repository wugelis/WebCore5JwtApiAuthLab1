using EasyArchitect.OutsideManaged.AuthExtensions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
        /// <summary>
        /// 驗證過濾器
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (User) context.HttpContext.Items["User"];
            if (user == null)
            {
                // 未登入時複寫 IActionResult
                context.Result = new JsonResult(new { message = "未授權存取此資源 (Unauthorized).\r\n請確認您具備外部人員的帳號與權限。" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
        }
    }
}
