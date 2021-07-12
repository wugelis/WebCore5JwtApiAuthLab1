using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCore5ApiLab1.Dto;

namespace WebCore5ApiLab1.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JwtAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (User)context.HttpContext.Items["User"];
            if (user == null)
            {
                // 未登入時複寫 IActionResult
                context.Result = new JsonResult(new { message = "未授權存取此資源 (Unauthorized)." }) 
                { 
                    StatusCode = StatusCodes.Status401Unauthorized 
                };
            }
        }
    }
}
