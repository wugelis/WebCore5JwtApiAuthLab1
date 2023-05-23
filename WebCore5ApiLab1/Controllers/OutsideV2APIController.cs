﻿using EasyArchitect.OutsideApiControllerBase;
using EasyArchitect.OutsideManaged.AuthExtensions.Attributes;
using EasyArchitect.OutsideManaged.AuthExtensions.Filters;
using EasyArchitect.OutsideManaged.AuthExtensions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mxic.FrameworkCore.Core;
using OutsideManaged.ViewObjects;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using WebCore5ApiLab1.Configuration;

namespace WebCore5ApiLab1.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V2))]
    public class OutsideV2APIController : OutsideBaseApiController
    {
        private readonly ILogger<OutsideBaseApiController> _logger;
        private readonly IUserService _userService;
        private readonly IUriExtensions _uriExtensions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="userService"></param>
        /// <param name="uriExtensions"></param>
        public OutsideV2APIController(
            ILogger<OutsideBaseApiController> logger,
            IUserService userService,
            IUriExtensions uriExtensions)
            :base(logger, userService)
        {
            _logger = logger;
            _userService = userService;
            _uriExtensions = uriExtensions;
        }

        /// <summary>
        /// Get OutsideAccount 範例程式（需要驗證）
        /// </summary>
        /// <returns></returns>
        [NeedAuthorize]
        [HttpGet]
        [APIName("GetOutsideAccountView")]
        [ApiLogException]
        [ApiLogonInfoAttribute]
        public async Task<IEnumerable<OutsideAccountView>> GetOutsideAccountView()
        {
            return await Task.FromResult(new OutsideAccountView[]
            {
                new OutsideAccountView()
                {
                    UserAccount = "Gelis",
                    Title = "軟體架構師",
                    ContactName = "Gelis"
                }
            });
        }

        // 取得所有 User 的資料


    }
}