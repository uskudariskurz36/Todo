using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Todo.API.Entities;
using Todo.API.Models;

namespace Todo.API.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IConfiguration _configuration;
        private DatabaseContext _databaseContext;

        public AccountController(IConfiguration configuration, DatabaseContext databaseContext)
        {
            _configuration = configuration;
            _databaseContext = databaseContext;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult SignIn(SignInModel model)
        {
            Entities.User user = _databaseContext.Users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password); 

            if (user == null) {
                ModelState.AddModelError("", "Kullanıcı adı ve şifre eşleşmiyor.");
                return BadRequest(ModelState);
            }


            string secretKey = _configuration.GetValue<string>("Authentication:SecretKey");
            byte[] key = Encoding.UTF8.GetBytes(secretKey);

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("app", "todo.api"));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.Username));
            claims.Add(new Claim(ClaimTypes.Role, user.Role));

            JwtSecurityToken securityToken = 
                new JwtSecurityToken(
                    claims: claims, 
                    signingCredentials: credentials, 
                    expires: DateTime.Now.AddDays(1));

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return Ok(token);
        }
    }
}
