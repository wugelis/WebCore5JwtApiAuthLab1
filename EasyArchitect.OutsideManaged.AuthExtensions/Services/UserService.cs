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
        private List<User> _users = new List<User>
        {
            new User 
            { 
                Id = 0, 
                FirstName = "Gelis", 
                LastName = "Wu", 
                Username = "gelis", 
                Password = "123456" 
            }
        };

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _users
                .SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            if (user == null)
            {
                return null;
            }

            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        public User GetByUsername(string username)
        {
            return _users.Where(c => c.Username == username).FirstOrDefault();
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
