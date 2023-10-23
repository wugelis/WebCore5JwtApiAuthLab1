using EasyArchitect.OutsideManaged.AuthExtensions.Models;
using EasyArchitectCore.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasyArchitect.OutsideControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    //[Authorize]
    public class OutsideBaseController: Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OutsideBaseController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 處理登入與產生 Token 作業（當外部使用者存在時）
        /// </summary>
        /// <param name="account"></param>
        protected virtual bool ProcessLogin(AuthenticateRequest account)
        {
            bool result = true;

            ClaimsPrincipal principal = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[]
                {
                        new Claim(ClaimTypes.Name, account.Username),
                        new Claim(ClaimTypes.Role, "Admin")
                },
                CookieAuthenticationDefaults.AuthenticationScheme
            ));

            try
            {
                _httpContextAccessor.HttpContext.SignInAsync(principal);

                CookieOptions cookieOptions = new CookieOptions()
                {
                    Expires = DateTime.Now.AddMinutes(double.Parse(ConfigurationManager.AppSettings["TimeoutMinutes"])),
                    HttpOnly = true
                };

                _httpContextAccessor.HttpContext.Response.Cookies.Append(UserInfo.LOGIN_USER_INFO, JsonSerializer.Serialize(account));
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 進行登出作業
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public virtual async Task<IActionResult> LogoutProcess()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync();

            _httpContextAccessor.HttpContext.Response.Cookies.Delete(UserInfo.LOGIN_USER_INFO);

            return View("Login");
        }
    }
}
