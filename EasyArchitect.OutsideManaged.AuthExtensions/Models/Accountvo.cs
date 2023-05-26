using System;
using System.Collections.Generic;

namespace EasyArchitect.OutsideManaged.AuthExtensions.Models
{
    public partial class Accountvo
    {
        public string Userid { get; set; } = null!;
        public string? Password { get; set; }
        /// <summary>
        /// 流水號（識別用）
        /// </summary>
        public decimal? ID { get; set; }
    }
}
