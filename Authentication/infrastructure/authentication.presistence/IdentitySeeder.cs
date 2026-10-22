using authentication.application;
using authentication.domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace authentication.presistence
{
    /// <summary>
    /// Runs at startup:
    ///   1. makes sure the built-in roles (Admin, Customer) exist;
    ///   2. if nobody has the Admin role yet, promotes or creates the account named in
    ///      "BootstrapAdmin:Email" (and "BootstrapAdmin:Password" when it has to be created).
    /// Without step 2 nobody could reach the admin-only endpoints after the lock-down.
    /// </summary>
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration, ILogger logger)
        {
            using var scope = services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            foreach (var name in new[] { AppRoles.Admin, AppRoles.Customer })
            {
                if (!await roleManager.RoleExistsAsync(name))
                {
                    var created = await roleManager.CreateAsync(new Role { Name = name });
                    if (created.Succeeded)
                        logger.LogInformation("Created built-in role {Role}", name);
                    else
                        logger.LogError("Could not create role {Role}: {Errors}", name, string.Join("; ", created.Errors.Select(e => e.Description)));
                }
            }

            var admins = await userManager.GetUsersInRoleAsync(AppRoles.Admin);
            if (admins.Count > 0)
                return;

            var email = configuration["BootstrapAdmin:Email"];
            if (string.IsNullOrWhiteSpace(email))
            {
                logger.LogWarning(
                    "No user has the Admin role. Set BootstrapAdmin:Email (and BootstrapAdmin:Password to create a new account) " +
                    "in user-secrets or environment variables and restart, otherwise user and role management are unreachable.");
                return;
            }

            var normalized = userManager.NormalizeEmail(email);
            var user = await userManager.Users.Where(u => u.NormalizedEmail == normalized).OrderBy(u => u.Id).FirstOrDefaultAsync();
            if (user == null)
            {
                var password = configuration["BootstrapAdmin:Password"];
                if (string.IsNullOrWhiteSpace(password))
                {
                    logger.LogWarning("BootstrapAdmin:Email {Email} does not exist and no BootstrapAdmin:Password was given; no admin was created.", email);
                    return;
                }

                user = new User
                {
                    Email = email,
                    UserName = configuration["BootstrapAdmin:UserName"] ?? "admin",
                    Name = "System",
                    family = "Administrator",
                    Country = string.Empty,
                    CurrentStatus = true,
                    Date = DateTime.Now.Date,
                    personID = 1,
                };
                var created = await userManager.CreateAsync(user, password);
                if (!created.Succeeded)
                {
                    logger.LogError("Could not create bootstrap admin: {Errors}", string.Join("; ", created.Errors.Select(e => e.Description)));
                    return;
                }
            }

            var added = await userManager.AddToRoleAsync(user, AppRoles.Admin);
            if (added.Succeeded)
                logger.LogWarning("Granted the Admin role to {Email} (bootstrap). Remove BootstrapAdmin:Password from configuration now.", email);
            else
                logger.LogError("Could not grant Admin to {Email}: {Errors}", email, string.Join("; ", added.Errors.Select(e => e.Description)));
        }
    }
}
