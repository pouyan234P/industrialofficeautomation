using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace Shared.Infrastructure.Logging
{
    /// <summary>
    /// One call sets up structured JSON logging identically across every service.
    ///
    /// Usage — first line of every Program.cs:
    ///     builder.AddSerilog("WorkflowService");
    ///
    /// Every log line automatically includes:
    ///     Service, CorrelationId, MachineName, ThreadId, Environment
    ///
    /// CorrelationId is injected by CorrelationIdMiddleware via LogContext —
    /// it appears on every log line for the duration of that HTTP request
    /// or RabbitMQ message handler.
    /// </summary>
    public static class SerilogExtensions
    {
        public static WebApplicationBuilder AddSerilog(
            this WebApplicationBuilder builder,
            string serviceName)
        {
            var env = builder.Environment;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(env.IsDevelopment()
                    ? LogEventLevel.Debug
                    : LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft",                  LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System",                     LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithProperty("Service",     serviceName)
                .Enrich.WithProperty("Environment", env.EnvironmentName)
                .WriteTo.Console(new CompactJsonFormatter())
                .CreateLogger();

            builder.Host.UseSerilog();

            return builder;
        }
    }
}
