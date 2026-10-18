using authentication.application.IRepository;
using authentication.domain;
using authentication.presistence.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.presistence
{
    public static class persistenceServicesRegistration
    {
        public static IServiceCollection configurePersistenceServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<authDb>(options => options.UseSqlServer(configuration.GetConnectionString("myconn")));
            services.AddScoped<IAuthformRepository, AuthformRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IPictureRepository, PictureRepository>();
            IdentityBuilder builder = services.AddIdentityCore<User>(opt =>
            {
                // Same rules as createAuthCommandValidator, enforced again by Identity itself
                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = true;

                // Lock an account for 15 minutes after 5 wrong passwords in a row
                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

                opt.User.RequireUniqueEmail = true;
            });

            // Explicitly using the custom Role class
            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);

            // Wiring up the Database Context
            builder.AddEntityFrameworkStores<authDb>();

            // Adding Managers and Validators
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();
            return services;
        }
    }
}
