using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Infrastructure.HealthChecks
{
    public static class HealthCheckExtensions
    {
        /// <summary>
        /// The Shared Library ONLY sets up the core health check framework.
        /// It does NOT know about SQL, Mongo, etc. 
        /// </summary>
        public static IServiceCollection AddServiceHealthChecks(
            this IServiceCollection services,
            Action<IHealthChecksBuilder>? configureDependencies = null)
        {
            var hc = services.AddHealthChecks();

            // Let the consuming project register its specific dependencies
            configureDependencies?.Invoke(hc);

            return services;
        }

        public static WebApplication MapHealthEndpoints(this WebApplication app)
        {
            app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                Predicate = _ => false
            });

            app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                ResponseWriter = WriteJsonResponse
            });

            app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                ResponseWriter = WriteJsonResponse
            });

            return app;
        }

        private static async Task WriteJsonResponse(HttpContext context, HealthReport report)
        {
            context.Response.ContentType = "application/json";
            var json = System.Text.Json.JsonSerializer.Serialize(new
            {
                status = report.Status.ToString(),
                duration = report.TotalDuration.TotalMilliseconds,
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    duration = e.Value.Duration.TotalMilliseconds
                })
            });
            await context.Response.WriteAsync(json);
        }
    }
}