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
        User GetById(int id);
        User GetByUsername(string username);
    }
}
