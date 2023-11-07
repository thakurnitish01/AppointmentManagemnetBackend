using DotNetAngularApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotNetAngularApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public IConfiguration _configuration;
        public readonly FormContext _formContext;
        public LoginController(IConfiguration configuration, FormContext formContext)
        {
            _formContext = formContext;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<User> GetUser(string username, string password)
        {
            var user = await _formContext.user.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
            if (user == null)
            {
                throw new Exception("User not found with the provided credentials.");
            }
            return user;
        }

        [HttpPost]
        public async Task<IActionResult> Post(User user)
        {
            if (user != null && user.UserName != null && user.Password != null)
            {
                var userData = await GetUser(user.UserName, user.Password);
                var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
                if (userData != null) // Check the correct variable for user data
                {
                    var claims = new[]
                    {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("Id", userData.UserId.ToString()), // Use userData instead of user
                new Claim("UserName", userData.UserName), // Use userData instead of user
                // Avoid including sensitive data like password in claims
            };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.key));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                                    jwt.Issuer,
                                    jwt.Audience,
                                    claims,
                                    expires: DateTime.Now.AddMinutes(20),
                                    signingCredentials: signIn
                                    );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(new { token = tokenString }); // Return the token as a JSON object
                }
                else
                {
                    return BadRequest("Invalid Credentials..");
                }
            }
            else
            {
                return BadRequest("Invalid Credentials..");
            }
        }

        [HttpPost("signup")]
        public async Task<ActionResult<User>> UserSignUp(User userData)
        {          
                if (userData == null || string.IsNullOrEmpty(userData.UserName) || string.IsNullOrEmpty(userData.Password))
                {
                    return BadRequest("Invalid user data. Please provide a valid username and password.");
                }
            var checkEmail = await _formContext.user.FirstOrDefaultAsync(u => u.UserName == userData.UserName); 
            if(checkEmail != null)
            {
                return BadRequest("This Email already exists.");
            }

            var addedUser = _formContext.user.Add(userData);
                await _formContext.SaveChangesAsync();

                return Ok(addedUser.Entity);
           
        }


    }
}
