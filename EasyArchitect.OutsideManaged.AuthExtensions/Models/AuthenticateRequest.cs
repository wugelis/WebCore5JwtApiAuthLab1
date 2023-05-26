using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EasyArchitect.OutsideManaged.AuthExtensions.Models
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        /// <summary>
        /// 是否為 MXIC 的管理人員
        /// </summary>
        [Required]
        public bool IsMXIC { get; set; }
    }
}
