using AutoMapper;
using CorrespondenceCore.Application.IRepository;
using CorrespondenceCore.Presistence.helper;
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
            services.AddSingleton<IhtmlbodyMongoRepository, htmlbodyMongoRepository>();
            services.Configure<Mongosettings>(x =>
            {
                x.Connection = configuration.GetSection("MongoSettings:Connection").Value;
                x.DatabaseName = configuration.GetSection("MongoSettings:DatabaseName").Value;
            });
            return services;
        }
    }
}
