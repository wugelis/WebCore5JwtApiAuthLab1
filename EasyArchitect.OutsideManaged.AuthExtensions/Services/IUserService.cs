using EasyArchitect.OutsideManaged.AuthExtensions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyArchitect.OutsideManaged.AuthExtensions.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        //User GetById(int id);
        User GetByUsername(string username);
        /// <summary>
        /// 取得目前 Scoped 下的使用者
        /// </summary>
        string IdentityUser { get; set; }
        /// <summary>
        /// 取得目前 Scoped 下的使用者 ID
        /// </summary>
        decimal? IdentityId { get; set; }
    }
}
