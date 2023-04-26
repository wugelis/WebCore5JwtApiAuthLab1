using Microsoft.AspNetCore.Http;
using Mxic.FrameworkCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyArchitect.OutsideApiControllerBase
{
    /// <summary>
    /// 提供 Uri 相關的 Extension 方法
    /// </summary>
    public class UriExtensions : IUriExtensions
    {
        /// <summary>
        /// 取得原始絕對網址
        /// </summary>
        /// <param name="httpRequest">Extension 自帶型別</param>
        /// <returns></returns>
        public string GetAbsoluteUri(HttpRequest httpRequest)
        {
            var absoluteUri = string.Concat(
                        httpRequest.Scheme,
                        "://",
                        httpRequest.Host.ToUriComponent(),
                        httpRequest.PathBase.ToUriComponent(),
                        httpRequest.Path.ToUriComponent(),
                        httpRequest.QueryString.ToUriComponent());

            return absoluteUri;
        }
        /// <summary>
        /// 取得原始絕對網址（使用自定義的 Uri Path）
        /// </summary>
        /// <param name="httpRequest">IHttpContextAccessor 的 HttpContext</param>
        /// <param name="customPathUri">自定義的 Uri（不包括 Schema 與 Host）</param>
        /// <returns></returns>
        public string GetAbsoluteUri(HttpRequest httpRequest, string customPathUri)
        {
            var absoluteUri = string.Concat(
                        httpRequest.Scheme,
                        "://",
                        httpRequest.Host.ToUriComponent(),
                        httpRequest.PathBase.ToUriComponent(),
                        customPathUri,
                        httpRequest.QueryString.ToUriComponent());

            return absoluteUri;
        }
    }
}
