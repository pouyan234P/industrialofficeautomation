using CorrespondenceCore.Application;
using CorrespondenceCore.Presistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using Shared.Infrastructure.HealthChecks;
using Shared.Infrastructure.Middleware;
using Shared.Infrastructure.Logging;
var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureApplicationService();
builder.Services.configurePersistenceServices(builder.Configuration);
// Add services to the container.

// ── 1. Serilog (from Shared.Infrastructure) ──────────────────────────────────
// Structured JSON logs. Every line includes: Service, CorrelationId,
// MachineName, ThreadId, Environment.
// FIX: Use the correct method to configure Serilog for WebApplicationBuilder.
builder.AddSerilog("CorrespondenceService");

builder.Host.UseSerilog(Log.Logger);

// ── 3. Health checks (from Shared.Infrastructure) ────────────────────────────
// Pass true only for dependencies this service actually uses.
// Requires AspNetCore.HealthChecks.SqlServer and .MongoDb in the .csproj.

builder.Services.AddServiceHealthChecks(hc =>
{
    hc.AddSqlServer(
        // 1. Connection String
        builder.Configuration.GetConnectionString("myconn")!,
        // 2. Health Query (Required to bypass the named parameters)
        "SELECT 1;",
        // 3. Configure action (null is fine)
        null,
        // 4. Name
        "sql-server",
        // 5. Failure Status
        HealthStatus.Unhealthy,
        // 6. Tags
        ["db", "sql"]
    );

    hc.AddMongoDb(
        // Modern package requirement: Use a factory to instantiate the driver
        clientFactory: sp => new MongoDB.Driver.MongoClient(builder.Configuration.GetConnectionString("MongoSettings")!),
        name: "mongodb",
        failureStatus: HealthStatus.Degraded,
        tags: ["db", "mongo"]
    );

});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ── 4. Serilog request logging ────────────────────────────────────────────────
// Logs every HTTP request as one structured line with method, path,
// status code, and elapsed time.
app.UseSerilogRequestLogging();

// ── 5. Correlation ID middleware (from Shared.Infrastructure) ─────────────────
// Reads or generates X-Correlation-Id, writes it to the response,
// and injects it into every Serilog log line for this request.
// Must come BEFORE UseRouting.
app.UseMiddleware<CorrelationIdMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// FIXED: UseAuthentication must come before UseAuthorization.
// The original had them in the wrong order — JWT tokens were never validated.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ── 6. Health endpoints (from Shared.Infrastructure) ─────────────────────────
// GET /health       → readiness (checks SQL + MongoDB)
// GET /health/live  → liveness  (always 200 if process is alive)
// GET /health/ready → same as /health
app.MapHealthEndpoints();

try
{
    Log.Information("Starting CorrespondenceService");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "CorrespondenceService failed to start");
}
finally
{
    Log.CloseAndFlush();
}