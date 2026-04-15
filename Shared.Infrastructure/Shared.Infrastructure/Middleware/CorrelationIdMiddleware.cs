using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Shared.Infrastructure.Middleware
{
    /// <summary>
    /// Reads X-Correlation-Id from the incoming request (or generates a new GUID),
    /// writes it back on the response, makes it available in HttpContext.Items,
    /// and pushes it into every Serilog log line for this request via LogContext.
    ///
    /// Register in every Program.cs BEFORE app.UseRouting():
    ///     app.UseMiddleware&lt;CorrelationIdMiddleware&gt;();
    ///
    /// Reading the ID inside a controller or service:
    ///     var id = HttpContext.Items[CorrelationIdMiddleware.HeaderName]?.ToString();
    ///
    /// Forwarding it on an outbound HTTP call (BaseService does this automatically):
    ///     message.Headers.Add(CorrelationIdMiddleware.HeaderName, correlationId);
    ///
    /// Forwarding it into a RabbitMQ message (all senders do this automatically):
    ///     new BasicProperties { CorrelationId = correlationId }
    /// </summary>
    public class CorrelationIdMiddleware
    {
        public const string HeaderName = "X-Correlation-Id";

        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Request.Headers[HeaderName].FirstOrDefault()
                                ?? Guid.NewGuid().ToString();

            context.Response.Headers[HeaderName] = correlationId;
            context.Items[HeaderName] = correlationId;

            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _next(context);
            }
        }
    }
}
