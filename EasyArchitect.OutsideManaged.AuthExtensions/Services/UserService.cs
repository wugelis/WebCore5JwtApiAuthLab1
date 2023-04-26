using EasyArchitect.OutsideManaged.AuthExtensions.Models;
using EasyArchitect.OutsideManaged.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EasyArchitect.OutsideManaged.AuthExtensions.Services
{
    /// <summary>
    /// 外部人員權限管理系統驗證服務
    /// </summary>
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly ModelContext _context;
        private string _identityUser;

        /// <summary>
        /// 取得目前 Scoped 下的使用者
        /// </summary>
        public string IdentityUser { get => _identityUser; set => _identityUser = value; }

#pragma warning disable CS8618 // 退出建構函式時，不可為 Null 的欄位必須包含非 Null 值。請考慮宣告為可為 Null。
        /// <summary>
        /// 外部人員初始化
        /// </summary>
        /// <param name="appSettings"></param>
        public UserService(IOptions<AppSettings> appSettings)
#pragma warning restore CS8618 // 退出建構函式時，不可為 Null 的欄位必須包含非 Null 值。請考慮宣告為可為 Null。
        {
            _appSettings = appSettings.Value;
            //_context = context;
        }

        /// <summary>
        /// 外部人員初始化（包含 DbContext）
        /// </summary>
        /// <param name="appSettings">使用原生 AppSettings 物件</param>
        /// <param name="context">DbContext 物件</param>
        public UserService(AppSettings appSettings, ModelContext context)
        {
            _appSettings = appSettings;
            _context = context;
        }


        /// <summary>
        /// 外部人員驗證方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            //throw new SyntaxErrorException("ORA-00933:SQL命令未正确结束。");

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

            // 若目前登入的使用者存在於系統中，將目前 User 名稱記錄在 IdentityUser 屬性中（此屬性只在目前 Scoped Lifecycle 中有效）
            IdentityUser = model.Username;

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
