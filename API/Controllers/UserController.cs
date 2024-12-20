using Core;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly SymmetricSecurityKey key;

        public UserController( 
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"]));
        }

        [HttpPost("login")]
        public async Task<IActionResult> UserLogin([FromBody] Login login)
        {
            try
            {
                var user = await userManager.Users.Where(x => x.Email == login.Email).FirstOrDefaultAsync() ?? throw new Exception("Unorthorized");

                var signInResult = await signInManager.CheckPasswordSignInAsync(user, login.Password, false) ?? throw new Exception("Unorthorized");

                var response = new LoginResponse
                {
                    Id = user.Id,
                    Name = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserType = user.UserType,
                    Token = CreateToken(user)
                };

                return Ok(response);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("signup")]
        public async Task<IActionResult> UserSignUp(SignUp signUp)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(signUp.Email);

                if (user == null)
                {
                    var appUser = new AppUser
                    {
                        Email = signUp.Email,
                        UserName = signUp.Name,
                        PhoneNumber = signUp.PhoneNumber,
                        UserType = signUp.UserType,
                    };

                    var result = await userManager.CreateAsync(appUser, signUp.Password);

                    if (!result.Succeeded) throw new Exception("System Error!. Somthing went wrong");

                    return Ok();
                }
                else
                {
                    throw new Exception("Email already registerd");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.GivenName, user.UserName)
            };

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials,
                Issuer = configuration["Token:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
