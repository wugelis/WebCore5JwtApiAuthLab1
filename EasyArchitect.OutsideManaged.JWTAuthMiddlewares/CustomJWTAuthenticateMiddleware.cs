using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyArchitect.OutsideManaged.JWTAuthMiddlewares
{
    /// <summary>
    /// JWT 客製化 Middleware
    /// </summary>
    public static class CustomJWTAuthenticateMiddleware
    {
        /// <summary>
        /// 使用 JwtMiddleware 的驗證功能
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseJwtAuthenticate(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtMiddleware>();
        }
    }
}
