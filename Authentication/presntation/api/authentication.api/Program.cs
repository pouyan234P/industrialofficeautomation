using authentication.application;
using authentication.domain;
using authentication.presistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureApplicationService();
builder.Services.configurePersistenceServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();

// ─── JWT validation ──────────────────────────────────────────────────────────
// Fails fast at startup if the signing key is missing/too short (see JwtSettings).
var jwt = JwtSettings.From(builder.Configuration);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = jwt.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
        };
    });

// ─── Authorization ───────────────────────────────────────────────────────────
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy(Policies.RequireAdminRole, policy => policy.RequireRole(AppRoles.Admin));
    opt.AddPolicy(Policies.RequireCustomerRole, policy => policy.RequireRole(AppRoles.Customer));
});

builder.Services.AddControllers(opt =>
{
    // Every endpoint needs a valid token unless it is explicitly [AllowAnonymous] (only login is).
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
})
.AddNewtonsoftJson(opt =>
{
    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ─── Seed built-in roles + bootstrap admin ───────────────────────────────────
try
{
    await IdentitySeeder.SeedAsync(app.Services, app.Configuration, app.Logger);
}
catch (Exception ex)
{
    // Don't block startup if the database is temporarily unreachable; it is retried on next start.
    app.Logger.LogError(ex, "Seeding roles/admin failed");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// UseAuthentication was missing, so tokens were never read and [Authorize] could not work.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
