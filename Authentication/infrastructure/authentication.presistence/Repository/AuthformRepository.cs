using authentication.application;
using authentication.application.DTO;
using authentication.application.IRepository;
using authentication.domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace authentication.presistence.Repository
{
    public class AuthformRepository : IAuthformRepository
    {
        // One message for "no such user" and "wrong password" so the login form
        // cannot be used to find out which email addresses have an account.
        private const string InvalidLogin = "Invalid email or password.";

        private readonly UserManager<User> _userManger;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly authDb _db;

        public AuthformRepository(UserManager<User> userManger, SignInManager<User> signInManager, RoleManager<Role> roleManager, IConfiguration configuration, authDb db)
        {
            _userManger = userManger;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _db = db;
        }

        // ─── Roles ───────────────────────────────────────────────────────────

        public async Task<List<string>> createRole(string role)
        {
            role = role?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(role))
                return new List<string> { "Role name should be provided." };

            if (await _roleManager.RoleExistsAsync(role))
                return new List<string> { $"Role '{role}' already exists." };

            var roleResult = await _roleManager.CreateAsync(new Role { Name = role });
            return roleResult.Succeeded
                ? new List<string>()
                : roleResult.Errors.Select(e => e.Description).ToList();
        }

        public async Task<IEnumerable<roleDTO>> getRoles()
        {
            var counts = await _db.UserRoles
                .GroupBy(ur => ur.RoleId)
                .Select(g => new { RoleId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.RoleId, x => x.Count);

            var roles = await _roleManager.Roles
                .OrderBy(r => r.Name)
                .Select(r => new { r.Id, r.Name })
                .ToListAsync();

            return roles.Select(r => new roleDTO
            {
                id = r.Id,
                name = r.Name ?? string.Empty,
                userCount = counts.TryGetValue(r.Id, out var c) ? c : 0
            }).ToList();
        }

        public async Task<List<string>> setUserRoles(int userId, IEnumerable<string> roles, int? actingUserId)
        {
            var user = await _userManger.FindByIdAsync(userId.ToString());
            if (user == null)
                return new List<string> { $"User {userId} was not found." };

            // Resolve every requested name to the stored role so casing is consistent.
            var wanted = new List<string>();
            var errors = new List<string>();
            foreach (var name in roles.Select(r => r.Trim()).Where(r => r.Length > 0).Distinct(StringComparer.OrdinalIgnoreCase))
            {
                var role = await _roleManager.FindByNameAsync(name);
                if (role?.Name == null)
                    errors.Add($"Role '{name}' does not exist.");
                else
                    wanted.Add(role.Name);
            }
            if (errors.Count > 0)
                return errors;

            var current = await _userManger.GetRolesAsync(user);
            var toRemove = current.Where(c => !wanted.Contains(c, StringComparer.OrdinalIgnoreCase)).ToList();
            var toAdd = wanted.Where(w => !current.Contains(w, StringComparer.OrdinalIgnoreCase)).ToList();

            if (toRemove.Contains(AppRoles.Admin, StringComparer.OrdinalIgnoreCase))
            {
                if (actingUserId == userId)
                    return new List<string> { "You cannot remove the Admin role from your own account." };

                var admins = await _userManger.GetUsersInRoleAsync(AppRoles.Admin);
                if (admins.Count <= 1)
                    return new List<string> { "At least one user must keep the Admin role." };
            }

            if (toRemove.Count > 0)
            {
                var removed = await _userManger.RemoveFromRolesAsync(user, toRemove);
                if (!removed.Succeeded)
                    return removed.Errors.Select(e => e.Description).ToList();
            }
            if (toAdd.Count > 0)
            {
                var added = await _userManger.AddToRolesAsync(user, toAdd);
                if (!added.Succeeded)
                    return added.Errors.Select(e => e.Description).ToList();
            }
            return new List<string>();
        }

        // ─── Login / token ───────────────────────────────────────────────────

        public async Task<loginResultDTO> login(string email, string password)
        {
            // Query by normalized email instead of FindByEmailAsync: older data may contain
            // duplicate emails, which would make FindByEmailAsync throw.
            var normalized = _userManger.NormalizeEmail(email);
            var user = await _userManger.Users
                .Where(t => t.NormalizedEmail == normalized)
                .OrderBy(t => t.Id)
                .FirstOrDefaultAsync();

            if (user == null)
                return loginResultDTO.Fail(InvalidLogin);

            // lockoutOnFailure: repeated wrong passwords lock the account for a while (see Identity options).
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
            if (result.IsLockedOut)
                return loginResultDTO.Fail("Too many failed attempts. Try again in a few minutes.");
            if (!result.Succeeded)
                return loginResultDTO.Fail(InvalidLogin);

            // Only reveal the account state once the password was proven correct.
            if (!user.CurrentStatus)
                return loginResultDTO.Fail("This account is inactive. Contact an administrator.");

            return loginResultDTO.Ok(await Generatejwt(user));
        }

        private async Task<string> Generatejwt(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var role in await _userManger.GetRolesAsync(user))
                claims.Add(new Claim(ClaimTypes.Role, role));

            var jwt = JwtSettings.From(_configuration);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = jwt.Issuer,
                Audience = jwt.Audience,
                Expires = DateTime.UtcNow.AddHours(jwt.LifetimeHours),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
            };
            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(handler.CreateToken(descriptor));
        }

        // ─── Users ───────────────────────────────────────────────────────────

        public async Task<List<string>> regiseter(User user, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(role) || !await _roleManager.RoleExistsAsync(role))
                return new List<string> { $"Role '{role}' does not exist." };

            if (user.SignitureImage != null && user.SignitureImage.Id > 0)
            {
                // Link the already-uploaded image by id and stop EF from inserting it again.
                user.SignitureImageId = user.SignitureImage.Id;
                user.SignitureImage = null;
            }

            var created = await _userManger.CreateAsync(user, password);
            if (!created.Succeeded)
                return created.Errors.Select(e => e.Description).ToList();

            var added = await _userManger.AddToRoleAsync(user, role);
            if (!added.Succeeded)
            {
                // Don't leave a user behind without the role that was asked for.
                await _userManger.DeleteAsync(user);
                return added.Errors.Select(e => e.Description).ToList();
            }
            return new List<string>();
        }

        public async Task<IEnumerable<getuserDTO>> getAll()
        {
            // Project only what the list needs; the signature image bytes are never loaded here.
            var users = await _userManger.Users
                .AsNoTracking()
                .OrderBy(u => u.Id)
                .Select(u => new { u.Id, u.UserName, u.personID, u.CurrentStatus, u.SignitureImageId })
                .ToListAsync();

            var userRoles = await (from ur in _db.UserRoles
                                   join r in _db.Roles on ur.RoleId equals r.Id
                                   select new { ur.UserId, r.Name }).ToListAsync();
            var rolesByUser = userRoles.ToLookup(x => x.UserId, x => x.Name ?? string.Empty);

            return users.Select(u => new getuserDTO
            {
                id = u.Id,
                username = u.UserName ?? string.Empty,
                personID = u.personID,
                CurrentStatus = u.CurrentStatus,
                signitureimageid = u.SignitureImageId.HasValue ? new getimageidDTO { id = u.SignitureImageId.Value } : null,
                roles = rolesByUser[u.Id].OrderBy(n => n).ToList()
            }).ToList();
        }

        public async Task<List<string>> updateUser(int id, updateUserDTO dto, bool callerIsAdmin)
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

            // PersonID and Active/Inactive are administrative fields: when people edit
            // their own account, those two keep their current values.
            if (callerIsAdmin)
            {
                user.personID = dto.personID;
                user.CurrentStatus = dto.CurrentStatus;
            }

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
