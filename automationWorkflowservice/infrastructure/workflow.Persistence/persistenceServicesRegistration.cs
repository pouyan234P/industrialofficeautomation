using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workflow.Appliction.IRepository;
using workflow.Persistence.helper;
using workflow.Persistence.Repository;

namespace workflow.Persistence
{
    public static class persistenceServicesRegistration
    {
        public static IServiceCollection configurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Mongosettings>(x =>
            {
                x.Connection = configuration.GetSection("MongoSettings:Connection").Value;
                x.DatabaseName = configuration.GetSection("MongoSettings:DatabaseName").Value;
            });
            services.AddSingleton<IReferralRepository, referralRepository>();
            return services;
        }
    }
}
