using authentication.application.DTO;
using authentication.application.IRepository;
using authentication.domain;
using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace authentication.presistence.Repository
{
    public class AuthformRepository : IAuthformRepository
    {
        private readonly UserManager<User> _userManger;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthformRepository(UserManager<User> userManger, SignInManager<User> signInManager, RoleManager<Role> roleManager, IConfiguration configuration)
        {
            _userManger = userManger;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        public async Task<string> createRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return "Role name should be provided.";
            }

            var newRole = new Role
            {
                Name = role
            };

            var roleResult = await _roleManager.CreateAsync(newRole);

            if (roleResult.Succeeded)
            {
                return "ok";
            }
            return string.Join(", ", roleResult.Errors.Select(e => e.Description));
        }

        public async Task<string> login(string email, string password)
        {
            var user = await _userManger.Users.Where(t => t.Email == email).Select(t => t).Include(t => t.userRoles).ThenInclude(t => t.Role).FirstOrDefaultAsync();
            if (user.CurrentStatus == true)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
                if (result.Succeeded)
                {
                    string token = Generatejwt(user).Result;
                    return token;
                }
            }
            else
            {
                return "is not active";
            }
            //next we will create token that use two information inside it

            return "your user or login is incorrect";
        }
        private async Task<string> Generatejwt(User user)
        {
            var claim = new List<Claim>
                        {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Actor,user.EmailConfirmed.ToString()),
                new Claim(ClaimTypes.Email,user.Email)
            };
            var roles = await _userManger.GetRolesAsync(user);
            foreach (var item in roles)
            {
                claim.Add(new Claim(ClaimTypes.Role, item));
            }
            //we need a key to sing in our token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("appSetting:Token").Value));
            //we create signing credential
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //we going to create tokendescription to describe our expire date and signing credential
            var tokendescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred,

            };
            var tokenhandler = new JwtSecurityTokenHandler();
            var token = tokenhandler.CreateToken(tokendescription);
            return tokenhandler.WriteToken(token);
        }
        public async Task<User?> regiseter(User user, string password,string role)
        {
            if (user.SignitureImage != null && user.SignitureImage.Id > 0)
            {
                // 2. Save the integer ID to the REAL database column property
                user.SignitureImageId = user.SignitureImage.Id;

                // 3. Null out the object to stop EF Core from trying to insert it again!
                user.SignitureImage = null;
            }
            var users = await _userManger.CreateAsync(user, password);
            var Myuser = _userManger.Users.SingleOrDefault(u => u.Email == user.Email);
            var result = await _userManger.AddToRoleAsync(Myuser!, role);
            if (user != null || result != null)
            {
                return Myuser;
            }
            return null;
        }

        public async Task<IEnumerable<User>> getAll()
        {
            var user=await _userManger.Users.Include(x=>x.SignitureImage).ToListAsync();
            return user;
        }
    }
}
