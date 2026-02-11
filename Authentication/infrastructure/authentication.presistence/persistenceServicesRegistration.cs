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
            IdentityBuilder builder = services.AddIdentityCore<User>();

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
