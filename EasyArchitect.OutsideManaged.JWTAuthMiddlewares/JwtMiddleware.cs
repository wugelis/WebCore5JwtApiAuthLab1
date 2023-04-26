using EasyArchitect.OutsideManaged.AuthExtensions.Services;
using EasyArchitect.OutsideManaged.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyArchitect.OutsideManaged.JWTAuthMiddlewares
{
    /// <summary>
    /// JWT 的 Middleware 中間層物件
    /// </summary>
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Pipelines 的觸發方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userService"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                attachUserToContext(context, userService, token);

            await _next(context);
        }

        /// <summary>
        /// 驗證 JWT Token 是否為合法的 Token，如為合法就附掛權限上去
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userService"></param>
        /// <param name="token"></param>
        private void attachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // 將 clockskew 設為零，以便 Token 到期時間是準時到期（而不是有 5 分鐘的誤差）
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                string userId = jwtToken.Claims.First(x => x.Type == "Username").Value;

                // 將原先用來產生 Claim 的欄位從 UserServices 裡撈出來，並塞入目前的工作階段身分物件裡
                context.Items["User"] = userService.GetByUsername(userId);
                //System.Security.Claims.ClaimsIdentity c = new System.Security.Claims.ClaimsIdentity(userId);
                //System.Security.Claims.ClaimsPrincipal p = new System.Security.Claims.ClaimsPrincipal(jwtToken.);
                userService.IdentityUser = userId;
            }
            catch
            {
                // 當 Validate JWT Token 失敗時不做任何事情，目的是直接讓驗證失敗
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
