using AutoMapper;
using CorrespondenceCore.Application.IRepository;
using CorrespondenceCore.Presistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Presistence
{
    public static class persistenceServicesRegistration
    {
        public static IServiceCollection configurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CorrespondenceCoreDB>(options => options.UseSqlServer(configuration.GetConnectionString("myconn")));
            services.AddScoped(typeof(IGenericRepository<>), typeof(genericRepository<>));
            services.AddScoped<IattachmentRepository, attachmentRepository>();
            services.AddScoped<ILetterRepository, letterRepository>();
            return services;
        }
    }
}
