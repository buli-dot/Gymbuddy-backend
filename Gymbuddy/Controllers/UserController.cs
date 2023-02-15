using Gymbuddy.Entities;
using Gymbuddy.Models;
using Gymbuddy.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gymbuddy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserController : Controller
    {
        private readonly DB _db;
        private readonly IConfiguration _config;
        public UserController(DB db, IConfiguration config) { 
            _db= db;
            _config= config;
        }
        [HttpPost("register-user")]
        public IActionResult User([FromForm] RegisterUser mod)
        {
            User model = new User();
            model.UserName = mod.username;
            var salt = Cryptography.Salt.Create();
            var hash = Cryptography.Hash.Create(mod.password, salt);
            model.PasswordSalt = salt;
            model.PasswordHash = hash;
            model.Age = mod.age;
            model.Email = mod.email;
            model.FirstName = mod.firstname;
            model.LastName = mod.lastname;
            _db.Users.Add(model);
            _db.SaveChanges();
            return Ok();
               
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromForm] LoginPost postLogin)
        {
            var userAuth = Authenticate(postLogin);
            if(userAuth != null)
            {
                
                var token = Generate(userAuth);
                return Ok(token);
            }
            return Unauthorized();
        }

        private object Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var roles = _db.UserRoles.Where(x => x.UserId == user.Id).Include(x => x.Role);
            var model = JsonConvert.SerializeObject(roles);
            var claims = new[]
            {
                new Claim("Email", user.Email),
                new Claim("Lastname", user.LastName),
                new Claim("Username", user.UserName),
                new Claim("FirstName", user.FirstName),
                new Claim("Roles",model),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(240),
              signingCredentials: credentials) ;

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User Authenticate(LoginPost model)
        {
            var user = _db.Users.FirstOrDefault(x=>x.UserName == model.username );
            if(user != null && Cryptography.Hash.Validate(model.password, user.PasswordSalt, user.PasswordHash))
            {
                return user;
            }
            return null;
        }

    }
}
