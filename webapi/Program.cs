using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using Shared.Infrastructure.HealthChecks;
using Shared.Infrastructure.Logging;
using Shared.Infrastructure.Middleware;
using System.Text;
using webapi;
using webapi.RabbmitmqSender;
using webapi.Services;
using webapi.Services.CorrespondenceService;
using webapi.Services.IdentityService;
using webapi.Services.IServices.ICorrespondenceService;
using webapi.Services.IServices.Identity;
using webapi.Services.IServices.ISearchEngineService;
using webapi.Services.IServices.IWorkflowService;
using webapi.Services.SearchEngineService;
using webapi.Services.WorkflowService;

var builder = WebApplication.CreateBuilder(args);
// ─── 1. SERILOG ─────────────────────────────────────────────────────────────
builder.AddSerilog("WebAPI");

// ─── 2. HTTP CLIENTS with POLLY (Retries + Circuit Breaker) ─────────────────
builder.Services.AddHttpClient("industrial", c =>
{
    c.Timeout = TimeSpan.FromSeconds(30);
})
.AddPolicyHandler(GetRetryPolicy())
.AddPolicyHandler(GetCircuitBreakerPolicy());

// ─── 3. IHttpContextAccessor ─────────────────────────────────────────────────
// Required by BaseService to read CorrelationId from the current request
// and forward it on every outbound HTTP call to downstream services
builder.Services.AddHttpContextAccessor();

// ─── 4. REDIS ────────────────────────────────────────────────────────────────
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

// ─── 5. JWT AUTHENTICATION ───────────────────────────────────────────────────
// The key must be the same one the auth service signs with. It is read from
// user-secrets / the appSetting__Token environment variable, never from appsettings.json.
var secret = builder.Configuration["appSetting:Token"];
if (string.IsNullOrWhiteSpace(secret) || Encoding.UTF8.GetByteCount(secret) < 32)
    throw new InvalidOperationException(
        "JWT signing key 'appSetting:Token' is missing or shorter than 32 bytes. Set it with " +
        "`dotnet user-secrets set \"appSetting:Token\" \"<same value as the auth service>\"` or appSetting__Token.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["appSetting:Issuer"] ?? "industrial-office-auth",
            ValidateAudience = true,
            ValidAudience = builder.Configuration["appSetting:Audience"] ?? "industrial-office",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
        };
    });

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy(Policies.RequireAdminRole, p => p.RequireRole(Policies.AdminRole));
});

// ─── 5b. CORS ────────────────────────────────────────────────────────────────
// Only the Angular app's origin(s) may call the API from a browser.
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                     ?? new[] { "http://localhost:4200" };
builder.Services.AddCors(o => o.AddPolicy("frontend", p => p
    .WithOrigins(allowedOrigins)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .WithExposedHeaders("Pagination")));

// ─── 6. DEPENDENCY INJECTION ─────────────────────────────────────────────────
builder.Services.AddScoped<ILetterCorrespondenceService, LetterCorrespondenceService>();
builder.Services.AddScoped<IgenerateNextNumbeService, generateNextNumbeService>();
builder.Services.AddScoped<IAttachmentCorrespondenceService, AttachmentCorrespondenceService>();
builder.Services.AddScoped<IreferralWorkflowService, referralWorkflowService>();
builder.Services.AddScoped<IletterelsiSearchEngineService, letterelsiSearchEngineService>();
builder.Services.AddScoped<IauthIdentityService, authIdentityService>();
builder.Services.AddScoped<IpositionIdentityService, positionIdentityService>();
builder.Services.AddScoped<IdepartmentIdentityService, departmentIdentityService>();
builder.Services.AddScoped<IpictureIdentityService, pictureIdentityService>();
builder.Services.AddScoped<BaseService>();
SD.gatewayApiBase = builder.Configuration["ServiceUrls:gatewayApi"]!;
SD.identityApiBase = builder.Configuration["ServiceUrls:IdentityApi"]!;

// RabbitMQ senders are singletons — they create a fresh connection per publish
builder.Services.AddSingleton<IRabbitMQreferralMessageSender, RabbitMqreferralMessageSender>();
builder.Services.AddSingleton<IRabbitMQsearchMessageSender, RabbitMQsearchMessageSender>();

// ─── 7. HEALTH CHECKS ────────────────────────────────────────────────────────
builder.Services.AddServiceHealthChecks(hc =>
{
    hc.AddRedis(
        // 1. Connection String
        builder.Configuration.GetConnectionString("Redis")!,
        // 2. Name
        "redis",
        // 3. Failure Status
        HealthStatus.Degraded,
        // 4. Tags
        ["cache"]
    );
});

builder.Services.AddControllers(opt =>
{
    // Every endpoint needs a valid token unless it is marked [AllowAnonymous] (only login is).
    opt.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
}).AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ─────────────────────────────────────────────────────────────────────────────
var app = builder.Build();

// ─── 8. SERILOG REQUEST LOGGING ──────────────────────────────────────────────
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate =
        "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
});

// ─── 9. CORRELATION ID MIDDLEWARE ────────────────────────────────────────────
// Must be before UseRouting so the ID is in HttpContext.Items
// for controllers and BaseService to read
app.UseMiddleware<CorrelationIdMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ─── 10. HEALTH ENDPOINTS ────────────────────────────────────────────────────
app.MapHealthEndpoints();

try
{
    Log.Information("Starting WebAPI");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "WebAPI failed to start");
}
finally
{
    Log.CloseAndFlush();
}

// ─── POLLY POLICIES ──────────────────────────────────────────────────────────

 static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
    HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: attempt =>
                TimeSpan.FromSeconds(Math.Pow(2, attempt))
                + TimeSpan.FromMilliseconds(Random.Shared.Next(0, 200)),
            onRetry: (outcome, timespan, attempt, _) =>
                Log.Warning(
                    "HTTP retry {Attempt}/3 after {Delay}ms. Reason: {Reason}",
                    attempt,
                    timespan.TotalMilliseconds,
                    outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString()));

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
    HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 5,
            durationOfBreak: TimeSpan.FromSeconds(30),
            onBreak: (outcome, duration) =>
                Log.Error("Circuit breaker OPENED for {Duration}s. Reason: {Reason}",
                    duration.TotalSeconds,
                    outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString()),
            onReset: () => Log.Information("Circuit breaker CLOSED — resuming normally"),
            onHalfOpen: () => Log.Warning("Circuit breaker HALF-OPEN — testing next call"));