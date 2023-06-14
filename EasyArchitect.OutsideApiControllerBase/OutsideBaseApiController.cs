using EasyArchitect.OutsideManaged.AuthExtensions.Filters;
using EasyArchitect.OutsideManaged.AuthExtensions.Loggers;
using EasyArchitect.OutsideManaged.AuthExtensions.Models;
using EasyArchitect.OutsideManaged.AuthExtensions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mxic.FrameworkCore.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection.Metadata;
using System.Reflection;

namespace EasyArchitect.OutsideApiControllerBase
{
    /// <summary>
    /// 外部人員使用的獨立 API Controller 框架
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OutsideBaseApiController: ControllerBase
    {
        private readonly ILogger<OutsideBaseApiController> _logger;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _errorMessage = "使用的『帳號(Username)』 或 『密碼(password)』 是不正確的！請確認！.";

        public OutsideBaseApiController(
            ILogger<OutsideBaseApiController> logger, 
            IUserService userService, 
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 外部人員 獨立框架的（登入/Login）方法
        /// </summary>
        /// <param name="authenticateModel"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        [ApiLogonInfo]
        [ApiLogException]
        public IActionResult Login(AuthenticateRequest authenticateModel)
        {
            var response = _userService.Authenticate(authenticateModel);

            if (response == null)
            {
                ApiLogger.WriteErrorLog(_httpContextAccessor.HttpContext, _errorMessage);

                return BadRequest(new { message = _errorMessage });
            }

            return Ok(response);
        }
    }
}
