using EasyArchitect.OutsideManaged.AuthExtensions.Models;
using EasyArchitect.OutsideManaged.AuthExtensions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public OutsideBaseApiController(ILogger<OutsideBaseApiController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// 外部人員 獨立框架的（登入/Login）方法
        /// </summary>
        /// <param name="authenticateModel"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public IActionResult Login(AuthenticateRequest authenticateModel)
        {
            var response = _userService.Authenticate(authenticateModel);

            if (response == null)
            {
                return BadRequest(new { message = "使用的『帳號(Username)』 或 『密碼(password)』 是不正確的！請確認！." });
            }

            return Ok(response);
        }
    }
}
