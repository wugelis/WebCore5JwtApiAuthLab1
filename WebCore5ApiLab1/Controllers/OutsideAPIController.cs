using EasyArchitect.OutsideApiControllerBase;
using EasyArchitect.OutsideManaged.AuthExtensions.Attributes;
using EasyArchitect.OutsideManaged.AuthExtensions.Filters;
using EasyArchitect.OutsideManaged.AuthExtensions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mxic.FrameworkCore.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebCore5ApiLab1.Controllers
{
    /// <summary>
    /// 外部人員 ApiController 程式碼範本
    /// </summary>
    public class OutsideAPIController : OutsideBaseApiController
    {
        private readonly IUriExtensions _uriExtensions;
        private readonly IUserService _userService;

        public OutsideAPIController(
            ILogger<OutsideBaseApiController> logger, 
            IUserService userService,
            IUriExtensions uriExtensions) 
            : base(logger, userService)
        {
            _userService = userService;
            _uriExtensions = uriExtensions;
        }

        /// <summary>
        /// 範例程式（需要驗證）
        /// </summary>
        /// <returns></returns>
        [NeedAuthorize]
        [HttpGet]
        [APIName("GetPersons")]
        [ApiLogException]
        [ApiLogonInfoAttribute]
        public async Task<IEnumerable<Person>> GetPersons()
        {
            return await Task.FromResult(new Person[] 
            { 
                new Person() 
                { 
                    ID = 1,
                    Name = "Gelis Wu"
                }
            });
        }
    }

    /// <summary>
    /// 範例程式：請放置在你的 Models/Dto/VO 專案裡
    /// </summary>
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
