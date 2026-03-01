using webapi;
using webapi.Services.CorrespondenceService;
using webapi.Services.IServices.ICorrespondenceService;
using webapi.Services.IServices.IWorkflowService;
using webapi.Services.WorkflowService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ILetterCorrespondenceService, LetterCorrespondenceService>();
builder.Services.AddScoped<IAttachmentCorrespondenceService,AttachmentCorrespondenceService>();
builder.Services.AddScoped<IreferralWorkflowService, referralWorkflowService>();
SD.gatewayApiBase = builder.Configuration["ServiceUrls:gatewayApi"]!;
// Add services to the container.

builder.Services.AddControllers();
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
