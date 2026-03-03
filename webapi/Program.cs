using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using webapi;
using webapi.RabbmitmqSender;
using webapi.Services.CorrespondenceService;
using webapi.Services.IdentityService;
using webapi.Services.IServices.ICorrespondenceService;
using webapi.Services.IServices.Identity;
using webapi.Services.IServices.IWorkflowService;
using webapi.Services.WorkflowService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddScoped<ILetterCorrespondenceService, LetterCorrespondenceService>();
builder.Services.AddScoped<IAttachmentCorrespondenceService,AttachmentCorrespondenceService>();
builder.Services.AddScoped<IreferralWorkflowService, referralWorkflowService>();
builder.Services.AddScoped<IauthIdentityService, authIdentityService>();
builder.Services.AddScoped<IdepartmentIdentityService,departmentIdentityService>();
builder.Services.AddScoped<IpositionIdentityService, positionIdentityService>();
SD.gatewayApiBase = builder.Configuration["ServiceUrls:gatewayApi"]!;
SD.identityApiBase = builder.Configuration["ServiceUrls:IdentityApi"]!;
builder.Services.AddScoped<IRabbitMQsearchMessageSender, RabbitMQsearchMessageSender>();
// Add services to the container.
IdentityModelEventSource.ShowPII = true;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("appSetting:Token").Value!)),
        ValidateIssuer = false,
        ValidateAudience = false

    };
});
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    opt.AddPolicy("RequireCutomerRole", policy => policy.RequireRole("Customer"));
});
builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
}).AddNewtonsoftJson(opt =>
{
    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
