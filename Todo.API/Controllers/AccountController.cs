using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Todo.API.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignIn()
        {
            string secretKey = _configuration.GetValue<string>("Authentication:SecretKey");
            byte[] key = Encoding.UTF8.GetBytes(secretKey);

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("app", "todo.api"));

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
