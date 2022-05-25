using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application_Core.Contracts.Services;
using Application_Core.Exceptions;
using Application_Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace MovieShopMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // showing the empty page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // when user clicks on Submit/register button
        // 
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            // save the info in User Table
            try
            {
                var user = await _accountService.RegisterUser(model);
            }
            catch(ConflictException)
            {
                throw;
                // logging the exceptions later to text/json files
            }
        
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            //Model Binding, it looks at the incoming request from clinet/browser and look at the infor and if th
            //matches with the properties of the model it will get the values automatically
            try
            {
                var user = await _accountService.LoginUser(model.Email, model.Password);
                if(user != null)
                {
                    // create a cookie, userid, email -> encrypted, expiration time
                    // each and every tiime you make an http request the cookies are sent to server in http
                    // cookie based authetication

                    // Claim called Admin Role to enter admin pages
                    var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())

            };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // create the cookie with above claims

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    //ASP.NET, how long the cookie is gonna be valid
                    // Name of the cookie
                    return LocalRedirect("~/");
                }
            }
            catch(Exception)
            {
                return View();
                throw;
            }
            
            return View();
        }

        

    }
}
