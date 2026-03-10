using MediatR;
using searchengine.Api.RabbitMQ;
using searchengine.Application;
using searchengine.Application.Feature.leatterFeature.request.Command;
using searchengine.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureApplicationService();
builder.Services.configurePersistenceServices();
builder.Services.AddHostedService<RabbitMQsearchConsumer>();
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
