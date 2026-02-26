using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using searchengine.Application.IRepository;
using searchengine.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchengine.Persistence
{
    public static class persistenceServicesRegistration
    {
        public static IServiceCollection configurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new ElasticsearchClientSettings(new Uri("http://localhost:9200"))
            .DefaultIndex("letters_index") // نام دیتابیس (ایندکس) پیش‌فرض
                                    // در محیط Production باید یوزر و پسورد تنظیم کنید:
                                    // .Authentication(new BasicAuthentication("elastic", "your_password"))
     ;

            var elasticClient = new ElasticsearchClient(settings);

            // 2. تزریق وابستگی (DI)
            services.AddSingleton(elasticClient);
            services.AddScoped<ILetterSearchRepository, LetterSearchRepository>();
            return services;
        }
    }
}
