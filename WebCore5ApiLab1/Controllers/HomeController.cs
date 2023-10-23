using EasyArchitect.OutsideControllerBase;
using EasyArchitect.OutsideManaged.AuthExtensions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebCore5ApiLab1.Controllers
{
    /// <summary>
    /// Home Controller 範例程式
    /// </summary>
    public class HomeController : OutsideBaseController
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public HomeController(IHttpContextAccessor httpContextAccessor) 
            : base(httpContextAccessor)
        {
        }

        /// <summary>
        /// 示範首頁（需要登入）
        /// </summary>
        /// <returns></returns>
        [NeedLogin]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 隱私權畫面
        /// </summary>
        /// <returns></returns>
        [NeedLogin]
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// 登入系統畫面
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 執行登入作業
        /// </summary>
        /// <param name="authenticateRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(AuthenticateRequest authenticateRequest)
        {
            if(ModelState.IsValid)
            {
                if (ProcessLogin(authenticateRequest))
                {
                    return RedirectToAction("Index");
                }
            }

            return View();
        }

        /// <summary>
        /// 登出系統
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Logout()
        {
            return View();
        }

        /// <summary>
        /// 登出系統
        /// </summary>
        /// <param name="forms"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async new Task<IActionResult> Logout(IFormCollection forms)
        {
            return await base.LogoutProcess();
        }
    }
}
