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

        public async Task<List<string>> updateUser(int id, updateUserDTO dto)
        {
            var user = await _userManger.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return new List<string>() { $"User {id} was not found." };
            }

            var newUsername = dto.username!.Trim();

            // Validate the new password BEFORE changing anything, so a weak password
            // can never leave the user half-updated or without a password.
            if (!string.IsNullOrEmpty(dto.password))
            {
                var passwordErrors = new List<string>();
                foreach (var validator in _userManger.PasswordValidators)
                {
                    var check = await validator.ValidateAsync(_userManger, user, dto.password);
                    if (!check.Succeeded)
                        passwordErrors.AddRange(check.Errors.Select(e => e.Description));
                }
                if (passwordErrors.Count > 0)
                    return passwordErrors;
            }

            // Usernames must stay unique
            if (!string.Equals(user.UserName, newUsername, StringComparison.Ordinal))
            {
                var existing = await _userManger.FindByNameAsync(newUsername);
                if (existing != null && existing.Id != user.Id)
                    return new List<string>() { $"Username '{newUsername}' is already taken." };
                user.UserName = newUsername;
            }

            user.personID = dto.personID;
            user.CurrentStatus = dto.CurrentStatus;

            // Only replace the signature when a new image was uploaded
            if (dto.signitureImageId.HasValue)
            {
                user.SignitureImageId = dto.signitureImageId.Value;
                user.SignitureImage = null;
            }

            // UpdateAsync also refreshes NormalizedUserName and the concurrency stamp
            var updateResult = await _userManger.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return updateResult.Errors.Select(e => e.Description).ToList();

            if (!string.IsNullOrEmpty(dto.password))
            {
                // No reset-token providers are registered, so swap the password directly.
                // It was validated above, so AddPasswordAsync is not expected to fail.
                if (await _userManger.HasPasswordAsync(user))
                {
                    var removeResult = await _userManger.RemovePasswordAsync(user);
                    if (!removeResult.Succeeded)
                        return removeResult.Errors.Select(e => e.Description).ToList();
                }
                var addResult = await _userManger.AddPasswordAsync(user, dto.password);
                if (!addResult.Succeeded)
                    return addResult.Errors.Select(e => e.Description).ToList();
            }

            return new List<string>();
        }
    }
}
