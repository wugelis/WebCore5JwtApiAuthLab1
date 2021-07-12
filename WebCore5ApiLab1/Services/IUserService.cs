using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCore5ApiLab1.Dto;
using WebCore5ApiLab1.Models;

namespace WebCore5ApiLab1.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}
