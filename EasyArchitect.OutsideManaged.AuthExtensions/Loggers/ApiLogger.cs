using EasyArchitect.OutsideManaged.AuthExtensions.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Mxic.FrameworkCore.Core;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyArchitect.OutsideManaged.AuthExtensions.Loggers
{
    public class ApiLogger
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private static IUserService _userService;
        private static IUriExtensions _uriExtensions;

        public ApiLogger() { }
        /// <summary>
        /// 紀錄執行 Log Info 紀錄
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="startTime"></param>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <param name="paramesString"></param>
        /// <param name="currentTime"></param>
        /// <param name="sw"></param>
        /// <param name="absoluteUri"></param>
        public static void WriteLog(
            string userName,
            DateTime? startTime,
            string controllerName,
            string actionName,
            string paramesString,
            DateTime currentTime,
            long sw,
            string absoluteUri)
        {
            _logger.Info(string.Format("\t{0}\t[來源：{7}]\t執行\t{2}.{3}\t[parame:{4}]\t[開始時間：{1}]\t[結束時間：{5}]\t[花費時間：{6}]",
                    userName,
                    startTime,
                    controllerName,
                    actionName,
                    paramesString,
                    DateTime.Now,
                    sw,
                    absoluteUri));
        }
        /// <summary>
        /// 記錄詳細的錯誤訊息
        /// </summary>
        /// <param name="absoluteUri"></param>
        /// <param name="absoluteUri2"></param>
        /// <param name="httpMethod"></param>
        /// <param name="parames"></param>
        /// <param name="errorMeg"></param>
        /// <param name="stackTrace"></param>
        public static void WriteErrorLog(
            FilterContext context,
            string absoluteUri,
            string absoluteUri2,
            string httpMethod,
            string parames,
            string errorMeg,
            string? stackTrace)
        {
            _userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
            _uriExtensions = context.HttpContext.RequestServices.GetRequiredService<IUriExtensions>();

            _logger.Error(string.Format("\t[{0}]\t執行失敗\t{1}.{2}\t[parame:{3}]\t{4}\tStackTrace:{5}",
                                context.HttpContext.Request.GetAbsoluteUri(_uriExtensions),
                                context.HttpContext.Request.GetAbsoluteUri(_uriExtensions),
                                context.HttpContext.Request.Method,
                                "",
                                errorMeg,
                                stackTrace));
        }

        //public static void WriteErrorLog(
        //    FilterContext context,
        //    string absoluteUri,
        //    string absoluteUri2,
        //    string httpMethod,
        //    string parames,
        //    string errorMeg,
        //    string? stackTrace)
        //{
        //    _userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
        //    _uriExtensions = context.HttpContext.RequestServices.GetRequiredService<IUriExtensions>();

        //    _logger.Error(string.Format("\t[{0}]\t執行失敗\t{1}.{2}\t[parame:{3}]\t{4}\tStackTrace:{5}",
        //                        context.HttpContext.Request.GetAbsoluteUri(_uriExtensions),
        //                        context.HttpContext.Request.GetAbsoluteUri(_uriExtensions),
        //                        context.HttpContext.Request.Method,
        //                        "",
        //                        errorMeg,
        //                        stackTrace));
        //}
    }
}
