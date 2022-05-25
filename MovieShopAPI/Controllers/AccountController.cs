using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application_Core.Contracts.Services;
using Application_Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [Route("Account/{id:int}")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            var user = await _accountService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        [Route("Account")]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            var newUser = await _accountService.RegisterUser(model);
            return Ok(newUser);

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            var user = await _accountService.LoginUser(model.Email, model.Password);
            var jwtToken =GenerateJwtToken(user);

            if (user == null)
            {
                return Unauthorized("Wrong Email/Password");
            }
            return Ok(new {token = jwtToken});
        }

        private string GenerateJwtToken(UserLoginResponseModel user)
        {
            //create claims so that we have those in the payload

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)

            };

            var identityClaims = new ClaimsIdentity();

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MyTopSecretKey"));

            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenExpiration = DateTime.UtcNow.AddHours(24);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDetails = new SecurityTokenDescriptor
            {
                Subject = identityClaims,
                Expires = tokenExpiration,
                SigningCredentials = credentials,
                Issuer = "MovieShop, Inc",
                Audience = "MovieShop Users"
            };

            var encodedJwt = tokenHandler.CreateToken(tokenDetails);

            return tokenHandler.WriteToken(encodedJwt);

        }
    }
}
