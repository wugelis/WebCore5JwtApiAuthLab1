using EasyArchitect.OutsideManaged.AuthExtensions.Models;
using EasyArchitect.OutsideManaged.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EasyArchitect.OutsideManaged.AuthExtensions.Services
{
    public class UserService : IUserService
    {
        //private List<User> _users = new List<User>
        //{
        //    new User 
        //    { 
        //        Id = 0, 
        //        FirstName = "Gelis", 
        //        LastName = "Wu", 
        //        Username = "gelis", 
        //        Password = "123456" 
        //    }
        //};

        private readonly AppSettings _appSettings;
        private readonly ModelContext _context;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            //_context = context;
        }

        public UserService(AppSettings appSettings, ModelContext context)
        {
            _appSettings = appSettings;
            _context = context;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _context
                .Accountvos
                .Where(x => x.Userid == model.Username && x.Password == model.Password)
                .Select(u => new User() 
                { 
                    Id = 1,
                    Username = u.Userid
                })
                .FirstOrDefault();

            if (user == null)
            {
                return null;
            }

            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {
            return _context
                .Accountvos
                .Select(x => new User() 
                { 
                    Username = x.Userid
                });
        }

        //public User GetById(int id)
        //{
        //    return _context
        //        .Accountvos.FirstOrDefault(x => x.Userid == id);
        //}

        public User GetByUsername(string username)
        {
#pragma warning disable CS8603 // 可能有 Null 參考傳回。
            return _context
                .Accountvos
                .Where(c => c.Userid== username)
                .Select(x => new User()
                {
                    Username = x.Userid
                })
                .FirstOrDefault();
#pragma warning restore CS8603 // 可能有 Null 參考傳回。
        }

        /// <summary>
        /// 產生 Jwt Token.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string generateJwtToken(User user)
        {
            // 產生一個期限只有 【appSettings/TimeoutMinutes】所設定之時間（分鐘）
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("Username", user.Username) }),
                Expires = DateTime.UtcNow.AddMinutes(_appSettings.TimeoutMinutes.HasValue ? _appSettings.TimeoutMinutes.Value : 30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
