using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyArchitect.OutsideManaged.AuthExtensions.Attributes
{
    /// <summary>
    /// 提供衍伸的 API Controller 正名公開的 Route API Name
    /// </summary>
    public class APIName : RouteAttribute
    {
        /// <summary>
        /// 設定 API 方法名稱
        /// </summary>
        /// <param name="template"></param>
        public APIName(string template) 
            : base(template)
        {
        }
    }
}
